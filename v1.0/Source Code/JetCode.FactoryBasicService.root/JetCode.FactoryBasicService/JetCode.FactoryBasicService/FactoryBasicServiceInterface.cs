using System;
using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryBasicService
{
    public class FactoryBasicServiceInterface : FactoryBase
    {
        public FactoryBasicServiceInterface(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.IBasicService", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic interface IBasicServiceFactory");
            writer.WriteLine("\t{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            string dllName = string.Format("{0}.IDataService.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> item in typeList)
            {
                string className = item.Key.Substring(1, item.Key.Length - 12);//IACPanelDataService

                writer.WriteLine("\t\tbyte[] Get{0}Result(string actionName, byte[] paras);", className);
            }
        }
    }
}
