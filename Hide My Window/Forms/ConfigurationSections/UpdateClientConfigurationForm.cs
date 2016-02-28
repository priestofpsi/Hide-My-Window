namespace theDiary.Tools.HideMyWindow.Forms.ConfigurationSections
{
    using System;
    using System.Windows.Forms;

    public partial class UpdateClientConfigurationForm : UserControl, IConfigurationSection
    {
        #region Constructors

        public UpdateClientConfigurationForm()
        {
            this.InitializeComponent();
            this.check.Click += this.Check_Click;
        }

        #endregion

        #region Declarations

        #region Private Static Declarations

        private UpdaterClient updaterClient;

        private void _updaterClient_Notification(object sender, NotificationEventArgs e)
        {
            this.listBox1.Items.Add(string.Format("[{0}]\t{1}", e.NotificationDate, e.Message));
        }

        private void _updaterClient_Updating(object sender, UpdaterClientEventArgs e)
        {
            this.listBox1.Items.Add(string.Format("[{0}]\t{1}", e.NotificationDate, e.Message));
            if (!e.Completed)
                return;

            string message = (e.Success)
                ? string.Format("[{0}]\t{1} Updates available.",
                    e.NotificationDate, e.Count)
                : string.Format("[{0}]\tError: {1}",
                    e.NotificationDate, e.Error.Message);
            this.listBox1.Items.Add(message);
        }

        private void Check_Click(object sender, EventArgs e)
        {
            this.check.Enabled = false;
            using (this.updaterClient = new UpdaterClient())
            {
                this.updaterClient.UpdatesAvailable += this.UpdaterClientOnUpdatesAvailable;
                this.updaterClient.NoUpdatesAvailable += this._updaterClient_NoUpdatesAvailable;
                this.updaterClient.Notification += this._updaterClient_Notification;
                this.updaterClient.Updating += this._updaterClient_Updating;
                this.updaterClient.GetAvailableUpdates();
            }
            this.check.Enabled = true;
        }

        private void _updaterClient_NoUpdatesAvailable(object sender, UpdaterClientEventArgs e)
        {
            this.download.Visible = false;
            this.availableUpdates.Visible = false;
        }

        private void UpdaterClientOnUpdatesAvailable(object sender, AvailableUpdatesEventArgs e)
        {
            this.availableUpdates.DataSource = e.Items;
            this.download.Visible = true;
            this.availableUpdates.Visible = true;
        }

        #endregion

        #endregion

        public string SectionName
        {
            get { return "Updates"; }
        }

        public bool ConfigurationChanged
        {
            get { return false; }
        }

        public void LoadConfiguration(object sender, EventArgs e)
        {
            using (this.updaterClient = new UpdaterClient())
                this.updaterClient.GetAvailableUpdates();
        }

        public void Activated(object sender, EventArgs e)
        {
        }

        public void ResetConfiguration(object sender, EventArgs e)
        {
        }

        public void SaveConfiguration(object sender, EventArgs e)
        {
        }
    }
}