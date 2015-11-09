using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    public partial class UnlockForm : Form
    {
        #region Public Constructors
        public UnlockForm()
        {
            this.InitializeComponent();
        }

        public UnlockForm(string title)
            : this()
        {
            this.Text = string.Format("Unlock - {0}", title);
        }
        #endregion

        #region Properties
        public bool PasswordMatched
        {
            get
            {
                return this.passwordTextBox1.Password.GetMD5Hash().Equals(Runtime.Instance.Settings.Password);
            }
        }
        #endregion

        #region Methods & Functions
        private static string GetHashString(byte[] value)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in value)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

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