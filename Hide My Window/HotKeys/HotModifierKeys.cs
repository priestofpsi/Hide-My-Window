using System;
namespace theDiary.Tools.HideMyWindow
{
    [Flags]
    public enum HotModifierKeys : uint
    {
        /// <summary>
        /// Indicates that no modifier is required for the <see cref="HotKey"/>.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates that the ALT key is associated to the <see cref="HotKey"/>.
        /// </summary>
        Alt = 1,

        /// <summary>
        /// Indicates that the CTRL key is associated to the <see cref="HotKey"/>.
        /// </summary>
        Control = 2,

        /// <summary>
        /// Indicates that the SHIFT key is associated to the <see cref="HotKey"/>.
        /// </summary>
        Shift = 4,

        /// <summary>
        /// Indicates that the WINDOWS key is associated to the <see cref="HotKey"/>.
        /// </summary>
        Win = 8,

        All = Alt | Control | Shift | Win
    }
}