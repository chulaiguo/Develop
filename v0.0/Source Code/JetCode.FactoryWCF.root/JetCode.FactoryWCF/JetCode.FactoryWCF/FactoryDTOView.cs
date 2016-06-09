using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWCF
{
    public class FactoryDTOView : FactoryBase
    {
        public FactoryDTOView(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Runtime.Serialization;");

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.DTO", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            SortedList<string, ObjectSchema> tableList = new SortedList<string, ObjectSchema>();
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                string key = string.Format("{0}Data", item.Name);
                if(tableList.ContainsKey(key))
                    continue;

                tableList.Add(key, item);
            }

            string dllName = string.Format("{0}.Data.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> item in typeList)
            {
                if(tableList.ContainsKey(item.Key))
                    continue;

                if(item.Key.EndsWith("Collection"))
                    continue;

                writer.WriteLine("\t[DataContract]");
                writer.WriteLine("\tpublic class {0}DTO", item.Key);
                writer.WriteLine("\t{");

                List<PropertyInfo> list = this.GetPropertyList(item.Value);
                writer.WriteLine("\t\t#region Fields");
                this.WriteFields(writer, list);
                writer.WriteLine("\t\t#endregion");
                writer.WriteLine();
                writer.WriteLine("\t\t#region Properties");
                this.WriteProperties(writer, list);
                writer.WriteLine("\t\t#endregion");
                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }

        private void WriteFields(StringWriter writer, List<PropertyInfo> list)
        {
            foreach (PropertyInfo item in list)
            {
                writer.WriteLine("\t\tprivate {0} _{1};", item.PropertyType.Name, base.LowerFirstLetter(item.Name));
            }
            writer.WriteLine();
        }

        private void WriteProperties(StringWriter writer, List<PropertyInfo> list)
        {
            foreach (PropertyInfo item in list)
            {
                writer.WriteLine("\t\t[DataMember]");
                writer.WriteLine("\t\tpublic {0} {1}", item.PropertyType.Name, item.Name);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget {{ return this._{0}; }}", base.LowerFirstLetter(item.Name));
                writer.WriteLine("\t\t\tset {{ this._{0} = value; }}", base.LowerFirstLetter(item.Name));
                writer.WriteLine("\t\t}");
                writer.WriteLine();
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

                if (info.PropertyType.IsValueType || info.PropertyType == typeof(string)
                    || info.PropertyType == typeof(byte[]))
                {

                    retList.Add(info);
                }
            }

            return retList;
        }
    }
}
