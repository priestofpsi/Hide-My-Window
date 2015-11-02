using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace theDiary.Tools.HideMyWindow
{
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