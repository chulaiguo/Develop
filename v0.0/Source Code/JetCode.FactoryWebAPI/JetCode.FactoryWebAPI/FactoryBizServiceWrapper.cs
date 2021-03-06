using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWebAPI
{
    public class FactoryBizServiceWrapper : FactoryBase
    {
        public FactoryBizServiceWrapper(MappingSchema mappingSchema)
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
            writer.WriteLine("namespace {0}.{1}ServiceWrapper", base.ProjectName, Utils._ServiceName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            string dllName = string.Format("{0}.{1}Service.dll", base.ProjectName, Utils._ServiceName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> item in typeList)
            {
                if (!item.Key.EndsWith("Service") || !item.Value.IsPublic)
                    continue;

                string className = item.Key.Substring(0, item.Key.Length - "Service".Length);//BizLoginService
                writer.WriteLine("\tpublic static class {0}Wrapper", className);
                writer.WriteLine("\t{");

                MethodInfo[] list = item.Value.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
                foreach (MethodInfo info in list)
                {
                    writer.WriteLine("\t\tpublic static {0} {1}({2} SecurityToken token)",
                        info.ReturnType.FullName, info.Name, this.GetInputParas(info));
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\treturn {0}({1} token, TimeSpan.FromSeconds(100));", info.Name, this.GetInvokeParas(info));
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();


                    writer.WriteLine("\t\tpublic static {0} {1}({2} SecurityToken token, TimeSpan timeout)",
                    //writer.WriteLine("\t\tpublic static {0} {1}({2} SecurityToken token)",
                       info.ReturnType.FullName, info.Name, this.GetInputParas(info));
                    writer.WriteLine("\t\t{");

                    ParameterInfo[] paraList = info.GetParameters();
                    writer.WriteLine("\t\t\tstring[] paraNames = new string[{0}];", paraList.Length);
                    writer.WriteLine("\t\t\tobject[] paraValues = new object[{0}];", paraList.Length);
                    for (int i = 0; i < paraList.Length; i++)
                    {
                        ParameterInfo para = paraList[i];
                        writer.WriteLine("\t\t\tparaNames[{0}] = \"{1}\";", i, para.Name);
                        writer.WriteLine("\t\t\tparaValues[{0}] = {1};", i, para.Name);
                    }
                    writer.WriteLine();
                    writer.WriteLine("\t\t\tSecurityToken _token_ = SecurityToken.CreateFrameworkToken(token, paraNames, paraValues);");
                    writer.WriteLine();

                    writer.WriteLine("\t\t\tstring baseAddress = System.Configuration.ConfigurationManager.AppSettings[\"{0}_{1}Service:BaseAddress\"];", this.ProjectName, Utils._ServiceName);
                    writer.WriteLine("\t\t\tHttpClient client = new HttpClient();");
                    writer.WriteLine("\t\t\tclient.Timeout = timeout;");
                    writer.WriteLine("\t\t\tclient.BaseAddress = new Uri(string.Format(\"{{0}}/{0}Service/\", baseAddress.TrimEnd('/')));", Utils._ServiceName);
                    writer.WriteLine("\t\t\tclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(\"image/jpg\"));");
                    writer.WriteLine();
                    writer.WriteLine("\t\t\tHttpContent content = new ByteArrayContent({0}ServiceHelper.Serialize(_token_));", Utils._ServiceName);
                    writer.WriteLine("\t\t\tHttpResponseMessage res = client.PostAsync(\"Get{0}Result/{1}\", content).Result;", className, info.Name);
                    writer.WriteLine("\t\t\tif (res.StatusCode == System.Net.HttpStatusCode.NotFound)");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tthrow new Exception(\"The remote server returned an error: (404) Not Found\");");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine();
                    writer.WriteLine("\t\t\tif (!res.IsSuccessStatusCode)");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tthrow {0}ServiceHelper.DeserializeException(res.Content.ReadAsByteArrayAsync().Result);", Utils._ServiceName);
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine();
                    writer.WriteLine("\t\t\tbyte[] _data_ = res.Content.ReadAsByteArrayAsync().Result;");
                    if (info.ReturnType.IsValueType)
                    {
                         writer.WriteLine("\t\t\treturn ({0})Compression.DecompressToObject(_data_);", info.ReturnType.FullName);
                    }
                    else
                    {
                        writer.WriteLine("\t\t\tif(_data_ != null && _data_.Length > 0)");
                        writer.WriteLine("\t\t\t{");
                        if (info.ReturnType == typeof(byte[]))
                        {
                            writer.WriteLine("\t\t\t\treturn Compression.DecompressToByteArray(_data_) as {0};", info.ReturnType.FullName);
                        }
                        else
                        {
                            writer.WriteLine("\t\t\t\treturn Compression.DecompressToObject(_data_) as {0};", info.ReturnType.FullName);
                        }
                        writer.WriteLine("\t\t\t}");
                        writer.WriteLine();
                        writer.WriteLine("\t\t\treturn null;");
                    }
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }

                writer.WriteLine("\t}");
                writer.WriteLine();
            }

            this.WriteBizServiceBuilder(writer);
        }

        private void WriteBizServiceBuilder(StringWriter writer)
        {
            writer.WriteLine("\tinternal static class {0}ServiceHelper", Utils._ServiceName);
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tinternal static byte[] Serialize(object obj)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tusing (System.IO.MemoryStream stream = new System.IO.MemoryStream())");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tnew System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Serialize(stream, obj);");
            writer.WriteLine("\t\t\t\treturn stream.ToArray();");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tinternal static Exception DeserializeException(byte[] data)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tException result;");
            writer.WriteLine("\t\t\tusing (System.IO.MemoryStream stream = new System.IO.MemoryStream(data))");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tresult = (new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Deserialize(stream) as Exception);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\treturn result;");
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

        private string GetInvokeParas(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] list = method.GetParameters();
            foreach (ParameterInfo info in list)
            {
                builder.AppendFormat("{0},", info.Name);
            }

            return builder.ToString();
        }
    }
}
