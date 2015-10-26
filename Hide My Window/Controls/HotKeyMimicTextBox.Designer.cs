namespace theDiary.Tools.HideMyWindow
{
    partial class HotKeyMimicTextBox
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
            this.txtHotKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtHotKey
            // 
            this.txtHotKey.Location = new System.Drawing.Point(123, 0);
            this.txtHotKey.Name = "txtHotKey";
            this.txtHotKey.ReadOnly = true;
            this.txtHotKey.Size = new System.Drawing.Size(159, 20);
            this.txtHotKey.TabIndex = 2;
            this.txtHotKey.Text = "Mimic Hot Key In Here";
            this.txtHotKey.Enter += new System.EventHandler(this.txtHotKey_Enter);
            this.txtHotKey.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtHotKey_KeyUp);
            this.txtHotKey.Leave += new System.EventHandler(this.txtHotKey_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // HotKeyMimicTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtHotKey);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "HotKeyMimicTextBox";
            this.Size = new System.Drawing.Size(285, 23);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtHotKey;
        private System.Windows.Forms.Label label1;
    }
}
