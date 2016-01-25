using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow
{
    public delegate void ClientUpdatesEventHandler(object sender, UpdaterClientEventArgs e);

    public delegate void ClientUpdatesAvailableEventHandler(object sender, AvailableUpdatesEventArgs e);
}
