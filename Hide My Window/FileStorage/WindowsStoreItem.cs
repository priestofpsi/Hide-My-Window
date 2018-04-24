namespace theDiary.Tools.HideMyWindow
{
    using System;
    using System.Xml.Serialization;

    public class WindowsStoreItem 
        : IEquatable<WindowsStoreItem>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsStoreItem"/> class.
        /// </summary>
        public WindowsStoreItem()
        {
            if (this.Handle == IntPtr.Zero)
                return;

            WindowInfo window = WindowInfo.Find(this.Handle);
        }

        /// <summary>
        /// Initializes a new instance of the<see cref="WindowsStoreItem"/> class from the specified <paramref name="window"/> instance.
        /// </summary>
        /// <param name="window">An instance of <see cref="WindowInfo"/> used to initialize the <see cref="WindowsStoreItem"/>.</param>
        internal WindowsStoreItem(WindowInfo window)
        {
            this.Handle = window.Handle;
            this.state = window.CanShow ? WindowStates.Hidden : WindowStates.Normal;
            if (window.IsPasswordProtected)
                this.state &= WindowStates.Protected;
            if (window.IsPinned)
                this.state &= WindowStates.Pinned;
            this.LastState = window.CurrentState.ToInt64();
            this.RegisterHandlers(window);
        }

        #endregion

        #region Event Declarations
        /// <summary>
        /// The event that is raised when the <c>State</c> of the <see cref="WindowsStoreItem"/> changes.
        /// </summary>
        public event EventHandler StateChanged;
        #endregion

        #region Private Declarations

        private bool handlersRegistered;

        private WindowStates state;

        #endregion

        #region Properties

        [XmlAttribute]
        public long LastState { get; set; }

        [XmlAttribute]
        public long HandleValue { get; set; }

        [XmlIgnore]
        public IntPtr Handle
        {
            get { return new IntPtr(this.HandleValue); }
            set { this.HandleValue = value.ToInt64(); }
        }

        /// <summary>
        /// Indicates if the stored <see cref="WindowInfo"/> in the <see cref="WindowsStoreItem"/> is <c>Hidden</c> or not.
        /// </summary>
        [XmlIgnore]
        public bool IsHidden
        {
            get { return this.State.HasFlag(WindowStates.Hidden); }
        }

        /// <summary>
        /// Indicates if the stored <see cref="WindowInfo"/> in the <see cref="WindowsStoreItem"/> is <c>Pinned</c> or not.
        /// </summary>
        [XmlIgnore]
        public bool IsPinned
        {
            get { return this.State.HasFlag(WindowStates.Pinned); }
        }

        /// <summary>
        ///Indicates if the stored <see cref="WindowInfo"/> in the <see cref="WindowsStoreItem"/> is <c>Password Protected</c> or not.
        /// </summary>
        [XmlIgnore]
        public bool IsPasswordProtected
        {
            get { return this.State.HasFlag(WindowStates.Protected); }
        }

        /// <summary>
        /// Gets or sets a flagged value of <see cref="WindowStates"/> indicating the current state of the <see cref="WindowInfo"/> item stored in the <see cref="WindowsStoreItem"/>.
        /// </summary>
        [XmlAttribute]
        public WindowStates State
        {
            get { return this.state; }
            set
            {
                if (this.state == value)
                    return;
                this.state = value;
                if (this.StateChanged != null)
                    this.StateChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Methods & Functions

        public bool Equals(WindowsStoreItem other)
        {
            if (other == null)
                return false;

            return this.HandleValue == other.HandleValue;
        }

        

        public void RegisterHandlers(WindowInfo window = null)
        {
            if (window == null)
                window = WindowInfo.Find(this.Handle);

            if (!Runtime.Instance.WindowManager.Exists(window)
                || this.handlersRegistered)
                return;

            window.OriginalState = this.LastState;
            window.IsPinned = this.IsPinned;
            window.Hidden += (s, e) => { this.State |= WindowStates.Hidden; };
            window.Shown += (s, e) => { this.State &= ~WindowStates.Hidden; };
            window.Pinned += (s, e) => { this.State |= WindowStates.Pinned; };
            window.Unpinned += (s, e) => { this.State &= ~WindowStates.Pinned; };
            window.Locked += (s, e) => { this.State |= WindowStates.Protected; };
            window.Unlocked += (s, e) => { this.State &= ~WindowStates.Protected; };
            this.StateChanged += (s, e) => { HiddenWindowStore.Save(Runtime.Instance.Store); };
            this.handlersRegistered = true;
        }

        #endregion
    }
}