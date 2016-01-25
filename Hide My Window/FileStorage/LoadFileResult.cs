namespace theDiary.Tools.HideMyWindow
{
    internal class LoadFileResult<T>
        where T : class, IIsolatedStorageFile, new()
    {
        #region Constructors

        public LoadFileResult(T result, bool wasCreated)
        {
            this.Result = result;
            this.WasCreated = wasCreated;
        }

        #endregion

        #region Properties

        public T Result { get; }

        public bool WasCreated { get; }

        #endregion
    }
}