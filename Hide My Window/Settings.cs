using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace theDiary.Tools.HideMyWindow
{
    public class Settings
    {
        public Settings()
        {
            this.Hotkey = new HotKeyBindingList();
        }

        private Point location;

        [XmlIgnore]
        internal static string settingsxml = "Settings.xml";

        [XmlElement]
        public HotKeyBindingList Hotkey;

        [XmlElement]
        public View CurrentView
        {
            get; set;
        }
        [XmlElement]
        public bool HideStatusbar
        {
            get; set;
        }
        public Point Location
        {
            get
            {
                return this.location;
            }
            set
            {
                this.location = value;
            }
        }

        internal static void Save(Settings settings)
        {
            if (settings == null)
                settings = Settings.Load();

            System.IO.Stream stream = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly().OpenFile(settingsxml, FileMode.OpenOrCreate, FileAccess.Write);

            var xs = new XmlSerializer(typeof(Settings));
            using (var tw = new StreamWriter(stream))
                xs.Serialize(tw, settings);
        }
        private static bool failed = false;
        internal static Settings Load()
        {
            System.IO.Stream stream = null;
            if (System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly().FileExists(settingsxml))
                stream = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly().OpenFile(settingsxml, FileMode.Open);

            try
            {
                if (stream == null)
                    stream = new FileStream(settingsxml, FileMode.OpenOrCreate);
                var xs = new XmlSerializer(typeof(Settings));
                using (var fileStream = new StreamReader(stream))
                    return (Settings)xs.Deserialize(fileStream);
            }
            catch (Exception ex)
            {
                if (failed)
                    return new Settings();

                failed = true;
                if (System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly().FileExists(settingsxml))
                    System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly().DeleteFile(settingsxml);
                return Settings.Load();
            }
        }
    }
}
