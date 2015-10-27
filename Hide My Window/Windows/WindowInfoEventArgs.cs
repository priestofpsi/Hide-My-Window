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
            this.State = window.CanShow ? WindowStates.Hidden : WindowStates.Normal;
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

        public WindowStates State
        {
            get;
            private set;
        }
    }
}
