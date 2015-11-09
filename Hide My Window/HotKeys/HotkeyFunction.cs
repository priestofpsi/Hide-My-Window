using System;
using System.Collections.Generic;
using System.Linq;

namespace theDiary.Tools.HideMyWindow
{
    /// <summary>
    ///     The hotkey functions that are natively support by the application.
    /// </summary>
    public enum HotkeyFunction
    {
        /// <summary>
        ///     The function used to hide the window currently focused.
        /// </summary>
        HideCurrentWindow,

        /// <summary>
        ///     The function used to toggle the last hidden window.
        /// </summary>
        ToggleLastWindow,

        /// <summary>
        ///     The function used to unhide the last hidden window.
        /// </summary>
        UnhideLastWindow,

        /// <summary>
        ///     The function used to unhide all hidden windows.
        /// </summary>
        UnhideAllWindows
    }
}