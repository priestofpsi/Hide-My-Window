using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace theDiary.Tools.HideMyWindow
{
    public partial class Settings
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        public Settings()
        {
            this.Hotkey = new HotKeyBindingList();
        }

        #endregion

        #region Constant Declarations

        private const string StorageFileName = "Settings.xml";

        private const string RegistryStartPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

        private static bool failedToLoad = false;

        #endregion

        #region Private Declarations

        private bool autoStartWithWindows;
        private FormState lastState;
        public HotKeyBindingList hotkeys;
        #endregion

        public static event FileEventHandler FileNotification;

        /// <summary>
        /// The event that is raised
        /// </summary>
        public event NotificationEventHandler Notification;

        #region Properties
        [XmlElement]
        public HotKeyBindingList Hotkey
        {
            get
            {
                return this.hotkeys;
            }
            set
            {
                this.hotkeys = value;
            }
        }

        [XmlAttribute]
        public bool AutoStartWithWindows
        {
            get { return this.autoStartWithWindows; }
            set
            {
                if (this.autoStartWithWindows == value)
                    return;
                this.autoStartWithWindows = value;
                this.CheckWindowsRegistryAutoStart();
            }
        }

        [XmlAttribute]
        public View CurrentView { get; set; }

        [XmlAttribute]
        public bool HideStatusbar { get; set; }

        [XmlAttribute]
        public bool StartInTaskBar { get; set; }

        [XmlAttribute]
        public bool MinimizeToTaskBar { get; set; }

        [XmlAttribute]
        public bool CloseToTaskBar { get; set; }

        [XmlAttribute]
        public bool RestoreWindowsOnExit { get; set; }

        [XmlAttribute]
        public bool ConfirmApplicationExit { get; set; }

        [XmlElement]
        public FormState LastState
        {
            get
            {
                if (this.lastState == null)
                    this.lastState = new FormState();

                return this.lastState;
            }
            set { this.lastState = value; }
        }


        [XmlIgnore]
        public bool PasswordIsSet
        {
            get { return !string.IsNullOrWhiteSpace(this.Password); }
        }

        [XmlElement]
        public bool RequirePasswordOnShow { get; set; }


        [XmlElement]
        public string Password
        {
            get; set;
        }

        [XmlAttribute]
        public bool SmallToolbarIcons { get; set; }

        [XmlAttribute]
        public bool HideToolbarText { get; set; }

        [XmlIgnore]
        public string HashedPassword
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.Password = string.Empty;
                }
                else
                {
                    this.Password = value.GetMD5Hash();
                }
            }
        }
        #endregion

        #region Methods & Functions
        public Hotkey GetHotKeyByFunction(HotkeyFunction function)
        {
            foreach (Hotkey key in this.Hotkey)
                if (key.Function == function)
                    return key;

            return new Hotkey {Function = function};
        }

        public void Save()
        {
            if (this.Notification != null)
                this.Notification(this, new NotificationEventArgs("Saving"));

            Settings.Save(Runtime.Instance.Settings);

            if (this.Notification != null)
                this.Notification(this, new NotificationEventArgs("Saved"));
        }

        public void Reset()
        {
            if (this.Notification != null)
                this.Notification(this, new NotificationEventArgs("Resetting"));

            Runtime.Instance.Settings = Settings.Load();

            if (this.Notification != null)
                this.Notification(this, new NotificationEventArgs("Reset"));
        }

        private bool HasRegistryEntry(string path, string name)
        {
            RegistryKey key;
            return Settings.TryGetRegistryEntry(path, name, out key);
        }

        private static bool TryGetRegistryEntry(string path, string name, out RegistryKey key)
        {
            key = Registry.CurrentUser.OpenSubKey(path, true);
            return (key != null && key.GetValue(name) != null);
        }

        private void CheckWindowsRegistryAutoStart()
        {
            RegistryKey key;
            AssemblyTitleAttribute attribute;
            Assembly.GetEntryAssembly().TryGetCustomAttribute(out attribute);
            bool hasKey = Settings.TryGetRegistryEntry(Settings.RegistryStartPath, attribute.Title, out key);
            bool autoStartWithWindows = this.AutoStartWithWindows;
            if ((!autoStartWithWindows && hasKey)
                || (autoStartWithWindows && !hasKey))
            {
                if (autoStartWithWindows)
                {
                    key.SetValue(attribute.Title, string.Format("\"{0}\"", Assembly.GetEntryAssembly().Location));
                }
                else
                {
                    key.DeleteValue(attribute.Title);
                }
            }
        }

        internal static void Save(Settings settings)
        {
            Task task = Task.Run(() =>
            {
                if (settings == null)
                    settings = Settings.Load();
                if (FileNotification != null)
                    FileNotification(settings, new FileEventArgs(Settings.StorageFileName, FileEventTypes.Saving));
                using (
                    Stream stream = IsolatedStorageFile.GetUserStoreForAssembly()
                        .OpenFile(Settings.StorageFileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    XmlSerializer xs = new XmlSerializer(typeof (Settings));
                    using (StreamWriter tw = new StreamWriter(stream))
                        xs.Serialize(tw, settings);
                }
                if (FileNotification != null)
                    FileNotification(settings, new FileEventArgs(Settings.StorageFileName, FileEventTypes.Saving));
            });
            Task.WaitAll(task);
            if (FileNotification != null)
                FileNotification(null, new FileEventArgs(Settings.StorageFileName));
        }

        internal static Settings Load()
        {
            Task<Settings> task = Task.Run(() =>
            {
                Settings returnValue = null;
                Stream stream = null;
                try
                {
                    if (Settings.FileNotification != null)
                        Settings.FileNotification(null, new FileEventArgs(Settings.StorageFileName, FileEventTypes.Opening));
                    if (IsolatedStorageFile.GetUserStoreForAssembly().FileExists(Settings.StorageFileName))
                        stream = IsolatedStorageFile.GetUserStoreForAssembly()
                            .OpenFile(Settings.StorageFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

                    Program.IsConfigured = (stream != null);
                    if (stream == null)
                    {
                        if (Settings.FileNotification != null)
                            Settings.FileNotification(null, new FileEventArgs(Settings.StorageFileName, FileEventTypes.Creating));
                        stream = new FileStream(Settings.StorageFileName, FileMode.OpenOrCreate);
                        if (Settings.FileNotification != null)
                            Settings.FileNotification(null, new FileEventArgs(Settings.StorageFileName, FileEventTypes.Created));
                    }
                    XmlSerializer xs = new XmlSerializer(typeof (Settings));
                    using (StreamReader fileStream = new StreamReader(stream))
                    {
                        if (Settings.FileNotification != null)
                            Settings.FileNotification(null, new FileEventArgs(Settings.StorageFileName, FileEventTypes.Loading));

                        returnValue = (Settings) xs.Deserialize(fileStream);
                        Program.IsConfigured = (returnValue.Hotkey.Count != 0);
                    }
                }
                catch
                {
                    if (Settings.failedToLoad)
                    {
                        returnValue = new Settings();
                    }
                    else
                    {
                        Settings.failedToLoad = true;
                        if (IsolatedStorageFile.GetUserStoreForAssembly().FileExists(Settings.StorageFileName))
                            IsolatedStorageFile.GetUserStoreForAssembly().DeleteFile(Settings.StorageFileName);
                        returnValue = Settings.Load();
                    }
                }
                finally
                {
                    if (stream != null)
                        stream.Dispose();
                    if (returnValue != null)
                        returnValue.CheckWindowsRegistryAutoStart();

                    if (Settings.FileNotification != null)
                        Settings.FileNotification(null, new FileEventArgs(Settings.StorageFileName, FileEventTypes.Loaded));
                }

                return returnValue;
            });
            Task.WaitAll(task);
            if (FileNotification != null)
                FileNotification(null, new FileEventArgs(Settings.StorageFileName));
            return task.Result;
        }

        #endregion
    }
}