using System;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    public partial class HotKeyMimicTextBox : UserControl
    {
        #region Constructors

        public HotKeyMimicTextBox()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Declarations

        private bool _firstFocus = true;

        private Hotkey hotkey;
        private Hotkey originalHotkey;

        #endregion

        #region Properties

        public Hotkey HotKey
        {
            get { return this.hotkey; }
            set
            {
                this.hotkey = value;
                if (this.hotkey == null)
                    return;

                this.label1.Text = this.hotkey.Function.ToString().AsReadable();
                this.txtHotKey_Leave(this, new EventArgs());
            }
        }

        #endregion

        #region Methods & Functions

        public event EventHandler HotKeyChanged;

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
            if (this._firstFocus)
            {
                this.txtHotKey.Text = "";
                this._firstFocus = false;
            }
        }

        private void txtHotKey_Leave(object sender, EventArgs e)
        {
            this.txtHotKey.Text = this.hotkey.HotKeyString;
            if (this.txtHotKey.Text == "None")
                this.txtHotKey.Text = "Mimic Hot Key In Here";
        }

        #endregion
    }
}