using System;

namespace theDiary.Tools.HideMyWindow
{
    public abstract partial class IsolatedStorageFileBase 
        : IIsolatedStorageFile
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedStorageFileBase"/> class.
        /// </summary>
        /// <param name="storageFileName">The name of the file to be assigned to the instance.</param>
        /// <exception cref="ArgumentNullException">thrown if the <paramref name="storageFileName"/> is <c>Null</c>.</exception>
        protected IsolatedStorageFileBase(string storageFileName)
        {
            if (string.IsNullOrWhiteSpace(storageFileName))
                throw new ArgumentNullException();

            this.storageFileName = storageFileName;
        }

        #endregion

        #region Private Declarations

        private string storageFileName;

        #endregion

        #region Event Definitions
        /// <summary>
        /// The event that is raised when an action is performed on an <see cref="IsolatedStorageFileBase"/> implemented instance.
        /// </summary>
        public event NotificationEventHandler Notification;
        #endregion

        #region Methods & Functions

        /// <summary>
        /// Method used to raise the <see cref="Notification"/> event for the implemented instance.
        /// </summary>
        /// <param name="e">An instance of the <see cref="NotificationEventArgs"/> class.</param>
        protected void RaiseNotification(NotificationEventArgs e)
        {
            this.RaiseNotification(this, e);
        }

        /// <summary>
        /// Method used to raise the <see cref="Notification"/> event for the implemented instance.
        /// </summary>
        /// <param name="sender">The source used when raising the <see cref="Notification"/> event.</param>
        /// <param name="e">An instance of the <see cref="NotificationEventArgs"/> class.</param>
        protected void RaiseNotification(object sender, NotificationEventArgs e)
        {
            if (this.Notification != null)
                this.Notification(sender, e);
        }

        #endregion

        #region Interface Implementations

        string IIsolatedStorageFile.GetStorageFileName()
        {
            return this.storageFileName;
            //return HiddenWindowStore.StorageFileName;
        }

        /// <summary>
        /// Abstract method used to Save a <see cref="IsolatedStorageFileBase"/> instance.
        /// </summary>
        public abstract void Save();

        /// <summary>
        /// Abstract method used to Reset a <see cref="IsolatedStorageFileBase"/> instance.
        /// </summary>
        public abstract void Reset();

        #endregion
    }
}