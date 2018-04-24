namespace theDiary.Tools.HideMyWindow
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Windows.Forms;

    public partial class SettingsStore
    {
        #region Declarations

        #region Static Declarations

        private static readonly Icon DefaultApplicationIcon =
            new Icon(
                typeof (SettingsStore).Assembly.GetManifestResourceStream(
                    "theDiary.Tools.HideMyWindow.Resources.application.ico"));

        #endregion

        #region Private Declarations

        private Icon applicationIcon;

        private string applicationIconPath;

        #endregion

        #endregion

        #region Public Event Declarations
        /// <summary>
        /// The event that is raised when the application icon has changed.
        /// </summary>
        public event EventHandler<IconEventArgs> ApplicationIconChanged;

        /// <summary>
        /// The event that is raised when prior to changing the application icon.
        /// </summary>
        public event MessageBoxHandler ConfirmIconOverride;
        #endregion

        #region Public Properties

        public Icon ApplicationIcon
        {
            get
            {
                if (this.applicationIcon == null)
                {
                    if (!string.IsNullOrWhiteSpace(this.ApplicationIconPath)
                        && IsolatedStorageFile.GetUserStoreForAssembly().FileExists(this.ApplicationIconPath))
                        this.applicationIcon = new Icon(this.ApplicationIconPath);

                    this.applicationIcon = DefaultApplicationIcon;
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

        /// <summary>
        /// Sets the application to use the default icon, specified by <c>DefaultApplicationIcon</c>.
        /// </summary>
        public void ClearIcon()
        {
            this.ApplicationIconPath = null;
        }

        /// <summary>
        /// Sets the icon used by the application.
        /// </summary>
        /// <param name="iconPath">A <see cref="String"/> value with the path to the icon to be used.</param>
        /// <exception cref="ArgumentNullException">thrown if the <paramref name="iconPath"/> is <c>Null</c> or <c>Empty</c>.</exception>
        /// <exception cref="FileNotFoundException">thrown if the file specified by <paramref name="iconPath"/> does not exist.</exception>
        public void SetIcon(string iconPath)
        {
            if (string.IsNullOrWhiteSpace(iconPath))
                throw new ArgumentNullException("iconPath");

            if (!File.Exists(iconPath))
                throw new FileNotFoundException("iconPath");

            string iconFileName = Path.GetFileName(iconPath);
            if (!IsolatedStorageFile.GetUserStoreForAssembly().FileExists(iconFileName)
                || (this.ConfirmIconOverride != null && this.ConfirmIconOverride(this, new MessageBoxEventArgs
                {
                    Text =
                        "Replace existing application Icon?",
                    Caption =
                        "Replace Application Icon",
                    Buttons =
                        MessageBoxButtons.YesNo,
                    CancelResult =
                        DialogResult.No
                })))
            {
                IsolatedStorageFile.GetUserStoreForAssembly().CopyFile(iconPath, iconFileName);
                this.ApplicationIconPath = iconPath;
            }
        }
        #endregion
    }
}