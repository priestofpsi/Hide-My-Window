using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace theDiary.Tools.HideMyWindow
{
    /// <summary>
    /// Contains information about the placement of a window on the screen.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowPlacement
    {
        #region Public Fields
        /// <summary>
        /// The length of the structure, in bytes. 
        /// </summary>
        public int Length;

        /// <summary>
        /// The flags that control the position of the minimized window and the method by which the window is restored.
        /// </summary>
        public WindowPlacementFlags Flags;

        /// <summary>
        /// The current show state of the window.
        /// </summary>
        public WindowShowCommand ShowCommand;

        /// <summary>
        /// The coordinates of the window's upper-left corner when the window is minimized.
        /// </summary>
        public Point MinPosition;

        /// <summary>
        /// The coordinates of the window's upper-left corner when the window is maximized.
        /// </summary>
        public Point MaxPosition;

        /// <summary>
        /// The window's coordinates when the window is in the restored position.
        /// </summary>
        public Rectangle NormalPosition;
        #endregion

        #region Public Static Methods & Functions
        public static WindowPlacement New()
        {
            WindowPlacement returnValue = new WindowPlacement();
            returnValue.Length = Marshal.SizeOf(returnValue);

            return returnValue;
        }

        public static WindowPlacement Load(IntPtr hWnd)
        {
            WindowPlacement returnValue = WindowPlacement.New();
            WindowPlacement.GetWindowPlacement(hWnd, ref returnValue);
            return returnValue;
        }
        #endregion

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowPlacement(IntPtr hWnd, ref WindowPlacement lpwndpl);
    }
}