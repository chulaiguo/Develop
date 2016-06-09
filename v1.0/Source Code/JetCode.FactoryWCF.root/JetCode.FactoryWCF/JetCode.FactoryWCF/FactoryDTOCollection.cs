using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWCF
{
    public class FactoryDTOCollection : FactoryBase
    {
        public FactoryDTOCollection(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Runtime.Serialization;");
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
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                writer.WriteLine("\t[DataContract]");
                writer.WriteLine("\tpublic class {0}DTOCollection : DTOCollectionBase", item.Alias);
                writer.WriteLine("\t{");
                writer.WriteLine("\t\tprivate List<{0}DTO> _list = new List<{0}DTO>();", item.Alias);
                writer.WriteLine();
                writer.WriteLine("\t\t[DataMember]");
                writer.WriteLine("\t\tpublic List<{0}DTO> InnerList", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget { return this._list; }");
                writer.WriteLine("\t\t\tset { this._list = value; }");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic void Add({0}DTO item)", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tthis.InnerList.Add(item);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic void Insert(int index, {0}DTO item)", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tthis.InnerList.Insert(index, item);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic void Remove({0}DTO item)", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tthis.InnerList.Remove(item);");
                writer.WriteLine("\t\t}");
                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }
    }
}
