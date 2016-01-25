namespace theDiary.Tools.HideMyWindow
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using Forms.ConfigurationSections;

    public partial class ConfigurationForm : Form
    {
        #region Constructors

        public ConfigurationForm()
        {
            this.InitializeComponent();
            this.hotKeyMimicTextBox1.HotKey =
                Runtime.Instance.Settings.GetHotKeyByFunction(HotKeyFunction.HideCurrentWindow);
            this.hotKeyMimicTextBox2.HotKey =
                Runtime.Instance.Settings.GetHotKeyByFunction(HotKeyFunction.UnhideLastWindow);
            this.hotKeyMimicTextBox3.HotKey =
                Runtime.Instance.Settings.GetHotKeyByFunction(HotKeyFunction.ToggleLastWindow);
            this.hotKeyMimicTextBox4.HotKey =
                Runtime.Instance.Settings.GetHotKeyByFunction(HotKeyFunction.UnhideAllWindows);
            this.hotKeyMimicTextBox1.HotKeyChanged += this.OnHotKeyChanged;
            this.hotKeyMimicTextBox2.HotKeyChanged += this.OnHotKeyChanged;
            this.hotKeyMimicTextBox3.HotKeyChanged += this.OnHotKeyChanged;
            this.hotKeyMimicTextBox4.HotKeyChanged += this.OnHotKeyChanged;


            this.AddConfigurationSections();
            this.FormClosing += (s, e) =>
            {
                //if (this.password.ClearPassword)
                //    Runtime.Instance.Settings.HashedPassword = string.Empty;
                //else if (this.password.Password != string.Empty)
                //    Runtime.Instance.Settings.HashedPassword = this.password.Password;
            };
            this.InitializeToolTips(null);
        }

        #endregion

        #region Declarations

        #region Private Declarations

        private bool FlagHotKeysAsChanged;

        #endregion

        #endregion

        #region Properties

        public TabPage ActivePage
        {
            get { return this.tabControl.SelectedTab; }
        }

        #endregion

        private void OnHotKeyChanged(object sender, EventArgs e)
        {
            HotKey hotKey = (sender as HotKeyMimicTextBox).HotKey;
            Runtime.Instance.Settings.HotKeys[hotKey.Function] = hotKey;
            if (this.FlagHotKeysAsChanged)
                return;
            this.FlagHotKeysAsChanged = true;
            this.FormClosing += (s, e1) =>
            {
                ExternalReferences.UnregisterAll();
                ExternalReferences.RegisterAll();
            };
        }

        #region Methods & Functions

        private void InitializeToolTips(Control.ControlCollection container)
        {
            if (container == null)
                container = this.Controls;

            foreach (Control control in container)
            {
                control.MouseEnter += this.ShowTooltip;
                control.MouseLeave += this.HideTooltip;

                this.InitializeToolTips(control.Controls);
            }
        }

        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {
        }

        private void password_TextChanged(object sender, EventArgs e)
        {
            //this.clearPassword.Visible = !string.IsNullOrWhiteSpace(this.password.Password);
        }

        private void HideTooltip(object sender, EventArgs e)
        {
            this.tooltipLabel.Text = string.Empty;
        }

        private void ShowTooltip(object sender, EventArgs e)
        {
            this.tooltipLabel.Text = ((Control) sender).AccessibleDescription;
        }


        private IEnumerable<IConfigurationSection> GetConfigurationSections()
        {
            return
                this.GetType()
                    .Assembly.GetTypes()
                    .Where(
                        type =>
                            type.GetInterfaces().Any(interfaceType => interfaceType == typeof (IConfigurationSection)))
                    .Select<Type, IConfigurationSection>(
                        type => (IConfigurationSection) System.Activator.CreateInstance(type));
        }

        private void AddConfigurationSection(IConfigurationSection section)
        {
            if (this.tabControl.TabPages.ContainsKey(section.SectionName))
                return;

            Control control = (Control) section;
            TabPage configurationPage = new TabPage(section.SectionName);
            configurationPage.Controls.Add(control);
            this.tabControl.Selected += (s, e) =>
            {
                if (this.tabControl.SelectedTab.Text == section.SectionName)
                    section.Activated(s, e);
            };
            this.tabControl.TabPages.Add(configurationPage);
            control.Dock = DockStyle.Fill;
        }

        private void TabControl_Selected(object sender, TabControlEventArgs e)
        {
            
        }

        private void AddConfigurationSections()
        {
            IEnumerable<IConfigurationSection> sections = this.GetConfigurationSections().ToArray();
            foreach(var section in sections)
                this.AddConfigurationSection(section);
        }

    #endregion
    }
}