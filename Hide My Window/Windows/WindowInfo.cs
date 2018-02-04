namespace theDiary.Tools.HideMyWindow
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Automation;
    using System.Windows.Forms;

    public partial class WindowInfo
    {
        #region Constructors

        private WindowInfo(AutomationElement automationElement)
            : this(new IntPtr(automationElement.Current.NativeWindowHandle))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowInfo"/> class, from the pointer specified by <paramref name="wHnd"/>.
        /// </summary>
        /// <param name="wHnd">A pointer to the Window Handle.</param>
        private WindowInfo(IntPtr wHnd)
        {
            this.ApplicationProcess = NativeMethods.GetWindowProcess(wHnd);
            if (this.ApplicationProcess.Id == 0)
                return;

            this.Handle = wHnd;
            this.OriginalState = 0;
            this.AutomationElement = AutomationElement.FromHandle(this.Handle);
            this.Pinned += this.AddAutomationEvents;
            this.Unpinned += this.RemoveAutomationEvents;
            this.ApplicationProcess.Exited += this.ApplicationProcessExited;
            this.OriginalTitle = NativeMethods.GetWindowText(this.Handle);
        }

        #endregion

        #region Declarations

        #region Private Declarations
        private event WindowEventHandler hiddenEventHandler;
        private readonly object syncObject = new object();
        private bool hasAutomationEvents;
        
        private bool isPinned;
        private string title;
        private UnlockWindowDelegate unlockWindowHandler;
        private string originalTitle;
        private Icon originalApplicationIcon;
        #endregion

        #endregion

        #region Properties

        private bool HasIconOverlay
        {
            get
            {
                return this.originalApplicationIcon != null;
            }
        }

        private bool IsHandleFromHideMyWindow
        {
            get
            {
                return NativeMethods.GetWindowProcess(this.Handle).Id == Process.GetCurrentProcess().Id;
            }
        }

        private bool HasHiddenRegistered
        {
            get
            {
                lock(this.syncObject)
                    return this.hiddenEventHandler != null;
            }
        }

        internal string Key
        {
            get
            {
                return this.Handle.ToString();
            }
        }

        protected internal Process ApplicationProcess
        {
            get;
        }

        public AutomationElement AutomationElement
        {
            get;
        }

        /// <summary>
        /// Gets the pointer to the underlying windows form.
        /// </summary>
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
                return NativeMethods.CurrentState(this.Handle);
            }
        }

        /// <summary>
        /// Returns a value indicating if the associated Window can be Hidden.
        /// </summary>
        public bool CanHide
        {
            get
            {
                return (!this.IsHandleFromHideMyWindow
                    && !this.Handle.Equals(GlobalHotKeyManager.MainFormHandle)
                    && this.OriginalState == 0);
            }
        }

        /// <summary>
        /// Returns a value indicating if the associated Window can be Shown.
        /// </summary>
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

        /// <summary>
        /// Gets or sets whether the associated Window is <c>Pinned</c>.
        /// </summary>
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
                if (this.isPinned)
                    this.Pinned?.Invoke(this, new WindowInfoEventArgs(this));
                else
                    this.Unpinned?.Invoke(this, new WindowInfoEventArgs(this));
                Task.Run(() => this.ApplyPinnedSettings());
            }
        }

        public string OriginalTitle
        {
            get
            {
                string returnValue = NativeMethods.GetWindowText(this.Handle);
                if (!this.originalTitle.Equals(returnValue))
                {
                    this.TitleChanging?.Invoke(this, new WindowInfoEventArgs(this));
                    this.originalTitle = returnValue;
                    this.TitleChanged?.Invoke(this, new WindowInfoEventArgs(this));
                }
                return this.originalTitle;
            }
            private set
            {
            }
        }

        public string Title
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.title))
                    return NativeMethods.GetWindowText(this.Handle);
                return this.title;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    value = null;

                if (this.title == value)
                    return;

                this.TitleChanging?.Invoke(this, new WindowInfoEventArgs(this));
                this.title = value;
                this.TitleChanged?.Invoke(this, new WindowInfoEventArgs(this));
            }
        }

        /// <summary>
        ///     Indicates if the <see cref="WindowInfo" /> instance is currently valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return this.ApplicationProcess.Id != 0 && this.Handle != IntPtr.Zero;
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

        #region Public Methods & Functions
        /// <summary>
        /// Locks the specified <c>Window</c>, so that it can't be shown with out confirmation.
        /// </summary>
        /// <param name="handler">The delegate that performs the authentication.</param>
        /// <exception cref="ArgumentNullException">thrown if the <paramref name="handler"/> is <c>Null</c>.</exception>
        public void Lock(UnlockWindowDelegate handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            if (this.unlockWindowHandler == handler)
                return;

            this.Locking?.Invoke(this, new WindowInfoEventArgs(this));
            this.unlockWindowHandler = handler;
            this.Locked?.Invoke(this, new WindowInfoEventArgs(this));
            Task.Run(() => this.ApplyPinnedSettings());
        }

        /// <summary>
        /// Toggles the associated <c>Window</c> to be either <c>Hidden</c> or <c>Not Hidden</c>.
        /// </summary>
        public void ToggleHidden()
        {
            if (this.CanHide)
                this.Hide();
            else
                this.Show();
        }

        /// <summary>
        /// Toggles the associated <c>Window</c> to be either Pinned or Not Pinned.
        /// </summary>
        public void TogglePinned()
        {
            this.IsPinned = !this.IsPinned;
        }

        /// <summary>
        /// Executes the delegate used to unlock a <c>Window</c>, if the <see cref="WindowInfo"/> instance has been marked as locked.
        /// </summary>
        public void Unlock()
        {
            if (this.unlockWindowHandler == null)
                return;

            this.Unlocking?.Invoke(this, new WindowInfoEventArgs(this));
            this.unlockWindowHandler = null;
            this.Unlocked?.Invoke(this, new WindowInfoEventArgs(this));
            Task.Run(() => this.ApplyPinnedSettings());
        }

        /// <summary>
        ///     The method used to hide a Window associated to a <see cref="WindowInfo" /> instance.
        /// </summary>
        /// <returns><c>True</c> if the window was hidden, otherwise <c>False</c>.</returns>
        public bool Hide()
        {
            if (!this.CanHide)
                return false;
            bool returnValue = false;
            try
            {
                this.Hidding?.Invoke(this, new WindowInfoEventArgs(this));
                var hideWindowTask = Task.Run<bool>(() => NativeMethods.HideWindow(this));
                hideWindowTask.Wait();
                returnValue = hideWindowTask.Result;

            }
            catch
            {
                Task.Run(() => NativeMethods.ShowWindow(this)).Wait();
            }
            finally
            {
                if (returnValue)
                    this.hiddenEventHandler?.Invoke(this, new WindowInfoEventArgs(this));
                else
                    this.Error?.Invoke(this, new WindowInfoEventArgs(this));
            }
            return returnValue;
        }

        /// <summary>
        ///     The method used to show a Window associated to a <see cref="WindowInfo" /> instance.
        /// </summary>
        /// <returns><c>True</c> if the window was shown, otherwise <c>False</c>.</returns>
        public bool Show()
        {
            return this.Show(false);
        }

        public override string ToString()
        {
            return this.Title;
        }

        public override bool Equals(object obj)
        {
            if (obj is WindowInfo)
                return ((WindowInfo)obj).Handle.Equals(this.Handle);
            if (obj is IntPtr)
                return ((IntPtr)obj) == this.Handle;

            return false;
        }

        public override int GetHashCode()
        {
            return this.Handle.GetHashCode();
        }
        #endregion

        #region Private Methods & Functions
        private void ApplyPinnedSettings()
        {
            if (Runtime.Instance.Settings.PinnedSettings.AddIconOverlay)
                this.SetWindowIcon();
            if (Runtime.Instance.Settings.PinnedSettings.ModifyWindowTitle)
                this.SetWindowText();
        }

        private void SetWindowText()
        {
            StringBuilder suffix = new StringBuilder();
            if (this.IsPinned)
                suffix.Append("- Pinned");
            if (this.IsPasswordProtected)
                suffix.AppendFormat("{0}Locked", suffix.Length > 0 ? ":" : "- ");
            if (suffix.Length > 0)
            {
                suffix.Append(Runtime.Instance.Settings.PinnedSettings.SufixWindowText ?? string.Empty);
                this.SetWindowText(Runtime.Instance.Settings.PinnedSettings.PrefixWindowText, suffix.ToString());
            }
            else
                this.RestoreWindowText();
        }

        private void SetWindowIcon()
        {
            try
            {
                if (!this.IsPinned
                    && !this.IsPasswordProtected)
                    this.RestoreWindowIcon();
                else
                {
                    if (!this.HasIconOverlay)
                        this.originalApplicationIcon = this.ApplicationIcon;

                    Icon icon = this.ApplicationIcon;

                    if (this.IsPinned)
                        icon = icon.AddOverlay(ActionResource.tack, ImageOverlayPosition.BottomRight, 0.66);

                    if (this.IsPasswordProtected)
                        icon = icon.AddOverlay(ActionResource.lockwindow, ImageOverlayPosition.BottomLeft,
                            new Size(
                                (SystemInformation.FrameBorderSize.Width + SystemInformation.BorderSize.Width) * -1, 0),
                            0.66);

                    this.SetWindowIcon(icon);
                }
            }
            catch
            {
                this.RestoreWindowIcon();
            }
        }

        private void RestoreWindowIcon()
        {
            try
            {
                if (!this.HasIconOverlay)
                    return;
                this.SetWindowIcon(this.originalApplicationIcon);
                this.SetWindowText(this.OriginalTitle);
            }

            finally
            {

            }
        }

        private void ApplicationProcessExited(object sender, EventArgs e)
        {
            this.ApplicationExited?.Invoke(sender, new WindowInfoEventArgs(this));
            this.ApplicationProcess.Exited -= this.ApplicationProcessExited;
        }

        private void RemoveAutomationEvents(object sender, WindowInfoEventArgs e)
        {
            lock (this.syncObject)
            {
                if (!this.hasAutomationEvents
                    && !Runtime.Instance.Settings.PinnedSettings.HideOnMinimize)
                    return;

                Automation.RemoveAutomationPropertyChangedEventHandler(this.AutomationElement, this.HideOnMinimize);
                this.Shown -= this.RestoreOnShow;
                this.ApplicationExited -= this.RemoveAutomationEvents;
                this.hasAutomationEvents = false;
            }
        }

        private void AddAutomationEvents(object sender, WindowInfoEventArgs e)
        {
            lock (this.syncObject)
            {
                if (this.hasAutomationEvents
                    && !Runtime.Instance.Settings.PinnedSettings.HideOnMinimize)
                    return;

                Automation.AddAutomationPropertyChangedEventHandler(this.AutomationElement, TreeScope.Element,
                    this.HideOnMinimize, WindowPattern.WindowVisualStateProperty);
                this.Shown += this.RestoreOnShow;
                this.ApplicationExited += this.RemoveAutomationEvents;
                this.hasAutomationEvents = true;
            }
        }

        private void RestoreOnShow(object sender, WindowInfoEventArgs e)
        {
            WindowPlacement windowplacement = WindowPlacement.Load(this.Handle);
            WindowPattern windowPattern =
                (WindowPattern)this.AutomationElement.GetCurrentPattern(WindowPattern.Pattern);
            WindowVisualState visualState = ((windowplacement.Flags & WindowPlacementFlags.RestoreToMaximize) > 0)
                ? WindowVisualState.Maximized
                : WindowVisualState.Normal;
            windowPattern.SetWindowVisualState(visualState);

            this.Restored?.Invoke(sender, new WindowInfoEventArgs(this));
        }

        private void HideOnMinimize(object sender, AutomationPropertyChangedEventArgs e)
        {
            this.Minimized?.Invoke(sender, new WindowInfoEventArgs(this));

            if ((WindowVisualState)e.NewValue == WindowVisualState.Minimized)
                this.Hide();
        }
        #endregion

        internal void NotifyApplicationExited()
        {
            this.ApplicationExited?.Invoke(this, new WindowInfoEventArgs(this));
        }

        internal bool Show(bool forceShow)
        {
            if (!this.CanShow)
                return false;
            bool returnValue = false;
            try
            {
                this.Showing?.Invoke(this, new WindowInfoEventArgs(this));

                if (forceShow || (this.unlockWindowHandler == null
                    || this.unlockWindowHandler(this)))
                {
                    Task.Run(() => NativeMethods.ShowWindow(this));

                    returnValue = true;
                }
            }
            catch
            {
            }
            finally
            {
                if (returnValue)
                    this.Shown?.Invoke(this, new WindowInfoEventArgs(this));
                else
                    this.Error?.Invoke(this, new WindowInfoEventArgs(this));
            }
            return returnValue;
        }

        internal bool CheckWindowProcess()
        {
            bool returnValue = false;
            try
            {
                returnValue = (this.ApplicationProcess.Id == 0 || this.ApplicationProcess.HasExited);
            }
            catch
            {
                returnValue = true;
            }
            finally
            {
                if (returnValue)
                    this.ApplicationExited?.Invoke(this, new WindowInfoEventArgs(this));
            }
            return !returnValue;
        }
        #endregion

        #region Static Methods & Functions
        /// <summary>
        ///     Gets the window that currently is focused.
        /// </summary>
        /// <returns>A <see cref="WindowInfo" /> instance of the currently focused window.</returns>
        public static WindowInfo CurrentWindow
        {
            get
            {
                return NativeMethods.GetForegroundWindow();
            }
        }

        public static IEnumerable<WindowInfo> All
        {
            get
            {
                return Runtime.Instance.WindowManager.Where(window => window.CanShow);
            }
        }

        public static WindowInfo Last
        {
            get
            {
                if (Runtime.Instance.WindowManager.LastWindow == null)
                    return Runtime.Instance.WindowManager.GetLastWindow();
                return Runtime.Instance.WindowManager.LastWindow;
            }
        }

        public static WindowInfo Find(IntPtr handle)
        {
            WindowInfo returnValue = WindowInfoManager.Find(handle) ?? new WindowInfo(handle);
            if (!returnValue.HasHiddenRegistered)
                returnValue.Hidden += (s, e) => Runtime.Instance.WindowManager.Register(e.Window);

            return returnValue;
        }

        private static void ReturnValue_Hidden(object sender, WindowInfoEventArgs e)
        {

        }

        internal static bool TryFind(WindowsStoreItem storeItem, out WindowInfo windowInfo)
        {
            windowInfo = null;
            if (storeItem != null)
            {
                windowInfo = WindowInfo.Find(storeItem.Handle);
                windowInfo.OriginalState = storeItem.LastState;
                windowInfo.isPinned = storeItem.IsPinned;
            }
            return (windowInfo != null && windowInfo.IsValid);
        }
        #endregion
    }
}