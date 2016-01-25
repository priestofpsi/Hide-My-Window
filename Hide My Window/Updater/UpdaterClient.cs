using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow
{
    using System;
    using System.Collections.Generic;
    using Octokit;

    public class UpdaterClient : IDisposable
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="UpdaterClient" /> class.
        /// </summary>
        public UpdaterClient()
            : this(gitHubUser, gitHubProject)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UpdaterClient" /> class with the specified
        ///     <paramref name="userName" /> and <paramref name="projectName" />.
        /// </summary>
        /// <param name="userName">The name of the GitHub user that the specified <paramref name="projectName" /> belongs to.</param>
        /// <param name="projectName">The name of the Project on GitHub to attach to.</param>
        public UpdaterClient(string userName, string projectName)
        {
            this.UserName = userName;
            this.ProjectName = projectName;
        }

        #endregion

        #region Declarations

        #region Public Static Declarations

        private const string gitHubUser = "priestofpsi";
        private const string gitHubProject = "Hide-My-Window";

        #endregion

        #region Private Static Declarations

        private readonly object syncObject = new object();
        private bool disposedValue;

        #endregion

        #endregion

        #region Interface Implementations

        public void Dispose()
        {
            this.Dispose(true);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the name of the GitHub user that the project belongs to.
        /// </summary>
        public string UserName
        {
            get;
        }

        /// <summary>
        ///     Gets the name of the Project on GitHub to attach to.
        /// </summary>
        public string ProjectName
        {
            get;
        }
        #endregion

        #region Methods & Functions

        public event NotificationEventHandler Notification;
        public event ClientUpdatesEventHandler Updating;
        public event ClientUpdatesAvailableEventHandler UpdatesAvailable;
        public event ClientUpdatesEventHandler NoUpdatesAvailable;

        public async void GetAvailableUpdates()
        {
            try
            {
                var client = this.InitializeClient();
                this.Notification?.Invoke(this, new NotificationEventArgs("Checking for available Updates"));
                this.Updating?.Invoke(this, new UpdaterClientEventArgs());

                IReadOnlyList<Release> returnValue =
                    await client.Release.GetAll(gitHubUser, gitHubProject);
                
                this.Updating?.Invoke(this, new UpdaterClientEventArgs(returnValue));
                AvailableUpdate[] updates = this.GetUpdates(returnValue);
                if (returnValue.Count > 0)
                {
                    this.UpdatesAvailable?.Invoke(this, new AvailableUpdatesEventArgs(updates));
                }
                else
                {
                    this.NoUpdatesAvailable?.Invoke(this, new UpdaterClientEventArgs());
                }
            }
            catch (Exception ex)
            {
                if (this.Updating != null)
                    this.Updating(this, new UpdaterClientEventArgs(ex));
            }
            finally
            {
                if (this.Notification != null)
                    this.Notification(this, new NotificationEventArgs("Finished checking for available updates."));
            }
        }
        private AvailableUpdate[] GetUpdates(IEnumerable<Release> items)
        {
            List<AvailableUpdate> updates = new List<AvailableUpdate>();
            foreach(var item in items)
            {
                var client = this.InitializeClient();
                var assets = client.Release.GetAllAssets(this.UserName, this.ProjectName, item.Id);
                assets.Wait();
                updates.Add(new AvailableUpdate(item, assets.Result));
            }

            return updates.ToArray();
        }
        /// <summary>
        ///     Initializes a new instance of the <see cref="GitHubClient" />.
        /// </summary>
        /// <returns>An instance of <see cref="GitHubClient" />.</returns>
        private GitHubClient InitializeClient()
        {
            GitHubClient returnValue = null;
            if (this.Notification != null)
                this.Notification(this, new NotificationEventArgs("Client Initializing"));

            returnValue = new GitHubClient(new ProductHeaderValue(this.UserName, this.ProjectName));

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

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                this.disposedValue = true;
            }
        }

        #endregion
    }
}