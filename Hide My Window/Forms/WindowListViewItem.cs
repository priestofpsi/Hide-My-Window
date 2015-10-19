using System;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    public class WindowListViewItem
        : ListViewItem
    {
        public WindowListViewItem(WindowInfo window)
            : base()
        {
            this.Text = window.Title;
            this.Name = window.Key;
            this.ImageKey = window.Key;
            this.SubItems.Add(new ListViewSubItem(this, window.IsPasswordProtected ? "Yes" : "No")
            {
                Tag = window.IsPasswordProtected
            });
            this.SubItems.Add(new ListViewSubItem(this, window.IsPinned ? "Yes" : "No")
            {
                Tag = window.IsPinned
            });
            this.SubItems.Add(new ListViewSubItem()
            {
                Text = window.ApplicationPathName,
            });
            this.Tag = window.Handle;
        }

        public override int GetHashCode()
        {
            return this.Window.GetHashCode();
        }

        public WindowInfo Window
        {
            get
            {
                return Runtime.Instance.FindWindow((IntPtr)this.Tag);
            }
        }

        public static implicit operator WindowInfo(WindowListViewItem item)
        {
            return item.Window;
        }
    }
}