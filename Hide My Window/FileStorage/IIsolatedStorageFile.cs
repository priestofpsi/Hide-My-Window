using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Microsoft.Win32;


namespace theDiary.Tools.HideMyWindow
{
    public interface IIsolatedStorageFile
    {
        event NotificationEventHandler Notification;

        string GetStorageFileName();

        void Save();

        void Reset();
    }
}
