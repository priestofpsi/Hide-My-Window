using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow
{
    internal static class Extensions
    {
        public static Icon GetApplicationIcon(this Process process)
        {
            return Icon.ExtractAssociatedIcon(process.MainModule.FileName);
        }
    }
}
