namespace theDiary.Tools.HideMyWindow.Forms.ConfigurationSections
{
    using System;
    using System.Windows.Forms;

    public partial class PinnedWindowConfiguration : UserControl, IConfigurationSection<PinnedApplications>
    {
        #region Constructors

        public PinnedWindowConfiguration()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Properties
        public PinnedApplications ConfigurationSection
        {
            get
            {
                return Runtime.Instance.Settings.PinnedSettings;
            }
        }

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
            this.pinnedHideWhenMinimized.Checked = Runtime.Instance.Settings.PinnedSettings.HideOnMinimize;
            this.modifyWindowText.Checked = Runtime.Instance.Settings.PinnedSettings.ModifyWindowTitle;
            this.windowTitlePrefix.Text = Runtime.Instance.Settings.PinnedSettings.PrefixWindowText;
            this.windowTitleSuffix.Text = Runtime.Instance.Settings.PinnedSettings.SuffixWindowText;
        }

        public void ResetConfiguration(object sender, EventArgs e)
        {
            
        }

        public void SaveConfiguration(object sender, EventArgs e)
        {
            this.SetConfigurationFromControl(this.pinnedHideWhenMinimized, "Checked", "HideOnMinimize");
            this.SetConfigurationFromControl(this.modifyWindowText, "Checked", "ModifyWindowTitle");
            this.SetConfigurationFromControl(this.windowTitlePrefix, "Text", "PrefixWindowText");
            this.SetConfigurationFromControl(this.windowTitleSuffix, "Text", "SuffixWindowText");
            this.SetConfigurationFromControl(this.addIconOverlay, "Checked", "AddIconOverlay");
        }

        #endregion
    }
}