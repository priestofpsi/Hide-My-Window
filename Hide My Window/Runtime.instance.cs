using System;

namespace theDiary.Tools.HideMyWindow
{
    public partial class Runtime
    {
        #region Private Declarations
        private const int globalHotKeyApplicationId = 2134;
        private readonly object syncObject = new object();
        private SettingsStore settingsStore;
        private HiddenWindowStore store;
        private volatile WindowInfoManager windowManager = new WindowInfoManager();
        #endregion

        #region Public Event Declarations
        /// <summary>
        /// The event that is raised when the application has opened.
        /// </summary>
        public event WindowEventHandler ApplicationOpened;

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
    }
}