namespace theDiary.Tools.HideMyWindow
{
    partial class ConfigurationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tooltipLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.save = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.startWithWindows = new System.Windows.Forms.CheckBox();
            this.glowPanel1 = new theDiary.Tools.HideMyWindow.GlowPanel();
            this.password = new theDiary.Tools.HideMyWindow.PasswordTextBox();
            this.clearPassword = new System.Windows.Forms.CheckBox();
            this.requirePasswordOnShow = new System.Windows.Forms.CheckBox();
            this.startInTaskbar = new System.Windows.Forms.CheckBox();
            this.confirmWhenExiting = new System.Windows.Forms.CheckBox();
            this.restoreWindowsOnExit = new System.Windows.Forms.CheckBox();
            this.closeToTaskbar = new System.Windows.Forms.CheckBox();
            this.minimizeToTaskbar = new System.Windows.Forms.CheckBox();
            this.tabHotKeys = new System.Windows.Forms.TabPage();
            this.hotKeyMimicTextBox4 = new theDiary.Tools.HideMyWindow.HotKeyMimicTextBox();
            this.hotKeyMimicTextBox3 = new theDiary.Tools.HideMyWindow.HotKeyMimicTextBox();
            this.hotKeyMimicTextBox2 = new theDiary.Tools.HideMyWindow.HotKeyMimicTextBox();
            this.hotKeyMimicTextBox1 = new theDiary.Tools.HideMyWindow.HotKeyMimicTextBox();
            this.tpUpdate = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.tabBehaviour = new System.Windows.Forms.TabPage();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.glowPanel1.SuspendLayout();
            this.tabHotKeys.SuspendLayout();
            this.tpUpdate.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.save);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 328);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(627, 34);
            this.panel1.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.statusStrip1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(5, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(542, 24);
            this.panel3.TabIndex = 1;
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tooltipLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(542, 24);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tooltipLabel
            // 
            this.tooltipLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tooltipLabel.Name = "tooltipLabel";
            this.tooltipLabel.Size = new System.Drawing.Size(0, 19);
            this.tooltipLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // save
            // 
            this.save.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.save.Dock = System.Windows.Forms.DockStyle.Right;
            this.save.Location = new System.Drawing.Point(547, 5);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(75, 24);
            this.save.TabIndex = 0;
            this.save.Text = "&Close";
            this.save.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5);
            this.panel2.Size = new System.Drawing.Size(627, 328);
            this.panel2.TabIndex = 2;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabGeneral);
            this.tabControl.Controls.Add(this.tabBehaviour);
            this.tabControl.Controls.Add(this.tabHotKeys);
            this.tabControl.Controls.Add(this.tpUpdate);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.ImageList = this.imageList1;
            this.tabControl.Location = new System.Drawing.Point(5, 5);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(617, 318);
            this.tabControl.TabIndex = 0;
            this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl_Selected);
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.startWithWindows);
            this.tabGeneral.Controls.Add(this.glowPanel1);
            this.tabGeneral.Controls.Add(this.clearPassword);
            this.tabGeneral.Controls.Add(this.requirePasswordOnShow);
            this.tabGeneral.Controls.Add(this.startInTaskbar);
            this.tabGeneral.Controls.Add(this.confirmWhenExiting);
            this.tabGeneral.Controls.Add(this.restoreWindowsOnExit);
            this.tabGeneral.Controls.Add(this.closeToTaskbar);
            this.tabGeneral.Controls.Add(this.minimizeToTaskbar);
            this.tabGeneral.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabGeneral.ImageIndex = 0;
            this.tabGeneral.Location = new System.Drawing.Point(4, 23);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(5);
            this.tabGeneral.Size = new System.Drawing.Size(609, 291);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // startWithWindows
            // 
            this.startWithWindows.AccessibleDescription = "Hide My Window will start up when logging into Windows.";
            this.startWithWindows.AutoSize = true;
            this.startWithWindows.Location = new System.Drawing.Point(8, 152);
            this.startWithWindows.Name = "startWithWindows";
            this.startWithWindows.Size = new System.Drawing.Size(119, 18);
            this.startWithWindows.TabIndex = 9;
            this.startWithWindows.Text = "Start with Windows";
            this.startWithWindows.UseCompatibleTextRendering = true;
            this.startWithWindows.UseVisualStyleBackColor = true;
            // 
            // glowPanel1
            // 
            this.glowPanel1.Controls.Add(this.password);
            this.glowPanel1.EffectColor = System.Drawing.Color.Firebrick;
            this.glowPanel1.EffectEnabled = true;
            this.glowPanel1.FeatherEffect = 100;
            this.glowPanel1.GlowThickness = 5;
            this.glowPanel1.Location = new System.Drawing.Point(8, 176);
            this.glowPanel1.Name = "glowPanel1";
            this.glowPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.glowPanel1.Size = new System.Drawing.Size(201, 30);
            this.glowPanel1.TabIndex = 8;
            this.glowPanel1.Trigger = theDiary.Tools.HideMyWindow.EffectTrigger.AlwaysOn;
            // 
            // password
            // 
            this.password.ClearPassword = false;
            this.password.Font = new System.Drawing.Font("Wingdings", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.password.Location = new System.Drawing.Point(4, 4);
            this.password.Name = "password";
            this.password.Password = "";
            this.password.PasswordChar = 'l';
            this.password.Size = new System.Drawing.Size(191, 20);
            this.password.TabIndex = 6;
            this.password.WaterMark = "Enter Password";
            this.password.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.password.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.password.TextChanged += new System.EventHandler(this.password_TextChanged);
            // 
            // clearPassword
            // 
            this.clearPassword.AutoSize = true;
            this.clearPassword.Location = new System.Drawing.Point(216, 181);
            this.clearPassword.Name = "clearPassword";
            this.clearPassword.Size = new System.Drawing.Size(99, 17);
            this.clearPassword.TabIndex = 7;
            this.clearPassword.Text = "Clear Password";
            this.clearPassword.UseVisualStyleBackColor = true;
            // 
            // requirePasswordOnShow
            // 
            this.requirePasswordOnShow.AccessibleName = "When showing from the Tool Tray the application will confirm password.";
            this.requirePasswordOnShow.AutoSize = true;
            this.requirePasswordOnShow.Location = new System.Drawing.Point(8, 128);
            this.requirePasswordOnShow.Name = "requirePasswordOnShow";
            this.requirePasswordOnShow.Size = new System.Drawing.Size(192, 18);
            this.requirePasswordOnShow.TabIndex = 5;
            this.requirePasswordOnShow.Text = "Require Password when Showing";
            this.requirePasswordOnShow.UseCompatibleTextRendering = true;
            this.requirePasswordOnShow.UseVisualStyleBackColor = true;
            // 
            // startInTaskbar
            // 
            this.startInTaskbar.AccessibleDescription = "Application will start in the Tool Tray.";
            this.startInTaskbar.AutoSize = true;
            this.startInTaskbar.Location = new System.Drawing.Point(8, 104);
            this.startInTaskbar.Name = "startInTaskbar";
            this.startInTaskbar.Size = new System.Drawing.Size(103, 18);
            this.startInTaskbar.TabIndex = 4;
            this.startInTaskbar.Text = "Start in Taskbar";
            this.startInTaskbar.UseCompatibleTextRendering = true;
            this.startInTaskbar.UseVisualStyleBackColor = true;
            // 
            // confirmWhenExiting
            // 
            this.confirmWhenExiting.AccessibleDescription = "Confirm before exiting application.";
            this.confirmWhenExiting.AutoSize = true;
            this.confirmWhenExiting.Location = new System.Drawing.Point(8, 80);
            this.confirmWhenExiting.Name = "confirmWhenExiting";
            this.confirmWhenExiting.Size = new System.Drawing.Size(129, 18);
            this.confirmWhenExiting.TabIndex = 3;
            this.confirmWhenExiting.Text = "Confirm when exiting";
            this.confirmWhenExiting.UseCompatibleTextRendering = true;
            this.confirmWhenExiting.UseVisualStyleBackColor = true;
            // 
            // restoreWindowsOnExit
            // 
            this.restoreWindowsOnExit.AutoSize = true;
            this.restoreWindowsOnExit.Location = new System.Drawing.Point(8, 56);
            this.restoreWindowsOnExit.Name = "restoreWindowsOnExit";
            this.restoreWindowsOnExit.Size = new System.Drawing.Size(178, 18);
            this.restoreWindowsOnExit.TabIndex = 2;
            this.restoreWindowsOnExit.Text = "Restore Windows when exiting";
            this.restoreWindowsOnExit.UseCompatibleTextRendering = true;
            this.restoreWindowsOnExit.UseVisualStyleBackColor = true;
            // 
            // closeToTaskbar
            // 
            this.closeToTaskbar.AccessibleDescription = "Closing the window will send the application to the Tool Tray.";
            this.closeToTaskbar.AutoSize = true;
            this.closeToTaskbar.Location = new System.Drawing.Point(8, 32);
            this.closeToTaskbar.Name = "closeToTaskbar";
            this.closeToTaskbar.Size = new System.Drawing.Size(109, 18);
            this.closeToTaskbar.TabIndex = 1;
            this.closeToTaskbar.Text = "Close to Taskbar";
            this.closeToTaskbar.UseCompatibleTextRendering = true;
            this.closeToTaskbar.UseVisualStyleBackColor = true;
            // 
            // minimizeToTaskbar
            // 
            this.minimizeToTaskbar.AccessibleDescription = "Closing the window will send the application to the Tool Tray.";
            this.minimizeToTaskbar.AutoSize = true;
            this.minimizeToTaskbar.Location = new System.Drawing.Point(8, 8);
            this.minimizeToTaskbar.Name = "minimizeToTaskbar";
            this.minimizeToTaskbar.Size = new System.Drawing.Size(124, 18);
            this.minimizeToTaskbar.TabIndex = 0;
            this.minimizeToTaskbar.Text = "Minimize to Taskbar";
            this.minimizeToTaskbar.UseCompatibleTextRendering = true;
            this.minimizeToTaskbar.UseVisualStyleBackColor = true;
            // 
            // tabHotKeys
            // 
            this.tabHotKeys.Controls.Add(this.hotKeyMimicTextBox4);
            this.tabHotKeys.Controls.Add(this.hotKeyMimicTextBox3);
            this.tabHotKeys.Controls.Add(this.hotKeyMimicTextBox2);
            this.tabHotKeys.Controls.Add(this.hotKeyMimicTextBox1);
            this.tabHotKeys.Location = new System.Drawing.Point(4, 22);
            this.tabHotKeys.Name = "tabHotKeys";
            this.tabHotKeys.Padding = new System.Windows.Forms.Padding(3);
            this.tabHotKeys.Size = new System.Drawing.Size(609, 292);
            this.tabHotKeys.TabIndex = 1;
            this.tabHotKeys.Text = "Hot Keys";
            this.tabHotKeys.UseVisualStyleBackColor = true;
            // 
            // hotKeyMimicTextBox4
            // 
            this.hotKeyMimicTextBox4.AutoSize = true;
            this.hotKeyMimicTextBox4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.hotKeyMimicTextBox4.HotKey = null;
            this.hotKeyMimicTextBox4.Location = new System.Drawing.Point(3, 72);
            this.hotKeyMimicTextBox4.Margin = new System.Windows.Forms.Padding(0);
            this.hotKeyMimicTextBox4.Name = "hotKeyMimicTextBox4";
            this.hotKeyMimicTextBox4.Size = new System.Drawing.Size(285, 23);
            this.hotKeyMimicTextBox4.TabIndex = 4;
            // 
            // hotKeyMimicTextBox3
            // 
            this.hotKeyMimicTextBox3.AutoSize = true;
            this.hotKeyMimicTextBox3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.hotKeyMimicTextBox3.HotKey = null;
            this.hotKeyMimicTextBox3.Location = new System.Drawing.Point(3, 49);
            this.hotKeyMimicTextBox3.Margin = new System.Windows.Forms.Padding(0);
            this.hotKeyMimicTextBox3.Name = "hotKeyMimicTextBox3";
            this.hotKeyMimicTextBox3.Size = new System.Drawing.Size(285, 23);
            this.hotKeyMimicTextBox3.TabIndex = 3;
            // 
            // hotKeyMimicTextBox2
            // 
            this.hotKeyMimicTextBox2.AutoSize = true;
            this.hotKeyMimicTextBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.hotKeyMimicTextBox2.HotKey = null;
            this.hotKeyMimicTextBox2.Location = new System.Drawing.Point(3, 26);
            this.hotKeyMimicTextBox2.Margin = new System.Windows.Forms.Padding(0);
            this.hotKeyMimicTextBox2.Name = "hotKeyMimicTextBox2";
            this.hotKeyMimicTextBox2.Size = new System.Drawing.Size(285, 23);
            this.hotKeyMimicTextBox2.TabIndex = 2;
            // 
            // hotKeyMimicTextBox1
            // 
            this.hotKeyMimicTextBox1.AutoSize = true;
            this.hotKeyMimicTextBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.hotKeyMimicTextBox1.HotKey = null;
            this.hotKeyMimicTextBox1.Location = new System.Drawing.Point(3, 3);
            this.hotKeyMimicTextBox1.Margin = new System.Windows.Forms.Padding(0);
            this.hotKeyMimicTextBox1.Name = "hotKeyMimicTextBox1";
            this.hotKeyMimicTextBox1.Size = new System.Drawing.Size(285, 23);
            this.hotKeyMimicTextBox1.TabIndex = 1;
            // 
            // tpUpdate
            // 
            this.tpUpdate.Controls.Add(this.button1);
            this.tpUpdate.Controls.Add(this.listBox1);
            this.tpUpdate.Location = new System.Drawing.Point(4, 22);
            this.tpUpdate.Name = "tpUpdate";
            this.tpUpdate.Padding = new System.Windows.Forms.Padding(3);
            this.tpUpdate.Size = new System.Drawing.Size(609, 292);
            this.tpUpdate.TabIndex = 2;
            this.tpUpdate.Text = "Update";
            this.tpUpdate.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(528, 263);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(6, 6);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(597, 251);
            this.listBox1.TabIndex = 0;
            // 
            // tabBehaviour
            // 
            this.tabBehaviour.Location = new System.Drawing.Point(4, 22);
            this.tabBehaviour.Name = "tabBehaviour";
            this.tabBehaviour.Size = new System.Drawing.Size(609, 292);
            this.tabBehaviour.TabIndex = 3;
            this.tabBehaviour.Text = "Behaviour";
            this.tabBehaviour.UseVisualStyleBackColor = true;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Ball (Red).ico");
            this.imageList1.Images.SetKeyName(1, "Ball (Green).ico");
            // 
            // ConfigurationForm
            // 
            this.AcceptButton = this.save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 362);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ConfigurationForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configuration";
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.glowPanel1.ResumeLayout(false);
            this.glowPanel1.PerformLayout();
            this.tabHotKeys.ResumeLayout(false);
            this.tabHotKeys.PerformLayout();
            this.tpUpdate.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabHotKeys;
        private System.Windows.Forms.CheckBox restoreWindowsOnExit;
        private System.Windows.Forms.CheckBox closeToTaskbar;
        private System.Windows.Forms.CheckBox minimizeToTaskbar;
        private System.Windows.Forms.CheckBox confirmWhenExiting;
        private System.Windows.Forms.CheckBox startInTaskbar;
        private System.Windows.Forms.CheckBox requirePasswordOnShow;
        private PasswordTextBox password;
        private System.Windows.Forms.CheckBox clearPassword;
        private GlowPanel glowPanel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tooltipLabel;
        private HotKeyMimicTextBox hotKeyMimicTextBox1;
        private HotKeyMimicTextBox hotKeyMimicTextBox4;
        private HotKeyMimicTextBox hotKeyMimicTextBox3;
        private HotKeyMimicTextBox hotKeyMimicTextBox2;
        private System.Windows.Forms.CheckBox startWithWindows;
        private System.Windows.Forms.TabPage tpUpdate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TabPage tabBehaviour;
        private System.Windows.Forms.ImageList imageList1;
    }
}