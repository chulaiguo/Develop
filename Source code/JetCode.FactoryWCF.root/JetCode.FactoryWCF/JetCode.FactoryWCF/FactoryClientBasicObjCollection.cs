using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWCF
{
    public class FactoryClientBasicObjCollection : FactoryBase
    {
        public FactoryClientBasicObjCollection(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Collections;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("using Cheke.BusinessEntity;");

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.ViewObj", base.ProjectName);
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
                writer.WriteLine("\tpublic partial class {0}Collection : BusinessCollectionBase, IList<{0}>", item.Alias);
                writer.WriteLine("\t{");
                writer.WriteLine("\t\tprivate readonly List<{0}> _list = new List<{0}>();", item.Alias);
                writer.WriteLine("\t\tprivate readonly List<{0}> _deletedList = new List<{0}>();", item.Alias);
                writer.WriteLine();
                writer.WriteLine("\t\tpublic {0}Collection()", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic override Type ItemType");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget {{ return typeof({0}); }}", item.Alias);
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tprotected override IList InnerList");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget { return this._list; }");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tprotected override IList InnerDeletedList");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget { return this._deletedList; }");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t#region IList<{0}> Members", item.Alias);
                writer.WriteLine();
                writer.WriteLine("\t\tpublic int IndexOf({0} item)", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\treturn this._list.IndexOf(item);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic void Insert(int index, {0} item)", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tthis._list.Insert(index, item);");
                writer.WriteLine("\t\t\tbase.OnInsertItem(index, item);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic void RemoveAt(int index)");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\t{0} item = this._list[index];", item.Alias);
                writer.WriteLine("\t\t\tif(item == null)");
                writer.WriteLine("\t\t\t\treturn;");
                writer.WriteLine();
                writer.WriteLine("\t\t\tthis._deletedList.Add(item);");
                writer.WriteLine("\t\t\tthis._list.RemoveAt(index);");
                writer.WriteLine("\t\t\tbase.OnRemoveItem(item, index);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic {0} this[int index]", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\treturn this._list[index];");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\tset");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tthis._list[index] = value;");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t#endregion");
                writer.WriteLine();
                writer.WriteLine("\t\t#region ICollection<{0}> Members", item.Alias);
                writer.WriteLine();
                writer.WriteLine("\t\tpublic void Add({0} item)", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tthis.Insert(this._list.Count, item);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic void Clear()");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tforeach({0} item in this._list)", item.Alias);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tthis._deletedList.Add(item);");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\tthis._list.Clear();");
                writer.WriteLine("\t\t\tbase.OnClearItems();");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic bool Contains({0} item)", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\treturn this._list.Contains(item);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic void CopyTo({0}[] array, int arrayIndex)", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tthis._list.CopyTo(array, arrayIndex);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic int Count");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget { return this._list.Count; }");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic bool IsReadOnly");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget { return false; }");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic bool Remove({0} item)", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tint index = this.IndexOf(item);");
                writer.WriteLine("\t\t\tif (index == -1)"); 
                writer.WriteLine("\t\t\t\treturn false;");
                writer.WriteLine();
                writer.WriteLine("\t\t\tthis.RemoveAt(index);");
                writer.WriteLine("\t\t\treturn true;");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t#endregion");
                writer.WriteLine();
                writer.WriteLine("\t\t#region IEnumerable<{0}> Members", item.Alias);
                writer.WriteLine();
                writer.WriteLine("\t\tpublic IEnumerator<{0}> GetEnumerator()", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\treturn this._list.GetEnumerator();");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t#endregion");
                writer.WriteLine();
                writer.WriteLine("\t\t#region IEnumerable Members");
                writer.WriteLine();
                writer.WriteLine("\t\tIEnumerator IEnumerable.GetEnumerator()");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\treturn ((IEnumerable)this._list).GetEnumerator();");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t#endregion");
                writer.WriteLine();
                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }
    }
}
