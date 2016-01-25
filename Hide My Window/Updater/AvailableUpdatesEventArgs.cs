using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace theDiary.Tools.HideMyWindow
{


    public class AvailableUpdatesEventArgs
        : EventArgs
    {
        public AvailableUpdatesEventArgs(IEnumerable<AvailableUpdate> items)
            : base()
        {
            this.Items = items.ToArray();

        }

        public AvailableUpdate[] Items { get; private set; }
    }
}
