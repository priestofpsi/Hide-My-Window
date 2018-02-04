namespace theDiary.Tools.HideMyWindow
{
    public partial class Runtime
    {
        #region Static Declarations
        private static volatile Runtime instance;
        private static readonly object singletonSync = new object();
        #endregion

        #region Properties

        /// <summary>
        /// Gets a thread safe instance of the <see cref="Runtime"/> class.
        /// </summary>
        public static Runtime Instance
        {
            get
            {
                lock (Runtime.singletonSync)
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