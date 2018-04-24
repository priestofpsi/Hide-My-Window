namespace theDiary.Tools.HideMyWindow
{
    using System.Text;
    using System.Windows.Forms;

    public partial class UnlockForm : Form
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnlockForm"/> class.
        /// </summary>
        public UnlockForm()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnlockForm"/> class with the specified <paramref name="title"/>.
        /// </summary>
        /// <param name="title">A <see cref="String"/> value to use as the <c>Title</c> of the form.</param>
        public UnlockForm(string title)
            : this()
        {
            if (!string.IsNullOrWhiteSpace(title))
                this.Text = $"Unlock - {title}";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating if the value entered is a match.
        /// </summary>
        public bool IsMatched
        {
            get
            {
                return this.passwordTextBox1.Password.GetMd5Hash().Equals(Runtime.Instance.Settings.Password);
            }
        }

        #endregion

        #region Methods & Functions
        private void UnlockForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        #endregion
    }
}