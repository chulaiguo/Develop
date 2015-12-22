using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryFacadeService
{
    public class FactoryFacadeServiceWrapper_REST : FactoryBase
    {
        public FactoryFacadeServiceWrapper_REST(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Net.Http;");
            writer.WriteLine("using System.Net.Http.Headers;");
            writer.WriteLine("using Cheke;");
            writer.WriteLine("using {0}.Data;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.FacadeServiceWrapper", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
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
                writer.WriteLine("\tpublic static class {0}Wrapper", className);
                writer.WriteLine("\t{");

                MethodInfo[] list = item.Value.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
                foreach (MethodInfo info in list)
                {
                    writer.WriteLine("\t\tpublic static {0} {1}({2} SecurityToken token)",
                       info.ReturnType.FullName, info.Name, this.GetInputParas(info));
                    writer.WriteLine("\t\t{");
                   
                    ParameterInfo[] paraList = info.GetParameters();
                    writer.WriteLine("\t\t\tstring[] paraNames = new string[{0}];", paraList.Length);
                    writer.WriteLine("\t\t\tobject[] paraValues = new object[{0}];", paraList.Length);
                    for (int i = 0; i < paraList.Length; i++)
                    {
                        ParameterInfo para = paraList[i];
                        writer.WriteLine("\t\t\tparaNames[{0}] = \"{1}\";", i, para.Name);

                        if (para.ParameterType == typeof(DateTime))
                        {
                            writer.WriteLine("\t\t\tparaValues[{0}] = {1}.ToUniversalTime();", i, para.Name);
                        }
                        else
                        {
                            writer.WriteLine("\t\t\tparaValues[{0}] = {1};", i, para.Name);
                        }
                    }
                    writer.WriteLine();
                    writer.WriteLine("\t\t\tSecurityToken _token_ = SecurityToken.CreateFrameworkToken(token, paraNames, paraValues);");
                    writer.WriteLine();

                    writer.WriteLine("\t\t\tstring baseAddress = System.Configuration.ConfigurationManager.AppSettings[\"{0}_FacadeService:BaseAddress\"];", this.ProjectName);
                    writer.WriteLine("\t\t\tHttpClient client = new HttpClient();");
                    writer.WriteLine("\t\t\tclient.BaseAddress = new Uri(string.Format(\"{0}/FacadeService/\", baseAddress.TrimEnd('/')));");
                    writer.WriteLine("\t\t\tclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(\"image/jpg\"));");
                    writer.WriteLine();
                    writer.WriteLine("\t\t\tHttpContent content = new ByteArrayContent(FacadeServiceHelper.Serialize(_token_));");
                    writer.WriteLine("\t\t\tHttpResponseMessage res = client.PostAsync(\"Get{0}Result/{1}\", content).Result;", className, info.Name);
                    writer.WriteLine("\t\t\tif (!res.IsSuccessStatusCode)");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tthrow new ApplicationException(string.Format(\"The server is currently busy, please try again late.(StatusCode ={0})\", res.StatusCode));");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine();
                    writer.WriteLine("\t\t\tbyte[] _data_ = res.Content.ReadAsByteArrayAsync().Result;");
                    if (info.ReturnType.IsValueType)
                    {
                        writer.WriteLine("\t\t\t{0} _result_ = ({0})Compression.DecompressToObject(_data_);", info.ReturnType.FullName);
                        if (info.ReturnType == typeof(DateTime))
                        {
                            writer.WriteLine("\t\t\t_result_ = new DateTime(_result_.Ticks, DateTimeKind.Utc).ToLocalTime();");
                        }
                    }
                    else
                    {
                        writer.WriteLine("\t\t\t{0} _result_ = null;", info.ReturnType.FullName);
                        writer.WriteLine("\t\t\tif(_data_ != null)");
                        writer.WriteLine("\t\t\t{");
                        if (info.ReturnType == typeof(byte[]))
                        {
                            writer.WriteLine("\t\t\t\t_result_ =  Compression.DecompressToByteArray(_data_) as {0};", info.ReturnType.FullName);
                        }
                        else
                        {
                            writer.WriteLine("\t\t\t\t_result_ =  Compression.DecompressToObject(_data_) as {0};", info.ReturnType.FullName);
                        }
                        writer.WriteLine("\t\t\t}");
                    }
                    writer.WriteLine("\t\t\treturn _result_;");
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }

                writer.WriteLine("\t}");
                writer.WriteLine();
            }

            this.WriteBasicServiceBuilder(writer);
        }

        private void WriteBasicServiceBuilder(StringWriter writer)
        {
            writer.WriteLine("\tinternal static class FacadeServiceHelper");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tinternal static byte[] Serialize(object obj)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tusing (System.IO.MemoryStream stream = new System.IO.MemoryStream())");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tnew System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Serialize(stream, obj);");
            writer.WriteLine("\t\t\t\treturn stream.ToArray();");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t}");
        }

        private string GetInputParas(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] list = method.GetParameters();
            foreach (ParameterInfo info in list)
            {
                builder.AppendFormat("{0} {1},", info.ParameterType.FullName, info.Name);
            }

            return builder.ToString();
        }
    }
}
