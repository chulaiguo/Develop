using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWCF
{
    public class FactoryWCFBasicService : FactoryBase
    {
        private SortedList<string, Type> _dataTypeList = null;

        public FactoryWCFBasicService(MappingSchema mappingSchema)
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
            writer.WriteLine("using {0}.Data;", base.ProjectName);
            writer.WriteLine("using {0}.BasicServiceWrapper;", base.ProjectName);
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

            dllName = string.Format("{0}.IDataService.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);

            foreach (KeyValuePair<string, Type> item in typeList)
            {
                if(item.Key == "IDataServiceFactory")
                    continue;

                string objName = item.Key.Substring(1, item.Key.Length - "DataService".Length - 1);

                writer.WriteLine("\t[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]");
                writer.WriteLine("\tpublic class {0}WCFService : I{0}WCFService", objName);
                writer.WriteLine("\t{");
                this.WriteMethod(writer, item.Value, objName);
                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }

        private void WriteMethod(StringWriter writer, Type type, string objName)
        {
            writer.WriteLine();
            writer.WriteLine("\t\tpublic ResultDTO SaveItem({0}DataDTO entity, TokenDTO token)", objName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tSecurityToken securityToken = TokenConverter.ConvertToSecurityToken(token);");
            writer.WriteLine("\t\t\t{0}Data data = {0}Converter.ConvertToData(entity);", objName);
            writer.WriteLine("\t\t\tResult result = {0}Wrapper.Save(data, securityToken);", objName);
            writer.WriteLine("\t\t\treturn ResultConverter.ConvertToResultDTO(result);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic ResultDTO SaveList({0}DataDTOCollection list, TokenDTO token)", objName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tSecurityToken securityToken = TokenConverter.ConvertToSecurityToken(token);");
            writer.WriteLine("\t\t\t{0}DataCollection dataList = {0}Converter.ConvertToDataList(list);", objName);
            writer.WriteLine("\t\t\tResult result = {0}Wrapper.Save(dataList, securityToken);", objName);
            writer.WriteLine("\t\t\treturn ResultConverter.ConvertToResultDTO(result);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            MethodInfo[] methodList = type.GetMethods();
            foreach (MethodInfo item in methodList)
            {
                if (item.Name == "Save")
                    continue;

                string paras = this.GetParas(item);
                string invokeParas = this.GetInvokeParas(item);
                if (item.ReturnType.Name.EndsWith("DataCollection"))
                {
                    writer.WriteLine("\t\tpublic {0}DataDTOCollection {1}({2})", objName, item.Name, paras);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tSecurityToken securityToken = TokenConverter.ConvertToSecurityToken(token);");
                    this.ProcessInvokeParas(writer, item);
                    writer.WriteLine("\t\t\t{0}DataCollection dataList = {0}Wrapper.{1}({2});", objName, item.Name, invokeParas);
                    writer.WriteLine("\t\t\treturn {0}Converter.ConvertToDataDTOList(dataList);", objName);
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                    continue;
                }

                if (item.ReturnType.Name.EndsWith("Data"))
                {
                    writer.WriteLine("\t\tpublic {0}DataDTO {1}({2})", objName, item.Name, paras);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tSecurityToken securityToken = TokenConverter.ConvertToSecurityToken(token);");
                    this.ProcessInvokeParas(writer, item); 
                    writer.WriteLine("\t\t\t{0}Data data = {0}Wrapper.{1}({2});", objName, item.Name, invokeParas);
                    writer.WriteLine("\t\t\treturn {0}Converter.ConvertToDataDTO(data);", objName);
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                    continue;
                }

                if (item.ReturnType.Name.EndsWith("ViewCollection"))
                {
                    string viewObj = item.ReturnType.Name.Substring(0, item.ReturnType.Name.Length - "ViewCollection".Length);

                    writer.WriteLine("\t\tpublic {0}ViewDTOCollection {1}({2})", viewObj, item.Name, paras);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tSecurityToken securityToken = TokenConverter.ConvertToSecurityToken(token);");
                    this.ProcessInvokeParas(writer, item); 
                    writer.WriteLine("\t\t\t{0}ViewCollection viewList = {1}Wrapper.{2}({3});", viewObj, objName, item.Name, invokeParas);
                    writer.WriteLine("\t\t\treturn {0}Converter.ConvertToViewDTOList(viewList);", viewObj);
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                    continue;
                }

                if (item.ReturnType.Name.EndsWith("View"))
                {
                    string viewObj = item.ReturnType.Name.Substring(0, item.ReturnType.Name.Length - "View".Length);

                    writer.WriteLine("\t\tpublic {0}ViewDTO {1}({2})", viewObj, item.Name, paras);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tSecurityToken securityToken = TokenConverter.ConvertToSecurityToken(token);");
                    this.ProcessInvokeParas(writer, item);
                    writer.WriteLine("\t\t\t{0}View view = {1}Wrapper.{2}({3});", viewObj, objName, item.Name, invokeParas);
                    writer.WriteLine("\t\t\treturn {0}Converter.ConvertToViewDTO(view);", viewObj);
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
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
                    writer.WriteLine("\t\t\tResult result = {0}Wrapper.{1}({2});", objName, item.Name, invokeParas);
                    writer.WriteLine("\t\t\treturn ResultConverter.ConvertToResultDTO(result);");
                }
                else
                {
                    writer.WriteLine("\t\t\treturn {0}Wrapper.{1}({2});", objName, item.Name, invokeParas);
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

            builder.AppendFormat("securityToken");
            return builder.ToString();
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
            }

            return string.Format("{0} {1}, ", returnTypeName, item.Name);
        }
    }
}
