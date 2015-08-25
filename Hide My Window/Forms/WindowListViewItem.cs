using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            this.SubItems.Add(new ListViewSubItem()
            {
                Text = window.ApplicationPathName,
            });
            this.Tag = window;
            window.SetListViewItem(this);
        }
        
        public override int GetHashCode()
        {
            return this.Window.GetHashCode();
        }
        public WindowInfo Window
        {
            get
            {
                return this.Tag as WindowInfo;
            }
        }

        public static implicit operator WindowInfo(WindowListViewItem item)
        {
            return item.Window;
        }
    }
}
