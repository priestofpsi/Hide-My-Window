using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    public class MessageBoxEventArgs
                : EventArgs
    {
        public MessageBoxEventArgs()
            : base()
        {
            this.CancelResult = DialogResult.Cancel;
        }

        public MessageBoxButtons Buttons
        {
            get;
            set;
        }

        public string Caption
        {
            get; set;
        }

        public string Text
        {
            get; set;
        }

        public bool Cancel
        {
            get; private set;
        }

        public DialogResult CancelResult
        {
            get; set;
        }
        public static void ShowMessageBox(MessageBoxEventArgs e)
        {
            e.Cancel = (MessageBox.Show(e.Text, e.Caption, e.Buttons) == e.CancelResult);
        }

        public static void ShowMessageBox(MessageBoxEventArgs e, IWin32Window owner)
        {
            e.Cancel = (MessageBox.Show(owner, e.Text, e.Caption, e.Buttons) == e.CancelResult);
        }
    }
}
