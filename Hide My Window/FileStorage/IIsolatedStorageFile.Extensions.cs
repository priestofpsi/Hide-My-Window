﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

using System.Diagnostics;
using System.Collections.Concurrent;

namespace theDiary.Tools.HideMyWindow
{


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
                Type type = typeof(T);
                if (!failedToLoad.ContainsKey(type))
                    failedToLoad.Add(type, false);

                if (value.HasValue)
                    failedToLoad[type] = value.Value;

                return failedToLoad[type];
            }
        }

        private static string GetFileName<T>(this T container)
            where T : class, IIsolatedStorageFile, new()
        {
            lock (syncObject)
            {
                Type type = typeof(T);
                if (!isolatedStorageFileNames.ContainsKey(type))
                {
                    isolatedStorageFileNames.Add(type,
                        Activator.CreateInstance<T>().GetStorageFileName());
                }

                return isolatedStorageFileNames[type];
            }
        }

        public static void SaveFile<T>(this T container)
            where T : class, IIsolatedStorageFile, new()
        {
            //Task task = Task.Run(() =>
            //taskGroups.Append(() =>
            //{                
            string fileName = container.GetFileName();
            Runtime.Instance.WriteDebug(nameof(SaveFile), typeof(T).Name, $"Saving {fileName}", "File");
            if (container == null)
                container = container.LoadFile();

            using (Stream stream = container.OpenFile(FileMode.OpenOrCreate, FileAccess.Write))
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                using (StreamWriter tw = new StreamWriter(stream))
                    xs.Serialize(tw, container);
            }
            Runtime.Instance.WriteDebug(nameof(SaveFile), typeof(T).Name, $"Saved {fileName}", "File");
            //});
            //Task.WaitAll(task);
        }

        public static T LoadFile<T>(this T container)
            where T : class, IIsolatedStorageFile, new()
        {
            bool wasCreated;
            return container.LoadFile(out wasCreated);
        }

        public static T LoadFile<T>(this T container, out bool wasCreated)
            where T : class, IIsolatedStorageFile, new()
        {
            //Task<LoadFileResult<T>> task = Task.Run(() =>            
            //{
            T returnValue = null;
            Stream stream = null;
            string fileName = container.GetFileName();
            wasCreated = false;
            try
            {
                wasCreated = container.OpenFile(FileMode.Open,
                    FileAccess.ReadWrite, FileShare.ReadWrite, out stream);
                XmlSerializer xs = new XmlSerializer(typeof(T));
                using (StreamReader fileStream = new StreamReader(stream))
                    returnValue = (T)xs.Deserialize(fileStream);
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

            //return new LoadFileResult<T>(returnValue, isNew);
            //});
            //Task.WaitAll(task);
            //LoadFileResult<T> loadResult = task.Result;

            //wasCreated = loadResult.WasCreated;
            return returnValue;//loadResult.Result;
        }

        #endregion

        private static TaskGroup taskGroups = new TaskGroup();
    }




    public interface IAppendable
    {
        void Append(Action action);
    }

    public class TaskGroup : IAppendable
    {
        public int CurrentlyQueuedTasks { get { return _currentlyQueued; } }

        private readonly object _previousTaskMonitor;
        private Task _previousTask;
        private int _currentlyQueued;

        public TaskGroup()
        {
            _previousTaskMonitor = new object();
            _previousTask = Task.FromResult(false);
        }

        public void Append(Action action)
        {
            lock (_previousTaskMonitor)
            {
                Interlocked.Increment(ref _currentlyQueued);
                _previousTask = _previousTask.ContinueWith(task =>
                {
                    try
                    {
                        action();
                    }
                    catch (Exception)
                    {
                        //TODO
                    }
                    finally
                    {
                        Interlocked.Decrement(ref _currentlyQueued);
                    }
                });
            }
        }

        public TResult Append<TResult>(Func<TResult> action)
        {

            lock (_previousTaskMonitor)
            {
                Interlocked.Increment(ref _currentlyQueued);
                TResult result = default(TResult);
                _previousTask = _previousTask.ContinueWith<TResult>(task =>
                {
                    try
                    {
                        result = action();
                    }
                    catch (Exception)
                    {
                        //TODO
                    }
                    finally
                    {
                        Interlocked.Decrement(ref _currentlyQueued);
                    }
                    return result;
                });
                return result;
            }
        }
    }

    public class QueueAppendable : IAppendable, IDisposable
    {
        public int CurrentlyQueuedTasks { get { return _Queue.Count; } }

        BlockingCollection<Action> _Queue = new BlockingCollection<Action>();

        public QueueAppendable()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    try
                    {
                        var action = _Queue.Take();
                        action();
                    }
                    catch (InvalidOperationException)
                    {
                        break;
                    }
                    catch
                    {
                        // TODO log me
                    }
                }
            });
        }

        public void Append(Action action)
        {
            _Queue.Add(action);
        }

        public void Dispose()
        {
            _Queue.CompleteAdding();
        }
    }

}

