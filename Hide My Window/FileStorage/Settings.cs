using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

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

        #endregion Private Declarations

        #region Internal Constant & Static Declarations

        [XmlIgnore]
        internal const string StorageFileName = "Settings.xml";

        private static bool FailedToLoad = false;

        #endregion Internal Constant & Static Declarations

        public static event FileEventHandler Notification;

        #region Public Properties

        [XmlElement]
        public HotKeyBindingList Hotkey;

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

                        Settings returnValue = (Settings)xs.Deserialize(fileStream);
                        Program.IsConfigured = (returnValue.Hotkey.Count != 0);
                        return returnValue;
                    }
                }
                catch
                {
                    if (Settings.FailedToLoad)
                        return new Settings();

                    Settings.FailedToLoad = true;
                    if (System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly().FileExists(StorageFileName))
                        System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly().DeleteFile(StorageFileName);
                    return Settings.Load();
                }
                finally
                {
                    if (stream != null)
                        stream.Dispose();
                    if (Settings.Notification != null)
                        Settings.Notification(null, new FileEventArgs(Settings.StorageFileName, FileEventTypes.Loaded));
                }
            });
            System.Threading.Tasks.Task.WaitAll(task);
            if (Settings.Notification != null)
                Settings.Notification(null, new FileEventArgs(Settings.StorageFileName));
            return task.Result;
        }

        #endregion Internal Static Methods & Functions
    }
}