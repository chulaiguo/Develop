using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryData
{
    public class FactoryLogicDelete : FactoryBase
    {
        public FactoryLogicDelete(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
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
                FieldSchema field = this.GetLogicDeleteField(item);
                if(field == null)
                    continue;

                writer.WriteLine("\tpublic partial class {0}Data", item.Alias);
                writer.WriteLine("\t{");

                writer.WriteLine("\t\tpublic override bool IsActive");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\treturn !this.{0};", field.Alias);
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                writer.WriteLine("\t\tpublic override bool IsLogicDeleted");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tif(!this.IsDeleted)");
                writer.WriteLine("\t\t\t\t\treturn false;");
                writer.WriteLine();
                writer.WriteLine("\t\t\t\treturn !this.{0};", field.Alias);
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                writer.WriteLine("\t\tpublic override void ToLogicDeleted()");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tthis.AcceptSelfChanges();");
                writer.WriteLine("\t\t\tthis.{0} = true;", field.Alias);
                writer.WriteLine("\t\t}");
                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }
    }
}
