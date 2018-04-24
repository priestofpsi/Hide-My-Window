using System;

namespace theDiary.Tools.HideMyWindow
{
    
    /// <summary>
    /// Represents the information for an action been performed on a file.
    /// </summary>
    public class FileEventArgs 
        : EventArgs
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

        /// <summary>
        /// Initializes a new instance of the <see cref="FileEventArgs"/> class, with the specified <paramref name="fileName"/> value and <paramref name="event"/>.
        /// </summary>
        /// <param name="fileName">A <see cref="String"/> containing the filename of associated to the event been raised.</param>
        /// <param name="event">A valuer of <see cref="FileEventTypes"/> indicating the type of action been performed on the <paramref name="fileName"/>.</param>
        public FileEventArgs(string fileName, FileEventTypes @event)
            : this()
        {
            this.FileName = fileName;
            this.Event = @event;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the file associated to the event.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets a value of <see cref="FileEventTypes".
        /// </summary>
        public FileEventTypes Event { get; }

        /// <summary>
        /// Gets a <see cref="String"/> representation of the <c>Event</c>.
        /// </summary>
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

    
}