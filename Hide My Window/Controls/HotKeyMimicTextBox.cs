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
    public partial class HotKeyMimicTextBox : UserControl
    {   
        public HotKeyMimicTextBox()
        {
            InitializeComponent();
        }

        private Hotkey hotkey;
        private Hotkey originalHotkey;
        private bool _firstFocus = true;

        public event EventHandler HotKeyChanged;

        public Hotkey HotKey
        {
            get
            {
                return this.hotkey;
            }
            set
            {
                this.hotkey = value;
                if (this.hotkey == null)
                    return;

                this.label1.Text = this.hotkey.Function.ToString().AsReadable();
                this.txtHotKey_Leave(this, new EventArgs());
            }
        }

        private void txtHotKey_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.Menu)
                return;

            this.hotkey.HotKey = e.KeyCode;
            this.hotkey.ModifierKeys = HotModifierKeys.None;

            if (e.Control)
                this.hotkey.ModifierKeys = this.hotkey.ModifierKeys | HotModifierKeys.Control;

            if (e.Alt)
                this.hotkey.ModifierKeys = this.hotkey.ModifierKeys | HotModifierKeys.Alt;

            if (e.Shift)
                this.hotkey.ModifierKeys = this.hotkey.ModifierKeys | HotModifierKeys.Shift;

            if (e.Modifiers == Keys.LWin || e.Modifiers == Keys.RWin)
                this.hotkey.ModifierKeys = this.hotkey.ModifierKeys | HotModifierKeys.Win;

            this.txtHotKey.Text = this.hotkey.HotKeyString;
            if (this.hotkey != this.originalHotkey
                && this.HotKeyChanged != null)
                this.HotKeyChanged(this, new EventArgs());
        }

        private void txtHotKey_Enter(object sender, EventArgs e)
        {
            if (_firstFocus)
            {
                txtHotKey.Text = "";
                _firstFocus = false;
            }
        }

        private void txtHotKey_Leave(object sender, EventArgs e)
        {
            this.txtHotKey.Text = this.hotkey.HotKeyString;
            if (this.txtHotKey.Text == "None")
                this.txtHotKey.Text = "Mimic Hot Key In Here";
        }
    }
}
