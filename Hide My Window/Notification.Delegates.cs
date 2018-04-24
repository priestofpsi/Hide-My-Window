using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    /// <summary>
    ///     The delegate used to handle basic notification events.
    /// </summary>
    /// <param name="sender">The source that raised the notification.</param>
    /// <param name="e">An instance of <see cref="NotificationEventArgs" /> containing the details of the notification.</param>
    public delegate void NotificationEventHandler(object sender, NotificationEventArgs e);

    /// <summary>
    /// The delegate used to raise notification events.
    /// </summary>
    /// <param name="timeout">The time period, in milliseconds, the balloon tip should display. This parameter is deprecated as of Windows Vista. Notification display times are now based on system accessibility settings.</param>
    /// <param name="title">The title to display on the balloon tip.</param>
    /// <param name="message">The text to display on the balloon tip.</param>
    /// <param name="icon">One of the System.Windows.Forms.ToolTipIcon values.</param>
    public delegate void NotificationDelegate(int timeout, string title, string message, ToolTipIcon icon);
}