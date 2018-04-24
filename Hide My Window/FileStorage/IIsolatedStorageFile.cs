namespace theDiary.Tools.HideMyWindow
{
    /// <summary>
    /// Represents the implementation used for a file stored in IsolatedStorage.
    /// </summary>
    public interface IIsolatedStorageFile
    {

        #region Event Definitions
        /// <summary>
        /// The even that is used to raised notifications associated with the <see cref="IIsolatedStorageFile"/> instance.
        /// </summary>
        event NotificationEventHandler Notification;
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Method used to get the filename associated to the <see cref="IIsolatedStorageFile"/> instance.
        /// </summary>
        /// <returns>A <see cref="String"/> value specifying the name of the file.</returns>
        string GetStorageFileName();

        /// <summary>
        /// The method used to <c>Save</c> a <see cref="IIsolatedStorageFile"/> instance.
        /// </summary>
        void Save();

        /// <summary>
        /// The method used to<c>Reset</c> a <see cref = "IIsolatedStorageFile" /> instance to its original state.
        /// </summary>
        void Reset();

        #endregion
    }
}