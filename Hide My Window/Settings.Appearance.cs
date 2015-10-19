using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    public partial class Settings
    {
        private string applicationIconPath = null;
        private static Icon DefaultApplicationIcon = new Icon(typeof(Settings).Assembly.GetManifestResourceStream("theDiary.Tools.HideMyWindow.Resources.application.ico"));

        #region Public Events
        public event EventHandler<IconEventArgs> ApplicationIconChanged;

        public event MessageBoxHandler ConfirmIconOverride;
        #endregion


        public Icon ApplicationIcon
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.ApplicationIconPath)
                    && System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly().FileExists(this.ApplicationIconPath))
                    return new Icon(this.ApplicationIconPath);

                return Settings.DefaultApplicationIcon;
            }
        }

        public string ApplicationIconPath
        {
            get
            {
                return this.applicationIconPath;
            }
            set
            {
                if (this.applicationIconPath == value)
                    return;

                this.applicationIconPath = value;
                if (this.ApplicationIconChanged != null)
                    this.ApplicationIconChanged(this, new IconEventArgs(this.ApplicationIcon));
            }
        }

        public void ClearIcon()
        {
            this.ApplicationIconPath = null;
        }

        public void SetIcon(string iconPath)
        {
            if (string.IsNullOrWhiteSpace(iconPath))
                throw new ArgumentNullException("iconPath");
            if (!File.Exists(iconPath))
                throw new FileNotFoundException("iconPath");

            string iconFileName = Path.GetFileName(iconPath);
            if (!System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly().FileExists(iconFileName)
                || (this.ConfirmIconOverride != null
                    && this.ConfirmIconOverride(this, new MessageBoxEventArgs() { Text = "Replace existing application Icon?", Caption = "Replace Application Icon", Buttons = MessageBoxButtons.YesNo, CancelResult = DialogResult.No })))
            {
                System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly().CopyFile(iconPath, iconFileName);
                this.ApplicationIconPath = iconPath;
            }
        }
        
    }
    public delegate bool MessageBoxHandler(object sender, MessageBoxEventArgs e);
}
