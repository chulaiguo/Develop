using System;
using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryService
{
    public class FactoryGatewayService : FactoryBase
    {
        public FactoryGatewayService(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using {0}.I{1}Service;", base.ProjectName, Utils._ServiceName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.{1}GatewayService", base.ProjectName, Utils._ServiceName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            writer.WriteLine("\tpublic class {0}GatewayServiceFactory : MarshalByRefObject, I{0}ServiceFactory", Utils._ServiceName);
            writer.WriteLine("\t{");

            writer.WriteLine("\t\tprivate I{0}ServiceFactory GetFactory(string factoryName)", Utils._ServiceName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn (I{0}ServiceFactory)Cheke.ClassFactory.ClassBuilder.GetFactory(factoryName);", Utils._ServiceName);
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            string dllName = string.Format("{0}.{1}Service.dll", base.ProjectName, Utils._ServiceName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> item in typeList)
            {
                if (!item.Key.StartsWith("Biz"))
                    continue;

                string className = item.Key.Substring(0, item.Key.Length - 7);//BizCreatorService

                writer.WriteLine("\t\tpublic byte[] Get{0}Result(string actionName, byte[] paras)", className);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\treturn this.GetFactory(\"{0}.{2}ServiceFactory\").Get{1}Result(actionName, paras);", this.ProjectName, className, Utils._ServiceName);
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

            writer.WriteLine("\t}");
            writer.WriteLine();
        }
    }
}
