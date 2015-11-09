using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    public partial class WindowInfo
    {
        #region Private Constructors
        private WindowInfo(AutomationElement automationElement)
            : this(new IntPtr(automationElement.Current.NativeWindowHandle)) {}

        private WindowInfo(IntPtr wHnd)
        {
            this.ApplicationProcess = ExternalReferences.GetWindowProcess(wHnd);
            if (this.ApplicationProcess.Id == 0)
                return;

            this.Handle = wHnd;
            this.OriginalState = 0;
            this.AutomationElement = AutomationElement.FromHandle(this.Handle);
            this.Pinned += this.AddAutomationEvents;
            this.Unpinned += this.RemoveAutomationEvents;
            this.ApplicationProcess.Exited += this.ApplicationProcessExited;
            this.OriginalTitle = ExternalReferences.GetWindowText(this.Handle);
        }
        #endregion

        #region Declarations
        private readonly object syncObject = new object();
        private bool hasAutomationEvents;
        private bool hasHiddenRegistered;

        private bool hasIconOverlay;
        private bool isPinned;
        private string title;
        private UnlockWindowDelegate unlockWindowHandler;
        #endregion

        #region Properties
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

        public IntPtr Handle
        {
            get;
            internal set;
        }

        public long OriginalState
        {
            get;
            internal set;
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
                return (!this.Handle.Equals(Program.MainForm.Handle) && this.OriginalState == 0);
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
                if (this.isPinned)
                    this.Pinned?.Invoke(this, new WindowInfoEventArgs(this));
                else
                    this.Unpinned?.Invoke(this, new WindowInfoEventArgs(this));
                Task.Run(() => this.ApplyPinnedSettings());
            }
        }

        public string OriginalTitle
        {
            get;
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

        /// <summary>
        ///     Gets the window that currently is focused.
        /// </summary>
        /// <returns>A <see cref="WindowInfo" /> instance of the currently focused window.</returns>
        public static WindowInfo CurrentWindow
        {
            get
            {
                return WindowInfo.Find(ExternalReferences.GetForegroundWindow());
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
        #endregion

        #region Methods & Functions
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
                    this.hasIconOverlay = false;
                    Icon icon = this.ApplicationIcon;

                    if (this.IsPinned)
                    {
                        icon = icon.AddOverlay(ActionResource.tack, ImageOverlayPosition.BottomRight, 0.66);
                        this.hasIconOverlay = true;
                    }
                    if (this.IsPasswordProtected)
                    {
                        icon = icon.AddOverlay(ActionResource.lockwindow, ImageOverlayPosition.BottomLeft,
                            new Size(
                                (SystemInformation.FrameBorderSize.Width + SystemInformation.BorderSize.Width) * -1, 0),
                            0.66);
                        this.hasIconOverlay = true;
                    }

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
                this.SetWindowIcon(this.ApplicationIcon);
                this.SetWindowText(this.OriginalTitle);
            }
            finally
            {
                this.hasIconOverlay = false;
            }
        }

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

        public void Toggle()
        {
            if (this.CanHide)
                this.Hide();
            else
                this.Show();
        }

        public void Unlock()
        {
            if (this.unlockWindowHandler == null)
                return;

            this.Unlocking?.Invoke(this, new WindowInfoEventArgs(this));
            this.unlockWindowHandler = null;
            this.Unlocked?.Invoke(this, new WindowInfoEventArgs(this));
            Task.Run(() => this.ApplyPinnedSettings());
        }

        private void ApplyPinnedSettings()
        {
            if (Runtime.Instance.Settings.PinnedSettings.AddIconOverlay)
                this.SetWindowIcon();
            if (Runtime.Instance.Settings.PinnedSettings.ModifyWindowTitle)
                this.SetWindowText();
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
                Task.Run(() => returnValue = ExternalReferences.HideWindow(this)).Wait();

                if (returnValue)
                    this.Hidden?.Invoke(this, new WindowInfoEventArgs(this));
            }
            catch
            {
                Task.Run(() => ExternalReferences.ShowWindow(this));
            }
            return returnValue;
        }

        /// <summary>
        ///     The method used to show a Window associated to a <see cref="WindowInfo" /> instance.
        /// </summary>
        /// <returns><c>True</c> if the window was shown, otherwise <c>False</c>.</returns>
        public bool Show()
        {
            if (!this.CanShow)
                return false;

            this.Showing?.Invoke(this, new WindowInfoEventArgs(this));
            bool returnValue = false;
            if (this.unlockWindowHandler == null
                || this.unlockWindowHandler(this))
            {
                Task.Run(() => ExternalReferences.ShowWindow(this));
                this.Shown?.Invoke(this, new WindowInfoEventArgs(this));
                returnValue = true;
            }
            return returnValue;
        }

        public override string ToString()
        {
            return this.Title;
        }

        public override bool Equals(object obj)
        {
            if (obj is WindowInfo)
                return ((WindowInfo) obj).Handle.Equals(this.Handle);
            if (obj is IntPtr)
                return ((IntPtr) obj) == this.Handle;

            return false;
        }

        public override int GetHashCode()
        {
            return this.Handle.GetHashCode();
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
            WindowPlacement windowplacement = new WindowPlacement();
            ExternalReferences.GetWindowPlacement(this.Handle, ref windowplacement);
            WindowPattern windowPattern =
                (WindowPattern) this.AutomationElement.GetCurrentPattern(WindowPattern.Pattern);
            WindowVisualState visualState = ((windowplacement.flags & (int) WindowPlacementFlags.RestoreToMaximize) > 0)
                ? WindowVisualState.Maximized
                : WindowVisualState.Normal;
            windowPattern.SetWindowVisualState(visualState);

            this.Restored?.Invoke(sender, new WindowInfoEventArgs(this));
        }

        private void HideOnMinimize(object sender, AutomationPropertyChangedEventArgs e)
        {
            this.Minimized?.Invoke(sender, new WindowInfoEventArgs(this));

            if ((WindowVisualState) e.NewValue == WindowVisualState.Minimized)
                this.Hide();
        }

        internal void NotifyApplicationExited()
        {
            this.ApplicationExited?.Invoke(this, new WindowInfoEventArgs(this));
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

        public static WindowInfo Find(IntPtr handle)
        {
            WindowInfo returnValue = WindowInfoManager.Find(handle) ?? new WindowInfo(handle);
            if (!returnValue.hasHiddenRegistered)
            {
                returnValue.Hidden += WindowInfo.ReturnValue_Hidden;
                returnValue.hasHiddenRegistered = true;
            }
            return returnValue;
        }

        private static void ReturnValue_Hidden(object sender, WindowInfoEventArgs e)
        {
            Runtime.Instance.WindowManager.Register(e.Window);
        }

        public static WindowInfo Find(WindowsStoreItem storeItem)
        {
            WindowInfo returnValue = WindowInfo.Find(storeItem.Handle);

            returnValue.OriginalState = storeItem.LastState;
            returnValue.isPinned = storeItem.IsPinned;

            return returnValue;
        }
        #endregion
    }
}