using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace theDiary.Tools.HideMyWindow
{
    public class WindowsStoreItem : IEquatable<WindowsStoreItem>
    {
        #region Public Constructors
        public WindowsStoreItem()
        {
            if (this.Handle == IntPtr.Zero)
                return;

            WindowInfo window = WindowInfo.Find(this.Handle);
        }
        #endregion

        #region Internal Constructors
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

        #region Declarations
        private bool handlersRegistered;

        private WindowStates state;
        #endregion

        #region Properties
        [XmlAttribute]
        public long LastState
        {
            get;
            set;
        }

        [XmlAttribute]
        public long HandleValue
        {
            get;
            set;
        }

        [XmlIgnore]
        public IntPtr Handle
        {
            get
            {
                return new IntPtr(this.HandleValue);
            }
            set
            {
                this.HandleValue = value.ToInt64();
            }
        }

        [XmlIgnore]
        public bool IsHidden
        {
            get
            {
                return this.State.HasFlag(WindowStates.Hidden);
            }
        }

        [XmlIgnore]
        public bool IsPinned
        {
            get
            {
                return this.State.HasFlag(WindowStates.Pinned);
            }
        }

        [XmlIgnore]
        public bool IsPasswordProtected
        {
            get
            {
                return this.State.HasFlag(WindowStates.Protected);
            }
        }

        [XmlAttribute]
        public WindowStates State
        {
            get
            {
                return this.state;
            }
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
        public event EventHandler StateChanged;

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

        #region Interface Implementations
        public bool Equals(WindowsStoreItem other)
        {
            if (other == null)
                return false;

            return this.HandleValue == other.HandleValue;
        }
        #endregion
    }
}