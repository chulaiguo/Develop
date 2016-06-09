using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWCF
{
    public class FactoryClientBasicObjMethods : FactoryBase
    {
        public FactoryClientBasicObjMethods(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Threading;");
            writer.WriteLine("using System.ServiceModel;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using {0}.DTO;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.ViewObj", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            string dllName = string.Format("{0}.IDataService.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);

            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                string typeKey = string.Format("I{0}DataService", item.Alias);
                if(!typeList.ContainsKey(typeKey))
                    continue;

                writer.WriteLine("\tpublic partial class {0}Collection", item.Alias);
                writer.WriteLine("\t{");
                writer.WriteLine("\t\tprivate SaveCallBack _saveCallBack = null;");
                writer.WriteLine("\t\tpublic override void Save(SaveCallBack callBack)");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\t{0}DataDTOCollection dtoList = this.GetChangedDTOList(Identity.Token.UserId, DateTime.Now);", item.Alias);
                writer.WriteLine("\t\t\tif(dtoList == null || dtoList.Count == 0)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tcallBack(new Result());");
                writer.WriteLine("\t\t\t\treturn;");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t\tthis._saveCallBack = callBack;");
                writer.WriteLine("\t\t\t{0}Wrapper.SaveList(dtoList, saveDTO_CallBack);", item.Alias);
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tprivate void saveDTO_CallBack(object obj)");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tResultDTO dto = obj as ResultDTO;");
                writer.WriteLine("\t\t\tif (dto == null)");
                writer.WriteLine("\t\t\t\treturn;");
                writer.WriteLine("");
                writer.WriteLine("\t\t\tResult result = ResultConverter.ConvertTo(dto);");
                writer.WriteLine("\t\t\tthis._saveCallBack(result);");
                writer.WriteLine("\t\t}");
                writer.WriteLine("\t}");

                writer.WriteLine();
                writer.WriteLine("\tpublic partial class {0}", item.Alias);
                writer.WriteLine("\t{");
                writer.WriteLine("\t\tprivate SaveCallBack _saveCallBack = null;");
                writer.WriteLine("\t\tpublic override void Save(SaveCallBack callBack)");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\t{0}DataDTO dto = this.GetChangedDTO(Identity.Token.UserId, DateTime.Now);", item.Alias);
                writer.WriteLine("\t\t\tif(dto == null)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tcallBack(new Result());");
                writer.WriteLine("\t\t\t\treturn;");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t\tthis._saveCallBack = callBack;");
                writer.WriteLine("\t\t\t{0}Wrapper.SaveItem(dto, saveDTO_CallBack);", item.Alias);
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tprivate void saveDTO_CallBack(object obj)");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tResultDTO dto = obj as ResultDTO;");
                writer.WriteLine("\t\t\tif (dto == null)");
                writer.WriteLine("\t\t\t\treturn;");
                writer.WriteLine();
                writer.WriteLine("\t\t\tResult result = ResultConverter.ConvertTo(dto);");
                writer.WriteLine("\t\t\tthis._saveCallBack(result);");
                writer.WriteLine("\t\t}");

                ////Methods
                //MethodInfo[] methodList = typeList[typeKey].GetMethods();
                //SortedList<string, string> methodIndex = new SortedList<string, string>();
                //methodIndex.Add("Save", "Save");
                //foreach (MethodInfo method in methodList)
                //{
                //    if(methodIndex.ContainsKey(method.Name))
                //        continue;

                //    methodIndex.Add(method.Name, method.Name);

                //    writer.WriteLine("\t\tpublic static void {0}({1})", method.Name, this.GetParas(method));
                //    writer.WriteLine("\t\t{");
                //    writer.WriteLine("\t\t\t{0}Wrapper.{1}({2});", item.Alias, method.Name, this.GetInvokeParas(method));
                //    writer.WriteLine("\t\t}");
                //    writer.WriteLine();
                //}

                writer.WriteLine("\t}");
            }

            //ResultConverter
            writer.WriteLine("\tpublic static class ResultConverter");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tpublic static Result ConvertTo(ResultDTO dto)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tResult result = new Result();");
            writer.WriteLine("\t\t\tforeach (KeyValuePair<Guid, string> item in dto.ErrorList)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tresult.ErrorList.Add(item.Key, item.Value);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tforeach (KeyValuePair<Guid, byte[]> item in dto.RowversionList)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tresult.RowversionList.Add(item.Key, item.Value);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tforeach (KeyValuePair<string, string> item in dto.TagList)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tresult.TagList.Add(item.Key, item.Value);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn result;");
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t}");
        }

        private string GetInvokeParas(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();

            ParameterInfo[] paras = method.GetParameters();
            foreach (ParameterInfo item in paras)
            {
                builder.AppendFormat("{0}, ", item.Name);
            }

            builder.AppendFormat("updateUI");
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

            builder.AppendFormat("UpdateUIDelegate updateUI");
            return builder.ToString();
        }

        private string GetParameterType(ParameterInfo item)
        {
            string returnTypeName = item.ParameterType.Name;
            if (item.ParameterType.IsClass)
            {
                string dtoType = this.GetDTOType(returnTypeName);
                return string.Format("{0} {1}, ", dtoType, item.Name);
            }

            return string.Format("{0} {1}, ", returnTypeName, item.Name);
        }

        private string GetDTOType(string originalTypeName)
        {
            if (originalTypeName == "Result")
            {
                return "ResultDTO";
            }

            if (originalTypeName.EndsWith("DataCollection"))
            {
                string objName = originalTypeName.Substring(0, originalTypeName.Length - "DataCollection".Length);
                return string.Format("{0}DataDTOCollection", objName);
            }

            if (originalTypeName.EndsWith("ViewCollection"))
            {
                string objName = originalTypeName.Substring(0, originalTypeName.Length - "ViewCollection".Length);
                return string.Format("{0}ViewDTOCollection", objName);

            }

            if (originalTypeName.EndsWith("Data"))
            {
                string objName = originalTypeName.Substring(0, originalTypeName.Length - "Data".Length);
                return string.Format("{0}DataDTO", objName);

            }

            if (originalTypeName.EndsWith("View"))
            {
                string objName = originalTypeName.Substring(0, originalTypeName.Length - "View".Length);
                return string.Format("{0}ViewDTO", objName);
            }

            return originalTypeName;
        }
    }
}
