﻿namespace theDiary.Tools.HideMyWindow
{
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;
    using System.Xml.Serialization;
    using Microsoft.Win32;

    public partial class SettingsStore : IsolatedStorageFileBase
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SettingsStore" /> class.
        /// </summary>
        public SettingsStore()
            : base(StorageFileName)
        {
            this.HotKeys = new HotKeyBindingList();
        }

        #endregion

        #region Declarations

        #region Static Declarations

        private const string StorageFileName = "SettingsStore.xml";

        private const string RegistryStartPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

        #endregion

        #region Private Declarations
        private bool autoStartWithWindows;
        private FormState lastState;
        private PinnedApplications pinnedSettings;
        private bool enableNotifications;
        #endregion

        #endregion

        #region Properties

        [XmlElement]
        public HotKeyBindingList HotKeys { get; set; }

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
        public string Password { get; set; }

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
                    this.Password = string.Empty;
                else
                    this.Password = value.GetMd5Hash();
            }
        }

        [XmlElement]
        public PinnedApplications PinnedSettings
        {
            get
            {
                if (this.pinnedSettings == null)
                    this.pinnedSettings = new PinnedApplications();
                return this.pinnedSettings;
            }
            set { this.pinnedSettings = value; }
        }

        [XmlAttribute]
        public bool EnableNotifications
        {
            get
            {
                return this.enableNotifications;
            }
            set
            {
                if (this.enableNotifications == value)
                    return;

                this.enableNotifications = value;
                //Program.MainForm.EnableNotifications(value);
                Runtime.Instance.MainFormStateChanged += (s, e) => { ((MainForm)s).EnableNotifications(value); };
                //if (Program.MainForm == null)
                //{
                //    Runtime.Instance.MainFormStateChanged += (s, e) => { ((MainForm)s).EnableNotifications(value); };
                //}
                //else{
                //    Runtime.Instance.GetMainForm().EnableNotifications(value);
                //}
                
            }
        }
        #endregion

        #region Methods & Functions

        public HotKey GetHotKeyByFunction(HotKeyFunction function)
        {
            return this.HotKeys.FirstOrDefault(hotKey => hotKey.Function == function) ?? new HotKey(function);
        }

        private bool HasRegistryEntry(string path, string name)
        {
            RegistryKey key;
            return TryGetRegistryEntry(path, name, out key);
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
            bool hasKey = SettingsStore.TryGetRegistryEntry(SettingsStore.RegistryStartPath, attribute.Title, out key);
            bool checkAutoStartWithWindows = this.AutoStartWithWindows;
            if ((!checkAutoStartWithWindows && hasKey)
                || (checkAutoStartWithWindows && !hasKey))
            {
                if (checkAutoStartWithWindows)
                    key.SetValue(attribute.Title, string.Format("\"{0}\"", Assembly.GetEntryAssembly().Location));
                else
                    key.DeleteValue(attribute.Title);
            }
        }

        internal static void Save(SettingsStore settingsStore)
        {
            SettingsStore.RaiseFileNotification(settingsStore, new FileEventArgs(SettingsStore.StorageFileName, FileEventTypes.Saving));

            settingsStore.SaveFile();

            SettingsStore.RaiseFileNotification(settingsStore, new FileEventArgs(SettingsStore.StorageFileName, FileEventTypes.Saved));
        }

        internal static SettingsStore Load()
        {
            SettingsStore.RaiseFileNotification(null, new FileEventArgs(StorageFileName, FileEventTypes.Opening));
            bool wasCreated;
            SettingsStore returnValue = LoadFile<SettingsStore>(out wasCreated);
            if (returnValue.HotKeys.Count == 0)
            {
                returnValue.HotKeys.Add(HotKey.DefaultHideCurrentWindow);
                returnValue.HotKeys.Add(HotKey.DefaultUnhideLastWindow);
                returnValue.HotKeys.Add(HotKey.DefaultToggleLastWindow);
            }

            Program.IsConfigured = (wasCreated || returnValue.HotKeys.Count != 0);
            SettingsStore.RaiseFileNotification(null, new FileEventArgs(StorageFileName, FileEventTypes.Loaded));

            return returnValue;
        }

        #endregion

        #region Interface Implementations

        /// <summary>
        /// Saves the <see cref="SettingsStore"/>, storing any changed values.
        /// </summary>
        public override void Save()
        {
            this.RaiseNotification(this, new NotificationEventArgs("Saving"));

            this.SaveFile();

            this.RaiseNotification(this, new NotificationEventArgs("Saved"));
        }

        /// <summary>
        /// Resets the <see cref="SettingsStore"/> to its original values, discarding any changes made.
        /// </summary>
        public override void Reset()
        {
            this.RaiseNotification(this, new NotificationEventArgs("Resetting"));

            Runtime.Instance.Settings = this.LoadFile();

            this.RaiseNotification(this, new NotificationEventArgs("Reset"));
        }

        #endregion
    }
}