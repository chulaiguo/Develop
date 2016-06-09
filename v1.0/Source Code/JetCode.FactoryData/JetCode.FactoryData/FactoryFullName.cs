using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryData
{
    public class FactoryFullName : FactoryBase
    {
        public FactoryFullName(MappingSchema mappingSchema)
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
            SortedList<string, Type> typeList = Utils.GetDataTypeList(base.ProjectName);

            foreach (KeyValuePair<string, Type> pair in typeList)
            {
                SortedList<string, PropertyInfo> index = this.GetPropertyList(pair.Value);
                if(!index.ContainsKey("FirstName"))
                    continue;

                if(!index.ContainsKey("LastName"))
                    continue;

                writer.WriteLine("\tpublic partial class {0}", pair.Key);
                writer.WriteLine("\t{");
                writer.WriteLine("\t\tpublic string FullName");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tif (String.IsNullOrEmpty(this.FirstName))");
                writer.WriteLine("\t\t\t\t\treturn this.LastName;");
                writer.WriteLine();
                writer.WriteLine("\t\t\t\treturn string.Format(\"{0}, {1}\", this.LastName, this.FirstName);");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine("\t}");
            }

            writer.WriteLine();
        }

        private SortedList<string, PropertyInfo> GetPropertyList(Type type)
        {
            SortedList<string, PropertyInfo> retList = new SortedList<string, PropertyInfo>();

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
            foreach (PropertyInfo info in properties)
            {
                if (!info.CanWrite || !info.CanRead)
                    continue;

                if (info.PropertyType != typeof(string))
                    continue;

                if(retList.ContainsKey(info.Name))
                    continue;

                retList.Add(info.Name, info);
            }

            return retList;
        }
    }
}
