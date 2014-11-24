using System;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
    public class FactoryTest : FactoryBase
    {
        public FactoryTest(MappingSchema mappingSchema)
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

                bool lastModifyBy = false;
                bool lastModifyAt = false;
                this.HasModifiedTag(item, ref lastModifyBy, ref lastModifyAt);

                if (lastModifyBy)
                {
                    writer.WriteLine("\t\tpublic string ModifiedBy");
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tget{ return this.LastModifyBy; }");
                    writer.WriteLine("\t\t\tset{ this.LastModifyBy = value; }");
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }

                if (lastModifyAt)
                {
                    writer.WriteLine("\t\tpublic DateTime ModifiedOn");
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tget{ return this.LastModifyAt; }");
                    writer.WriteLine("\t\t\tset{ this.LastModifyAt = value; }");
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }

                writer.WriteLine("\t}");
            }
        }

        private void HasModifiedTag(ObjectSchema objSchema, ref bool lastModifyBy, ref bool lastModifyAt)
        {
            foreach (FieldSchema item in objSchema.Fields)
            {
                Type fieldType = base.Utilities.ToDotNetType(item.DataType);
                if (item.Name == "LastModifyBy" && fieldType == typeof(string))
                {
                    lastModifyBy = true;
                }
                else if (item.Name == "LastModifyAt" && fieldType == typeof(DateTime))
                {
                    lastModifyAt = true;
                }
            }
        }
    }
}
