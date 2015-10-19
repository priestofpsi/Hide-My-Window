using System;

namespace theDiary.Tools.HideMyWindow
{
    public delegate void ApplicationExited(object sender, WindowInfoEventArgs e);

    public class WindowInfoEventArgs
        : EventArgs
    {
        public WindowInfoEventArgs()
            : base()
        {
        }

        internal WindowInfoEventArgs(WindowInfo window)
        {
            this.Handle = window.Handle;
            this.ProcessId = window.ApplicationId;
            this.State = window.CanShow ? WindowState.Hidden : WindowState.Normal;
        }

        public IntPtr Handle
        {
            get;
            private set;
        }

        public int ProcessId
        {
            get;
            private set;
        }

        public WindowState State
        {
            get;
            private set;
        }
    }
}