namespace theDiary.Tools.HideMyWindow
{
    using System;

    [Flags]
    public enum HotModifierKeys : uint
    {
        None = 0,

        Alt = 1,

        Control = 2,

        Shift = 4,

        Win = 8,

        All = Alt | Control | Shift | Win
    }
}