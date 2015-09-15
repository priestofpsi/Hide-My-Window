using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    public partial class UnlockForm : Form
    {
        public UnlockForm()
        {
            InitializeComponent();
        }

        public UnlockForm(string title)
            : this()
        {
            this.Text = string.Format("Unlock - {0}", title);
        }

        public bool PasswordMatched
        {
            get
            {
                return this.passwordTextBox1.Password.GetMD5Hash().Equals(Runtime.Instance.Settings.Password);
            }
        }

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
    }
}
