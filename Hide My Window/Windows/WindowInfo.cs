using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Automation;

namespace theDiary.Tools.HideMyWindow
{
    public class WindowInfo
    {
        #region Constructors
        private WindowInfo(AutomationElement automationElement)
            : this(new IntPtr(automationElement.Current.NativeWindowHandle))
        {
            this.automationElement = automationElement;
        }

        private WindowInfo(IntPtr wHnd)
        {
            this.Handle = wHnd;
            this.OriginalState = 0;
            this.applicationProcess = ExternalReferences.GetWindowProcess(this.Handle);
        }

        #endregion

        #region Declarations

        private readonly object syncObject = new object();
        private volatile Process applicationProcess;
        private bool isPinned;

        private string title;
        private UnlockWindowDelegate unlockWindowHandler;
        private AutomationElement automationElement = null;

        #endregion

        #region Properties

        public AutomationElement AutomationElement
        {
            get
            {
                if (this.automationElement == null)
                    this.automationElement = AutomationElement.FromHandle(this.Handle);

                return this.automationElement;
            }
        }

        internal string Key
        {
            get
            {
                return this.Handle.ToString();
            }
        }

        public IntPtr Handle
        {
            get; internal set;
        }

        public long OriginalState
        {
            get; internal set;
        }

        public IntPtr CurrentState
        {
            get
            {
                return ExternalReferences.CurrentState(this.Handle);
            }
        }

        public bool CanHide
        {
            get
            {
                return this.OriginalState == 0;
            }
        }

        public bool CanShow
        {
            get
            {
                return this.OriginalState != 0;
            }
        }

        public bool IsPasswordProtected
        {
            get
            {
                return this.unlockWindowHandler != null;
            }
        }

        public bool IsPinned
        {
            get
            {
                return this.isPinned;
            }
            set
            {
                if (this.isPinned == value)
                    return;

                this.isPinned = value;
                if (this.isPinned && this.Pinned != null)
                    this.Pinned(this, new WindowEventArgs(this));
                else if (!this.isPinned && this.Unpinned != null)
                    this.Unpinned(this, new WindowEventArgs(this));
            }
        }

        public string Title
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.title))
                    return ExternalReferences.GetWindowText(this.Handle);

                return this.title;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    value = null;

                if (this.title == value)
                    return;

                this.title = value;
                if (this.TitleChanged != null)
                    this.TitleChanged(this, new WindowEventArgs(this));
            }
        }

        protected internal Process ApplicationProcess
        {
            get
            {
                if (this.applicationProcess == null)
                    this.LoadApplicationProcess();

                return this.applicationProcess;
            }
        }

        public string ApplicationPathName
        {
            get
            {
                return this.ApplicationProcess.MainModule.FileName;
            }
        }

        public int ApplicationId
        {
            get
            {
                return this.ApplicationProcess.Id;
            }
        }

        public Icon ApplicationIcon
        {
            get
            {
                return this.ApplicationProcess.GetApplicationIcon();
            }
        }

        #endregion

        #region Methods & Functions

        /// <summary>
        ///     The event that is raised when the underlying application for a <see cref="WindowInfo" />
        ///     instance is exited, or terminated.
        /// </summary>
        public event ApplicationExited ApplicationExited;

        /// <summary>
        ///     The event that is raised when a <see cref="WindowInfo" /> instance is flagged as pinned.
        /// </summary>
        public event WindowEventHandler Pinned;

        /// <summary>
        ///     The event that is raised when a <see cref="WindowInfo" /> instance is flagged as unpinned.
        /// </summary>
        public event WindowEventHandler Unpinned;

        /// <summary>
        ///     The event that is raised when a <see cref="WindowInfo" /> instance is flagged as protected.
        /// </summary>
        public event WindowEventHandler Protected;

        /// <summary>
        ///     The event that is raised when a <see cref="WindowInfo" /> instance is flagged as been unprotected.
        /// </summary>
        public event WindowEventHandler Unprotected;

        /// <summary>
        ///     The event that is raised when the title for a <see cref="WindowInfo" /> instance is changed.
        /// </summary>
        public event WindowEventHandler TitleChanged;

        public void Lock(UnlockWindowDelegate handler)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            if (this.unlockWindowHandler == handler)
                return;

            this.unlockWindowHandler = handler;
            if (this.Protected != null)
                this.Protected(this, new WindowEventArgs(this));
        }

        public void Unlock()
        {
            if (this.unlockWindowHandler == null)
                return;

            this.unlockWindowHandler = null;
            if (this.Unprotected != null)
                this.Unprotected(this, new WindowEventArgs(this));
        }

        public bool Hide()
        {
            if (!this.CanHide)
                return false;

            return ExternalReferences.HideWindow(this);
        }

        public void Show()
        {
            if (!this.CanShow)
                return;

            if (this.unlockWindowHandler == null
                || this.unlockWindowHandler(this))
                ExternalReferences.ShowWindow(this);
        }

        public override string ToString()
        {
            return this.Title;
        }

        public override bool Equals(object obj)
        {
            if (obj is WindowInfo)
                return ((WindowInfo) obj).Handle.Equals(this.Handle);
            if (obj.GetType() == typeof(IntPtr))
                return ((IntPtr) obj) == this.Handle;

            return false;
        }

        public override int GetHashCode()
        {
            return this.Handle.GetHashCode();
        }

        private void LoadApplicationProcess()
        {
            lock (this.syncObject)
            {
                if (this.applicationProcess != null)
                    return;
                this.applicationProcess = ExternalReferences.GetWindowProcess(this.Handle);
                if (this.applicationProcess != null
                    && !this.applicationProcess.HasExited)
                    this.applicationProcess.Exited += this.applicationProcess_Exited;
            }
        }

        private void applicationProcess_Exited(object sender, EventArgs e)
        {
            if (this.ApplicationExited != null)
                this.ApplicationExited(this, new WindowInfoEventArgs(this));

            this.ApplicationProcess.Exited -= this.applicationProcess_Exited;
        }

        internal void NotifyApplicationExited()
        {
            if (this.ApplicationExited != null)
                this.ApplicationExited(this, new WindowInfoEventArgs(this));
        }

        public static WindowInfo Find(long handle)
        {
            return WindowInfo.FindByHandle(new IntPtr(handle));
        }

        public static WindowInfo FindByHandle(IntPtr handle)
        {
            return Runtime.Instance.FindWindow(handle) ?? new WindowInfo(handle);
        }

        #endregion
    }
}