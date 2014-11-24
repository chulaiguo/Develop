using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryTest
{
    public class FactoryMicroIService : FactoryBase
    {
        public FactoryMicroIService(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System.ServiceModel;");
            writer.WriteLine("using System.ServiceModel.Web;");

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.IMicroService", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            //writer.WriteLine("\t[ServiceContract(Namespace=\"http://www.cheke.com/\")]");
            writer.WriteLine("\t[ServiceContract]");
            writer.WriteLine("\tpublic partial interface IServiceFactory");
            writer.WriteLine("\t{");

            SortedList<string, Type> dataTypeList = Utils.GetTypeList(this.ProjectName, "E3000.DataService.dll");
            foreach (ObjectSchema obj in base.MappingSchema.Objects)
            {
                if (obj.Alias == "UsrAccount" || obj.Alias == "CacheOffline")
                    continue;

                string key = string.Format("{0}DataService", obj.Alias);
                if (!dataTypeList.ContainsKey(key))
                    continue;

                this.WriteBasicMethods(writer, obj, dataTypeList[key]);
            }
            writer.WriteLine("\t}");
        }

        private void WriteBasicMethods(StringWriter writer, ObjectSchema obj, Type type)
        {
            List<MethodInfo> list = this.GetMethodList(type);
            foreach (MethodInfo method in list)
            {
                if(method.ReturnType.Name.EndsWith("ViewCollection"))
                    continue;

                string methodName = method.Name;
                if (methodName == "GetAll")
                {
                    methodName = string.Format("Get{0}All", obj.Alias);

                    writer.WriteLine("\t\t[OperationContract]");
                    writer.WriteLine("\t\t[WebGet(UriTemplate=\"/{0}\", ResponseFormat = WebMessageFormat.Json)]", obj.Alias);
                    writer.WriteLine("\t\tE3000.MicroData.{0}[] {1}({2});", obj.Alias, methodName, this.GetParas(method));
                    continue;
                }
               

                if (!methodName.StartsWith("GetBy"))
                    continue;

                ParameterInfo[] paras = method.GetParameters();
                if (paras.Length != 1)
                {
                    if(methodName == "GetByModifiedOn")
                    {
                        if(paras.Length == 3)
                        {
                            methodName = string.Format("Get{0}ByModifiedOnAndPanel", obj.Alias);
                        }
                        else
                        {
                            methodName = string.Format("Get{0}ByModifiedOn", obj.Alias);
                        }
                    }
                    else
                    {
                        methodName = string.Format("Get{0}{1}", obj.Alias, methodName.Substring(3));
                    }
                    if (method.ReturnType.Name.EndsWith("Collection"))
                    {
                        writer.WriteLine("\t\t[OperationContract]");
                        writer.WriteLine(
                            "\t\t[WebGet(UriTemplate=\"/{0}/{1}?{2}\", ResponseFormat = WebMessageFormat.Json)]",
                            obj.Alias, methodName, this.GetParasWeb(method));
                        writer.WriteLine("\t\tE3000.MicroData.{0}[] {1}({2});", obj.Alias, methodName, this.GetParas(method));
                    }
                    else
                    {
                        writer.WriteLine("\t\t[OperationContract]");
                        writer.WriteLine(
                            "\t\t[WebGet(UriTemplate=\"/{0}/{1}?{2}\", ResponseFormat = WebMessageFormat.Json)]",
                            obj.Alias, methodName, this.GetParasWeb(method));
                        writer.WriteLine("\t\tE3000.MicroData.{0} {1}({2});", obj.Alias, methodName, this.GetParas(method));
                    }
                    continue;
                }

                methodName = string.Format("Get{0}By{1}", obj.Alias, method.Name.Substring(5));
                string by = method.Name.Substring(5);
                if (paras[0].ParameterType == typeof(Guid) && by != "PK")
                {
                    by = by + "PK";
                }
                if (method.ReturnType.Name.EndsWith("Collection"))
                {
                    writer.WriteLine("\t\t[OperationContract]");
                    writer.WriteLine("\t\t[WebGet(UriTemplate=\"/{0}/{1}?{2}={{{2}}}\", ResponseFormat = WebMessageFormat.Json)]", obj.Alias, method.Name, by);
                    writer.WriteLine("\t\tE3000.MicroData.{0}[] {1}({2} {3});", obj.Alias, methodName, paras[0].ParameterType.FullName, by);
                }
                else if (method.ReturnType.Name.EndsWith("Data"))
                {
                    writer.WriteLine("\t\t[OperationContract]");
                    writer.WriteLine("\t\t[WebGet(UriTemplate=\"/{0}/{1}?{2}={{{2}}}\", ResponseFormat = WebMessageFormat.Json)]", obj.Alias, method.Name, by);
                    writer.WriteLine("\t\tE3000.MicroData.{0} {1}({2} {3});", obj.Alias, methodName, paras[0].ParameterType.FullName, by);
                }
            }
        }

        private List<MethodInfo> GetMethodList(Type type)
        {
            List<MethodInfo> retList = new List<MethodInfo>();
            MethodInfo[] properties = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (MethodInfo info in properties)
            {
                retList.Add(info);
            }

            return retList;
        }

        private string GetParas(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] paras = method.GetParameters();
            foreach (ParameterInfo item in paras)
            {
                builder.AppendFormat(" {0} {1},", item.ParameterType.FullName, item.Name);
            }

            return builder.ToString().TrimEnd(',').Trim();
        }

        private string GetParasWeb(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] paras = method.GetParameters();
            foreach (ParameterInfo item in paras)
            {
                builder.AppendFormat("{0}={{{1}}}&", item.Name, item.Name);
            }

            return builder.ToString().TrimEnd('&').Trim();
        }
    }
}
