using System;

namespace theDiary.Tools.HideMyWindow
{
    public delegate void WindowEventHandler(object sender, WindowEventArgs e);

    public class WindowEventArgs
        : EventArgs
    {
        #region Constructors

        public WindowEventArgs()
        {
        }

        public WindowEventArgs(WindowInfo window)
            : this()
        {
            if (window == null)
                throw new ArgumentNullException("window");

            this.Window = window;
        }

        #endregion

        #region Properties

        public WindowInfo Window { get; protected set; }

        #endregion
    }
}