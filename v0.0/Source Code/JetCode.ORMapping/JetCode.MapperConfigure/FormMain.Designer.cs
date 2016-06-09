namespace JetCode.MapperConfigure
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuMainFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mappingNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainTools = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSynchronize = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCheckErrors = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.addNewSchemaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolConnect = new System.Windows.Forms.ToolStripButton();
            this.toolOpen = new System.Windows.Forms.ToolStripButton();
            this.toolSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolHelp = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusMain = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lstObjects = new System.Windows.Forms.ListBox();
            this.tableCtrl1 = new JetCode.MapperConfigure.Controls.TableAliasCtrl();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabFields = new System.Windows.Forms.TabPage();
            this.gridFieldCtrl1 = new JetCode.MapperConfigure.Controls.GridFieldCtrl();
            this.tabJoins = new System.Windows.Forms.TabPage();
            this.gridJoinCtrl1 = new JetCode.MapperConfigure.Controls.GridJoinCtrl();
            this.tabChildren = new System.Windows.Forms.TabPage();
            this.gridChildCtrl1 = new JetCode.MapperConfigure.Controls.GridChildCtrl();
            this.tabParents = new System.Windows.Forms.TabPage();
            this.gridParentCtrl1 = new JetCode.MapperConfigure.Controls.GridParentCtrl();
            this.tabIndexs = new System.Windows.Forms.TabPage();
            this.gridIndexCtrl1 = new JetCode.MapperConfigure.Controls.GridIndexCtrl();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabFields.SuspendLayout();
            this.tabJoins.SuspendLayout();
            this.tabChildren.SuspendLayout();
            this.tabParents.SuspendLayout();
            this.tabIndexs.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainFile,
            this.menuMainTools,
            this.menuMainHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(701, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuMainFile
            // 
            this.menuMainFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuConnect,
            this.menuOpen,
            this.toolStripSeparator3,
            this.menuSave,
            this.mappingNameToolStripMenuItem,
            this.toolStripSeparator1,
            this.menuExit});
            this.menuMainFile.Name = "menuMainFile";
            this.menuMainFile.Size = new System.Drawing.Size(37, 20);
            this.menuMainFile.Text = "&File";
            // 
            // menuConnect
            // 
            this.menuConnect.Name = "menuConnect";
            this.menuConnect.Size = new System.Drawing.Size(166, 22);
            this.menuConnect.Text = "&New...";
            this.menuConnect.Click += new System.EventHandler(this.menuConnect_Click);
            // 
            // menuOpen
            // 
            this.menuOpen.Name = "menuOpen";
            this.menuOpen.Size = new System.Drawing.Size(166, 22);
            this.menuOpen.Text = "&Open...";
            this.menuOpen.Click += new System.EventHandler(this.menuOpen_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(163, 6);
            // 
            // menuSave
            // 
            this.menuSave.Enabled = false;
            this.menuSave.Name = "menuSave";
            this.menuSave.Size = new System.Drawing.Size(166, 22);
            this.menuSave.Text = "&Save";
            this.menuSave.Click += new System.EventHandler(this.menuSave_Click);
            // 
            // mappingNameToolStripMenuItem
            // 
            this.mappingNameToolStripMenuItem.Name = "mappingNameToolStripMenuItem";
            this.mappingNameToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.mappingNameToolStripMenuItem.Text = "&Mapping Name...";
            this.mappingNameToolStripMenuItem.Click += new System.EventHandler(this.mappingNameToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(163, 6);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(166, 22);
            this.menuExit.Text = "&Exit";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // menuMainTools
            // 
            this.menuMainTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSynchronize,
            this.menuCheckErrors,
            this.toolStripSeparator4,
            this.addNewSchemaToolStripMenuItem});
            this.menuMainTools.Enabled = false;
            this.menuMainTools.Name = "menuMainTools";
            this.menuMainTools.Size = new System.Drawing.Size(48, 20);
            this.menuMainTools.Text = "&Tools";
            // 
            // menuSynchronize
            // 
            this.menuSynchronize.Name = "menuSynchronize";
            this.menuSynchronize.Size = new System.Drawing.Size(168, 22);
            this.menuSynchronize.Text = "&Synchronize...";
            this.menuSynchronize.Click += new System.EventHandler(this.menuSynchronize_Click);
            // 
            // menuCheckErrors
            // 
            this.menuCheckErrors.Name = "menuCheckErrors";
            this.menuCheckErrors.Size = new System.Drawing.Size(168, 22);
            this.menuCheckErrors.Text = "&Check Errors...";
            this.menuCheckErrors.Click += new System.EventHandler(this.menuCheckErrors_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(165, 6);
            // 
            // addNewSchemaToolStripMenuItem
            // 
            this.addNewSchemaToolStripMenuItem.Name = "addNewSchemaToolStripMenuItem";
            this.addNewSchemaToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.addNewSchemaToolStripMenuItem.Text = "&Add New Schema";
            this.addNewSchemaToolStripMenuItem.Click += new System.EventHandler(this.addNewSchemaToolStripMenuItem_Click);
            // 
            // menuMainHelp
            // 
            this.menuMainHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAbout});
            this.menuMainHelp.Name = "menuMainHelp";
            this.menuMainHelp.Size = new System.Drawing.Size(44, 20);
            this.menuMainHelp.Text = "&Help";
            // 
            // menuAbout
            // 
            this.menuAbout.Name = "menuAbout";
            this.menuAbout.Size = new System.Drawing.Size(116, 22);
            this.menuAbout.Text = "&About...";
            this.menuAbout.Click += new System.EventHandler(this.menuAbout_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolConnect,
            this.toolOpen,
            this.toolSave,
            this.toolStripSeparator2,
            this.toolHelp});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(701, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolConnect
            // 
            this.toolConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolConnect.Image = ((System.Drawing.Image)(resources.GetObject("toolConnect.Image")));
            this.toolConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolConnect.Name = "toolConnect";
            this.toolConnect.Size = new System.Drawing.Size(23, 22);
            this.toolConnect.Text = "&New";
            this.toolConnect.Click += new System.EventHandler(this.menuConnect_Click);
            // 
            // toolOpen
            // 
            this.toolOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolOpen.Image")));
            this.toolOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolOpen.Name = "toolOpen";
            this.toolOpen.Size = new System.Drawing.Size(23, 22);
            this.toolOpen.Text = "&Open";
            this.toolOpen.Click += new System.EventHandler(this.menuOpen_Click);
            // 
            // toolSave
            // 
            this.toolSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolSave.Enabled = false;
            this.toolSave.Image = ((System.Drawing.Image)(resources.GetObject("toolSave.Image")));
            this.toolSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSave.Name = "toolSave";
            this.toolSave.Size = new System.Drawing.Size(23, 22);
            this.toolSave.Text = "&Save";
            this.toolSave.Click += new System.EventHandler(this.menuSave_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolHelp
            // 
            this.toolHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolHelp.Image = ((System.Drawing.Image)(resources.GetObject("toolHelp.Image")));
            this.toolHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolHelp.Name = "toolHelp";
            this.toolHelp.Size = new System.Drawing.Size(23, 22);
            this.toolHelp.Text = "He&lp";
            this.toolHelp.Click += new System.EventHandler(this.menuAbout_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusMain});
            this.statusStrip1.Location = new System.Drawing.Point(0, 397);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(701, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusMain
            // 
            this.statusMain.Name = "statusMain";
            this.statusMain.Size = new System.Drawing.Size(39, 17);
            this.statusMain.Text = "Ready";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 49);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lstObjects);
            this.splitContainer1.Panel1.Controls.Add(this.tableCtrl1);
            this.splitContainer1.Panel1MinSize = 100;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(701, 348);
            this.splitContainer1.SplitterDistance = 106;
            this.splitContainer1.TabIndex = 3;
            // 
            // lstObjects
            // 
            this.lstObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstObjects.FormattingEnabled = true;
            this.lstObjects.Location = new System.Drawing.Point(0, 0);
            this.lstObjects.Name = "lstObjects";
            this.lstObjects.Size = new System.Drawing.Size(106, 277);
            this.lstObjects.Sorted = true;
            this.lstObjects.TabIndex = 0;
            this.lstObjects.SelectedIndexChanged += new System.EventHandler(this.lstObjects_SelectedIndexChanged);
            this.lstObjects.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstObjects_KeyUp);
            // 
            // tableCtrl1
            // 
            this.tableCtrl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableCtrl1.Enabled = false;
            this.tableCtrl1.Location = new System.Drawing.Point(0, 283);
            this.tableCtrl1.Name = "tableCtrl1";
            this.tableCtrl1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableCtrl1.Size = new System.Drawing.Size(106, 65);
            this.tableCtrl1.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabFields);
            this.tabControl1.Controls.Add(this.tabJoins);
            this.tabControl1.Controls.Add(this.tabChildren);
            this.tabControl1.Controls.Add(this.tabParents);
            this.tabControl1.Controls.Add(this.tabIndexs);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(591, 348);
            this.tabControl1.TabIndex = 0;
            // 
            // tabFields
            // 
            this.tabFields.Controls.Add(this.gridFieldCtrl1);
            this.tabFields.Location = new System.Drawing.Point(4, 22);
            this.tabFields.Name = "tabFields";
            this.tabFields.Padding = new System.Windows.Forms.Padding(3);
            this.tabFields.Size = new System.Drawing.Size(583, 322);
            this.tabFields.TabIndex = 0;
            this.tabFields.Text = "Fields";
            this.tabFields.UseVisualStyleBackColor = true;
            // 
            // gridFieldCtrl1
            // 
            this.gridFieldCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridFieldCtrl1.Location = new System.Drawing.Point(3, 3);
            this.gridFieldCtrl1.Name = "gridFieldCtrl1";
            this.gridFieldCtrl1.Size = new System.Drawing.Size(577, 316);
            this.gridFieldCtrl1.TabIndex = 0;
            // 
            // tabJoins
            // 
            this.tabJoins.Controls.Add(this.gridJoinCtrl1);
            this.tabJoins.Location = new System.Drawing.Point(4, 22);
            this.tabJoins.Name = "tabJoins";
            this.tabJoins.Padding = new System.Windows.Forms.Padding(3);
            this.tabJoins.Size = new System.Drawing.Size(583, 322);
            this.tabJoins.TabIndex = 1;
            this.tabJoins.Text = "Joins";
            this.tabJoins.UseVisualStyleBackColor = true;
            // 
            // gridJoinCtrl1
            // 
            this.gridJoinCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridJoinCtrl1.Location = new System.Drawing.Point(3, 3);
            this.gridJoinCtrl1.Name = "gridJoinCtrl1";
            this.gridJoinCtrl1.Size = new System.Drawing.Size(577, 316);
            this.gridJoinCtrl1.TabIndex = 0;
            // 
            // tabChildren
            // 
            this.tabChildren.Controls.Add(this.gridChildCtrl1);
            this.tabChildren.Location = new System.Drawing.Point(4, 22);
            this.tabChildren.Name = "tabChildren";
            this.tabChildren.Padding = new System.Windows.Forms.Padding(3);
            this.tabChildren.Size = new System.Drawing.Size(583, 322);
            this.tabChildren.TabIndex = 2;
            this.tabChildren.Text = "Children";
            this.tabChildren.UseVisualStyleBackColor = true;
            // 
            // gridChildCtrl1
            // 
            this.gridChildCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridChildCtrl1.Location = new System.Drawing.Point(3, 3);
            this.gridChildCtrl1.Name = "gridChildCtrl1";
            this.gridChildCtrl1.Size = new System.Drawing.Size(577, 316);
            this.gridChildCtrl1.TabIndex = 0;
            // 
            // tabParents
            // 
            this.tabParents.Controls.Add(this.gridParentCtrl1);
            this.tabParents.Location = new System.Drawing.Point(4, 22);
            this.tabParents.Name = "tabParents";
            this.tabParents.Padding = new System.Windows.Forms.Padding(3);
            this.tabParents.Size = new System.Drawing.Size(583, 322);
            this.tabParents.TabIndex = 3;
            this.tabParents.Text = "Parents";
            this.tabParents.UseVisualStyleBackColor = true;
            // 
            // gridParentCtrl1
            // 
            this.gridParentCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridParentCtrl1.Location = new System.Drawing.Point(3, 3);
            this.gridParentCtrl1.Name = "gridParentCtrl1";
            this.gridParentCtrl1.Size = new System.Drawing.Size(577, 316);
            this.gridParentCtrl1.TabIndex = 0;
            // 
            // tabIndexs
            // 
            this.tabIndexs.Controls.Add(this.gridIndexCtrl1);
            this.tabIndexs.Location = new System.Drawing.Point(4, 22);
            this.tabIndexs.Name = "tabIndexs";
            this.tabIndexs.Padding = new System.Windows.Forms.Padding(3);
            this.tabIndexs.Size = new System.Drawing.Size(583, 322);
            this.tabIndexs.TabIndex = 4;
            this.tabIndexs.Text = "Indexs";
            this.tabIndexs.UseVisualStyleBackColor = true;
            // 
            // gridIndexCtrl1
            // 
            this.gridIndexCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridIndexCtrl1.Location = new System.Drawing.Point(3, 3);
            this.gridIndexCtrl1.Name = "gridIndexCtrl1";
            this.gridIndexCtrl1.Size = new System.Drawing.Size(577, 316);
            this.gridIndexCtrl1.TabIndex = 0;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            this.openFileDialog1.Title = "Open Configure File";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "Schema";
            this.saveFileDialog1.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            this.saveFileDialog1.Title = "Save Configure File";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 419);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "O/R Mapper Configure";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabFields.ResumeLayout(false);
            this.tabJoins.ResumeLayout(false);
            this.tabChildren.ResumeLayout(false);
            this.tabParents.ResumeLayout(false);
            this.tabIndexs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem menuMainFile;
        private System.Windows.Forms.ToolStripMenuItem menuConnect;
        private System.Windows.Forms.ToolStripMenuItem menuOpen;
        private System.Windows.Forms.ToolStripMenuItem menuSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.ToolStripMenuItem menuMainHelp;
        private System.Windows.Forms.ToolStripStatusLabel statusMain;
        private System.Windows.Forms.ToolStripButton toolConnect;
        private System.Windows.Forms.ToolStripButton toolOpen;
        private System.Windows.Forms.ToolStripButton toolSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolHelp;
        private System.Windows.Forms.ListBox lstObjects;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabFields;
        private System.Windows.Forms.TabPage tabJoins;
        private MapperConfigure.Controls.GridFieldCtrl gridFieldCtrl1;
        private MapperConfigure.Controls.GridJoinCtrl gridJoinCtrl1;
        private System.Windows.Forms.TabPage tabChildren;
        private MapperConfigure.Controls.GridChildCtrl gridChildCtrl1;
        private System.Windows.Forms.ToolStripMenuItem menuAbout;
        private JetCode.MapperConfigure.Controls.TableAliasCtrl tableCtrl1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuMainTools;
        private System.Windows.Forms.TabPage tabParents;
        private JetCode.MapperConfigure.Controls.GridParentCtrl gridParentCtrl1;
        private System.Windows.Forms.ToolStripMenuItem menuCheckErrors;
        private System.Windows.Forms.ToolStripMenuItem menuSynchronize;
        private System.Windows.Forms.TabPage tabIndexs;
        private JetCode.MapperConfigure.Controls.GridIndexCtrl gridIndexCtrl1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem addNewSchemaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mappingNameToolStripMenuItem;
    }
}