using System;

namespace theDiary.Tools.HideMyWindow
{
    /// <summary>
    /// The delegate that handles the events for a <see cref="IsolatedStorageFileBase" /> instance.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">An instance of <see cref=""FileEventArgs/> containing the details of the event.</param>
    public delegate void FileEventHandler(object sender, FileEventArgs e);

    public class FileEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileEventArgs"/> class.
        /// </summary>
        public FileEventArgs()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileEventArgs"/> class, with the specified <paramref name="fileName"/> value.
        /// </summary>
        /// <param name="fileName">A <see cref="String"/> containing the filename of associated to the event been raised.</param>
        public FileEventArgs(string fileName)
            : this(fileName, FileEventTypes.None)
        {

        }

        public FileEventArgs(string fileName, FileEventTypes @event)
            : this()
        {
            this.FileName = fileName;
            this.Event = @event;
        }

        #endregion

        #region Properties

        public string FileName { get; private set; }

        public FileEventTypes Event { get; }

        public string EventText
        {
            get
            {
                if (this.Event == FileEventTypes.None)
                    return string.Empty;

                return this.Event.ToString();
            }
        }

        #endregion
    }

    public enum FileEventTypes
    {
        None,

        Creating,

        Created,

        Opening,

        Opened,

        Loading,

        Loaded,

        Saving,

        Saved,

        Deleting,

        Deleted
    }
}