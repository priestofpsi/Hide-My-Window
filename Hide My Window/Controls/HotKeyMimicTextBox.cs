namespace theDiary.Tools.HideMyWindow
{
    using System;
    using System.Windows.Forms;

    public partial class HotKeyMimicTextBox : UserControl
    {
        #region Constructors

        public HotKeyMimicTextBox()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Declarations

        #region Private Declarations

        private HotKey _hotKey;
        private HotKey _originalHotKey;
        private bool firstFocus = true;

        #endregion

        #endregion

        #region Properties

        public HotKey HotKey
        {
            get { return this._hotKey; }
            set
            {
                this._hotKey = value;
                if (this._originalHotKey == null && value != null)
                    this._originalHotKey = new HotKey(value);
                if (this._hotKey == null)
                    return;

                this.label1.Text = this._hotKey.Function.ToString().AsReadable();
                this.txtHotKey_Leave(this, new EventArgs());
            }
        }

        #endregion

        #region Methods & Functions

        public event EventHandler HotKeyChanged;

        private void txtHotKey_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey
                || e.KeyCode == Keys.ControlKey
                || e.KeyCode == Keys.Menu)
                return;

            this._hotKey.Key = e.KeyCode;
            this._hotKey.ModifierKeys = HotModifierKeys.None;

            if (e.Control)
                this._hotKey.ModifierKeys = this._hotKey.ModifierKeys | HotModifierKeys.Control;

            if (e.Alt)
                this._hotKey.ModifierKeys = this._hotKey.ModifierKeys | HotModifierKeys.Alt;

            if (e.Shift)
                this._hotKey.ModifierKeys = this._hotKey.ModifierKeys | HotModifierKeys.Shift;

            if (e.Modifiers == Keys.LWin
                || e.Modifiers == Keys.RWin)
                this._hotKey.ModifierKeys = this._hotKey.ModifierKeys | HotModifierKeys.Win;

            this.txtHotKey.Text = this._hotKey.ToString();
            if (this._hotKey != this._originalHotKey
                && this.HotKeyChanged != null)
                this.HotKeyChanged(this, new EventArgs());
        }

        private void txtHotKey_Enter(object sender, EventArgs e)
        {
            if (!this.firstFocus)
                return;

            this.txtHotKey.Text = "";
            this.firstFocus = false;
        }

        private void txtHotKey_Leave(object sender, EventArgs e)
        {
            if (this._hotKey == null
                || this._hotKey.IsEmpty)
                this.txtHotKey.Text = "Mimic Hot Key In Here";
            else
                this.txtHotKey.Text = this._hotKey.ToString();
        }

        #endregion
    }
}