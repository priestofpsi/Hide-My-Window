﻿namespace theDiary.Tools.HideMyWindow
{
    using System;

    public class WindowInfoEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="WindowInfoEventArgs" /> class.
        /// </summary>
        public WindowInfoEventArgs()
        {
        }

        internal WindowInfoEventArgs(WindowInfo window)
        {
            this.Window = window;
            this.State = window.CanShow ? WindowStates.Hidden : WindowStates.Normal;
            if (window.IsPasswordProtected)
                this.State |= WindowStates.Protected;
            if (window.IsPinned)
                this.State |= WindowStates.Pinned;
        }

        #endregion

        #region Properties

        public IntPtr Handle
        {
            get { return this.Window.Handle; }
        }

        public int ProcessId
        {
            get { return this.Window.ApplicationId; }
        }

        public WindowStates State { get; }

        public WindowInfo Window { get; }

        #endregion
    }
}