﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace theDiary.Tools.HideMyWindow
{
    public interface IWindowStateProvider
    {
        #region Methods & Functions
        IntPtr GetWindowHandle();

        WindowStates GetState();

        event WindowStateChangedEventHandler StateChanged;
        #endregion
    }

    public delegate void WindowStateChangedEventHandler(IWindowStateProvider provider, WindowStateEventArgs e);

    public class WindowStateEventArgs : EventArgs
    {
        #region Public Constructors
        public WindowStateEventArgs(IWindowStateProvider provider)
        {
            this.Handle = provider.GetWindowHandle();
            this.State = provider.GetState();
        }
        #endregion

        #region Properties
        public IntPtr Handle
        {
            get;
            private set;
        }

        public WindowStates State
        {
            get;
            private set;
        }
        #endregion
    }
}