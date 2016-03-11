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
    public class FactoryBizService : FactoryBase
    {
        public FactoryBizService(MappingSchema mappingSchema)
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
            writer.WriteLine("using Newtonsoft.Json;");
            writer.WriteLine("using {0}.DTO;", base.ProjectName);
            writer.WriteLine("using {0}.DataService;", base.ProjectName);
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
            writer.WriteLine("\tpublic class Json{0}ServiceController : ApiController", Utils._ServiceName);
            writer.WriteLine("\t{");

            string dllName = string.Format("{0}.{1}Service.dll", base.ProjectName, Utils._ServiceName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> item in typeList)
            {
                if (!item.Key.EndsWith("Service") || !item.Value.IsPublic)
                    continue;

                string className = item.Key.Substring(0, item.Key.Length - "Service".Length);//BizLoginService

                writer.WriteLine("\t\t[HttpPost]");
                writer.WriteLine("\t\tpublic HttpResponseMessage Get{0}Result(string actionName)", className);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\ttry");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tstring json  = this.Request.Content.ReadAsStringAsync().Result;");
                writer.WriteLine();
                writer.WriteLine("\t\t\t\tstring data = this.Get{0}Result(actionName, json);", className);
                writer.WriteLine("\t\t\t\tHttpResponseMessage res = new HttpResponseMessage(HttpStatusCode.OK);");
                writer.WriteLine("\t\t\t\tres.Content = new StringContent(data);");
                writer.WriteLine("\t\t\t\tres.Content.Headers.ContentType = new MediaTypeHeaderValue(\"application/json\");");
                writer.WriteLine("\t\t\t\treturn res;");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\tcatch (System.Exception ex)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\treturn this.CreateExceptionResponse(ex);");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                writer.WriteLine("\t\tprivate string Get{0}Result(string actionName, string json)", className);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tSecurityTokenDTO dto = JsonConvert.DeserializeObject<SecurityTokenDTO>(json);");
                writer.WriteLine("\t\t\tCheke.SecurityToken token = dto.Deserialize();");
                writer.WriteLine("\t\t\tUsrAccountWrapper.CheckAuthorize(token);");
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
                        writer.WriteLine("\t\t\t\t\t{0} _result_  = {1}Wrapper.{2}({3});", method.ReturnType.FullName, className, pair.Key, this.GetInputParas(method));
                        if (method.ReturnType.IsValueType)
                        {
                            if (method.ReturnType == typeof(DateTime))
                            {
                                writer.WriteLine("\t\t\t\t\t_result_ = _result_.ToUniversalTime();");
                            }

                            writer.WriteLine("\t\t\t\t\treturn JsonConvert.SerializeObject(_result_);");
                        }
                        else
                        {
                            writer.WriteLine("\t\t\t\t\tif(_result_ == null)");
                            writer.WriteLine("\t\t\t\t\t\treturn string.Empty;");
                            writer.WriteLine();

                            if (method.ReturnType.Name == "Result"
                                || (method.ReturnType.Name.StartsWith("Biz") && !method.ReturnType.Name.EndsWith("Collection"))
                                || method.ReturnType.Name.EndsWith("Data")
                                || method.ReturnType.Name.EndsWith("View"))
                            {
                                writer.WriteLine("\t\t\t\t\t{0}DTO _result_dto_ = {0}DTO.Serialize(_result_);",  method.ReturnType.Name);
                                writer.WriteLine("\t\t\t\t\treturn JsonConvert.SerializeObject(_result_dto_);");
                            }
                            else if (method.ReturnType.Name.EndsWith("Collection"))
                            {
                                if (method.ReturnType == typeof(StringCollection))
                                {
                                    writer.WriteLine("\t\t\t\t\t\tstring[] _result_dto_ = new string[_result_.Count];");
                                    writer.WriteLine("\t\t\t\t\t\t_result_.CopyTo(_result_dto_, 0);");
                                    writer.WriteLine("\t\t\t\t\t\treturn JsonConvert.SerializeObject(_result_dto_);");
                                }
                                else
                                {
                                    string dtoType = method.ReturnType.Name.Substring(0, method.ReturnType.Name.Length - "Collection".Length);
                                    writer.WriteLine("\t\t\t\t\t\t{0}DTO[] _result_dto_ = {0}DTO.Serialize(_result_);", dtoType);
                                    writer.WriteLine("\t\t\t\t\t\treturn JsonConvert.SerializeObject(_result_dto_);");
                                }
                            }
                            else
                            {
                                writer.WriteLine("\t\t\t\t\treturn JsonConvert.SerializeObject(_result_);");
                            }
                        }
                    }
                    else
                    {
                        foreach (MethodInfo method in pair.Value)
                        {
                            writer.WriteLine("\t\t\t\t\tif(dto.ParameterNames == \"{0}\")", this.GetParaNameList(method));
                            writer.WriteLine("\t\t\t\t\t{");
                            writer.WriteLine("\t\t\t\t\t\t{0} _result_  = {1}Wrapper.{2}({3});",
                                method.ReturnType.FullName, className, pair.Key, this.GetInputParas(method));
                            if (method.ReturnType.IsValueType)
                            {
                                if (method.ReturnType == typeof (DateTime))
                                {
                                    writer.WriteLine("\t\t\t\t\t\t_result_ = _result_.ToUniversalTime();");
                                }

                                writer.WriteLine("\t\t\t\t\t\treturn JsonConvert.SerializeObject(_result_);");
                            }
                            else
                            {
                                writer.WriteLine("\t\t\t\t\t\tif(_result_ == null)");
                                writer.WriteLine("\t\t\t\t\t\t\treturn string.Empty;");
                                writer.WriteLine();

                                if (method.ReturnType.Name == "Result"
                                    || (method.ReturnType.Name.StartsWith("Biz") && !method.ReturnType.Name.EndsWith("Collection"))
                                    || method.ReturnType.Name.EndsWith("Data")
                                    || method.ReturnType.Name.EndsWith("View"))
                                {
                                    writer.WriteLine("\t\t\t\t\t\t{0}DTO _result_dto_ = {0}DTO.Serialize(_result_);", method.ReturnType.Name);
                                    writer.WriteLine("\t\t\t\t\t\treturn JsonConvert.SerializeObject(_result_dto_);");
                                }
                                else if (method.ReturnType.Name.EndsWith("Collection"))
                                {
                                    if (method.ReturnType == typeof(StringCollection))
                                    {
                                        writer.WriteLine("\t\t\t\t\t\tstring[] _result_dto_ = new string[_result_.Count];");
                                        writer.WriteLine("\t\t\t\t\t\t_result_.CopyTo(_result_dto_, 0);");
                                        writer.WriteLine("\t\t\t\t\t\treturn JsonConvert.SerializeObject(_result_dto_);");
                                    }
                                    else
                                    {
                                        string dtoType = method.ReturnType.Name.Substring(0, method.ReturnType.Name.Length - "Collection".Length);
                                        writer.WriteLine("\t\t\t\t\t\t{0}DTO[] _result_dto_ = {0}DTO.Serialize(_result_);", dtoType);
                                        writer.WriteLine("\t\t\t\t\t\treturn JsonConvert.SerializeObject(_result_dto_);");
                                    }
                                }
                                else
                                {
                                    writer.WriteLine("\t\t\t\t\t\treturn JsonConvert.SerializeObject(_result_);");
                                }
                                writer.WriteLine("\t\t\t\t\t}");
                            }
                        }

                        writer.WriteLine("\t\t\t\t\treturn string.Empty;");
                    }
                    writer.WriteLine("\t\t\t\t}");
                }

                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\treturn string.Empty;");
                writer.WriteLine("\t\t}");

            }

            writer.WriteLine();
            writer.WriteLine("\t\tprivate HttpResponseMessage CreateExceptionResponse(System.Exception ex)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tHttpResponseMessage res = new HttpResponseMessage(HttpStatusCode.InternalServerError);");
            writer.WriteLine("\t\t\tres.Content = new StringContent(ex.Message);");
            writer.WriteLine("\t\t\tres.Content.Headers.ContentType = new MediaTypeHeaderValue(\"application/json\");");
            writer.WriteLine("\t\t\treturn res;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t}");
            writer.WriteLine();
        }

        private SortedList<string, List<MethodInfo>> GetMethods(Type type)
        {
            SortedList<string, List<MethodInfo>> retIndex = new SortedList<string, List<MethodInfo>>();

            MethodInfo[] list = type.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
            foreach (MethodInfo info in list)
            {
                if (!this.IsReturnTypeValid(info))
                    continue;
                
                if (!retIndex.ContainsKey(info.Name))
                {
                    retIndex.Add(info.Name, new List<MethodInfo>());
                }

                retIndex[info.Name].Add(info);
            }

            return retIndex;
        }

        private bool IsReturnTypeValid(MethodInfo info)
        {
            if (info.ReturnType.IsValueType || info.ReturnType == typeof (string)
                || info.ReturnType == typeof (byte[])
                || info.ReturnType.Name == "Result"
                || info.ReturnType.Name.StartsWith("Biz")
                || info.ReturnType.Name.EndsWith("Data")
                || info.ReturnType.Name.EndsWith("View")
                || info.ReturnType.Name.EndsWith("Collection"))
                return true;

            return false;
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
                    builder.AppendFormat("new DateTime(((DateTime)dto.GetParameter({0})).Ticks, DateTimeKind.Utc).ToLocalTime(),", i);
                }
                else
                {
                    if (info.ParameterType.Name.EndsWith("Data") || info.ParameterType.Name.EndsWith("View"))
                    {
                        builder.AppendFormat("(({0}DTO)dto.GetParameter({1})).Deserialize(),", info.ParameterType.Name, i);
                    }
                    else if (info.ParameterType.Name.EndsWith("Collection"))
                    {
                        string dtoType = info.ParameterType.Name.Substring(0, info.ParameterType.Name.Length - "Collection".Length);
                        builder.AppendFormat("{0}DTO.Deserialize(({0}DTO[])dto.GetParameter({1})),", dtoType, i);
                    }
                    else
                    {
                        builder.AppendFormat("({0})dto.GetParameter({1}),", info.ParameterType.FullName, i);
                    }
                }
            }

            return string.Format("{0} token", builder);
        }
    }
}
