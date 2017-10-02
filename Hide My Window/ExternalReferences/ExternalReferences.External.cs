using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow
{
    internal static partial class NativeMethods
    {
        #region Static Declarations
        internal const int WindowSmallIcon = 0;

        internal const int WindowLargeIcon = 1;

        internal const int WindowNewStyle = -16;

        internal const int WindowNewExtendedStyle = -20;

        internal const int WindowsMessageSetIcon = 0x80;

        internal const long WindowStateVisible = 0x10000000;

        internal const long WindowStateMaximize = 0x01000000;

        internal const long WindowStateBorder = 0x00800000;

        internal const long WindowStateChild = 0x40000000;

        internal const long WindowStateExApplicationWindow = 0x00040000;

        internal const long WindowStateExToolWindow = 0x00000080;

        #endregion

        [StructLayout(LayoutKind.Sequential)]
        internal struct WindowRect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        #region Shared External Methods & Functions
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool RegisterHotKey(IntPtr hwnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern void UnregisterHotKey(IntPtr hwnd, int id);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int GetWindowModuleFileName(IntPtr hWnd, out string fileName, uint maxLength);

        [DllImport("user32.dll", SetLastError = true)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowRect(IntPtr hWnd, ref WindowRect lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int SendMessage(IntPtr hWnd, int message, int wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int top, int left, int width, int height, WindowPositionFlags flags);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool SetWindowText(IntPtr hWnd, string lpString);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "GetForegroundWindow")]
        internal static extern IntPtr GetForegroundWindowHandle();

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "GetWindowTextLength")]
        internal static extern int GetWindowTextLength(IntPtr hwnd);
        #endregion

        #region x32 Architecture External Methods & Functions
        /// <summary>
        /// Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
        /// </summary>
        /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
        /// <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, 
        /// minus the size of an integer. To set any other value, specify one of the following values.</param>
        /// <param name="dwNewLong">The replacement value.</param>
        /// <returns>If the function succeeds, the return value is the previous value of the specified 32-bit integer; otherwise  the return value is zero.</returns>
        [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowLong")]
        [SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "return",
            Justification = "This declaration is not used on 64-bit Windows.")]
        [SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "2",
            Justification = "This declaration is not used on 64-bit Windows.")]
        internal static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);


        [DllImport("user32.dll", SetLastError = true, EntryPoint = "GetWindowLong")]
        [SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "return",
            Justification = "This declaration is not used on 64-bit Windows.")]
        [SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "2",
            Justification = "This declaration is not used on 64-bit Windows.")]
        internal static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);
        #endregion

        #region x64 Architecture External Methods & Functions
        /// <summary>
        /// Changes an attribute of the specified window. The function also sets a value at the specified offset in the extra window memory.
        /// </summary>
        /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs. The SetWindowLongPtr64 function fails if the process that owns 
        /// the window specified by the hWnd parameter is at a higher process privilege in the UIPI hierarchy than the process the calling thread resides in.</param>
        /// <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory,
        /// minus the size of an integer. To set any other value, specify one of the following values.</param>
        /// <param name="dwNewLong">The replacement value.</param>
        /// <returns>If the function succeeds, the return value is the previous value of the specified offset; otherwise  the return value is zero.</returns>
        [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowLongPtr")]
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist",
            Justification = "Entry point does exist on 64-bit Windows.")]
        internal static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "GetWindowLongPtr")]
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist",
            Justification = "Entry point does exist on 64-bit Windows.")]
        internal static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);
        #endregion

        #region Helper Methods & Functions
        internal static bool ShowWindow(IntPtr hWnd, WindowShowCommand showCommand)
        {
            return NativeMethods.ShowWindow(hWnd, (int) showCommand);
        }
        #endregion
    }
}
