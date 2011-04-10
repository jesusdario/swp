namespace SWPEditor.IU
{
    partial class SWPEditorIU
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SWPEditorIU));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ediciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cortarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copiarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pegarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imprimirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.formatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.negrillaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cursivaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subrayadoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enlargeFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reduceFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeFontColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeFontBackgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolBar = new System.Windows.Forms.ToolStrip();
            this.Bold = new System.Windows.Forms.ToolStripButton();
            this.Italic = new System.Windows.Forms.ToolStripButton();
            this.Underline = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.IncreaseFontSize = new System.Windows.Forms.ToolStripButton();
            this.ReduceFontSize = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.FontSelection = new System.Windows.Forms.ToolStripButton();
            this.FontSizeList = new System.Windows.Forms.ToolStripComboBox();
            this.AlignLeft = new System.Windows.Forms.ToolStripButton();
            this.AlignCenter = new System.Windows.Forms.ToolStripButton();
            this.AlignRight = new System.Windows.Forms.ToolStripButton();
            this.FontColor = new System.Windows.Forms.ToolStripButton();
            this.BackgroundColor = new System.Windows.Forms.ToolStripButton();
            this.IncreaseLineSpace = new System.Windows.Forms.ToolStripButton();
            this.DecreaseLineSpace = new System.Windows.Forms.ToolStripButton();
            this.IncreaseSpaceBeforeParagraph = new System.Windows.Forms.ToolStripButton();
            this.DecreaseSpaceBeforeParagraph = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.LabelPage = new System.Windows.Forms.ToolStripStatusLabel();
            this.LabelLine = new System.Windows.Forms.ToolStripStatusLabel();
            this.LabelCharacter = new System.Windows.Forms.ToolStripStatusLabel();
            this.swpEditor1 = new SWPEditor.IU.SWPEditorControl();
            this.menuStrip1.SuspendLayout();
            this.ToolBar.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ediciónToolStripMenuItem,
            this.formatToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(538, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ediciónToolStripMenuItem
            // 
            this.ediciónToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cortarToolStripMenuItem,
            this.copiarToolStripMenuItem,
            this.pegarToolStripMenuItem,
            this.imprimirToolStripMenuItem,
            this.toolStripSeparator3,
            this.selectAllToolStripMenuItem});
            this.ediciónToolStripMenuItem.Name = "ediciónToolStripMenuItem";
            this.ediciónToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.ediciónToolStripMenuItem.Text = "Edit";
            // 
            // cortarToolStripMenuItem
            // 
            this.cortarToolStripMenuItem.Name = "cortarToolStripMenuItem";
            this.cortarToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.cortarToolStripMenuItem.Text = "Cut";
            this.cortarToolStripMenuItem.Click += new System.EventHandler(this.cortarToolStripMenuItem_Click);
            // 
            // copiarToolStripMenuItem
            // 
            this.copiarToolStripMenuItem.Name = "copiarToolStripMenuItem";
            this.copiarToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.copiarToolStripMenuItem.Text = "Copy";
            this.copiarToolStripMenuItem.Click += new System.EventHandler(this.copiarToolStripMenuItem_Click);
            // 
            // pegarToolStripMenuItem
            // 
            this.pegarToolStripMenuItem.Name = "pegarToolStripMenuItem";
            this.pegarToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.pegarToolStripMenuItem.Text = "Paste";
            this.pegarToolStripMenuItem.Click += new System.EventHandler(this.pegarToolStripMenuItem_Click);
            // 
            // imprimirToolStripMenuItem
            // 
            this.imprimirToolStripMenuItem.Name = "imprimirToolStripMenuItem";
            this.imprimirToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.imprimirToolStripMenuItem.Text = "Print";
            this.imprimirToolStripMenuItem.Click += new System.EventHandler(this.imprimirToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(119, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // formatToolStripMenuItem
            // 
            this.formatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.negrillaToolStripMenuItem,
            this.cursivaToolStripMenuItem,
            this.subrayadoToolStripMenuItem,
            this.enlargeFontToolStripMenuItem,
            this.reduceFontToolStripMenuItem,
            this.changeFontColorToolStripMenuItem,
            this.changeFontBackgroundToolStripMenuItem,
            this.changeFontToolStripMenuItem});
            this.formatToolStripMenuItem.Name = "formatToolStripMenuItem";
            this.formatToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.formatToolStripMenuItem.Text = "Format";
            // 
            // negrillaToolStripMenuItem
            // 
            this.negrillaToolStripMenuItem.Name = "negrillaToolStripMenuItem";
            this.negrillaToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.negrillaToolStripMenuItem.Text = "Bold";
            this.negrillaToolStripMenuItem.Click += new System.EventHandler(this.Bold_Click);
            // 
            // cursivaToolStripMenuItem
            // 
            this.cursivaToolStripMenuItem.Name = "cursivaToolStripMenuItem";
            this.cursivaToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.cursivaToolStripMenuItem.Text = "Italic";
            this.cursivaToolStripMenuItem.Click += new System.EventHandler(this.Italic_Click);
            // 
            // subrayadoToolStripMenuItem
            // 
            this.subrayadoToolStripMenuItem.Name = "subrayadoToolStripMenuItem";
            this.subrayadoToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.subrayadoToolStripMenuItem.Text = "Underlined";
            this.subrayadoToolStripMenuItem.Click += new System.EventHandler(this.Underline_Click);
            // 
            // enlargeFontToolStripMenuItem
            // 
            this.enlargeFontToolStripMenuItem.Name = "enlargeFontToolStripMenuItem";
            this.enlargeFontToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.enlargeFontToolStripMenuItem.Text = "Increase Font Size";
            this.enlargeFontToolStripMenuItem.Click += new System.EventHandler(this.enlargeFontToolStripMenuItem_Click);
            // 
            // reduceFontToolStripMenuItem
            // 
            this.reduceFontToolStripMenuItem.Name = "reduceFontToolStripMenuItem";
            this.reduceFontToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.reduceFontToolStripMenuItem.Text = "Decrease Font Size";
            this.reduceFontToolStripMenuItem.Click += new System.EventHandler(this.reduceFontToolStripMenuItem_Click);
            // 
            // changeFontColorToolStripMenuItem
            // 
            this.changeFontColorToolStripMenuItem.Name = "changeFontColorToolStripMenuItem";
            this.changeFontColorToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.changeFontColorToolStripMenuItem.Text = "Change Font Color...";
            this.changeFontColorToolStripMenuItem.Click += new System.EventHandler(this.changeFontColorToolStripMenuItem_Click);
            // 
            // changeFontBackgroundToolStripMenuItem
            // 
            this.changeFontBackgroundToolStripMenuItem.Name = "changeFontBackgroundToolStripMenuItem";
            this.changeFontBackgroundToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.changeFontBackgroundToolStripMenuItem.Text = "Change Font Background...";
            this.changeFontBackgroundToolStripMenuItem.Click += new System.EventHandler(this.changeFontBackgroundToolStripMenuItem_Click);
            // 
            // changeFontToolStripMenuItem
            // 
            this.changeFontToolStripMenuItem.Name = "changeFontToolStripMenuItem";
            this.changeFontToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.changeFontToolStripMenuItem.Text = "Change Font...";
            this.changeFontToolStripMenuItem.Click += new System.EventHandler(this.changeFontToolStripMenuItem_Click);
            // 
            // ToolBar
            // 
            this.ToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Bold,
            this.Italic,
            this.Underline,
            this.toolStripSeparator1,
            this.IncreaseFontSize,
            this.ReduceFontSize,
            this.toolStripSeparator2,
            this.FontSelection,
            this.FontSizeList,
            this.AlignLeft,
            this.AlignCenter,
            this.AlignRight,
            this.FontColor,
            this.BackgroundColor,
            this.IncreaseLineSpace,
            this.DecreaseLineSpace,
            this.IncreaseSpaceBeforeParagraph,
            this.DecreaseSpaceBeforeParagraph});
            this.ToolBar.Location = new System.Drawing.Point(0, 24);
            this.ToolBar.Name = "ToolBar";
            this.ToolBar.Size = new System.Drawing.Size(538, 25);
            this.ToolBar.TabIndex = 2;
            this.ToolBar.Text = "toolStrip1";
            // 
            // Bold
            // 
            this.Bold.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Bold.Image = ((System.Drawing.Image)(resources.GetObject("Bold.Image")));
            this.Bold.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Bold.Name = "Bold";
            this.Bold.Size = new System.Drawing.Size(23, 22);
            this.Bold.Text = "Bold";
            this.Bold.Click += new System.EventHandler(this.Bold_Click);
            // 
            // Italic
            // 
            this.Italic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Italic.Image = ((System.Drawing.Image)(resources.GetObject("Italic.Image")));
            this.Italic.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Italic.Name = "Italic";
            this.Italic.Size = new System.Drawing.Size(23, 22);
            this.Italic.Text = "Italic";
            this.Italic.ToolTipText = "Italic";
            this.Italic.Click += new System.EventHandler(this.Italic_Click);
            // 
            // Underline
            // 
            this.Underline.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Underline.Image = ((System.Drawing.Image)(resources.GetObject("Underline.Image")));
            this.Underline.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Underline.Name = "Underline";
            this.Underline.Size = new System.Drawing.Size(23, 22);
            this.Underline.Text = "Underline";
            this.Underline.ToolTipText = "Underline";
            this.Underline.Click += new System.EventHandler(this.Underline_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // IncreaseFontSize
            // 
            this.IncreaseFontSize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.IncreaseFontSize.Image = ((System.Drawing.Image)(resources.GetObject("IncreaseFontSize.Image")));
            this.IncreaseFontSize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.IncreaseFontSize.Name = "IncreaseFontSize";
            this.IncreaseFontSize.Size = new System.Drawing.Size(23, 22);
            this.IncreaseFontSize.Text = "Increase Font Size";
            this.IncreaseFontSize.Click += new System.EventHandler(this.IncreaseFontSize_Click);
            // 
            // ReduceFontSize
            // 
            this.ReduceFontSize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ReduceFontSize.Image = ((System.Drawing.Image)(resources.GetObject("ReduceFontSize.Image")));
            this.ReduceFontSize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ReduceFontSize.Name = "ReduceFontSize";
            this.ReduceFontSize.Size = new System.Drawing.Size(23, 22);
            this.ReduceFontSize.Text = "Reduce Font Size";
            this.ReduceFontSize.Click += new System.EventHandler(this.ReduceFontSize_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // FontSelection
            // 
            this.FontSelection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.FontSelection.Image = ((System.Drawing.Image)(resources.GetObject("FontSelection.Image")));
            this.FontSelection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FontSelection.Name = "FontSelection";
            this.FontSelection.Size = new System.Drawing.Size(23, 22);
            this.FontSelection.Text = "Font Selection";
            this.FontSelection.Click += new System.EventHandler(this.FontSelection_Click);
            // 
            // FontSizeList
            // 
            this.FontSizeList.DropDownWidth = 80;
            this.FontSizeList.Items.AddRange(new object[] {
            "8",
            "9",
            "10",
            "11",
            "12",
            "14",
            "16",
            "18",
            "20",
            "22",
            "24",
            "30",
            "40",
            "50",
            "60",
            "70"});
            this.FontSizeList.Name = "FontSizeList";
            this.FontSizeList.Size = new System.Drawing.Size(121, 25);
            this.FontSizeList.TextChanged += new System.EventHandler(this.FontSizeList_TextChanged);
            // 
            // AlignLeft
            // 
            this.AlignLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AlignLeft.Image = ((System.Drawing.Image)(resources.GetObject("AlignLeft.Image")));
            this.AlignLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AlignLeft.Name = "AlignLeft";
            this.AlignLeft.Size = new System.Drawing.Size(23, 22);
            this.AlignLeft.Text = "Align Left";
            this.AlignLeft.Click += new System.EventHandler(this.AlignLeft_Click);
            // 
            // AlignCenter
            // 
            this.AlignCenter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AlignCenter.Image = ((System.Drawing.Image)(resources.GetObject("AlignCenter.Image")));
            this.AlignCenter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AlignCenter.Name = "AlignCenter";
            this.AlignCenter.Size = new System.Drawing.Size(23, 22);
            this.AlignCenter.Text = "Align Center";
            this.AlignCenter.Click += new System.EventHandler(this.AlignCenter_Click);
            // 
            // AlignRight
            // 
            this.AlignRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AlignRight.Image = ((System.Drawing.Image)(resources.GetObject("AlignRight.Image")));
            this.AlignRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AlignRight.Name = "AlignRight";
            this.AlignRight.Size = new System.Drawing.Size(23, 22);
            this.AlignRight.Text = "Align Right";
            this.AlignRight.Click += new System.EventHandler(this.AlignRight_Click);
            // 
            // FontColor
            // 
            this.FontColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.FontColor.Image = ((System.Drawing.Image)(resources.GetObject("FontColor.Image")));
            this.FontColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FontColor.Name = "FontColor";
            this.FontColor.Size = new System.Drawing.Size(23, 22);
            this.FontColor.Text = "Font Color";
            this.FontColor.Click += new System.EventHandler(this.FontColor_Click);
            // 
            // BackgroundColor
            // 
            this.BackgroundColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BackgroundColor.Image = ((System.Drawing.Image)(resources.GetObject("BackgroundColor.Image")));
            this.BackgroundColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BackgroundColor.Name = "BackgroundColor";
            this.BackgroundColor.Size = new System.Drawing.Size(23, 22);
            this.BackgroundColor.Text = "Background Color";
            this.BackgroundColor.Click += new System.EventHandler(this.BackgroundColor_Click);
            // 
            // IncreaseLineSpace
            // 
            this.IncreaseLineSpace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.IncreaseLineSpace.Image = ((System.Drawing.Image)(resources.GetObject("IncreaseLineSpace.Image")));
            this.IncreaseLineSpace.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.IncreaseLineSpace.Name = "IncreaseLineSpace";
            this.IncreaseLineSpace.Size = new System.Drawing.Size(23, 22);
            this.IncreaseLineSpace.Text = "Increase Line Space";
            this.IncreaseLineSpace.Click += new System.EventHandler(this.IncreaseLineSpace_Click);
            // 
            // DecreaseLineSpace
            // 
            this.DecreaseLineSpace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DecreaseLineSpace.Image = ((System.Drawing.Image)(resources.GetObject("DecreaseLineSpace.Image")));
            this.DecreaseLineSpace.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DecreaseLineSpace.Name = "DecreaseLineSpace";
            this.DecreaseLineSpace.Size = new System.Drawing.Size(23, 22);
            this.DecreaseLineSpace.Text = "Decrease Line Space";
            this.DecreaseLineSpace.Click += new System.EventHandler(this.DecreaseLineSpace_Click);
            // 
            // IncreaseSpaceBeforeParagraph
            // 
            this.IncreaseSpaceBeforeParagraph.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.IncreaseSpaceBeforeParagraph.Image = ((System.Drawing.Image)(resources.GetObject("IncreaseSpaceBeforeParagraph.Image")));
            this.IncreaseSpaceBeforeParagraph.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.IncreaseSpaceBeforeParagraph.Name = "IncreaseSpaceBeforeParagraph";
            this.IncreaseSpaceBeforeParagraph.Size = new System.Drawing.Size(23, 22);
            this.IncreaseSpaceBeforeParagraph.Text = "IncreaseSpaceBeforeParagraph";
            this.IncreaseSpaceBeforeParagraph.Click += new System.EventHandler(this.IncreaseSpaceBeforeParagraph_Click);
            // 
            // DecreaseSpaceBeforeParagraph
            // 
            this.DecreaseSpaceBeforeParagraph.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DecreaseSpaceBeforeParagraph.Image = ((System.Drawing.Image)(resources.GetObject("DecreaseSpaceBeforeParagraph.Image")));
            this.DecreaseSpaceBeforeParagraph.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DecreaseSpaceBeforeParagraph.Name = "DecreaseSpaceBeforeParagraph";
            this.DecreaseSpaceBeforeParagraph.Size = new System.Drawing.Size(23, 22);
            this.DecreaseSpaceBeforeParagraph.Text = "ReduceSpaceBeforeParagraph";
            this.DecreaseSpaceBeforeParagraph.Click += new System.EventHandler(this.DecreaseSpaceBeforeParagraph_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LabelPage,
            this.LabelLine,
            this.LabelCharacter});
            this.statusStrip1.Location = new System.Drawing.Point(0, 305);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(538, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // LabelPage
            // 
            this.LabelPage.Name = "LabelPage";
            this.LabelPage.Size = new System.Drawing.Size(0, 17);
            // 
            // LabelLine
            // 
            this.LabelLine.Name = "LabelLine";
            this.LabelLine.Size = new System.Drawing.Size(0, 17);
            // 
            // LabelCharacter
            // 
            this.LabelCharacter.Name = "LabelCharacter";
            this.LabelCharacter.Size = new System.Drawing.Size(0, 17);
            // 
            // swpEditor1
            // 
            this.swpEditor1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.swpEditor1.CausesValidation = false;
            this.swpEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.swpEditor1.Location = new System.Drawing.Point(0, 49);
            this.swpEditor1.Name = "swpEditor1";
            this.swpEditor1.Size = new System.Drawing.Size(538, 256);
            this.swpEditor1.TabIndex = 1;
            // 
            // SWPEditorIU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 327);
            this.Controls.Add(this.swpEditor1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.ToolBar);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SWPEditorIU";
            this.Text = "SWP. Simple Text Editor";
            this.Load += new System.EventHandler(this.PresentadorDocumento_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ToolBar.ResumeLayout(false);
            this.ToolBar.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SWPEditorControl swpEditor1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ediciónToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cortarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copiarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pegarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imprimirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStrip ToolBar;
        private System.Windows.Forms.ToolStripButton IncreaseFontSize;
        private System.Windows.Forms.ToolStripButton ReduceFontSize;
        private System.Windows.Forms.ToolStripButton Bold;
        private System.Windows.Forms.ToolStripButton Italic;
        private System.Windows.Forms.ToolStripButton Underline;
        private System.Windows.Forms.ToolStripButton FontSelection;
        private System.Windows.Forms.ToolStripComboBox FontSizeList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton AlignLeft;
        private System.Windows.Forms.ToolStripButton AlignCenter;
        private System.Windows.Forms.ToolStripButton AlignRight;
        private System.Windows.Forms.ToolStripMenuItem formatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem negrillaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cursivaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem subrayadoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enlargeFontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reduceFontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeFontColorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeFontBackgroundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeFontToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton FontColor;
        private System.Windows.Forms.ToolStripButton BackgroundColor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton IncreaseLineSpace;
        private System.Windows.Forms.ToolStripButton DecreaseLineSpace;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel LabelPage;
        private System.Windows.Forms.ToolStripStatusLabel LabelLine;
        private System.Windows.Forms.ToolStripStatusLabel LabelCharacter;
        private System.Windows.Forms.ToolStripButton IncreaseSpaceBeforeParagraph;
        private System.Windows.Forms.ToolStripButton DecreaseSpaceBeforeParagraph;
    }
}