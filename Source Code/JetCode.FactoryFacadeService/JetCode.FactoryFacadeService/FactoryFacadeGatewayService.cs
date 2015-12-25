using System;
using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryFacadeService
{
    public class FactoryFacadeGatewayService : FactoryBase
    {
        public FactoryFacadeGatewayService(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using {0}.IFacadeService;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.FacadeGatewayService", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            writer.WriteLine("\tpublic class FacadeGatewayServiceFactory : MarshalByRefObject, IFacadeServiceFactory");
            writer.WriteLine("\t{");

            writer.WriteLine("\t\tprivate IFacadeServiceFactory GetFactory(string factoryName)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn (IFacadeServiceFactory)Cheke.ClassFactory.ClassBuilder.GetFactory(factoryName);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            string dllName = string.Format("{0}.FacadeService.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> item in typeList)
            {
                if (!item.Key.StartsWith("Biz"))
                    continue;

                string className = item.Key.Substring(0, item.Key.Length - 7);//BizCreatorService

                writer.WriteLine("\t\tpublic byte[] Get{0}Result(string actionName, byte[] paras)", className);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\treturn this.GetFactory(\"{0}.FacadeServiceFactory\").Get{1}Result(actionName, paras);", this.ProjectName, className);
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

            writer.WriteLine("\t}");
            writer.WriteLine();
        }
    }
}
