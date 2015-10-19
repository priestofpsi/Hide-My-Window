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
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToggle = new System.Windows.Forms.ToolStripMenuItem();
            this.viewDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.viewLargeIcons = new System.Windows.Forms.ToolStripMenuItem();
            this.viewSmallIcons = new System.Windows.Forms.ToolStripMenuItem();
            this.viewList = new System.Windows.Forms.ToolStripMenuItem();
            this.viewTiles = new System.Windows.Forms.ToolStripMenuItem();
            this.toolbarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showToolbarText = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.largeToolbarIcons = new System.Windows.Forms.ToolStripMenuItem();
            this.smallToolbarIcons = new System.Windows.Forms.ToolStripMenuItem();
            this.statusbarToggle = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.openConfigurationForm = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.imageListBig = new System.Windows.Forms.ImageList(this.components);
            this.imageListSmall = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.showWindow = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.pinWindow = new System.Windows.Forms.ToolStripButton();
            this.renameWindow = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.lockWindow = new System.Windows.Forms.ToolStripButton();
            this.unlockWindow = new System.Windows.Forms.ToolStripButton();
            this.actionImageListSmall = new System.Windows.Forms.ImageList(this.components);
            this.actionImageList = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.hiddenWindows = new theDiary.Tools.HideMyWindow.HiddenWindowsListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hiddenWindowsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.protectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unprotectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.hiddenWindowsContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // showAll
            // 
            this.showAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.showAll.Enabled = false;
            this.showAll.Location = new System.Drawing.Point(384, 300);
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
            this.show.Location = new System.Drawing.Point(303, 300);
            this.show.Name = "show";
            this.show.Size = new System.Drawing.Size(75, 23);
            this.show.TabIndex = 2;
            this.show.Text = "&Show";
            this.show.UseVisualStyleBackColor = true;
            this.show.Click += new System.EventHandler(this.ToggleHiddenWindows);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 326);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(471, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(408, 17);
            this.statusLabel.Spring = true;
            this.statusLabel.Text = "Windows: 0";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStatusLabel3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStatusLabel3.Image")));
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabel3.Text = "toolStripStatusLabel3";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStatusLabel1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStatusLabel1.Image")));
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStatusLabel2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStatusLabel2.Image")));
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(471, 24);
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
            this.exitToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exitToolStripMenuItem.Image")));
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitApplication);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToggle,
            this.toolbarToolStripMenuItem,
            this.statusbarToggle,
            this.toolStripMenuItem1,
            this.openConfigurationForm});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.viewToolStripMenuItem.Text = "&Options";
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
            this.viewDetails.Tag = System.Windows.Forms.View.Details;
            this.viewDetails.Text = "&Details";
            this.viewDetails.Click += new System.EventHandler(this.ToggleView);
            // 
            // viewLargeIcons
            // 
            this.viewLargeIcons.Name = "viewLargeIcons";
            this.viewLargeIcons.Size = new System.Drawing.Size(134, 22);
            this.viewLargeIcons.Tag = System.Windows.Forms.View.LargeIcon;
            this.viewLargeIcons.Text = "&Large Icons";
            this.viewLargeIcons.Click += new System.EventHandler(this.ToggleView);
            // 
            // viewSmallIcons
            // 
            this.viewSmallIcons.Name = "viewSmallIcons";
            this.viewSmallIcons.Size = new System.Drawing.Size(134, 22);
            this.viewSmallIcons.Tag = System.Windows.Forms.View.SmallIcon;
            this.viewSmallIcons.Text = "&Small Icons";
            this.viewSmallIcons.Click += new System.EventHandler(this.ToggleView);
            // 
            // viewList
            // 
            this.viewList.Name = "viewList";
            this.viewList.Size = new System.Drawing.Size(134, 22);
            this.viewList.Tag = System.Windows.Forms.View.List;
            this.viewList.Text = "L&ist";
            this.viewList.Click += new System.EventHandler(this.ToggleView);
            // 
            // viewTiles
            // 
            this.viewTiles.Name = "viewTiles";
            this.viewTiles.Size = new System.Drawing.Size(134, 22);
            this.viewTiles.Tag = System.Windows.Forms.View.Tile;
            this.viewTiles.Text = "&Tiles";
            this.viewTiles.Click += new System.EventHandler(this.ToggleView);
            // 
            // toolbarToolStripMenuItem
            // 
            this.toolbarToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolbarText,
            this.toolStripMenuItem2,
            this.largeToolbarIcons,
            this.smallToolbarIcons});
            this.toolbarToolStripMenuItem.Name = "toolbarToolStripMenuItem";
            this.toolbarToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.toolbarToolStripMenuItem.Text = "&Toolbar";
            this.toolbarToolStripMenuItem.DropDownOpening += new System.EventHandler(this.toolbarToggle_DropDownOpening);
            // 
            // showToolbarText
            // 
            this.showToolbarText.CheckOnClick = true;
            this.showToolbarText.Name = "showToolbarText";
            this.showToolbarText.Size = new System.Drawing.Size(134, 22);
            this.showToolbarText.Text = "Show &Text";
            this.showToolbarText.Click += new System.EventHandler(this.showToolbarText_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(131, 6);
            // 
            // largeToolbarIcons
            // 
            this.largeToolbarIcons.Name = "largeToolbarIcons";
            this.largeToolbarIcons.Size = new System.Drawing.Size(134, 22);
            this.largeToolbarIcons.Text = "Large Icons";
            this.largeToolbarIcons.Click += new System.EventHandler(this.largeIconsToolStripMenuItem_Click);
            // 
            // smallToolbarIcons
            // 
            this.smallToolbarIcons.Name = "smallToolbarIcons";
            this.smallToolbarIcons.Size = new System.Drawing.Size(134, 22);
            this.smallToolbarIcons.Text = "&Small Icons";
            this.smallToolbarIcons.Click += new System.EventHandler(this.smallIconsToolStripMenuItem_Click);
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
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(124, 6);
            // 
            // openConfigurationForm
            // 
            this.openConfigurationForm.Image = ((System.Drawing.Image)(resources.GetObject("openConfigurationForm.Image")));
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
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showWindow,
            this.toolStripSeparator1,
            this.pinWindow,
            this.renameWindow,
            this.toolStripSeparator2,
            this.lockWindow,
            this.unlockWindow});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(471, 86);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // showWindow
            // 
            this.showWindow.Enabled = false;
            this.showWindow.Image = ((System.Drawing.Image)(resources.GetObject("showWindow.Image")));
            this.showWindow.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.showWindow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.showWindow.Name = "showWindow";
            this.showWindow.Size = new System.Drawing.Size(87, 83);
            this.showWindow.Text = "Show Window";
            this.showWindow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.showWindow.Click += new System.EventHandler(this.ToggleHiddenWindows);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 86);
            // 
            // pinWindow
            // 
            this.pinWindow.Image = ((System.Drawing.Image)(resources.GetObject("pinWindow.Image")));
            this.pinWindow.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pinWindow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pinWindow.Name = "pinWindow";
            this.pinWindow.Size = new System.Drawing.Size(68, 83);
            this.pinWindow.Text = "Pin";
            this.pinWindow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.pinWindow.ToolTipText = "A pinned window will not be removed from the list of hidden windows.";
            this.pinWindow.Click += new System.EventHandler(this.pinWindow_Click);
            // 
            // renameWindow
            // 
            this.renameWindow.Image = ((System.Drawing.Image)(resources.GetObject("renameWindow.Image")));
            this.renameWindow.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.renameWindow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.renameWindow.Name = "renameWindow";
            this.renameWindow.Size = new System.Drawing.Size(68, 83);
            this.renameWindow.Text = "Rename";
            this.renameWindow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.renameWindow.ToolTipText = "A pinned window will not be removed from the list of hidden windows.";
            this.renameWindow.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 86);
            // 
            // lockWindow
            // 
            this.lockWindow.Image = ((System.Drawing.Image)(resources.GetObject("lockWindow.Image")));
            this.lockWindow.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.lockWindow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.lockWindow.Name = "lockWindow";
            this.lockWindow.Size = new System.Drawing.Size(68, 83);
            this.lockWindow.Text = "Protect";
            this.lockWindow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.lockWindow.ToolTipText = "Enables password protection from the selected Window(s)";
            this.lockWindow.Click += new System.EventHandler(this.lockWindow_Click);
            // 
            // unlockWindow
            // 
            this.unlockWindow.Image = ((System.Drawing.Image)(resources.GetObject("unlockWindow.Image")));
            this.unlockWindow.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.unlockWindow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.unlockWindow.Name = "unlockWindow";
            this.unlockWindow.Size = new System.Drawing.Size(68, 83);
            this.unlockWindow.Text = "Unlock";
            this.unlockWindow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.unlockWindow.ToolTipText = "Removes the Password protection from the selected Window(s)";
            // 
            // actionImageListSmall
            // 
            this.actionImageListSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("actionImageListSmall.ImageStream")));
            this.actionImageListSmall.TransparentColor = System.Drawing.Color.Transparent;
            this.actionImageListSmall.Images.SetKeyName(0, "show-small.png");
            this.actionImageListSmall.Images.SetKeyName(1, "hide-small.png");
            this.actionImageListSmall.Images.SetKeyName(2, "lock-small.png");
            this.actionImageListSmall.Images.SetKeyName(3, "unlock-small.png");
            this.actionImageListSmall.Images.SetKeyName(4, "rename-small.png");
            // 
            // actionImageList
            // 
            this.actionImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("actionImageList.ImageStream")));
            this.actionImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.actionImageList.Images.SetKeyName(0, "show.png");
            this.actionImageList.Images.SetKeyName(1, "hide.png");
            this.actionImageList.Images.SetKeyName(2, "lock.png");
            this.actionImageList.Images.SetKeyName(3, "unlock.png");
            this.actionImageList.Images.SetKeyName(4, "rename.png");
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.hiddenWindows);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 110);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(471, 216);
            this.panel1.TabIndex = 7;
            // 
            // hiddenWindows
            // 
            this.hiddenWindows.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hiddenWindows.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader2});
            this.hiddenWindows.ContextMenuStrip = this.hiddenWindowsContextMenu;
            this.hiddenWindows.LabelEdit = true;
            this.hiddenWindows.LargeImageList = this.imageListBig;
            this.hiddenWindows.Location = new System.Drawing.Point(12, 15);
            this.hiddenWindows.Name = "hiddenWindows";
            this.hiddenWindows.Size = new System.Drawing.Size(446, 188);
            this.hiddenWindows.SmallImageList = this.imageListSmall;
            this.hiddenWindows.TabIndex = 6;
            this.hiddenWindows.UseCompatibleStateImageBehavior = false;
            this.hiddenWindows.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.hiddenWindows_AfterLabelEdit);
            this.hiddenWindows.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.hiddenWindows_DrawSubItem);
            this.hiddenWindows.SelectedIndexChanged += new System.EventHandler(this.hiddenWindows_SelectedIndexChanged);
            this.hiddenWindows.DoubleClick += new System.EventHandler(this.ToggleHiddenWindows);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Title";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Protected";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Pinned";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Path";
            // 
            // hiddenWindowsContextMenu
            // 
            this.hiddenWindowsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showWindowToolStripMenuItem,
            this.toolStripMenuItem3,
            this.protectToolStripMenuItem,
            this.unprotectToolStripMenuItem,
            this.toolStripMenuItem4,
            this.renameToolStripMenuItem});
            this.hiddenWindowsContextMenu.Name = "hiddenWindowsContextMenu";
            this.hiddenWindowsContextMenu.Size = new System.Drawing.Size(153, 126);
            this.hiddenWindowsContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.hiddenWindowsContextMenu_Opening);
            // 
            // showWindowToolStripMenuItem
            // 
            this.showWindowToolStripMenuItem.Image = global::theDiary.Tools.HideMyWindow.ActionResource.show_small;
            this.showWindowToolStripMenuItem.Name = "showWindowToolStripMenuItem";
            this.showWindowToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.showWindowToolStripMenuItem.Text = "&Show Window";
            this.showWindowToolStripMenuItem.Click += new System.EventHandler(this.ToggleHiddenWindows);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(149, 6);
            // 
            // protectToolStripMenuItem
            // 
            this.protectToolStripMenuItem.Image = global::theDiary.Tools.HideMyWindow.ActionResource.lockwindow_small;
            this.protectToolStripMenuItem.Name = "protectToolStripMenuItem";
            this.protectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.protectToolStripMenuItem.Text = "&Protect";
            this.protectToolStripMenuItem.Click += new System.EventHandler(this.lockWindow_Click);
            // 
            // unprotectToolStripMenuItem
            // 
            this.unprotectToolStripMenuItem.Image = global::theDiary.Tools.HideMyWindow.ActionResource.unlockwindow_small;
            this.unprotectToolStripMenuItem.Name = "unprotectToolStripMenuItem";
            this.unprotectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.unprotectToolStripMenuItem.Text = "&Unprotect";
            this.unprotectToolStripMenuItem.Click += new System.EventHandler(this.unlockWindow_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(149, 6);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Image = global::theDiary.Tools.HideMyWindow.ActionResource.rename_small;
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.renameToolStripMenuItem.Text = "&Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 348);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.show);
            this.Controls.Add(this.showAll);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(300, 350);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Hide My Window";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.hiddenWindowsContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStrip toolStrip1;
        private ToolStripButton showWindow;
        private ToolStripButton lockWindow;
        private ImageList actionImageListSmall;
        private ImageList actionImageList;
        private Panel panel1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem toolbarToolStripMenuItem;
        private ToolStripMenuItem showToolbarText;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem largeToolbarIcons;
        private ToolStripMenuItem smallToolbarIcons;
        private ContextMenuStrip hiddenWindowsContextMenu;
        private ToolStripMenuItem showWindowToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripMenuItem protectToolStripMenuItem;
        private ToolStripMenuItem unprotectToolStripMenuItem;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ToolStripSeparator toolStripMenuItem4;
        private ToolStripMenuItem renameToolStripMenuItem;
        private ToolStripButton pinWindow;
        private HiddenWindowsListView hiddenWindows;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton renameWindow;
        private ToolStripStatusLabel toolStripStatusLabel3;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ToolStripButton unlockWindow;
    }
}

