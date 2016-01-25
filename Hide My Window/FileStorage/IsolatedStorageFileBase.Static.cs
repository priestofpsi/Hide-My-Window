namespace theDiary.Tools.HideMyWindow
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    public abstract partial class IsolatedStorageFileBase
    {
        #region Declarations

        #region Static Declarations

        private static volatile Dictionary<Type, string> isolatedStorageFileNames = new Dictionary<Type, string>();
        private static volatile Dictionary<Type, bool> failedToLoad = new Dictionary<Type, bool>();
        private static readonly object syncObject = new object();
        private static readonly IsolatedStorageFile IsolatedStore = IsolatedStorageFile.GetUserStoreForAssembly();

        #endregion

        #endregion

        #region Methods & Functions

        public static event FileEventHandler FileNotification;

        private static bool Exists<T>() where T : class, IIsolatedStorageFile, new()
        {
            return IsolatedStore.FileExists(GetFileName<T>());
        }

        private static string GetFileName<T>() where T : class, IIsolatedStorageFile, new()
        {
            lock (syncObject)
            {
                Type type = typeof (T);
                if (!isolatedStorageFileNames.ContainsKey(type))
                {
                    isolatedStorageFileNames.Add(type,
                        Activator.CreateInstance<T>().GetStorageFileName());
                }

                return isolatedStorageFileNames[type];
            }
        }

        private static void Delete<T>() where T : class, IIsolatedStorageFile, new()
        {
            if (Exists<T>())
                IsolatedStore.DeleteFile(GetFileName<T>());
        }

        private static T Recreate<T>() where T : class, IIsolatedStorageFile, new()
        {
            Delete<T>();
            return LoadFile<T>();
        }

        public static T LoadFile<T>() where T : class, IIsolatedStorageFile, new()
        {
            bool wasCreated;
            return LoadFile<T>(out wasCreated);
        }

        private static bool OpenFile<T>(FileMode mode, FileAccess access, FileShare share, out Stream stream)
            where T : class, IIsolatedStorageFile, new()
        {
            bool wasCreated = false;
            string fileName = GetFileName<T>();
            if (Exists<T>())
            {
                stream = IsolatedStore.OpenFile(fileName, mode,
                    access, share);
            }
            else
            {
                stream = new FileStream(fileName, FileMode.OpenOrCreate);
                wasCreated = true;
            }
            return wasCreated;
        }

        private static Stream OpenFile<T>(FileMode mode, FileAccess access, FileShare share)
            where T : class, IIsolatedStorageFile, new()
        {
            Stream returnValue;
            OpenFile<T>(mode, access, share, out returnValue);

            return returnValue;
        }

        protected static void RaiseFileNotification(object sender, FileEventArgs e)
        {
            FileNotification?.Invoke(sender, e);
        }

        protected static void RaiseFileNotification(FileEventArgs e)
        {
            RaiseFileNotification(null, e);
        }

        private static bool FailedToLoad<T>(bool? value = null) where T : class, IIsolatedStorageFile, new()
        {
            lock (syncObject)
            {
                Type type = typeof (T);
                if (!failedToLoad.ContainsKey(type))
                    failedToLoad.Add(type, false);

                if (value.HasValue)
                    failedToLoad[type] = value.Value;

                return failedToLoad[type];
            }
        }

        private static LoadFileResult<T> LoadFileWithResult<T>() where T : class, IIsolatedStorageFile, new()
        {
            T returnValue = null;
            Stream stream = null;
            string fileName = GetFileName<T>();
            bool isNew = false;
            try
            {
                isNew = OpenFile<T>(FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite,
                    out stream);
                XmlSerializer xs = new XmlSerializer(typeof (T));
                using (StreamReader fileStream = new StreamReader(stream))
                    returnValue = (T) xs.Deserialize(fileStream);
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex);
#endif
                if (FailedToLoad<T>())
                    returnValue = Activator.CreateInstance<T>();
                else
                {
                    FailedToLoad<T>(true);
                    returnValue = Recreate<T>();
                }
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }

            return new LoadFileResult<T>(returnValue, isNew);
        }

        private static async Task<LoadFileResult<T>> LoadFileWithResultAsync<T>()
            where T : class, IIsolatedStorageFile, new()
        {
            return Task.Run(() => LoadFileWithResult<T>()).Result;
        }

        public static T LoadFile<T>(out bool wasCreated) where T : class, IIsolatedStorageFile, new()
        {
            LoadFileResult<T> loadResult = LoadFileWithResult<T>(); //LoadFileWithResultAsync<T>().Result;
            wasCreated = loadResult.WasCreated;
            return loadResult.Result;
        }

        #endregion
    }
}