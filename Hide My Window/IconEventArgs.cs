using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace theDiary.Tools.HideMyWindow
{
    public class IconEventArgs
        : EventArgs

    {
        #region Constructors

        public IconEventArgs()
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

        #region Properties

        public Icon Icon { get; set; }

        public bool HasIcon
        {
            get { return this.Icon != null; }
        }

        #endregion
    }
}