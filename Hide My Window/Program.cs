namespace theDiary.Tools.HideMyWindow
{
    using System;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Forms;

    internal static class Program
    {
        #region Declarations

        #region Static Declarations

        internal static MainForm MainForm;
        internal static bool IsConfigured;

        #endregion

        #endregion

        #region Methods & Functions

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            if (Environment.Is64BitOperatingSystem != Environment.Is64BitProcess)
                MessageBox.Show("The application is not compiled for this architecture.");
            else
            {
                Mutex mutex;
                if (Program.IsAlreadyRunning(out mutex))
                    return;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.ApplicationExit += Application_ApplicationExit;
                MainForm = new MainForm();
                Application.Run(MainForm);
                GC.KeepAlive(mutex);
            }
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (Runtime.Instance.Settings.RestoreWindowsOnExit)
                MainForm.UnhideAllWindows(sender, e);
            SettingsStore.Save(Runtime.Instance.Settings);
        }

        private static bool IsAlreadyRunning(out Mutex mutex)
        {
            AssemblyTitleAttribute attribute;
            Assembly.GetEntryAssembly().TryGetCustomAttribute(out attribute);
            bool onlyInstance = false;
            mutex = new Mutex(true, attribute.Title, out onlyInstance);

#if DEBUG
            onlyInstance = true;
#endif
            return !onlyInstance;
        }

        #endregion
    }
}