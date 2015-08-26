﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
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
        }
        #endregion

        #region Public Event Declarations
        public event EventHandler HiddenWindowsChanged;
        #endregion

        #region Protected Overriden Methods & Functions
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            bool passThrough = true;
            switch (m.Msg)
            {
                case ExternalReferences.WM_HOTKEY:
                    this.HotKeyPressed((short)m.WParam);
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
        #endregion

        #region Event Handlers
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
                    && MessageBox.Show(this, "Do you wish to exit Hide My Window?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
                if (Runtime.Instance.Settings.RestoreWindowsOnExit)
                    this.UnhideAllWindows(this, EventArgs.Empty);

                Runtime.Instance.Settings.LastState = new FormState(this);
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
            this.SetHiddenWindowsView((View)item.Tag);
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
                item.Checked = Object.Equals(item.Tag, this.hiddenWindows.View);
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
        #endregion

        #region Private Methods & Functions
        private void InitializeFormHandlers()
        {
            ExternalReferences.MainHandle = this.Handle;
            ExternalReferences.RegisterAll();
            this.FormClosing += this.Form1_FormClosing;
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

        private void InitializeFormFromSettings()
        {
            this.SetHiddenWindowsView(Runtime.Instance.Settings.CurrentView);
            this.statusStrip1.Visible = !Runtime.Instance.Settings.HideStatusbar;
            this.statusStrip1.VisibleChanged += (s, e) => Runtime.Instance.Settings.HideStatusbar = !this.statusStrip1.Visible;
            Runtime.Instance.Settings.LastState.SetFormState(this);
            if (Runtime.Instance.Settings.StartInTaskBar)
                this.Shown += (s, e) => this.MinimizeToTray();
            Runtime.Instance.Store.ForEach(item => this.AddHiddenWindow(new WindowInfo(item)));
        }

        private void MinimizeToTray()
        {
            this.Hide();
            this.notifyIcon.Visible = true;
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
        #endregion

    }
}