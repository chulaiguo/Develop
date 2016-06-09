using System;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
    public class FactoryFormPickup : FactoryBase
    {
        public FactoryFormPickup(MappingSchema mappingSchema, ObjectSchema objectSchema)
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
            writer.WriteLine("using {0}.Schema;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Manager.FormPickup", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic partial class FormPickup{0} : FormPickupBase", base.ObjectSchema.Alias);
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tprivate SortedList<Guid, Guid> _pkList = null;");
            writer.WriteLine("\t\tprivate {0}Collection _list = null;", base.ObjectSchema.Alias);
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
            this.WriteInitializeForm(writer);
            this.WritePickup(writer);
        }

        private void WriteConstruct(StringWriter writer)
        {
            writer.WriteLine("\t\tpublic FormPickup{0}()", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tInitializeComponent();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic FormPickup{0}(SortedList<Guid, Guid> pkList)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tInitializeComponent();");
            writer.WriteLine("\t\t\tthis._pkList = pkList;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0}Collection {0}List", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget{ return this._list; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteInitializeForm(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void InitializeForm()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase.InitializeForm();");
            writer.WriteLine();            
            writer.WriteLine("\t\t\t{0}ViewCollection dataSource = new {0}ViewCollection();", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t{0}ViewCollection viewList = {0}.GetViewAll();", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tforeach ({0}View item in viewList)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif (this._pkList.ContainsKey(item.{0}PK))", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t\t\tcontinue;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tdataSource.Add(item);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tif (dataSource.Count == 0)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.CanPickup = false;");
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            string uk = this.GetUKField();
            if(uk.Length == 0)
            {
                writer.WriteLine("\t\t\t//this.ListBox.DisplayMember = {0}ViewSchema.XXX;", base.ObjectSchema.Alias);
            }
            else
            {
                writer.WriteLine("\t\t\tthis.ListBox.DisplayMember = {0}ViewSchema.{1};", base.ObjectSchema.Alias, uk);
            }
            writer.WriteLine("\t\t\tthis.ListBox.DataSource = dataSource;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private string GetUKField()
        {
            FieldSchemaCollection list = base.GetUKFields();
            if (list.Count == 0)
                return string.Empty;

            foreach (FieldSchema item in list)
            {
                Type dotnetType = base.Utilities.ToDotNetType(item.DataType);
                if (dotnetType == typeof(string))
                    return item.Alias;
            }

            return list[0].Alias;
        }

        private void WritePickup(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void Pickup()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}ViewCollection viewList = new {0}ViewCollection();", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tforeach (object item in this.ListBox.CheckedItems)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t{0}View entity = item as {0}View;", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t\tif (entity == null)");
            writer.WriteLine("\t\t\t\t\tcontinue;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tviewList.Add(entity);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tthis._list = new {0}Collection(viewList.To{0}DataCollection());", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t}");
        }
    }
}
