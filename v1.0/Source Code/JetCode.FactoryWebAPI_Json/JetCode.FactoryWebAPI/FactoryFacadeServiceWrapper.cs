using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Text;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWebAPI
{
    public class FactoryFacadeServiceWrapper : FactoryBase
    {
        public FactoryFacadeServiceWrapper(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Net.Http;");
            writer.WriteLine("using System.Net.Http.Headers;");
            writer.WriteLine("using Newtonsoft.Json;");
            writer.WriteLine("using {0}.DTO;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.WebAPIJsonWrapper", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            SortedList<string, Type> typeList = Utils.GetFacadeServiceTypeList(base.ProjectName);
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
                    if (!this.IsReturnTypeValid(info))
                        continue;

                    writer.WriteLine("\t\tpublic static {0} {1}({2} SecurityTokenDTO token)",
                       this.GetDTOType(info.ReturnType), info.Name, this.GetInputParas(info));
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\treturn {0}({1} token, TimeSpan.FromSeconds(100));", info.Name, this.GetInvokeParas(info));
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();


                    writer.WriteLine("\t\tpublic static {0} {1}({2} SecurityTokenDTO token, TimeSpan timeout)",
                    //writer.WriteLine("\t\tpublic static {0} {1}({2} SecurityTokenDTO token)",
                       this.GetDTOType(info.ReturnType), info.Name, this.GetInputParas(info));
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
                    writer.WriteLine("\t\t\ttoken.AttachParameters(paraNames, paraValues);");
                    writer.WriteLine();

                    writer.WriteLine("\t\t\tHttpClient client = new HttpClient();");
                    writer.WriteLine("\t\t\tclient.Timeout = timeout;");
                    writer.WriteLine("\t\t\tclient.BaseAddress = new Uri(string.Format(\"{0}/JsonFacadeService/\", ConfigurationManager.FacadeServiceBaseAddress.TrimEnd('/')));");
                    writer.WriteLine("\t\t\tclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(\"application/json\"));");
                    writer.WriteLine();
                    writer.WriteLine("\t\t\tHttpContent content = new StringContent(JsonConvert.SerializeObject(token));");
                    writer.WriteLine("\t\t\tHttpResponseMessage res = client.PostAsync(\"Get{0}Result/{1}\", content).Result;", className, info.Name);
                    writer.WriteLine("\t\t\tif (res.StatusCode == System.Net.HttpStatusCode.NotFound)");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tthrow new Exception(\"The remote server returned an error: (404) Not Found\");");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine();
                    writer.WriteLine("\t\t\tif (!res.IsSuccessStatusCode)");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tthrow new Exception(res.Content.ReadAsStringAsync().Result);");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine();
                    writer.WriteLine("\t\t\tstring _data_ = res.Content.ReadAsStringAsync().Result;");
                    if (info.ReturnType.IsValueType)
                    {
                        writer.WriteLine("\t\t\treturn JsonConvert.DeserializeObject<{0}>(_data_);", info.ReturnType.FullName);
                    }
                    else
                    {
                        writer.WriteLine("\t\t\tif(!string.IsNullOrEmpty(_data_))");
                        writer.WriteLine("\t\t\t{");
                        writer.WriteLine("\t\t\t\treturn JsonConvert.DeserializeObject<{0}>(_data_);", this.GetDTOType(info.ReturnType));
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
        }

        private bool IsReturnTypeValid(MethodInfo info)
        {
            if (info.ReturnType.IsValueType || info.ReturnType == typeof(string)
                || info.ReturnType.Name.EndsWith("[]")
                || info.ReturnType.Name == "Result"
                || info.ReturnType.Name.StartsWith("Biz")
                || info.ReturnType.Name.EndsWith("Data")
                || info.ReturnType.Name.EndsWith("View")
                || info.ReturnType.Name.EndsWith("Collection"))
                return true;

            return false;
        }

        private string GetInputParas(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] list = method.GetParameters();
            foreach (ParameterInfo info in list)
            {
                builder.AppendFormat("{0} {1},", this.GetDTOType(info.ParameterType), info.Name);
            }

            return builder.ToString();
        }

        private string GetDTOType(Type type)
        {
            if (type == typeof (StringCollection))
                return "string[]";

            if (type.Name.EndsWith("Collection"))
            {
                return string.Format("{0}DTO[]", type.Name.Substring(0, type.Name.Length - "Collection".Length));
            }

            if (type.Name.EndsWith("Data[]") || type.Name.EndsWith("View[]"))
            {
                return string.Format("{0}DTO[]", type.Name.Substring(0, type.Name.Length - "[]".Length));
            }

            if (type.Name.EndsWith("Data") || type.Name.EndsWith("View") || type.Name.StartsWith("Biz") || type.Name == "Result")
            {
                return string.Format("{0}DTO", type.Name);
            }

            return type.Name;
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
