﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    public partial class ConfigurationForm : Form
    {
        #region Constructors

        public ConfigurationForm()
        {
            this.InitializeComponent();
            ExternalReferences.UnregisterAll();
            this.minimizeToTaskbar.DataBindings.Add(new Binding("Checked", Runtime.Instance.Settings,
                "MinimizeToTaskBar"));
            this.closeToTaskbar.DataBindings.Add(new Binding("Checked", Runtime.Instance.Settings, "CloseToTaskBar"));
            this.restoreWindowsOnExit.DataBindings.Add(new Binding("Checked", Runtime.Instance.Settings,
                "RestoreWindowsOnExit"));
            this.confirmWhenExiting.DataBindings.Add(new Binding("Checked", Runtime.Instance.Settings,
                "ConfirmApplicationExit"));
            this.startInTaskbar.DataBindings.Add(new Binding("Checked", Runtime.Instance.Settings, "StartInTaskBar"));
            this.requirePasswordOnShow.DataBindings.Add(new Binding("Checked", Runtime.Instance.Settings,
                "RequirePasswordOnShow"));
            this.requirePasswordOnShow.DataBindings.Add(new Binding("Enabled", Runtime.Instance.Settings,
                "PasswordIsSet"));
            this.clearPassword.CheckedChanged += (s, e) => this.glowPanel1.Enabled = !this.clearPassword.Checked;
            this.startWithWindows.DataBindings.Add(new Binding("Checked", Runtime.Instance.Settings,
                "AutoStartWithWindows"));
            this.hotKeyMimicTextBox1.HotKey =
                Runtime.Instance.Settings.GetHotKeyByFunction(HotkeyFunction.HideCurrentWindow);
            this.hotKeyMimicTextBox2.HotKey =
                Runtime.Instance.Settings.GetHotKeyByFunction(HotkeyFunction.UnhideLastWindow);
            this.hotKeyMimicTextBox3.HotKey =
                Runtime.Instance.Settings.GetHotKeyByFunction(HotkeyFunction.HideAllWindows);
            this.hotKeyMimicTextBox4.HotKey =
                Runtime.Instance.Settings.GetHotKeyByFunction(HotkeyFunction.UnhideAllWindows);
            this.hotKeyMimicTextBox1.HotKeyChanged += (s, e) =>
            {
                if (!Runtime.Instance.Settings.Hotkey.Contains(this.hotKeyMimicTextBox1.HotKey))
                {
                    Runtime.Instance.Settings.Hotkey.Add(this.hotKeyMimicTextBox1.HotKey);
                }
                else
                {
                    Runtime.Instance.Settings.Hotkey[0] = this.hotKeyMimicTextBox1.HotKey;
                }
                this.hotkeysChanged = true;
            };
            this.hotKeyMimicTextBox2.HotKeyChanged += (s, e) =>
            {
                if (!Runtime.Instance.Settings.Hotkey.Contains(this.hotKeyMimicTextBox2.HotKey))
                {
                    Runtime.Instance.Settings.Hotkey.Add(this.hotKeyMimicTextBox2.HotKey);
                }
                else
                {
                    Runtime.Instance.Settings.Hotkey[1] = this.hotKeyMimicTextBox2.HotKey;
                }
                this.hotkeysChanged = true;
            };
            this.hotKeyMimicTextBox3.HotKeyChanged += (s, e) =>
            {
                if (!Runtime.Instance.Settings.Hotkey.Contains(this.hotKeyMimicTextBox3.HotKey))
                {
                    Runtime.Instance.Settings.Hotkey.Add(this.hotKeyMimicTextBox3.HotKey);
                }
                else
                {
                    Runtime.Instance.Settings.Hotkey[0] = this.hotKeyMimicTextBox3.HotKey;
                }
                this.hotkeysChanged = true;
            };
            this.hotKeyMimicTextBox4.HotKeyChanged += (s, e) =>
            {
                if (!Runtime.Instance.Settings.Hotkey.Contains(this.hotKeyMimicTextBox4.HotKey))
                {
                    Runtime.Instance.Settings.Hotkey.Add(this.hotKeyMimicTextBox4.HotKey);
                }
                else
                {
                    Runtime.Instance.Settings.Hotkey[0] = this.hotKeyMimicTextBox4.HotKey;
                }
                this.hotkeysChanged = true;
            };

            this.glowPanel1.EffectColor = Runtime.Instance.Settings.PasswordIsSet ? Color.LimeGreen : Color.Firebrick;
            if (!Runtime.Instance.Settings.PasswordIsSet)
                this.password.TextChanged += (s, e) =>
                {
                    bool isSet = !string.IsNullOrWhiteSpace(this.password.Text);
                    this.glowPanel1.EffectColor = isSet ? Color.LimeGreen : Color.Firebrick;
                    this.password.AccessibleDescription = isSet
                        ? "The password has been configured."
                        : "The password has not been set.";
                    this.glowPanel1.AccessibleDescription = isSet
                        ? "The password has been configured."
                        : "The password has not been set.";
                    this.requirePasswordOnShow.Enabled = isSet;
                };
            this.password.AccessibleDescription = Runtime.Instance.Settings.PasswordIsSet
                ? "The password has been configured."
                : "The password has not been set.";
            this.glowPanel1.AccessibleDescription = Runtime.Instance.Settings.PasswordIsSet
                ? "The password has been configured."
                : "The password has not been set.";
            this.clearPassword.AccessibleDescription = "Check to have your password cleared when closing.";
            this.FormClosing += (s, e) =>
            {
                if (this.password.ClearPassword)
                {
                    Runtime.Instance.Settings.HashedPassword = string.Empty;
                }
                else if (this.password.Password != string.Empty)
                {
                    Runtime.Instance.Settings.HashedPassword = this.password.Password;
                }
                ExternalReferences.RegisterAll();
            };
            this.InitializeToolTips(null);
        }

        #endregion

        #region Declarations

        private bool hotkeysChanged;

        private Updater updater;

        #endregion

        #region Properties

        public TabPage ActivePage
        {
            get { return this.tabControl.SelectedTab; }
        }

        #endregion

        #region Methods & Functions

        private void InitializeToolTips(Control.ControlCollection container)
        {
            if (container == null)
                container = this.Controls;

            foreach (Control control in container)
            {
                control.MouseEnter += this.ShowTooltip;
                control.MouseLeave += this.HideTooltip;

                this.InitializeToolTips(control.Controls);
            }
        }

        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {
        }

        private void password_TextChanged(object sender, EventArgs e)
        {
            this.clearPassword.Visible = !string.IsNullOrWhiteSpace(this.password.Password);
        }

        private void HideTooltip(object sender, EventArgs e)
        {
            this.tooltipLabel.Text = string.Empty;
        }

        private void ShowTooltip(object sender, EventArgs e)
        {
            this.tooltipLabel.Text = ((Control) sender).AccessibleDescription;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (this.updater = new Updater())
            {
                this.updater.Notification +=
                    (s, ne) =>
                        this.listBox1.Items.Add(string.Format("[{0}]\t{1}", ne.NotificationDate, ne.Message));
                this.updater.Updating +=
                    (s, ne) =>
                    {
                        this.listBox1.Items.Add(string.Format("[{0}]\t{1}", ne.NotificationDate, ne.Message));
                        if (ne.Completed)
                        {
                            if (ne.Success)
                            {
                                this.listBox1.Items.Add(string.Format("[{0}]\t{1} Updates available.",
                                    ne.NotificationDate, ne.Count));
                            }
                            else
                            {
                                this.listBox1.Items.Add(string.Format("[{0}]\tError: {1}", ne.NotificationDate,
                                    ne.Error.Message));
                            }
                        }
                    };

                this.updater.GetAvailableUpdates();
            }
        }

        #endregion
    }
}