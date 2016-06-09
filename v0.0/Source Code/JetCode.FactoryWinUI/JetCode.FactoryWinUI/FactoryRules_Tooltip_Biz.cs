using System;
using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
    public class FactoryRules_Tooltip_Biz : FactoryBase
    {
        public FactoryRules_Tooltip_Biz(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using {0}.Schema;", base.ProjectName);

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
                writer.WriteLine("\tpublic partial class {0}",item.Alias);
                writer.WriteLine("\t{");

                writer.WriteLine("\t\tprivate static string GetBizTooltip(string propertyName)");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\treturn string.Empty;");
                writer.WriteLine("\t\t}");
                writer.WriteLine("\t}");
            }
        }

    }
}
