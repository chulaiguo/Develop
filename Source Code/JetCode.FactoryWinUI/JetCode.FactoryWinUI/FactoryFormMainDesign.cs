using System;
using System.IO;
using JetCode.Factory;
using JetCode.BizSchema;

namespace JetCode.FactoryWinUI
{
    public class FactoryFormMainDesign : FactoryBase
    {
        public FactoryFormMainDesign(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Manager", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic partial class FormMain");
            writer.WriteLine("\t{");

            writer.WriteLine("\t\t/// <summary>");
            writer.WriteLine("\t\t/// Required designer variable.");
            writer.WriteLine("\t\t/// </summary>");
            writer.WriteLine("\t\tprivate System.ComponentModel.IContainer components = null;");
            writer.WriteLine();

            writer.WriteLine("\t\t/// <summary>");
            writer.WriteLine("\t\t/// Clean up any resources being used.");
            writer.WriteLine("\t\t/// </summary>");
            writer.WriteLine("\t\t/// <param name=\"disposing\">true if managed resources should be disposed; otherwise, false.</param>");
            writer.WriteLine("\t\tprotected override void Dispose(bool disposing)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (disposing && (components != null))");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tcomponents.Dispose();");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tbase.Dispose(disposing);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            this.WriteInitializeComponent(writer);
            this.WriteFields(writer);
        }

        private void WriteInitializeComponent(StringWriter writer)
        {
            writer.WriteLine("\t\t#region Windows Form Designer generated code");
            writer.WriteLine();
            writer.WriteLine("\t\t/// <summary>");
            writer.WriteLine("\t\t/// Required method for Designer support - do not modify");
            writer.WriteLine("\t\t/// the contents of this method with the code editor.");
            writer.WriteLine("\t\t/// </summary>");
            writer.WriteLine("\t\tprivate void InitializeComponent()");
            writer.WriteLine("\t\t{");

            this.WriteBeginInit(writer);
            this.WriteDetail(writer);
            this.WriteEndInit(writer);

            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();
        }

        private void WriteBeginInit(StringWriter writer)
        {
            writer.WriteLine("\t\t\tthis.components = new System.ComponentModel.Container();");
            writer.WriteLine("\t\t\tthis.barDockControlTop = new DevExpress.XtraBars.BarDockControl();");
            writer.WriteLine("\t\t\tthis.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();");
            writer.WriteLine("\t\t\tthis.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();");
            writer.WriteLine("\t\t\tthis.barDockControlRight = new DevExpress.XtraBars.BarDockControl();");
            writer.WriteLine("\t\t\tthis.barManager1 = new DevExpress.XtraBars.BarManager(this.components);");
            writer.WriteLine("\t\t\tthis.barMainMenu = new DevExpress.XtraBars.Bar();");
            writer.WriteLine("\t\t\tthis.menuFile = new DevExpress.XtraBars.BarSubItem();");
            writer.WriteLine("\t\t\tthis.menuFileCloseAll = new DevExpress.XtraBars.BarButtonItem();");
            writer.WriteLine("\t\t\tthis.menuFileClose = new DevExpress.XtraBars.BarButtonItem();");
            writer.WriteLine("\t\t\tthis.menuExit = new DevExpress.XtraBars.BarButtonItem();");
            writer.WriteLine("\t\t\tthis.menuDictionary = new DevExpress.XtraBars.BarSubItem();");
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                if (!item.Alias.StartsWith("Util"))
                    continue;

                writer.WriteLine("\t\t\tthis.menu{0} = new DevExpress.XtraBars.BarButtonItem();", item.Alias);
            }
            writer.WriteLine("\t\t\tthis.menuTools = new DevExpress.XtraBars.BarSubItem();");
            writer.WriteLine("\t\t\tthis.menuChangePassword = new DevExpress.XtraBars.BarButtonItem();");
            writer.WriteLine("\t\t\tthis.menuWhoAmI = new DevExpress.XtraBars.BarButtonItem();");
            writer.WriteLine("\t\t\tthis.menuUserAccount = new DevExpress.XtraBars.BarButtonItem();");
            writer.WriteLine("\t\t\tthis.menuWindow = new DevExpress.XtraBars.BarSubItem();");
            writer.WriteLine("\t\t\tthis.menuWindowsList = new DevExpress.XtraBars.BarListItem();");
            writer.WriteLine("\t\t\tthis.menuHelp = new DevExpress.XtraBars.BarSubItem();");
            writer.WriteLine("\t\t\tthis.menuAbout = new DevExpress.XtraBars.BarButtonItem();");
            writer.WriteLine("\t\t\tthis.barStatus = new DevExpress.XtraBars.Bar();");
            writer.WriteLine("\t\t\tthis.splitContainerMain = new DevExpress.XtraEditors.SplitContainerControl();");
            writer.WriteLine("\t\t\tthis.navBarLeft = new DevExpress.XtraNavBar.NavBarControl();");
            writer.WriteLine("\t\t\tthis.imgLarge = new System.Windows.Forms.ImageList(this.components);");
            writer.WriteLine("\t\t\tthis.imgSmall = new System.Windows.Forms.ImageList(this.components);");
            writer.WriteLine("\t\t\tthis.nbgGeneral = new DevExpress.XtraNavBar.NavBarGroup();");
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                if (item.GetPKList().Count == 2)
                    continue;

