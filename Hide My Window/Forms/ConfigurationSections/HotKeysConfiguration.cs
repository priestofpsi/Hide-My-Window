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
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HotKeysConfiguration"/> control.
        /// </summary>
        public HotKeysConfiguration()
        {
            InitializeComponent();
        }
        #endregion
        
        #region Private Declarations
        private bool flagHotKeysAsChanged;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets a value indicating if the configuration for the <see cref="HotKeysConfiguration"/> control has changed.
        /// </summary>
        public bool ConfigurationChanged
        {
            get
            {
                return this.flagHotKeysAsChanged;
            }
        }

        /// <summary>
        /// Gets the name identifying the section of the <see cref="IConfigurationSection"/> instance.
        /// </summary>
        public string SectionName
        {
            get
            {
                return "Hot Keys";
            }
        }
        #endregion

        public void Activated(object sender, EventArgs e)
        {

        }


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
            if (this.flagHotKeysAsChanged)
                return;

            this.flagHotKeysAsChanged = true;
            this.ParentForm.FormClosing += (s, e1) =>
            {
                GlobalHotKeyManager.UnregisterAll();
                GlobalHotKeyManager.RegisterAll();
            };
        }
    }
}
