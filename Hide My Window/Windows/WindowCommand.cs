using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow
{
    public enum WindowCommand
           : int
    {
        HideWindow = 0,

        ShowNormal = 1,

        ShowMinimized = 2,

        ShowAndActivate = 5,

        ShowDefault = 10,

        ShowNormalNoActivate = 4,

        ShowNoActivate = 8,

        Maximize = 3,

        Minimize = 6,

        ShowMinimizedNoActivate = 7,

        ForceMinimize = 11,

        Restore = 9,
    }
}
