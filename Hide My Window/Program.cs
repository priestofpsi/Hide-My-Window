using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    static class Program
    {
        internal static MainForm MainForm;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            //Runtime.Instance.Settings.Hotkey.Add(new Hotkey()
            //{
            //    Control = true,
            //    Alt = true,
            //    Key = "H",
            //    Function = HotkeyFunction.HideCurrentWindow,
            //});
            //Runtime.Instance.Settings.Hotkey.Add(new Hotkey()
            //{
            //    Control = true,
            //    Alt = true,
            //    Key = "S",
            //    Function = HotkeyFunction.UnhideLastWindow,
            //});

            
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
