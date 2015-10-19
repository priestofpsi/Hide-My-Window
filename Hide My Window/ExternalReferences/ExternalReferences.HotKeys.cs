using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    internal static partial class ExternalReferences
    {
        #region Private Declarations

        internal const int WM_HOTKEY = 0x0312;
        internal const int WM_SYSCOMMAND = 0x0112;
        internal const int SC_MINIMIZE = 0xF020;

        private static short HotKeyIDCounter = 16345;
        internal static IntPtr MainHandle;

        #endregion Private Declarations

        internal static void SetHandle(Form mainForm)
        {
            if (ExternalReferences.MainHandle == IntPtr.Zero)
                ExternalReferences.MainHandle = mainForm.Handle;
        }

        #region Private Static Methods & Functions

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hwnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern void UnregisterHotKey(IntPtr hwnd, int id);

        #endregion Private Static Methods & Functions
    }
}