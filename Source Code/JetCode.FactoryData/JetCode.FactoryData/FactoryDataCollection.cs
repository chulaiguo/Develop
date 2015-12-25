using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryData
{
    public class FactoryDataCollection : FactoryBase
    {
        public FactoryDataCollection(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Data;");
            writer.WriteLine("using Cheke.BusinessEntity;");

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Data", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {   
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                writer.WriteLine("\t[Serializable]");
                writer.WriteLine("\tpublic class {0}DataCollection : BusinessCollectionBase", item.Alias);
                writer.WriteLine("\t{");

                this.WriteConstructor(writer, item);
                this.WriteTableName(writer, item);
                this.WriteAdd(writer, item);
                this.WriteAddRange(writer, item);
                this.WriteRemove(writer, item);
                this.WriteInsert(writer, item);
                this.WriteContains(writer, item);
                this.WriteContainsDeleted(writer, item);
                this.WriteItem(writer, item);
                this.WriteOnValidate(writer, item);

                writer.WriteLine("\t}");
            }
        }

        private void WriteConstructor(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\t#region Constructors");

            writer.WriteLine("\t\tpublic {0}DataCollection()", item.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase._itemType = typeof({0}Data);", item.Alias);
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0}DataCollection({0}DataCollection list)", item.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase._itemType = typeof({0}Data);", item.Alias);
            writer.WriteLine("\t\t\tforeach({0}Data item in list)", item.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.Add(new {0}Data(item));", item.Alias);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tSystem.Collections.ArrayList deletedList = list.GetDeletedList();");
            writer.WriteLine("\t\t\tforeach({0}Data item in deletedList)", item.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t{0}Data entity = new {0}Data(item);", item.Alias);
            writer.WriteLine("\t\t\t\tthis.Add(entity);");
            writer.WriteLine("\t\t\t\tthis.Remove(entity);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();
        }

        private void WriteTableName(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic override string TableName");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget {{ return \"{0}\"; }}", item.Alias);
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteAdd(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic void Add({0}Data obj)", item.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase.List.Add(obj);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteAddRange(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic void AddRange({0}DataCollection list)", item.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tforeach({0}Data item in list)", item.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.Add(item);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteRemove(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic void Remove({0}Data obj)", item.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase.List.Remove(obj);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteInsert(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic void Insert(int index, {0}Data obj)", item.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t base.List.Insert(index, obj);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteContains(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic bool Contains({0}Data item)", item.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tforeach ({0}Data data in base.List)", item.Name);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif (data.Equals(item))");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t return true;");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\treturn false;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteContainsDeleted(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic bool ContainsDeleted({0}Data item)", item.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tforeach ({0}Data data in  this._deletedList)", item.Name);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif (data.Equals(item))");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t return true;");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\treturn false;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteOnValidate(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tprotected override void OnValidate(object item)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tSystem.Type t = item.GetType();");
            writer.WriteLine("\t\t\tif (t != base._itemType && !t.IsSubclassOf(base._itemType))");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tthrow new ArgumentException(\"The item must be a type of {0}Data\");", item.Alias);
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteItem(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic new {0}Data this[int index]", item.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn ({0}Data) base.List[index];", item.Name);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tset");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tbase.List[index] = value;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }
    }
}