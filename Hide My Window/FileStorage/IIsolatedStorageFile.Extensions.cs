namespace theDiary.Tools.HideMyWindow
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    public static class IIsolatedStorageFileExtensions
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

        private static Stream OpenFile<T>(this T container, FileMode mode, FileAccess access, FileShare share)
            where T : class, IIsolatedStorageFile, new()
        {
            Stream returnValue;
            container.OpenFile(mode, access, share, out returnValue);

            return returnValue;
        }

        private static bool OpenFile<T>(this T container, FileMode mode, FileAccess access, FileShare share,
            out Stream stream) where T : class, IIsolatedStorageFile, new()
        {
            bool wasCreated = false;
            if (container.Exists())
            {
                stream = IsolatedStore.OpenFile(container.GetFileName(), mode, access,
                    share);
            }
            else
            {
                stream = new FileStream(container.GetFileName(), FileMode.OpenOrCreate);
                wasCreated = true;
            }
            return wasCreated;
        }

        private static Stream OpenFile<T>(this T container, FileMode mode, FileAccess access)
            where T : class, IIsolatedStorageFile, new()
        {
            return IsolatedStore.OpenFile(container.GetFileName(), mode, access);
        }

        private static bool Exists<T>(this T container) where T : class, IIsolatedStorageFile, new()
        {
            return IsolatedStore.FileExists(container.GetFileName());
        }

        private static void Delete<T>(this T container) where T : class, IIsolatedStorageFile, new()
        {
            if (container.Exists())
                IsolatedStore.DeleteFile(container.GetFileName());
        }

        private static T Recreate<T>(this T container) where T : class, IIsolatedStorageFile, new()
        {
            container.Delete();
            return container.LoadFile();
        }

        private static bool FailedToLoad<T>(this T container, bool? value = null)
            where T : class, IIsolatedStorageFile, new()
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

        private static string GetFileName<T>(this T container) where T : class, IIsolatedStorageFile, new()
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

        public static void SaveFile<T>(this T container) where T : class, IIsolatedStorageFile, new()
        {
            Task task = Task.Run(() =>
            {
                string fileName = container.GetFileName();
                if (container == null)
                    container = container.LoadFile();

                using (Stream stream = container.OpenFile(FileMode.OpenOrCreate, FileAccess.Write))
                {
                    XmlSerializer xs = new XmlSerializer(typeof (T));
                    using (StreamWriter tw = new StreamWriter(stream))
                        xs.Serialize(tw, container);
                }
            });
            Task.WaitAll(task);
        }

        public static T LoadFile<T>(this T container) where T : class, IIsolatedStorageFile, new()
        {
            bool wasCreated;
            return container.LoadFile(out wasCreated);
        }

        public static T LoadFile<T>(this T container, out bool wasCreated) where T : class, IIsolatedStorageFile, new()
        {
            Task<LoadFileResult<T>> task = Task.Run(() =>
            {
                T returnValue = null;
                Stream stream = null;
                string fileName = container.GetFileName();
                bool isNew = false;
                try
                {
                    isNew = container.OpenFile(FileMode.Open,
                        FileAccess.ReadWrite, FileShare.ReadWrite, out stream);
                    XmlSerializer xs = new XmlSerializer(typeof (T));
                    using (StreamReader fileStream = new StreamReader(stream))
                        returnValue = (T) xs.Deserialize(fileStream);
                }
                catch
                {
                    if (container.FailedToLoad())
                        returnValue = Activator.CreateInstance<T>();
                    else
                    {
                        container.FailedToLoad(true);
                        returnValue = container.Recreate();
                    }
                }
                finally
                {
                    if (stream != null)
                        stream.Dispose();
                }

                return new LoadFileResult<T>(returnValue, isNew);
            });
            Task.WaitAll(task);
            LoadFileResult<T> loadResult = task.Result;

            wasCreated = loadResult.WasCreated;
            return loadResult.Result;
        }

        #endregion
    }
}