using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWCF
{
    public class FactoryWCFFacadeService : FactoryBase
    {
        private SortedList<string, Type> _dataTypeList = null;

        public FactoryWCFFacadeService(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.ServiceModel;");
            writer.WriteLine("using System.ServiceModel.Activation;");
            writer.WriteLine("using Cheke;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using Cheke.ClassFactory;");
            writer.WriteLine("using {0}.Data;", base.ProjectName);
            writer.WriteLine("using {0}.BizData;", base.ProjectName);
            writer.WriteLine("using {0}.BasicServiceWrapper;", base.ProjectName);
            writer.WriteLine("using {0}.IFacadeService;", base.ProjectName);
            writer.WriteLine("using {0}.DTO;", base.ProjectName);
            writer.WriteLine("using {0}.IWCFService;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.WCFService", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            string dllName = string.Format("{0}.Data.dll", base.ProjectName);
            this._dataTypeList = Utils.GetTypeList(base.ProjectName, dllName);

            dllName = string.Format("{0}.IFacadeService.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);

            foreach (KeyValuePair<string, Type> item in typeList)
            {
                if (item.Key == "IFacadeServiceFactory" || item.Key == "IBizExcelService")
                    continue;

                string objName = item.Key.Substring(1, item.Key.Length - "Service".Length - 1);

                writer.WriteLine("\t[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]");
                writer.WriteLine("\tpublic class {0}WCFService : I{0}WCFService", objName);
                writer.WriteLine("\t{");
                this.WriteMethod(writer, item.Value);
                writer.WriteLine("\t}");
                writer.WriteLine();
            }

            this.WriteFacadeServiceBuilder(writer);
        }

        private void WriteFacadeServiceBuilder(StringWriter writer)
        {
            string dllName = string.Format("{0}.IFacadeService.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);

            writer.WriteLine("\tinternal static class FacadeServiceBuilder");
            writer.WriteLine("\t{");
           
            foreach (KeyValuePair<string, Type> item in typeList)
            {
                if (item.Key == "IFacadeServiceFactory" || item.Key == "IBizExcelService")
                    continue;

                writer.WriteLine("\t\tinternal static {0} Get{1}(SecurityToken securityToken)", item.Key, item.Key.Substring(1));
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\treturn ServiceFactory.Get{0}(securityToken);", item.Key.Substring(1));
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

            writer.WriteLine("\t\tprivate static IFacadeServiceFactory ServiceFactory");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget{{ return (IFacadeServiceFactory) ClassBuilder.GetFactory(\"{0}.FacadeServiceFactory\"); }}", base.ProjectName);
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t\t");

            writer.WriteLine("\t}");
        }

        private void WriteMethod(StringWriter writer, Type type)
        {
            MethodInfo[] methodList = type.GetMethods();
            foreach (MethodInfo item in methodList)
            {
                string paras = this.GetParas(item);
                string invokeParas = this.GetInvokeParas(item);

                if (item.ReturnType.Name.EndsWith("DataCollection"))
                {
                    string objName = item.ReturnType.Name.Substring(0, item.ReturnType.Name.Length - "DataCollection".Length );
                    string serviceName = type.Name.Substring(1);

                    writer.WriteLine("\t\tpublic {0}DataDTOCollection {1}({2})", objName, item.Name, paras);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tSecurityToken securityToken = TokenConverter.ConvertToSecurityToken(token);");
                    this.ProcessInvokeParas(writer, item);
                    writer.WriteLine("\t\t\t{0}DataCollection dataList = FacadeServiceBuilder.Get{1}(securityToken).{2}({3});", objName, serviceName, item.Name, invokeParas);
                    writer.WriteLine("\t\t\treturn {0}Converter.ConvertToDataDTOList(dataList);", objName);
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                    continue;
                }

                if (item.ReturnType.Name.EndsWith("Data"))
                {
                    string objName = item.ReturnType.Name.Substring(0, item.ReturnType.Name.Length - "Data".Length);
                    string serviceName = type.Name.Substring(1);

                    writer.WriteLine("\t\tpublic {0}DataDTO {1}({2})", objName, item.Name, paras);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tSecurityToken securityToken = TokenConverter.ConvertToSecurityToken(token);");
                    this.ProcessInvokeParas(writer, item);
                    writer.WriteLine("\t\t\t{0}Data data = FacadeServiceBuilder.Get{1}(securityToken).{2}({3});", objName, serviceName, item.Name, invokeParas);
                    writer.WriteLine("\t\t\treturn {0}Converter.ConvertToDataDTO(data);", objName);
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                    continue;
                }

                if (item.ReturnType.Name.EndsWith("ViewCollection"))
                {
                    string viewObj = item.ReturnType.Name.Substring(0, item.ReturnType.Name.Length - "ViewCollection".Length);
                    string serviceName = type.Name.Substring(1);

                    writer.WriteLine("\t\tpublic {0}ViewDTOCollection {1}({2})", viewObj, item.Name, paras);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tSecurityToken securityToken = TokenConverter.ConvertToSecurityToken(token);");
                    this.ProcessInvokeParas(writer, item);
                    writer.WriteLine("\t\t\t{0}ViewCollection viewList = FacadeServiceBuilder.Get{1}(securityToken).{2}({3});", viewObj, serviceName, item.Name, invokeParas);
                    writer.WriteLine("\t\t\treturn {0}Converter.ConvertToViewDTOList(viewList);", viewObj);
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                    continue;
                }

                if (item.ReturnType.Name.EndsWith("View"))
                {
                    string viewObj = item.ReturnType.Name.Substring(0, item.ReturnType.Name.Length - "View".Length);
                    string serviceName = type.Name.Substring(1);

                    writer.WriteLine("\t\tpublic {0}ViewDTO {1}({2})", viewObj, item.Name, paras);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tSecurityToken securityToken = TokenConverter.ConvertToSecurityToken(token);");
                    this.ProcessInvokeParas(writer, item);
                    writer.WriteLine("\t\t\t{0}View view = FacadeServiceBuilder.Get{1}(securityToken).{2}({3});", viewObj, serviceName, item.Name, invokeParas);
                    writer.WriteLine("\t\t\treturn {0}Converter.ConvertToViewDTO(view);", viewObj);
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                    continue;
                }

                if (item.ReturnType.Name.StartsWith("Biz"))
                {
                    string serviceName = type.Name.Substring(1);
                    if (item.ReturnType.Name.EndsWith("Collection"))
                    {
                        string objName = item.ReturnType.Name.Substring(0, item.ReturnType.Name.Length - "Collection".Length);

                        writer.WriteLine("\t\tpublic {0}DTOCollection {1}({2})", objName, item.Name, paras);
                        writer.WriteLine("\t\t{");
                        writer.WriteLine("\t\t\tSecurityToken securityToken = TokenConverter.ConvertToSecurityToken(token);");
                        this.ProcessInvokeParas(writer, item);
                        writer.WriteLine("\t\t\t{0}Collection bizList = FacadeServiceBuilder.Get{1}(securityToken).{2}({3});", objName, serviceName, item.Name, invokeParas);
                        writer.WriteLine("\t\t\treturn {0}Converter.ConvertToDTOList(bizList);", objName);
                        writer.WriteLine("\t\t}");
                        writer.WriteLine();
                    }
                    else
                    {
                        string objName = item.ReturnType.Name;

                        writer.WriteLine("\t\tpublic {0}DTO {1}({2})", objName, item.Name, paras);
                        writer.WriteLine("\t\t{");
                        writer.WriteLine("\t\t\tSecurityToken securityToken = TokenConverter.ConvertToSecurityToken(token);");
                        this.ProcessInvokeParas(writer, item);
                        writer.WriteLine("\t\t\t{0} biz = FacadeServiceBuilder.Get{1}(securityToken).{2}({3});", objName,serviceName, item.Name, invokeParas);
                        writer.WriteLine("\t\t\treturn {0}Converter.ConvertToDTO(biz);", objName);
                        writer.WriteLine("\t\t}");
                        writer.WriteLine();
                    }

                    continue;
                }

                string returnTypeName;
                if(item.ReturnType.Name == "Result")
                {
                    returnTypeName = "ResultDTO";
                }
                else
                {
                    returnTypeName = item.ReturnType.Name;
                }
                writer.WriteLine("\t\tpublic {0} {1}({2})", returnTypeName, item.Name, paras);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tSecurityToken securityToken = TokenConverter.ConvertToSecurityToken(token);");
                this.ProcessInvokeParas(writer, item); 
                if (item.ReturnType.Name == "Result")
                {
                    string serviceName = type.Name.Substring(1);
                    writer.WriteLine("\t\t\tResult result = FacadeServiceBuilder.Get{0}(securityToken).{1}({2});", serviceName, item.Name, invokeParas);
                    writer.WriteLine("\t\t\treturn ResultConverter.ConvertToResultDTO(result);");
                }
                else
                {
                    string serviceName = type.Name.Substring(1);
                    writer.WriteLine("\t\t\treturn FacadeServiceBuilder.Get{0}(securityToken).{1}({2});", serviceName, item.Name, invokeParas);
                }
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }
        }

        private void ProcessInvokeParas(StringWriter writer, MethodInfo method)
        {
            ParameterInfo[] paras = method.GetParameters();
            foreach (ParameterInfo item in paras)
            {
                if (!this._dataTypeList.ContainsKey(item.ParameterType.Name))
                    continue;

                Type type = this._dataTypeList[item.ParameterType.Name];

                if (item.ParameterType.Name.EndsWith("View"))
                {
                    string objName = type.Name.Substring(0, type.Name.Length - "View".Length);

                    writer.WriteLine("\t\t\t{0} {1} = {2}Converter.ConvertToView({1}DTO);", type.Name, item.Name, objName);
                    writer.WriteLine();
                    continue;
                }

                if (item.ParameterType.Name.EndsWith("ViewCollection"))
                {
                    string objName = type.Name.Substring(0, type.Name.Length - "ViewCollection".Length);

                    writer.WriteLine("\t\t\t{0} {1} = {2}Converter.ConvertToViewList({1}DTO);", type.Name, item.Name, objName);
                    writer.WriteLine();
                    continue;
                }

                if (item.ParameterType.Name.EndsWith("Data"))
                {
                    string objName = type.Name.Substring(0, type.Name.Length - "Data".Length);

                    writer.WriteLine("\t\t\t{0} {1} = {2}Converter.ConvertToData({1}DTO);", type.Name, item.Name, objName);
                    writer.WriteLine();
                    continue;
                }

                if (item.ParameterType.Name.EndsWith("DataCollection"))
                {
                    string objName = type.Name.Substring(0, type.Name.Length - "DataCollection".Length);

                    writer.WriteLine("\t\t\t{0} {1} = {2}Converter.ConvertToDataList({1}DTO);", type.Name, item.Name, objName);
                    writer.WriteLine();
                    continue;
                }
            }
        }

        private string GetInvokeParas(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] paras = method.GetParameters();
            foreach (ParameterInfo item in paras)
            {
                builder.AppendFormat("{0}, ", item.Name);
            }

            return builder.ToString().TrimEnd(' ').TrimEnd(',');
        }

        private string GetParas(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] paras = method.GetParameters();
            foreach (ParameterInfo item in paras)
            {
               builder.AppendFormat("{0}", this.GetParameterType(item));
            }

            builder.AppendFormat("TokenDTO token");
            return builder.ToString();
        }

        private string GetParameterType(ParameterInfo item)
        {
            string returnTypeName = item.ParameterType.Name;
            if (item.ParameterType.IsClass)
            {
                if (returnTypeName.EndsWith("DataCollection"))
                {
                    string objName = returnTypeName.Substring(0, returnTypeName.Length - "DataCollection".Length );
                    return string.Format("{0}DataDTOCollection {1}DTO, ", objName, item.Name);
                }

                if (returnTypeName.EndsWith("ViewCollection"))
                {
                    string objName = returnTypeName.Substring(0, returnTypeName.Length - "ViewCollection".Length );
                    return string.Format("{0}ViewDTOCollection {1}DTO, ", objName, item.Name);

                }

                if (returnTypeName.EndsWith("Data"))
                {
                    string objName = returnTypeName.Substring(0, returnTypeName.Length - "Data".Length);
                    return string.Format("{0}DataDTO {1}DTO, ", objName, item.Name);

                }

                if (returnTypeName.EndsWith("View"))
                {
                    string objName = returnTypeName.Substring(0, returnTypeName.Length - "View".Length);
                    return string.Format("{0}ViewDTO {1}DTO, ", objName, item.Name);
                }

                if (returnTypeName.StartsWith("Biz"))
                {
                    if (returnTypeName.EndsWith("Collection"))
                    {
                        string bizObj = returnTypeName.Substring(0, returnTypeName.Length - "Collection".Length);
                        return string.Format("{0}DTO {1}DTO, ", bizObj, item.Name);
                    }
                  
                    return string.Format("{0}DTO {1}DTO, ", returnTypeName, item.Name);
                }
            }

            return string.Format("{0} {1}, ", returnTypeName, item.Name);
        }
    }
}
