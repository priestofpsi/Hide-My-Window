using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace theDiary.Tools.HideMyWindow
{
    public class HiddenWindowStore
        : List<long>
    {
        #region Constructors
        public HiddenWindowStore()
        {
            this.Added += Items_Added;
            this.Removed += Items_Removed;
        }
        #endregion

        #region Public Event Declarations
        public event EventHandler Added;

        public event EventHandler Removed;
        #endregion

        public void Add(IntPtr handle)
        {
            long value = handle.ToInt64();
            if (base.Contains(value))
                return;
            base.Add(value);
            if (this.Added != null)
                this.Added(this, new EventArgs());
        }

        public bool Remove(IntPtr handle)
        {
            long value = handle.ToInt64();
            bool returnValue = base.Remove(value);

            if (returnValue && this.Removed != null)
                this.Removed(this, new EventArgs());

            return returnValue;
        }

        
        [XmlIgnore]
        internal static string StoreFileName = "WindowStore.xml";
        private static bool failed = false;

       

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

            System.IO.Stream stream = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly().OpenFile(HiddenWindowStore.StoreFileName, FileMode.OpenOrCreate, FileAccess.Write);

            var xs = new XmlSerializer(typeof(HiddenWindowStore));
            using (var tw = new StreamWriter(stream))
                xs.Serialize(tw, store);
        }
        
        internal static HiddenWindowStore Load()
        {
            System.IO.Stream stream = null;
            if (System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly().FileExists(HiddenWindowStore.StoreFileName))
                stream = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly().OpenFile(HiddenWindowStore.StoreFileName, FileMode.Open);

            try
            {
                if (stream == null)
                    stream = new FileStream(HiddenWindowStore.StoreFileName, FileMode.OpenOrCreate);
                var xs = new XmlSerializer(typeof(HiddenWindowStore));
                using (var fileStream = new StreamReader(stream))
                    return (HiddenWindowStore)xs.Deserialize(fileStream);
            }
            catch (Exception ex)
            {
                if (failed)
                    return new HiddenWindowStore();

                failed = true;
                if (System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly().FileExists(HiddenWindowStore.StoreFileName))
                    System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly().DeleteFile(HiddenWindowStore.StoreFileName);
                return HiddenWindowStore.Load();
            }
        }

        
    }
    
}
