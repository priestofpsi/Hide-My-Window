namespace theDiary.Tools.HideMyWindow
{
    public class PasswordTextBox : WatermarkTextBox
    {
        #region Properties

        public string Password
        {
            get
            {
                if (string.IsNullOrEmpty(this.Text)
                    || this.Text == this.WaterMark)
                    return string.Empty;

                return this.Text;
            }
            set { this.Text = value; }
        }

        public bool ClearPassword { get; set; }

        #endregion
    }
}