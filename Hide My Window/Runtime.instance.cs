using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;

namespace theDiary.Tools.HideMyWindow
{
    public partial class Runtime
    {
        #region Constructors

        public Runtime()
        {
           
        }

        public event WindowEventHandler ApplicationOpened;
        #endregion

        #region Declarations

        private volatile WindowInfoManager windowManager = new WindowInfoManager();
        private Settings settings;
        private HiddenWindowStore store;

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
        internal Random randomizer = new Random();
    }
}