using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Automation;

namespace theDiary.Tools.HideMyWindow
{
    public partial class WindowInfo
    {
        #region Constructors

        private WindowInfo(AutomationElement automationElement)
            : this(new IntPtr(automationElement.Current.NativeWindowHandle))
        {
        }

        private WindowInfo(IntPtr wHnd)
        {

            this.applicationProcess = ExternalReferences.GetWindowProcess(wHnd);
            if (this.applicationProcess.Id == 0)
                return;

            this.Handle = wHnd;
            this.OriginalState = 0;
            this.automationElement = AutomationElement.FromHandle(this.Handle);
            this.Pinned += this.AddAutomationEvents;
            this.Unpinned += this.RemoveAutomationEvents;
            this.applicationProcess.Exited += this.ApplicationProcessExited;
        }

        #endregion

        #region Declarations

        private readonly object syncObject = new object();
        private readonly Process applicationProcess;
        private readonly AutomationElement automationElement;
        private bool hasAutomationEvents = false;
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
            get
            {
                //if (this.applicationProcess == null)
                //    this.LoadApplicationProcess();

                return this.applicationProcess;
            }
        }

        public AutomationElement AutomationElement
        {
            get
            {
                return this.automationElement;
            }
        }

        public IntPtr Handle { get; internal set; }

        public long OriginalState { get; internal set; }

        public IntPtr CurrentState
        {
            get { return ExternalReferences.CurrentState(this.Handle); }
        }

        public bool CanHide
        {
            get
            {
                return (!this.Handle.Equals(Program.MainForm.Handle)
                              && this.OriginalState == 0);
            }
        }

        public bool CanShow
        {
            get { return this.OriginalState != 0; }
        }

        public bool IsPasswordProtected
        {
            get { return this.unlockWindowHandler != null; }
        }

        public bool IsPinned
        {
            get { return this.isPinned; }
            set
            {
                if (this.isPinned == value)
                    return;

                this.isPinned = value;
                if (this.isPinned)
                {
                    if (this.Pinned != null)
                        this.Pinned(this, new WindowInfoEventArgs(this));
                }
                else
                {
                    if (this.Unpinned != null)
                        this.Unpinned(this, new WindowInfoEventArgs(this));
                }
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
                if (this.TitleChanging != null)
                    this.TitleChanging(this, new WindowInfoEventArgs(this));

                this.title = value;
                if (this.TitleChanged != null)
                    this.TitleChanged(this, new WindowInfoEventArgs(this));
            }
        }
        public bool IsValid
        {
            get { return this.applicationProcess.Id != 0 && this.Handle != IntPtr.Zero; }
        }


        public string ApplicationPathName
        {
            get { return this.ApplicationProcess.MainModule.FileName; }
        }

        public int ApplicationId
        {
            get { return this.ApplicationProcess.Id; }
        }

        public Icon ApplicationIcon
        {
            get { return this.ApplicationProcess.GetApplicationIcon(); }
        }

        #endregion

        #region Methods & Functions
        #region Public Methods & Functions
        public void Lock(UnlockWindowDelegate handler)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            if (this.unlockWindowHandler == handler)
                return;

            if (this.Locking != null)
                this.Locking(this, new WindowInfoEventArgs(this));

            this.unlockWindowHandler = handler;

