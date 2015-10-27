using System;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    public class MessageBoxEventArgs
        : EventArgs
    {
        #region Constructors

        public MessageBoxEventArgs()
        {
            this.CancelResult = DialogResult.Cancel;
        }

        #endregion

        #region Properties

        public MessageBoxIcon Icon { get; set; }

        public MessageBoxButtons Buttons { get; set; }

        public string Caption { get; set; }

        public string Text { get; set; }

        public DialogResult CancelResult { get; set; }

        public bool Cancel { get; private set; }

        #endregion

        #region Methods & Functions

        public static void ShowMessageBox(MessageBoxEventArgs e)
        {
            e.Cancel = (MessageBox.Show(e.Text, e.Caption, e.Buttons) == e.CancelResult);
        }

        public static void ShowMessageBox(MessageBoxEventArgs e, IWin32Window owner)
        {
            e.Cancel = (MessageBox.Show(owner, e.Text, e.Caption, e.Buttons) == e.CancelResult);
        }

        #endregion
    }
}