using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace theDiary.Tools.HideMyWindow
{
    public partial class WindowInfo
    {
        #region Methods & Functions
        /// <summary>
        ///     The event that is raised when a Window associated to a <c>Pinned</c> <see cref="WindowInfo" /> instance has been
        ///     minimized.
        /// </summary>
        public event WindowEventHandler Minimized;

        /// <summary>
        ///     The event that is raised when a Window associated to a <c>Pinned</c> <see cref="WindowInfo" /> instance has been
        ///     restored from a minimized hidden state.
        /// </summary>
        public event WindowEventHandler Restored;

        /// <summary>
        ///     The event that is raised when the underlying <see cref="Process" /> for a <see cref="WindowInfo" />
        ///     instance is exited, or terminated.
        /// </summary>
        public event ApplicationExitedHandler ApplicationExited;

        /// <summary>
        ///     The event that is raised when a <see cref="WindowInfo" /> instance is flagged as pinned.
        /// </summary>
        public event WindowEventHandler Pinned;

        /// <summary>
        ///     The event that is raised when a <see cref="WindowInfo" /> instance is flagged as unpinned.
        /// </summary>
        public event WindowEventHandler Unpinned;

        /// <summary>
        ///     The event that is raised when a <see cref="WindowInfo" /> instance is applying the protected flagged.
        /// </summary>
        public event WindowEventHandler Locking;

        /// <summary>
        ///     The event that is raised when a <see cref="WindowInfo" /> instance is flagged as protected.
        /// </summary>
        public event WindowEventHandler Locked;

        /// <summary>
        ///     The event that is raised when a <see cref="WindowInfo" /> instance is removing the protected flag.
        /// </summary>
        public event WindowEventHandler Unlocking;

        /// <summary>
        ///     The event that is raised when a <see cref="WindowInfo" /> instance is flagged as been unprotected.
        /// </summary>
        public event WindowEventHandler Unlocked;

        /// <summary>
        ///     The event that is raised when the title for a <see cref="WindowInfo" /> instance is changed.
        /// </summary>
        public event WindowEventHandler TitleChanging;

        /// <summary>
        ///     The event that is raised when the title for a <see cref="WindowInfo" /> instance is changed.
        /// </summary>
        public event WindowEventHandler TitleChanged;

        /// <summary>
        ///     The event that is raised when the Window associated with a <see cref="WindowInfo" /> instance is about to hide.
        /// </summary>
        public event WindowEventHandler Hidding;

        /// <summary>
        ///     The event that is raised when the Window associated with a <see cref="WindowInfo" /> instance has been hidden.
        /// </summary>
        public event WindowEventHandler Hidden;

        /// <summary>
        ///     The event that is raised when the Window associated with a <see cref="WindowInfo" /> instance is about to show.
        /// </summary>
        public event WindowEventHandler Showing;

        /// <summary>
        ///     The event that is raised when the Window associated with a <see cref="WindowInfo" /> instance has been shown.
        /// </summary>
        public event WindowEventHandler Shown;
        #endregion
    }
}