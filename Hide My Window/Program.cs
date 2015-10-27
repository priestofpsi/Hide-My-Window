using System;
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += Program.Application_ApplicationExit;
            Program.MainForm = new MainForm();
            Application.Run(Program.MainForm);
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (Runtime.Instance.Settings.RestoreWindowsOnExit)
                Program.MainForm.UnhideAllWindows(sender, e);
            Settings.Save(Runtime.Instance.Settings);
        }

        #endregion
    }
}