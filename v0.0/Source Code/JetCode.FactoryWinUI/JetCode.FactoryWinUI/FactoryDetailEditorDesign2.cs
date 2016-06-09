using System;
using System.IO;
using JetCode.Factory;
using JetCode.BizSchema;

namespace JetCode.FactoryWinUI
{
    public class FactoryDetailEditorDesign2 : FactoryBase
    {
        public FactoryDetailEditorDesign2(MappingSchema mappingSchema, ObjectSchema objectSchema)
            : base(mappingSchema, objectSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Manager.FormDetailEditor", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic partial class FormDetail{0}", base.ObjectSchema.Alias);
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
            FieldSchemaCollection list = base.GetBindableFields();

            this.WriteInitializeComponent(writer, list);
            this.WriteFields(writer, list);
        }

        private void WriteInitializeComponent(StringWriter writer, FieldSchemaCollection list)
        {
            writer.WriteLine("\t\t#region Windows Form Designer generated code");
            writer.WriteLine();
            writer.WriteLine("\t\t/// <summary>");
            writer.WriteLine("\t\t/// Required method for Designer support - do not modify");
            writer.WriteLine("\t\t/// the contents of this method with the code editor.");
            writer.WriteLine("\t\t/// </summary>");
            writer.WriteLine("\t\tprivate void InitializeComponent()");
            writer.WriteLine("\t\t{");

            this.WriteFieldsInitialize(writer, list);
            this.WriteBeginInit(writer, list);
            this.WriteAddFields(writer, list);
            this.WriteFieldDetail(writer, list);
            this.WriteFormDetail(writer);
            this.WriteEndInit(writer, list);

            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();
        }

        private void WriteFieldsInitialize(StringWriter writer, FieldSchemaCollection list)
        {
            foreach (FieldSchema item in list)
            {
                Type dotnetType = base.Utilities.ToDotNetType(item.DataType);
                if (dotnetType == typeof(bool))
                {
                    writer.WriteLine("\t\t\tthis.chk{0} = new Cheke.WinCtrl.Common.CheckEditEx();", item.Alias);
                }
                else if (dotnetType == typeof(DateTime))
                {
                    writer.WriteLine("\t\t\tthis.date{0} = new Cheke.WinCtrl.Common.DateEditEx();", item.Alias);
                }
                else if (dotnetType == typeof(string))
                {
                    int size;
                    int.TryParse(item.Size, out size);
                    if(size >= 256)
                    {
                        writer.WriteLine("\t\t\tthis.txt{0} = new Cheke.WinCtrl.Common.MemoEditEx();", item.Alias);  
                    }
                    else
                    {
                        writer.WriteLine("\t\t\tthis.txt{0} = new Cheke.WinCtrl.Common.TextEditEx();", item.Alias);
                    }
                }
                else
                {
                    writer.WriteLine("\t\t\tthis.txt{0} = new Cheke.WinCtrl.Common.TextEditEx();", item.Alias);
                }
            }

            foreach (ParentSchema item in base.ObjectSchema.Parents)
            {
                writer.WriteLine("\t\t\tthis.txt{0} = new Cheke.WinCtrl.Common.ButtonEditEx();", item.Alias);
            }

            writer.WriteLine("\t\t\tthis.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();");
            writer.WriteLine("\t\t\tthis.tabDetail = new DevExpress.XtraTab.XtraTabPage();");
            foreach (ChildSchema item in this.ObjectSchema.Children)
            {
                writer.WriteLine("\t\t\tthis.tab{0} = new DevExpress.XtraTab.XtraTabPage();", item.Alias);
                writer.WriteLine("\t\t\tthis.grd{0} = new DevExpress.XtraGrid.GridControl();", item.Alias);
                writer.WriteLine("\t\t\tthis.grd{0}View = new DevExpress.XtraGrid.Views.Grid.GridView();", item.Alias);
            }
        }

