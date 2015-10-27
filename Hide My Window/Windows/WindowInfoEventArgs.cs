using System;

namespace theDiary.Tools.HideMyWindow
{
    public delegate void ApplicationExited(object sender, WindowInfoEventArgs e);

    public class WindowInfoEventArgs
        : EventArgs
    {
        #region Constructors

        public WindowInfoEventArgs()
        {
        }

        internal WindowInfoEventArgs(WindowInfo window)
        {
            this.Handle = window.Handle;
            this.ProcessId = window.ApplicationId;
            this.State = window.CanShow ? WindowStates.Hidden : WindowStates.Normal;
        }

        #endregion

        #region Properties

        public IntPtr Handle { get; private set; }

        public int ProcessId { get; private set; }

        public WindowStates State { get; private set; }

        #endregion
    }
}