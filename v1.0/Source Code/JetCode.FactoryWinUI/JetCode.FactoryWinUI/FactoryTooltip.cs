using System;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
    public class FactoryTooltip : FactoryBase
    {
        public FactoryTooltip(MappingSchema mappingSchema)
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
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                writer.WriteLine("\tpublic partial class {0}Data", item.Alias);
                writer.WriteLine("\t{");

                foreach (FieldSchema field in item.Fields)
                {
                    if (field.IsJoined)
                        continue;

                    if (field.Name == "CreatedBy" || field.Name == "ModifiedBy")
                        continue;

                    if (field.DataType.ToLower() == "varchar" || field.DataType.ToLower() == "nvarchar"
                        || field.DataType.ToLower() == "text" || field.DataType.ToLower() == "ntext")
                    {

                        if (!field.IsNullable)
                        {
                            writer.WriteLine(
                                "\t\tpublic const string {0}_DBRule = \"The max length of {{0}} is {1}, and it can't be empty.\";",
                                field.Alias, field.Size);
                        }
                        else
                        {
                            writer.WriteLine(
                                "\t\tpublic const string {0}_DBRule = \"The max length of {{0}} is {1}.\";", field.Alias,
                                field.Size);
                        }

                        continue;
                    }

                    if (field.DataType.ToLower() == "char" || field.DataType.ToLower() == "nchar")
                    {
                        if (!field.IsNullable)
                        {
                            writer.WriteLine(
                                "\t\tpublic const string {0}_DBRule = \"The length of {{0}} is equal to {1}, and it can't be empty.\";",
                                field.Alias, field.Size);
                        }
                        else
                        {
                            writer.WriteLine(
                                "\t\tpublic const string {0}_DBRule = \"The length of {{0}} is equal to {1}.\";",
                                field.Alias, field.Size);
                        }

                        continue;
                    }
                }

                writer.WriteLine("\t}");
            }
        }
    }
}
