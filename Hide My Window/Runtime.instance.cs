namespace theDiary.Tools.HideMyWindow
{
    using System;

    public partial class Runtime
    {
        #region Declarations

        #region Private Declarations

        private SettingsStore settingsStore;
        private HiddenWindowStore store;

        private volatile WindowInfoManager windowManager = new WindowInfoManager();

        #endregion

        #endregion

        internal Random randomizer = new Random();

        #region Methods & Functions

        public event WindowEventHandler ApplicationOpened;

        #endregion

        #region Properties

        public SettingsStore Settings
        {
            get
            {
                if (this.settingsStore == null)
                    this.settingsStore = SettingsStore.Load();

                return this.settingsStore;
            }
            internal set { this.settingsStore = value; }
        }

        public HiddenWindowStore Store
        {
            get
            {
                if (this.store == null)
                    this.store = HiddenWindowStore.Load();

                return this.store;
            }
            internal set { this.store = value; }
        }

        public WindowInfoManager WindowManager
        {
            get { return this.windowManager; }
        }

        #endregion

        public short GlobalHotKeyApplicationId
        {
            get{
                return 2134;
           }
        }
        
    }
}