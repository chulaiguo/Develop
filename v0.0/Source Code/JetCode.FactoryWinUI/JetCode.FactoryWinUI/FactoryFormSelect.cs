using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
    public class FactoryFormSelect : FactoryBase
    {
        public FactoryFormSelect(MappingSchema mappingSchema, ObjectSchema objectSchema)
            : base(mappingSchema, objectSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("using Cheke.WinCtrl;");
            writer.WriteLine("using {0}.Data;", base.ProjectName);
            writer.WriteLine("using {0}.ViewObj;", base.ProjectName);
            writer.WriteLine("using {0}.Manager.GridSelect;", base.ProjectName);
            writer.WriteLine("using {0}.Manager.FormDetailEditor;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Manager.FormSelect", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic partial class FormSelect{0} : FormSelectBase", base.ObjectSchema.Alias);
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tprivate GridSelect{0}Decorator _decorator = null;", base.ObjectSchema.Alias);
            writer.WriteLine("\t\tprivate readonly SortedList<Guid, Guid> _pkList = null;");

            writer.WriteLine();
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            this.WriteConstruct(writer);
            this.WriteInitializeDecorator(writer);
            this.WriteInitializeForm(writer);
            this.WriteShowDetail(writer);
            this.WriteDataBinding(writer);
        }

        private void WriteConstruct(StringWriter writer)
        {
            writer.WriteLine("\t\tpublic FormSelect{0}()", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tInitializeComponent();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic FormSelect{0}(string userid)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t: base(userid)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tInitializeComponent();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic FormSelect{0}(string userid, SortedList<Guid, Guid> pkList)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t: base(userid)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tInitializeComponent();");
            writer.WriteLine();
            writer.WriteLine("\t\t\tthis._pkList = pkList;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0} {0}", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget{{ return base.SelectedEntity as {0}; }}", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0}Collection {0}List", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget{{ return base.SelectedList as {0}Collection; }}", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteInitializeDecorator(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void InitializeDecorator()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._decorator = new GridSelect{0}Decorator(base.UserId, base.GridControl);", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tthis._decorator.Initialize();");
            writer.WriteLine("\t\t\tthis._decorator.Editable = false;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteInitializeForm(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void InitializeForm()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase.InitializeForm();");
            writer.WriteLine("\t\t\tthis.DataBinding();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteShowDetail(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void ShowDetail()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0} entity = {0}.GetByPK(this.{0}.{0}PK);", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tif (entity == null)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tbase.ShowNoViewPrivilegeWarning();");
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            if (this.IsMapTable())
            {
                writer.WriteLine("\t\t\tFormDetail{0} dlg = new FormDetail{0}(base.UserId, entity, false);", base.ObjectSchema.Alias);
                writer.WriteLine("\t\t\tdlg.ShowDialog();");
            }
            else
            {
                writer.WriteLine("\t\t\tFormDetail{0} dlg = new FormDetail{0}(base.UserId, entity);", base.ObjectSchema.Alias);
                writer.WriteLine("\t\t\tdlg.ShowDialog();");
            }
           
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteDataBinding(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void DataBinding()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._decorator.DataSource = this.GetDataSource();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate {0}Collection GetDataSource()", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}ViewCollection list = this.GetOriginalDataSource();", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t{0}Collection retList = new {0}Collection();", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tforeach ({0}View item in list)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif(this._pkList != null && this._pkList.ContainsKey(item.{0}PK))", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t\t\tcontinue;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tretList.Add(new {0}(item.To{0}Data()));", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tretList.AcceptChanges();");
            writer.WriteLine("\t\t\treturn retList;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate {0}ViewCollection GetOriginalDataSource()", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn {0}.GetViewAll();", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t}");
        }
    }
}
