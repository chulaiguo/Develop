using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryTest
{
    public class FactoryModified : FactoryBase
    {
        public FactoryModified(MappingSchema mappingSchema)
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
            writer.WriteLine("namespace {0}.Data", base.ProjectName);
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
                writer.WriteLine("\tpublic partial class {0}Data", obj.Alias);
                writer.WriteLine("\t{");

                writer.WriteLine("\t\tpublic override string ModifiedBy");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget{ return this.LastModifyBy; }");
                writer.WriteLine("\t\t\tset{ this.LastModifyBy = value; }");
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                writer.WriteLine("\t\tpublic override DateTime ModifiedOn");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget{ return this.LastModifyAt; }");
                writer.WriteLine("\t\t\tset{ this.LastModifyAt = value; }");
                writer.WriteLine("\t\t}");

                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }
    }
}
