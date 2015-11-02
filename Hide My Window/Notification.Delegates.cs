using System;
using System.Collections.Generic;
using System.Linq;

namespace theDiary.Tools.HideMyWindow
{
    /// <summary>
    ///     The delegate used to handle basic notification events.
    /// </summary>
    /// <param name="sender">The source that raised the notification.</param>
    /// <param name="e">An instance of <see cref="NotificationEventArgs" /> containing the details of the notification.</param>
    public delegate void NotificationEventHandler(object sender, NotificationEventArgs e);
}