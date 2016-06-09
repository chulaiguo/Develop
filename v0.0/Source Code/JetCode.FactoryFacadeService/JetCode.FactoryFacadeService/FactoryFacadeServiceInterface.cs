using System;
using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryFacadeService
{
    public class FactoryFacadeServiceInterface : FactoryBase
    {
        public FactoryFacadeServiceInterface(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.IFacadeService", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic interface IFacadeServiceFactory");
            writer.WriteLine("\t{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            string dllName = string.Format("{0}.FacadeService.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> item in typeList)
            {
                if (!item.Key.StartsWith("Biz"))
                    continue;

                string className = item.Key.Substring(0, item.Key.Length - 7);//BizCreatorService

                writer.WriteLine("\t\tbyte[] Get{0}Result(string actionName, byte[] paras);", className);
            }
        }
    }
}
