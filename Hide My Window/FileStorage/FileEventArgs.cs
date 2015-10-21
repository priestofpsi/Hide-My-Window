using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow
{
    public delegate void FileEventHandler(object sender, FileEventArgs e);

    public class FileEventArgs
        : EventArgs
    {
        public FileEventArgs()
            : base()
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

        public string FileName
        {
            get; private set;
        }
        public FileEventTypes Event
        {
            get; private set;
        }

        public string EventText
        {
            get
            {
                if (this.Event == FileEventTypes.None)
                    return string.Empty;

                return this.Event.ToString();
            }
        }
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
        Deleted,
    }
}
