using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWCF
{
    public class FactoryBizRule : FactoryBase
    {
        public FactoryBizRule(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");

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
                writer.WriteLine("\tpublic partial class {0}", item.Alias);
                writer.WriteLine("\t{");
                this.WriteCheckDBRule(writer, item);
                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }

        private void WriteCheckDBRule(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tprivate string CheckBizPropertyValue(string name, object objValue)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn string.Empty;");
            writer.WriteLine("\t\t}");
        }
    }
}
