using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;

namespace theDiary.Tools.HideMyWindow
{
    public class HiddenWindowStore
        : IList<long>
    {
        #region Constructors

        public HiddenWindowStore()
        {
            this.Added += Items_Added;
            this.Removed += Items_Removed;
        }

        #endregion Constructors

        #region Private Declarations

        private List<long> items = new List<long>();

        #endregion Private Declarations

        #region Public Event Declarations

        public event EventHandler<WindowEventArgs> Added;

        public event EventHandler<WindowEventArgs> Removed;

        #endregion Public Event Declarations

        #region Public Methods & Functions

        public void Add(IntPtr handle)
        {
            long value = handle.ToInt64();
            if (this.items.Contains(value))
                return;
            this.items.Add(value);
            if (this.Added != null)
                this.Added(this, new WindowEventArgs(WindowInfo.Find(handle)));
        }

        public bool Remove(IntPtr handle)
        {
            long value = handle.ToInt64();
            bool returnValue = this.items.Remove(value);

            if (returnValue && this.Removed != null)
                this.Removed(this, new WindowEventArgs(WindowInfo.Find(handle)));

            return returnValue;
        }

        public void ForEach(Action<long> action)
        {
            this.items.ForEach(action);
        }

        public void ForEach(Action<IntPtr> action)
        {
            this.items.ForEach(item => action(new IntPtr(item)));
        }

        IEnumerator<long> IEnumerable<long>.GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods & Functions

        [XmlIgnore]
        internal static string StoreFileName = "WindowStore.xml";

        private static bool FailedToLoad = false;

        int ICollection<long>.Count
        {
            get
            {
                return this.items.Count;
            }
        }

        bool ICollection<long>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        long IList<long>.this[int index]
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
            if (store == null)
                store = HiddenWindowStore.Load();

            System.IO.Stream stream = IsolatedStorageFile.GetUserStoreForAssembly().OpenFile(HiddenWindowStore.StoreFileName, FileMode.OpenOrCreate, FileAccess.Write);

            var xs = new XmlSerializer(typeof(HiddenWindowStore));
            using (var tw = new StreamWriter(stream))
                xs.Serialize(tw, store);
        }

        internal static HiddenWindowStore Load()
        {
            System.IO.Stream stream = null;
            try
            {
                if (IsolatedStorageFile.GetUserStoreForAssembly().FileExists(HiddenWindowStore.StoreFileName))
                    stream = IsolatedStorageFile.GetUserStoreForAssembly().OpenFile(HiddenWindowStore.StoreFileName, FileMode.OpenOrCreate);
                if (stream.Length == 0)
                    return new HiddenWindowStore();
                var xs = new XmlSerializer(typeof(HiddenWindowStore));
                using (var fileStream = new StreamReader(stream))
                    return (HiddenWindowStore)xs.Deserialize(fileStream);
            }
            catch
            {
                if (HiddenWindowStore.FailedToLoad)
                    return new HiddenWindowStore();

                HiddenWindowStore.FailedToLoad = true;
                if (IsolatedStorageFile.GetUserStoreForAssembly().FileExists(HiddenWindowStore.StoreFileName))
                    IsolatedStorageFile.GetUserStoreForAssembly().DeleteFile(HiddenWindowStore.StoreFileName);

                return HiddenWindowStore.Load();
            }
        }

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
                this.Removed(this, new WindowEventArgs(WindowInfo.Find(handle)));
        }

        public void Add(long item)
        {
            if (this.items.Contains(item))
                return;
            this.items.Add(item);
        }

        void ICollection<long>.Clear()
        {
            var handleVals = this.items.ToArray();
            this.items.Clear();
            if (this.Removed == null)
                return;

            foreach (var value in handleVals)
            {
                IntPtr handle = new IntPtr(value);
                this.Removed(this, new WindowEventArgs(WindowInfo.Find(handle)));
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
    }
}