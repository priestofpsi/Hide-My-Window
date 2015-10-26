using System;

namespace theDiary.Tools.HideMyWindow
{
    public delegate void WindowEventHandler(object sender, WindowEventArgs e);

    public class WindowEventArgs
           : EventArgs
    {
        public WindowEventArgs()
            : base()
        {
        }

        public WindowEventArgs(WindowInfo window)
            : this()
        {
            if (window == null)
                throw new ArgumentNullException("window");

            this.Window = window;
        }

        public WindowInfo Window
        {
            get;
            protected set;
        }
    }
}