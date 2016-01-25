namespace theDiary.Tools.HideMyWindow
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal static partial class ExternalReferences
    {
        #region Methods & Functions

        internal static IntPtr CurrentState(IntPtr handle)
        {
            return GetWindowLongPtr64(handle, GwlStyle);
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
            window.OriginalState =
                GetWindowLongPtr(window.Handle, GwlStyle).ToInt64();

            long style = window.GetOriginalStateStyle();

            ShowWindow(window.Handle, (int) ShowWindowCommands.Hide); // hide the window
            SetWindowLongPtr(window.Handle, GwlStyle, new IntPtr(style));
            ShowWindow(window.Handle, (int) ShowWindowCommands.ShowNA);

            // show the window for the new style to
            bool returnValue = ShowWindow(window.Handle, (int) ShowWindowCommands.Hide);
            if (!returnValue)
                ShowWindow(window);

            return returnValue;
        }

        internal static void ShowWindow(WindowInfo window)
        {
            IntPtr style1 = GetWindowLongPtr(window.Handle, GwlStyle);

            ShowWindow(window.Handle, (int) ShowWindowCommands.ShowNA);

            // show the window for the new style to
            SetWindowLongPtr(window.Handle, GwlStyle,
                new IntPtr(window.OriginalState)); // set the style
            ShowWindow(window.Handle, (int) ShowWindowCommands.Hide);

            // show the window for the new style to
            ShowWindow(window.Handle, (int) ShowWindowCommands.ShowNA);

            // show the window for the new style to
            window.OriginalState = 0;
        }

        public static void SetWindowState(IntPtr hWnd, ShowWindowCommands state)
        {
            ShowWindow(hWnd, (int) state);
        }

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, string text, int count);

        [DllImport("user32.dll")]
        public static extern int GetWindowModuleFileName(IntPtr hWnd, out string fileName, uint maxLength);

        internal static short RegisterGlobalHotKey(short oldId, HotModifierKeys modifierKeys, Keys hotKey)
        {
            if (oldId != 0)
                UnregisterGlobalHotKey(oldId);

            try
            {
                short newId = (short) Runtime.Instance.randomizer.Next(short.MinValue, short.MaxValue);
                if (
                    !RegisterHotKey(MainHandle, newId, (uint) modifierKeys,
                        (uint) hotKey))
                    return 0;
                return newId;
            }
            catch
            {
                return 0;
            }
        }

        internal static void UnregisterGlobalHotKey(short id)
        {
            UnregisterHotKey(MainHandle, id);
        }

        internal static HotKey AddOrFind(HotKeyFunction hotKeyFunction)
        {
            HotKey hkey = Runtime.Instance.Settings.HotKeys.ToList().Find(x => x.Function == hotKeyFunction);
            if (hkey == null)
            {
                Runtime.Instance.Settings.HotKeys.Add(hkey = new HotKey(hotKeyFunction));
            }

            return hkey;
        }

        internal static void RegisterAll()
        {
            foreach (HotKey hotkey in Runtime.Instance.Settings.HotKeys)
                hotkey.Register();
        }

        internal static void UnregisterAll()
        {
            foreach (HotKey hotkey in Runtime.Instance.Settings.HotKeys)
                hotkey.Unregister();
        }

        #endregion
    }
}