        private void WriteBeginInit(StringWriter writer, FieldSchemaCollection list)
        {
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.pnlButtons)).BeginInit();");
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.pnlContent)).BeginInit();");
            writer.WriteLine("\t\t\tthis.pnlContent.SuspendLayout();");
            foreach (FieldSchema item in list)
            {
                Type dotnetType = base.Utilities.ToDotNetType(item.DataType);
                if (dotnetType == typeof(bool))
                {
                    writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.chk{0}.Properties)).BeginInit();", item.Alias);
                }
                else if (dotnetType == typeof(DateTime))
                {
                    writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.date{0}.Properties.VistaTimeProperties)).BeginInit();", item.Alias);
                    writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.date{0}.Properties)).BeginInit();", item.Alias);
                }
                else
                {
                    writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.txt{0}.Properties)).BeginInit();", item.Alias);
                }
            }

            foreach (ParentSchema item in base.ObjectSchema.Parents)
            {
                writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.txt{0}.Properties)).BeginInit();", item.Alias);
            }

            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();");
            foreach (ChildSchema item in base.ObjectSchema.Children)
            {
                writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.grd{0})).BeginInit();", item.Alias);
                writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.grd{0}View)).BeginInit();", item.Alias);
            }
            writer.WriteLine("\t\t\tthis.SuspendLayout();");
        }

        private void WriteAddFields(StringWriter writer, FieldSchemaCollection list)
        {
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\t// pnlContent");
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\tthis.pnlContent.Controls.Add(this.xtraTabControl1);");
            writer.WriteLine("\t\t\t//");

            writer.WriteLine("\t\t\t// xtraTabControl1");
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\tthis.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;");
            writer.WriteLine("\t\t\tthis.xtraTabControl1.Name = \"xtraTabControl1\";");
            writer.WriteLine("\t\t\tthis.xtraTabControl1.SelectedTabPage = this.tabDetail;");
            writer.WriteLine("\t\t\tthis.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {");
            writer.Write("\t\t\tthis.tabDetail");
            foreach (ChildSchema item in this.ObjectSchema.Children)
            {
                writer.Write(",this.tab{0}", item.Alias);
            }
            writer.Write("});");
            writer.WriteLine();
            writer.WriteLine("\t\t\tthis.xtraTabControl1.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraTabControl1_SelectedPageChanged);");
            
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\t// tabDetail");
            writer.WriteLine("\t\t\t//");
            foreach (FieldSchema item in list)
            {
                Type dotnetType = base.Utilities.ToDotNetType(item.DataType);
                if (dotnetType == typeof(bool))
                {
                    writer.WriteLine("\t\t\tthis.tabDetail.Controls.Add(this.chk{0});", item.Alias);
                }
                else if (dotnetType == typeof(DateTime))
                {
                    writer.WriteLine("\t\t\tthis.tabDetail.Controls.Add(this.date{0});", item.Alias);
                }
                else
                {
                    writer.WriteLine("\t\t\tthis.tabDetail.Controls.Add(this.txt{0});", item.Alias);
                }
            }
            foreach (ParentSchema item in base.ObjectSchema.Parents)
            {
                writer.WriteLine("\t\t\tthis.tabDetail.Controls.Add(this.txt{0});", item.Alias);
            }
            writer.WriteLine("\t\t\tthis.tabDetail.Name = \"tabDetail\";");
            writer.WriteLine("\t\t\tthis.tabDetail.Text = \"Detail\";");

            foreach (ChildSchema item in this.ObjectSchema.Children)
            {
                writer.WriteLine("\t\t\t//");
                writer.WriteLine("\t\t\t// tab{0}", item.Alias);
                writer.WriteLine("\t\t\t//");
                writer.WriteLine("\t\t\tthis.tab{0}.Controls.Add(this.grd{0});", item.Alias);
                writer.WriteLine("\t\t\tthis.tab{0}.Name = \"tab{0}\";", item.Alias);
                writer.WriteLine("\t\t\tthis.tab{0}.Text = \"{0}\";", item.Alias);
                writer.WriteLine("\t\t\tthis.tab{0}.Enter += new System.EventHandler(this.tab{0}_Enter);", item.Alias);
            }

            foreach (ChildSchema item in this.ObjectSchema.Children)
            {
                writer.WriteLine("\t\t\t//");
                writer.WriteLine("\t\t\t// grd{0}", item.Alias);
                writer.WriteLine("\t\t\t//");
                writer.WriteLine("\t\t\tthis.grd{0}.Dock = System.Windows.Forms.DockStyle.Fill;", item.Alias);
                writer.WriteLine("\t\t\tthis.grd{0}.MainView = this.grd{0}View;", item.Alias);
                writer.WriteLine("\t\t\tthis.grd{0}.Name = \"grd{0}\";", item.Alias);
                writer.WriteLine("\t\t\tthis.grd{0}.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {{this.grd{0}View}});", item.Alias);

                writer.WriteLine("\t\t\t//");
                writer.WriteLine("\t\t\t// grd{0}View", item.Alias);
                writer.WriteLine("\t\t\t//");
                writer.WriteLine("\t\t\tthis.grd{0}View.GridControl = this.grd{0};", item.Alias);
                writer.WriteLine("\t\t\tthis.grd{0}View.Name = \"grd{0}View\";", item.Alias);
            }
        }

        private void WriteFormDetail(StringWriter writer)
        {
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\t// FormDetail{0}", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\tthis.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);");
            writer.WriteLine("\t\t\tthis.Name = \"FormDetail{0}\";", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tthis.Text = \"{0} Detail\";", base.ObjectSchema.Alias);
        }

        private void WriteFieldDetail(StringWriter writer, FieldSchemaCollection list)
        {
            int tabIndex = -1;
            int xPos = 5;
            int yPos = -40;
            foreach (FieldSchema item in list)
            {
                tabIndex++;
                yPos += 40;
                if (yPos >= 300)
                {
                    yPos = 0;
                    xPos += 155;
                }

                Type dotnetType = base.Utilities.ToDotNetType(item.DataType);
                if (dotnetType == typeof(bool))
                {
                    writer.WriteLine("\t\t\t//");
                    writer.WriteLine("\t\t\t// chk{0}", item.Alias);
                    writer.WriteLine("\t\t\t//");
                    writer.WriteLine("\t\t\tthis.chk{0}.Location = new System.Drawing.Point({1}, {2});", item.Alias, xPos, yPos + 16);
                    writer.WriteLine("\t\t\tthis.chk{0}.Name = \"chk{0}\";", item.Alias);
                    writer.WriteLine("\t\t\tthis.chk{0}.TabIndex = {1};", item.Alias, tabIndex);
                    writer.WriteLine("\t\t\tthis.chk{0}.Properties.Caption = \"{0}\";", item.Alias);
                }
                else if (dotnetType == typeof(DateTime))
                {
                    writer.WriteLine("\t\t\t//");
                    writer.WriteLine("\t\t\t// date{0}", item.Alias);
                    writer.WriteLine("\t\t\t//");
                    writer.WriteLine("\t\t\tthis.date{0}.EditValue =  new System.DateTime({1}, 1, 1, 0, 0, 0, 0);", item.Alias, DateTime.Now.Year);
                    writer.WriteLine("\t\t\tthis.date{0}.Location = new System.Drawing.Point({1}, {2});", item.Alias, xPos, yPos);
                    writer.WriteLine("\t\t\tthis.date{0}.Name = \"date{0}\";", item.Alias);

                    writer.WriteLine("\t\t\tthis.date{0}.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[]", item.Alias);
                    writer.WriteLine("\t\t\t\t{");
                    writer.WriteLine("\t\t\t\t\tnew DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)");
                    writer.WriteLine("\t\t\t\t});");

                    writer.WriteLine("\t\t\tthis.date{0}.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[]", item.Alias);
                    writer.WriteLine("\t\t\t\t{");
                    writer.WriteLine("\t\t\t\t\tnew DevExpress.XtraEditors.Controls.EditorButton()");
                    writer.WriteLine("\t\t\t\t});");

                    writer.WriteLine("\t\t\tthis.date{0}.TabIndex = {1};", item.Alias, tabIndex);
                    writer.WriteLine("\t\t\tthis.date{0}.Title = \"{0}\";", item.Alias);
                }
                else if (dotnetType == typeof(string))
                {
                    int size;
                    int.TryParse(item.Size, out size);

                    writer.WriteLine("\t\t\t//");
                    writer.WriteLine("\t\t\t// txt{0}", item.Alias);
                    writer.WriteLine("\t\t\t//");
                    writer.WriteLine("\t\t\tthis.txt{0}.EditValue = \"\";", item.Alias);
                    if (size > 128)
                    {
                        writer.WriteLine("\t\t\tthis.txt{0}.Location = new System.Drawing.Point({1}, {2});", item.Alias, xPos, yPos);
                        yPos += 120;
                    }
                    else
                    {
                        writer.WriteLine("\t\t\tthis.txt{0}.Location = new System.Drawing.Point({1}, {2});", item.Alias, xPos, yPos);
                    }
                    writer.WriteLine("\t\t\tthis.txt{0}.Name = \"txt{0}\";", item.Alias);
                    writer.WriteLine("\t\t\tthis.txt{0}.TabIndex = {1};", item.Alias, tabIndex);
                    writer.WriteLine("\t\t\tthis.txt{0}.Title = \"{0}\";", item.Alias);

                    //if (item.DataType.ToLower() != "text" && item.DataType.ToLower() != "ntext")
                    //{
                    //    writer.WriteLine("\t\t\tthis.txt{0}.Properties.MaxLength = {1};", item.Alias, item.Size);
                    //}
                }
                else
                {
                    writer.WriteLine("\t\t\t//");
                    writer.WriteLine("\t\t\t// txt{0}", item.Alias);
                    writer.WriteLine("\t\t\t//");
                    writer.WriteLine("\t\t\tthis.txt{0}.EditValue = \"\";", item.Alias);
                    writer.WriteLine("\t\t\tthis.txt{0}.Location = new System.Drawing.Point({1}, {2});", item.Alias, xPos, yPos);
                    writer.WriteLine("\t\t\tthis.txt{0}.Name = \"txt{0}\";", item.Alias);
                    writer.WriteLine("\t\t\tthis.txt{0}.TabIndex = {1};", item.Alias, tabIndex);
                    switch (item.DataType.ToLower())
                    {
                        case "tinyint":
                        case "smallint":
                        case "int": 
                            writer.WriteLine("\t\t\tthis.txt{0}.Title = \"{0}\";", item.Alias);
                            writer.WriteLine("\t\t\tthis.txt{0}.Properties.Mask.EditMask = \"n0\";", item.Alias);
                            writer.WriteLine("\t\t\tthis.txt{0}.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;", item.Alias);
                            writer.WriteLine("\t\t\tthis.txt{0}.Properties.Mask.UseMaskAsDisplayFormat = true;", item.Alias);
                            break;
                        case "smallmoney":
                        case "money":
                            writer.WriteLine("\t\t\tthis.txt{0}.Title = \"{0}($)\";", item.Alias);
                            writer.WriteLine("\t\t\tthis.txt{0}.Properties.Mask.EditMask = \"c2\";", item.Alias);
                            writer.WriteLine("\t\t\tthis.txt{0}.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;", item.Alias);
                            writer.WriteLine("\t\t\tthis.txt{0}.Properties.Mask.UseMaskAsDisplayFormat = true;", item.Alias);
                            break;
                        case "decimal":
                            //if (item.Alias.ToLower().EndsWith("rate"))
                            //{
                            //    writer.WriteLine("\t\t\tthis.txt{0}.Title = \"{0}(%)\";", item.Alias);
                            //    writer.WriteLine("\t\t\tthis.txt{0}.Properties.Mask.EditMask = \"p2\";", item.Alias);
                            //}
                            //else
                            {
                                
                                string[] splits = item.Size.Split(',');
                                if(splits.Length == 2 && splits[1].Length > 1 && char.IsNumber(splits[1][0]))
                                {
                                    int scale = int.Parse(splits[1].Substring(0, 1));
                                    writer.WriteLine("\t\t\tthis.txt{0}.Title = \"{0}\";", item.Alias);
                                    writer.WriteLine("\t\t\tthis.txt{0}.Properties.Mask.EditMask = \"f{1}\";", item.Alias, scale);
                                }
                                else
                                {
                                    writer.WriteLine("\t\t\tthis.txt{0}.Title = \"{0}\";", item.Alias);
                                    writer.WriteLine("\t\t\tthis.txt{0}.Properties.Mask.EditMask = \"f2\";", item.Alias);
                                }
                            }
                            writer.WriteLine("\t\t\tthis.txt{0}.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;", item.Alias);
                            writer.WriteLine("\t\t\tthis.txt{0}.Properties.Mask.UseMaskAsDisplayFormat = true;", item.Alias);
                            break;
                        case "float":
                            writer.WriteLine("\t\t\tthis.txt{0}.Title = \"{0}\";", item.Alias);
                            writer.WriteLine("\t\t\tthis.txt{0}.Properties.Mask.EditMask = \"f2\";", item.Alias);
                            writer.WriteLine("\t\t\tthis.txt{0}.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;", item.Alias);
                            writer.WriteLine("\t\t\tthis.txt{0}.Properties.Mask.UseMaskAsDisplayFormat = true;", item.Alias);
                            break;  
                        default:
                            writer.WriteLine("\t\t\tthis.txt{0}.Title = \"{0}\";", item.Alias);
                            break;
                    }
                }
            }

            foreach (ParentSchema item in base.ObjectSchema.Parents)
            {
                tabIndex++;
                yPos += 40;
                if (yPos >= 300)
                {
                    yPos = 0;
                    xPos += 155;
                }

                writer.WriteLine("\t\t\t//");
                writer.WriteLine("\t\t\t// txt{0}", item.Alias);
                writer.WriteLine("\t\t\t//");
                writer.WriteLine("\t\t\tthis.txt{0}.EditValue = \"\";", item.Alias);
                writer.WriteLine("\t\t\tthis.txt{0}.Location = new System.Drawing.Point({1}, {2});", item.Alias, xPos, yPos);
                writer.WriteLine("\t\t\tthis.txt{0}.Name = \"txt{0}\";", item.Alias);

                writer.WriteLine("\t\t\tthis.txt{0}.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] ", item.Alias);
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tnew DevExpress.XtraEditors.Controls.EditorButton()");
                writer.WriteLine("\t\t\t\t});");

                writer.WriteLine("\t\t\tthis.txt{0}.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;", item.Alias);
                writer.WriteLine("\t\t\tthis.txt{0}.ReadOnly = false;", item.Alias);
                writer.WriteLine("\t\t\tthis.txt{0}.TabIndex = {1};", item.Alias, tabIndex);
                writer.WriteLine("\t\t\tthis.txt{0}.Title = \"{0}\";", item.Alias);
                writer.WriteLine("\t\t\tthis.txt{0}.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txt{0}_ButtonClick);", item.Alias);
            }
        }

        private void WriteEndInit(StringWriter writer, FieldSchemaCollection list)
        {
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.pnlButtons)).EndInit();");
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.pnlContent)).EndInit();");
            writer.WriteLine("\t\t\tthis.pnlContent.ResumeLayout(false);");
            foreach (FieldSchema item in list)
            {
                Type dotnetType = base.Utilities.ToDotNetType(item.DataType);
                if (dotnetType == typeof(bool))
                {
                    writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.chk{0}.Properties)).EndInit();", item.Alias);
                }
                else if (dotnetType == typeof(DateTime))
                {
                    writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.date{0}.Properties.VistaTimeProperties)).EndInit();", item.Alias);
                    writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.date{0}.Properties)).EndInit();", item.Alias);
                }
                else
                {
                    writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.txt{0}.Properties)).EndInit();", item.Alias);
                }
            }

            foreach (ParentSchema item in base.ObjectSchema.Parents)
            {
                writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.txt{0}.Properties)).EndInit();", item.Alias);
            }

            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();");
            foreach (ChildSchema item in base.ObjectSchema.Children)
            {
                writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.grd{0})).EndInit();", item.Alias);
                writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.grd{0}View)).EndInit();", item.Alias);
            }
            writer.WriteLine("\t\t\tthis.ResumeLayout(false);");
        }

        private void WriteFields(StringWriter writer, FieldSchemaCollection list)
        {
            foreach (FieldSchema item in list)
            {
                Type dotnetType = base.Utilities.ToDotNetType(item.DataType);
                if (dotnetType == typeof(bool))
                {
                    writer.WriteLine("\t\tprivate Cheke.WinCtrl.Common.CheckEditEx chk{0};", item.Alias);
                }
                else if (dotnetType == typeof(DateTime))
                {
                    writer.WriteLine("\t\tprivate Cheke.WinCtrl.Common.DateEditEx date{0};", item.Alias);
                }
                else if (dotnetType == typeof(string))
                {
                    int size;
                    int.TryParse(item.Size, out size);
                    if (size >= 256)
                    {
                        writer.WriteLine("\t\tprivate Cheke.WinCtrl.Common.MemoEditEx txt{0};", item.Alias);
                    }
                    else
                    {
                        writer.WriteLine("\t\tprivate Cheke.WinCtrl.Common.TextEditEx txt{0};", item.Alias);
                    }
                }
                else
                {
                    writer.WriteLine("\t\tprivate Cheke.WinCtrl.Common.TextEditEx txt{0};", item.Alias);
                }
            }

            foreach (ParentSchema item in base.ObjectSchema.Parents)
            {
                writer.WriteLine("\t\tprivate Cheke.WinCtrl.Common.ButtonEditEx txt{0};", item.Alias);
            }

            writer.WriteLine("\t\tprivate DevExpress.XtraTab.XtraTabControl xtraTabControl1;");
            writer.WriteLine("\t\tprivate DevExpress.XtraTab.XtraTabPage tabDetail;");
            foreach (ChildSchema item in this.ObjectSchema.Children)
            {
                writer.WriteLine("\t\tprivate DevExpress.XtraTab.XtraTabPage tab{0};", item.Alias);
                writer.WriteLine("\t\tprivate DevExpress.XtraGrid.GridControl grd{0};", item.Alias);
                writer.WriteLine("\t\tprivate DevExpress.XtraGrid.Views.Grid.GridView grd{0}View;", item.Alias);
            }
        }
    }
}
