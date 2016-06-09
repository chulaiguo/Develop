using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;
using System;

namespace JetCode.FactoryTest
{
    public class FactoryGetByModifiedOnInterface : FactoryBase
    {
        public FactoryGetByModifiedOnInterface(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using {0}.Data;", base.ProjectName);
            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.IDataService", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            foreach (ObjectSchema obj in base.MappingSchema.Objects)
            {
                WriteGetByModifiedOnInterface(writer, obj.Name);
            }
        }

        private void WriteGetByModifiedOnInterface(StringWriter writer, string tableName)
        {
            writer.WriteLine("\tpublic partial interface I{0}DataService", tableName);
            writer.WriteLine("\t{");
            writer.WriteLine("\t\t{0}DataCollection GetByModifiedOn(DateTime begin, DateTime end);", tableName);
            writer.WriteLine("\t}");
            writer.WriteLine();
        }
    }
}
