using System;
using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
    public class FactoryRules_Tooltip : FactoryBase
    {
        public FactoryRules_Tooltip(MappingSchema mappingSchema, ObjectSchema objectSchema)
            : base(mappingSchema, objectSchema)
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
            writer.WriteLine("\tpublic partial class {0}", base.ObjectSchema.Alias);
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tpublic static string GetTooltip(string propertyName)");
            writer.WriteLine("\t\t{");

            writer.WriteLine("\t\t\tconst string tooltip = \"The maximum length is {0} characters\";");
            writer.WriteLine("\t\t\tconst string tooltipRequired = \"Required. The maximum length is {0} characters\";");
            writer.WriteLine("\t\t\t");
            writer.WriteLine("\t\t\tswitch(propertyName)");
            writer.WriteLine("\t\t\t{");
            List<FieldSchema> fieldList = this.GetFieldSchemaList(base.ObjectSchema);
            foreach (FieldSchema field in fieldList)
            {
                
                writer.WriteLine("\t\t\t\tcase {0}Schema.{1}:", base.ObjectSchema.Alias, field.Alias);
                if(field.IsNullable)
                {
                    writer.WriteLine("\t\t\t\t\treturn string.Format(tooltip, {0});", field.Size);
                }
                else
                {
                    writer.WriteLine("\t\t\t\t\treturn string.Format(tooltipRequired, {0});", field.Size);
                }
            }
            writer.WriteLine("\t\t\t\tdefault:");
            writer.WriteLine("\t\t\t\t\treturn string.Empty;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t}");
        }

        private List<FieldSchema> GetFieldSchemaList(ObjectSchema objSchema)
        {
            List<FieldSchema> retList = new List<FieldSchema>();
            foreach (FieldSchema field in objSchema.Fields)
            {
                Type type = base.Utilities.ToDotNetType(field.DataType);
                if (type != typeof(string))
                    continue;

                if (field.Name == "CreatedOn" || field.Name == "CreatedBy" || field.Name == "ModifiedOn" || field.Name == "ModifiedBy")
                    continue;

                retList.Add(field);
            }

            return retList;
        }
    }
}
