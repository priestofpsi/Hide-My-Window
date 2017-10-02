namespace theDiary.Tools.HideMyWindow
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Text;

    internal static partial class NativeMethods
    {
        #region Methods & Functions
        internal static WindowInfo GetForegroundWindow()
        {
            IntPtr hWnd = NativeMethods.GetForegroundWindowHandle();
            if (hWnd == IntPtr.Zero)
                return null;

            return WindowInfo.Find(hWnd);
        }

        /// <summary>
        /// Changes an attribute of the specified window. The function also sets a value at the specified offset in the extra window memory.
        /// </summary>
        /// <remarks>This method will execute according to the underlying system architecture.</remarks>
        /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
        /// <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, 
        /// minus the size of an integer. To set any other value, specify one of the following values.</param>
        /// <param name="dwNewLong"></param>
        /// <returns>If the function succeeds, the return value is the previous value of the specified 32-bit integer; otherwise  the return value is zero.</returns>
        internal static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            IntPtr returnValue;
            if (IntPtr.Size == 4)
                returnValue = NativeMethods.SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
            returnValue = NativeMethods.SetWindowLongPtr64(hWnd, nIndex, dwNewLong);

            System.Drawing.Size size = NativeMethods.GetWindowSize(hWnd);
            System.Drawing.Point location = NativeMethods.GetWindowPosition(hWnd);
            SetWindowPos(hWnd, IntPtr.Zero, location.Y, location.X, size.Width, size.Height, WindowPositionFlags.FrameChanged);

            return returnValue;
        }

        internal static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 4)
                return NativeMethods.GetWindowLongPtr32(hWnd, nIndex);
            return NativeMethods.GetWindowLongPtr64(hWnd, nIndex);
        }

        public static void RestoreWindowText(this WindowInfo window)
        {
            NativeMethods.SetWindowText(window.Handle, window.OriginalTitle);
        }

        public static void SetWindowText(this WindowInfo window, string text)
        {
            NativeMethods.SetWindowText(window.Handle, text);
        }

        public static void SetWindowText(this WindowInfo window, string prefix, string suffix)
        {
            NativeMethods.SetWindowText(window.Handle,
                string.Format("{0}{2}{1}", prefix, suffix, window.OriginalTitle));
        }

        public static void SetWindowIcon(this WindowInfo window, Icon icon)
        {
            NativeMethods.SendMessage(window.Handle, NativeMethods.WindowsMessageSetIcon, NativeMethods.WindowSmallIcon, icon.Handle);
            NativeMethods.SendMessage(window.Handle, NativeMethods.WindowsMessageSetIcon, NativeMethods.WindowLargeIcon, icon.Handle);
        }

        public static System.Drawing.Point GetWindowPosition(IntPtr hWnd)
        {
            NativeMethods.WindowRect rect = new NativeMethods.WindowRect();
            if (!NativeMethods.GetWindowRect(hWnd, ref rect))
                throw new InvalidOperationException("Unable to get the position of the Window.");

            return new Point(rect.Top, rect.Left);
        }

        public static System.Drawing.Size GetWindowSize(IntPtr hWnd)
        {
            NativeMethods.WindowRect rect = new NativeMethods.WindowRect();
            if (!NativeMethods.GetWindowRect(hWnd, ref rect))
                throw new InvalidOperationException("Unable to get the size of the Window.");

            return new Size(rect.Right - rect.Left, rect.Bottom - rect.Top);
        }

        internal static WindowPlacement GetWindowPlacement(IntPtr hWnd)
        {
            return WindowPlacement.Load(hWnd);
        }

        internal static string GetWindowText(IntPtr hWnd)
        {
            int textLength = NativeMethods.GetWindowTextLength(hWnd);
            StringBuilder returnValue = new StringBuilder(textLength + 1);
            int a = NativeMethods.GetWindowText(hWnd, returnValue, returnValue.Capacity);

            return returnValue.ToString();
        }

        internal static Process GetWindowProcess(WindowInfo window)
        {
            return NativeMethods.GetWindowProcess(window.Handle);
        }

        /// <summary>
        /// Gets the <see cref="Process"/> that owns a window handle.
        /// </summary>
        /// <param name="hWnd">The handle identifing the window.</param>
        /// <returns>A <see cref="Process"/> instances that the associated window handle belongs to.</returns>
        internal static Process GetWindowProcess(IntPtr hWnd)
        {
            uint processId;
            uint callResult = NativeMethods.GetWindowThreadProcessId(hWnd, out processId);
            Process returnValue = Process.GetProcessById((int) processId);

            return returnValue;
        }

        #endregion

        
    }
}