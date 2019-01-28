namespace theDiary.Tools.HideMyWindow
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Security.Permissions;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class MainForm : Form
    {
        #region Constructors

        public MainForm()
        {            
            this.InitializeComponent();            
            this.InitializeFormHandlers();
            this.InitializeFormFromSettings();
            this.Icon = Runtime.Instance.Settings.ApplicationIcon;
            IsolatedStorageFileBase.FileNotification += (s, e) => this.Invoke(()=> this.labelNotifications.Text = e.EventText);
            IsolatedStorageFileBase.FileNotification += (s, e) => this.Invoke(() => this.labelNotifications.Text = e.EventText);
            Runtime.Instance.Settings.ApplicationIconChanged += (s, e) => this.Invoke(() =>
            {
                this.Icon = e.Icon;
                this.notifyIcon.Icon = e.Icon;
            });
            //Runtime.Instance.Store.Removed += (s, e) => this.HiddenWindowsChanged(s, EventArgs.Empty);
            this.Load += (s, e) => this.Invoke(() =>
            {               

                if (Runtime.Instance.Settings.StartInTaskBar)
                    this.MinimizeToTray();
                
            });
        }

        #endregion

        #region Declarations

        #region Private Declarations

        private FormInitState formInitializationState = FormInitState.NotInitialized;

        #endregion

        public FormInitState FormState
        {
            get
            {
                return this.formInitializationState;
            }
            set
            {
                if (this.formInitializationState == value)
                    return;
                this.formInitializationState = value;
                Runtime.Instance.RaiseFormStateChanged(this, new FormInitializeEventArgs(this.formInitializationState));
            }
        }
        #endregion

        #region Methods & Functions
        public event EventHandler<FormInitializeEventArgs> InitializeStateChanged;

        public event EventHandler HiddenWindowsChanged;

        public event NotificationEventHandler Notification;

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            bool passThrough = true;
            switch (m.Msg)
            {
                case GlobalHotKeyManager.HotKeyPressedMessageIdentifier:
                    this.HotKeyPressed((short)m.WParam, (WindowInfo.CurrentWindow == null) ? IntPtr.Zero : WindowInfo.CurrentWindow.Handle);
                    passThrough = false;
                    break;

                case GlobalHotKeyManager.WindowCommandMessageIdentifier:
                    int command = m.WParam.ToInt32() & 0xfff0;
                    if (command == GlobalHotKeyManager.MinimizeMessageIdentifier
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
            this.Invoke(() =>
            {
                this.statusLabel.Text = string.Format("{0} items selected",
                    this.hiddenWindows.SelectedItems.Count);
                bool hasSelectedItems = this.hiddenWindows.SelectedItems.Count > 0;
                this.hiddenWindows.ContextMenuStrip = (hasSelectedItems)
                    ? this.hiddenWindowsContextMenu
                    : null;
                this.hideWindow.Visible = hasSelectedItems
                                          && this.hiddenWindows.SelectedItems.Cast<WindowListViewItem>()
                                              .Any(item => item.Window.CanHide);
                this.showWindow.Visible = !this.hideWindow.Visible;
                this.showWindow.Enabled = hasSelectedItems;
                this.unlockWindow.Visible = Runtime.Instance.Settings.PasswordIsSet && hasSelectedItems
                                            && this.hiddenWindows.SelectedItems.Cast<WindowListViewItem>()
                                                .Any(item => item.Window.IsPasswordProtected);
                this.lockWindow.Visible = Runtime.Instance.Settings.PasswordIsSet && hasSelectedItems
                                          && this.hiddenWindows.SelectedItems.Cast<WindowListViewItem>()
                                              .Any(item => !item.Window.IsPasswordProtected);
                this.pinWindow.Enabled = hasSelectedItems;
                this.pinWindow.Text = (hasSelectedItems
                                       && this.hiddenWindows.SelectedItems.Cast<WindowListViewItem>()
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
                item.Window.Lock(
                    this.UnlockWindow);
                item.Update();
            });
            this.hiddenWindows_SelectedIndexChanged(sender, e);
        }

        private bool UnlockWindow(WindowInfo window)
        {
            using (UnlockForm password = new UnlockForm(window.Title))
                return (password.ShowDialog(this) == DialogResult.OK && password.IsMatched);
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
                    this.hideWindow.Image = ActionResource.hide_small;
                    this.pinWindow.Image = ActionResource.tack_small;
                    this.renameWindow.Image = ActionResource.rename_small;
                    break;

                case false:
                    this.unlockWindow.Image = ActionResource.unlockwindow;
                    this.lockWindow.Image = ActionResource.lockwindow;
                    this.showWindow.Image = ActionResource.show;
                    this.hideWindow.Image = ActionResource.hide;
                    this.pinWindow.Image = ActionResource.tack;
                    this.renameWindow.Image = ActionResource.rename;
                    break;
            }
        }

        private void SetToolbarText()
        {
            ToolStripItemDisplayStyle style = Runtime.Instance.Settings.HideToolbarText
                ? ToolStripItemDisplayStyle.Image
                : ToolStripItemDisplayStyle.ImageAndText;
            this.showWindow.DisplayStyle = style;
            this.hideWindow.DisplayStyle = style;
            this.unlockWindow.DisplayStyle = style;
            this.lockWindow.DisplayStyle = style;
            this.pinWindow.DisplayStyle = style;
            this.renameWindow.DisplayStyle = style;
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
            this.hideWindowToolStripMenuItem.Visible = this.hiddenWindows.SelectedItems.Count > 0
                                                       && this.hiddenWindows.SelectedItems.Cast<WindowListViewItem>()
                                                           .Any(item => item.Window.CanHide);
            this.showWindowToolStripMenuItem.Visible = this.hiddenWindows.SelectedItems.Count > 0
                                                       && this.hiddenWindows.SelectedItems.Cast<WindowListViewItem>()
                                                           .Any(item => item.Window.CanShow);
            this.protectToolStripMenuItem.Visible = Runtime.Instance.Settings.PasswordIsSet 
                                                    && this.hiddenWindows.SelectedItems.Count > 0
                                                    && this.hiddenWindows.SelectedItems.Cast<WindowListViewItem>()
                                                        .ToList()
                                                        .Any(item => !item.Window.IsPasswordProtected);
            this.unprotectToolStripMenuItem.Visible = this.hiddenWindows.SelectedItems.Count > 0
                                                      && this.hiddenWindows.SelectedItems.Cast<WindowListViewItem>()
                                                          .ToList()
                                                          .Any(item => item.Window.IsPasswordProtected);
            this.toolStripProtectSeperator.Visible = this.protectToolStripMenuItem.Visible || this.unprotectToolStripMenuItem.Visible;
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
                item.Window.TogglePinned();
                item.Update();
            });
            this.hiddenWindows_SelectedIndexChanged(sender, e);
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
                    && MessageBox.Show(this, "Do you wish to exit Hide My Window?", "Exit", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
                if (Runtime.Instance.Settings.RestoreWindowsOnExit && Runtime.Instance.Store.Count > 0)
                    WindowInfo.All.All(window => window.Show(true));

                Runtime.Instance.Settings.LastState = new FormState(this);
            }
        }

        private void ToggleHiddenWindows(object sender, EventArgs e)
        {
            Task task = Task.Run(() =>
            {
                Runtime.Instance.WriteDebug(nameof(ToggleHiddenWindows), string.Empty, e, "HiddenWindows");
                this.Invoke(() => this.hiddenWindows.SelectedItems.Cast<WindowListViewItem>().Select<WindowListViewItem, WindowInfo>(a => a.Window).ToList().ForEach(window => window.ToggleHidden()));

            }).ContinueWith(t => this.hiddenWindows_SelectedIndexChanged(sender, e));
        }

        internal void UnhideAllWindows(object sender, EventArgs e)
        {
            Task task = Task.Run(() =>
            {
                Runtime.Instance.WriteDebug(nameof(UnhideAllWindows), string.Empty, e, "HiddenWindows");
                this.Invoke(() => WindowInfo.All.ToList().ForEach(window => window.Show()));
                
            }).ContinueWith(t => this.hiddenWindows_SelectedIndexChanged(sender, e));
        }

        private void ExitApplication(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            bool canShow = !(Runtime.Instance.Settings.PasswordIsSet && Runtime.Instance.Settings.RequirePasswordOnShow);
            if (!canShow)
            {
                using (UnlockForm password = new UnlockForm("Hide My Window"))
                    canShow = (password.ShowDialog(this) == DialogResult.OK && password.IsMatched);
                if (!canShow)
                    this.Notification?.Invoke(this, new NotificationEventArgs("Incorrect Password", NotificationType.Error));
            }
            if (canShow)
                this.RestoreFromTray();
        }

        private void ToggleView(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            this.SetHiddenWindowsView((View)item.Tag);
        }

        private void SetHiddenWindowsView(View view)
        {
            Runtime.Instance.Settings.CurrentView = view;

            this.hiddenWindows.View = view;
            if (this.hiddenWindows.View == View.Details)
                this.hiddenWindows.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void viewToggle_DropDownOpening(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in this.viewToggle.DropDownItems)
                item.Checked = Equals(item.Tag, this.hiddenWindows.View);
        }

        private void statusbarToggle_Click(object sender, EventArgs e)
        {
            Runtime.Instance.Settings.HideStatusbar = !this.statusbarToggle.Checked;
            this.statusStrip1.Visible = Runtime.Instance.Settings.HideStatusbar;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutBox about = new AboutBox())
                about.ShowDialog(this);
        }

        private void openConfigurationForm_Click(object sender, EventArgs e)
        {
            using (ConfigurationForm form = new ConfigurationForm())
            {
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
        }

        private void InitializeFormHandlers()
        {
            if (this.FormState == FormInitState.Initialized)
                return;

            this.FormState = FormInitState.Initializing;
            GlobalHotKeyManager.RegisterAll(this.Handle);
            this.FormClosing += this.Form1_FormClosing;
            this.hiddenWindows.SelectedIndexChanged += (s, e) => this.Invoke(() => this.show.Enabled = this.hiddenWindows.SelectedItems.Count != 0);
            this.HiddenWindowsChanged += this.OnHiddenWindowsChanged;
            this.VisibleChanged += (s, e) => this.Invoke(() => this.notifyIcon.Visible = !this.Visible);
        }

        private void OnHiddenWindowsChanged(object sender, EventArgs e)
        {
            this.Invoke(() =>
            {
                this.showAll.Enabled = this.hiddenWindows.Items.Count != 0;
                this.ItemsCount.Text = string.Format("{0} items", this.hiddenWindows.Items.Count);
                if (this.hiddenWindows.View == View.Details)
                    this.hiddenWindows.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            });
        }

        private void InitializeFormFromSettings()
        {
            if (this.FormState == FormInitState.Initialized)
                return;
            this.SetHiddenWindowsView(Runtime.Instance.Settings.CurrentView);
            this.statusStrip1.Visible = !Runtime.Instance.Settings.HideStatusbar;
            this.statusStrip1.VisibleChanged +=
                (s, e) => Runtime.Instance.Settings.HideStatusbar = !this.statusStrip1.Visible;
            Runtime.Instance.Settings.LastState.SetFormState(this);

            this.Shown += (s, e) =>
            {
                Runtime.Instance.Store.All(item =>
                {
                    WindowInfo window;
                    if (WindowInfo.TryFind(item, out window) 
                        && Runtime.Instance.WindowManager.Register(item.Handle))
                    {
                        if (item.IsPasswordProtected)
                            window.Lock(this.UnlockWindow);
                        if (item.IsHidden)
                            window.Hide();
                        item.RegisterHandlers(window);
                    }
                });
                                
                this.SetToolbarIcons();
                this.SetToolbarText();

                this.hiddenWindows_SelectedIndexChanged(s, e);

                if (!Program.IsConfigured)
                    this.openConfigurationForm_Click(this, EventArgs.Empty);
            };
            Runtime.Instance.WindowManager.Registered += this.Store_Added;
            Runtime.Instance.WindowManager.UnRegistered += this.Store_Removed;
            Runtime.Instance.WindowManager.WindowHidden += (s,e)=> this.Notification?.Invoke(this, new NotificationEventArgs("Window Hidden", $"The window '{e.Window.Title}' has been hidden.", NotificationType.General));
            Runtime.Instance.WindowManager.WindowShown += (s, e) => this.Notification?.Invoke(this, new NotificationEventArgs("Window Restored", $"The window '{e.Window.Title}' has been restored.", NotificationType.General));

            Runtime.Instance.WindowManager.WindowPinned += (s, e) => this.Notification?.Invoke(this, new NotificationEventArgs("Window Pinned", $"The window '{e.Window.Title}' has been pinned.", NotificationType.Info));
            Runtime.Instance.WindowManager.WindowUnpinned += (s, e) => this.Notification?.Invoke(this, new NotificationEventArgs("Window Unpinned", $"The window '{e.Window.Title}' has been unpinned.", NotificationType.Info));
            this.FormState = FormInitState.Initialized;
        }


        private void SetWindowImageList(WindowInfo window, bool forceUpdate)
        {
            string key = window.Key;
            Bitmap image = window.ApplicationIcon.ToBitmap();
            if (forceUpdate)
            {
                if (this.imageListSmall.Images.ContainsKey(key))
                    this.imageListSmall.Images.RemoveByKey(key);
                if (this.imageListBig.Images.ContainsKey(key))
                    this.imageListBig.Images.RemoveByKey(key);
            }
            if (forceUpdate || !this.imageListSmall.Images.ContainsKey(key))
                this.imageListSmall.Images.Add(key, image);
            if (forceUpdate || !this.imageListBig.Images.ContainsKey(key))
                this.imageListBig.Images.Add(key, image);
        }

        private void Store_Removed(object sender, WindowInfoEventArgs e)
        {
            this.Invoke(() =>
            {
                if (!e.Window.IsPinned)
                    this.hiddenWindows.Items.RemoveByKey(e.Window.Key);
                this.HiddenWindowsChanged?.Invoke(this, new EventArgs());
            });
        }

        private void Store_Added(object sender, WindowInfoEventArgs e)
        {
            this.Invoke(() =>
            {
                WindowListViewItem item = new WindowListViewItem(e.Window);
                this.SetWindowImageList(e.Window, false);

                if (!this.hiddenWindows.Items.ContainsKey(e.Window.Key))
                {
                    this.hiddenWindows.Items.Add(item);
                    e.Window.ApplicationExited += this.RemoveClosedApplication;
                }
                this.HiddenWindowsChanged?.Invoke(this, new EventArgs());
            });
        }

        internal void EnableNotifications(bool attach)
        {
            if (attach)
            {
                this.Notification += this.ShowNotification;
            }
            else
            {
                this.Notification -= this.ShowNotification;
            }
        }

        private void ShowNotification(object sender, NotificationEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Message))
                return;

            this.Invoke(() =>
            {
                bool trayIconState = this.notifyIcon.Visible;
                if (!trayIconState)
                    this.notifyIcon.Visible = true;

                e.RaiseNotification(this.notifyIcon.ShowBalloonTip);
                Runtime.Instance.WriteDebug(nameof(ShowNotification), e.Message, "Notifications");
                if (!trayIconState)
                    this.notifyIcon.Visible = false;
            });
        }

        private void RemoveClosedApplication(object sender, WindowInfoEventArgs e)
        {
            this.Invoke(() =>
            {
                string key = e.Handle.ToInt64().ToString();
                if (!this.hiddenWindows.Items.ContainsKey(key))
                    return;

                WindowInfo.Find(e.Handle).ApplicationExited -= this.RemoveClosedApplication;
                this.hiddenWindows.Items.RemoveByKey(key);
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

        private void HotKeyPressed(short id, IntPtr currentWindowHandle)
        {
            HotKeyFunction functionCalled = Runtime.Instance.Settings.HotKeys.GetFunction(id);
            Runtime.Instance.WriteDebug(nameof(HotKeyPressed), nameof(functionCalled), functionCalled, "HotKeys");
            switch (functionCalled)
            {
                case HotKeyFunction.HideCurrentWindow:
                    if (!currentWindowHandle.Equals(IntPtr.Zero))
                        WindowInfo.Find(currentWindowHandle).Hide();
                    break;
                case HotKeyFunction.UnhideAllWindows:
                    this.UnhideAllWindows(this, new EventArgs());
                    break;
                case HotKeyFunction.ToggleLastWindow:
                    WindowInfo.Last?.ToggleHidden();
                    break;
                case HotKeyFunction.UnhideLastWindow:
                    Runtime.Instance.WindowManager.GetLastWindow()?.Show();
                    break;
                default:
                    break;
            }
        }

        private void notifyIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
            mi.Invoke(notifyIcon, null);
        }
        #endregion

    }

    

    public delegate bool UnlockWindowDelegate(WindowInfo window);
}