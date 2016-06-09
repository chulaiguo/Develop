using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWCF
{
    public class FactoryDTOBiz : FactoryBase
    {
        public FactoryDTOBiz(MappingSchema mappingSchema)
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
            string dllName = string.Format("{0}.BizData.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> pair in typeList)
            {
                if(pair.Key.EndsWith("Collection"))
                    continue;

                writer.WriteLine("\t[DataContract]");
                writer.WriteLine("\tpublic class {0}DTO", pair.Key);
                writer.WriteLine("\t{");

                PropertyInfo[] properties = pair.Value.GetProperties();

                //Field
                foreach (PropertyInfo info in properties)
                {
                    writer.WriteLine("\t\tprivate {0} _{1};", info.PropertyType.Name, base.LowerFirstLetter(info.Name));
                }

                //Property
                writer.WriteLine();
                foreach (PropertyInfo info in properties)
                {
                    writer.WriteLine("\t\t[DataMember]");
                    writer.WriteLine("\t\tpublic {0} {1}", info.PropertyType.Name, info.Name);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tget {{ return this._{0}; }}", base.LowerFirstLetter(info.Name));
                    writer.WriteLine("\t\t\tset {{ this._{0} = value; }}", base.LowerFirstLetter(info.Name));
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }

                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }
    }
}
