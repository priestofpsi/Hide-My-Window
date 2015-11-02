using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace theDiary.Tools.HideMyWindow
{
    public class HotKeyBindingList
        : BindingList<Hotkey>
    {
        #region Constant Declarations

        private static Hotkey defaultHideCurrentWindowHotKey;
        private static Hotkey defaultUnhideLastWindowHotKey;

        #endregion

        #region Properties

        public static Hotkey DefaultHideCurrentWindowHotKey
        {
            get
            {
                if (HotKeyBindingList.defaultHideCurrentWindowHotKey == null)
                    HotKeyBindingList.defaultHideCurrentWindowHotKey = new Hotkey
                    {
                        Control = true,
                        Alt = true,
                        Key = "H",
                        Function = HotkeyFunction.HideCurrentWindow
                    };

                return HotKeyBindingList.defaultHideCurrentWindowHotKey;
            }
        }

        public static Hotkey DefaultUnhideLastWindowHotKey
        {
            get
            {
                if (HotKeyBindingList.defaultUnhideLastWindowHotKey == null)
                    HotKeyBindingList.defaultUnhideLastWindowHotKey = new Hotkey
                    {
                        Control = true,
                        Alt = true,
                        Key = "S",
                        Function = HotkeyFunction.UnhideLastWindow
                    };

                return HotKeyBindingList.defaultUnhideLastWindowHotKey;
            }
        }

        #endregion
    }
}