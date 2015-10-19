using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    internal static partial class ExternalReferences
    {
        internal static IntPtr CurrentState(IntPtr handle)
        {
            return ExternalReferences.GetWindowLongPtr64(handle, ExternalReferences.GWL_STYLE);
        }

        internal static bool HideWindow(WindowInfo window)
        {
            window.OriginalState = ExternalReferences.GetWindowLongPtr(window.Handle, ExternalReferences.GWL_STYLE).ToInt64();
            long style = window.OriginalState;
            style &= ~(ExternalReferences.WS_VISIBLE);    // this works - window become invisible

            style |= ExternalReferences.WS_EX_TOOLWINDOW;   // flags don't work - windows remains in taskbar
            style &= ~(ExternalReferences.WS_EX_APPWINDOW);

            ExternalReferences.ShowWindow(window.Handle, (int)WindowCommand.HideWindow); // hide the window
            ExternalReferences.SetWindowLongPtr(window.Handle, ExternalReferences.GWL_STYLE, new IntPtr(style)); // set the style
            ExternalReferences.ShowWindow(window.Handle, (int)WindowCommand.ShowNoActivate); // show the window for the new style to
            bool returnValue = ExternalReferences.ShowWindow(window.Handle, (int)WindowCommand.HideWindow);
            if (!returnValue)
                ExternalReferences.ShowWindow(window);

            return returnValue;
        }

        internal static void ShowWindow(WindowInfo window)
        {
            IntPtr style1 = ExternalReferences.GetWindowLongPtr(window.Handle, ExternalReferences.GWL_STYLE);

            ExternalReferences.ShowWindow(window.Handle, (int)WindowCommand.ShowNoActivate); // show the window for the new style to
            ExternalReferences.SetWindowLongPtr(window.Handle, ExternalReferences.GWL_STYLE, new IntPtr(window.OriginalState)); // set the style
            ExternalReferences.ShowWindow(window.Handle, (int)WindowCommand.HideWindow); // show the window for the new style to
            ExternalReferences.ShowWindow(window.Handle, (int)WindowCommand.ShowNoActivate); // show the window for the new style to
            window.OriginalState = 0;
        }

        public static void SetWindowState(IntPtr hWnd, WindowCommand state)
        {
            ShowWindow(hWnd, (int)state);
        }

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, string text, int count);

        [DllImport("user32.dll")]
        public static extern int GetWindowModuleFileName(IntPtr hWnd, out string fileName, uint maxLength);

        internal static short RegisterGlobalHotKey(short oldID, HotModifierKeys modifierKeys, Keys hotKey)
        {
            if (oldID != 0)
                UnregisterGlobalHotKey(oldID);

            try
            {
                var newID = HotKeyIDCounter++;
                if (!RegisterHotKey(ExternalReferences.MainHandle, newID, (uint)modifierKeys, (uint)hotKey))
                    return 0;
                return newID;
            }
            catch
            {
                return 0;
            }
        }

        internal static void UnregisterGlobalHotKey(short id)
        {
            UnregisterHotKey(ExternalReferences.MainHandle, id);
        }

        internal static Hotkey AddOrFind(HotkeyFunction hotkeyFunction)
        {
            var hkey = Runtime.Instance.Settings.Hotkey.ToList().Find(x => x.Function == hotkeyFunction);
            if (hkey == null)
                Runtime.Instance.Settings.Hotkey.Add(hkey = new Hotkey { Function = hotkeyFunction });

            return hkey;
        }

        internal static void RegisterAll()
        {
            foreach (var hotkey in Runtime.Instance.Settings.Hotkey)
                hotkey.Register();
        }

        internal static void UnregisterAll()
        {
            foreach (var hotkey in Runtime.Instance.Settings.Hotkey)
                hotkey.Unregister();
        }
    }
}
