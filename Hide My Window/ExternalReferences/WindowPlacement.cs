namespace theDiary.Tools.HideMyWindow
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowPlacement
    {
        public int length;
        public int flags;
        public ShowWindowCommands showCmd;
        public Point ptMinPosition;
        public Point ptMaxPosition;
        public Rectangle rcNormalPosition;
    }
}