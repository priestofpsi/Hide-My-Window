using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow.Forms.ConfigurationSections
{
    public partial class GeneralConfiguration 
        : UserControl, IConfigurationSection
    {
        public GeneralConfiguration()
        {
            InitializeComponent();
        }

        public bool ConfigurationChanged
        {
            get
            {
                return false;
            }
        }

        public string SectionName
        {
            get { return "General"; }
        }

        public void Activated(object sender, EventArgs e)
        {
            
        }

        public void LoadConfiguration(object sender, EventArgs e)
        {
            this.minimizeToTaskbar.DataBindings.Add(new Binding("Checked", Runtime.Instance.Settings, "MinimizeToTaskBar"));
            this.closeToTaskbar.DataBindings.Add(new Binding("Checked", Runtime.Instance.Settings, "CloseToTaskBar"));
            this.restoreWindowsOnExit.DataBindings.Add(new Binding("Checked", Runtime.Instance.Settings,
                "RestoreWindowsOnExit"));
            this.confirmWhenExiting.DataBindings.Add(new Binding("Checked", Runtime.Instance.Settings,
                "ConfirmApplicationExit"));
            this.startInTaskbar.DataBindings.Add(new Binding("Checked", Runtime.Instance.Settings, "StartInTaskBar"));
            this.requirePasswordOnShow.DataBindings.Add(new Binding("Checked", Runtime.Instance.Settings,
                "RequirePasswordOnShow"));
            this.requirePasswordOnShow.DataBindings.Add(new Binding("Enabled", Runtime.Instance.Settings,
                "PasswordIsSet"));
            this.clearPassword.CheckedChanged += (s, e1) => this.glowPanel1.Enabled = !this.clearPassword.Checked;
            this.startWithWindows.DataBindings.Add(new Binding("Checked", Runtime.Instance.Settings,
                "AutoStartWithWindows"));
            this.glowPanel1.EffectColor = Runtime.Instance.Settings.PasswordIsSet ? Color.LimeGreen : Color.Firebrick;
            if (!Runtime.Instance.Settings.PasswordIsSet)
                this.password.TextChanged += (s, e1) =>
                {
                    bool isSet = !string.IsNullOrWhiteSpace(this.password.Text);
                    this.glowPanel1.EffectColor = isSet ? Color.LimeGreen : Color.Firebrick;
                    this.password.AccessibleDescription = isSet
                        ? "The password has been configured."
                        : "The password has not been set.";
                    this.glowPanel1.AccessibleDescription = isSet
                        ? "The password has been configured."
                        : "The password has not been set.";
                    this.requirePasswordOnShow.Enabled = isSet;
                };
            this.password.AccessibleDescription = Runtime.Instance.Settings.PasswordIsSet
                ? "The password has been configured."
                : "The password has not been set.";
            this.glowPanel1.AccessibleDescription = Runtime.Instance.Settings.PasswordIsSet
                ? "The password has been configured."
                : "The password has not been set.";
            this.clearPassword.AccessibleDescription = "Check to have your password cleared when closing.";
        }

        public void ResetConfiguration(object sender, EventArgs e)
        {
            
        }

        public void SaveConfiguration(object sender, EventArgs e)
        {
            
        }
    }
}
