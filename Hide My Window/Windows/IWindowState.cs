using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow
{
    public interface IWindowStateProvider
    {
        IntPtr GetWindowHandle();

        WindowStates GetState();

        event WindowStateChangedEventHandler StateChanged;
    }

    public delegate void WindowStateChangedEventHandler(IWindowStateProvider provider, WindowStateEventArgs e);

    public class WindowStateEventArgs
        : EventArgs
    {
        public WindowStateEventArgs(IWindowStateProvider provider)
            : base()
        {
            this.Handle = provider.GetWindowHandle();
            this.State = provider.GetState();
        }
        public IntPtr Handle
        {
            get; private set;
        }
        public WindowStates State
        {
            get;
            private set;
        }
    }

}
