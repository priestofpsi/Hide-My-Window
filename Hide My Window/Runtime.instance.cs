using System;

namespace theDiary.Tools.HideMyWindow
{
    public partial class Runtime
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Runtime"/> class.
        /// </summary>
        private Runtime()
            : base()
        {
            this.MainFormStateChanged += (s, e) =>
            {
                this.mainForm = (MainForm) s;
            };
        }
        #endregion

        #region Private Declarations
        private const int globalHotKeyApplicationId = 2134;
        private readonly object syncObject = new object();
        private SettingsStore settingsStore;
        private HiddenWindowStore store;
        private volatile WindowInfoManager windowManager = new WindowInfoManager();
        private MainForm mainForm;
        #endregion

        public event EventHandler<FormInitializeEventArgs> MainFormStateChanged;

        
        public void RaiseFormStateChanged(object sender, FormInitializeEventArgs e)
        {
            this.MainFormStateChanged?.Invoke(sender, e);
        }

        #region Public Event Declarations
        /// <summary>
        /// The event that is raised when the application has opened.
        /// </summary>
        public event WindowEventHandler ApplicationOpened;
        internal MainForm GetMainForm()
        {
            return this.mainForm;
        }
        #endregion

        #region Properties

        public SettingsStore Settings
        {
            get
            {
                lock (this.syncObject)
                {
                    if (this.settingsStore == null)
                        this.settingsStore = SettingsStore.Load();

                    return this.settingsStore;
                }
            }
            internal set
            {
                lock(this.syncObject)
                    this.settingsStore = value;
            }
        }

        public HiddenWindowStore Store
        {
            get
            {
                lock (this.syncObject)
                {
                    if (this.store == null)
                        this.store = HiddenWindowStore.Load();

                    return this.store;
                }
            }
            internal set
            {
                lock (this.syncObject)
                    this.store = value;
            }
        }

        public WindowInfoManager WindowManager
        {
            get
            {
                lock (this.syncObject)
                    return this.windowManager;
            }
        }

        public short GlobalHotKeyApplicationId
        {
            get
            {
                return Runtime.globalHotKeyApplicationId;
            }
        }
        #endregion

        #region Internal Methods & Functions
        internal void WriteDebug(string methodName, string valueIdentifier, object value, string category = null)
        {
            if (!System.Diagnostics.Debugger.IsAttached)
                return;

            if (value == null)
                value = "<NULL>";

            System.Diagnostics.Debug.WriteLine($"[{methodName}]\t{valueIdentifier}:{value}", category);
        }
        #endregion
    }
}