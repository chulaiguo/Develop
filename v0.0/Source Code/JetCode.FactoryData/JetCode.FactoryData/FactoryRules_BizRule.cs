using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryData
{
    public class FactoryRules_BizRule : FactoryBase
    {
        public FactoryRules_BizRule(MappingSchema mappingSchema, ObjectSchema objectSchema)
            : base(mappingSchema, objectSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Text;");
            writer.WriteLine("using {0}.RulesData;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Rules", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            writer.WriteLine("\tpublic class {0}Rule : {0}DataRule", base.ObjectSchema.Alias);
            writer.WriteLine("\t{");
            writer.WriteLine("\t}");
            writer.WriteLine();
        }
    }
}
