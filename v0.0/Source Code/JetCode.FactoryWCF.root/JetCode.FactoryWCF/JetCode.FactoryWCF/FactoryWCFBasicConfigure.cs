using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWCF
{
    public class FactoryWCFBasicConfigure : FactoryBase
    {
        public FactoryWCFBasicConfigure(MappingSchema mappingSchema)
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
            writer.WriteLine("<system.serviceModel>");
            writer.WriteLine("<behaviors>");
            writer.WriteLine("\t<serviceBehaviors>");
            writer.WriteLine("\t<behavior name=\"cheke.behavior\">");
            writer.WriteLine("\t\t<serviceMetadata httpGetEnabled=\"true\" />");
            writer.WriteLine("\t\t<serviceDebug includeExceptionDetailInFaults=\"true\" />");
            writer.WriteLine("\t</behavior>");
            writer.WriteLine("\t</serviceBehaviors>");
            writer.WriteLine("</behaviors>");
            writer.WriteLine();
            writer.WriteLine("<bindings>");
            writer.WriteLine("\t<basicHttpBinding>");
            writer.WriteLine("\t<binding name=\"cheke.binding\" maxBufferSize=\"2147483647\" maxReceivedMessageSize=\"2147483647\">");
            writer.WriteLine("\t\t<security mode=\"None\" />");
            writer.WriteLine("\t</binding>");
            writer.WriteLine("\t</basicHttpBinding>");
            writer.WriteLine("</bindings>");
            writer.WriteLine();
            writer.WriteLine("<serviceHostingEnvironment aspNetCompatibilityEnabled=\"true\" />");
            writer.WriteLine();
            writer.WriteLine("<services>");

            writer.WriteLine();
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                writer.WriteLine("\t<service behaviorConfiguration=\"cheke.behavior\" name=\"{0}.WCFService.{1}WCFService\">", base.ProjectName, item.Alias);
                writer.WriteLine("\t<endpoint address=\"\" binding=\"basicHttpBinding\" bindingConfiguration=\"cheke.binding\"");
                writer.WriteLine("\tcontract=\"{0}.IWCFService.I{1}WCFService\" />", base.ProjectName, item.Alias);
                writer.WriteLine("\t<endpoint address=\"mex\" binding=\"mexHttpBinding\" contract=\"IMetadataExchange\" />");
                writer.WriteLine("\t</service>");
                writer.WriteLine();
            }

            writer.WriteLine("</services>");
            writer.WriteLine("</system.serviceModel>");
        }
    }
}
