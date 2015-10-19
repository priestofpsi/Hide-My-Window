namespace theDiary.Tools.HideMyWindow
{
    public class PasswordTextBox
        : WatermarkTextBox
    {

        #region Constructors
        /// <summary>
        /// Initializes a new instances of the <see cref="PasswordTextBox"/> control.
        /// </summary>
        public PasswordTextBox()
        {
        }
        #endregion

        #region Public Properties
        public string Password
        {
            get
            {
                if (string.IsNullOrEmpty(this.Text)
                    || this.Text == this.WaterMark)
                    return string.Empty;

                return this.Text;
            }
            set
            {
                this.Text = value;
            }
        }

        public bool ClearPassword
        {
            get;
            set;
        }
        #endregion
    }
}