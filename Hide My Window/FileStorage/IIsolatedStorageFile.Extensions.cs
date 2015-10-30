using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace theDiary.Tools.HideMyWindow
{
    public static class IIsolatedStorageFileExtensions
    {
        private static volatile Dictionary<Type, string> isolatedStorageFileNames = new Dictionary<Type, string>();
        private static volatile Dictionary<Type, bool> failedToLoad = new Dictionary<Type, bool>();
        private static readonly object syncObject = new object();
        private static readonly IsolatedStorageFile IsolatedStore = IsolatedStorageFile.GetUserStoreForAssembly();

        private static bool Exists<T>(this T container)
            where T : class, IIsolatedStorageFile, new()
        {
            return IIsolatedStorageFileExtensions.IsolatedStore.FileExists(container.GetFileName());
        }

        private static void Delete<T>(this T container)
            where T : class, IIsolatedStorageFile, new()
        {
            if (container.Exists())
                IIsolatedStorageFileExtensions.IsolatedStore.DeleteFile(container.GetFileName());
        }

        private static T Recreate<T>(this T container)
            where T : class, IIsolatedStorageFile, new()
        {
            container.Delete();
            return container.LoadFile();
        }

        private static bool FailedToLoad<T>(this T container, bool? value = null)
            where T : class, IIsolatedStorageFile, new()
        {
            lock (IIsolatedStorageFileExtensions.syncObject)
            {
                Type type = typeof (T);
                if (!IIsolatedStorageFileExtensions.failedToLoad.ContainsKey(type))
                    IIsolatedStorageFileExtensions.failedToLoad.Add(type, false);

                if (value.HasValue)
                    IIsolatedStorageFileExtensions.failedToLoad[type] = value.Value;

                return IIsolatedStorageFileExtensions.failedToLoad[type];
            }
        }

        private static string GetFileName<T>(this T container)
            where T : class, IIsolatedStorageFile, new()
        {
            lock (IIsolatedStorageFileExtensions.syncObject)
            {
                Type type = typeof(T);
                if (!IIsolatedStorageFileExtensions.isolatedStorageFileNames.ContainsKey(type))
                    IIsolatedStorageFileExtensions.isolatedStorageFileNames.Add(type, System.Activator.CreateInstance<T>().GetStorageFileName());

                return IIsolatedStorageFileExtensions.isolatedStorageFileNames[type];
            }
        }



        public static void SaveFile<T>(this T container)
            where T : class, IIsolatedStorageFile, new()
        {
            Task task = Task.Run(() =>
            {
                string fileName = container.GetFileName();
                if (container == null)
                    container = container.LoadFile();

                using (
                    Stream stream = IIsolatedStorageFileExtensions.IsolatedStore.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    using (StreamWriter tw = new StreamWriter(stream))
                        xs.Serialize(tw, container);
                }
            });
            Task.WaitAll(task);
        }

        public static T LoadFile<T>(this T container)
            where T : class, IIsolatedStorageFile, new()
        {
            Task<T> task = Task.Run(() =>
            {
                T returnValue = null;
                Stream stream = null;
                string fileName = container.GetFileName();
                try
                {
                    
                    if (container.Exists())
                        stream = IIsolatedStorageFileExtensions.IsolatedStore.OpenFile(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

                    Program.IsConfigured = (stream != null);
                    if (stream == null)
                        stream = new FileStream(fileName, FileMode.OpenOrCreate);

                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    using (StreamReader fileStream = new StreamReader(stream))
                    {
                        returnValue = (T) xs.Deserialize(fileStream);
                    }
                }
                catch
                {
                    if (container.FailedToLoad())
                    {
                        returnValue = System.Activator.CreateInstance<T>();
                    }
                    else
                    {
                        container.FailedToLoad(true);
                        container.Recreate();
                    }
                }
                finally
                {
                    if (stream != null)
                        stream.Dispose();
                }

                return returnValue;
            });
            Task.WaitAll(task);
            return task.Result;
        }
    }
}
