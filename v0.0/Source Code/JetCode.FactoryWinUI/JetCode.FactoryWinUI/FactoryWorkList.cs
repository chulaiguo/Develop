using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
    public class FactoryWorkList : FactoryBase
    {
        public FactoryWorkList(MappingSchema mappingSchema, ObjectSchema objectSchema)
            : base(mappingSchema, objectSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
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
            writer.WriteLine("namespace {0}.Manager.FormWorkList", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic partial class FormWork{0}List : FormWorkListBase", base.ObjectSchema.Alias);
            writer.WriteLine("\t{");

            writer.WriteLine("\t\tprivate {0}Collection _list = null;", base.ObjectSchema.Alias);
            writer.WriteLine("\t\tprivate Grid{0}Decorator _decorator = null;", base.ObjectSchema.Alias);
            writer.WriteLine();

            writer.WriteLine("\t\tpublic FormWork{0}List()", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tInitializeComponent();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic FormWork{0}List(string userid, Control parent)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t: base(userid, parent)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tInitializeComponent();");
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
            this.WriteInit(writer);
            this.WriteDataBinding(writer);
            if (this.HasUnknownFactor())
            {
                this.WriteCommentImport(writer);
            }
            else
            {
                this.WriteImport(writer);
            }
        }

        private void WriteInit(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void InitializeDecorator()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._decorator = new Grid{0}Decorator(base.UserId, this.gridControl1);", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tthis._decorator.Initialize();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected override void InitializeForm()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase.InitializeForm();");
            writer.WriteLine("\t\t\tthis.Caption = \"{0} List\";", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tthis.ShowImportButton = FormMain.Instance.HasAddNewPrivilege({0}Schema.TableName);", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteDataBinding(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void DataBinding()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis.Cursor = Cursors.WaitCursor;");
            writer.WriteLine("\t\t\tthis._list = {0}.GetAll();", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tthis._decorator.DataSource = this._list;");
            writer.WriteLine("\t\t\tthis.Cursor = Cursors.Default;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private bool HasUnknownFactor()
        {
            FieldSchemaCollection list = base.GetReadableUKFields(base.ObjectSchema.Name);
            if (list == null || list.Count == 0)
                return true;

            foreach (ParentSchema item in base.ObjectSchema.Parents)
            {
                list = base.GetReadableUKFields(item.Name);
                if (list == null || list.Count == 0)
                    return true;
            }

            return false;
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

        private void WriteCommentImport(StringWriter writer)
        {
            writer.WriteLine("\t\t//protected override void ProcessImportData(BusinessCollectionBase src, BusinessCollectionBase imported, List<PropertyInfo> columnList)");
            writer.WriteLine("\t\t//{");
            writer.WriteLine("\t\t\t//{0}Collection importedList = imported as {0}Collection;", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t//if (importedList == null)");
            writer.WriteLine("\t\t\t\t//return;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t//{0}Collection srcList = src as {0}Collection;", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t//if (srcList == null)");
            writer.WriteLine("\t\t\t\t//return;");
            writer.WriteLine();
            foreach (ParentSchema item in base.ObjectSchema.Parents)
            {
                writer.WriteLine("\t\t\t//{0}", item.Alias);
                writer.WriteLine("\t\t\t//SortedList<string, {0}> {1}Index = new SortedList<string, {0}>();", item.Alias, base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t////{0}ViewCollection {1}ViewList = {0}.GetViewAll();", item.Alias, base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t////{0}Collection {1}List = new {0}Collection();", item.Alias, base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t////{0}List.AddRange({0}ViewList);", base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t//{0}Collection {1}List = {0}.GetAll();", item.Alias, base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t//foreach ({0} item in {1}List)", item.Alias, base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t//{");
                writer.WriteLine("\t\t\t\t//{0}", this.GetKeys(item.Name, item.Alias));
                writer.WriteLine("\t\t\t\t//if ({0}Index.ContainsKey({0}Key))", base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t\t\t//continue;");
                writer.WriteLine();
                writer.WriteLine("\t\t\t\t//{0}Index.Add({0}Key, item);", base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t//}");
                writer.WriteLine();
            }
            writer.WriteLine("\t\t\t//SortedList<string, {0}> srcIndex = new SortedList<string, {0}>();", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t//foreach ({0} item in srcList)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t//{");
            writer.WriteLine("\t\t\t\t//{0}", this.GetKeys(base.ObjectSchema.Name, base.ObjectSchema.Alias));
            writer.WriteLine("\t\t\t\t//if (srcIndex.ContainsKey({0}Key))", base.LowerFirstLetter(base.ObjectSchema.Alias));
            writer.WriteLine("\t\t\t\t\t//continue;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\t//srcIndex.Add({0}Key, item);", base.LowerFirstLetter(base.ObjectSchema.Alias));
            writer.WriteLine("\t\t\t//}");
            writer.WriteLine();
            writer.WriteLine("\t\t\t//foreach ({0} item in importedList)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t//{");
            writer.WriteLine("\t\t\t\t//{0}", this.GetKeys(base.ObjectSchema.Name, base.ObjectSchema.Alias));
            writer.WriteLine("\t\t\t\t//if (!srcIndex.ContainsKey({0}Key))", base.LowerFirstLetter(base.ObjectSchema.Alias));
            writer.WriteLine("\t\t\t\t//{");
            foreach (ParentSchema item in base.ObjectSchema.Parents)
            {
                writer.WriteLine("\t\t\t\t\t////{0}", item.Alias);
                writer.WriteLine("\t\t\t\t\t//{0}", this.GetKeys(item.Name, item.Alias));
                writer.WriteLine("\t\t\t\t\t//if({0}Index.ContainsKey({0}Key))", base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t\t\t//{");
                writer.WriteLine("\t\t\t\t\t\t//item.{0} = {1}Index[{1}Key];", item.Alias, base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t\t\t//}");
                writer.WriteLine();
            }
            writer.WriteLine("\t\t\t\t\t//srcIndex.Add({0}Key, item);", base.LowerFirstLetter(base.ObjectSchema.Alias));
            writer.WriteLine("\t\t\t\t\t//srcList.Add(item);");
            writer.WriteLine("\t\t\t\t//}");
            writer.WriteLine("\t\t\t\t//else");
            writer.WriteLine("\t\t\t\t//{");
            writer.WriteLine("\t\t\t\t\t//{0} srcEntity = srcIndex[{1}Key];", base.ObjectSchema.Alias, base.LowerFirstLetter(base.ObjectSchema.Alias));
            writer.WriteLine();
            foreach (ParentSchema item in base.ObjectSchema.Parents)
            {
                writer.WriteLine("\t\t\t\t\t////{0}", item.Alias);
                writer.WriteLine("\t\t\t\t\t//{0}", this.GetKeys(item.Name, item.Alias));
                writer.WriteLine("\t\t\t\t\t//if({0}Index.ContainsKey({0}Key))", base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t\t\t//{");
                writer.WriteLine("\t\t\t\t\t\t//srcEntity.{0} = {1}Index[{1}Key];", item.Alias, base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t\t\t//}");
                writer.WriteLine();
            }
            writer.WriteLine("\t\t\t\t\t//foreach (PropertyInfo property in columnList)");
            writer.WriteLine("\t\t\t\t\t//{");
            writer.WriteLine("\t\t\t\t\t\t//object obj = property.GetValue(item, null);");
            writer.WriteLine("\t\t\t\t\t\t//if (obj == null)");
            writer.WriteLine("\t\t\t\t\t\t\t//continue;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\t\t\t//property.SetValue(srcEntity, obj, null);");
            writer.WriteLine("\t\t\t\t\t//}");
            writer.WriteLine("\t\t\t\t//}");
            writer.WriteLine("\t\t\t//}");
            writer.WriteLine("\t\t//}");
            writer.WriteLine();
        }


        private string GetKeys(string name, string alias)
        {
            FieldSchemaCollection list = base.GetReadableUKFields(name);
            if (list == null)
            {
                list = base.GetRequiredFields(name);
            }

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
