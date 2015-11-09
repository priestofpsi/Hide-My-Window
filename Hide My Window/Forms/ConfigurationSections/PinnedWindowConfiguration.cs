using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow.Forms.ConfigurationSections
{
    public partial class PinnedWindowConfiguration : UserControl, IConfigurationSection
    {
        #region Public Constructors
        public PinnedWindowConfiguration()
        {
            this.InitializeComponent();
        }
        #endregion

        #region Properties
        public bool ConfigurationChanged
        {
            get
            {
                return false;
            }
        }

        public string SectionName
        {
            get
            {
                return "Pinned Windows";
            }
        }
        #endregion

        #region Interface Implementations
        public void Activated(object sender, EventArgs e) {}

        public void LoadConfiguration(object sender, EventArgs e)
        {
            this.pinnedHideWhenMinimized.DataBindings.Add(new Binding("Checked",
                Runtime.Instance.Settings.PinnedSettings, "HideOnMinimize"));
        }

        public void ResetConfiguration(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void SaveConfiguration(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}