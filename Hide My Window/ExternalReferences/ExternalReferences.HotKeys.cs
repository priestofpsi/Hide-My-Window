namespace theDiary.Tools.HideMyWindow
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal static partial class ExternalReferences
    {
        #region Declarations

        #region Static Declarations

        internal const int WmHotkey = 0x0312;
        internal const int WmSyscommand = 0x0112;
        internal const int ScMinimize = 0xF020;
        internal static IntPtr MainHandle;

        #endregion

        #endregion

        #region Methods & Functions

        internal static void SetHandle(Form mainForm)
        {
            if (MainHandle == IntPtr.Zero)
                MainHandle = mainForm.Handle;
        }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hwnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern void UnregisterHotKey(IntPtr hwnd, int id);

        #endregion
    }
}