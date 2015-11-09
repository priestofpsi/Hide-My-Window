using System;
using System.Collections.Generic;
using System.Linq;

namespace theDiary.Tools.HideMyWindow
{
    /// <summary>
    ///     Represents the class that contains details about a Notification.
    /// </summary>
    public class NotificationEventArgs : EventArgs
    {
        #region Public Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="NotificationEventArgs" /> class with the specified
        ///     <paramref name="message" />.
        /// </summary>
        /// <param name="message">A <see cref="string" /> value describing the notification</param>
        /// <exception cref="ArgumentNullException">thrown when <paramref name="message" /> is <c>Null</c> or <c>Empty</c>.</exception>
        public NotificationEventArgs(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message));

            this.NotificationDate = DateTime.Now;
            this.Message = message;
        }
        #endregion

        #region Protected Constructors
        /// <summary>
        ///     Protected initialize
        /// </summary>
        /// <param name="message"></param>
        /// <param name="notificationDate"></param>
        protected NotificationEventArgs(string message, DateTime notificationDate)
        {
            this.NotificationDate = DateTime.Now;
            this.Message = message;
        }
        #endregion

        #region Properties
        /// <summary>
        ///     Gets the <see cref="DateTime" /> that the notification was raised.
        /// </summary>
        public DateTime NotificationDate
        {
            get;
            protected set;
        }

        /// <summary>
        ///     Gets the message describing the raised notification.
        /// </summary>
        public string Message
        {
            get;
            protected set;
        }
        #endregion
    }
}