using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryData
{
    public class FactoryRules : FactoryBase
    {
        public FactoryRules(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Text;");

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
            this.WriteRulesData(writer);
            this.WriteRulesBase(writer);
            this.WriteRules(writer);
            this.WriteBizRules(writer);
        }

        private void WriteRulesData(StringWriter writer)
        {
            string dllName = string.Format("{0}.Data.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);

            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                string typeKey = string.Format("{0}Data", item.Name);
                if (!typeList.ContainsKey(typeKey))
                    continue;

                writer.WriteLine("\t[Serializable]");
                writer.WriteLine("\tpublic class {0}RuleData", item.Alias);
                writer.WriteLine("\t{");

                List<PropertyInfo> list = this.GetPropertyList(typeList[typeKey]);
                foreach (PropertyInfo field in list)
                {
                    writer.WriteLine("\t\tprivate {0} _{1};", field.PropertyType.FullName, base.LowerFirstLetter(field.Name));
                }
                writer.WriteLine();

                foreach (PropertyInfo field in list)
                {
                    writer.WriteLine("\t\tpublic {0} {1}", field.PropertyType.FullName, field.Name);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tget {{ return this._{0}; }}", base.LowerFirstLetter(field.Name));
                    writer.WriteLine("\t\t\tset {{ this._{0} = value; }}", base.LowerFirstLetter(field.Name));
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }

                writer.WriteLine("\t}");
            }

            writer.WriteLine();
        }

        private List<PropertyInfo> GetPropertyList(Type type)
        {
            List<PropertyInfo> retList = new List<PropertyInfo>();

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
            foreach (PropertyInfo info in properties)
            {
                if (info.Name == "IsDirty" || info.Name == "IsValid" || info.Name == "PKString"
                        || info.Name == "MarkAsDeleted" || info.Name == "TableName")
                    continue;

                if (!info.CanWrite || !info.CanRead)
                    continue;

                if (info.PropertyType.IsValueType || info.PropertyType == typeof(string))
                {

                    retList.Add(info);
                }
            }

            return retList;
        }

        private void WriteRulesBase(StringWriter writer)
        {
            writer.WriteLine("\tpublic partial class RuleBase");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tprotected virtual string CreateRule(string key, string rule)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn string.Format(\"[{0}]: {1};\", key, rule);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected virtual string BrokenNotEmptyRule(string key, string value)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif(string.IsNullOrEmpty(value))");
            writer.WriteLine("\t\t\t\treturn this.CreateRule(key, \"It can't be empty\");");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn string.Empty;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected virtual string BrokenNotEmptyRule(string key, Guid value)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif(value == Guid.Empty)");
            writer.WriteLine("\t\t\t\treturn this.CreateRule(key, \"It can't be empty\");");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn string.Empty;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected virtual string BrokenMaxLengthRule(string key, int maxLength, string value)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif(value.Length > maxLength)");
            writer.WriteLine("\t\t\t\treturn this.CreateRule(key, string.Format(\"It was too long, the maximum length is {0} characters\", maxLength));");
            writer.WriteLine("");
            writer.WriteLine("\t\t\treturn string.Empty;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected virtual string BrokenLengthEqualRule(string key, int length, string value)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif(value.Length > 0 && value.Length != length)");
            writer.WriteLine("\t\t\t\treturn this.CreateRule(key, string.Format(\"The length is not equal to {0}\", length));");
            writer.WriteLine("");
            writer.WriteLine("\t\t\treturn string.Empty;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected virtual string BrokenGreaterThanOrEqualToRule(string key, int value, int target)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn value < target ? this.CreateRule(key, String.Format(\"It can't be less than {0}.\", target)) : string.Empty;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected virtual string BrokenGreaterThanOrEqualToRule(string key, decimal value, int target)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn value < target ? this.CreateRule(key, String.Format(\"It can't be less than {0}.\", target)) : string.Empty;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected virtual string BrokenGreaterThanRule(string key, int value, int target)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn value <= target ? this.CreateRule(key, String.Format(\"It can't be less than or equal to {0}.\", target)) : string.Empty;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected virtual string BrokenGreaterThanRule(string key, decimal value, int target)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn value <= target ? this.CreateRule(key, String.Format(\"It can't be less than or equal to {0}.\", target)) : string.Empty;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected virtual string BrokenLessThanOrEqualToRule(string key, decimal value, int target)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn value > target ? this.CreateRule(key, String.Format(\"It can't be greater than {0}.\", target)) : string.Empty;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected virtual string BrokenLessThanOrEqualToRule(string key, int value, int target)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn value > target ? this.CreateRule(key, String.Format(\"It can't be greater than {0}.\", target)) : string.Empty;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected virtual string BrokenLessThanRule(string key, int value, int target)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn value >= target ? this.CreateRule(key, String.Format(\"It can't be greater than or equal to {0}.\", target)) : string.Empty;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected virtual string BrokenLessThanRule(string key, decimal value, int target)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn value >= target ? this.CreateRule(key, String.Format(\"It can't be greater than or equal to {0}.\", target)) : string.Empty;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected virtual string BrokenBeginTimeBeforeEndTimeRule(string key, DateTime begin, DateTime end)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn begin > end ? this.CreateRule(key, \"The beginning date must be before the end date.\") : string.Empty;");
            writer.WriteLine("\t\t}");

            writer.WriteLine("\t\tprotected virtual string BrokenGreaterThanOrEqualToRule(string key, string value, int target)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tstring rule = this.BrokenDigitsOnlyRule(key, value);");
            writer.WriteLine("\t\t\tif (rule.Length > 0)");
            writer.WriteLine("\t\t\t\treturn rule;");
            writer.WriteLine("");
            writer.WriteLine("\t\t\treturn int.Parse(value) < target ? this.CreateRule(key, String.Format(\"It can't be less than {0}.\", target)) : string.Empty;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

          

            writer.WriteLine("\t\tprotected virtual string BrokenGreaterThanRule(string key, string value, int target)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tstring rule = this.BrokenDigitsOnlyRule(key, value);");
            writer.WriteLine("\t\t\tif (rule.Length > 0)");
            writer.WriteLine("\t\t\t\treturn rule;");
            writer.WriteLine("");
            writer.WriteLine("\t\t\treturn int.Parse(value) <= target ? this.CreateRule(key, String.Format(\"It can't be less than or equal to {0}.\", target)) : string.Empty;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected virtual string BrokenLessThanOrEqualToRule(string key, string value, int target)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tstring rule = this.BrokenDigitsOnlyRule(key, value);");
            writer.WriteLine("\t\t\tif (rule.Length > 0)");
            writer.WriteLine("\t\t\t\treturn rule;");
            writer.WriteLine("");
            writer.WriteLine("\t\t\treturn int.Parse(value) > target ? this.CreateRule(key, String.Format(\"It can't be greater than {0}.\", target)) : string.Empty;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected virtual string BrokenLessThanRule(string key, string value, int target)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tstring rule = this.BrokenDigitsOnlyRule(key, value);");
            writer.WriteLine("\t\t\tif (rule.Length > 0)");
            writer.WriteLine("\t\t\t\treturn rule;");
            writer.WriteLine("");
            writer.WriteLine("\t\t\treturn int.Parse(value) >= target ? this.CreateRule(key, String.Format(\"It can't be greater than or equal to {0}.\", target)) : string.Empty;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected virtual string BrokenBeginTimeBeforeEndTimeRule(string key, string begin, string end)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tint beginTime = this.TimeToMinute(begin);");
            writer.WriteLine("\t\t\tint endTime = this.TimeToMinute(end);");
            writer.WriteLine("");
            writer.WriteLine("\t\t\tif (beginTime == -1 || endTime == -1)");
            writer.WriteLine("\t\t\t\treturn this.CreateRule(key, \"The begin time and end time must be between 0000 and 2359\");");
            writer.WriteLine("");
            writer.WriteLine("\t\t\treturn beginTime > endTime ? this.CreateRule(key, \"The begin time must be before the end time.\") : string.Empty;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected virtual string BrokenValidTimeRule(string key, string time)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn this.TimeToMinute(time) == -1 ? this.CreateRule(key, \"It must be between 0000 and 2359.\") : string.Empty;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected virtual string BrokenDigitsOnlyRule(string key, string value)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn this.IsDigits(value) ? this.CreateRule(key, \"It can only contain digits.\") : string.Empty;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\t#region Helper Function");
            writer.WriteLine();
            writer.WriteLine("\t\tprotected int TimeToMinute(string time)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (time.Length != 4)");
            writer.WriteLine("\t\t\t\treturn -1;");
            writer.WriteLine();
            writer.WriteLine("\t\t\tint hour;");
            writer.WriteLine("\t\t\tif (!int.TryParse(time.Substring(0, 2), out hour))");
            writer.WriteLine("\t\t\t\treturn -1;");
            writer.WriteLine();
            writer.WriteLine("\t\t\tif (hour < 0 || hour > 24)");
            writer.WriteLine("\t\t\t\treturn -1;");
            writer.WriteLine();
            writer.WriteLine("\t\t\tint minute;");
            writer.WriteLine("\t\t\tif (!int.TryParse(time.Substring(2), out minute))");
            writer.WriteLine("\t\t\t\treturn -1;");
            writer.WriteLine();
            writer.WriteLine("\t\t\tif (minute < 0 || minute > 59)");
            writer.WriteLine("\t\t\t\treturn -1;");
            writer.WriteLine();
            writer.WriteLine("\t\t\tif (hour == 24 && minute != 0)");
            writer.WriteLine("\t\t\t\treturn -1;");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn hour * 60 + minute;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tprotected bool IsDigits(string value)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tfor (int i = 0; i < value.Length; i++)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif (!char.IsDigit(value[i]))");
            writer.WriteLine("\t\t\t\t\treturn false;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn true;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t#endregion");

            writer.WriteLine("\t}");
            writer.WriteLine();
        }

        private void WriteRules(StringWriter writer)
        {
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                List<FieldSchema> fieldList = this.GetFieldSchemaList(item);

                writer.WriteLine("\tpublic class {0}DataRule : RuleBase", item.Alias);
                writer.WriteLine("\t{");

                //CheckRules
                writer.WriteLine("\t\tpublic string CheckRules({0}RuleData data)", item.Alias);
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
                writer.WriteLine("\t\tprotected virtual string CheckComprehensiveRules({0}RuleData data)", item.Alias);
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
                        writer.WriteLine("\t\t\tstring rule;");
                        if (this.IsRequiredField(item, field))
                        {
                            writer.WriteLine("\t\t\trule = this.BrokenNotEmptyRule(this.{0}, value);", field.Alias);
                            writer.WriteLine("\t\t\tif(rule.Length > 0)");
                            writer.WriteLine("\t\t\t\treturn rule;");
                            writer.WriteLine();
                        }

                        if (field.DataType.ToLower() == "char" || field.DataType.ToLower() == "nchar")
                        {
                            writer.WriteLine("\t\t\trule = this.BrokenLengthEqualRule(this.{0}, {1}, value);", field.Alias, field.Size);
                            writer.WriteLine("\t\t\tif(rule.Length > 0)");
                            writer.WriteLine("\t\t\t\treturn rule;");
                            writer.WriteLine();
                        }
                        if (field.DataType.ToLower() == "text" || field.DataType.ToLower() == "ntext")
                        {

                        }
                        else
                        {
                            writer.WriteLine("\t\t\trule = this.BrokenMaxLengthRule(this.{0}, {1}, value);", field.Alias, field.Size);
                            writer.WriteLine("\t\t\tif(rule.Length > 0)");
                            writer.WriteLine("\t\t\t\treturn rule;");
                            writer.WriteLine();
                        }

                    }
                    else if (type == typeof(Guid))
                    {
                        writer.WriteLine("\t\t\tstring rule;");
                        if (!field.IsNullable)
                        {
                            writer.WriteLine("\t\t\trule = this.BrokenNotEmptyRule(this.{0}, value);", field.Alias);
                            writer.WriteLine("\t\t\tif(rule.Length > 0)");
                            writer.WriteLine("\t\t\t\treturn rule;");
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
            writer.WriteLine();
        }

        private bool IsRequiredField(ObjectSchema objSchema, FieldSchema field)
        {
            if(field.IsNullable)
                return false;

            foreach (IndexSchema item in objSchema.Indexs)
            {
                if (!item.IsUniqueConstraint)
                    continue;

                if (item.Keys.Count <= 1)
                    continue;

                foreach (string key in item.Keys)
                {
                    if (key == field.Alias)
                        return false;
                }
            }

            return true;
        }

        private List<FieldSchema> GetFieldSchemaList(ObjectSchema objSchema)
        {
            List<FieldSchema> retList = new List<FieldSchema>();
            foreach (FieldSchema field in objSchema.Fields)
            {
                Type type = base.Utilities.ToDotNetType(field.DataType);
                if(type == typeof(byte[]))
                    continue;

                if(field.IsJoined)
                    continue;

                if (field.Name == "CreatedOn" || field.Name == "CreatedBy" || field.Name == "ModifiedOn" || field.Name == "ModifiedBy")
                    continue;

                retList.Add(field);
            }

            return retList;
        }

        private void WriteBizRules(StringWriter writer)
        {
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                writer.WriteLine("\tpublic partial class {0}Rule : {0}DataRule", item.Alias);
                writer.WriteLine("\t{");
                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }
    }
}
