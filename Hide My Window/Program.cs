using System;

using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    internal static class Program
    {
        #region Internal Static Declarations

        internal static MainForm MainForm;
        internal static bool IsConfigured;
        #endregion Internal Static Declarations

        /// <summary>
        /// The main entry point for the application. 
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += Application_ApplicationExit;
            Program.MainForm = new HideMyWindow.MainForm();
            Application.Run(Program.MainForm);
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (Runtime.Instance.Settings.RestoreWindowsOnExit)
                Program.MainForm.UnhideAllWindows(sender, e);
            Settings.Save(Runtime.Instance.Settings);
        }
    }
}
