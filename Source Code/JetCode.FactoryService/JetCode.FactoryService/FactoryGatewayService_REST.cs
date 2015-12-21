using System;
using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryService
{
    public class FactoryGatewayService_REST : FactoryBase
    {
        public FactoryGatewayService_REST(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System.Net;");
            writer.WriteLine("using System.Net.Http;");
            writer.WriteLine("using System.Net.Http.Headers;");
            writer.WriteLine("using System.Web.Http;");
            writer.WriteLine("using {0}.{1}Service;", base.ProjectName, Utils._ServiceName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.WebAPI.Controllers", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            writer.WriteLine("\tpublic class {0}ServiceController : ApiController", Utils._ServiceName);
            writer.WriteLine("\t{");

            string dllName = string.Format("{0}.{1}Service.dll", base.ProjectName, Utils._ServiceName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> item in typeList)
            {
                if (!item.Key.StartsWith("Biz"))
                    continue;

                string className = item.Key.Substring(0, item.Key.Length - 7);//BizCreatorService

                writer.WriteLine("\t\t[HttpPost]");
                writer.WriteLine("\t\tpublic HttpResponseMessage Get{0}Result(string actionName)", className);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tbyte[] token  = this.Request.Content.ReadAsByteArrayAsync().Result;");
                writer.WriteLine();
                writer.WriteLine("\t\t\tFacadeServiceFactory factory = new FacadeServiceFactory();");
                writer.WriteLine("\t\t\tbyte[] data = factory.Get{0}Result(actionName, token);", className);
                writer.WriteLine();
                writer.WriteLine("\t\t\tHttpResponseMessage res = new HttpResponseMessage(HttpStatusCode.OK);");
                writer.WriteLine("\t\t\tres.Content = new ByteArrayContent(data);");
                writer.WriteLine("\t\t\tres.Content.Headers.ContentType = new MediaTypeHeaderValue(\"image/jpg\");");
                writer.WriteLine("\t\t\treturn res;");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

            writer.WriteLine("\t}");
            writer.WriteLine();
        }
    }
}
