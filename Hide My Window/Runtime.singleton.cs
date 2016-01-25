namespace theDiary.Tools.HideMyWindow
{
    public partial class Runtime
    {
        #region Declarations

        #region Static Declarations

        private static volatile Runtime instance;
        private static readonly object syncObject = new object();

        #endregion

        #endregion

        #region Properties

        public static Runtime Instance
        {
            get
            {
                lock (syncObject)
                {
                    if (instance == null)
                        instance = new Runtime();

                    return instance;
                }
            }
        }

        #endregion
    }
}