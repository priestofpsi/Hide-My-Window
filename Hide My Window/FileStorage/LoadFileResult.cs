namespace theDiary.Tools.HideMyWindow
{
    /// <summary>
    /// Represents the results of a file load operation.
    /// </summary>
    /// <typeparam name="T">A <see cref="Type"/> of <see cref="IIsolatedStorageFile"/> implementation.</typeparam>
    public class LoadFileResult<T>
        where T : class, IIsolatedStorageFile, new()
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadFileResult{T}"/> class, with the specified <paramref name="result"/> and <paramref name="wasCreated"/> values.
        /// </summary>
        /// <param name="result">An instance of <typeparamref name="T"/>.</param>
        /// <param name="wasCreated">A value indicating if a file was created or not.</param>
        public LoadFileResult(T result, bool wasCreated)
        {
            this.Result = result;
            this.WasCreated = wasCreated;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value of <typeparamref name="T"/> indicating the result of the file loading action.
        /// </summary>
        public T Result { get; }

        /// <summary>
        /// Gets a value indicating if a file was created or not.
        /// </summary>
        public bool WasCreated { get; }

        #endregion
    }
}