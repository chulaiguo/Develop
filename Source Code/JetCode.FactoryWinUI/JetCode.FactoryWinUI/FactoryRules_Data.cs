using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
    public class FactoryRules_Data : FactoryBase
    {
        public FactoryRules_Data(MappingSchema mappingSchema)
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
            writer.WriteLine("namespace {0}.RulesData", base.ProjectName);
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
