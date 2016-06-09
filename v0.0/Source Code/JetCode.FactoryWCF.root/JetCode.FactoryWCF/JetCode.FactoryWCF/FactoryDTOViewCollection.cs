using System;
using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWCF
{
    public class FactoryDTOViewCollection : FactoryBase
    {
        public FactoryDTOViewCollection(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Runtime.Serialization;");
            writer.WriteLine("using System.Collections;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.DTO", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            string dllName = string.Format("{0}.Data.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> item in typeList)
            {
                if (!item.Key.EndsWith("Collection"))
                    continue;

                if (item.Key.EndsWith("DataCollection"))
                    continue;

                string objName = item.Key.Substring(0, item.Key.Length - "Collection".Length);

                writer.WriteLine("\t[DataContract]");
                writer.WriteLine("\tpublic class {0}DTOCollection : IList<{0}DTO>", objName);
                writer.WriteLine("\t{");
                writer.WriteLine("\t\tprivate int _blockCount = -1;");
                writer.WriteLine("\t\tprivate int _blockIndex = -1;");
                writer.WriteLine("\t\tprivate int _blockSize = -1;");
                writer.WriteLine();
                writer.WriteLine("\t\tprivate List<{0}DTO> _list = null;", objName);
                writer.WriteLine();
                writer.WriteLine("\t\tpublic {0}DTOCollection()", objName);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tthis._list = new List<{0}DTO>();", objName);
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t[DataMember]");
                writer.WriteLine("\t\tpublic int BlockCount");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget { return this._blockCount; }");
                writer.WriteLine("\t\t\tset { this._blockCount = value; }");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t[DataMember]");
                writer.WriteLine("\t\tpublic int BlockIndex");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget { return this._blockIndex; }");
                writer.WriteLine("\t\t\tset { this._blockIndex = value; }");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t[DataMember]");
                writer.WriteLine("\t\tpublic int BlockSize");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget { return this._blockSize; }");
                writer.WriteLine("\t\t\tset { this._blockSize = value; }");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t#region IList<{0}DTO> Members", objName);
                writer.WriteLine();
                writer.WriteLine("\t\tpublic int IndexOf({0}DTO item)", objName);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\treturn this._list.IndexOf(item);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t[DataMember]");
                writer.WriteLine("\t\tpublic List<{0}DTO> InnerList", objName);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget { return this._list; }");
                writer.WriteLine("\t\t\tset { this._list = value; }");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic void Insert(int index, {0}DTO item)", objName);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tthis._list.Insert(index, item);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic void RemoveAt(int index)");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tthis._list.RemoveAt(index);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic {0}DTO this[int index]", objName);
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
                writer.WriteLine("\t\t#region ICollection<{0}DTO> Members", objName);
                writer.WriteLine();
                writer.WriteLine("\t\tpublic void Add({0}DTO item)", objName);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tthis._list.Add(item);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic void Clear()");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tthis._list.Clear();");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic bool Contains({0}DTO item)", objName);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\treturn this._list.Contains(item);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic void CopyTo({0}DTO[] array, int arrayIndex)", objName);
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
                writer.WriteLine("\t\tpublic bool Remove({0}DTO item)", objName);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\treturn this._list.Remove(item);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t#endregion");
                writer.WriteLine();
                writer.WriteLine("\t\t#region IEnumerable<{0}DTO> Members", objName);
                writer.WriteLine();
                writer.WriteLine("\t\tpublic IEnumerator<{0}DTO> GetEnumerator()", objName);
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
                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }
    }
}
