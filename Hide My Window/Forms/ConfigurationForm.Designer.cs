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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.save = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.tabHotKeys = new System.Windows.Forms.TabPage();
            this.minimizeToTaskbar = new System.Windows.Forms.CheckBox();
            this.closeToTaskbar = new System.Windows.Forms.CheckBox();
            this.restoreWindowsOnExit = new System.Windows.Forms.CheckBox();
            this.confirmWhenExiting = new System.Windows.Forms.CheckBox();
            this.startInTaskbar = new System.Windows.Forms.CheckBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Function = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Control = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Alt = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Shift = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Win = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.HotKey = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.tabHotKeys.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.save);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 257);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(324, 34);
            this.panel1.TabIndex = 1;
            // 
            // save
            // 
            this.save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.save.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.save.Location = new System.Drawing.Point(245, 5);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(75, 23);
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
            this.panel2.Size = new System.Drawing.Size(324, 257);
            this.panel2.TabIndex = 2;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabGeneral);
            this.tabControl.Controls.Add(this.tabHotKeys);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(5, 5);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(314, 247);
            this.tabControl.TabIndex = 0;
            this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl_Selected);
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.startInTaskbar);
            this.tabGeneral.Controls.Add(this.confirmWhenExiting);
            this.tabGeneral.Controls.Add(this.restoreWindowsOnExit);
            this.tabGeneral.Controls.Add(this.closeToTaskbar);
            this.tabGeneral.Controls.Add(this.minimizeToTaskbar);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(5);
            this.tabGeneral.Size = new System.Drawing.Size(306, 221);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // tabHotKeys
            // 
            this.tabHotKeys.Controls.Add(this.dataGridView1);
            this.tabHotKeys.Location = new System.Drawing.Point(4, 22);
            this.tabHotKeys.Name = "tabHotKeys";
            this.tabHotKeys.Padding = new System.Windows.Forms.Padding(3);
            this.tabHotKeys.Size = new System.Drawing.Size(306, 221);
            this.tabHotKeys.TabIndex = 1;
            this.tabHotKeys.Text = "Hot Keys";
            this.tabHotKeys.UseVisualStyleBackColor = true;
            // 
            // minimizeToTaskbar
            // 
            this.minimizeToTaskbar.AutoSize = true;
            this.minimizeToTaskbar.Location = new System.Drawing.Point(8, 8);
            this.minimizeToTaskbar.Name = "minimizeToTaskbar";
            this.minimizeToTaskbar.Size = new System.Drawing.Size(124, 18);
            this.minimizeToTaskbar.TabIndex = 0;
            this.minimizeToTaskbar.Text = "Minimize to Taskbar";
            this.minimizeToTaskbar.UseCompatibleTextRendering = true;
            this.minimizeToTaskbar.UseVisualStyleBackColor = true;
            // 
            // closeToTaskbar
            // 
            this.closeToTaskbar.AutoSize = true;
            this.closeToTaskbar.Location = new System.Drawing.Point(8, 32);
            this.closeToTaskbar.Name = "closeToTaskbar";
            this.closeToTaskbar.Size = new System.Drawing.Size(109, 18);
            this.closeToTaskbar.TabIndex = 1;
            this.closeToTaskbar.Text = "Close to Taskbar";
            this.closeToTaskbar.UseCompatibleTextRendering = true;
            this.closeToTaskbar.UseVisualStyleBackColor = true;
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
            // confirmWhenExiting
            // 
            this.confirmWhenExiting.AutoSize = true;
            this.confirmWhenExiting.Location = new System.Drawing.Point(8, 80);
            this.confirmWhenExiting.Name = "confirmWhenExiting";
            this.confirmWhenExiting.Size = new System.Drawing.Size(129, 18);
            this.confirmWhenExiting.TabIndex = 3;
            this.confirmWhenExiting.Text = "Confirm when exiting";
            this.confirmWhenExiting.UseCompatibleTextRendering = true;
            this.confirmWhenExiting.UseVisualStyleBackColor = true;
            // 
            // startInTaskbar
            // 
            this.startInTaskbar.AutoSize = true;
            this.startInTaskbar.Location = new System.Drawing.Point(8, 104);
            this.startInTaskbar.Name = "startInTaskbar";
            this.startInTaskbar.Size = new System.Drawing.Size(103, 18);
            this.startInTaskbar.TabIndex = 4;
            this.startInTaskbar.Text = "Start in Taskbar";
            this.startInTaskbar.UseCompatibleTextRendering = true;
            this.startInTaskbar.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Function,
            this.Control,
            this.Alt,
            this.Shift,
            this.Win,
            this.HotKey});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(300, 215);
            this.dataGridView1.TabIndex = 0;
            // 
            // Function
            // 
            this.Function.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Function.HeaderText = "Function";
            this.Function.Name = "Function";
            this.Function.Width = 54;
            // 
            // Control
            // 
            this.Control.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Control.HeaderText = "Control";
            this.Control.Name = "Control";
            this.Control.Width = 46;
            // 
            // Alt
            // 
            this.Alt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Alt.HeaderText = "Alt";
            this.Alt.Name = "Alt";
            this.Alt.Width = 25;
            // 
            // Shift
            // 
            this.Shift.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Shift.HeaderText = "Shift";
            this.Shift.Name = "Shift";
            this.Shift.Width = 34;
            // 
            // Win
            // 
            this.Win.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Win.HeaderText = "Win";
            this.Win.Name = "Win";
            this.Win.Width = 32;
            // 
            // HotKey
            // 
            this.HotKey.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.HotKey.HeaderText = "Hot Key";
            this.HotKey.Name = "HotKey";
            // 
            // ConfigurationForm
            // 
            this.AcceptButton = this.save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 291);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ConfigurationForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configuration";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.tabHotKeys.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
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
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewComboBoxColumn Function;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Control;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Alt;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Shift;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Win;
        private System.Windows.Forms.DataGridViewComboBoxColumn HotKey;
    }
}