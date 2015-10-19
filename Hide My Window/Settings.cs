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
        internal const string settingsxml = "Settings.xml";

        private static bool FailedToLoad = false;

        #endregion Internal Constant & Static Declarations

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
            if (settings == null)
                settings = Settings.Load();

            System.IO.Stream stream = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly().OpenFile(settingsxml, FileMode.OpenOrCreate, FileAccess.Write);

            var xs = new XmlSerializer(typeof(Settings));
            using (var tw = new StreamWriter(stream))
                xs.Serialize(tw, settings);
        }

        internal static Settings Load()
        {
            System.IO.Stream stream = null;

            try
            {
                if (System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly().FileExists(settingsxml))
                    stream = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly().OpenFile(settingsxml, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

                Program.IsConfigured = (stream != null);
                if (stream == null)                
                    stream = new FileStream(settingsxml, FileMode.OpenOrCreate);

                var xs = new XmlSerializer(typeof(Settings));
                using (var fileStream = new StreamReader(stream))
                {
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
                if (System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly().FileExists(settingsxml))
                    System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly().DeleteFile(settingsxml);
                return Settings.Load();
            }
        }

        #endregion Internal Static Methods & Functions
    }
}