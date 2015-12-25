using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetCode.Factory;
using JetCode.BizSchema;
using System;

namespace JetCode.FactoryWinUI
{
    public class FactoryGridSelectDecorator2 : FactoryBase
    {
        public FactoryGridSelectDecorator2(MappingSchema mappingSchema, ObjectSchema objectSchema)
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

            List<PropertyInfo> retList = this.GetViewFields();
            foreach (PropertyInfo item in retList)
            {
                writer.WriteLine("\t\t\tGridColumn col{0} = new GridColumn();", item.Name);
                writer.WriteLine("\t\t\tcol{0}.Caption = \"{0}\";", item.Name);
                writer.WriteLine("\t\t\tcol{0}.FieldName = {1}ViewSchema.{0};", item.Name, base.ObjectSchema.Alias);
                writer.WriteLine("\t\t\tcol{0}.VisibleIndex = view.Columns.Count;", item.Name);
                writer.WriteLine("\t\t\tcol{0}.OptionsColumn.AllowEdit = false;", item.Name);
                writer.WriteLine("\t\t\tcol{0}.OptionsColumn.AllowFocus = false;", item.Name);
                writer.WriteLine("\t\t\tview.Columns.Add(col{0});", item.Name);
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

        private List<PropertyInfo> GetViewFields()
        {
            List<PropertyInfo> retList = new List<PropertyInfo>();
            Type type = Utils.GetDataViewType(this.MappingSchema, this.ObjectSchema);
            if (type == null)
                return retList;

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
            foreach (PropertyInfo info in properties)
            {
                if (!info.CanWrite || !info.CanRead)
                    continue;

                if(info.PropertyType == typeof(Guid))
                    continue;

                if (info.PropertyType.IsValueType || info.PropertyType == typeof(string))
                {
                    retList.Add(info);
                }
            }

            return retList;
        }
    }
}
