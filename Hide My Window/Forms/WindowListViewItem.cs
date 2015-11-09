using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    public class WindowListViewItem : ListViewItem
    {
        #region Public Constructors
        public WindowListViewItem(WindowInfo window)
        {
            this.Text = window.Title;
            this.Name = window.Key;
            this.ImageKey = window.Key;
            this.SubItems.Add(new ListViewSubItem(this, window.IsPasswordProtected ? "Yes" : "No")
                              {
                                  Tag = window.IsPasswordProtected,
                                  Name = "IsPasswordProtected"
                              });
            this.SubItems.Add(new ListViewSubItem(this, window.IsPinned ? "Yes" : "No")
                              {
                                  Tag = window.IsPinned,
                                  Name = "IsPinned"
                              });
            this.SubItems.Add(new ListViewSubItem
                              {
                                  Text = window.ApplicationPathName,
                                  Name = "ApplicationPathName"
                              });
            this.windowHandle = window.Handle;
            this.Tag = window.Handle;
        }
        #endregion

        #region Declarations
        private IntPtr windowHandle;
        #endregion

        #region Properties
        public IntPtr WindowHandle
        {
            get
            {
                if (this.windowHandle == IntPtr.Zero)
                    this.windowHandle = (IntPtr) this.Tag;

                return this.windowHandle;
            }
        }

        public WindowInfo Window
        {
            get
            {
                return WindowInfo.Find(this.WindowHandle);
            }
        }
        #endregion

        #region Methods & Functions
        public override int GetHashCode()
        {
            return this.Window.GetHashCode();
        }

        public void Update()
        {
            this.SubItems["IsPasswordProtected"].Text = this.Window.IsPasswordProtected ? "Yes" : "No";
            this.SubItems["IsPinned"].Text = this.Window.IsPinned ? "Yes" : "No";
            this.SubItems["ApplicationPathName"].Text = this.Window.ApplicationPathName;
        }

        public static implicit operator WindowInfo(WindowListViewItem item)
        {
            return item.Window;
        }
        #endregion
    }
}