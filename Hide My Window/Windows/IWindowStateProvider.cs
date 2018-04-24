using System;

namespace theDiary.Tools.HideMyWindow
{
    public interface IWindowStateProvider
    {
        #region Event Definitions
        /// <summary>
        /// Defines the event that is raised when the Window State has changed.
        /// </summary>
        event WindowStateChangedEventHandler StateChanged;
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Gets a pointer to the associated Windows <c>Handle</c>.
        /// </summary>
        /// <returns></returns>
        IntPtr GetWindowHandle();

        /// <summary>
        /// Gets a bitwise flag value of <see cref="WindowStates"/> for the associated Window <c>Handle</c>.
        /// </summary>
        /// <returns></returns>
        WindowStates GetState();
        #endregion
    }
}