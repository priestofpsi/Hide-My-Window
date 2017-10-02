using System;

namespace theDiary.Tools.HideMyWindow
{
    /// <summary>
    /// The flags that control the position of the minimized window and the method by which the window is restored. 
    /// This member can be one or more of the following values.
    /// </summary>
    [Flags]
    public enum WindowPlacementFlags
        : int
    {
        /// <summary>
        /// If the calling thread and the thread that owns the window are attached to different input queues, the system posts the request to the thread that owns the window. 
        /// This prevents the calling thread from blocking its execution while other threads process the request.
        /// </summary>
        AsyncWindowPlacement = 0x0004,

        /// <summary>
        /// The restored window will be maximized, regardless of whether it was maximized before it was minimized. This setting is only valid the next time the window is restored. 
        /// It does not change the default restoration behavior.
        /// This flag is only valid when the SW_SHOWMINIMIZED value is specified for the showCmd member.
        /// </summary>
        RestoreToMaximize = 0x0002,

        /// <summary>
        /// The coordinates of the minimized window may be specified. 
        /// This flag must be specified if the coordinates are set in the ptMinPosition member.
        /// </summary>
        SetMinimizedPostion = 0x0001
    }
}