using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    static class Program
    {
        private static MainForm mainForm;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Runtime.Instance.Settings.Hotkey.Add(new Hotkey()
            {
                Control = true,
                Alt = true,
                Key = "H",
                Function = HotkeyFunction.HideCurrentWindow,
            });
            Runtime.Instance.Settings.Hotkey.Add(new Hotkey()
            {
                Control = true,
                Alt = true,
                Key = "S",
                Function = HotkeyFunction.UnhideLastWindow,
            });


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += Application_ApplicationExit;
            Program.mainForm = new MainForm();
            Application.Run(Program.mainForm);
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Program.mainForm.UnhideAllWindows(sender, e);
            Settings.Save(Runtime.Instance.Settings);
        }
    }
}
