using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
    public class FactoryDBSchema : FactoryBase
    {
        public FactoryDBSchema(MappingSchema mappingSchema) 
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
            writer.WriteLine("namespace {0}.Schema", this.ProjectName);
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
                writer.WriteLine("\tpublic partial class {0}Schema", obj.Alias);
                writer.WriteLine("\t{");

                writer.WriteLine("\t\tpublic const string TableName = \"{0}\";", obj.Name);
                //foreach (FieldSchema item in obj.Fields)
                //{
                //    writer.WriteLine("\t\tpublic const string {0} = \"{0}\";", item.Alias);
                //}
                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }
    }
}
