using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow
{
    /// <summary>
    /// The delegate that handles the events for a <see cref="IsolatedStorageFileBase" /> instance.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">An instance of <see cref=""FileEventArgs/> containing the details of the event.</param>
    public delegate void FileEventHandler(object sender, FileEventArgs e);
}
