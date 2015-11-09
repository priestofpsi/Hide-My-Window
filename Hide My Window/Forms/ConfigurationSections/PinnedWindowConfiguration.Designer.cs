namespace theDiary.Tools.HideMyWindow.Forms.ConfigurationSections
{
    partial class PinnedWindowConfiguration
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
            this.pinnedHideWhenMinimized = new System.Windows.Forms.CheckBox();
            this.modifyWindowText = new System.Windows.Forms.CheckBox();
            this.watermarkTextBox1 = new theDiary.Tools.HideMyWindow.WatermarkTextBox();
            this.watermarkTextBox2 = new theDiary.Tools.HideMyWindow.WatermarkTextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // pinnedHideWhenMinimized
            // 
            this.pinnedHideWhenMinimized.AccessibleDescription = "Pinned windows will automatically hide when minimized.";
            this.pinnedHideWhenMinimized.AutoSize = true;
            this.pinnedHideWhenMinimized.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pinnedHideWhenMinimized.Location = new System.Drawing.Point(3, 3);
            this.pinnedHideWhenMinimized.Name = "pinnedHideWhenMinimized";
            this.pinnedHideWhenMinimized.Size = new System.Drawing.Size(131, 18);
            this.pinnedHideWhenMinimized.TabIndex = 2;
            this.pinnedHideWhenMinimized.Text = "Hide when Minimized";
            this.pinnedHideWhenMinimized.UseCompatibleTextRendering = true;
            this.pinnedHideWhenMinimized.UseVisualStyleBackColor = true;
            // 
            // modifyWindowText
            // 
            this.modifyWindowText.AccessibleDescription = "Pinned windows will automatically hide when minimized.";
            this.modifyWindowText.AutoSize = true;
            this.modifyWindowText.Enabled = false;
            this.modifyWindowText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modifyWindowText.Location = new System.Drawing.Point(3, 27);
            this.modifyWindowText.Name = "modifyWindowText";
            this.modifyWindowText.Size = new System.Drawing.Size(125, 18);
            this.modifyWindowText.TabIndex = 3;
            this.modifyWindowText.Text = "Modify Window Title";
            this.modifyWindowText.UseCompatibleTextRendering = true;
            this.modifyWindowText.UseVisualStyleBackColor = true;
            // 
            // watermarkTextBox1
            // 
            this.watermarkTextBox1.Location = new System.Drawing.Point(3, 51);
            this.watermarkTextBox1.Name = "watermarkTextBox1";
            this.watermarkTextBox1.Size = new System.Drawing.Size(147, 20);
            this.watermarkTextBox1.TabIndex = 4;
            this.watermarkTextBox1.WaterMark = "Window Title Prefix";
            this.watermarkTextBox1.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.watermarkTextBox1.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.watermarkTextBox1.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // watermarkTextBox2
            // 
            this.watermarkTextBox2.Location = new System.Drawing.Point(157, 52);
            this.watermarkTextBox2.Name = "watermarkTextBox2";
            this.watermarkTextBox2.Size = new System.Drawing.Size(147, 20);
            this.watermarkTextBox2.TabIndex = 5;
            this.watermarkTextBox2.WaterMark = "Window Title Suffix";
            this.watermarkTextBox2.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.watermarkTextBox2.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.watermarkTextBox2.WaterMarkForeColor = System.Drawing.Color.LightGray;
            // 
            // checkBox1
            // 
            this.checkBox1.AccessibleDescription = "Pinned windows will automatically hide when minimized.";
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.Location = new System.Drawing.Point(3, 78);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(110, 18);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Add Icon Overlay";
            this.checkBox1.UseCompatibleTextRendering = true;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "TopLeft",
            "Top Right",
            "BottomRight",
            "BottomLeft",
            "Centred"});
            this.comboBox1.Location = new System.Drawing.Point(3, 102);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 8;
            // 
            // PinnedWindowConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.watermarkTextBox2);
            this.Controls.Add(this.watermarkTextBox1);
            this.Controls.Add(this.modifyWindowText);
            this.Controls.Add(this.pinnedHideWhenMinimized);
            this.Name = "PinnedWindowConfiguration";
            this.Size = new System.Drawing.Size(377, 267);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox pinnedHideWhenMinimized;
        private System.Windows.Forms.CheckBox modifyWindowText;
        private WatermarkTextBox watermarkTextBox1;
        private WatermarkTextBox watermarkTextBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}