            if (this.Locked != null)
                this.Locked(this, new WindowInfoEventArgs(this));
        }

        public void Toggle()
        {
            if (this.CanHide)
            {
                this.Hide();
            }
            else
            {
                this.Show();
            }
        }

        public void Unlock()
        {
            if (this.unlockWindowHandler == null)
                return;

            if (this.Unlocking != null)
                this.Unlocking(this, new WindowInfoEventArgs(this));

            this.unlockWindowHandler = null;

            if (this.Unlocked != null)
                this.Unlocked(this, new WindowInfoEventArgs(this));
        }

        /// <summary>
        /// The method used to hide a Window associated to a <see cref="WindowInfo"/> instance.
        /// </summary>
        /// <returns><c>True</c> if the window was hidden, otherwise <c>False</c>.</returns>
        public bool Hide()
        {
            if (!this.CanHide)
                return false;

            if (this.Hidding != null)
                this.Hidding(this, new WindowInfoEventArgs(this));

            bool returnValue = ExternalReferences.HideWindow(this);
            if (returnValue)
            {
                if (this.Hidden != null)
                    this.Hidden(this, new WindowInfoEventArgs(this));
            }
            return returnValue;
        }

        /// <summary>
        /// The method used to show a Window associated to a <see cref="WindowInfo"/> instance.
        /// </summary>
        /// <returns><c>True</c> if the window was shown, otherwise <c>False</c>.</returns>
        public bool Show()
        {
            if (!this.CanShow)
                return false;

            if (this.Showing != null)
                this.Showing(this, new WindowInfoEventArgs(this));

            bool returnValue = false;
            if (this.unlockWindowHandler == null
                || this.unlockWindowHandler(this))
            {
                ExternalReferences.ShowWindow(this);
                if (this.Shown != null)
                    this.Shown(this, new WindowInfoEventArgs(this));
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
            if (obj.GetType() == typeof (IntPtr))
                return ((IntPtr) obj) == this.Handle;

            return false;
        }

        public override int GetHashCode()
        {
            return this.Handle.GetHashCode();
        }
        #endregion

#region Private Methods & Functions
        private void LoadApplicationProcess()
        {
            /*lock (this.syncObject)
            {
                if (this.applicationProcess != null)
                    return;
                this.applicationProcess = ExternalReferences.GetWindowProcess(this.Handle);
                if (this.applicationProcess != null
                    && !this.applicationProcess.HasExited)
                    this.applicationProcess.Exited += this.ApplicationProcessExited;
            }*/
        }

        private void ApplicationProcessExited(object sender, EventArgs e)
        {
            if (this.ApplicationExited != null)
                this.ApplicationExited(sender, new WindowInfoEventArgs(this));
            
            this.ApplicationProcess.Exited -= this.ApplicationProcessExited;
        }

        private void RemoveAutomationEvents(object sender, WindowInfoEventArgs e)
        {
            lock(this.syncObject)
            {
                if (!this.hasAutomationEvents && !Runtime.Instance.Settings.PinnedSettings.HideOnMinimize)
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
                if (this.hasAutomationEvents && !Runtime.Instance.Settings.PinnedSettings.HideOnMinimize)
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

            if (this.Restored != null)
                this.Restored(sender, new WindowInfoEventArgs(this));
        }

        private void HideOnMinimize(object sender, AutomationPropertyChangedEventArgs e)
        {
            if (this.Minimized != null)
                this.Minimized(sender, new WindowInfoEventArgs(this));

            if ((WindowVisualState) e.NewValue == WindowVisualState.Minimized)
                this.Hide();
        }
        #endregion

        internal void NotifyApplicationExited()
        {
            if (this.ApplicationExited != null)
                this.ApplicationExited(this, new WindowInfoEventArgs(this));
        }

        internal bool CheckWindowProcess()
        {
            bool returnValue = true;
            try
            {
                if (this.ApplicationProcess.Id == 0
                    || this.ApplicationProcess.HasExited)
                {
                    returnValue = false;
                }
            }
            catch
            {
                returnValue = false;
            }
            finally
            {
                if (!returnValue)
                    this.NotifyApplicationExited();
            }
            return returnValue;
        }

        private bool hasHiddenRegistered;

        #region Public Static Methods & Functions
        public static WindowInfo Find(IntPtr handle)
        {
            WindowInfo returnValue = WindowInfoManager.Find(handle) ?? new WindowInfo(handle);
            if (!returnValue.hasHiddenRegistered)
            {
                returnValue.Hidden += ReturnValue_Hidden;
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

        /// <summary>
        /// Gets the window that currently is focused.
        /// </summary>
        /// <returns>A <see cref="WindowInfo"/> instance of the currently focused window.</returns>
        public static WindowInfo CurrentWindow()
        {
            return WindowInfo.Find(ExternalReferences.GetForegroundWindow());
        }
        #endregion
        #endregion
    }
}