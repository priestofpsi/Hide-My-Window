using System;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    /// <summary>
    ///  Represents the class that contains details about a Notification.
    /// </summary>
    public class NotificationEventArgs
        : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationEventArgs" /> class with the specified <paramref name="message" />.
        /// </summary>
        /// <param name="message">A <see cref="string" /> value describing the notification.</param>
        /// <exception cref="ArgumentNullException">thrown when <paramref name="message" /> is <c>Null</c> or <c>Empty</c>.</exception>
        public NotificationEventArgs(string message)
            : this(string.Empty, message, NotificationType.Info)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationEventArgs"/> class, with the specified <paramref name="message"/> and <paramref name="type"/>.
        /// </summary>
        /// <param name="message">A <see cref="string" /> value describing the notification.</param>
        /// <param name="type">A value of <see cref="NotificationType"/> indicating the type of notification.</param>
        /// <exception cref="ArgumentNullException">thrown when <paramref name="message" /> is <c>Null</c> or <c>Empty</c>.</exception>
        public NotificationEventArgs(string message, NotificationType type)
            : this(string.Empty, message, type)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationEventArgs"/> class, with the specified <paramref name="title"/>, <paramref name="message"/> and <paramref name="type"/>.
        /// </summary>
        /// <param name="title">A <see cref="String"/> value specifying the title of the notification.</param>
        /// <param name="message">A <see cref="string" /> value describing the notification.</param>
        /// <param name="type">A value of <see cref="NotificationType"/> indicating the type of notification.</param>
        public NotificationEventArgs(string title, string message, NotificationType type)
            : this(DateTime.Now)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message));

            if (string.IsNullOrWhiteSpace(title))
                title = Application.ProductName;

            this.Title = title;
            this.Message = message;
            this.Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationEventArgs"/> class, with the specified <paramref name="notificationDate"/>.
        /// </summary>
        /// <param name="notificationDate">The date and time that the notification was raised.</param>
        private NotificationEventArgs(DateTime notificationDate)
            : base()
        {
            this.NotificationDate = notificationDate;
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the <see cref="DateTime" /> that the notification was raised.
        /// </summary>
        public DateTime NotificationDate { get; protected set; }

        /// <summary>
        /// Gets the message describing the raised notification.
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        ///  Gets the title used by the raised notification.
        /// </summary>
        public string Title
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets a value of <see cref="NotificationType"/> to be used by the raised notification.
        /// </summary>
        public NotificationType Type
        {
            get; protected set;
        }

        #endregion

        internal protected void RaiseNotification(NotificationDelegate @delegate, int timeout = 1000)
        {
            if (timeout <= 0)
                throw new ArgumentOutOfRangeException(nameof(timeout));

            @delegate(timeout, this.Title, this.Message, (ToolTipIcon)(int)this.Type);
        }
    }
}