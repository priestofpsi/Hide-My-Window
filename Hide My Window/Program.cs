using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    internal static class Program
    {
        #region Constant Declarations
        internal static MainForm MainForm;
        internal static bool IsConfigured;
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
                Application.ApplicationExit += Program.Application_ApplicationExit;
                Program.MainForm = new MainForm();
                Application.Run(Program.MainForm);
                GC.KeepAlive(mutex);
            }
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (Runtime.Instance.Settings.RestoreWindowsOnExit)
                Program.MainForm.UnhideAllWindows(sender, e);
            Settings.Save(Runtime.Instance.Settings);
        }

        private static bool IsAlreadyRunning(out Mutex mutex)
        {
            AssemblyTitleAttribute attribute;
            Assembly.GetEntryAssembly().TryGetCustomAttribute(out attribute);
            bool onlyInstance = false;
            mutex = new Mutex(true, attribute.Title, out onlyInstance);
            return !onlyInstance;
        }
        #endregion
    }
}