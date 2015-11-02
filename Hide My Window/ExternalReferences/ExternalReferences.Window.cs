using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace theDiary.Tools.HideMyWindow
{
    internal static partial class ExternalReferences
    {
        private static WindowPlacement GetPlacement(IntPtr hwnd)
        {
            WindowPlacement placement = new WindowPlacement();
            placement.length = Marshal.SizeOf(placement);
            GetWindowPlacement(hwnd, ref placement);
            return placement;
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowPlacement(
            IntPtr hWnd, ref WindowPlacement lpwndpl);

        

        #region Constant Declarations

        internal const int GWL_STYLE = -16,
            GWL_EXSTYLE = -20;

        internal const long WS_VISIBLE = 0x10000000,
            WS_MAXIMIZE = 0x01000000,
            WS_BORDER = 0x00800000,
            WS_CHILD = 0x40000000,
            WS_EX_APPWINDOW = 0x00040000,
            WS_EX_TOOLWINDOW = 0x00000080;

        #endregion

        #region Methods & Functions

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowLong")]
        [SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "return",
            Justification = "This declaration is not used on 64-bit Windows.")]
        [SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "2",
            Justification = "This declaration is not used on 64-bit Windows.")]
        private static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowLongPtr")]
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist",
            Justification = "Entry point does exist on 64-bit Windows.")]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "GetWindowLong")]
        [SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "return",
            Justification = "This declaration is not used on 64-bit Windows.")]
        [SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "2",
            Justification = "This declaration is not used on 64-bit Windows.")]
        private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "GetWindowLongPtr")]
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist",
            Justification = "Entry point does exist on 64-bit Windows.")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "GetWindowTextLength", SetLastError = true)]
        private static extern int GetWindowTextLength(IntPtr hwnd);

        internal static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 4)
            {
                return ExternalReferences.SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
            }
            return ExternalReferences.SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
        }

        internal static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 4)
            {
                return ExternalReferences.GetWindowLongPtr32(hWnd, nIndex);
            }
            return ExternalReferences.GetWindowLongPtr64(hWnd, nIndex);
        }

        internal static string GetWindowText(IntPtr hWnd)
        {
            int textLength = ExternalReferences.GetWindowTextLength(hWnd);
            StringBuilder returnValue = new StringBuilder(textLength + 1);
            int a = ExternalReferences.GetWindowText(hWnd, returnValue, returnValue.Capacity);

            return returnValue.ToString();
        }

        internal static Process GetWindowProcess(IntPtr hWnd)
        {
            uint processId;
            uint callResult = ExternalReferences.GetWindowThreadProcessId(hWnd, out processId);
            Process returnValue = Process.GetProcessById((int) processId);
            //returnValue.EnableRaisingEvents = true;

            return returnValue;
        }

        #endregion
    }
}