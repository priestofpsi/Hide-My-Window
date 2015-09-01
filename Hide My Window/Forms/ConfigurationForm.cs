using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            
            this.Function.DataSource = Enum.GetNames(typeof(HotkeyFunction));
            this.Function.DataPropertyName = "HKFunction";

            this.HotKey.DataSource = Enum.GetNames(typeof(Keys));
            this.HotKey.DataPropertyName = "Key";

            this.Control.DataPropertyName = "Control";
            this.Alt.DataPropertyName = "Alt";
            this.Shift.DataPropertyName = "Shift";
            this.Win.DataPropertyName = "Win";

            this.dataGridView1.DataSource = Runtime.Instance.Settings.Hotkey;
            this.dataGridView1.CellValueChanged += (s,e)=> this.hotkeysChanged = true;
            this.FormClosing += (s, e) =>
            {
                if (this.password.Password != string.Empty)
                {
                    Runtime.Instance.Settings.HashedPassword = this.password.Password;
                }
                else if (this.password.ClearPassword)
                {
                    Runtime.Instance.Settings.HashedPassword = string.Empty;
                }
            };
        }
        private bool hotkeysChanged;

        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {
            
        }
    }
}
