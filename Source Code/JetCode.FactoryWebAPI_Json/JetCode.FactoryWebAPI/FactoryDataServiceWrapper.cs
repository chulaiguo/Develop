using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWebAPI
{
    public class FactoryDataServiceWrapper : FactoryBase
    {
        public FactoryDataServiceWrapper(MappingSchema mappingSchema)
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
            writer.WriteLine("namespace {0}.JsonDataServiceWrapper", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            string dllName = string.Format("{0}.DataService.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> item in typeList)
            {
                if (!item.Key.EndsWith("DataService") || !item.Value.IsPublic)
                    continue;

                string className = item.Key.Substring(0, item.Key.Length - "DataService".Length);//ACPanelDataService
                writer.WriteLine("\tpublic static class Json{0}Wrapper", className);
                writer.WriteLine("\t{");

                writer.WriteLine("\t\tprivate static string _baseAddress = string.Empty;");
                writer.WriteLine("\t\tpublic static string BaseAddress");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget { return _baseAddress; }");
                writer.WriteLine("\t\t\tset { _baseAddress = value; }");
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                SortedList<string, List<MethodInfo>> index = this.GetMethods(item.Value);
                foreach (KeyValuePair<string, List<MethodInfo>> pair in index)
                {
                    foreach (MethodInfo info in pair.Value)
                    {
                        //writer.WriteLine("\t\tpublic static {0} {1}({2} SecurityTokenDTO token)",
                        //  this.GetDTOType(info.ReturnType.Name), info.Name, this.GetInputParas(info));
                        //writer.WriteLine("\t\t{");
                        //writer.WriteLine("\t\t\treturn {0}({1} token, new TimeSpan(0, 0, 0, 100));", info.Name, this.GetInvokeParas(info));
                        //writer.WriteLine("\t\t}");
                        //writer.WriteLine();

                        //writer.WriteLine("\t\tpublic static {0} {1}({2} SecurityTokenDTO token, TimeSpan timeout)",
                        writer.WriteLine("\t\tpublic static {0} {1}({2} SecurityTokenDTO token)",
                            this.GetDTOType(info.ReturnType.Name), info.Name, this.GetInputParas(info));
                        writer.WriteLine("\t\t{");

                        ParameterInfo[] paraList = info.GetParameters();
                        writer.WriteLine("\t\t\tstring[] paraNames = new string[{0}];", paraList.Length);
                        writer.WriteLine("\t\t\tobject[] paraValues = new object[{0}];", paraList.Length);
                        for (int i = 0; i < paraList.Length; i++)
                        {
                            ParameterInfo para = paraList[i];
                            writer.WriteLine("\t\t\tparaNames[{0}] = \"{1}\";", i, para.Name);

                            if (para.ParameterType == typeof (DateTime))
                            {
                                writer.WriteLine("\t\t\tparaValues[{0}] = {1}.ToUniversalTime();", i, para.Name);
                            }
                            else
                            {
                                writer.WriteLine("\t\t\tparaValues[{0}] = {1};", i, para.Name);
                            }
                        }
                        writer.WriteLine();
                        writer.WriteLine("\t\t\ttoken.AttachParameters(paraNames, paraValues);");
                        writer.WriteLine();

                        writer.WriteLine("\t\t\tHttpClient client = new HttpClient();");
                        //writer.WriteLine("\t\t\tclient.Timeout = timeout;");
                        writer.WriteLine("\t\t\tclient.BaseAddress = new Uri(string.Format(\"{0}/DataService/\", BaseAddress.TrimEnd('/')));");
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
                            if (info.ReturnType == typeof(DateTime))
                            {
                                writer.WriteLine("\t\t\treturn new DateTime((JsonConvert.DeserializeObject<DateTime>(_data)).Ticks, DateTimeKind.Utc).ToLocalTime();");
                            }
                            else
                            {
                                writer.WriteLine("\t\t\tJsonConvert.DeserializeObject<{0}>(_data);", info.ReturnType.FullName);
                            }
                        }
                        else
                        {
                            writer.WriteLine("\t\t\tif(_data_ != null && _data_.Length > 0)");
                            writer.WriteLine("\t\t\t{");
                            writer.WriteLine("\t\t\t\tJsonConvert.DeserializeObject<{0}>(_data);", this.GetDTOType(info.ReturnType.Name));
                            writer.WriteLine("\t\t\t}");
                            writer.WriteLine();
                            writer.WriteLine("\t\t\treturn null;");
                        }
                        writer.WriteLine("\t\t}");
                        writer.WriteLine();
                    }
                }

                writer.WriteLine("\t}");
                writer.WriteLine();
            }
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

                if (!this.IsReturnTypeValid(info))
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

                    if (!this.IsReturnTypeValid(info))
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

        private bool IsReturnTypeValid(MethodInfo info)
        {
            if (info.ReturnType.IsValueType || info.ReturnType == typeof(string)
                || info.ReturnType == typeof(byte[])
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
                builder.AppendFormat("{0} {1},", this.GetDTOType(info.ParameterType.Name), info.Name);
            }

            return builder.ToString();
        }

        private string GetDTOType(string name)
        {
            if (name.EndsWith("Collection"))
            {
                return string.Format("{0}DTO[]", name.Substring(0, name.Length - "Collection".Length));
            }

            if (name.EndsWith("Data") || name.EndsWith("View") || name.StartsWith("Biz") || name == "Result")
            {
                return string.Format("{0}DTO", name);
            }

            return name;
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
