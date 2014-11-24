using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
    public class FactoryWorkSearch : FactoryBase
    {
        public FactoryWorkSearch(MappingSchema mappingSchema, ObjectSchema objectSchema)
            : base(mappingSchema, objectSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Reflection;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("using System.Windows.Forms;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using Cheke.WinCtrl;");
            writer.WriteLine("using {0}.Schema;", base.ProjectName);
            writer.WriteLine("using {0}.Data;", base.ProjectName);
            writer.WriteLine("using {0}.ViewObj;", base.ProjectName);
            writer.WriteLine("using {0}.Manager.GridDecorator;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Manager.FormWorkSearch", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic partial class FormWork{0}Search : FormWorkSearchBase", base.ObjectSchema.Alias);
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
            this.WriteOverride(writer);
            this.WriteDataBinding(writer);
            this.WriteEvents(writer);
            this.WriteImport(writer);
        }

        private void WriteConstruct(StringWriter writer)
        {
            writer.WriteLine("\t\tprivate {0}Collection _list = null;", base.ObjectSchema.Alias);
            writer.WriteLine("\t\tprivate Grid{0}Decorator _decorator = null;", base.ObjectSchema.Alias);
            writer.WriteLine();

            writer.WriteLine("\t\tpublic FormWork{0}Search()", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tInitializeComponent();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic FormWork{0}Search(string userId, Control parent)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t: base(userId, parent)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tInitializeComponent();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteOverride(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void InitializeDecorator()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._decorator = new Grid{0}Decorator(base.UserId, base.GridControl);", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tthis._decorator.Initialize();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected override void InitializeForm()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase.InitializeForm();");
            writer.WriteLine("\t\t\tthis.Caption = \"{0} Search\";", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tthis.ShowImportButton = FormMain.Instance.HasAddNewPrivilege({0}Schema.TableName);", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteDataBinding(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void DataBinding()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t//this._list = {0}.Search(...);", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tthis._list = {0}.GetAll();//for test", base.ObjectSchema.Alias);
            writer.WriteLine();
            writer.WriteLine("\t\t\tthis._decorator.DataSource = this._list;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteEvents(StringWriter writer)
        {
            writer.WriteLine("\t\tprivate void btnSearch_Click(object sender, EventArgs e)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase.Search();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteImport(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void ProcessImportData(BusinessCollectionBase src, BusinessCollectionBase imported, List<PropertyInfo> columnList)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}Collection importedList = imported as {0}Collection;", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tif (importedList == null)");
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t{0}Collection srcList = src as {0}Collection;", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tif (srcList == null)");
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine();
            foreach (ParentSchema item in base.ObjectSchema.Parents)
            {
                writer.WriteLine("\t\t\t//{0}", item.Alias);
                writer.WriteLine("\t\t\tSortedList<string, {0}> {1}Index = new SortedList<string, {0}>();", item.Alias, base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t//{0}ViewCollection {1}ViewList = {0}.GetViewAll();", item.Alias, base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t//{0}Collection {1}List = new {0}Collection();", item.Alias, base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t//{0}List.AddRange({0}ViewList);", base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t{0}Collection {1}List = {0}.GetAll();", item.Alias, base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\tforeach ({0} item in {1}List)", item.Alias, base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\t{0}", this.GetKeys(item.Name, item.Alias));
                writer.WriteLine("\t\t\t\tif ({0}Index.ContainsKey({0}Key))", base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t\t\tcontinue;");
                writer.WriteLine();
                writer.WriteLine("\t\t\t\t{0}Index.Add({0}Key, item);", base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();
            }
            writer.WriteLine("\t\t\tSortedList<string, {0}> srcIndex = new SortedList<string, {0}>();", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tforeach ({0} item in srcList)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t{0}", this.GetKeys(base.ObjectSchema.Name, base.ObjectSchema.Alias));
            writer.WriteLine("\t\t\t\tif (srcIndex.ContainsKey({0}Key))", base.LowerFirstLetter(base.ObjectSchema.Alias));
            writer.WriteLine("\t\t\t\t\tcontinue;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tsrcIndex.Add({0}Key, item);", base.LowerFirstLetter(base.ObjectSchema.Alias));
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tforeach ({0} item in importedList)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t{0}", this.GetKeys(base.ObjectSchema.Name, base.ObjectSchema.Alias));
            writer.WriteLine("\t\t\t\tif (!srcIndex.ContainsKey({0}Key))", base.LowerFirstLetter(base.ObjectSchema.Alias));
            writer.WriteLine("\t\t\t\t{");
            foreach (ParentSchema item in base.ObjectSchema.Parents)
            {
                writer.WriteLine("\t\t\t\t\t//{0}", item.Alias);
                writer.WriteLine("\t\t\t\t\t{0}", this.GetKeys(item.Name, item.Alias));
                writer.WriteLine("\t\t\t\t\tif({0}Index.ContainsKey({0}Key))", base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\titem.{0} = {1}Index[{1}Key];", item.Alias, base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t\t\t}");
                writer.WriteLine();
            }
            writer.WriteLine("\t\t\t\t\tsrcIndex.Add({0}Key, item);", base.LowerFirstLetter(base.ObjectSchema.Alias));
            writer.WriteLine("\t\t\t\t\tsrcList.Add(item);");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\telse");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t{0} srcEntity = srcIndex[{1}Key];", base.ObjectSchema.Alias, base.LowerFirstLetter(base.ObjectSchema.Alias));
            writer.WriteLine();
            foreach (ParentSchema item in base.ObjectSchema.Parents)
            {
                writer.WriteLine("\t\t\t\t\t//{0}", item.Alias);
                writer.WriteLine("\t\t\t\t\t{0}", this.GetKeys(item.Name, item.Alias));
                writer.WriteLine("\t\t\t\t\tif({0}Index.ContainsKey({0}Key))", base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\tsrcEntity.{0} = {1}Index[{1}Key];", item.Alias, base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t\t\t}");
                writer.WriteLine();
            }
            writer.WriteLine("\t\t\t\t\tforeach (PropertyInfo property in columnList)");
            writer.WriteLine("\t\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t\tobject obj = property.GetValue(item, null);");
            writer.WriteLine("\t\t\t\t\t\tif (obj == null)");
            writer.WriteLine("\t\t\t\t\t\t\tcontinue;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\t\t\tproperty.SetValue(srcEntity, obj, null);");
            writer.WriteLine("\t\t\t\t\t}");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private string GetKeys(string name, string alias)
        {
            FieldSchemaCollection list = base.GetReadableUKFields(name);
            if (list == null)
                list = base.GetRequiredFields(name);

            if (list == null || list.Count == 0)
                return "string " + base.LowerFirstLetter(alias) + "Key = string.Format(\"{0}\", item.XXX)";

            string format = string.Empty;
            string value = string.Empty;
            int index = 0;
            foreach (FieldSchema item in list)
            {
                format += "{" + string.Format("{0}", index) + "}";
                format += index == list.Count - 1 ? string.Empty : ":";
                value += ", item." + item.Alias;

                index++;
            }

            return "string " + base.LowerFirstLetter(alias) + "Key = string.Format(\"" + format + "\"" + value + ");";
        }
    }
}
