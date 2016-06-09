using System;
using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryBasicService
{
    public class FactoryBasicGatewayService_REST : FactoryBase
    {
        public FactoryBasicGatewayService_REST(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System.Net;");
            writer.WriteLine("using System.Net.Http;");
            writer.WriteLine("using System.Net.Http.Headers;");
            writer.WriteLine("using System.Web.Http;");
            writer.WriteLine("using {0}.BasicService;", base.ProjectName);

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
            writer.WriteLine("\tpublic class BasicServiceController : ApiController");
            writer.WriteLine("\t{");

            string dllName = string.Format("{0}.IDataService.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> item in typeList)
            {
                string className = item.Key.Substring(1, item.Key.Length - 12);//IACPanelDataService

                writer.WriteLine("\t\t[HttpPost]");
                writer.WriteLine("\t\tpublic HttpResponseMessage Get{0}Result(string actionName)", className);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\ttry");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tbyte[] token  = this.Request.Content.ReadAsByteArrayAsync().Result;");
                writer.WriteLine();
                writer.WriteLine("\t\t\t\tBasicServiceFactory factory = new BasicServiceFactory();");
                writer.WriteLine("\t\t\t\tbyte[] data = factory.Get{0}Result(actionName, token);", className);
                writer.WriteLine();
                writer.WriteLine("\t\t\t\tHttpResponseMessage res = new HttpResponseMessage(HttpStatusCode.OK);");
                writer.WriteLine("\t\t\t\tres.Content = new ByteArrayContent(data);");
                writer.WriteLine("\t\t\t\tres.Content.Headers.ContentType = new MediaTypeHeaderValue(\"image/jpg\");");
                writer.WriteLine("\t\t\t\treturn res;");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\tcatch (System.Exception ex)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\treturn this.CreateExceptionResponse(ex);");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

            writer.WriteLine("\t\tprivate HttpResponseMessage CreateExceptionResponse(System.Exception ex)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbyte[] data;");
            writer.WriteLine("\t\t\tusing (System.IO.MemoryStream stream = new System.IO.MemoryStream())");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tnew System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Serialize(stream, ex);");
            writer.WriteLine("\t\t\t\tdata = stream.ToArray();");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tHttpResponseMessage res = new HttpResponseMessage(HttpStatusCode.InternalServerError);");
            writer.WriteLine("\t\t\tres.Content = new ByteArrayContent(data);");
            writer.WriteLine("\t\t\tres.Content.Headers.ContentType = new MediaTypeHeaderValue(\"image/jpg\");");
            writer.WriteLine("\t\t\treturn res;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t}");
            writer.WriteLine();
        }
    }
}
