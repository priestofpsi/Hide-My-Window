namespace theDiary.Tools.HideMyWindow.Forms.ConfigurationSections
{
    partial class UpdateClientConfigurationForm
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
            this.check = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.availableUpdates = new System.Windows.Forms.ComboBox();
            this.download = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // check
            // 
            this.check.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.check.Location = new System.Drawing.Point(499, 312);
            this.check.Name = "check";
            this.check.Size = new System.Drawing.Size(77, 23);
            this.check.TabIndex = 3;
            this.check.Text = "&Check";
            this.check.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(3, 3);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(573, 303);
            this.listBox1.TabIndex = 2;
            // 
            // availableUpdates
            // 
            this.availableUpdates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.availableUpdates.DisplayMember = "Name";
            this.availableUpdates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.availableUpdates.FormattingEnabled = true;
            this.availableUpdates.Location = new System.Drawing.Point(4, 312);
            this.availableUpdates.Name = "availableUpdates";
            this.availableUpdates.Size = new System.Drawing.Size(219, 21);
            this.availableUpdates.TabIndex = 4;
            this.availableUpdates.Visible = false;
            // 
            // download
            // 
            this.download.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.download.AutoSize = true;
            this.download.Location = new System.Drawing.Point(229, 315);
            this.download.Name = "download";
            this.download.Size = new System.Drawing.Size(55, 13);
            this.download.TabIndex = 5;
            this.download.TabStop = true;
            this.download.Text = "Download";
            this.download.Visible = false;
            // 
            // UpdateClientConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.download);
            this.Controls.Add(this.availableUpdates);
            this.Controls.Add(this.check);
            this.Controls.Add(this.listBox1);
            this.Name = "UpdateClientConfigurationForm";
            this.Size = new System.Drawing.Size(579, 343);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button check;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ComboBox availableUpdates;
        private System.Windows.Forms.LinkLabel download;
    }
}
