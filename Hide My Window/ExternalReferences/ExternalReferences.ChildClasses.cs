using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow
{
    internal static partial class ExternalReferences
    {
        
    }

    internal struct WindowStateStyle
    {
        private WindowStateStyle(IntPtr state)
            : this(state.ToInt64())
        {
        }

        private WindowStateStyle(long state)
        {
            this.value = state;
        }

        private long value;

        public static explicit operator IntPtr(WindowStateStyle state)
        {
            return new IntPtr(state.value);
        }

        public static explicit operator long(WindowStateStyle state)
        {
            return state.value;
        }
    }
}
