using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow
{
    [Flags]
    public enum WindowPlacementFlags
    {
        AsyncWindowPlacement = 0x0004,

        RestoreToMaximize = 0x0002,

        SetMinimizedPostion = 0x0001

    }
}
