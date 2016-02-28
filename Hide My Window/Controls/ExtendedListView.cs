using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    public class ExtendedListView : ListView
    {

        private ListViewItem.ListViewSubItem _clickedsubitem; //clicked ListViewSubItem
        private ListViewItem _clickeditem; //clicked ListViewItem
        private int _col; //index of doubleclicked ListViewSubItem
        private TextBox txtbx; //the default edit control
        private int _sortcol; //index of clicked ColumnHeader
        private Brush _sortcolbrush; //color of items in sorted column
        private Brush _highlightbrush; //color of highlighted items
        private int _cpadding; //padding of the embedded controls

        private const UInt32 LVM_FIRST = 0x1000;
        private const UInt32 LVM_SCROLL = (LVM_FIRST + 20);
        private const int WM_HSCROLL = 0x114;
        private const int WM_VSCROLL = 0x115;
        private const int WM_MOUSEWHEEL = 0x020A;
        private const int WM_PAINT = 0x000F;

        private struct EmbeddedControl
        {
            public Control MyControl;
            public ExtendedControlListViewSubItem MySubItem;
        }

        private ArrayList _controls;

        [DllImport("user32.dll")]
        private static extern bool SendMessage(IntPtr hWnd, UInt32 m, int wParam, int lParam);

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_PAINT)
            {
                foreach (EmbeddedControl c in _controls)
                {
                    Rectangle r = c.MySubItem.Bounds;
                    if (r.Y > 0 && r.Y < this.ClientRectangle.Height)
                    {
                        c.MyControl.Visible = true;
                        c.MyControl.Bounds = new Rectangle(r.X + _cpadding, r.Y + _cpadding, r.Width - (2 * _cpadding), r.Height - (2 * _cpadding));
                    }
                    else
                    {
                        c.MyControl.Visible = false;
                    }
                }
            }
            switch (m.Msg)
            {
                case WM_HSCROLL:
                case WM_VSCROLL:
                case WM_MOUSEWHEEL:
                    this.Focus();
                    break;
            }
            base.WndProc(ref m);
        }

        private void ScrollMe(int x, int y)
        {
            SendMessage((IntPtr)this.Handle, LVM_SCROLL, x, y);
        }

        public ExtendedListView()
        {
            _cpadding = 4;
            _controls = new ArrayList();
            _sortcol = -1;
            _sortcolbrush = SystemBrushes.ControlLight;
            _highlightbrush = SystemBrushes.Highlight;
            this.OwnerDraw = true;
            this.FullRowSelect = true;
            this.View = View.Details;
            this.MouseDown += new MouseEventHandler(this_MouseDown);
            this.MouseDoubleClick += new MouseEventHandler(this_MouseDoubleClick);
            this.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(this_DrawColumnHeader);
            this.DrawSubItem += new DrawListViewSubItemEventHandler(this_DrawSubItem);
            this.MouseMove += new MouseEventHandler(this_MouseMove);
            this.ColumnClick += new ColumnClickEventHandler(this_ColumnClick);
            txtbx = new TextBox();
            txtbx.Visible = false;
            this.Controls.Add(txtbx);
            txtbx.Leave += new EventHandler(c_Leave);
            txtbx.KeyPress += new KeyPressEventHandler(txtbx_KeyPress);
        }

        public void AddControlToSubItem(Control control, ExtendedControlListViewSubItem subitem)
        {
            this.Controls.Add(control);
            subitem.MyControl = control;
            EmbeddedControl ec;
            ec.MyControl = control;
            ec.MySubItem = subitem;
            this._controls.Add(ec);
        }

        public void RemoveControlFromSubItem(ExtendedControlListViewSubItem subitem)
        {
            Control c = subitem.MyControl;
            for (int i = 0; i < this._controls.Count; i++)
            {
                if (((EmbeddedControl)this._controls[i]).MySubItem == subitem)
                {
                    this._controls.RemoveAt(i);
                    subitem.MyControl = null;
                    this.Controls.Remove(c);
                    c.Dispose();
                    return;
                }
            }
        }

        public int ControlPadding
        {
            get
            {
                return _cpadding;
            }
            set
            {
                _cpadding = value;
            }
        }

        public Brush MySortBrush
        {
            get
            {
                return _sortcolbrush;
            }
            set
            {
                _sortcolbrush = value;
            }
        }

        public Brush MyHighlightBrush
        {
            get
            {
                return _highlightbrush;
            }
            set
            {
                _highlightbrush = value;
            }
        }

        private void txtbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                _clickedsubitem.Text = txtbx.Text;
                txtbx.Visible = false;
                _clickeditem.Tag = null;
            }
        }

        private void c_Leave(object sender, EventArgs e)
        {
            Control c = (Control)sender;
            _clickedsubitem.Text = c.Text;
            c.Visible = false;
            _clickeditem.Tag = null;
        }

        private void this_MouseDown(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo lstvinfo = this.HitTest(e.X, e.Y);
            ListViewItem.ListViewSubItem subitem = lstvinfo.SubItem;
            if (subitem == null)
                return;
            int subx = subitem.Bounds.Left;
            if (subx < 0)
            {
                this.ScrollMe(subx, 0);
            }
        }

        private void this_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ExtendedListViewItem lstvItem = this.GetItemAt(e.X, e.Y) as ExtendedListViewItem;
            if (lstvItem == null)
                return;
            _clickeditem = lstvItem;
            int x = lstvItem.Bounds.Left;
            int i;
            for (i = 0; i < this.Columns.Count; i++)
            {
                x = x + this.Columns[i].Width;
                if (x > e.X)
                {
                    x = x - this.Columns[i].Width;
                    _clickedsubitem = lstvItem.SubItems[i];
                    _col = i;
                    break;
                }
            }
            if (!(this.Columns[i] is ExtendedColumnHeader))
                return;
            ExtendedColumnHeader col = (ExtendedColumnHeader)this.Columns[i];
            if (col.GetType() == typeof(ExtendedEditableColumnHeader))
            {
                ExtendedEditableColumnHeader editcol = (ExtendedEditableColumnHeader)col;
                if (editcol.MyControl != null)
                {
                    Control c = editcol.MyControl;
                    if (c.Tag != null)
                    {
                        this.Controls.Add(c);
                        c.Tag = null;
                        if (c is ComboBox)
                        {
                            ((ComboBox)c).SelectedValueChanged += new EventHandler(cmbx_SelectedValueChanged);
                        }
                        c.Leave += new EventHandler(c_Leave);
                    }
                    c.Location = new Point(x, this.GetItemRect(this.Items.IndexOf(lstvItem)).Y);
                    c.Width = this.Columns[i].Width;
                    if (c.Width > this.Width)
                        c.Width = this.ClientRectangle.Width;
                    c.Text = _clickedsubitem.Text;
                    c.Visible = true;
                    c.BringToFront();
                    c.Focus();
                }
                else
                {
                    txtbx.Location = new Point(x, this.GetItemRect(this.Items.IndexOf(lstvItem)).Y);
                    txtbx.Width = this.Columns[i].Width;
                    if (txtbx.Width > this.Width)
                        txtbx.Width = this.ClientRectangle.Width;
                    txtbx.Text = _clickedsubitem.Text;
                    txtbx.Visible = true;
                    txtbx.BringToFront();
                    txtbx.Focus();
                }
            }
            else if (col.GetType() == typeof(ExtendedBoolColumnHeader))
            {
                ExtendedBoolColumnHeader boolcol = (ExtendedBoolColumnHeader)col;
                if (boolcol.Editable)
                {
                    ExtendedBoolListViewSubItem boolsubitem = (ExtendedBoolListViewSubItem)_clickedsubitem;
                    if (boolsubitem.BoolValue == true)
                    {
                        boolsubitem.BoolValue = false;
                    }
                    else
                    {
                        boolsubitem.BoolValue = true;
                    }
                    this.Invalidate(boolsubitem.Bounds);
                }
            }
        }

        private void cmbx_SelectedValueChanged(object sender, EventArgs e)
        {
            if (((Control)sender).Visible == false || _clickedsubitem == null)
                return;
            if (sender.GetType() == typeof(ExtendedComboBox))
            {
                ExtendedComboBox excmbx = (ExtendedComboBox)sender;
                object item = excmbx.SelectedItem;
                //Is this an combobox item with one image?
                if (item.GetType() == typeof(ExtendedComboBox.EXImageItem))
                {
                    ExtendedComboBox.EXImageItem imgitem = (ExtendedComboBox.EXImageItem)item;
                    //Is the first column clicked -- in that case it's a ListViewItem
                    if (_col == 0)
                    {
                        if (_clickeditem.GetType() == typeof(ExtendedImageListViewItem))
                        {
                            ((ExtendedImageListViewItem)_clickeditem).MyImage = imgitem.MyImage;
                        }
                        else if (_clickeditem.GetType() == typeof(ExtendedMultipleImagesListViewItem))
                        {
                            ExtendedMultipleImagesListViewItem imglstvitem = (ExtendedMultipleImagesListViewItem)_clickeditem;
                            imglstvitem.MyImages.Clear();
                            imglstvitem.MyImages.AddRange(new object[] { imgitem.MyImage });
                        }
                        //another column than the first one is clicked, so we have a ListViewSubItem
                    }
                    else
                    {
                        if (_clickedsubitem.GetType() == typeof(ExtendedImageListViewSubItem))
                        {
                            ExtendedImageListViewSubItem imgsub = (ExtendedImageListViewSubItem)_clickedsubitem;
                            imgsub.MyImage = imgitem.MyImage;
                        }
                        else if (_clickedsubitem.GetType() == typeof(ExtendedMultipleImagesListViewSubItem))
                        {
                            ExtendedMultipleImagesListViewSubItem imgsub = (ExtendedMultipleImagesListViewSubItem)_clickedsubitem;
                            imgsub.MyImages.Clear();
                            imgsub.MyImages.Add(imgitem.MyImage);
                            imgsub.MyValue = imgitem.MyValue;
                        }
                    }
                    //or is this a combobox item with multiple images?
                }
                else if (item.GetType() == typeof(ExtendedComboBox.EXMultipleImagesItem))
                {
                    ExtendedComboBox.EXMultipleImagesItem imgitem = (ExtendedComboBox.EXMultipleImagesItem)item;
                    if (_col == 0)
                    {
                        if (_clickeditem.GetType() == typeof(ExtendedImageListViewItem))
                        {
                            ((ExtendedImageListViewItem)_clickeditem).MyImage = (Image)imgitem.MyImages[0];
                        }
                        else if (_clickeditem.GetType() == typeof(ExtendedMultipleImagesListViewItem))
                        {
                            ExtendedMultipleImagesListViewItem imglstvitem = (ExtendedMultipleImagesListViewItem)_clickeditem;
                            imglstvitem.MyImages.Clear();
                            imglstvitem.MyImages.AddRange(imgitem.MyImages);
                        }
                    }
                    else
                    {
                        if (_clickedsubitem.GetType() == typeof(ExtendedImageListViewSubItem))
                        {
                            ExtendedImageListViewSubItem imgsub = (ExtendedImageListViewSubItem)_clickedsubitem;
                            if (imgitem.MyImages != null)
                            {
                                imgsub.MyImage = (Image)imgitem.MyImages[0];
                            }
                        }
                        else if (_clickedsubitem.GetType() == typeof(ExtendedMultipleImagesListViewSubItem))
                        {
                            ExtendedMultipleImagesListViewSubItem imgsub = (ExtendedMultipleImagesListViewSubItem)_clickedsubitem;
                            imgsub.MyImages.Clear();
                            imgsub.MyImages.AddRange(imgitem.MyImages);
                            imgsub.MyValue = imgitem.MyValue;
                        }
                    }
                }
            }
            ComboBox c = (ComboBox)sender;
            _clickedsubitem.Text = c.Text;
            c.Visible = false;
            _clickeditem.Tag = null;
        }

        private void this_MouseMove(object sender, MouseEventArgs e)
        {
            ListViewItem item = this.GetItemAt(e.X, e.Y);
            if (item != null && item.Tag == null)
            {
                this.Invalidate(item.Bounds);
                item.Tag = "t";
            }
        }

        private void this_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void this_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawBackground();
            if (e.ColumnIndex == _sortcol)
            {
                e.Graphics.FillRectangle(_sortcolbrush, e.Bounds);
            }
            if ((e.ItemState & ListViewItemStates.Selected) != 0)
            {
                e.Graphics.FillRectangle(_highlightbrush, e.Bounds);
            }
            int fonty = e.Bounds.Y + ((int)(e.Bounds.Height / 2)) - ((int)(e.SubItem.Font.Height / 2));
            int x = e.Bounds.X + 2;
            if (e.ColumnIndex == 0)
            {
                ExtendedListViewItem item = (ExtendedListViewItem)e.Item;
                if (item.GetType() == typeof(ExtendedImageListViewItem))
                {
                    ExtendedImageListViewItem imageitem = (ExtendedImageListViewItem)item;
                    if (imageitem.MyImage != null)
                    {
                        Image img = imageitem.MyImage;
                        int imgy = e.Bounds.Y + ((int)(e.Bounds.Height / 2)) - ((int)(img.Height / 2));
                        e.Graphics.DrawImage(img, x, imgy, img.Width, img.Height);
                        x += img.Width + 2;
                    }
                }
                e.Graphics.DrawString(e.SubItem.Text, e.SubItem.Font, new SolidBrush(e.SubItem.ForeColor), x, fonty);
                return;
            }
            ExtendedListViewSubItemAB subitem = e.SubItem as ExtendedListViewSubItemAB;
            if (subitem == null)
            {
                e.DrawDefault = true;
            }
            else
            {
                x = subitem.DoDraw(e, x, this.Columns[e.ColumnIndex] as ExtendedColumnHeader);
                e.Graphics.DrawString(e.SubItem.Text, e.SubItem.Font, new SolidBrush(e.SubItem.ForeColor), x, fonty);
            }
        }

        private void this_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (this.Items.Count == 0)
                return;
            for (int i = 0; i < this.Columns.Count; i++)
            {
                this.Columns[i].ImageKey = null;
            }
            for (int i = 0; i < this.Items.Count; i++)
            {
                this.Items[i].Tag = null;
            }
            if (e.Column != _sortcol)
            {
                _sortcol = e.Column;
                this.Sorting = SortOrder.Ascending;
                this.Columns[e.Column].ImageKey = "up";
            }
            else
            {
                if (this.Sorting == SortOrder.Ascending)
                {
                    this.Sorting = SortOrder.Descending;
                    this.Columns[e.Column].ImageKey = "down";
                }
                else
                {
                    this.Sorting = SortOrder.Ascending;
                    this.Columns[e.Column].ImageKey = "up";
                }
            }
            if (_sortcol == 0)
            {
                //ListViewItem
                if (this.Items[0].GetType() == typeof(ExtendedListViewItem))
                {
                    //sorting on text
                    this.ListViewItemSorter = new ListViewItemComparerText(e.Column, this.Sorting);
                }
                else
                {
                    //sorting on value
                    this.ListViewItemSorter = new ListViewItemComparerValue(e.Column, this.Sorting);
                }
            }
            else
            {
                //ListViewSubItem
                if (this.Items[0].SubItems[_sortcol].GetType() == typeof(ExtendedListViewSubItemAB))
                {
                    //sorting on text
                    this.ListViewItemSorter = new ListViewSubItemComparerText(e.Column, this.Sorting);
                }
                else
                {
                    //sorting on value
                    this.ListViewItemSorter = new ListViewSubItemComparerValue(e.Column, this.Sorting);
                }
            }
        }

        class ListViewSubItemComparerText : System.Collections.IComparer
        {

            private int _col;
            private SortOrder _order;

            public ListViewSubItemComparerText()
            {
                _col = 0;
                _order = SortOrder.Ascending;
            }

            public ListViewSubItemComparerText(int col, SortOrder order)
            {
                _col = col;
                _order = order;
            }

            public int Compare(object x, object y)
            {
                int returnVal = -1;

                string xstr = ((ListViewItem)x).SubItems[_col].Text;
                string ystr = ((ListViewItem)y).SubItems[_col].Text;

                decimal dec_x;
                decimal dec_y;
                DateTime dat_x;
                DateTime dat_y;

                if (Decimal.TryParse(xstr, out dec_x) && Decimal.TryParse(ystr, out dec_y))
                {
                    returnVal = Decimal.Compare(dec_x, dec_y);
                }
                else if (DateTime.TryParse(xstr, out dat_x) && DateTime.TryParse(ystr, out dat_y))
                {
                    returnVal = DateTime.Compare(dat_x, dat_y);
                }
                else
                {
                    returnVal = String.Compare(xstr, ystr);
                }
                if (_order == SortOrder.Descending)
                    returnVal *= -1;
                return returnVal;
            }

        }

        class ListViewSubItemComparerValue : System.Collections.IComparer
        {

            private int _col;
            private SortOrder _order;

            public ListViewSubItemComparerValue()
            {
                _col = 0;
                _order = SortOrder.Ascending;
            }

            public ListViewSubItemComparerValue(int col, SortOrder order)
            {
                _col = col;
                _order = order;
            }

            public int Compare(object x, object y)
            {
                int returnVal = -1;

                string xstr = ((ExtendedListViewSubItemAB)((ListViewItem)x).SubItems[_col]).MyValue;
                string ystr = ((ExtendedListViewSubItemAB)((ListViewItem)y).SubItems[_col]).MyValue;

                decimal dec_x;
                decimal dec_y;
                DateTime dat_x;
                DateTime dat_y;

                if (Decimal.TryParse(xstr, out dec_x) && Decimal.TryParse(ystr, out dec_y))
                {
                    returnVal = Decimal.Compare(dec_x, dec_y);
                }
                else if (DateTime.TryParse(xstr, out dat_x) && DateTime.TryParse(ystr, out dat_y))
                {
                    returnVal = DateTime.Compare(dat_x, dat_y);
                }
                else
                {
                    returnVal = String.Compare(xstr, ystr);
                }
                if (_order == SortOrder.Descending)
                    returnVal *= -1;
                return returnVal;
            }

        }

        class ListViewItemComparerText : System.Collections.IComparer
        {

            private int _col;
            private SortOrder _order;

            public ListViewItemComparerText()
            {
                _col = 0;
                _order = SortOrder.Ascending;
            }

            public ListViewItemComparerText(int col, SortOrder order)
            {
                _col = col;
                _order = order;
            }

            public int Compare(object x, object y)
            {
                int returnVal = -1;

                string xstr = ((ListViewItem)x).Text;
                string ystr = ((ListViewItem)y).Text;

                decimal dec_x;
                decimal dec_y;
                DateTime dat_x;
                DateTime dat_y;

                if (Decimal.TryParse(xstr, out dec_x) && Decimal.TryParse(ystr, out dec_y))
                {
                    returnVal = Decimal.Compare(dec_x, dec_y);
                }
                else if (DateTime.TryParse(xstr, out dat_x) && DateTime.TryParse(ystr, out dat_y))
                {
                    returnVal = DateTime.Compare(dat_x, dat_y);
                }
                else
                {
                    returnVal = String.Compare(xstr, ystr);
                }
                if (_order == SortOrder.Descending)
                    returnVal *= -1;
                return returnVal;
            }

        }

        class ListViewItemComparerValue : System.Collections.IComparer
        {

            private int _col;
            private SortOrder _order;

            public ListViewItemComparerValue()
            {
                _col = 0;
                _order = SortOrder.Ascending;
            }

            public ListViewItemComparerValue(int col, SortOrder order)
            {
                _col = col;
                _order = order;
            }

            public int Compare(object x, object y)
            {
                int returnVal = -1;

                string xstr = ((ExtendedListViewItem)x).MyValue;
                string ystr = ((ExtendedListViewItem)y).MyValue;

                decimal dec_x;
                decimal dec_y;
                DateTime dat_x;
                DateTime dat_y;

                if (Decimal.TryParse(xstr, out dec_x) && Decimal.TryParse(ystr, out dec_y))
                {
                    returnVal = Decimal.Compare(dec_x, dec_y);
                }
                else if (DateTime.TryParse(xstr, out dat_x) && DateTime.TryParse(ystr, out dat_y))
                {
                    returnVal = DateTime.Compare(dat_x, dat_y);
                }
                else
                {
                    returnVal = String.Compare(xstr, ystr);
                }
                if (_order == SortOrder.Descending)
                    returnVal *= -1;
                return returnVal;
            }

        }

    }

    public class ExtendedColumnHeader : ColumnHeader
    {

        public ExtendedColumnHeader()
        {

        }

        public ExtendedColumnHeader(string text)
        {
            this.Text = text;
        }

        public ExtendedColumnHeader(string text, int width)
        {
            this.Text = text;
            this.Width = width;
        }

    }

    public class ExtendedEditableColumnHeader : ExtendedColumnHeader
    {

        private Control _control;

        public ExtendedEditableColumnHeader()
        {

        }

        public ExtendedEditableColumnHeader(string text)
        {
            this.Text = text;
        }

        public ExtendedEditableColumnHeader(string text, int width)
        {
            this.Text = text;
            this.Width = width;
        }

        public ExtendedEditableColumnHeader(string text, Control control)
        {
            this.Text = text;
            this.MyControl = control;
        }

        public ExtendedEditableColumnHeader(string text, Control control, int width)
        {
            this.Text = text;
            this.MyControl = control;
            this.Width = width;
        }

        public Control MyControl
        {
            get
            {
                return _control;
            }
            set
            {
                _control = value;
                _control.Visible = false;
                _control.Tag = "not_init";
            }
        }

    }

    public class ExtendedBoolColumnHeader : ExtendedColumnHeader
    {

        private Image _trueimage;
        private Image _falseimage;
        private bool _editable;

        public ExtendedBoolColumnHeader()
        {
            init();
        }

        public ExtendedBoolColumnHeader(string text)
        {
            init();
            this.Text = text;
        }

        public ExtendedBoolColumnHeader(string text, int width)
        {
            init();
            this.Text = text;
            this.Width = width;
        }

        public ExtendedBoolColumnHeader(string text, Image trueimage, Image falseimage)
        {
            init();
            this.Text = text;
            _trueimage = trueimage;
            _falseimage = falseimage;
        }

        public ExtendedBoolColumnHeader(string text, Image trueimage, Image falseimage, int width)
        {
            init();
            this.Text = text;
            _trueimage = trueimage;
            _falseimage = falseimage;
            this.Width = width;
        }

        private void init()
        {
            _editable = false;
        }

        public Image TrueImage
        {
            get
            {
                return _trueimage;
            }
            set
            {
                _trueimage = value;
            }
        }

        public Image FalseImage
        {
            get
            {
                return _falseimage;
            }
            set
            {
                _falseimage = value;
            }
        }

        public bool Editable
        {
            get
            {
                return _editable;
            }
            set
            {
                _editable = value;
            }
        }

    }

    public abstract class ExtendedListViewSubItemAB : ListViewItem.ListViewSubItem
    {

        private string _value = "";

        public ExtendedListViewSubItemAB()
        {

        }

        public ExtendedListViewSubItemAB(string text)
        {
            this.Text = text;
        }

        public string MyValue
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        //return the new x coordinate
        public abstract int DoDraw(DrawListViewSubItemEventArgs e, int x, ExtendedColumnHeader ch);

    }

    public class ExtendedListViewSubItem : ExtendedListViewSubItemAB
    {

        public ExtendedListViewSubItem()
        {

        }

        public ExtendedListViewSubItem(string text)
        {
            this.Text = text;
        }

        public override int DoDraw(DrawListViewSubItemEventArgs e, int x, ExtendedColumnHeader ch)
        {
            return x;
        }

    }

    public class ExtendedControlListViewSubItem : ExtendedListViewSubItemAB
    {

        private Control _control;

        public ExtendedControlListViewSubItem()
        {

        }

        public Control MyControl
        {
            get
            {
                return _control;
            }
            set
            {
                _control = value;
            }
        }

        public override int DoDraw(DrawListViewSubItemEventArgs e, int x, ExtendedColumnHeader ch)
        {
            return x;
        }

    }

    public class ExtendedImageListViewSubItem : ExtendedListViewSubItemAB
    {

        private Image _image;

        public ExtendedImageListViewSubItem()
        {

        }

        public ExtendedImageListViewSubItem(string text)
        {
            this.Text = text;
        }

        public ExtendedImageListViewSubItem(Image image)
        {
            _image = image;
        }

        public ExtendedImageListViewSubItem(Image image, string value)
        {
            _image = image;
            this.MyValue = value;
        }

        public ExtendedImageListViewSubItem(string text, Image image, string value)
        {
            this.Text = text;
            _image = image;
            this.MyValue = value;
        }

        public Image MyImage
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
            }
        }

        public override int DoDraw(DrawListViewSubItemEventArgs e, int x, ExtendedColumnHeader ch)
        {
            if (this.MyImage != null)
            {
                Image img = this.MyImage;
                int imgy = e.Bounds.Y + ((int)(e.Bounds.Height / 2)) - ((int)(img.Height / 2));
                e.Graphics.DrawImage(img, x, imgy, img.Width, img.Height);
                x += img.Width + 2;
            }
            return x;
        }

    }

    public class ExtendedMultipleImagesListViewSubItem : ExtendedListViewSubItemAB
    {

        private ArrayList _images;

        public ExtendedMultipleImagesListViewSubItem()
        {

        }

        public ExtendedMultipleImagesListViewSubItem(string text)
        {
            this.Text = text;
        }

        public ExtendedMultipleImagesListViewSubItem(ArrayList images)
        {
            _images = images;
        }

        public ExtendedMultipleImagesListViewSubItem(ArrayList images, string value)
        {
            _images = images;
            this.MyValue = value;
        }

        public ExtendedMultipleImagesListViewSubItem(string text, ArrayList images, string value)
        {
            this.Text = text;
            _images = images;
            this.MyValue = value;
        }

        public ArrayList MyImages
        {
            get
            {
                return _images;
            }
            set
            {
                _images = value;
            }
        }

        public override int DoDraw(DrawListViewSubItemEventArgs e, int x, ExtendedColumnHeader ch)
        {
            if (this.MyImages != null && this.MyImages.Count > 0)
            {
                for (int i = 0; i < this.MyImages.Count; i++)
                {
                    Image img = (Image)this.MyImages[i];
                    int imgy = e.Bounds.Y + ((int)(e.Bounds.Height / 2)) - ((int)(img.Height / 2));
                    e.Graphics.DrawImage(img, x, imgy, img.Width, img.Height);
                    x += img.Width + 2;
                }
            }
            return x;
        }

    }

    public class ExtendedBoolListViewSubItem : ExtendedListViewSubItemAB
    {

        private bool _value;

        public ExtendedBoolListViewSubItem()
        {

        }

        public ExtendedBoolListViewSubItem(bool val)
        {
            _value = val;
            this.MyValue = val.ToString();
        }

        public bool BoolValue
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                this.MyValue = value.ToString();
            }
        }

        public override int DoDraw(DrawListViewSubItemEventArgs e, int x, ExtendedColumnHeader ch)
        {
            ExtendedBoolColumnHeader boolcol = (ExtendedBoolColumnHeader)ch;
            Image boolimg;
            if (this.BoolValue == true)
            {
                boolimg = boolcol.TrueImage;
            }
            else
            {
                boolimg = boolcol.FalseImage;
            }
            int imgy = e.Bounds.Y + ((int)(e.Bounds.Height / 2)) - ((int)(boolimg.Height / 2));
            e.Graphics.DrawImage(boolimg, x, imgy, boolimg.Width, boolimg.Height);
            x += boolimg.Width + 2;
            return x;
        }

    }

    public class ExtendedListViewItem : ListViewItem
    {

        private string _value;

        public ExtendedListViewItem()
        {

        }

        public ExtendedListViewItem(string text)
        {
            this.Text = text;
        }

        public string MyValue
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

    }

    public class ExtendedImageListViewItem : ExtendedListViewItem
    {

        private Image _image;

        public ExtendedImageListViewItem()
        {

        }

        public ExtendedImageListViewItem(string text)
        {
            this.Text = text;
        }

        public ExtendedImageListViewItem(Image image)
        {
            _image = image;
        }

        public ExtendedImageListViewItem(string text, Image image)
        {
            _image = image;
            this.Text = text;
        }

        public ExtendedImageListViewItem(string text, Image image, string value)
        {
            this.Text = text;
            _image = image;
            this.MyValue = value;
        }

        public Image MyImage
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
            }
        }

    }

    public class ExtendedMultipleImagesListViewItem : ExtendedListViewItem
    {

        private ArrayList _images;

        public ExtendedMultipleImagesListViewItem()
        {

        }

        public ExtendedMultipleImagesListViewItem(string text)
        {
            this.Text = text;
        }

        public ExtendedMultipleImagesListViewItem(ArrayList images)
        {
            _images = images;
        }

        public ExtendedMultipleImagesListViewItem(string text, ArrayList images)
        {
            this.Text = text;
            _images = images;
        }

        public ExtendedMultipleImagesListViewItem(string text, ArrayList images, string value)
        {
            this.Text = text;
            _images = images;
            this.MyValue = value;
        }

        public ArrayList MyImages
        {
            get
            {
                return _images;
            }
            set
            {
                _images = value;
            }
        }

    }

}