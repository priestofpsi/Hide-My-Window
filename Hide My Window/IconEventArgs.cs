namespace theDiary.Tools.HideMyWindow
{
    using System;
    using System.Drawing;

    public class IconEventArgs : EventArgs

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