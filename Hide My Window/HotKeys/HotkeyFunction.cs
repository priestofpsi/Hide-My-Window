namespace theDiary.Tools.HideMyWindow
{
    /// <summary>
    ///     The hot key functions that are natively support by the application.
    /// </summary>
    public enum HotKeyFunction
    {
        None,

        /// <summary>
        ///     The function used to hide the window currently focused.
        /// </summary>
        HideCurrentWindow,

        /// <summary>
        ///     The function used to toggle the last hidden window.
        /// </summary>
        ToggleLastWindow,

        /// <summary>
        ///     The function used to un-hide the last hidden window.
        /// </summary>
        UnhideLastWindow,

        /// <summary>
        ///     The function used to un-hide all hidden windows.
        /// </summary>
        UnhideAllWindows
    }
}