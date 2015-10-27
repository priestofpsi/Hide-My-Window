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

        public Settings()
        {
            this.Hotkey = new HotKeyBindingList();
        }

        #endregion

        #region Constant Declarations

        [XmlIgnore] internal const string StorageFileName = "Settings.xml";

        private const string RegistryStartPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

        private static bool failedToLoad;

        #endregion

        #region Declarations

        private bool autoStartWithWindows;

        [XmlElement] public HotKeyBindingList Hotkey;

        private FormState lastState;

        #endregion

        #region Properties

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

        [XmlElement]
        public string Password { get; set; }

        [XmlIgnore]
        public bool PasswordIsSet
        {
            get { return !string.IsNullOrWhiteSpace(this.Password); }
        }

        [XmlElement]
        public bool RequirePasswordOnShow { get; set; }

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

        [XmlAttribute]
        public bool SmallToolbarIcons { get; set; }

        [XmlAttribute]
        public bool HideToolbarText { get; set; }

        #endregion

        #region Methods & Functions

        public static event FileEventHandler Notification;

        public Hotkey GetHotKeyByFunction(HotkeyFunction function)
        {
            foreach (Hotkey key in this.Hotkey)
                if (key.Function == function)
                    return key;

            return new Hotkey {Function = function};
        }

        public void Save()
        {
            Settings.Save(Runtime.Instance.Settings);
        }

        public void Reset()
        {
            Runtime.Instance.Settings = Settings.Load();
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
                if (Notification != null)
                    Notification(settings, new FileEventArgs(Settings.StorageFileName, FileEventTypes.Saving));
                using (
                    Stream stream = IsolatedStorageFile.GetUserStoreForAssembly()
                        .OpenFile(Settings.StorageFileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    XmlSerializer xs = new XmlSerializer(typeof (Settings));
                    using (StreamWriter tw = new StreamWriter(stream))
                        xs.Serialize(tw, settings);
                }
                if (Notification != null)
                    Notification(settings, new FileEventArgs(Settings.StorageFileName, FileEventTypes.Saving));
            });
            Task.WaitAll(task);
            if (Notification != null)
                Notification(null, new FileEventArgs(Settings.StorageFileName));
        }

        internal static Settings Load()
        {
            Task<Settings> task = Task.Run(() =>
            {
                Settings returnValue = null;
                Stream stream = null;
                try
                {
                    if (Notification != null)
                        Notification(null, new FileEventArgs(Settings.StorageFileName, FileEventTypes.Opening));
                    if (IsolatedStorageFile.GetUserStoreForAssembly().FileExists(Settings.StorageFileName))
                        stream = IsolatedStorageFile.GetUserStoreForAssembly()
                            .OpenFile(Settings.StorageFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

                    Program.IsConfigured = (stream != null);
                    if (stream == null)
                    {
                        if (Notification != null)
                            Notification(null, new FileEventArgs(Settings.StorageFileName, FileEventTypes.Creating));
                        stream = new FileStream(Settings.StorageFileName, FileMode.OpenOrCreate);
                        if (Notification != null)
                            Notification(null, new FileEventArgs(Settings.StorageFileName, FileEventTypes.Created));
                    }
                    XmlSerializer xs = new XmlSerializer(typeof (Settings));
                    using (StreamReader fileStream = new StreamReader(stream))
                    {
                        if (Notification != null)
                            Notification(null, new FileEventArgs(Settings.StorageFileName, FileEventTypes.Loading));

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

                    if (Notification != null)
                        Notification(null, new FileEventArgs(Settings.StorageFileName, FileEventTypes.Loaded));
                }

                return returnValue;
            });
            Task.WaitAll(task);
            if (Notification != null)
                Notification(null, new FileEventArgs(Settings.StorageFileName));
            return task.Result;
        }

        #endregion
    }
}