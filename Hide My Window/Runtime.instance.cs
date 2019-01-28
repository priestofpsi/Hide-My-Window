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

        #region Public Event Definitions
        /// <summary>
        /// The event that is raised when the <c>State</c> changes for the <see cref="MainForm"/>.
        /// </summary>
        public event EventHandler<FormInitializeEventArgs> MainFormStateChanged;
        #endregion

        #region Public Event Declarations
        /// <summary>
        /// The event that is raised when the application has opened.
        /// </summary>
        public event WindowEventHandler ApplicationOpened;
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets a thread safe, self initializing instance of the <see cref="SettingsStore"/>.
        /// </summary>
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

        /// <summary>
        /// Gets a thread safe instance of the <see cref="HiddenWindowStore"/>.
        /// </summary>
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

        /// <summary>
        /// Gets a thread safe instance of the <see cref="WindowInfoManager"/>.
        /// </summary>
        public WindowInfoManager WindowManager
        {
            get
            {
                lock (this.syncObject)
                    return this.windowManager;
            }
        }
        #endregion

        #region Internal Methods & Functions
        internal void RaiseFormStateChanged(object sender, FormInitializeEventArgs e)
        {
            this.MainFormStateChanged?.Invoke(sender, e);
        }

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