using System;
using System.Collections.Generic;
using System.Linq;

namespace theDiary.Tools.HideMyWindow
{
    public partial class Runtime
    {
        #region Constant Declarations

        private static volatile Runtime instance;
        private static readonly object syncObject = new object();

        #endregion

        #region Properties

        public static Runtime Instance
        {
            get
            {
                lock (Runtime.syncObject)
                {
                    if (Runtime.instance == null)
                        Runtime.instance = new Runtime();

                    return Runtime.instance;
                }
            }
        }

        #endregion
    }
}