using System;
using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryService
{
    public class FactoryServiceInterface : FactoryBase
    {
        public FactoryServiceInterface(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.I{1}Service", base.ProjectName, Utils._ServiceName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic interface I{0}ServiceFactory", Utils._ServiceName);
            writer.WriteLine("\t{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            string dllName = string.Format("{0}.{1}Service.dll", base.ProjectName, Utils._ServiceName);
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
