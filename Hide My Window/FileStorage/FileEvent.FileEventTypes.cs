using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow
{
    /// <summary>
    /// Specifies the possible types of actions been performed an a file.
    /// </summary>
    public enum FileEventTypes
    {
        /// <summary>
        /// Default value where no event has taken place on a file.
        /// </summary>
        None,

        /// <summary>
        /// Indicates that a file is been created.
        /// </summary>
        Creating,

        /// <summary>
        /// Indicates that a file was created.
        /// </summary>
        Created,

        /// <summary>
        /// Indicates that a file is been opened.
        /// </summary>
        Opening,

        /// <summary>
        /// Indicates that a file has been opened.
        /// </summary>
        Opened,

        /// <summary>
        /// Indicates that a file is been loaded.
        /// </summary>
        Loading,

        /// <summary>
        /// Indicates that a file has been loaded.
        /// </summary>
        Loaded,

        /// <summary>
        /// Indicates that a file is been saved.
        /// </summary>
        Saving,

        /// <summary>
        /// Indicates that a file has been saved.
        /// </summary>
        Saved,

        Deleting,

        /// <summary>
        /// Indicates that a file has been deleted.
        /// </summary>
        Deleted
    }
}
