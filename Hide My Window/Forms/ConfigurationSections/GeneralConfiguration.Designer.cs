namespace theDiary.Tools.HideMyWindow.Forms.ConfigurationSections
{
    partial class GeneralConfiguration
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
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
            this.glowPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // startWithWindows
            // 
            this.startWithWindows.AccessibleDescription = "Hide My Window will start up when logging into Windows.";
            this.startWithWindows.AutoSize = true;
            this.startWithWindows.Location = new System.Drawing.Point(3, 147);
            this.startWithWindows.Name = "startWithWindows";
            this.startWithWindows.Size = new System.Drawing.Size(119, 18);
            this.startWithWindows.TabIndex = 18;
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
            this.glowPanel1.Location = new System.Drawing.Point(3, 171);
            this.glowPanel1.Name = "glowPanel1";
            this.glowPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.glowPanel1.Size = new System.Drawing.Size(201, 30);
            this.glowPanel1.TabIndex = 17;
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
            // 
            // clearPassword
            // 
            this.clearPassword.AutoSize = true;
            this.clearPassword.Location = new System.Drawing.Point(211, 176);
            this.clearPassword.Name = "clearPassword";
            this.clearPassword.Size = new System.Drawing.Size(99, 17);
            this.clearPassword.TabIndex = 16;
            this.clearPassword.Text = "Clear Password";
            this.clearPassword.UseVisualStyleBackColor = true;
            // 
            // requirePasswordOnShow
            // 
            this.requirePasswordOnShow.AccessibleName = "When showing from the Tool Tray the application will confirm password.";
            this.requirePasswordOnShow.AutoSize = true;
            this.requirePasswordOnShow.Location = new System.Drawing.Point(3, 123);
            this.requirePasswordOnShow.Name = "requirePasswordOnShow";
            this.requirePasswordOnShow.Size = new System.Drawing.Size(192, 18);
            this.requirePasswordOnShow.TabIndex = 15;
            this.requirePasswordOnShow.Text = "Require Password when Showing";
            this.requirePasswordOnShow.UseCompatibleTextRendering = true;
            this.requirePasswordOnShow.UseVisualStyleBackColor = true;
            // 
            // startInTaskbar
            // 
            this.startInTaskbar.AccessibleDescription = "Application will start in the Tool Tray.";
            this.startInTaskbar.AutoSize = true;
            this.startInTaskbar.Location = new System.Drawing.Point(3, 99);
            this.startInTaskbar.Name = "startInTaskbar";
            this.startInTaskbar.Size = new System.Drawing.Size(103, 18);
            this.startInTaskbar.TabIndex = 14;
            this.startInTaskbar.Text = "Start in Taskbar";
            this.startInTaskbar.UseCompatibleTextRendering = true;
            this.startInTaskbar.UseVisualStyleBackColor = true;
            // 
            // confirmWhenExiting
            // 
            this.confirmWhenExiting.AccessibleDescription = "Confirm before exiting application.";
            this.confirmWhenExiting.AutoSize = true;
            this.confirmWhenExiting.Location = new System.Drawing.Point(3, 75);
            this.confirmWhenExiting.Name = "confirmWhenExiting";
            this.confirmWhenExiting.Size = new System.Drawing.Size(129, 18);
            this.confirmWhenExiting.TabIndex = 13;
            this.confirmWhenExiting.Text = "Confirm when exiting";
            this.confirmWhenExiting.UseCompatibleTextRendering = true;
            this.confirmWhenExiting.UseVisualStyleBackColor = true;
            // 
            // restoreWindowsOnExit
            // 
            this.restoreWindowsOnExit.AutoSize = true;
            this.restoreWindowsOnExit.Location = new System.Drawing.Point(3, 51);
            this.restoreWindowsOnExit.Name = "restoreWindowsOnExit";
            this.restoreWindowsOnExit.Size = new System.Drawing.Size(178, 18);
            this.restoreWindowsOnExit.TabIndex = 12;
            this.restoreWindowsOnExit.Text = "Restore Windows when exiting";
            this.restoreWindowsOnExit.UseCompatibleTextRendering = true;
            this.restoreWindowsOnExit.UseVisualStyleBackColor = true;
            // 
            // closeToTaskbar
            // 
            this.closeToTaskbar.AccessibleDescription = "Closing the window will send the application to the Tool Tray.";
            this.closeToTaskbar.AutoSize = true;
            this.closeToTaskbar.Location = new System.Drawing.Point(3, 27);
            this.closeToTaskbar.Name = "closeToTaskbar";
            this.closeToTaskbar.Size = new System.Drawing.Size(109, 18);
            this.closeToTaskbar.TabIndex = 11;
            this.closeToTaskbar.Text = "Close to Taskbar";
            this.closeToTaskbar.UseCompatibleTextRendering = true;
            this.closeToTaskbar.UseVisualStyleBackColor = true;
            // 
            // minimizeToTaskbar
            // 
            this.minimizeToTaskbar.AccessibleDescription = "Closing the window will send the application to the Tool Tray.";
            this.minimizeToTaskbar.AutoSize = true;
            this.minimizeToTaskbar.Location = new System.Drawing.Point(3, 3);
            this.minimizeToTaskbar.Name = "minimizeToTaskbar";
            this.minimizeToTaskbar.Size = new System.Drawing.Size(124, 18);
            this.minimizeToTaskbar.TabIndex = 10;
            this.minimizeToTaskbar.Text = "Minimize to Taskbar";
            this.minimizeToTaskbar.UseCompatibleTextRendering = true;
            this.minimizeToTaskbar.UseVisualStyleBackColor = true;
            // 
            // GeneralConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.startWithWindows);
            this.Controls.Add(this.glowPanel1);
            this.Controls.Add(this.clearPassword);
            this.Controls.Add(this.requirePasswordOnShow);
            this.Controls.Add(this.startInTaskbar);
            this.Controls.Add(this.confirmWhenExiting);
            this.Controls.Add(this.restoreWindowsOnExit);
            this.Controls.Add(this.closeToTaskbar);
            this.Controls.Add(this.minimizeToTaskbar);
            this.Name = "GeneralConfiguration";
            this.Size = new System.Drawing.Size(309, 207);
            this.glowPanel1.ResumeLayout(false);
            this.glowPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox startWithWindows;
        private GlowPanel glowPanel1;
        private PasswordTextBox password;
        private System.Windows.Forms.CheckBox clearPassword;
        private System.Windows.Forms.CheckBox requirePasswordOnShow;
        private System.Windows.Forms.CheckBox startInTaskbar;
        private System.Windows.Forms.CheckBox confirmWhenExiting;
        private System.Windows.Forms.CheckBox restoreWindowsOnExit;
        private System.Windows.Forms.CheckBox closeToTaskbar;
        private System.Windows.Forms.CheckBox minimizeToTaskbar;
    }
}
