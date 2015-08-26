using System.Windows.Forms;

namespace theDiary.Tools.HideMyWindow
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.showAll = new System.Windows.Forms.Button();
            this.show = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusbarToggle = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToggle = new System.Windows.Forms.ToolStripMenuItem();
            this.viewDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.viewLargeIcons = new System.Windows.Forms.ToolStripMenuItem();
            this.viewSmallIcons = new System.Windows.Forms.ToolStripMenuItem();
            this.viewList = new System.Windows.Forms.ToolStripMenuItem();
            this.viewTiles = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.openConfigurationForm = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.hiddenWindows = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageListBig = new System.Windows.Forms.ImageList(this.components);
            this.imageListSmall = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // showAll
            // 
            this.showAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.showAll.Enabled = false;
            this.showAll.Location = new System.Drawing.Point(197, 281);
            this.showAll.Name = "showAll";
            this.showAll.Size = new System.Drawing.Size(75, 23);
            this.showAll.TabIndex = 1;
            this.showAll.Text = "Show &All";
            this.showAll.UseVisualStyleBackColor = true;
            this.showAll.Click += new System.EventHandler(this.UnhideAllWindows);
            // 
            // show
            // 
            this.show.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.show.Enabled = false;
            this.show.Location = new System.Drawing.Point(116, 281);
            this.show.Name = "show";
            this.show.Size = new System.Drawing.Size(75, 23);
            this.show.TabIndex = 2;
            this.show.Text = "&Show";
            this.show.UseVisualStyleBackColor = true;
            this.show.Click += new System.EventHandler(this.UnhideWindows);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 307);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(284, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(110, 17);
            this.statusLabel.Text = "Hidden Windows: 0";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(284, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitApplication);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusbarToggle,
            this.viewToggle,
            this.toolStripMenuItem1,
            this.openConfigurationForm});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.viewToolStripMenuItem.Text = "&Options";
            // 
            // statusbarToggle
            // 
            this.statusbarToggle.Checked = true;
            this.statusbarToggle.CheckOnClick = true;
            this.statusbarToggle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.statusbarToggle.Name = "statusbarToggle";
            this.statusbarToggle.Size = new System.Drawing.Size(127, 22);
            this.statusbarToggle.Text = "&Statusbar";
            this.statusbarToggle.Click += new System.EventHandler(this.statusbarToggle_Click);
            // 
            // viewToggle
            // 
            this.viewToggle.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewDetails,
            this.viewLargeIcons,
            this.viewSmallIcons,
            this.viewList,
            this.viewTiles});
            this.viewToggle.Name = "viewToggle";
            this.viewToggle.Size = new System.Drawing.Size(127, 22);
            this.viewToggle.Text = "&View";
            this.viewToggle.DropDownOpening += new System.EventHandler(this.viewToggle_DropDownOpening);
            // 
            // viewDetails
            // 
            this.viewDetails.Name = "viewDetails";
            this.viewDetails.Size = new System.Drawing.Size(134, 22);
            this.viewDetails.Text = "&Details";
            this.viewDetails.Click += new System.EventHandler(this.ToggleView);
            // 
            // viewLargeIcons
            // 
            this.viewLargeIcons.Name = "viewLargeIcons";
            this.viewLargeIcons.Size = new System.Drawing.Size(134, 22);
            this.viewLargeIcons.Text = "&Large Icons";
            this.viewLargeIcons.Click += new System.EventHandler(this.ToggleView);
            // 
            // viewSmallIcons
            // 
            this.viewSmallIcons.Name = "viewSmallIcons";
            this.viewSmallIcons.Size = new System.Drawing.Size(134, 22);
            this.viewSmallIcons.Text = "&Small Icons";
            this.viewSmallIcons.Click += new System.EventHandler(this.ToggleView);
            // 
            // viewList
            // 
            this.viewList.Name = "viewList";
            this.viewList.Size = new System.Drawing.Size(134, 22);
            this.viewList.Text = "L&ist";
            this.viewList.Click += new System.EventHandler(this.ToggleView);
            // 
            // viewTiles
            // 
            this.viewTiles.Name = "viewTiles";
            this.viewTiles.Size = new System.Drawing.Size(134, 22);
            this.viewTiles.Text = "&Tiles";
            this.viewTiles.Click += new System.EventHandler(this.ToggleView);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(124, 6);
            // 
            // openConfigurationForm
            // 
            this.openConfigurationForm.Name = "openConfigurationForm";
            this.openConfigurationForm.Size = new System.Drawing.Size(127, 22);
            this.openConfigurationForm.Text = "&Configure";
            this.openConfigurationForm.Click += new System.EventHandler(this.openConfigurationForm_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Hide My Window";
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // hiddenWindows
            // 
            this.hiddenWindows.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hiddenWindows.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.hiddenWindows.LargeImageList = this.imageListBig;
            this.hiddenWindows.Location = new System.Drawing.Point(13, 26);
            this.hiddenWindows.Name = "hiddenWindows";
            this.hiddenWindows.Size = new System.Drawing.Size(259, 251);
            this.hiddenWindows.SmallImageList = this.imageListSmall;
            this.hiddenWindows.TabIndex = 5;
            this.hiddenWindows.UseCompatibleStateImageBehavior = false;
            this.hiddenWindows.DoubleClick += new System.EventHandler(this.UnhideWindows);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Title";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Path";
            // 
            // imageListBig
            // 
            this.imageListBig.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListBig.ImageSize = new System.Drawing.Size(32, 32);
            this.imageListBig.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imageListSmall
            // 
            this.imageListSmall.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListSmall.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListSmall.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 329);
            this.Controls.Add(this.hiddenWindows);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.show);
            this.Controls.Add(this.showAll);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Hide My Window";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.viewDetails.Tag = View.Details;
            this.viewLargeIcons.Tag = View.LargeIcon;
            this.viewSmallIcons.Tag = View.SmallIcon;
            this.viewTiles.Tag = View.Tile;
            this.viewList.Tag = View.List;
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button showAll;
        private System.Windows.Forms.Button show;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ListView hiddenWindows;
        private System.Windows.Forms.ImageList imageListSmall;
        private System.Windows.Forms.ImageList imageListBig;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statusbarToggle;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem viewToggle;
        private System.Windows.Forms.ToolStripMenuItem openConfigurationForm;
        private System.Windows.Forms.ToolStripMenuItem viewDetails;
        private System.Windows.Forms.ToolStripMenuItem viewLargeIcons;
        private System.Windows.Forms.ToolStripMenuItem viewSmallIcons;
        private System.Windows.Forms.ToolStripMenuItem viewList;
        private System.Windows.Forms.ToolStripMenuItem viewTiles;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}

