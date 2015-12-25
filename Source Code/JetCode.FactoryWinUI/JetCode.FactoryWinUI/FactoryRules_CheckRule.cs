using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
    public class FactoryRules_CheckRule : FactoryBase
    {
        public FactoryRules_CheckRule(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using {0}.Rules;", base.ProjectName);
            writer.WriteLine("using {0}.RulesData;", base.ProjectName);

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
            string dllName = string.Format("{0}.Data.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);

            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                string typeKey = string.Format("{0}Data", item.Name);
                if (!typeList.ContainsKey(typeKey))
                    continue;

                List<PropertyInfo> list = this.GetPropertyList(typeList[typeKey]);

                writer.WriteLine("\tpublic partial class {0}Data", item.Alias);
                writer.WriteLine("\t{");
                writer.WriteLine("\t\tprotected override void CheckRules()");
                writer.WriteLine("\t\t{");

                writer.WriteLine("\t\t\t{0}RuleData data = new {0}RuleData();", item.Alias);
                foreach (PropertyInfo field in list)
                {
                    writer.WriteLine("\t\t\tdata.{0} = this._{1};", field.Name, base.LowerFirstLetter(field.Name));
                }
                writer.WriteLine();

                writer.WriteLine("\t\t\t{0}Rule rule = new {0}Rule();", item.Alias);
                writer.WriteLine("\t\t\tstring error = rule.CheckRules(data);");
                writer.WriteLine("\t\t\tif(error.Length > 0)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tbase.BrokenRules.Add(new Rule(\"BrokenRules\", error));");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine("\t}");
                writer.WriteLine();
            }
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
    }
}
