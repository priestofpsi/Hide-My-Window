using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow
{
    public partial class Runtime
    {
        private Settings settings;
        private HiddenWindowStore store;
        private volatile MainForm mainForm;

        public Settings Settings
        {
            get
            {
                if (this.settings == null)
                    this.settings = Settings.Load();

                return this.settings;
            }
            internal set{
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
        }
        internal MainForm MainForm
        {
            get
            {
                if (this.mainForm == null)
                    this.mainForm = new MainForm();

                return this.mainForm;
            }
        }
    }
}
