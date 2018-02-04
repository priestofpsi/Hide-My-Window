namespace theDiary.Tools.HideMyWindow
{
    using System;

    public interface IWindowStateProvider
    {
        #region Event Definitions
        event WindowStateChangedEventHandler StateChanged;
        #endregion

        #region Methods & Functions

        IntPtr GetWindowHandle();

        WindowStates GetState();
        #endregion
    }
}