using System.IO;
using JetCode.Factory;
using JetCode.BizSchema;
using System;

namespace JetCode.FactoryWinUI
{
    public class FactoryGridSelectDecorator : FactoryBase
    {
        public FactoryGridSelectDecorator(MappingSchema mappingSchema, ObjectSchema objectSchema)
            : base(mappingSchema, objectSchema)
        {
        }


        protected  override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using {0}.Schema;", base.ProjectName);
            writer.WriteLine("using Cheke.WinCtrl.Decoration;");
            writer.WriteLine("using DevExpress.Data;");
            writer.WriteLine("using DevExpress.Utils;");
            writer.WriteLine("using DevExpress.XtraEditors;");
            writer.WriteLine("using DevExpress.XtraGrid;");
            writer.WriteLine("using DevExpress.XtraGrid.Columns;");
            writer.WriteLine("using DevExpress.XtraGrid.Views.Grid;");

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Manager.GridSelect", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic class GridSelect{0}Decorator : GridControlDecorator", base.ObjectSchema.Alias);
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
            this.WriteInitialize(writer);
            this.WriteDisplayColumns(writer);
            this.WriteEntitySettingDictionary(writer);
        }

        private void WriteConstruct(StringWriter writer)
        {
            writer.WriteLine("\t\tpublic GridSelect{0}Decorator(string userId, GridControl gridControl)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t: base(userId, gridControl)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteInitialize(StringWriter writer)
        {
            writer.WriteLine("\t\tpublic override void Initialize()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase.Initialize();");
            writer.WriteLine();
            writer.WriteLine("\t\t\tbase.GridView.OptionsView.ShowGroupPanel = false;");
            writer.WriteLine("\t\t\tbase.MenuController.MenuOptions.ShowHistoryMenu = false;");
            writer.WriteLine("\t\t\tbase.MenuController.MenuOptions.ShowRefreshMenu = false;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteDisplayColumns(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void SetDisplayColumns(GridView view)");
            writer.WriteLine("\t\t{");

            foreach (FieldSchema item in base.ObjectSchema.Fields)
            {
                if(item.IsNullable)
                    continue;

                Type dotnetType = base.Utilities.ToDotNetType(item.DataType);
                if (dotnetType == typeof(Guid))
                    continue;

                if (!dotnetType.IsValueType && dotnetType != typeof(string))
                    continue;

                writer.WriteLine("\t\t\tGridColumn col{0} = new GridColumn();", item.Alias);
                writer.WriteLine("\t\t\tcol{0}.Caption = \"{0}\";", item.Alias);
                writer.WriteLine("\t\t\tcol{0}.FieldName = {1}ViewSchema.{0};", item.Alias, base.ObjectSchema.Alias);
                writer.WriteLine("\t\t\tcol{0}.VisibleIndex = view.Columns.Count;", item.Alias);
                writer.WriteLine("\t\t\tcol{0}.OptionsColumn.AllowEdit = false;", item.Alias);
                writer.WriteLine("\t\t\tcol{0}.OptionsColumn.AllowFocus = false;", item.Alias);

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


            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteEntitySettingDictionary(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void NavigatorEditClick(GridView view, NavigatorButtonClickEventArgs e)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\te.Handled = true;");
            writer.WriteLine("\t\t}");
        }
    }
}
