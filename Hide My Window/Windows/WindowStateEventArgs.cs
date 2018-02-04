using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow
{
    public class WindowStateEventArgs 
        : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowStateEventArgs"/> class from the specified <paramref name="provider"/>.
        /// </summary>
        /// <param name="provider">An instance of the <see cref="IWindowStateProvider"/>.</param>
        public WindowStateEventArgs(IWindowStateProvider provider)
        {
            this.Handle = provider.GetWindowHandle();
            this.State = provider.GetState();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the handle provided by the <see cref="IWindowStateProvider"/>.
        /// </summary>
        public IntPtr Handle { get; private set; }

        /// <summary>
        /// Gets a value of <see cref="WindowStates"/> from the <see cref="IWindowStateProvider"/>.
        /// </summary>
        public WindowStates State { get; private set; }

        #endregion
    }
}
