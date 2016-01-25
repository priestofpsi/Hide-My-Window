namespace theDiary.Tools.HideMyWindow
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    public class HiddenWindowStore : IsolatedStorageFileBase, IList<WindowsStoreItem>
    {
        #region Constructors

        public HiddenWindowStore()
            : base(StorageFileName)
        {
            this.Added += this.Items_Added;
            this.Removed += this.Items_Removed;
        }

        #endregion

        #region Declarations

        #region Static Declarations

        [XmlIgnore] internal static string StorageFileName = "WindowStore.xml";

        #endregion

        #region Private Declarations

        private readonly List<WindowsStoreItem> items = new List<WindowsStoreItem>();

        #endregion

        #endregion

        #region Properties

        public int Count
        {
            get { return this.items.Count; }
        }

        bool ICollection<WindowsStoreItem>.IsReadOnly
        {
            get { return false; }
        }

        public WindowsStoreItem this[int index]
        {
            get { return this.items[index]; }

            set { this.items[index] = value; }
        }

        private bool CanCheckProcesses
        {
            get { return this.items.ToArray().Length > 0; }
        }

        public WindowsStoreItem this[IntPtr handle]
        {
            get { return this.items.FirstOrDefault(item => item.Handle == handle); }
        }

        public WindowsStoreItem this[WindowInfo window]
        {
            get { return this[window.Handle]; }
        }

        #endregion

        #region Methods & Functions

        private void Items_Removed(object sender, EventArgs e)
        {
            Save(this);
        }

        private void Items_Added(object sender, EventArgs e)
        {
            Save(this);
        }

        internal static void Save(HiddenWindowStore store)
        {
            RaiseFileNotification(store, new FileEventArgs(StorageFileName, FileEventTypes.Saving));

            store.SaveFile();
            RaiseFileNotification(store, new FileEventArgs(StorageFileName));
        }

        internal static HiddenWindowStore Load()
        {
            RaiseFileNotification(new FileEventArgs(StorageFileName, FileEventTypes.Opening));

            bool wasCreated;
            HiddenWindowStore returnValue = LoadFile<HiddenWindowStore>(out wasCreated);
            ;

            RaiseFileNotification(new FileEventArgs(StorageFileName, FileEventTypes.Loaded));
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
                if (this.Added != null)
                {
                    WindowInfoEventArgs e = new WindowInfoEventArgs(window);
                    this.Added(this, e);
                }
            }
            if (returnValue != null)
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

            if (this.Removed != null && returnValue)
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

        public override void Save()
        {
            this.RaiseNotification(this, new NotificationEventArgs("Saving"));

            this.SaveFile();

            this.RaiseNotification(this, new NotificationEventArgs("Saved"));
        }

        public override void Reset()
        {
            this.RaiseNotification(this, new NotificationEventArgs("Resetting"));

            Runtime.Instance.Store = this.LoadFile();

            this.RaiseNotification(this, new NotificationEventArgs("Reset"));
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