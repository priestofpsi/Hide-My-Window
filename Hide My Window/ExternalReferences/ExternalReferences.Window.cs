namespace theDiary.Tools.HideMyWindow
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Text;

    internal static partial class ExternalReferences
    {
        #region Declarations

        #region Static Declarations

        internal const int WindowsMessageSetIcon = 0x80;

        internal const int IconSmall = 0,
            IconBig = 1;

        internal const int GwlStyle = -16,
            GwlExStyle = -20;

        internal const long WindowStateVisible = 0x10000000,
            WindowStateMaximize = 0x01000000,
            WindowStateBorder = 0x00800000,
            WindowStateChild = 0x40000000,
            WindowStateExApplicationWindow = 0x00040000,
            WindowStateExToolWindow = 0x00000080;

        #endregion

        #endregion

        #region Methods & Functions

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetWindowText(IntPtr hwnd, string lpString);

        public static void RestoreWindowText(this WindowInfo window)
        {
            SetWindowText(window.Handle, window.OriginalTitle);
        }

        public static void SetWindowText(this WindowInfo window, string text)
        {
            SetWindowText(window.Handle, text);
        }

        public static void SetWindowText(this WindowInfo window, string prefix, string suffix)
        {
            SetWindowText(window.Handle,
                string.Format("{0}{2}{1}", prefix, suffix, window.OriginalTitle));
        }

        public static void SetWindowIcon(this WindowInfo window, Icon icon)
        {
            SendMessage(window.Handle, WindowsMessageSetIcon, IconSmall,
                icon.Handle);
            SendMessage(window.Handle, WindowsMessageSetIcon, IconBig,
                icon.Handle);
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hwnd, int message, int wParam, IntPtr lParam);

        private static WindowPlacement GetPlacement(IntPtr hwnd)
        {
            WindowPlacement placement = new WindowPlacement();
            placement.length = Marshal.SizeOf(placement);
            GetWindowPlacement(hwnd, ref placement);
            return placement;
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowPlacement(IntPtr hWnd, ref WindowPlacement lpwndpl);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void ShowToFront(string windowName)
        {
            IntPtr firstInstance = FindWindow(null, windowName);
            ShowWindow(firstInstance, 1);
            SetForegroundWindow(firstInstance);
        }

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
                return SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
            return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
        }

        internal static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 4)
                return GetWindowLongPtr32(hWnd, nIndex);
            return GetWindowLongPtr64(hWnd, nIndex);
        }

        internal static string GetWindowText(IntPtr hWnd)
        {
            int textLength = GetWindowTextLength(hWnd);
            StringBuilder returnValue = new StringBuilder(textLength + 1);
            int a = GetWindowText(hWnd, returnValue, returnValue.Capacity);

            return returnValue.ToString();
        }

        internal static Process GetWindowProcess(IntPtr hWnd)
        {
            uint processId;
            uint callResult = GetWindowThreadProcessId(hWnd, out processId);
            Process returnValue = Process.GetProcessById((int) processId);

            //returnValue.EnableRaisingEvents = true;

            return returnValue;
        }

        #endregion
    }
}