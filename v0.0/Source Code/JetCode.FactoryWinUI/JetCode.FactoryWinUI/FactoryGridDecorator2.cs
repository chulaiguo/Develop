using System.IO;
using System.Reflection;
using JetCode.Factory;
using JetCode.BizSchema;
using System;
using System.Collections.Generic;

namespace JetCode.FactoryWinUI
{
    public class FactoryGridDecorator2 : FactoryBase
    {
        public FactoryGridDecorator2(MappingSchema mappingSchema, ObjectSchema objectSchema)
            : base(mappingSchema, objectSchema)
        {
        }


        protected  override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("using System.Windows.Forms;");
            writer.WriteLine("using DevExpress.Utils;");
            writer.WriteLine("using DevExpress.XtraEditors;");
            writer.WriteLine("using DevExpress.XtraGrid;");
            writer.WriteLine("using DevExpress.XtraGrid.Columns;");
            writer.WriteLine("using DevExpress.XtraGrid.Views.Grid;");
            writer.WriteLine("using Cheke.WinCtrl.Decoration;");
            writer.WriteLine("using {0}.ViewObj;", base.ProjectName);
            writer.WriteLine("using {0}.Schema;", base.ProjectName);
            writer.WriteLine("using {0}.Manager.FormSelect;", base.ProjectName);
            writer.WriteLine("using {0}.Manager.FormDetailMap;", base.ProjectName);
            writer.WriteLine("using {0}.Manager.FormDetailEditor;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Manager.GridDecorator", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic class Grid{0}Decorator : GridControlDecorator", base.ObjectSchema.Alias);
            writer.WriteLine("\t{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            this.WriteConstruct(writer);
            this.WriteDisplayColumns(writer);

            if(base.IsMapTable())
            {
                this.WriteMapEntitySettingDictionary(writer);
            }
            else
            {
                this.WriteEntitySettingDictionary(writer);
            }
        }

        private void WriteConstruct(StringWriter writer)
        {
            foreach (ParentSchema item in base.ObjectSchema.Parents)
            {
                writer.WriteLine("\t\tprivate {0} _{1} = null;", item.Alias, base.LowerFirstLetter(item.Alias));
            }
            writer.WriteLine();

            writer.WriteLine("\t\tpublic Grid{0}Decorator(string userId, GridControl gridControl)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t: base(userId, gridControl)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            foreach (ParentSchema item in base.ObjectSchema.Parents)
            {
                writer.WriteLine("\t\tpublic {0} {0}", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget {{ return this._{0}; }}", base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\tset {{ this._{0} = value; }}", base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }
            writer.WriteLine();
        }

        private void WriteDisplayColumns(StringWriter writer)
        {
            if(base.ObjectSchema.GetPKList().Count == 2)
            {
                writer.WriteLine("\t\tprotected override void SetProperties()");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tbase.SetProperties();");
                writer.WriteLine();
                writer.WriteLine("\t\t\tbase.MenuController.MenuOptions.ShowMulticopyMenu = false;");
                writer.WriteLine("\t\t\tbase.MenuController.MenuOptions.ShowBatchAppendMenu = false;");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

            writer.WriteLine("\t\tprotected override void SetDisplayColumns(GridView view)");
            writer.WriteLine("\t\t{");

            foreach (FieldSchema item in base.ObjectSchema.Fields)
            {
                Type dotnetType = base.Utilities.ToDotNetType(item.DataType);
                if (dotnetType == typeof(Guid))
                    continue;

                if (!dotnetType.IsValueType && dotnetType != typeof(string))
                    continue;

                writer.WriteLine("\t\t\tGridColumn col{0} = new GridColumn();", item.Alias);
                writer.WriteLine("\t\t\tcol{0}.Caption = \"{0}\";", item.Alias);
                writer.WriteLine("\t\t\tcol{0}.FieldName = {1}Schema.{0};", item.Alias, base.ObjectSchema.Alias);
                if(item.IsJoined)
                {
                    writer.WriteLine("\t\t\tcol{0}.OptionsColumn.AllowEdit = false;", item.Alias);
                    writer.WriteLine("\t\t\tcol{0}.OptionsColumn.AllowFocus = false;", item.Alias);
                }

                if (item.Alias == "CreatedOn" || item.Alias == "CreatedBy"
                    || item.Alias == "ModifiedOn" || item.Alias == "ModifiedBy"
                    || item.Alias == "LastModifiedAt" || item.Alias == "LastModifiedBy")
                {
                    writer.WriteLine("\t\t\tcol{0}.VisibleIndex = -1;", item.Alias);
                    writer.WriteLine("\t\t\tcol{0}.OptionsColumn.ShowInCustomizationForm = false;", item.Alias);
                }
                else
                {
                    writer.WriteLine("\t\t\tcol{0}.VisibleIndex = view.Columns.Count;", item.Alias);
                }
                switch (item.DataType.ToLower())
                {
                    case "tinyint":
                    case "smallint":
                    case "int":
                        writer.WriteLine("\t\t\tcol{0}.DisplayFormat.FormatType = FormatType.Numeric;", item.Alias);
                        writer.WriteLine("\t\t\tcol{0}.DisplayFormat.FormatString = \"f0\";", item.Alias);
                        break;
                    case "smallmoney":
                    case "money":
                        writer.WriteLine("\t\t\tcol{0}.DisplayFormat.FormatType = FormatType.Numeric;", item.Alias);
                        writer.WriteLine("\t\t\tcol{0}.DisplayFormat.FormatString = \"c2\";", item.Alias);
                        break;
                    case "float":
                        writer.WriteLine("\t\t\tcol{0}.DisplayFormat.FormatType = FormatType.Numeric;", item.Alias);
                        writer.WriteLine("\t\t\tcol{0}.DisplayFormat.FormatString = \"f2\";", item.Alias);
                        break;
                    case "decimal":
                        writer.WriteLine("\t\t\tcol{0}.DisplayFormat.FormatType = FormatType.Numeric;", item.Alias);

                        //if (item.Alias.ToLower().EndsWith("rate"))
                        //{
                        //    writer.WriteLine("\t\t\tcol{0}.DisplayFormat.FormatString = \"p2\";", item.Alias);
                        //}
                        //else
                        {
                            string[] splits = item.Size.Split(',');
                            if (splits.Length == 2 && splits[1].Length > 1 && char.IsNumber(splits[1][0]))
                            {
                                int scale = int.Parse(splits[1].Substring(0, 1));
                                writer.WriteLine("\t\t\tcol{0}.DisplayFormat.FormatString = \"f{1}\";", item.Alias, scale);
                            }
                            else
                            {
                                writer.WriteLine("\t\t\tcol{0}.DisplayFormat.FormatString = \"f2\";", item.Alias);
                            }
                        }
                        break;
                    case "smalldatetime":
                    case "datetime":
                        writer.WriteLine("\t\t\tcol{0}.DisplayFormat.FormatType = FormatType.DateTime;", item.Alias);
                        writer.WriteLine("\t\t\tcol{0}.DisplayFormat.FormatString = \"MM/dd/yyyy HH:mm:ss\";", item.Alias);
                        break;
                    default:
                        break;
                }
                writer.WriteLine("\t\t\tview.Columns.Add(col{0});", item.Alias);
                writer.WriteLine();
            }

            this.WriteJoinFields(writer);

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteEntitySettingDictionary(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void NavigatorEditClick(GridView view, NavigatorButtonClickEventArgs e)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\te.Handled = true;");
            writer.WriteLine("\t\t\t{0}Collection list = view.DataSource as {0}Collection;", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tif (list == null)");
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t{0} entity = view.GetRow(view.FocusedRowHandle) as {0};", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tif (entity == null)");
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine();
            writer.WriteLine("\t\t\tFormDetail{0} dlg = new FormDetail{0}(base.UserId, entity.Clone() as {0});", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tdlg.ShowDialog();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected override void NavigatorAppendClick(GridView view, NavigatorButtonClickEventArgs e)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\te.Handled = true;");
            writer.WriteLine("\t\t\t{0}Collection list = view.DataSource as {0}Collection;", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tif (list == null)");
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t{0} entity = new {0}();", base.ObjectSchema.Alias);
            foreach (ParentSchema item in base.ObjectSchema.Parents)
            {
                writer.WriteLine("\t\t\tif (this.{0} != null)", item.Alias);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tentity.CopyParent(this.{0});", item.Alias);
                writer.WriteLine("\t\t\t}");
            }
            writer.WriteLine();
            writer.WriteLine("\t\t\tFormDetail{0} dlg = new FormDetail{0}(base.UserId, entity);", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tdlg.ShowDialog();");
            writer.WriteLine("\t\t}");
        }

        private void WriteMapEntitySettingDictionary(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void NavigatorEditClick(GridView view, NavigatorButtonClickEventArgs e)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\te.Handled = true;");
            writer.WriteLine("\t\t\t{0}Collection list = view.DataSource as {0}Collection;", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tif (list == null)");
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t{0} entity = view.GetRow(view.FocusedRowHandle) as {0};", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tif (entity == null)");
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine();

            if(base.GetBindableFields().Count == 0)
            {
                writer.WriteLine("\t\t\tFormDetail{0} dlg = new FormDetail{0}(base.UserId, entity.Clone() as {0}, false);", base.ObjectSchema.Alias);
                writer.WriteLine("\t\t\tdlg.ShowDialog();");
            }
            else
            {
                writer.WriteLine("\t\t\tFormDetail{0} dlg = new FormDetail{0}(base.UserId, entity.Clone() as {0}, this.Editable);", base.ObjectSchema.Alias);
                writer.WriteLine("\t\t\tif (dlg.ShowDialog() == DialogResult.OK)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tentity.CopyFrom(dlg.{0}, true);", base.ObjectSchema.Alias);
                writer.WriteLine();
                writer.WriteLine("\t\t\t\tview.CloseEditor();");
                writer.WriteLine("\t\t\t\tview.UpdateCurrentRow();");
                writer.WriteLine("\t\t\t}");
            }
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected override void NavigatorAppendClick(GridView view, NavigatorButtonClickEventArgs e)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\te.Handled = true;");
            writer.WriteLine("\t\t\t{0}Collection list = view.DataSource as {0}Collection;", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tif (list == null)");
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine();

            if(base.ObjectSchema.Parents.Count != 2)
            {
                writer.WriteLine("\t\t}");
                return;
            }

            ParentSchema firstParent = base.ObjectSchema.Parents[0];
            FieldSchema firstParentPK = base.GetObjectByName(firstParent.Name).GetPKList()[0];
            ParentSchema secondParent = base.ObjectSchema.Parents[1];
            FieldSchema secondParentPK = base.GetObjectByName(secondParent.Name).GetPKList()[0];

            //First
            writer.WriteLine("\t\t\tif (this.{0} != null)", firstParent.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tSortedList<Guid, Guid> pkList = new SortedList<Guid, Guid>();");
            writer.WriteLine("\t\t\t\tforeach ({0} item in list)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tif (pkList.ContainsKey(item.{0}))", secondParentPK.Alias);
            writer.WriteLine("\t\t\t\t\t\tcontinue;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\t\tpkList.Add(item.{0}, item.{0});", secondParentPK.Alias);
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tFormSelect{0} dlg = new FormSelect{0}(base.UserId, pkList);", secondParent.Alias);
            writer.WriteLine("\t\t\t\tif (dlg.ShowDialog() != DialogResult.OK)");
            writer.WriteLine("\t\t\t\t\treturn;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tforeach ({0} item in dlg.{0}List)", secondParent.Alias);
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t{0} entity = new {0}();", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t\t\tentity.CopyParent(item);");
            writer.WriteLine("\t\t\t\t\tentity.CopyParent(this.{0});", firstParent.Alias);
            writer.WriteLine("\t\t\t\t\tlist.Add(entity);");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t}");

            //Second
            writer.WriteLine("\t\t\telse if (this.{0} != null)", secondParent.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tSortedList<Guid, Guid> pkList = new SortedList<Guid, Guid>();");
            writer.WriteLine("\t\t\t\tforeach ({0} item in list)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tif (pkList.ContainsKey(item.{0}))", firstParentPK.Alias);
            writer.WriteLine("\t\t\t\t\t\tcontinue;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\t\tpkList.Add(item.{0}, item.{0});", firstParentPK.Alias);
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tFormSelect{0} dlg = new FormSelect{0}(base.UserId, pkList);", firstParent.Alias);
            writer.WriteLine("\t\t\t\tif (dlg.ShowDialog() != DialogResult.OK)");
            writer.WriteLine("\t\t\t\t\treturn;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tforeach ({0} item in dlg.{0}List)", firstParent.Alias);
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t{0} entity = new {0}();", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t\t\tentity.CopyParent(item);");
            writer.WriteLine("\t\t\t\t\tentity.CopyParent(this.{0});", secondParent.Alias);
            writer.WriteLine("\t\t\t\t\tlist.Add(entity);");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t}");

            writer.WriteLine("\t\t}");
        }

        #region WriteJoin Fileds

        private void WriteJoinFields(StringWriter writer)
        {
            Type type = Utils.GetDataType(this.MappingSchema, this.ObjectSchema); 
            if (type == null)
                return;

            SortedList<string, string> sortedList = new SortedList<string, string>();
            foreach (FieldSchema item in base.ObjectSchema.Fields)
            {
                if(sortedList.ContainsKey(item.Name))
                    continue;

                sortedList.Add(item.Name, item.Name);
            }

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
            foreach (PropertyInfo info in properties)
            {
                if (info.Name == "IsDirty" || info.Name == "IsValid" || info.Name == "PKString"
                        || info.Name == "MarkAsDeleted" || info.Name == "TableName")
                    continue;

                if (info.PropertyType == typeof(Guid))
                    continue;

                if (!info.PropertyType.IsValueType && info.PropertyType != typeof(string))
                    continue;

                if(sortedList.ContainsKey(info.Name))
                    continue;

                writer.WriteLine("\t\t\tGridColumn col{0} = new GridColumn();", info.Name);
                writer.WriteLine("\t\t\tcol{0}.Caption = \"{0}\";", info.Name);
                writer.WriteLine("\t\t\tcol{0}.FieldName = {1}Schema.{0};", info.Name, type.Name.Substring(0, type.Name.Length - 4));
                writer.WriteLine("\t\t\tcol{0}.OptionsColumn.AllowEdit = false;", info.Name);
                writer.WriteLine("\t\t\tcol{0}.OptionsColumn.AllowFocus = false;", info.Name);
                writer.WriteLine("\t\t\tcol{0}.VisibleIndex = view.Columns.Count;", info.Name);
                writer.WriteLine("\t\t\tview.Columns.Add(col{0});", info.Name);
                writer.WriteLine();
            }
        }
        #endregion
    }
}
