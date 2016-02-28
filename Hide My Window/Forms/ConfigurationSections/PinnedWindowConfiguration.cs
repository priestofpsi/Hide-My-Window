namespace theDiary.Tools.HideMyWindow.Forms.ConfigurationSections
{
    using System;
    using System.Windows.Forms;

    public partial class PinnedWindowConfiguration : UserControl, IConfigurationSection
    {
        #region Constructors

        public PinnedWindowConfiguration()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Properties

        public bool ConfigurationChanged
        {
            get { return false; }
        }

        public string SectionName
        {
            get { return "Pinned Windows"; }
        }

        #endregion

        #region Interface Implementations

        public void Activated(object sender, EventArgs e)
        {
        }

        public void LoadConfiguration(object sender, EventArgs e)
        {
            this.modifyWindowText.CheckStateChanged += (s, e1) =>
            {
                this.windowTitlePrefix.Enabled = this.modifyWindowText.Checked;
                this.windowTitleSuffix.Enabled = this.modifyWindowText.Checked;
            };
            this.pinnedHideWhenMinimized.DataBindings.Add(new Binding("Checked",
                Runtime.Instance.Settings.PinnedSettings, "HideOnMinimize"));
            this.modifyWindowText.DataBindings.Add(new Binding("Checked", Runtime.Instance.Settings.PinnedSettings,
                "ModifyWindowTitle"));
            this.windowTitlePrefix.DataBindings.Add(new Binding("Text", Runtime.Instance.Settings.PinnedSettings,
                "PrefixWindowText"));
            this.windowTitleSuffix.DataBindings.Add(new Binding("Text", Runtime.Instance.Settings.PinnedSettings,
                "SufixWindowText"));
        }

        public void ResetConfiguration(object sender, EventArgs e)
        {
            
        }

        public void SaveConfiguration(object sender, EventArgs e)
        {
            Runtime.Instance.Settings.PinnedSettings.HideOnMinimize = this.pinnedHideWhenMinimized.Checked;
        }

        #endregion
    }
}