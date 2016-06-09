using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWCF
{
    public class FactoryDBRule : FactoryBase
    {
        public FactoryDBRule(MappingSchema mappingSchema)
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
            writer.WriteLine("\t\tprivate string CheckDBPropertyValue(string name, object objValue)");
            writer.WriteLine("\t\t{");

            FieldSchemaCollection list = this.GetCheckFields(item);
            if(list.Count == 0)
            {
                writer.WriteLine("\t\t\treturn string.Empty;");
                writer.WriteLine("\t\t}");
                return;
            }

            writer.WriteLine("\t\t\tswitch (name)");
            writer.WriteLine("\t\t\t{");
            foreach (FieldSchema field in list)
            {
                if (field.DataType.ToLower() == "varchar" || field.DataType.ToLower() == "nvarchar")
                {
                    writer.WriteLine("\t\t\t\tcase \"{0}\":", field.Alias);
                    writer.WriteLine("\t\t\t\t\t{");
                    writer.WriteLine("\t\t\t\t\t\tstring value = (string)objValue;");
                    if (!field.IsNullable)
                    {
                        writer.WriteLine("\t\t\t\t\t\tif (value.Trim().Length == 0)");
                        writer.WriteLine("\t\t\t\t\t\t\treturn \"[{0}] can't be empty;\";", field.Alias);
                        writer.WriteLine();
                    }
                    writer.WriteLine("\t\t\t\t\t\tif (value.Trim().Length > {0})", field.Size);
                    writer.WriteLine("\t\t\t\t\t\t\treturn \"The length of [{0}] can't be greater than {1};\";", field.Alias, field.Size);
                    writer.WriteLine();
                    writer.WriteLine("\t\t\t\t\t\treturn string.Empty;");
                    writer.WriteLine("\t\t\t\t\t}");
                    continue;
                }

                if (field.DataType.ToLower() == "char" || field.DataType.ToLower() == "nchar")
                {
                    writer.WriteLine("\t\t\t\tcase \"{0}\":", field.Alias);
                    writer.WriteLine("\t\t\t\t\t{");
                    writer.WriteLine("\t\t\t\t\t\tstring value = (string)objValue;");
                    if (!field.IsNullable)
                    {
                        writer.WriteLine("\t\t\t\t\t\tif (value.Trim().Length != {0})", field.Size);
                        writer.WriteLine("\t\t\t\t\t\t\treturn \"The length of [{0}] must be equal to {1}.\";", field.Alias, field.Size);
                        writer.WriteLine();
                    }
                    writer.WriteLine("\t\t\t\t\t\tif (value.Trim().Length > 0 && value.Trim().Length != {0})", field.Size);
                    writer.WriteLine("\t\t\t\t\t\t\treturn \"The length of [{0}] must be equal to {1};\";", field.Alias, field.Size);
                    writer.WriteLine();
                    writer.WriteLine("\t\t\t\t\t\treturn string.Empty;");
                    writer.WriteLine("\t\t\t\t\t}");
                    continue;
                }
            }
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn string.Empty;");
            writer.WriteLine("\t\t}");
        }

        private FieldSchemaCollection GetCheckFields(ObjectSchema item)
        {
            FieldSchemaCollection retList = new FieldSchemaCollection();
            foreach (FieldSchema field in item.Fields)
            {
                if (field.IsJoined)
                    continue;

                if (field.Alias == "CreatedBy" || field.Alias == "ModifiedBy")
                    continue;

                if (field.DataType.ToLower() == "varchar" || field.DataType.ToLower() == "nvarchar")
                {
                    retList.Add(field);
                }

                if (field.DataType.ToLower() == "char" || field.DataType.ToLower() == "nchar")
                {
                    retList.Add(field);
                }
            }

            return retList;
        }
    }
}