                writer.WriteLine("\t\t\tthis.nbi{0} = new DevExpress.XtraNavBar.NavBarItem();", item.Alias);
            }
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();");
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();");
            writer.WriteLine("\t\t\tthis.splitContainerMain.SuspendLayout();");
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.navBarLeft)).BeginInit();");
            writer.WriteLine("\t\t\tthis.SuspendLayout();");
        }

      
        private void WriteDetail(StringWriter writer)
        {
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\t// barManager1");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\tthis.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {");
            writer.WriteLine("\t\t\tthis.barMainMenu,");
            writer.WriteLine("\t\t\tthis.barStatus});");
            writer.WriteLine("\t\t\tthis.barManager1.DockControls.Add(this.barDockControlTop);");
            writer.WriteLine("\t\t\tthis.barManager1.DockControls.Add(this.barDockControlBottom);");
            writer.WriteLine("\t\t\tthis.barManager1.DockControls.Add(this.barDockControlLeft);");
            writer.WriteLine("\t\t\tthis.barManager1.DockControls.Add(this.barDockControlRight);");
            writer.WriteLine("\t\t\tthis.barManager1.Form = this;");
            writer.WriteLine("\t\t\tthis.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {");
            writer.WriteLine("\t\t\tthis.menuFile,");
            writer.WriteLine("\t\t\tthis.menuFileClose,");
            writer.WriteLine("\t\t\tthis.menuFileCloseAll,");
            writer.WriteLine("\t\t\tthis.menuExit,");
            writer.WriteLine("\t\t\tthis.menuDictionary,");
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                if (!item.Alias.StartsWith("Util"))
                    continue;

                writer.WriteLine("\t\t\tthis.menu{0},", item.Alias);
            }
            writer.WriteLine("\t\t\tthis.menuTools,");
            writer.WriteLine("\t\t\tthis.menuChangePassword,");
            writer.WriteLine("\t\t\tthis.menuWhoAmI,");
            writer.WriteLine("\t\t\tthis.menuUserAccount,");
            writer.WriteLine("\t\t\tthis.menuWindow,");
            writer.WriteLine("\t\t\tthis.menuWindowsList,");
            writer.WriteLine("\t\t\tthis.menuHelp,");
            writer.WriteLine("\t\t\tthis.menuAbout});");
            writer.WriteLine("\t\t\tthis.barManager1.MainMenu = this.barMainMenu;");
            writer.WriteLine("\t\t\tthis.barManager1.MaxItemId = 45;");
            writer.WriteLine("\t\t\tthis.barManager1.StatusBar = this.barStatus;");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\t// barMainMenu");
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\tthis.barMainMenu.BarName = \"MainBar\";");
            writer.WriteLine("\t\t\tthis.barMainMenu.DockCol = 0;");
            writer.WriteLine("\t\t\tthis.barMainMenu.DockRow = 0;");
            writer.WriteLine("\t\t\tthis.barMainMenu.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;");
            writer.WriteLine("\t\t\tthis.barMainMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {");
            writer.WriteLine("\t\t\tnew DevExpress.XtraBars.LinkPersistInfo(this.menuFile),");
            writer.WriteLine("\t\t\tnew DevExpress.XtraBars.LinkPersistInfo(this.menuDictionary),");
            writer.WriteLine("\t\t\tnew DevExpress.XtraBars.LinkPersistInfo(this.menuTools),");
            writer.WriteLine("\t\t\tnew DevExpress.XtraBars.LinkPersistInfo(this.menuWindow),");
            writer.WriteLine("\t\t\tnew DevExpress.XtraBars.LinkPersistInfo(this.menuHelp)});");
            writer.WriteLine("\t\t\tthis.barMainMenu.OptionsBar.MultiLine = true;");
            writer.WriteLine("\t\t\tthis.barMainMenu.OptionsBar.UseWholeRow = true;");
            writer.WriteLine("\t\t\tthis.barMainMenu.Text = \"MainBar\";");

            //Menu File
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\t// menuFile");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\tthis.menuFile.Caption = \"&File\";");
            writer.WriteLine("\t\t\tthis.menuFile.Id = 0;");
            writer.WriteLine("\t\t\tthis.menuFile.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {");
            writer.WriteLine("\t\t\tnew DevExpress.XtraBars.LinkPersistInfo(this.menuFileCloseAll),");
            writer.WriteLine("\t\t\tnew DevExpress.XtraBars.LinkPersistInfo(this.menuFileClose),");
            writer.WriteLine("\t\t\tnew DevExpress.XtraBars.LinkPersistInfo(this.menuExit, true)});");
            writer.WriteLine("\t\t\tthis.menuFile.Name = \"menuFile\";");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\t// menuFileCloseAll");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\tthis.menuFileCloseAll.Caption = \"Close &All\";");
            writer.WriteLine("\t\t\tthis.menuFileCloseAll.Id = 1;");
            writer.WriteLine("\t\t\tthis.menuFileCloseAll.Name = \"menuFileCloseAll\";");
            writer.WriteLine("\t\t\tthis.menuFileCloseAll.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.menuFileCloseAll_ItemClick);");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\t// menuFileClose");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\tthis.menuFileClose.Caption = \"&Close\";");
            writer.WriteLine("\t\t\tthis.menuFileClose.Id = 2;");
            writer.WriteLine("\t\t\tthis.menuFileClose.Name = \"menuFileClose\";");
            writer.WriteLine("\t\t\tthis.menuFileClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.menuFileClose_ItemClick);");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\t// menuExit");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\tthis.menuExit.Caption = \"&Exit\";");
            writer.WriteLine("\t\t\tthis.menuExit.Id = 3;");
            writer.WriteLine("\t\t\tthis.menuExit.Name = \"menuExit\";");
            writer.WriteLine("\t\t\tthis.menuExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.menuExit_ItemClick);");
            
            //UtilMenu
            int menuId = 4;
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\t// menuDictionary");
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\tthis.menuDictionary.Caption = \"&Dictionary\";");
            writer.WriteLine("\t\t\tthis.menuDictionary.Id = 4;");
            writer.WriteLine("\t\t\tthis.menuDictionary.Name = \"menuDictionary\";");
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                if (!item.Alias.StartsWith("Util"))
                    continue;

                menuId++;
                writer.WriteLine("\t\t\t//");
                writer.WriteLine("\t\t\t// menu{0}", item.Alias);
                writer.WriteLine("\t\t\t//");
                writer.WriteLine("\t\t\tthis.menu{0}.Caption = \"{0}\";", item.Alias);
                writer.WriteLine("\t\t\tthis.menu{0}.Id = {1};", item.Alias, menuId);
                writer.WriteLine("\t\t\tthis.menu{0}.Name = \"menu{0}\";", item.Alias);
                writer.WriteLine("\t\t\tthis.menu{0}.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.menu{0}_ItemClick);", item.Alias);
                
                writer.WriteLine("\t\t\tthis.menuDictionary.LinksPersistInfo.Add(new DevExpress.XtraBars.LinkPersistInfo(this.menu{0}));", item.Alias);
            }

            //MenuTools
            menuId++;
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\t// menuTools");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\tthis.menuTools.Caption = \"&Tools\";");
            writer.WriteLine("\t\t\tthis.menuTools.Id = {0};", menuId);
            writer.WriteLine("\t\t\tthis.menuTools.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {");
            writer.WriteLine("\t\t\tnew DevExpress.XtraBars.LinkPersistInfo(this.menuChangePassword),");
            writer.WriteLine("\t\t\tnew DevExpress.XtraBars.LinkPersistInfo(this.menuWhoAmI),");
            writer.WriteLine("\t\t\tnew DevExpress.XtraBars.LinkPersistInfo(this.menuUserAccount, true)});");
            writer.WriteLine("\t\t\tthis.menuTools.Name = \"menuTools\";");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\t// menuChangePassword");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\tthis.menuChangePassword.Caption = \"&Change Password...\";");
            menuId++;
            writer.WriteLine("\t\t\tthis.menuChangePassword.Id = {0};", menuId);
            writer.WriteLine("\t\t\tthis.menuChangePassword.Name = \"menuChangePassword\";");
            writer.WriteLine("\t\t\tthis.menuChangePassword.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.menuChangePassword_ItemClick);");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\t// menuWhoAmI");
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\tthis.menuWhoAmI.Caption = \"&Who Am I...\";");
            menuId++;
            writer.WriteLine("\t\t\tthis.menuWhoAmI.Id = {0};", menuId);
            writer.WriteLine("\t\t\tthis.menuWhoAmI.Name = \"menuWhoAmI\";");
            writer.WriteLine("\t\t\tthis.menuWhoAmI.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.menuWhoAmI_ItemClick);");
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\t// menuUserAccount");
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\tthis.menuUserAccount.Caption = \"&User Account\";");
            menuId++;
            writer.WriteLine("\t\t\tthis.menuUserAccount.Id = {0};", menuId);
            writer.WriteLine("\t\t\tthis.menuUserAccount.Name = \"menuUserAccount\";");
            writer.WriteLine("\t\t\tthis.menuUserAccount.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.menuUserAccount_ItemClick);");
           
            //MenuWindows
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\t// menuWindow");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\tthis.menuWindow.Caption = \"&Window\";");
            menuId++;
            writer.WriteLine("\t\t\tthis.menuWindow.Id = {0};", menuId);
            writer.WriteLine("\t\t\tthis.menuWindow.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {");
            writer.WriteLine("\t\t\tnew DevExpress.XtraBars.LinkPersistInfo(this.menuWindowsList)});");
            writer.WriteLine("\t\t\tthis.menuWindow.Name = \"menuWindow\";");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\t// menuWindowsList");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\tthis.menuWindowsList.Caption = \"&WindowsList\";");
            menuId++;
            writer.WriteLine("\t\t\tthis.menuWindowsList.Id = {0};", menuId);
            writer.WriteLine("\t\t\tthis.menuWindowsList.Name = \"menuWindowsList\";");
            writer.WriteLine("\t\t\tthis.menuWindowsList.ShowChecks = true;");
            writer.WriteLine("\t\t\tthis.menuWindowsList.ShowNumbers = true;");
            writer.WriteLine("\t\t\tthis.menuWindowsList.ListItemClick += new DevExpress.XtraBars.ListItemClickEventHandler(this.menuWindowsList_ListItemClick);");
            writer.WriteLine("\t\t\tthis.menuWindowsList.GetItemData += new System.EventHandler(this.menuWindowsList_GetItemData);");
            
            //MenuHelp
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\t// menuHelp");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\tthis.menuHelp.Caption = \"&Help\";");
            menuId++;
            writer.WriteLine("\t\t\tthis.menuHelp.Id = {0};", menuId);
            writer.WriteLine("\t\t\tthis.menuHelp.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {");
            writer.WriteLine("\t\t\tnew DevExpress.XtraBars.LinkPersistInfo(this.menuAbout)});");
            writer.WriteLine("\t\t\tthis.menuHelp.Name = \"menuHelp\";");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\t// menuAbout");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\tthis.menuAbout.Caption = \"&About\";");
            menuId++;
            writer.WriteLine("\t\t\tthis.menuAbout.Id = {0};", menuId);
            writer.WriteLine("\t\t\tthis.menuAbout.Name = \"menuAbout\";");
            writer.WriteLine("\t\t\tthis.menuAbout.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.menuAbout_ItemClick);");
            
            //Status
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\t// barStatus");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\tthis.barStatus.BarName = \"Status\";");
            writer.WriteLine("\t\t\tthis.barStatus.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;");
            writer.WriteLine("\t\t\tthis.barStatus.DockCol = 0;");
            writer.WriteLine("\t\t\tthis.barStatus.DockRow = 0;");
            writer.WriteLine("\t\t\tthis.barStatus.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;");
            writer.WriteLine("\t\t\tthis.barStatus.OptionsBar.AllowQuickCustomization = false;");
            writer.WriteLine("\t\t\tthis.barStatus.OptionsBar.DrawDragBorder = false;");
            writer.WriteLine("\t\t\tthis.barStatus.OptionsBar.UseWholeRow = true;");
            writer.WriteLine("\t\t\tthis.barStatus.Text = \"Status\";");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\t// splitContainerMain");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\tthis.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;");
            writer.WriteLine("\t\t\tthis.splitContainerMain.Location = new System.Drawing.Point(0, 24);");
            writer.WriteLine("\t\t\tthis.splitContainerMain.Name = \"splitContainerMain\";");
            writer.WriteLine("\t\t\tthis.splitContainerMain.Panel1.Controls.Add(this.navBarLeft);");
            writer.WriteLine("\t\t\tthis.splitContainerMain.Panel1.Text = \"Panel1\";");
            writer.WriteLine("\t\t\tthis.splitContainerMain.Panel2.Text = \"Panel2\";");
            writer.WriteLine("\t\t\tthis.splitContainerMain.Size = new System.Drawing.Size(741, 501);");
            writer.WriteLine("\t\t\tthis.splitContainerMain.SplitterPosition = 160;");
            writer.WriteLine("\t\t\tthis.splitContainerMain.TabIndex = 4;");
            writer.WriteLine("\t\t\tthis.splitContainerMain.Text = \"splitContainerControl1\";");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\t// navBarLeft");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\tthis.navBarLeft.ActiveGroup = this.nbgGeneral;");
            writer.WriteLine("\t\t\tthis.navBarLeft.Dock = System.Windows.Forms.DockStyle.Fill;");
            writer.WriteLine("\t\t\tthis.navBarLeft.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {");
            writer.WriteLine("\t\t\tthis.nbgGeneral});");

            writer.WriteLine("\t\t\tthis.navBarLeft.Items.AddRange(new DevExpress.XtraNavBar.NavBarItem[] {");
            int index = -1;
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                if (item.GetPKList().Count == 2)
                    continue;

                if(index == -1)
                {
                    writer.WriteLine("\t\t\tthis.nbi{0}", item.Alias);
                }
                else
                {
                    writer.WriteLine("\t\t\t,this.nbi{0}", item.Alias);
                }

                index++;
            }
            writer.WriteLine("\t\t\t});");
            //writer.WriteLine();

            writer.WriteLine("\t\t\tthis.navBarLeft.LargeImages = this.imgLarge;");
            writer.WriteLine("\t\t\tthis.navBarLeft.Location = new System.Drawing.Point(0, 0);");
            writer.WriteLine("\t\t\tthis.navBarLeft.Name = \"navBarLeft\";");
            writer.WriteLine("\t\t\tthis.navBarLeft.OptionsNavPane.ExpandedWidth = 156;");
            writer.WriteLine("\t\t\tthis.navBarLeft.OptionsNavPane.ShowExpandButton = false;");
            writer.WriteLine("\t\t\tthis.navBarLeft.PaintStyleKind = DevExpress.XtraNavBar.NavBarViewKind.NavigationPane;");
            writer.WriteLine("\t\t\tthis.navBarLeft.Size = new System.Drawing.Size(156, 497);");
            writer.WriteLine("\t\t\tthis.navBarLeft.SmallImages = this.imgSmall;");
            writer.WriteLine("\t\t\tthis.navBarLeft.TabIndex = 0;");
            writer.WriteLine("\t\t\tthis.navBarLeft.Text = \"navBarControl1\";");
            writer.WriteLine("\t\t\tthis.navBarLeft.View = new DevExpress.XtraNavBar.ViewInfo.SkinNavigationPaneViewInfoRegistrator();");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\t// imgLarge");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\tthis.imgLarge.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;");
            writer.WriteLine("\t\t\tthis.imgLarge.ImageSize = new System.Drawing.Size(16, 16);");
            writer.WriteLine("\t\t\tthis.imgLarge.TransparentColor = System.Drawing.Color.Transparent;");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\t// imgSmall");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\tthis.imgSmall.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;");
            writer.WriteLine("\t\t\tthis.imgSmall.ImageSize = new System.Drawing.Size(16, 16);");
            writer.WriteLine("\t\t\tthis.imgSmall.TransparentColor = System.Drawing.Color.Transparent;");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\t// nbgGeneral");
            writer.WriteLine("\t\t\t//"); 
            writer.WriteLine("\t\t\tthis.nbgGeneral.Caption = \"General\";");
            writer.WriteLine("\t\t\tthis.nbgGeneral.Expanded = true;");
            writer.WriteLine("\t\t\tthis.nbgGeneral.Name = \"nbgGeneral\";");
            writer.WriteLine("\t\t\tthis.nbgGeneral.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {");
            index = -1;
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                if (item.GetPKList().Count == 2)
                    continue;

                if (index == -1)
                {
                    writer.WriteLine("\t\t\tnew DevExpress.XtraNavBar.NavBarItemLink(this.nbi{0})", item.Alias);
                }
                else
                {
                    writer.WriteLine("\t\t\t,new DevExpress.XtraNavBar.NavBarItemLink(this.nbi{0})", item.Alias);
                }

                index++;
            }
            writer.WriteLine("\t\t\t});");
            //writer.WriteLine();

            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                if (item.GetPKList().Count == 2)
                    continue;

                writer.WriteLine("\t\t\t//");
                writer.WriteLine("\t\t\t// nbi{0}", item.Alias);
                writer.WriteLine("\t\t\t//");
                writer.WriteLine("\t\t\tthis.nbi{0}.Caption = \"{0}\";", item.Alias);
                writer.WriteLine("\t\t\tthis.nbi{0}.Name = \"nbi{0}\";", item.Alias);
                writer.WriteLine("\t\t\tthis.nbi{0}.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.nbi{0}_LinkClicked);", item.Alias);
            }

        }

        private void WriteEndInit(StringWriter writer)
            {
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\t// FormMain");
            writer.WriteLine("\t\t\t//");                                                                             
            writer.WriteLine("\t\t\tthis.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);");                   
            writer.WriteLine("\t\t\tthis.ClientSize = new System.Drawing.Size(741, 547);");                            
            writer.WriteLine("\t\t\tthis.Controls.Add(this.splitContainerMain);");                                     
            writer.WriteLine("\t\t\tthis.Controls.Add(this.barDockControlLeft);");                                     
            writer.WriteLine("\t\t\tthis.Controls.Add(this.barDockControlRight);");                                    
            writer.WriteLine("\t\t\tthis.Controls.Add(this.barDockControlBottom);");                                   
            writer.WriteLine("\t\t\tthis.Controls.Add(this.barDockControlTop);");                                      
            writer.WriteLine("\t\t\tthis.Name = \"FormMain\";");                                                         
            writer.WriteLine("\t\t\tthis.Text = \"Manager\";");                                                          
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();");       
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();");
            writer.WriteLine("\t\t\tthis.splitContainerMain.ResumeLayout(false);");                                    
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.navBarLeft)).EndInit();");        
            writer.WriteLine("\t\t\tthis.ResumeLayout(false);");                                                       
            }

        private void WriteFields(StringWriter writer)
        {
            writer.WriteLine("\t\tprivate DevExpress.XtraBars.BarManager barManager1;");
            writer.WriteLine("\t\tprivate DevExpress.XtraBars.BarDockControl barDockControlTop;");
            writer.WriteLine("\t\tprivate DevExpress.XtraBars.BarDockControl barDockControlBottom;");
            writer.WriteLine("\t\tprivate DevExpress.XtraBars.BarDockControl barDockControlLeft;");
            writer.WriteLine("\t\tprivate DevExpress.XtraBars.BarDockControl barDockControlRight;");
            writer.WriteLine("\t\tprivate DevExpress.XtraBars.Bar barMainMenu;");
            writer.WriteLine("\t\tprivate DevExpress.XtraBars.Bar barStatus;");
            writer.WriteLine();
            writer.WriteLine("\t\tprivate DevExpress.XtraEditors.SplitContainerControl splitContainerMain;");
            writer.WriteLine("\t\tprivate DevExpress.XtraNavBar.NavBarControl navBarLeft;");
            writer.WriteLine("\t\tprivate DevExpress.XtraBars.BarSubItem menuFile;");
            writer.WriteLine("\t\tprivate DevExpress.XtraBars.BarButtonItem menuFileClose;");
            writer.WriteLine("\t\tprivate DevExpress.XtraBars.BarButtonItem menuFileCloseAll;");
            writer.WriteLine("\t\tprivate DevExpress.XtraBars.BarButtonItem menuExit;");
            writer.WriteLine("\t\tprivate DevExpress.XtraBars.BarSubItem menuDictionary;");
            //UtilMenu
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                if (!item.Alias.StartsWith("Util"))
                    continue;

                writer.WriteLine("\t\tprivate DevExpress.XtraBars.BarButtonItem menu{0};", item.Alias);
            }
            writer.WriteLine("\t\tprivate DevExpress.XtraBars.BarSubItem menuTools;");
            writer.WriteLine("\t\tprivate DevExpress.XtraBars.BarButtonItem menuChangePassword;");
            writer.WriteLine("\t\tprivate DevExpress.XtraBars.BarButtonItem menuWhoAmI;");
            writer.WriteLine("\t\tprivate DevExpress.XtraBars.BarButtonItem menuUserAccount;");
            writer.WriteLine("\t\tprivate DevExpress.XtraBars.BarSubItem menuWindow;");
            writer.WriteLine("\t\tprivate DevExpress.XtraBars.BarListItem menuWindowsList;");
            writer.WriteLine("\t\tprivate DevExpress.XtraBars.BarSubItem menuHelp;");
            writer.WriteLine("\t\tprivate DevExpress.XtraBars.BarButtonItem menuAbout;");

            writer.WriteLine("\t\tprivate System.Windows.Forms.ImageList imgSmall;");
            writer.WriteLine("\t\tprivate System.Windows.Forms.ImageList imgLarge;");
            writer.WriteLine("\t\tprivate DevExpress.XtraNavBar.NavBarGroup nbgGeneral;");

            writer.WriteLine();
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                if (item.GetPKList().Count == 2)
                    continue;

                writer.WriteLine("\t\tprivate DevExpress.XtraNavBar.NavBarItem nbi{0};", item.Alias);
            }
        }
    }
}
