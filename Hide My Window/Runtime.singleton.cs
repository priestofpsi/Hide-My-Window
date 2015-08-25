using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow
{
    public partial class Runtime
    {
        private static volatile Runtime instance;
        private static readonly object syncObject = new object();

        public static Runtime Instance
        {
        get
            {
                lock(Runtime.syncObject)
                {
                    if (Runtime.instance == null)
                        Runtime.instance = new Runtime();

                    return Runtime.instance;
                }

            }
        }
    }
}
