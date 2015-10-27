using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace theDiary.Tools.HideMyWindow
{
    public class HiddenWindowStore
        : IList<long>
    {
        #region Constructors

        public HiddenWindowStore()
        {
            this.Added += this.Items_Added;
            this.Removed += this.Items_Removed;
        }

        #endregion

        #region Constant Declarations

        [XmlIgnore] internal static string StorageFileName = "WindowStore.xml";

        private static bool FailedToLoad;

        #endregion

        #region Declarations

        private readonly List<long> items = new List<long>();

        private bool running;

        #endregion

        #region Properties

        int ICollection<long>.Count
        {
            get { return this.items.Count; }
        }

        bool ICollection<long>.IsReadOnly
        {
            get { return false; }
        }

        long IList<long>.this[int index]
        {
            get { return this.items[index]; }

            set { this.items[index] = value; }
        }

        private bool CanCheckProcesses
        {
            get { return this.items.ToArray().Length > 0; }
        }

        #endregion

        #region Methods & Functions

        private void Items_Removed(object sender, EventArgs e)
        {
            HiddenWindowStore.Save(this);
            this.CheckWindowProcesses();
        }

        private void Items_Added(object sender, EventArgs e)
        {
            HiddenWindowStore.Save(this);
            this.CheckWindowProcesses();
        }

        internal static void Save(HiddenWindowStore store)
        {
            Task task = Task.Run(() =>
            {
                if (store == null)
                    store = HiddenWindowStore.Load();
                if (Notification != null)
                    Notification(store, new FileEventArgs(HiddenWindowStore.StorageFileName, FileEventTypes.Saving));

                Stream stream = IsolatedStorageFile.GetUserStoreForAssembly()
                    .OpenFile(HiddenWindowStore.StorageFileName, FileMode.OpenOrCreate, FileAccess.Write);

                XmlSerializer xs = new XmlSerializer(typeof (HiddenWindowStore));
                using (StreamWriter tw = new StreamWriter(stream))
                    xs.Serialize(tw, store);

                if (Notification != null)
                    Notification(store, new FileEventArgs(HiddenWindowStore.StorageFileName, FileEventTypes.Saved));
            });
            Task.WaitAll(task);
            if (Notification != null)
                Notification(null, new FileEventArgs(HiddenWindowStore.StorageFileName));
        }

        internal static HiddenWindowStore Load()
        {
            Task<HiddenWindowStore> task = Task.Run(() =>
            {
                HiddenWindowStore returnValue = null;
                Stream stream = null;
                try
                {
                    if (IsolatedStorageFile.GetUserStoreForAssembly().FileExists(HiddenWindowStore.StorageFileName))
                        stream = IsolatedStorageFile.GetUserStoreForAssembly()
                            .OpenFile(HiddenWindowStore.StorageFileName, FileMode.OpenOrCreate);
                    if (stream == null || stream.Length == 0)
                    {
                        returnValue = new HiddenWindowStore();
                    }
                    else
                    {
                        XmlSerializer xs = new XmlSerializer(typeof (HiddenWindowStore));
                        using (StreamReader fileStream = new StreamReader(stream))
                            returnValue = (HiddenWindowStore) xs.Deserialize(fileStream);
                    }
                }
                catch
                {
                    if (HiddenWindowStore.FailedToLoad)
                    {
                        returnValue = new HiddenWindowStore();
                    }
                    else
                    {
                        HiddenWindowStore.FailedToLoad = true;
                        if (IsolatedStorageFile.GetUserStoreForAssembly().FileExists(HiddenWindowStore.StorageFileName))
                            IsolatedStorageFile.GetUserStoreForAssembly().DeleteFile(HiddenWindowStore.StorageFileName);
                        returnValue = HiddenWindowStore.Load();
                    }
                }
                finally
                {
                    if (stream != null)
                        stream.Dispose();
                    if (returnValue != null)
                        returnValue.CheckWindowProcesses();
                }
                return returnValue;
            });
            Task.WaitAll(task);
            if (Notification != null)
                Notification(null, new FileEventArgs(HiddenWindowStore.StorageFileName));
            return task.Result;
        }

        public static event FileEventHandler Notification;

        public event EventHandler<WindowEventArgs> Added;

        public event EventHandler<WindowEventArgs> Removed;

        public void Add(IntPtr handle)
        {
            long value = handle.ToInt64();
            if (this.items.Contains(value))
                return;

            this.items.Add(value);
            WindowEventArgs e = new WindowEventArgs(WindowInfo.FindByHandle(handle));
            this.Added(this, e);
        }

        public bool Remove(IntPtr handle)
        {
            long value = handle.ToInt64();
            bool returnValue = this.items.Remove(value);

            if (returnValue)
            {
                WindowEventArgs e = new WindowEventArgs(WindowInfo.FindByHandle(handle));
                this.Removed(this, e);
            }
            return returnValue;
        }

        private void CheckWindowProcesses()
        {
            if (this.running)
                return;

            Task task = Task.Run(() =>
            {
                Thread.CurrentThread.Name = "HideMyWindow:CheckWindowProcesses";
                this.running = true;
                while (this.CanCheckProcesses)
                {
                    this.items.Select(handle => WindowInfo.Find(handle))
                        .AsParallel()
                        .ForAll(window => this.CheckWindowProcess(window));
                    Thread.Sleep(100);
                }

                this.running = false;
            });
        }

        private void CheckWindowProcess(WindowInfo window)
        {
            long handle = window.Handle.ToInt64();
            try
            {
                //state.ApplicationProcess.Refresh();
                if (window.ApplicationProcess.Id == 0)
                {
                    this.Remove(new IntPtr(handle));
                }
                else if (window.ApplicationProcess.HasExited)
                {
                    window.NotifyApplicationExited();
                    this.Remove(new IntPtr(handle));
                }
            }
            catch
            {
                this.Remove(new IntPtr(handle));
            }
        }

        public void ForEach(Action<long> action)
        {
            this.items.ForEach(action);
        }

        public void ForEach(Action<IntPtr> action)
        {
            this.items.ForEach(item => action(new IntPtr(item)));
        }

        #endregion

        #region Interface Implementations

        int IList<long>.IndexOf(long item)
        {
            throw new NotImplementedException();
        }

        void IList<long>.Insert(int index, long item)
        {
            throw new NotImplementedException();
        }

        void IList<long>.RemoveAt(int index)
        {
            IntPtr handle = new IntPtr(this.items[index]);
            this.items.RemoveAt(index);
            if (this.Removed != null)
                this.Removed(this, new WindowEventArgs(WindowInfo.FindByHandle(handle)));
        }

        public void Add(long item)
        {
            if (this.items.Contains(item))
                return;
            this.items.Add(item);
        }

        void ICollection<long>.Clear()
        {
            long[] handleVals = this.items.ToArray();
            this.items.Clear();
            if (this.Removed == null)
                return;

            foreach (long value in handleVals)
            {
                IntPtr handle = new IntPtr(value);
                this.Removed(this, new WindowEventArgs(WindowInfo.FindByHandle(handle)));
            }
        }

        bool ICollection<long>.Contains(long item)
        {
            throw new NotImplementedException();
        }

        void ICollection<long>.CopyTo(long[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        bool ICollection<long>.Remove(long item)
        {
            throw new NotImplementedException();
        }

        IEnumerator<long> IEnumerable<long>.GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}