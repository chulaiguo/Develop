using System;
using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
    public class FactoryDataRules : FactoryBase
    {
        public FactoryDataRules(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Text;");
            writer.WriteLine("using {0}.IRulesData;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Rules", base.ProjectName);
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
                List<FieldSchema> fieldList = this.GetFieldSchemaList(item);

                writer.WriteLine("\tpublic class {0}DataRule : {1}RuleBase", item.Alias, base.ProjectName);
                writer.WriteLine("\t{");

                //CheckRules
                writer.WriteLine("\t\tpublic string CheckRules(I{0}Data data)", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tStringBuilder builder = new StringBuilder();");
                writer.WriteLine();
                writer.WriteLine("\t\t\tstring rule;");
                foreach (FieldSchema field in fieldList)
                {
                    writer.WriteLine("\t\t\trule = this.Check{0}(data.{0});", field.Alias);
                    writer.WriteLine("\t\t\tif (rule.Length > 0)");
                    writer.WriteLine("\t\t\t\tbuilder.AppendLine(rule);");
                    writer.WriteLine();
                }
                writer.WriteLine();
                writer.WriteLine("\t\t\trule = this.CheckComprehensiveRules(data);");
                writer.WriteLine("\t\t\tif (rule.Length > 0)");
                writer.WriteLine("\t\t\t\tbuilder.AppendLine(rule);");
                writer.WriteLine();
                writer.WriteLine("\t\t\treturn builder.ToString();");
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                //CheckComprehensiveRules
                writer.WriteLine("\t\tprotected virtual string CheckComprehensiveRules(I{0}Data data)", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\treturn string.Empty;");
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                //Data Rule
                writer.WriteLine("\t\t#region Property Rules");
                foreach (FieldSchema field in fieldList)
                {
                    Type type = base.Utilities.ToDotNetType(field.DataType);
                    writer.WriteLine("\t\tpublic virtual string Check{0}({1} value)", field.Alias, type.FullName);
                    writer.WriteLine("\t\t{");
                    if (type == typeof(string))
                    {
                        if (!field.IsNullable)
                        {
                            writer.WriteLine("\t\t\tif (string.IsNullOrEmpty(value))");
                            writer.WriteLine("\t\t\t\treturn this.BrokenNotEmptyRule(this.{0});", field.Alias);
                            writer.WriteLine();
                        }

                        if (field.DataType.ToLower() == "char" || field.DataType.ToLower() == "nchar")
                        {
                            writer.WriteLine("\t\t\tif (value.Length > 0 && value.Length != {0})", field.Size);
                            writer.WriteLine("\t\t\t\treturn this.BrokenLengthRule(this.{0}, {1});", field.Alias, field.Size);
                            writer.WriteLine();
                        }
                        else
                        {
                            writer.WriteLine("\t\t\tif (value.Length > {0})", field.Size);
                            writer.WriteLine("\t\t\t\treturn this.BrokenMaxLengthRule(this.{0}, {1});", field.Alias, field.Size);
                            writer.WriteLine();
                        }
                       
                    }
                    else if(type == typeof(Guid))
                    {
                        if (!field.IsNullable)
                        {
                            writer.WriteLine("\t\t\tif (value == Guid.Empty)");
                            writer.WriteLine("\t\t\t\treturn this.BrokenNotEmptyRule(this.{0});", field.Alias);
                            writer.WriteLine();
                        }
                    }
                    writer.WriteLine("\t\t\treturn string.Empty;");
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }
                writer.WriteLine("\t\t#endregion");
                writer.WriteLine();

                //Rule Keys
                writer.WriteLine("\t\t#region Property Names");
                foreach (FieldSchema field in fieldList)
                {
                    writer.WriteLine("\t\tprotected virtual string {0}", field.Alias);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tget {{ return \"{0}\"; }}", field.Alias);
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }
                writer.WriteLine("\t\t#endregion");
         
                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }

        private List<FieldSchema> GetFieldSchemaList(ObjectSchema objSchema)
        {
            List<FieldSchema> retList = new List<FieldSchema>();
            foreach (FieldSchema field in objSchema.Fields)
            {
                Type type = base.Utilities.ToDotNetType(field.DataType);
                if(type == typeof(byte[]) || type == typeof(bool))
                    continue;

                if (field.Name == "CreatedOn" || field.Name == "CreatedBy" || field.Name == "ModifiedOn" || field.Name == "ModifiedBy")
                    continue;

                retList.Add(field);
            }

            return retList;
        }
    }
}
