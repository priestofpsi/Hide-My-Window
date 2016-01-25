namespace theDiary.Tools.HideMyWindow
{
    public abstract partial class IsolatedStorageFileBase : IIsolatedStorageFile
    {
        #region Constructors

        protected IsolatedStorageFileBase(string storageFileName)
        {
            this.storageFileName = storageFileName;
        }

        #endregion

        #region Declarations

        #region Private Declarations

        private string storageFileName;

        #endregion

        #endregion

        public event NotificationEventHandler Notification;

        #region Methods & Functions

        protected void RaiseNotification(NotificationEventArgs e)
        {
            this.RaiseNotification(this, e);
        }

        protected void RaiseNotification(object sender, NotificationEventArgs e)
        {
            if (this.Notification != null)
                this.Notification(sender, e);
        }

        #endregion

        #region Interface Implementations

        string IIsolatedStorageFile.GetStorageFileName()
        {
            return HiddenWindowStore.StorageFileName;
        }

        public abstract void Save();

        public abstract void Reset();

        #endregion
    }
}