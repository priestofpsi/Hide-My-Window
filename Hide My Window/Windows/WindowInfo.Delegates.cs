using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow
{
    /// <summary>
    /// The delegate that handles the event when the application for a hidden window is closed.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">An instance of <see cref="WindowInfoEventArgs"/> containing the details of the event.</param>
    public delegate void ApplicationExitedHandler(object sender, WindowInfoEventArgs e);

    /// <summary>
    /// The delegate that handles the events for a <see cref="WindowInfo"/> instance.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">An instance of <see cref="WindowInfoEventArgs"/> containing the details of the event.</param>
    public delegate void WindowEventHandler(object sender, WindowInfoEventArgs e);
}
