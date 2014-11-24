using System;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
    public class FactoryIRulesData : FactoryBase
    {
        public FactoryIRulesData(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.IRulesData", base.ProjectName);
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
                writer.WriteLine("\tpublic interface I{0}Data", item.Alias);
                writer.WriteLine("\t{");

                foreach (FieldSchema field in item.Fields)
                {
                    Type type = base.Utilities.ToDotNetType(field.DataType);
                    writer.WriteLine("\t\t{0} {1} {{ get; set; }}", type.FullName, field.Alias);
                }
                writer.WriteLine();
         
                writer.WriteLine("\t}");
            }
        }
    }
}
