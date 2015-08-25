using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow
{
    public class HotKeyBindingList
        : BindingList<Hotkey>
    {
        private static Hotkey defaultHideCurrentWindowHotKey;
        private static Hotkey defaultUnhideLastWindowHotKey;

        public static Hotkey DefaultHideCurrentWindowHotKey
        {
            get
            {
                if (HotKeyBindingList.defaultHideCurrentWindowHotKey == null)
                    HotKeyBindingList.defaultHideCurrentWindowHotKey = new Hotkey()
                        {
                            Control = true,
                            Alt = true,
                            Key = "H",
                            Function = HotkeyFunction.HideCurrentWindow,
                        };

                return HotKeyBindingList.defaultHideCurrentWindowHotKey;
            }
        }
        public static Hotkey DefaultUnhideLastWindowHotKey
        {
            get
            {
                if (HotKeyBindingList.defaultUnhideLastWindowHotKey == null)
                    HotKeyBindingList.defaultUnhideLastWindowHotKey = new Hotkey()
                    {
                        Control = true,
                        Alt = true,
                        Key = "S",
                        Function = HotkeyFunction.UnhideLastWindow,
                    };

                return HotKeyBindingList.defaultUnhideLastWindowHotKey;
            }
        }
    }
}
