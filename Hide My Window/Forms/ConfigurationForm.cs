using System;
using System.Drawing;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    public partial class ConfigurationForm : Form
    {
        public ConfigurationForm()
        {
            InitializeComponent();
            this.minimizeToTaskbar.DataBindings.Add(new Binding("Checked", Runtime.Instance.Settings, "MinimizeToTaskBar"));
            this.closeToTaskbar.DataBindings.Add(new Binding("Checked", Runtime.Instance.Settings, "CloseToTaskBar"));
            this.restoreWindowsOnExit.DataBindings.Add(new Binding("Checked", Runtime.Instance.Settings, "RestoreWindowsOnExit"));
            this.confirmWhenExiting.DataBindings.Add(new Binding("Checked", Runtime.Instance.Settings, "ConfirmApplicationExit"));
            this.startInTaskbar.DataBindings.Add(new Binding("Checked", Runtime.Instance.Settings, "StartInTaskBar"));
            this.requirePasswordOnShow.DataBindings.Add(new Binding("Checked", Runtime.Instance.Settings, "RequirePasswordOnShow"));
            this.requirePasswordOnShow.DataBindings.Add(new Binding("Enabled", Runtime.Instance.Settings, "PasswordIsSet"));
            this.Function.DataSource = Enum.GetNames(typeof(HotkeyFunction));
            this.Function.DataPropertyName = "HKFunction";
            this.clearPassword.CheckedChanged += (s, e) => this.glowPanel1.Enabled = !this.clearPassword.Checked;

            this.HotKey.DataSource = Enum.GetNames(typeof(Keys));
            this.HotKey.DataPropertyName = "Key";

            this.Control.DataPropertyName = "Control";
            this.Alt.DataPropertyName = "Alt";
            this.Shift.DataPropertyName = "Shift";
            this.Win.DataPropertyName = "Win";
            this.glowPanel1.EffectColor = Runtime.Instance.Settings.PasswordIsSet ? Color.LimeGreen : Color.Firebrick;
            if (!Runtime.Instance.Settings.PasswordIsSet)
                this.password.TextChanged += (s, e) =>
                {
                    bool isSet = !string.IsNullOrWhiteSpace(this.password.Text);
                    this.glowPanel1.EffectColor = isSet ? Color.LimeGreen : Color.Firebrick;
                    this.password.AccessibleDescription = isSet ? "The password has been configured." : "The password has not been set.";
                    this.glowPanel1.AccessibleDescription = isSet ? "The password has been configured." : "The password has not been set.";
                    this.requirePasswordOnShow.Enabled = isSet;
                };
            this.password.AccessibleDescription = Runtime.Instance.Settings.PasswordIsSet ? "The password has been configured." : "The password has not been set.";
            this.glowPanel1.AccessibleDescription = Runtime.Instance.Settings.PasswordIsSet ? "The password has been configured." : "The password has not been set.";
            this.clearPassword.AccessibleDescription = "Check to have your password cleared when closing.";
            this.dataGridView1.DataSource = Runtime.Instance.Settings.Hotkey;
            this.dataGridView1.CellValueChanged += (s, e) => this.hotkeysChanged = true;
            this.FormClosing += (s, e) =>
            {
                if (this.password.ClearPassword)                
                {
                    Runtime.Instance.Settings.HashedPassword = string.Empty;
                }
                else if (this.password.Password != string.Empty)
                {
                    Runtime.Instance.Settings.HashedPassword = this.password.Password;                    
                }
                if (this.hotkeysChanged)
                {
                    ExternalReferences.UnregisterAll();
                    ExternalReferences.RegisterAll();
                }
            };
            this.InitializeToolTips(null);
        }

        public TabPage ActivePage
        {
            get
            {
                return this.tabControl.SelectedTab;
            }
        }
        private bool hotkeysChanged = false;

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
            this.clearPassword.Visible = !string.IsNullOrWhiteSpace(this.password.Password);
        }

        private void HideTooltip(object sender, EventArgs e)
        {
            this.tooltipLabel.Text = string.Empty;
        }
        private void ShowTooltip(object sender, EventArgs e)
        {
            this.tooltipLabel.Text = ((Control)sender).AccessibleDescription;
        }
    }
}
