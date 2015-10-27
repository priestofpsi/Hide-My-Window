using System;
using System.IO;
using System.Reflection;
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

        #endregion Constructors

        #region Private Declarations

        private FormState lastState;
        private bool autoStartWithWindows = false;
        private bool registryChecked = false;

        #endregion Private Declarations

        #region Internal Constant & Static Declarations

        [XmlIgnore]
        internal const string StorageFileName = "Settings.xml";

        private const string RegistryStartPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

        private static bool FailedToLoad = false;

        #endregion Internal Constant & Static Declarations

        public static event FileEventHandler Notification;

        #region Public Properties
        public bool AutoStartWithWindows
        {
            get
            {
                return this.autoStartWithWindows;
            }
            set
            {
                if (this.autoStartWithWindows == value)
                    return;
                this.autoStartWithWindows = value;
                this.CheckWindowsRegistryAutoStart();
            }
        }

        [XmlElement]
        public HotKeyBindingList Hotkey;

        public Hotkey GetHotKeyByFunction(HotkeyFunction function)
        {
            foreach (var key in this.Hotkey)
                if (key.Function == function)
                    return key;

            return new HideMyWindow.Hotkey() { Function = function };
        }
        [XmlAttribute]
        public View CurrentView
        {
            get; set;
        }

        [XmlAttribute]
        public bool HideStatusbar
        {
            get; set;
        }

        [XmlAttribute]
        public bool StartInTaskBar
        {
            get; set;
        }

        [XmlAttribute]
        public bool MinimizeToTaskBar
        {
            get; set;
        }

        [XmlAttribute]
        public bool CloseToTaskBar
        {
            get; set;
        }

        [XmlAttribute]
        public bool RestoreWindowsOnExit
        {
            get; set;
        }

        [XmlAttribute]
        public bool ConfirmApplicationExit
        {
            get; set;
        }

        [XmlElement]
        public FormState LastState
        {
            get
            {
                if (this.lastState == null)
                    this.lastState = new FormState();

                return this.lastState;
            }
            set
            {
                this.lastState = value;
            }
        }

        [XmlElement]
        public string Password
        {
            get; set;
        }

        [XmlIgnore]
        public bool PasswordIsSet
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.Password);
            }
        }
        [XmlElement]
        public bool RequirePasswordOnShow
        {
            get; set;
        }

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
        public bool SmallToolbarIcons
        {
            get; set;
        }

        [XmlAttribute]
        public bool HideToolbarText
        {
            get; set;
        }

        #endregion Public Properties

        #region Public Methods & Functions

        public void Save()
        {
            Settings.Save(Runtime.Instance.Settings);
        }

        public void Reset()
        {
            Runtime.Instance.Settings = Settings.Load();
        }

        #endregion Public Methods & Functions

        #region Private Methods
        private bool HasRegistryEntry(string path, string name)
        {
            RegistryKey key;
            return this.TryGetRegistryEntry(path, name, out key);
        }

        private bool TryGetRegistryEntry(string path, string name, out RegistryKey key)
        {
            key = Registry.CurrentUser.OpenSubKey(path, true);
            return (key != null && key.GetValue(name) != null);
        }

        private void CheckWindowsRegistryAutoStart()
        {
            RegistryKey key;
            AssemblyTitleAttribute attribute;
            Assembly.GetEntryAssembly().TryGetCustomAttribute<AssemblyTitleAttribute>(out attribute);
            bool hasKey = this.TryGetRegistryEntry(Settings.RegistryStartPath, attribute.Title, out key);
            bool autoStartWithWindows = this.AutoStartWithWindows;
            if ((!autoStartWithWindows && hasKey)
                || (autoStartWithWindows && !hasKey))
            {
                if (autoStartWithWindows && !hasKey)
                {
                    key.SetValue(attribute.Title, string.Format("\"{0}\"", Assembly.GetEntryAssembly().Location));
                }
                else
                {
                    key.DeleteValue(attribute.Title);
                }
            }
            this.registryChecked = true;
            return;
        }
        #endregion Private Methods
        #region Internal Static Methods & Functions

        internal static void Save(Settings settings)
        {
            var task = System.Threading.Tasks.Task.Run(() =>
            {
                if (settings == null)
                    settings = Settings.Load();
                if (Settings.Notification != null)
                    Settings.Notification(settings, new FileEventArgs(Settings.StorageFileName, FileEventTypes.Saving));
                using (System.IO.Stream stream = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly().OpenFile(Settings.StorageFileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    var xs = new XmlSerializer(typeof(Settings));
                    using (var tw = new StreamWriter(stream))
                        xs.Serialize(tw, settings);
                }
                if (Settings.Notification != null)
                    Settings.Notification(settings, new FileEventArgs(Settings.StorageFileName, FileEventTypes.Saving));
            });
            System.Threading.Tasks.Task.WaitAll(task);
            if (Settings.Notification != null)
                Settings.Notification(null, new FileEventArgs(Settings.StorageFileName));
        }

        internal static Settings Load()
        {
            var task = System.Threading.Tasks.Task.Run<Settings>(() =>
            {
                Settings returnValue = null;
                System.IO.Stream stream = null;
                try
                {
                    if (Settings.Notification != null)
                        Settings.Notification(null, new FileEventArgs(Settings.StorageFileName, FileEventTypes.Opening));
                    if (System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly().FileExists(StorageFileName))
                        stream = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly().OpenFile(StorageFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

                    Program.IsConfigured = (stream != null);
                    if (stream == null)
                    {
                        if (Settings.Notification != null)
                            Settings.Notification(null, new FileEventArgs(Settings.StorageFileName, FileEventTypes.Creating));
                        stream = new FileStream(StorageFileName, FileMode.OpenOrCreate);
                        if (Settings.Notification != null)
                            Settings.Notification(null, new FileEventArgs(Settings.StorageFileName, FileEventTypes.Created));
                    }
                    var xs = new XmlSerializer(typeof(Settings));
                    using (var fileStream = new StreamReader(stream))
                    {
                        if (Settings.Notification != null)
                            Settings.Notification(null, new FileEventArgs(Settings.StorageFileName, FileEventTypes.Loading));

                        returnValue = (Settings) xs.Deserialize(fileStream);
                        Program.IsConfigured = (returnValue.Hotkey.Count != 0);
                    }
                }
                catch
                {
                    if (Settings.FailedToLoad)
                    {
                        returnValue = new Settings();
                    }
                    else
                    {
                        Settings.FailedToLoad = true;
                        if (System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly().FileExists(StorageFileName))
                            System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly().DeleteFile(StorageFileName);
                        returnValue = Settings.Load();
                    }
                }
                finally
                {
                    if (stream != null)
                        stream.Dispose();
                    if (returnValue != null)
                        returnValue.CheckWindowsRegistryAutoStart();

                    if (Settings.Notification != null)
                        Settings.Notification(null, new FileEventArgs(Settings.StorageFileName, FileEventTypes.Loaded));
                }

                return returnValue;
            });
            System.Threading.Tasks.Task.WaitAll(task);
            if (Settings.Notification != null)
                Settings.Notification(null, new FileEventArgs(Settings.StorageFileName));
            return task.Result;
        }

        #endregion Internal Static Methods & Functions
    }
}
