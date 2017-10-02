namespace theDiary.Tools.HideMyWindow
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Collections.Generic;
    using System.Linq;

    internal static class GlobalHotKeyManager
    {
        static GlobalHotKeyManager()
        {
        }
        #region Declarations

        #region Static Declarations
        private static int hotKeyId = 0;
        private static IntPtr mainFormHandle = IntPtr.Zero;
        public const int HotKeyPressedMessageIdentifier = 0x0312;

        public const int WindowCommandMessageIdentifier = 0x0112;

        public const int MinimizeMessageIdentifier = 0xF020;

        internal static IntPtr MainFormHandle
        {
            get
            {
                return GlobalHotKeyManager.mainFormHandle;
            }
        }

        #endregion

        #endregion

        #region Methods & Functions
        internal static void SetMainFormHandle(IntPtr handle)
        {
            GlobalHotKeyManager.mainFormHandle = handle;
        }

        internal static int  RegisterGlobalHotKey(int oldId, HotModifierKeys modifierKeys, Keys hotKey)
        {
            if (oldId != 0)
                UnregisterGlobalHotKey(oldId);
            try
            {
                int id = System.Threading.Interlocked.Increment(ref hotKeyId);
                // GlobalHotKeyApplicationId;
                if (!NativeMethods.RegisterHotKey(GlobalHotKeyManager.MainFormHandle, id, (uint) modifierKeys,
                        (uint) hotKey))
                    return 0;
                return id;
            }
            catch
            {
                return 0;
            }
        }

        internal static void UnregisterGlobalHotKey(int id)
        {
            NativeMethods.UnregisterHotKey(GlobalHotKeyManager.MainFormHandle, id);
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

        internal static void RegisterAll(IntPtr mainFormHandle)
        {
            GlobalHotKeyManager.mainFormHandle = mainFormHandle;
            GlobalHotKeyManager.RegisterAll();
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