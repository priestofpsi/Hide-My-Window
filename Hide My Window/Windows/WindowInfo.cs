﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow
{
    public class WindowInfo
    {
        #region Constructors
        private WindowInfo(IntPtr wHnd)
        {
            this.Handle = wHnd;
            this.OriginalState = 0;
            this.applicationProcess = ExternalReferences.GetWindowProcess(this.Handle);
        }
        #endregion

        #region Private Declarations
        private string title;
        private readonly object syncObject = new object();
        private volatile Process applicationProcess;
        private UnlockWindowDelegate unlockWindowHandler;
        #endregion

        #region Public Events
        public event ApplicationExited ApplicationExited;

        #endregion

        #region Public Read-Only Properties
        internal string Key
        {
            get
            {
                return this.Handle.ToString();
            }
        }

        public IntPtr Handle
        {
            get; set;
        }

        public long OriginalState
        {
            get; set;
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
                if (this.title == value)
                    return;

                this.title = value;
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
            get;
            set;
        }

        protected Process ApplicationProcess
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

        #region Public Methods & Functions
        public void Lock(UnlockWindowDelegate handler)
        {
            this.unlockWindowHandler = handler;
        }

        public void Unlock()
        {
            this.unlockWindowHandler = null;
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
                return ((WindowInfo)obj).Handle.Equals(this.Handle);
            if (obj.GetType() == typeof(IntPtr))
                return ((IntPtr)obj) == this.Handle;

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
        #endregion

        #region Public Static Methods & Functions
        

        

        public static WindowInfo Find(IntPtr handle)
        {
            return Runtime.Instance.FindWindow(handle) ?? new WindowInfo(handle);
        }
        #endregion
    }
}
