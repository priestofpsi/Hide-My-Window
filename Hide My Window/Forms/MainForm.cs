using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.viewDetails.Tag = View.Details;
            this.viewLargeIcons.Tag = View.LargeIcon;
            this.viewSmallIcons.Tag = View.SmallIcon;
            this.viewTiles.Tag = View.Tile;
            this.viewList.Tag = View.List;
            this.hiddenWindows.View = Runtime.Instance.Settings.CurrentView;
            this.statusStrip1.Visible = Runtime.Instance.Settings.HideStatusbar;
            Runtime.Instance.Store.ForEach(item => this.AddHiddenWindow(new WindowInfo(new IntPtr(item))));
            ExternalReferences.MainHandle = this.Handle;
            ExternalReferences.RegisterAll();
            this.FormClosing += Form1_FormClosing;
            this.hiddenWindows.SelectedIndexChanged += (s, e) =>
            {
                this.show.Enabled = this.hiddenWindows.SelectedItems.Count != 0;
            };
            this.HiddenWindowsChanged += (s, e) =>
            {
                this.showAll.Enabled = this.hiddenWindows.Items.Count != 0;
                this.statusLabel.Text = string.Format("Hidden Windows: {0}", this.hiddenWindows.Items.Count);
            };
            this.VisibleChanged += (s, e) =>
            {
                this.notifyIcon.Visible = !this.Visible;
            };
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;
                this.notifyIcon.Visible = true;
            }
            else
            {
                if (MessageBox.Show(this, "Do you wish to exit Hide My Window?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
                this.UnhideAllWindows(this, EventArgs.Empty);
            }
        }

        public event EventHandler HiddenWindowsChanged;
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case ExternalReferences.WM_HOTKEY:
                    this.HotKeyPressed((short)m.WParam);
                    break;

                case 0x84:
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        private void AddHiddenWindow(WindowInfo window)
        {
            if (window.Equals(this.Handle) 
                || !window.Hide())
                return;
            WindowListViewItem item = new WindowListViewItem(window);
            this.imageListSmall.Images.Add(window.ApplicationId.ToString(), window.ApplicationIcon.ToBitmap());
            this.imageListBig.Images.Add(window.ApplicationId.ToString(), window.ApplicationIcon.ToBitmap());
            item.ImageKey = window.ApplicationId.ToString();
            this.hiddenWindows.Items.Add(item);
            Runtime.Instance.Store.Add(window.Handle);
            if (this.HiddenWindowsChanged != null)
                this.HiddenWindowsChanged(this, new EventArgs());
        }

        private void RemoveHiddenWindow(WindowInfo window)
        {
            window.Show();
            Runtime.Instance.Store.Remove(window.Handle);
            this.hiddenWindows.Items.Remove(window.GetListViewItem());
            if (this.HiddenWindowsChanged != null)
                this.HiddenWindowsChanged(this, new EventArgs());
        }
        private void HotKeyPressed(short id)
        {
            var hkid = Runtime.Instance.Settings.Hotkey.First(hkey => hkey.ID == id);

            switch (hkid.Function)
            {
                case HotkeyFunction.HideCurrentWindow:
                    WindowInfo currentWindow = ExternalReferences.GetActiveWindow();
                    this.AddHiddenWindow(currentWindow);
                    break;

                case HotkeyFunction.UnhideLastWindow:
                    if (this.hiddenWindows.Items.Count == 0)
                        return;

                    WindowInfo lastWindow = this.hiddenWindows.Items.Cast<WindowListViewItem>().Last();
                    this.RemoveHiddenWindow(lastWindow);
                    break;
            }
        }

        private void UnhideWindows(object sender, EventArgs e)
        {
            this.hiddenWindows.SelectedItems.Cast<WindowListViewItem>().ToList().ForEach(item => this.RemoveHiddenWindow(item));
        }

        internal void UnhideAllWindows(object sender, EventArgs e)
        {
            this.hiddenWindows.Items.Cast<WindowListViewItem>().ToList().ForEach(item => this.RemoveHiddenWindow(item));
        }

        private void ExitApplication(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.notifyIcon.Visible = false;
        }

        private void ToggleView(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            this.hiddenWindows.View = (View) item.Tag;
            Runtime.Instance.Settings.CurrentView = this.hiddenWindows.View;
            if (this.hiddenWindows.View == View.Details)
                this.hiddenWindows.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void viewToggle_DropDownOpening(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in this.viewToggle.DropDownItems)
                item.Checked = Object.Equals(item.Tag,this.hiddenWindows.View);
        }

        private void statusStrip1_VisibleChanged(object sender, EventArgs e)
        {
            Runtime.Instance.Settings.HideStatusbar = !this.statusStrip1.Visible;
        }

        private void statusbarToggle_Click(object sender, EventArgs e)
        {
            this.statusStrip1.Visible = this.statusbarToggle.Checked;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutBox about = new AboutBox())
                about.ShowDialog(this);
        }
    }
}
