using System;
using System.Collections.Generic;
using System.Linq;

namespace theDiary.Tools.HideMyWindow
{
    public interface IIsolatedStorageFile
    {
        #region Methods & Functions
        event NotificationEventHandler Notification;

        string GetStorageFileName();

        void Save();

        void Reset();
        #endregion
    }
}