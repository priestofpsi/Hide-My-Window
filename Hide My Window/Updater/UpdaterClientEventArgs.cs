using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace theDiary.Tools.HideMyWindow
{
    public class UpdaterClientEventArgs : NotificationEventArgs
    {
        #region Constructors

        public UpdaterClientEventArgs()
            : this("Checking for Updates")
        {
            this.Completed = false;
        }

        public UpdaterClientEventArgs(Exception error)
            : this("Error Checking")
        {
            this.Error = error;
            this.Completed = true;
        }

        public UpdaterClientEventArgs(IEnumerable<AvailableUpdate> items)
            : this("Finished checking for Updates")
        {
            items.AsParallel().ForAll(item =>
            {
                this.items.Add(item.Date, item);
            });
            this.Completed = true;
        }
        public UpdaterClientEventArgs(IEnumerable<Release> items)
            : this("Finished checking for Updates")
        {
            items.AsParallel().ForAll(item =>
            {
                var update = new AvailableUpdate(item);
                this.items.Add(update.Date, update);
            });
            this.Completed = true;
        }
        private UpdaterClientEventArgs(string message)
            : base(message)
        {
            this.items = new SortedList<DateTime, AvailableUpdate>();
        }

        #endregion

        #region Declarations

        #region Private Declarations

        private readonly SortedList<DateTime, AvailableUpdate> items;

        #endregion

        #endregion

        #region Properties

        public Exception Error
        {
            get;
        }

        public bool Completed
        {
            get; private set;
        }

        public bool Success
        {
            get
            {
                return this.Error == null;
            }
        }

        public int Count
        {
            get
            {
                return this.items.Count;
            }
        }

        #endregion
    }
}
