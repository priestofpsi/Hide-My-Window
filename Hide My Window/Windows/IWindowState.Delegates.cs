using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow
{
    /// <summary>
    /// The delegate used to handle State changes for <see cref="IWindowStateProvider"/> implementations.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">An instance of ><see cref="WindowStateEventArgs"/> containing the information of the state changes.</param>
    public delegate void WindowStateChangedEventHandler(object sender, WindowStateEventArgs e);
}
