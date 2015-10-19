using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theDiary.Tools.HideMyWindow
{
    public class IconEventArgs
        : EventArgs

    {
        #region Public Constructors
        public IconEventArgs()
            : base()
        {
        }

        public IconEventArgs(Icon icon)
            : this()
        {
            if (icon == null)
                throw new ArgumentNullException("icon");

            this.Icon = icon;
        }
        #endregion

        #region Public Properties
        public Icon Icon
        {
            get; set;
        }

        public bool HasIcon
        {
            get
            {
                return this.Icon != null;
            }
        }
        #endregion
    }
}
