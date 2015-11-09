using System;
using System.Collections.Generic;
using System.Linq;
using Octokit;

namespace theDiary.Tools.HideMyWindow
{
    public class Updater : IDisposable
    {
        #region Constant Declarations
        private const string gitHubUser = "priestofpsi";
        private const string gitHubProject = "Hide-My-Window";
        #endregion

        #region Declarations
        private GitHubClient client;
        private bool disposedValue;
        #endregion

        #region Properties
        private GitHubClient Client
        {
            get
            {
                if (this.disposedValue)
                    throw new ObjectDisposedException("Updater");

                if (this.client == null)
                    this.InitializeClient();

                return this.client;
            }
        }
        #endregion

        #region Methods & Functions
        public event NotificationEventHandler Notification;
        public event ClientUpdatesEventHandler Updating;

        public async void GetAvailableUpdates()
        {
            try
            {
                if (this.Notification != null)
                    this.Notification(this, new NotificationEventArgs("Checking for available Updates"));

                if (this.Updating != null)
                    this.Updating(this, new ClientUpdatesEventArg());

                IReadOnlyList<Release> returnValue =
                    await this.Client.Release.GetAll(Updater.gitHubUser, Updater.gitHubProject);
                if (this.Updating != null)
                    this.Updating(this, new ClientUpdatesEventArg(returnValue));
            }
            catch (Exception ex)
            {
                if (this.Updating != null)
                    this.Updating(this, new ClientUpdatesEventArg(ex));
            }
            finally
            {
                if (this.Notification != null)
                    this.Notification(this, new NotificationEventArgs("Finished checking for available updates."));
            }
        }

        private GitHubClient InitializeClient()
        {
            GitHubClient returnValue = null;
            if (this.Notification != null)
                this.Notification(this, new NotificationEventArgs("Client Initializing"));

            this.client = new GitHubClient(new ProductHeaderValue(Updater.gitHubUser, Updater.gitHubProject));

            if (this.Notification != null)
                this.Notification(this, new NotificationEventArgs("Client Initialized"));

            return returnValue;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }
                this.client = null;

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                this.disposedValue = true;
            }
        }
        #endregion

        #region Interface Implementations
        public void Dispose()
        {
            this.Dispose(true);
        }
        #endregion
    }

    public delegate void ClientUpdatesEventHandler(object sender, ClientUpdatesEventArg e);

    public class ClientUpdatesEventArg : NotificationEventArgs
    {
        #region Public Constructors
        public ClientUpdatesEventArg()
            : this("Checking for Updates")
        {
            this.Completed = false;
        }

        public ClientUpdatesEventArg(Exception error)
            : this("Error Checking")
        {
            this.Error = error;
            this.Completed = true;
        }

        public ClientUpdatesEventArg(IEnumerable<Release> items)
            : this("Finished checking for Updates")
        {
            items.AsParallel().ForAll(item =>
                                      {
                                          AvailableUpdate update = new AvailableUpdate(item);
                                          this.items.Add(update.Date, update);
                                      });
            this.Completed = true;
        }
        #endregion

        #region Private Constructors
        private ClientUpdatesEventArg(string message)
            : base(message)
        {
            this.items = new SortedList<DateTime, AvailableUpdate>();
        }
        #endregion

        #region Declarations
        private readonly SortedList<DateTime, AvailableUpdate> items;
        #endregion

        #region Properties
        public Exception Error
        {
            get;
        }

        public bool Completed
        {
            get;
            private set;
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

    public class AvailableUpdate
    {
        #region Public Constructors
        public AvailableUpdate(Release releaseInfo)
        {
            this.Date = (releaseInfo.PublishedAt ?? releaseInfo.CreatedAt).LocalDateTime;
            this.Details = releaseInfo.Body;
            this.IsBeta = releaseInfo.Prerelease;
        }
        #endregion

        #region Properties
        public DateTime Date
        {
            get;
        }

        public string Details
        {
            get;
            private set;
        }

        public bool IsBeta
        {
            get;
            private set;
        }
        #endregion
    }
}