using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWCF
{
    public class FactoryDTO : FactoryBase
    {
        public FactoryDTO(MappingSchema mappingSchema)
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
            string dllName = string.Format("{0}.Data.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);

            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                writer.WriteLine("\t[DataContract]");
                writer.WriteLine("\tpublic class {0}DTO : DTOBase", item.Alias);
                writer.WriteLine("\t{");

                string typeKey = string.Format("{0}Data", item.Name);
                if(typeList.ContainsKey(typeKey))
                {
                    List<PropertyInfo> list = this.GetPropertyList(typeList[typeKey]);

                    writer.WriteLine("\t\t#region Fields");
                    this.WriteFields(writer, list);
                    this.WriteChildrenFields(writer, item);
                    writer.WriteLine("\t\t#endregion");
                    writer.WriteLine();

                    writer.WriteLine("\t\t#region Properties");
                    this.WriteProperties(writer, list);
                    this.WriteChildrenProperties(writer, item);
                    writer.WriteLine("\t\t#endregion");
                }
                else
                {
                    writer.WriteLine("\t\t#region Fields");
                    this.WriteFields(writer, item);
                    this.WriteChildrenFields(writer, item);
                    writer.WriteLine("\t\t#endregion");
                    writer.WriteLine();

                    writer.WriteLine("\t\t#region Properties");
                    this.WriteProperties(writer, item);
                    this.WriteChildrenProperties(writer, item);
                    writer.WriteLine("\t\t#endregion");
                }

                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }


        private void WriteFields(StringWriter writer, ObjectSchema obj)
        {
            foreach (FieldSchema item in obj.Fields)
            {
                writer.WriteLine("\t\tprivate {0} _{1};", Utilities.ToDotNetType(item.DataType).Name, base.LowerFirstLetter(item.Alias));
            }
            writer.WriteLine();
        }

        private void WriteProperties(StringWriter writer, ObjectSchema obj)
        {
            foreach (FieldSchema item in obj.Fields)
            {
                writer.WriteLine("\t\t[DataMember]");
                writer.WriteLine("\t\tpublic {0} {1}", Utilities.ToDotNetType(item.DataType).Name, item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget {{ return this._{0}; }}", base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\tset {{ this._{0} = value; }}", base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

            writer.WriteLine();
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

        private void WriteChildrenFields(StringWriter writer, ObjectSchema obj)
        {
            foreach (ChildSchema item in obj.Children)
            {
                writer.WriteLine("\t\tprivate {0}DTOCollection _{1}List;", item.Alias, base.LowerFirstLetter(item.Alias));
            }
            
            writer.WriteLine();
        }

        private void WriteChildrenProperties(StringWriter writer, ObjectSchema obj)
        {
            foreach (ChildSchema item in obj.Children)
            {
                writer.WriteLine("\t\t[DataMember]");
                writer.WriteLine("\t\tpublic {0}DTOCollection {0}List", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget {{ return this._{0}List; }}", base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\tset {{ this._{0}List = value; }}", base.LowerFirstLetter(item.Alias));
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
