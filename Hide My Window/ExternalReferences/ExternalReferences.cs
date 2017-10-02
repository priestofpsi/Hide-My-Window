namespace theDiary.Tools.HideMyWindow
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal static partial class NativeMethods
    {
        #region Methods & Functions

        internal static IntPtr CurrentState(IntPtr handle)
        {
            return GetWindowLongPtr64(handle, WindowNewStyle);
        }

        private static long GetOriginalStateStyle(this WindowInfo window)
        {
            long style = window.OriginalState;
            style &= ~(WindowStateVisible); // this works - window become invisible
            style |= WindowStateExToolWindow; // flags don't work - windows remains in taskbar
            style &= ~(WindowStateExApplicationWindow);

            return style;
        }

        internal static bool HideWindow(WindowInfo window)
        {
            IntPtr hWnd = window.Handle;
            window.OriginalState =
                NativeMethods.GetWindowLongPtr(hWnd, WindowNewStyle).ToInt64();

            long style = window.GetOriginalStateStyle();

            NativeMethods.SetWindowState(hWnd, WindowShowCommand.Hide); // hide the window
            NativeMethods.SetWindowLongPtr(hWnd, WindowNewStyle, new IntPtr(style));
            NativeMethods.SetWindowState(hWnd, WindowShowCommand.ShowCurrentWithNoActivation);

            // show the window for the new style to
            bool returnValue = NativeMethods.ShowWindow(hWnd, WindowShowCommand.Hide);
            if (!returnValue)
                NativeMethods.ShowWindow(window);

            return returnValue;
        }

        internal static void ShowWindow(WindowInfo window)
        {
            lock (window)
            {
                IntPtr hWnd = window.Handle;
                IntPtr style1 = NativeMethods.GetWindowLongPtr(hWnd, WindowNewStyle);

                NativeMethods.SetWindowState(hWnd, WindowShowCommand.ShowCurrentWithNoActivation);
                NativeMethods.SetWindowLongPtr(hWnd, WindowNewStyle, new IntPtr(window.OriginalState));
                NativeMethods.SetWindowState(hWnd, WindowShowCommand.Hide);
                NativeMethods.ShowWindow(hWnd, (int) WindowShowCommand.ShowCurrentWithNoActivation);
                window.OriginalState = 0;
            }
        }

        public static void SetWindowState(IntPtr hWnd, WindowShowCommand state)
        {
            NativeMethods.ShowWindow(hWnd, (int) state);
        }


        

        #endregion
    }
}