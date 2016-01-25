namespace theDiary.Tools.HideMyWindow
{
    using System;

    [Flags]
    public enum WindowPlacementFlags
    {
        AsyncWindowPlacement = 0x0004,

        RestoreToMaximize = 0x0002,

        SetMinimizedPostion = 0x0001
    }
}