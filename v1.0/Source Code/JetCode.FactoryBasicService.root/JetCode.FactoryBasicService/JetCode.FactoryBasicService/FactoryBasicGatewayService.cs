using System;
using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryBasicService
{
    public class FactoryBasicGatewayService : FactoryBase
    {
        public FactoryBasicGatewayService(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using {0}.IBasicService;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.BasicGatewayService", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            writer.WriteLine("\tpublic class BasicGatewayServiceFactory : MarshalByRefObject, IBasicServiceFactory");
            writer.WriteLine("\t{");

            writer.WriteLine("\t\tprivate IBasicServiceFactory GetFactory(string factoryName)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn (IBasicServiceFactory)Cheke.ClassFactory.ClassBuilder.GetFactory(factoryName);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            string dllName = string.Format("{0}.IDataService.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> item in typeList)
            {
                string className = item.Key.Substring(1, item.Key.Length - 12);//IACPanelDataService

                writer.WriteLine("\t\tpublic byte[] Get{0}Result(string actionName, byte[] paras)", className);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\treturn this.GetFactory(\"{0}.BasicServiceFactory\").Get{1}Result(actionName, paras);", this.ProjectName, className);
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

            writer.WriteLine("\t}");
            writer.WriteLine();
        }
    }
}
