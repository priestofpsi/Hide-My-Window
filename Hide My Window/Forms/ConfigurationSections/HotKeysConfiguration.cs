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
    public partial class HotKeysConfiguration : UserControl, IConfigurationSection
    {
        public HotKeysConfiguration()
        {
            InitializeComponent();
        }

        public bool ConfigurationChanged
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string SectionName
        {
            get { return "Hot Keys"; }
        }

        public void Activated(object sender, EventArgs e)
        {
            
        }
        private bool FlagHotKeysAsChanged;
        public void LoadConfiguration(object sender, EventArgs e)
        {
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
        }

        public void ResetConfiguration(object sender, EventArgs e)
        {
            
        }

        public void SaveConfiguration(object sender, EventArgs e)
        {
            
        }

        private void OnHotKeyChanged(object sender, EventArgs e)
        {
            HotKey hotKey = (sender as HotKeyMimicTextBox).HotKey;
            Runtime.Instance.Settings.HotKeys[hotKey.Function] = hotKey;
            if (this.FlagHotKeysAsChanged)
                return;
            this.FlagHotKeysAsChanged = true;
            this.ParentForm.FormClosing += (s, e1) =>
            {
                ExternalReferences.UnregisterAll();
                ExternalReferences.RegisterAll();
            };
        }
    }
}
