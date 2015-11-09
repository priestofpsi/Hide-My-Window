using System;
using System.Collections.Generic;
using System.Linq;

namespace theDiary.Tools.HideMyWindow
{
    public partial class Runtime
    {
        #region Declarations
        internal Random randomizer = new Random();
        private Settings settings;
        private HiddenWindowStore store;

        private volatile WindowInfoManager windowManager = new WindowInfoManager();
        #endregion

        #region Properties
        public Settings Settings
        {
            get
            {
                if (this.settings == null)
                    this.settings = Settings.Load();

                return this.settings;
            }
            internal set
            {
                this.settings = value;
            }
        }

        public HiddenWindowStore Store
        {
            get
            {
                if (this.store == null)
                    this.store = HiddenWindowStore.Load();

                return this.store;
            }
            internal set
            {
                this.store = value;
            }
        }

        public WindowInfoManager WindowManager
        {
            get
            {
                return this.windowManager;
            }
        }
        #endregion

        #region Methods & Functions
        public event WindowEventHandler ApplicationOpened;
        #endregion
    }
}