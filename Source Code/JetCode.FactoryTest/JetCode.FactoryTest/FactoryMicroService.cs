using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryTest
{
    public class FactoryMicroService : FactoryBase
    {
        public FactoryMicroService(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            //writer.WriteLine("using System.ServiceModel;");
            //writer.WriteLine("using System.ServiceModel.Activation;");
            writer.WriteLine("using {0}.IMicroService;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.MicroService", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            //Convert
            foreach (ObjectSchema obj in base.MappingSchema.Objects)
            {
                if (obj.Alias == "UsrAccount" || obj.Alias == "CacheOffline")
                    continue;

                this.WriteConvertMethod(writer, obj); 
            }
            writer.WriteLine();

            //Methods
            //writer.WriteLine("\t[ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]");
            //writer.WriteLine("\t[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]");
            writer.WriteLine("\tpublic partial class ServiceFactory : ServiceBase, IServiceFactory");
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
                    writer.WriteLine("\t\tpublic E3000.MicroData.{0}[] {1}({2})", obj.Alias, methodName, this.GetParas(method));
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tE3000.Data.{0}DataCollection list = E3000.BasicServiceWrapper.{0}Wrapper.{1}({2});",
                        obj.Alias, method.Name, this.GetParasInput(method));
                    writer.WriteLine("\t\t\treturn {0}Helper.ToMicorData(list);", obj.Alias);
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                    continue;
                }
              
                if (!methodName.StartsWith("GetBy"))
                    continue;

                ParameterInfo[] paras = method.GetParameters();
                if (paras.Length != 1)
                {
                    if (methodName == "GetByModifiedOn")
                    {
                        if (paras.Length == 3)
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
                        writer.WriteLine("\t\tpublic E3000.MicroData.{0}[] {1}({2})", obj.Alias, methodName, this.GetParas(method));
                        writer.WriteLine("\t\t{");
                        writer.WriteLine("\t\t\tE3000.Data.{0}DataCollection list = E3000.BasicServiceWrapper.{0}Wrapper.{1}({2});",
                            obj.Alias, method.Name, this.GetParasInput(method));
                        writer.WriteLine("\t\t\treturn {0}Helper.ToMicorData(list);", obj.Alias);
                        writer.WriteLine("\t\t}");
                        writer.WriteLine();
                    }
                    else
                    {
                        writer.WriteLine("\t\tpublic E3000.MicroData.{0} {1}({2})", obj.Alias, methodName, this.GetParas(method));
                        writer.WriteLine("\t\t{");
                        writer.WriteLine("\t\t\tE3000.Data.{0}Data data = E3000.BasicServiceWrapper.{0}Wrapper.{1}({2});",
                            obj.Alias, method.Name, this.GetParasInput(method));
                        writer.WriteLine("\t\t\tif(data == null)");
                        writer.WriteLine("\t\t\t\treturn null;");
                        writer.WriteLine();
                        writer.WriteLine("\t\t\treturn {0}Helper.ToMicorData(data);", obj.Alias);
                        writer.WriteLine("\t\t}");
                        writer.WriteLine();
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
                    writer.WriteLine("\t\tpublic E3000.MicroData.{0}[] {1}({2} {3})", obj.Alias, methodName, paras[0].ParameterType.FullName, by);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tE3000.Data.{0}DataCollection list = E3000.BasicServiceWrapper.{0}Wrapper.{1}({2}, this.Token);",
                        obj.Alias, method.Name, by);
                    writer.WriteLine("\t\t\treturn {0}Helper.ToMicorData(list);", obj.Alias);
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }
                else if (method.ReturnType.Name.EndsWith("Data"))
                {
                    writer.WriteLine("\t\tpublic E3000.MicroData.{0} {1}({2} {3})", obj.Alias, methodName, paras[0].ParameterType.FullName, by);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tE3000.Data.{0}Data data = E3000.BasicServiceWrapper.{0}Wrapper.{1}({2}, this.Token);",
                        obj.Alias, method.Name, by);
                    writer.WriteLine("\t\t\tif(data == null)");
                    writer.WriteLine("\t\t\t\treturn null;");
                    writer.WriteLine();
                    writer.WriteLine("\t\t\treturn {0}Helper.ToMicorData(data);", obj.Alias);
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }
            }
        }

        private void WriteConvertMethod(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\tinternal static class {0}Helper", obj.Alias);
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tpublic static E3000.MicroData.{0} ToMicorData(E3000.Data.{0}Data data)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tE3000.MicroData.{0} entity = new E3000.MicroData.{0}();", obj.Alias);
            foreach (FieldSchema field in obj.Fields)
            {
                if (field.Alias == "ModifiedOn" || field.Alias == "ModifiedBy"
                    || field.Alias == "CreatedOn" || field.Alias == "CreatedBy"
                    || field.Alias == "RowVersion")
                    continue;

                writer.WriteLine("\t\t\tentity.{0} = data.{0};", field.Alias);
            }

            writer.WriteLine("\t\t\treturn entity;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic static E3000.MicroData.{0}[] ToMicorData(E3000.Data.{0}DataCollection list)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tE3000.MicroData.{0}[] retList = new E3000.MicroData.{0}[list.Count];", obj.Alias);
            writer.WriteLine("\t\t\tfor(int i = 0; i < list.Count; i++)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tretList[i] = ToMicorData(list[i]);");
            writer.WriteLine("\t\t\t}");

            writer.WriteLine("\t\t\treturn retList;");
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t}");
            writer.WriteLine();
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

        private string GetParasInput(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] paras = method.GetParameters();
            foreach (ParameterInfo item in paras)
            {
                builder.AppendFormat("{0},", item.Name);
            }
            builder.AppendFormat("this.Token");

            return builder.ToString();
        }
    }
}
