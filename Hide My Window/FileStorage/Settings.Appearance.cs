using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    public partial class Settings
    {
        #region Constant Declarations

        private static readonly Icon DefaultApplicationIcon =
            new Icon(
                typeof (Settings).Assembly.GetManifestResourceStream(
                    "theDiary.Tools.HideMyWindow.Resources.application.ico"));

        #endregion

        #region Declarations

        private Icon applicationIcon;

        private string applicationIconPath;

        #endregion

        #region Properties

        public Icon ApplicationIcon
        {
            get
            {
                if (this.applicationIcon == null)
                {
                    if (!string.IsNullOrWhiteSpace(this.ApplicationIconPath)
                        && IsolatedStorageFile.GetUserStoreForAssembly().FileExists(this.ApplicationIconPath))
                        this.applicationIcon = new Icon(this.ApplicationIconPath);

                    this.applicationIcon = Settings.DefaultApplicationIcon;
                }
                return this.applicationIcon;
            }
        }

        public string ApplicationIconPath
        {
            get { return this.applicationIconPath; }
            set
            {
                if (this.applicationIconPath == value)
                    return;

                this.applicationIconPath = value;
                if (this.ApplicationIconChanged != null)
                    this.ApplicationIconChanged(this, new IconEventArgs(this.ApplicationIcon));
            }
        }

        #endregion

        #region Methods & Functions

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
            if (!IsolatedStorageFile.GetUserStoreForAssembly().FileExists(iconFileName)
                || (this.ConfirmIconOverride != null
                    && this.ConfirmIconOverride(this,
                        new MessageBoxEventArgs
                        {
                            Text = "Replace existing application Icon?",
                            Caption = "Replace Application Icon",
                            Buttons = MessageBoxButtons.YesNo,
                            CancelResult = DialogResult.No
                        })))
            {
                IsolatedStorageFile.GetUserStoreForAssembly().CopyFile(iconPath, iconFileName);
                this.ApplicationIconPath = iconPath;
            }
        }

        public event EventHandler<IconEventArgs> ApplicationIconChanged;

        public event MessageBoxHandler ConfirmIconOverride;

        #endregion
    }
}