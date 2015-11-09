using System;
using System.Collections.Generic;
using System.Linq;

namespace theDiary.Tools.HideMyWindow
{
    [Flags]
    public enum WindowStates
    {
        /// <summary>
        ///     Indicates that a Window is currently visible or not hidden.
        /// </summary>
        Normal = 0,

        /// <summary>
        ///     Indicates that a Window is currently hidden.
        /// </summary>
        Hidden = 1,

        /// <summary>
        ///     Indicates that a Window is password protected.
        /// </summary>
        Protected = 2,

        /// <summary>
        ///     Indicates that a Window is pinned.
        /// </summary>
        Pinned = 4
    }
}