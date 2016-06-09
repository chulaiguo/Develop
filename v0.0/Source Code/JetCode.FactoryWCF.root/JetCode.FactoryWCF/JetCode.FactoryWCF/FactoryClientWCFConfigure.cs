using System;
using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWCF
{
    public class FactoryClientWCFConfigure : FactoryBase
    {
        public FactoryClientWCFConfigure(MappingSchema mappingSchema)
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
            writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            writer.WriteLine("<configuration>");
            writer.WriteLine("<system.serviceModel>");
            writer.WriteLine();
            writer.WriteLine("\t<bindings>");
            writer.WriteLine("\t<basicHttpBinding>");
            writer.WriteLine(
                "\t\t<binding name=\"cheke.binding\" maxBufferSize=\"2147483647\" maxReceivedMessageSize=\"2147483647\">");
            writer.WriteLine("\t\t<security mode=\"None\" />");
            writer.WriteLine("\t\t</binding>");
            writer.WriteLine("\t</basicHttpBinding>");
            writer.WriteLine("\t</bindings>");
            writer.WriteLine();
            writer.WriteLine("\t<client>");
            writer.WriteLine();

            //Basic
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                writer.WriteLine("\t<endpoint name=\"{0}.WCFService.{1}WCFService\"", base.ProjectName, item.Alias);
                writer.WriteLine("\t\t\t\t\taddress=\"http://localhost:50741/{0}.svc\"", item.Alias);
                writer.WriteLine("\t\t\t\t\tbinding=\"basicHttpBinding\" bindingConfiguration=\"cheke.binding\"");
                writer.WriteLine("\t\t\t\t\tcontract=\"{0}.IWCFService.I{1}WCFService\"/>", base.ProjectName, item.Alias);
                writer.WriteLine();
            }

            //Facade
            string dllName = string.Format("{0}.IFacadeService.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> pair in typeList)
            {
                if (pair.Key == "IFacadeServiceFactory" || pair.Key == "IBizExcelService")
                    continue;

                string bizName = pair.Key.Substring(1, pair.Key.Length - "Service".Length - 1);
                writer.WriteLine("\t<endpoint name=\"{0}.WCFService.{1}WCFService\"", base.ProjectName, bizName);
                writer.WriteLine("\t\t\t\t\taddress=\"http://localhost:50741/{0}.svc\"", bizName);
                writer.WriteLine("\t\t\t\t\tbinding=\"basicHttpBinding\" bindingConfiguration=\"cheke.binding\"");
                writer.WriteLine("\t\t\t\t\tcontract=\"{0}.IWCFService.I{1}WCFService\"/>", base.ProjectName, bizName);
                writer.WriteLine();
            }  
            
            writer.WriteLine("\t</client>");
            writer.WriteLine("</system.serviceModel>");
            writer.WriteLine("");
            writer.WriteLine("</configuration>");
        }
    }
}
