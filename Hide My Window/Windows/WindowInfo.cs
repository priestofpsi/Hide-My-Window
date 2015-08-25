using System;
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
        public WindowInfo(IntPtr wHnd)
        {
            this.Handle = wHnd;
            this.OriginalState = IntPtr.Zero;
            this.applicationProcess = ExternalReferences.GetWindowProcess(this.Handle);
        }
        #endregion

        #region Private Declarations
        private readonly object syncObject = new object();
        private volatile Process applicationProcess;
        #endregion

        #region Public Events
        public event ApplicationExited ApplicationExited;
        
        #endregion
        #region Public Read-Only Properties
        public IntPtr Handle
        {
            get; set;
        }

        public IntPtr OriginalState
        {
            get; set;
        }

        public bool CanHide
        {
            get
            {
                return this.OriginalState == IntPtr.Zero;
            }
        }

        public string Title
        {
            get
            {
                return ExternalReferences.GetWindowText(this.Handle);
            }
        }

        public bool CanShow
        {
            get
            {
                return this.OriginalState != IntPtr.Zero;
            }
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

        private WindowListViewItem listViewItem;
        internal WindowListViewItem GetListViewItem()
        {
            return this.listViewItem;
        }
        internal void SetListViewItem(WindowListViewItem listViewItem)
        {
            this.listViewItem = listViewItem;
        }
    }
}
