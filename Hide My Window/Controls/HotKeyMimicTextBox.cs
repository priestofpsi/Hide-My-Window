using System;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    /// <summary>
    /// Provides a <see cref="TextBox"/> control used for capturing <c>HotKey</c> combinations.
    /// </summary>
    public partial class HotKeyMimicTextBox 
        : UserControl
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HotKeyMimicTextBox"/> control.
        /// </summary>
        public HotKeyMimicTextBox()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Event Declarations
        /// <summary>
        /// The event that is raised when a <c>HotKey</c> has changed.
        /// </summary>
        public event EventHandler HotKeyChanged;
        #endregion

        #region Private Declarations
        private HotKey hotKey;
        private HotKey originalHotKey;
        private bool firstFocus = true;
        #endregion

        #region PublicProperties
        /// <summary>
        /// Gets or sets a <see cref="HotKey"/> combination.
        /// </summary>
        public HotKey HotKey
        {
            get { return this.hotKey; }
            set
            {
                this.hotKey = value;
                if (this.originalHotKey == null && value != null)
                    this.originalHotKey = new HotKey(value);
                if (this.hotKey == null)
                    return;

                this.label1.Text = this.hotKey.Function.ToString().AsReadable();
                this.txtHotKey_Leave(this, new EventArgs());
            }
        }

        #endregion

        #region Methods & Functions
        private void txtHotKey_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey
                || e.KeyCode == Keys.ControlKey
                || e.KeyCode == Keys.Menu)
                return;

            this.hotKey.Key = e.KeyCode;
            this.hotKey.ModifierKeys = HotModifierKeys.None;

            if (e.Control)
                this.hotKey.ModifierKeys = this.hotKey.ModifierKeys | HotModifierKeys.Control;

            if (e.Alt)
                this.hotKey.ModifierKeys = this.hotKey.ModifierKeys | HotModifierKeys.Alt;

            if (e.Shift)
                this.hotKey.ModifierKeys = this.hotKey.ModifierKeys | HotModifierKeys.Shift;

            if (e.Modifiers == Keys.LWin
                || e.Modifiers == Keys.RWin)
                this.hotKey.ModifierKeys = this.hotKey.ModifierKeys | HotModifierKeys.Win;

            this.txtHotKey.Text = this.hotKey.ToString();
            if (this.hotKey != this.originalHotKey
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
            if (this.hotKey == null
                || this.hotKey.IsEmpty)
                this.txtHotKey.Text = "Mimic Hot Key In Here";
            else
                this.txtHotKey.Text = this.hotKey.ToString();
        }

        #endregion
    }
}