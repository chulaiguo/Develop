using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWCF
{
    public class FactoryWCFFacadeConfigure : FactoryBase
    {
        public FactoryWCFFacadeConfigure(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
        }

        protected override void BeginWrite(StringWriter writer)
        {
        }

        protected override void EndWrite(StringWriter writer)
        {
        }

        protected override void WriteContent(StringWriter writer)
        {
            string dllName = string.Format("{0}.IFacadeService.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> item in typeList)
            {
                if (item.Key == "IFacadeServiceFactory" || item.Key == "IBizExcelService")
                    continue;

                string objName = item.Key.Substring(1, item.Key.Length - "Service".Length - 1);

                writer.WriteLine("\t<service behaviorConfiguration=\"cheke.behavior\" name=\"{0}.WCFService.{1}WCFService\">", base.ProjectName, objName);
                writer.WriteLine("\t<endpoint address=\"\" binding=\"basicHttpBinding\" bindingConfiguration=\"cheke.binding\"");
                writer.WriteLine("\tcontract=\"{0}.IWCFService.I{1}WCFService\" />", base.ProjectName, objName);
                writer.WriteLine("\t<endpoint address=\"mex\" binding=\"mexHttpBinding\" contract=\"IMetadataExchange\" />");
                writer.WriteLine("\t</service>");
                writer.WriteLine();
            }
        }
    }
}
