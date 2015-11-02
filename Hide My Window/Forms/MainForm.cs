using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    public partial class MainForm : Form
    {
        #region Constructors

        public MainForm()
        {
            this.InitializeComponent();
            this.InitializeFormHandlers();
            this.InitializeFormFromSettings();
            this.Icon = Runtime.Instance.Settings.ApplicationIcon;
            Settings.FileNotification += (s, e) => this.labelNotifications.Text = e.EventText;
            HiddenWindowStore.FileNotification += (s, e) => this.labelNotifications.Text = e.EventText;
            Runtime.Instance.Settings.ApplicationIconChanged += (s, e) =>
            {
                this.Icon = e.Icon;
                this.notifyIcon.Icon = e.Icon;
            };
            Runtime.Instance.Store.Removed += (s, e) => this.HiddenWindowsChanged(s, EventArgs.Empty);
        }

        #endregion

        #region Declarations

        private FormInitState initializing = FormInitState.NotInitialized;

        #endregion

        #region Methods & Functions

        public event EventHandler HiddenWindowsChanged;

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            bool passThrough = true;
            switch (m.Msg)
            {
                case ExternalReferences.WM_HOTKEY:
                    this.HotKeyPressed((short) m.WParam);
                    passThrough = false;
                    break;

                case ExternalReferences.WM_SYSCOMMAND:
                    int command = m.WParam.ToInt32() & 0xfff0;
                    if (command == ExternalReferences.SC_MINIMIZE
                        && Runtime.Instance.Settings.MinimizeToTaskBar)
                    {
                        this.MinimizeToTray();
                        passThrough = false;
                    }
                    break;

                case 0x84:
                    passThrough = false;
                    break;
            }
            if (passThrough)
                base.WndProc(ref m);
        }

        private void hiddenWindows_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DoInvoke(() =>
            {
                this.statusLabel.Text = string.Format("{0} items selected", this.hiddenWindows.SelectedItems.Count);
                bool hasSelectedItems = this.hiddenWindows.SelectedItems.Count > 0;
                this.hiddenWindows.ContextMenuStrip = (hasSelectedItems) ? this.hiddenWindowsContextMenu : null;
                this.hideWindow.Visible = hasSelectedItems &&
                                          this.hiddenWindows.SelectedItems.Cast<WindowListViewItem>()
                                              .Any(item => item.Window.CanHide);
                this.showWindow.Visible = !this.hideWindow.Visible;
                this.showWindow.Enabled = hasSelectedItems;
                this.unlockWindow.Visible = Runtime.Instance.Settings.PasswordIsSet && hasSelectedItems &&
                                            this.hiddenWindows.SelectedItems.Cast<WindowListViewItem>()
                                                .Any(item => item.Window.IsPasswordProtected);
                this.lockWindow.Visible = Runtime.Instance.Settings.PasswordIsSet && hasSelectedItems &&
                                          this.hiddenWindows.SelectedItems.Cast<WindowListViewItem>()
                                              .Any(item => !item.Window.IsPasswordProtected);
                this.pinWindow.Enabled = hasSelectedItems;
                this.pinWindow.Text = (hasSelectedItems &&
                                       this.hiddenWindows.SelectedItems.Cast<WindowListViewItem>()
                                           .First()
                                           .Window.IsPinned)
                    ? "Unpin"
                    : "Pin";
                this.renameWindow.Enabled = hasSelectedItems;
                this.hiddenWindows.Invalidate();
            });
        }

        private void lockWindow_Click(object sender, EventArgs e)
        {
            this.hiddenWindows.SelectedItems.Cast<WindowListViewItem>().ToList().ForEach(item =>
            {
                item.Window.Lock(this.UnlockWindow);
                item.Update();
            });
            this.hiddenWindows_SelectedIndexChanged(sender, e);
        }

        private bool UnlockWindow(WindowInfo window)
        {
            using (UnlockForm password = new UnlockForm(window.Title))
            {
                return (password.ShowDialog(this) == DialogResult.OK && password.PasswordMatched);
            }
        }

        private void unlockWindow_Click(object sender, EventArgs e)
        {
            this.hiddenWindows.SelectedItems.Cast<WindowListViewItem>().ToList().ForEach(item => item.Window.Unlock());
            this.hiddenWindows_SelectedIndexChanged(sender, e);
        }

        private void smallIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Runtime.Instance.Settings.SmallToolbarIcons = true;
            this.SetToolbarIcons();
        }

        private void largeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Runtime.Instance.Settings.SmallToolbarIcons = false;
            this.SetToolbarIcons();
        }

        private void SetToolbarIcons()
        {
            switch (Runtime.Instance.Settings.SmallToolbarIcons)
            {
                case true:
                    this.unlockWindow.Image = ActionResource.unlockwindow_small;
                    this.lockWindow.Image = ActionResource.lockwindow_small;
                    this.showWindow.Image = ActionResource.show_small;
                    this.pinWindow.Image = ActionResource.tack_small;
                    this.renameWindow.Image = ActionResource.tack_small;
                    break;

                case false:
                    this.unlockWindow.Image = ActionResource.unlockwindow;
                    this.lockWindow.Image = ActionResource.lockwindow;
                    this.showWindow.Image = ActionResource.show;
                    this.renameWindow.Image = ActionResource.tack;
                    break;
            }
        }

        private void SetToolbarText()
        {
            ToolStripItemDisplayStyle style = Runtime.Instance.Settings.HideToolbarText
                ? ToolStripItemDisplayStyle.Image
                : ToolStripItemDisplayStyle.ImageAndText;
            this.showWindow.DisplayStyle = style;
            this.unlockWindow.DisplayStyle = style;
            this.lockWindow.DisplayStyle = style;
            this.pinWindow.DisplayStyle = style;
        }

        private void showToolbarText_Click(object sender, EventArgs e)
        {
            Runtime.Instance.Settings.HideToolbarText = !Runtime.Instance.Settings.HideToolbarText;
            this.SetToolbarText();
        }

        private void toolbarToggle_DropDownOpening(object sender, EventArgs e)
        {
            this.showToolbarText.Checked = !Runtime.Instance.Settings.HideToolbarText;
            this.largeToolbarIcons.Checked = !Runtime.Instance.Settings.SmallToolbarIcons;
            this.smallToolbarIcons.Checked = Runtime.Instance.Settings.SmallToolbarIcons;
        }

        private void hiddenWindowsContextMenu_Opening(object sender, CancelEventArgs e)
        {
            this.hideWindowToolStripMenuItem.Visible = this.hiddenWindows.SelectedItems.Count > 0 &&
                                                       this.hiddenWindows.SelectedItems.Cast<WindowListViewItem>()
                                                           .Any(item => item.Window.CanHide);
            this.showWindowToolStripMenuItem.Visible = this.hiddenWindows.SelectedItems.Count > 0 &&
                                                       this.hiddenWindows.SelectedItems.Cast<WindowListViewItem>()
                                                           .Any(item => item.Window.CanShow);
            this.protectToolStripMenuItem.Visible = this.hiddenWindows.SelectedItems.Count > 0 &&
                                                    this.hiddenWindows.SelectedItems.Cast<WindowListViewItem>()
                                                        .ToList()
                                                        .Any(item => !item.Window.IsPasswordProtected);
            this.unprotectToolStripMenuItem.Visible = this.hiddenWindows.SelectedItems.Count > 0 &&
                                                      this.hiddenWindows.SelectedItems.Cast<WindowListViewItem>()
                                                          .ToList()
                                                          .Any(item => item.Window.IsPasswordProtected);
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.hiddenWindows.SelectedItems.Cast<WindowListViewItem>().FirstOrDefault().BeginEdit();
        }

        private void hiddenWindows_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            (this.hiddenWindows.Items[e.Item] as WindowListViewItem).Window.Title = e.Label;
            //this.hiddenWindows.SelectedItems.Cast<WindowListViewItem>().FirstOrDefault().Window.Title = this.hiddenWindows.SelectedItems.Cast<WindowListViewItem>().FirstOrDefault().Text;
        }

        private void pinWindow_Click(object sender, EventArgs e)
        {
            this.hiddenWindows.SelectedItems.Cast<WindowListViewItem>().ToList().ForEach(item =>
            {
                item.Window.IsPinned = !item.Window.IsPinned;
                item.Update();
            });
            this.hiddenWindows_SelectedIndexChanged(sender, e);
        }

        private void hiddenWindows_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            Debug.WriteLine("On DrawSubItem");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing
                && Runtime.Instance.Settings.CloseToTaskBar)
            {
                this.MinimizeToTray();
                e.Cancel = true;
            }
            else
            {
                if (Runtime.Instance.Settings.ConfirmApplicationExit
                    &&
                    MessageBox.Show(this, "Do you wish to exit Hide My Window?", "Exit", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
                if (Runtime.Instance.Settings.RestoreWindowsOnExit)
                    this.UnhideAllWindows(this, EventArgs.Empty);

                Runtime.Instance.Settings.LastState = new FormState(this);
            }
        }

        private void ToggleHiddenWindows(object sender, EventArgs e)
        {
            Task task = Task.Run(() =>
            {
                this.DoInvoke(() =>
                {
                    this.hiddenWindows.SelectedItems.Cast<WindowListViewItem>()
                        .ToList()
                        .ForEach(item => item.Window.Toggle());
                });
                this.hiddenWindows_SelectedIndexChanged(sender, e);
            });
        }

        internal void UnhideAllWindows(object sender, EventArgs e)
        {
            Task task = Task.Run(() =>
            {
                this.hiddenWindows.Items.Cast<WindowListViewItem>()
                    .ToList()
                    .ForEach(item => item.Window.Show());
                this.hiddenWindows_SelectedIndexChanged(sender, e);
            });
        }

        private void ExitApplication(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            bool canShow = !Runtime.Instance.Settings.RequirePasswordOnShow;
            if (!canShow)

                using (UnlockForm password = new UnlockForm("Hide My Window"))
                {
                    canShow = (password.ShowDialog(this) == DialogResult.OK && password.PasswordMatched);
                }

            if (!canShow)
                return;
            this.RestoreFromTray();
        }

        private void ToggleView(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            this.SetHiddenWindowsView((View) item.Tag);
        }

        private void SetHiddenWindowsView(View view)
        {
            this.hiddenWindows.View = view;
            if (this.hiddenWindows.View == View.Details)
                this.hiddenWindows.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            Runtime.Instance.Settings.CurrentView = this.hiddenWindows.View;
        }

        private void viewToggle_DropDownOpening(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in this.viewToggle.DropDownItems)
                item.Checked = object.Equals(item.Tag, this.hiddenWindows.View);
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

        private void openConfigurationForm_Click(object sender, EventArgs e)
        {
            using (ConfigurationForm form = new ConfigurationForm())
                switch (form.ShowDialog(this))
                {
                    case DialogResult.OK:
                        Runtime.Instance.Settings.Save();
                        break;

                    default:
                        Runtime.Instance.Settings.Reset();
                        break;
                }
        }

        private void InitializeFormHandlers()
        {
            if (this.initializing == FormInitState.Initialized)
                return;

            this.initializing = FormInitState.Initializing;
            ExternalReferences.MainHandle = this.Handle;
            ExternalReferences.RegisterAll();
            this.FormClosing += this.Form1_FormClosing;
            this.hiddenWindows.SelectedIndexChanged +=
                (s, e) => { this.DoInvoke(() => this.show.Enabled = this.hiddenWindows.SelectedItems.Count != 0); };
            this.HiddenWindowsChanged += (s, e) =>
            {
                this.DoInvoke(() =>
                {
                    this.showAll.Enabled = this.hiddenWindows.Items.Count != 0;
                    this.ItemsCount.Text = string.Format("{0} items", this.hiddenWindows.Items.Count);
                    if (this.hiddenWindows.View == View.Details)
                        this.hiddenWindows.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                });
            };
            this.VisibleChanged += (s, e) => { this.DoInvoke(() => this.notifyIcon.Visible = !this.Visible); };
        }

        private void DoInvoke(Action action)
        {
            if (action == null)
                return;
            if (this.InvokeRequired)
            {
                this.Invoke(action);
                return;
            }
            action();
        }

        private void InitializeFormFromSettings()
        {
            if (this.initializing == FormInitState.Initialized)
                return;
            this.SetHiddenWindowsView(Runtime.Instance.Settings.CurrentView);
            this.statusStrip1.Visible = !Runtime.Instance.Settings.HideStatusbar;
            this.statusStrip1.VisibleChanged +=
                (s, e) => Runtime.Instance.Settings.HideStatusbar = !this.statusStrip1.Visible;
            Runtime.Instance.Settings.LastState.SetFormState(this);
            if (Runtime.Instance.Settings.StartInTaskBar)
                this.Shown += (s, e) => this.MinimizeToTray();

            this.Shown += (s, e) =>
            {
                Runtime.Instance.Store.All(item =>
                {
                    WindowInfo window = WindowInfo.Find(item);
                    if (window.IsValid && Runtime.Instance.WindowManager.Register(item.Handle))
                    {
                        if (item.IsPasswordProtected)
                            window.Lock(this.UnlockWindow);
                        if (item.IsHidden)
                            window.Hide();
                        item.RegisterHandlers(window);
                    }
                });
                this.hiddenWindows_SelectedIndexChanged(s, e);
                this.SetToolbarText();

                if (!Program.IsConfigured)
                    this.openConfigurationForm_Click(this, EventArgs.Empty);
            };
            Runtime.Instance.WindowManager.Registered += this.Store_Added;
            Runtime.Instance.WindowManager.UnRegistered += this.Store_Removed;
            this.initializing = FormInitState.Initialized;
        }

        private void Store_Removed(object sender, WindowInfoEventArgs e)
        {
            this.DoInvoke(() =>
            {
                if (!e.Window.IsPinned)
                    this.hiddenWindows.Items.RemoveByKey(e.Window.Key);

                if (this.HiddenWindowsChanged != null)
                    this.HiddenWindowsChanged(this, new EventArgs());
            });
        }

        private void Store_Added(object sender, WindowInfoEventArgs e)
        {
            this.DoInvoke(() =>
            {
                WindowListViewItem item = new WindowListViewItem(e.Window);
                if (!this.imageListSmall.Images.ContainsKey(e.Window.Key))
                    this.imageListSmall.Images.Add(e.Window.Key, e.Window.ApplicationIcon.ToBitmap());
                if (!this.imageListBig.Images.ContainsKey(e.Window.Key))
                    this.imageListBig.Images.Add(e.Window.Key, e.Window.ApplicationIcon.ToBitmap());

                if (!this.hiddenWindows.Items.ContainsKey(e.Window.Key))
                {
                    this.hiddenWindows.Items.Add(item);
                    e.Window.ApplicationExited += this.RemoveClosedApplication;
                }
                if (this.HiddenWindowsChanged != null)
                    this.HiddenWindowsChanged(this, new EventArgs());
            });
        }

        private void RemoveClosedApplication(object sender, WindowInfoEventArgs e)
        {
            this.DoInvoke(() =>
            {
                string key = e.Handle.ToInt64().ToString();
                if (this.hiddenWindows.Items.ContainsKey(key))
                {
                    WindowInfo.Find(e.Handle).ApplicationExited -= this.RemoveClosedApplication;
                    this.hiddenWindows.Items.RemoveByKey(key);
                }
            });
        }

        private void RestoreFromTray()
        {
            this.Show();
            this.notifyIcon.Visible = false;
        }

        private void MinimizeToTray()
        {
            this.Hide();
            this.notifyIcon.Visible = true;
        }

        private void HotKeyPressed(short id)
        {
            Hotkey hkid = Runtime.Instance.Settings.Hotkey.First(hkey => hkey.ID == id);

            switch (hkid.Function)
            {
                case HotkeyFunction.HideCurrentWindow:
                    WindowInfo currentWindow = WindowInfo.CurrentWindow();
                    currentWindow.Hide();
                    break;

                case HotkeyFunction.UnhideLastWindow:
                    WindowInfo lastWindow;
                    if (Runtime.Instance.WindowManager.TryGetLastWindow(out lastWindow))
                        lastWindow.Show();
                    break;
            }
        }

        #endregion
    }

    public enum FormInitState
    {
        NotInitialized,

        Initializing,

        Initialized
    }

    public delegate bool UnlockWindowDelegate(WindowInfo window);
}