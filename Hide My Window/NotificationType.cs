using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow
{
    /// <summary>
    /// Defines a set of notification types available.
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// A general notification,
        /// </summary>
        General = 0,

        /// <summary>
        /// An information notification.
        /// </summary>
        Info = 1,

        /// <summary>
        /// A warning notification.
        /// </summary>
        Warning = 2,

        /// <summary>
        /// An error notification.
        /// </summary>
        Error = 3
    }
}
