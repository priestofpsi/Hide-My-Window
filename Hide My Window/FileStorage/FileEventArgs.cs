using System;
using System.Collections.Generic;
using System.Linq;

namespace theDiary.Tools.HideMyWindow
{
    public delegate void FileEventHandler(object sender, FileEventArgs e);

    public class FileEventArgs
        : EventArgs
    {
        #region Constructors

        public FileEventArgs()
        {
        }

        public FileEventArgs(string fileName)
            : this()
        {
            this.Event = FileEventTypes.None;
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