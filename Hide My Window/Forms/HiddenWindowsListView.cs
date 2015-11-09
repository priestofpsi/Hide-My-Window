using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    public class HiddenWindowsListView
        : ListView
    {
        #region Constructors

        public HiddenWindowsListView()
        {
            //this.LabelEdit = true;
            //this.OwnerDraw = true;
            this.DrawItem += this.HiddenWindowsListView_DrawItem;
            this.DrawColumnHeader += this.HiddenWindowsListView_DrawColumnHeader;
            this.DrawSubItem += this.HiddenWindowsListView_DrawSubItem;
        }

        #endregion

        #region Methods & Functions

        private void HiddenWindowsListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (e.ColumnIndex == 0)
                return;
            e.DrawDefault = true;
            WindowInfo currentItem = (e.Item as WindowListViewItem).Window;
            switch (e.ColumnIndex)
            {
                case 1:
                    e.SubItem.Text = currentItem.IsPasswordProtected ? "Yes" : "No";
                    break;

                case 2:
                    e.SubItem.Text = currentItem.IsPinned ? "Yes" : "No";
                    break;
            }
        }

        private void HiddenWindowsListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 1:
                    e.DrawBackground();
                    e.Graphics.AddImage(e.Bounds, ActionResource.lockwindow_small);
                    break;

                case 2:
                    e.DrawBackground();
                    e.Graphics.AddImage(e.Bounds, ActionResource.tack_small);
                    break;

                default:
                    e.DrawDefault = true;
                    break;
            }
        }

        private void HiddenWindowsListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            WindowInfo currentItem = (e.Item as WindowListViewItem).Window;
            switch (this.View)
            {
                case View.LargeIcon:
                    e.DrawDefault = false;
                    e.DrawBackground();
                    Rectangle itemBounds = new Rectangle(e.Bounds.Location, new Size(e.Bounds.Width, 65));
                    //if (e.Item.Selected)
                    //    e.Graphics.FillRectangle(SystemColors.Highlight.ToBrush(), e.Bounds);

                    Rectangle iconBounds = e.Graphics.AddImage(e.Bounds, currentItem.ApplicationIcon, null,
                        (e.Item.Selected) ? 16 : 18);
                    e.Graphics.AddImage(e.Bounds,
                        (currentItem.IsPasswordProtected)
                            ? ActionResource.lockwindow_small
                            : ActionResource.unlockwindow_small, ImageOverlayPosition.TopLeft);
                    if (currentItem.IsPinned)
                        e.Graphics.AddImage(e.Bounds, ActionResource.tack_small, ImageOverlayPosition.TopRight);
                    //e.DrawText(TextFormatFlags.Bottom | TextFormatFlags.EndEllipsis | TextFormatFlags.HorizontalCenter);
                    Rectangle rec = new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 2, e.Bounds.Width - 4,
                        e.Bounds.Height - 4);
                    TextRenderer.DrawText(e.Graphics, e.Item.Text, e.Item.Font, rec, e.Item.ForeColor,
                        TextFormatFlags.Bottom | TextFormatFlags.Left | TextFormatFlags.EndEllipsis |
                        TextFormatFlags.ExpandTabs | TextFormatFlags.SingleLine);
                    //e.DrawFocusRectangle();
                    break;

                default:
                    e.DrawDefault = true;
                    break;
            }
            if ((bool) e.Item.SubItems[1].Tag != currentItem.IsPasswordProtected)
            {
                e.Item.SubItems[1].Tag = currentItem.IsPasswordProtected;
                e.Item.SubItems[1].Text = currentItem.IsPasswordProtected ? "Yes" : "No";
            }
            if ((bool) e.Item.SubItems[2].Tag != currentItem.IsPinned)
            {
                e.Item.SubItems[2].Tag = currentItem.IsPinned;
                e.Item.SubItems[2].Text = currentItem.IsPinned ? "Yes" : "No";
            }
        }

        #endregion
    }
}