using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow
{
    /// <summary>
    /// Defines a set of supplimentary values that can be used by the <code>SetWindowLong</code> and <code>SetWindowLongPtr</code> methods.
    /// </summary>
    [Flags]
    public enum SupplementaryWindowAttributes
        : int
    {
        /// <summary>
        /// Sets a new extended window style.
        /// <code>GWL_EXSTYLE</code>
        /// </summary>
        NewExtendedWindowStyle = -20,

        /// <summary>
        /// Sets a new application instance handle.
        /// <code>GWLP_HINSTANCE</code>
        /// </summary>
        NewApplicationInstanceHandle = -6,

        /// <summary>
        /// Sets a new identifier of the child window. The window cannot be a top-level window.
        /// <code>GWLP_ID</code>
        /// </summary>
        NewChildWindowIdentifier = -12,

        /// <summary>
        /// Sets a new window style.
        /// <code>GWL_STYLE</code>
        /// </summary>
        NewWindowStyle = -16,

        /// <summary>
        /// Sets the user data associated with the window. 
        /// This data is intended for use by the application that created the window.
        /// Its value is initially zero.
        /// <code>GWLP_USERDATA</code>
        /// </summary>
        WindowUserData = -21,

        /// <summary>
        /// Sets a new address for the window procedure.
        /// <code>GWLP_WNDPROC</code>
        /// </summary>
        NewWindowProcedureAddress = -4,
    }
}
