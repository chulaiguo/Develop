using System;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
    public class FactoryDataWithIRule : FactoryBase
    {
        public FactoryDataWithIRule(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using {0}.Rules;", base.ProjectName);
            writer.WriteLine("using {0}.IRulesData;", base.ProjectName);

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
                writer.WriteLine("\tpublic partial class {0}Data : I{0}Data", item.Alias);
                writer.WriteLine("\t{");
                writer.WriteLine("\t\tprotected override void CheckRules()");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\t{0}Rule rule = new {0}Rule();", item.Alias);
                writer.WriteLine("\t\t\tstring error = rule.CheckRules(this);");
                writer.WriteLine("\t\t\tif(error.Length > 0)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tbase.BrokenRules.Add(new Rule(string.Empty, error));");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }
    }
}
