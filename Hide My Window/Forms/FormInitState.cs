using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    /// <summary>
    /// Provides a list of <see cref="Form"/> initialization states.
    /// </summary>
    public enum FormInitState
    {
        /// <summary>
        /// Indicates that a <see cref="Form"/> instance has not been initialized.
        /// </summary>
        NotInitialized,

        /// <summary>
        /// Indicates that a <see cref="Form"/> instance is curretly been initialized.
        /// </summary>
        Initializing,

        /// <summary>
        /// Indicates that a <see cref="Form"/> instance has been initialized.
        /// </summary>
        Initialized
    }

    public class FormInitializeEventArgs
        : EventArgs
    {
        public FormInitializeEventArgs(FormInitState state)
            : base()
        {
            this.State = state;
        }

        public FormInitState State { get; private set; }
    }
}
