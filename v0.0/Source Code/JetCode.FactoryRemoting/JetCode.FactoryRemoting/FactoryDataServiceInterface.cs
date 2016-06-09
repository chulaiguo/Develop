using System;
using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryRemoting
{
    public class FactoryDataServiceInterface : FactoryBase
    {
        public FactoryDataServiceInterface(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.IRemotingService", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic interface IDataServiceFactory");
            writer.WriteLine("\t{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            string dllName = string.Format("{0}.DataService.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> item in typeList)
            {
                if (!item.Key.EndsWith("DataService") || !item.Value.IsPublic)
                    continue;

                string className = item.Key.Substring(0, item.Key.Length - "DataService".Length);//ACPanelDataService

                writer.WriteLine("\t\tbyte[] Get{0}Result(string actionName, byte[] paras);", className);
            }
        }
    }
}
