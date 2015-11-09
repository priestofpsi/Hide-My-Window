using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace theDiary.Tools.HideMyWindow
{
    public class HiddenWindowStore : IIsolatedStorageFile, IList<WindowsStoreItem>
    {
        #region Public Constructors
        public HiddenWindowStore()
        {
            this.Added += this.Items_Added;
            this.Removed += this.Items_Removed;
        }
        #endregion

        #region Constant Declarations
        [XmlIgnore]
        internal static string StorageFileName = "WindowStore.xml";
        #endregion

        #region Declarations
        private readonly List<WindowsStoreItem> items = new List<WindowsStoreItem>();

        private bool running;
        #endregion

        #region Properties
        public int Count
        {
            get
            {
                return this.items.Count;
            }
        }

        bool ICollection<WindowsStoreItem>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public WindowsStoreItem this[int index]
        {
            get
            {
                return this.items[index];
            }

            set
            {
                this.items[index] = value;
            }
        }

        private bool CanCheckProcesses
        {
            get
            {
                return this.items.ToArray().Length > 0;
            }
        }

        public WindowsStoreItem this[IntPtr handle]
        {
            get
            {
                return this.items.FirstOrDefault(item => item.Handle == handle);
            }
        }

        public WindowsStoreItem this[WindowInfo window]
        {
            get
            {
                return this[window.Handle];
            }
        }
        #endregion

        #region Methods & Functions
        public static event FileEventHandler FileNotification;

        private void Items_Removed(object sender, EventArgs e)
        {
            HiddenWindowStore.Save(this);
        }

        private void Items_Added(object sender, EventArgs e)
        {
            HiddenWindowStore.Save(this);
        }

        internal static void Save(HiddenWindowStore store)
        {
            if (FileNotification != null)
                FileNotification(store, new FileEventArgs(HiddenWindowStore.StorageFileName, FileEventTypes.Saving));

            store.SaveFile();

            if (FileNotification != null)
                FileNotification(store, new FileEventArgs(HiddenWindowStore.StorageFileName));
        }

        internal static HiddenWindowStore Load()
        {
            if (FileNotification != null)
                FileNotification(null, new FileEventArgs(HiddenWindowStore.StorageFileName, FileEventTypes.Opening));
            HiddenWindowStore returnValue = null;
            bool wasCreated;
            returnValue = returnValue.LoadFile(out wasCreated);

            if (FileNotification != null)
                FileNotification(null, new FileEventArgs(HiddenWindowStore.StorageFileName, FileEventTypes.Loaded));
            return returnValue;
        }

        public event WindowEventHandler Added;

        public event WindowEventHandler Removed;

        public WindowsStoreItem Add(IntPtr handle)
        {
            WindowsStoreItem returnValue = null;
            long value = handle.ToInt64();
            if (this.items.Any(item => item.Handle == handle))
            {
                returnValue = this.items.FirstOrDefault(item => item.Handle == handle);
                WindowInfo window = WindowInfo.Find(handle);
            }
            else
            {
                WindowInfo window = WindowInfo.Find(handle);
                returnValue = new WindowsStoreItem(window);
                this.items.Add(returnValue);
                WindowInfoEventArgs e = new WindowInfoEventArgs(window);
                this.Added(this, e);
            }
            returnValue.RegisterHandlers();
            return returnValue;
        }

        public bool Remove(IntPtr handle)
        {
            long value = handle.ToInt64();
            WindowsStoreItem match = this.items.FirstOrDefault(item => item.Handle == handle);
            if (match == null)
                return false;
            bool returnValue = this.items.Remove(match);

            if (returnValue)
            {
                WindowInfoEventArgs e = new WindowInfoEventArgs(WindowInfo.Find(handle));
                this.Removed(this, e);
            }
            return returnValue;
        }

        public void ForEach(Action<long> action)
        {
            this.items.ForEach(item => action(item.HandleValue));
        }

        public void All(Action<WindowsStoreItem> action)
        {
            this.items.ForEach(item => action(item));
        }

        public void ForEach(Action<IntPtr> action)
        {
            this.items.ForEach(item => action(item.Handle));
        }
        #endregion

        #region Interface Implementations
        public event NotificationEventHandler Notification;

        public void Save()
        {
            if (this.Notification != null)
                this.Notification(this, new NotificationEventArgs("Saving"));

            this.SaveFile();

            if (this.Notification != null)
                this.Notification(this, new NotificationEventArgs("Saved"));
        }

        public void Reset()
        {
            if (this.Notification != null)
                this.Notification(this, new NotificationEventArgs("Resetting"));

            Runtime.Instance.Store = this.LoadFile();

            if (this.Notification != null)
                this.Notification(this, new NotificationEventArgs("Reset"));
        }

        string IIsolatedStorageFile.GetStorageFileName()
        {
            return HiddenWindowStore.StorageFileName;
        }

        int IList<WindowsStoreItem>.IndexOf(WindowsStoreItem item)
        {
            throw new NotImplementedException();
        }

        void IList<WindowsStoreItem>.Insert(int index, WindowsStoreItem item)
        {
            throw new NotImplementedException();
        }

        void IList<WindowsStoreItem>.RemoveAt(int index)
        {
            IntPtr handle = this.items[index].Handle;
            this.items.RemoveAt(index);
            if (this.Removed != null)
                this.Removed(this, new WindowInfoEventArgs(WindowInfo.Find(handle)));
        }

        public void Add(WindowsStoreItem item)
        {
            item.RegisterHandlers(WindowInfo.Find(item.Handle));
            if (this.items.Contains(item))
                return;

            this.items.Add(item);
        }

        void ICollection<WindowsStoreItem>.Clear()
        {
            long[] handleVals = this.items.Select(item => item.HandleValue).ToArray();
            this.items.Clear();
            if (this.Removed == null)
                return;

            foreach (long value in handleVals)
            {
                IntPtr handle = new IntPtr(value);
                this.Removed(this, new WindowInfoEventArgs(WindowInfo.Find(handle)));
            }
        }

        bool ICollection<WindowsStoreItem>.Contains(WindowsStoreItem item)
        {
            return this.items.Contains(item);
        }

        void ICollection<WindowsStoreItem>.CopyTo(WindowsStoreItem[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        bool ICollection<WindowsStoreItem>.Remove(WindowsStoreItem item)
        {
            return this.items.Remove(item);
        }

        public IEnumerator<WindowsStoreItem> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.items.GetEnumerator();
        }
        #endregion
    }
}