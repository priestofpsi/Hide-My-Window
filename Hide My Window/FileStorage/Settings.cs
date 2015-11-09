using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace theDiary.Tools.HideMyWindow
{
    public partial class Settings
        : IIsolatedStorageFile
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Settings" /> class.
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

        #region Declarations

        private bool autoStartWithWindows;
        public HotKeyBindingList hotkeys;
        private FormState lastState;

        private PinnedApplicationSettings pinnedSettings;

        #endregion

        #region Properties

        [XmlElement]
        public HotKeyBindingList Hotkey
        {
            get { return this.hotkeys; }
            set { this.hotkeys = value; }
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
                {
                    this.Password = string.Empty;
                }
                else
                {
                    this.Password = value.GetMD5Hash();
                }
            }
        }

        [XmlElement]
        public PinnedApplicationSettings PinnedSettings
        {
            get
            {
                if (this.pinnedSettings == null)
                    this.pinnedSettings = new PinnedApplicationSettings();
                return this.pinnedSettings;
            }
            set { this.pinnedSettings = value; }
        }

        #endregion

        #region Methods & Functions

        public static event FileEventHandler FileNotification;

        public Hotkey GetHotKeyByFunction(HotkeyFunction function)
        {
            foreach (Hotkey key in this.Hotkey)
                if (key.Function == function)
                    return key;

            return new Hotkey {Function = function};
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
            if (FileNotification != null)
                FileNotification(settings, new FileEventArgs(Settings.StorageFileName, FileEventTypes.Saving));
            settings.SaveFile();
            if (FileNotification != null)
                FileNotification(null, new FileEventArgs(Settings.StorageFileName));
        }

        internal static Settings Load()
        {
            if (FileNotification != null)
                FileNotification(null, new FileEventArgs(Settings.StorageFileName, FileEventTypes.Opening));
            Settings returnValue = null;
            bool wasCreated;
            returnValue = returnValue.LoadFile(out wasCreated);
            if (returnValue.hotkeys.Count == 0)
            {
                returnValue.Hotkey.Add(HideMyWindow.Hotkey.DefaultHideCurrentWindow);
                returnValue.Hotkey.Add(HideMyWindow.Hotkey.DefaultUnhideLastWindow);
            }
                
            Program.IsConfigured = (wasCreated || returnValue.Hotkey.Count != 0);
            if (FileNotification != null)
                FileNotification(null, new FileEventArgs(Settings.StorageFileName, FileEventTypes.Loaded));
            return returnValue;
        }

        #endregion

        #region Interface Implementations

        /// <summary>
        ///     The event that is raised
        /// </summary>
        public event NotificationEventHandler Notification;

        public void Save()
        {
            if (this.Notification != null)
                this.Notification(this, new NotificationEventArgs("Saving"));

            this.SaveFile();

            if (this.Notification != null)
                this.Notification(this, new NotificationEventArgs("Saved"));
        }

        public void Reset()
        {
            if (this.Notification != null)
                this.Notification(this, new NotificationEventArgs("Resetting"));

            Runtime.Instance.Settings = this.LoadFile();

            if (this.Notification != null)
                this.Notification(this, new NotificationEventArgs("Reset"));
        }

        string IIsolatedStorageFile.GetStorageFileName()
        {
            return Settings.StorageFileName;
        }

        #endregion

        #region Child Classes

        public class PinnedApplicationSettings
        {
            #region Constructors

            public PinnedApplicationSettings()
            {
                this.HideOnMinimize = true;
            }

            #endregion

            #region Properties

            [XmlAttribute]
            public bool HideOnMinimize { get; set; }

            [XmlAttribute]
            public bool AddIconOverlay { get; set; }

            [XmlAttribute]
            public bool ModifyWindowTitle { get; set; }

            [XmlAttribute]
            public string PrefixWindowText { get; set; }

            [XmlAttribute]
            public string SufixWindowText { get; set; }

            #endregion
        }

        #endregion
    }
}