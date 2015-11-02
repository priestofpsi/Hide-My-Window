using System;
using System.Collections.Generic;
using System.Linq;

namespace theDiary.Tools.HideMyWindow
{
    [Flags]
    public enum HotModifierKeys : uint
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        Win = 8,
        All = HotModifierKeys.Alt | HotModifierKeys.Control | HotModifierKeys.Shift | HotModifierKeys.Win
    }
}