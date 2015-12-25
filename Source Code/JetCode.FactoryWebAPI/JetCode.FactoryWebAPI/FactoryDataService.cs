using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWebAPI
{
    public class FactoryDataService : FactoryBase
    {
        public FactoryDataService(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Net;");
            writer.WriteLine("using System.Net.Http;");
            writer.WriteLine("using System.Net.Http.Headers;");
            writer.WriteLine("using System.Web.Http;");
            writer.WriteLine("using {0}.DataService;", base.ProjectName);

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
            writer.WriteLine("\tpublic class DataServiceController : ApiController");
            writer.WriteLine("\t{");

            string dllName = string.Format("{0}.DataService.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> item in typeList)
            {
                if (!item.Key.EndsWith("Service") || !item.Value.IsPublic)
                    continue;

                string className = item.Key.Substring(0, item.Key.Length - "DataService".Length);//ACPanelDataService

                writer.WriteLine("\t\t[HttpPost]");
                writer.WriteLine("\t\tpublic HttpResponseMessage Get{0}Result(string actionName)", className);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\ttry");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tbyte[] token  = this.Request.Content.ReadAsByteArrayAsync().Result;");
                writer.WriteLine();
                writer.WriteLine("\t\t\t\tbyte[] data = this.Get{0}Result(actionName, token);", className);
                writer.WriteLine("\t\t\t\tif(data == null)");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tdata = new byte[0];");
                writer.WriteLine("\t\t\t\t}");
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

                writer.WriteLine("\t\tprivate byte[] Get{0}Result(string actionName, byte[] paras)", className);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tCheke.SecurityToken token = this.DeserializeToken(paras);");
                writer.WriteLine("\t\t\tDataServiceFactory.CheckAuthorize(token);");
                writer.WriteLine("\t\t\t{0}DataService svr = DataServiceFactory.Create{0}DataService(token, false);", className);
                writer.WriteLine("\t\t\tswitch (actionName)");
                writer.WriteLine("\t\t\t{");

                SortedList<string, List<MethodInfo>> index = this.GetMethods(item.Value);
                foreach (KeyValuePair<string, List<MethodInfo>> pair in index)
                {
                    writer.WriteLine("\t\t\t\tcase \"{0}\" :", pair.Key);
                    writer.WriteLine("\t\t\t\t{");
                    if (pair.Value.Count == 1)
                    {
                        MethodInfo method = pair.Value[0];
                        writer.WriteLine("\t\t\t\t\t{0} _result_  = svr.{1}({2});", method.ReturnType.FullName, pair.Key, this.GetInputParas(method));
                        if (method.ReturnType.IsValueType)
                        {
                            if (method.ReturnType == typeof(DateTime))
                            {
                                writer.WriteLine("\t\t\t\t\t_result_ = _result_.ToUniversalTime();");
                            }
                        }
                        else
                        {
                            writer.WriteLine("\t\t\t\t\tif(_result_ == null)");
                            writer.WriteLine("\t\t\t\t\t\treturn null;");
                            writer.WriteLine();
                        }

                        writer.WriteLine("\t\t\t\t\treturn Cheke.Compression.Compress(_result_);");
                    }
                    else
                    {
                        foreach (MethodInfo method in pair.Value)
                        {
                            writer.WriteLine("\t\t\t\t\tif(token.ParameterNames == \"{0}\")", this.GetParaNameList(method));
                            writer.WriteLine("\t\t\t\t\t{");
                            writer.WriteLine("\t\t\t\t\t\t{0} _result_  = svr.{1}({2});", method.ReturnType.FullName, pair.Key, this.GetInputParas(method));
                            if (method.ReturnType.IsValueType)
                            {
                                if (method.ReturnType == typeof(DateTime))
                                {
                                    writer.WriteLine("\t\t\t\t\t\t_result_ = _result_.ToUniversalTime();");
                                }
                            }
                            else
                            {
                                writer.WriteLine("\t\t\t\t\t\tif(_result_ == null)");
                                writer.WriteLine("\t\t\t\t\t\t\treturn null;");
                                writer.WriteLine();
                            }
                            writer.WriteLine("\t\t\t\t\t\treturn Cheke.Compression.Compress(_result_);");
                            writer.WriteLine("\t\t\t\t\t}");
                        }

                        writer.WriteLine("\t\t\t\t\treturn null;");
                    }
                    writer.WriteLine("\t\t\t\t}");
                }

                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\treturn null;");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

            writer.WriteLine();
            writer.WriteLine("\t\tprivate Cheke.SecurityToken DeserializeToken(byte[] data)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tusing (System.IO.MemoryStream stream = new System.IO.MemoryStream(data))");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Deserialize(stream) as Cheke.SecurityToken;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

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

        private SortedList<string, List<MethodInfo>> GetMethods(Type type)
        {
            SortedList<string, List<MethodInfo>> retIndex = new SortedList<string, List<MethodInfo>>();

            //self class
            MethodInfo[] methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            foreach (MethodInfo info in methods)
            {
                if (info.IsVirtual)
                    continue;

                if (!retIndex.ContainsKey(info.Name))
                {
                    retIndex.Add(info.Name, new List<MethodInfo>());
                }

                retIndex[info.Name].Add(info);
            }

            //base class
            Type baseType = type.BaseType;
            if (baseType != null && baseType != typeof(object))
            {
                methods = baseType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
                foreach (MethodInfo info in methods)
                {
                    if (info.Name == "GetRowVersion" || info.Name == "SaveUnderTransaction")
                        continue;

                    if (!retIndex.ContainsKey(info.Name))
                    {
                        retIndex.Add(info.Name, new List<MethodInfo>());
                    }

                    retIndex[info.Name].Add(info);
                }
            }

            return retIndex;
        }

        private string GetParaNameList(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();
            ParameterInfo[] list = method.GetParameters();
            foreach (ParameterInfo info in list)
            {
                builder.AppendFormat("{0}|", info.Name);
            }

            return builder.ToString().TrimEnd('|');
        }

        private string GetInputParas(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] list = method.GetParameters();
            for (int i = 0; i < list.Length; i++)
            {
                ParameterInfo info = list[i];
                if (info.ParameterType == typeof(DateTime))
                {
                    builder.AppendFormat("new DateTime(((DateTime)token.GetParameter({0})).Ticks, DateTimeKind.Utc).ToLocalTime(),", i);
                }
                else
                {
                    builder.AppendFormat("({0})token.GetParameter({1}),", info.ParameterType.FullName, i);
                }
            }

            return builder.ToString().TrimEnd(',');
        }
    }
}
