using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWCF
{
    public class FactoryWCFBasicConverter : FactoryBase
    {
        public FactoryWCFBasicConverter(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Text;");
            writer.WriteLine("using System.Collections;");
            writer.WriteLine("using Cheke;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using {0}.Data;", base.ProjectName);
            writer.WriteLine("using {0}.DTO;", base.ProjectName);

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
            //Database entity
            string dllName = string.Format("{0}.Data.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);

            this.WriteTokenConverter(writer);
            this.WriteResultConverter(writer);

            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                writer.WriteLine("\tpublic static class {0}Converter", item.Alias);
                writer.WriteLine("\t{");

                //Data
                string dataTypeKey = string.Format("{0}Data", item.Alias);
                if (typeList.ContainsKey(dataTypeKey))
                {
                    this.WriteConvertToData(writer, item, typeList[dataTypeKey]);
                    this.WriteConvertToDataDTO(writer, item, typeList[dataTypeKey]);
                }

                //View
                string viewTypeKey = string.Format("{0}View", item.Alias);
                if (typeList.ContainsKey(viewTypeKey))
                {
                    this.WriteConvertToView(writer, item.Alias, typeList[viewTypeKey], true);
                    this.WriteConvertToViewDTO(writer, item.Alias, typeList[viewTypeKey], true);
                }

                writer.WriteLine("\t}");
                writer.WriteLine();
            }

            //Other entity
            SortedList<string, ObjectSchema> tableIndex = new SortedList<string, ObjectSchema>();
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                string key = string.Format("{0}Data", item.Name);
                if (!tableIndex.ContainsKey(key))
                {
                    tableIndex.Add(key, item);
                }

                key = string.Format("{0}View", item.Name);
                if (!tableIndex.ContainsKey(key))
                {
                    tableIndex.Add(key, item);
                }

                key = string.Format("{0}DataCollection", item.Name);
                if (!tableIndex.ContainsKey(key))
                {
                    tableIndex.Add(key, item);
                }

                key = string.Format("{0}ViewCollection", item.Name);
                if (!tableIndex.ContainsKey(key))
                {
                    tableIndex.Add(key, item);
                }
            }
            foreach (KeyValuePair<string, Type> item in typeList)
            {
                if (tableIndex.ContainsKey(item.Key))
                    continue;

                if (item.Key.EndsWith("Collection"))
                    continue;

                string objName = item.Key.Substring(0, item.Key.Length - "View".Length);

                writer.WriteLine("\tpublic static class {0}Converter", objName);
                writer.WriteLine("\t{");

                string key = string.Format("{0}Collection", item.Key);
                bool includeList = typeList.ContainsKey(key);
                this.WriteConvertToView(writer, objName, item.Value, includeList);
                this.WriteConvertToViewDTO(writer, objName, item.Value, includeList);

                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }

        private void WriteTokenConverter(StringWriter writer)
        {
            writer.WriteLine("\tpublic static class TokenConverter");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tpublic static SecurityToken ConvertToSecurityToken(TokenDTO token)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tCheke.SecurityToken securityToken = new Cheke.SecurityToken(token.UserId, string.Empty, string.Empty, token.Ticks);");
            writer.WriteLine("\t\t\tsecurityToken.Password = token.Password;");
            writer.WriteLine("\t\t\tsecurityToken.TokenID = token.TokenID;");
            writer.WriteLine("\t\t\tsecurityToken.BlockIndex = token.BlockIndex;");
            writer.WriteLine("\t\t\tsecurityToken.BlockSize = token.BlockSize;");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn securityToken;");
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t}");
            writer.WriteLine();
        }

        private void WriteResultConverter(StringWriter writer)
        {
            writer.WriteLine("\tpublic static class ResultConverter");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tpublic static ResultDTO ConvertToResultDTO(Result r)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tResultDTO result = new ResultDTO();");
            writer.WriteLine();
            writer.WriteLine("\t\t\t//Rowversion");
            writer.WriteLine("\t\t\tforeach (DictionaryEntry item in r.RowVersions)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tresult.RowversionList.Add((Guid)item.Key, (byte[])item.Value);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\t//Error");
            writer.WriteLine("\t\t\tforeach (DictionaryEntry item in r.Errors)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tException ex = item.Value as Exception;");
            writer.WriteLine("\t\t\t\tif (ex == null)");
            writer.WriteLine("\t\t\t\t\tcontinue;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tresult.ErrorList.Add((Guid)item.Key, GetErrorMessage(ex));");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn result;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tprivate static string GetErrorMessage(Exception ex)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tStringBuilder builder = new StringBuilder();");
            writer.WriteLine("\t\t\tbuilder.AppendLine(ex.Message);");
            writer.WriteLine("\t\t\tbuilder.AppendLine();");
            writer.WriteLine();
            writer.WriteLine("\t\t\tfor (Exception exception = ex.InnerException; exception != null; exception = exception.InnerException)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tbuilder.AppendLine(exception.Message);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn builder.ToString();");
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t}");
            writer.WriteLine();
        }

        private void WriteConvertToViewDTO(StringWriter writer, string objName, Type viewType, bool includeList)
        {
            writer.WriteLine("\t\tpublic static {0}DTO ConvertToViewDTO({0} view)", viewType.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}DTO entity = new {0}DTO();", viewType.Name);

            PropertyInfo[] propertyInfoList = viewType.GetProperties();
            foreach (PropertyInfo info in propertyInfoList)
            {
                if (!info.CanWrite || !info.CanRead)
                    continue;

                writer.WriteLine("\t\t\tentity.{0} = view.{0};", info.Name);
            }

            writer.WriteLine();
            writer.WriteLine("\t\t\treturn entity;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            if (!includeList)
                return;

            writer.WriteLine("\t\tpublic static {0}DTOCollection ConvertToViewDTOList({0}Collection viewList)", viewType.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}DTOCollection retList = new {0}DTOCollection();", viewType.Name);
            writer.WriteLine("\t\t\tretList.BlockCount = viewList.Block.Count;");
            writer.WriteLine("\t\t\tretList.BlockIndex = viewList.Block.Index;");
            writer.WriteLine("\t\t\tretList.BlockSize = viewList.Block.Size;");
            writer.WriteLine("\t\t\tforeach ({0} item in viewList)", viewType.Name);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t{0}DTO entity = {1}Converter.ConvertToViewDTO(item);", viewType.Name, objName);
            writer.WriteLine("\t\t\t\tretList.Add(entity);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn retList;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteConvertToView(StringWriter writer, string objName, Type viewType, bool includeList)
        {
            writer.WriteLine("\t\tpublic static {0} ConvertToView({0}DTO dto)", viewType.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0} entity = new {0}();", viewType.Name);

            PropertyInfo[] propertyInfoList = viewType.GetProperties();
            foreach (PropertyInfo info in propertyInfoList)
            {
                if (!info.CanWrite || !info.CanRead)
                    continue;

                writer.WriteLine("\t\t\tentity.{0} = dto.{0};", info.Name);
            }

            writer.WriteLine();
            writer.WriteLine("\t\t\treturn entity;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            if (!includeList)
                return;

            writer.WriteLine("\t\tpublic static {0}Collection ConvertToViewList({0}DTOCollection dtoList)", viewType.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}Collection retList = new {0}Collection();", viewType.Name);
            writer.WriteLine("\t\t\tforeach ({0}DTO item in dtoList)", viewType.Name);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t{0} entity = {1}Converter.ConvertToView(item);", viewType.Name, objName);
            writer.WriteLine("\t\t\t\tretList.Add(entity);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn retList;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteConvertToData(StringWriter writer, ObjectSchema objSchema, Type dataType)
        {
            writer.WriteLine("\t\tpublic static {0} ConvertToData({0}DTO dto)", dataType.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0} data = new {0}();", dataType.Name);
            writer.WriteLine("\t\t\tif(!dto.IsNew)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tdata.AcceptChanges();");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tif(dto.IsDeleted)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tdata.AcceptChanges();");
            writer.WriteLine("\t\t\t\tdata.Delete();");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();

            PropertyInfo[] propertyInfoList = dataType.GetProperties();
            foreach (PropertyInfo info in propertyInfoList)
            {
                if (!info.CanWrite || !info.CanRead)
                    continue;

                if (info.PropertyType.IsValueType || info.PropertyType == typeof(string)
                    || info.PropertyType == typeof(byte[]))
                {
                    writer.WriteLine("\t\t\tdata.{0} = dto.{0};", info.Name);
                }
            }

            writer.WriteLine();
            writer.WriteLine("\t\t\tif(!dto.IsSelfDirty)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tdata.AcceptChanges();");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();

            foreach (ChildSchema item in objSchema.Children)
            {
                writer.WriteLine("\t\t\tif (dto.{0}List != null && dto.{0}List.Count > 0)", item.Alias);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tdata.{0}List = new {0}DataCollection();", item.Alias);
                writer.WriteLine("\t\t\t\tforeach ({0}DataDTO item in dto.{0}List)", item.Alias);
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tdata.{0}List.Add({0}Converter.ConvertToData(item));", item.Alias);
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();
            }

            writer.WriteLine("\t\t\treturn data;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine();
            writer.WriteLine("\t\tpublic static {0}Collection ConvertToDataList({0}DTOCollection dtoList)", dataType.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}Collection retList = new {0}Collection();", dataType.Name);
            writer.WriteLine("\t\t\tforeach ({0}DTO item in dtoList)", dataType.Name);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t{0} entity = {1}Converter.ConvertToData(item);", dataType.Name, objSchema.Alias);
            writer.WriteLine("\t\t\t\tretList.Add(entity);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn retList;");
            writer.WriteLine("\t\t}");
        }

        private void WriteConvertToDataDTO(StringWriter writer, ObjectSchema objSchema, Type dataType)
        {
            writer.WriteLine("\t\tpublic static {0}DTO ConvertToDataDTO({0} data)", dataType.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}DTO dto = new {0}DTO();", dataType.Name);
            writer.WriteLine("\t\t\tdto.ObjectID = data.ObjectID;");
            PropertyInfo[] propertyInfoList = dataType.GetProperties();
            foreach (PropertyInfo info in propertyInfoList)
            {
                if (!info.CanWrite || !info.CanRead)
                    continue;

                if (info.PropertyType.IsValueType || info.PropertyType == typeof(string)
                    || info.PropertyType == typeof(byte[]))
                {
                    writer.WriteLine("\t\t\tdto.{0} = data.{0};", info.Name);
                }
            }

            writer.WriteLine("\t\t\tdto.IsNew = data.IsNew;");
            writer.WriteLine("\t\t\tdto.IsSelfDirty = data.IsSelfDirty;");
            writer.WriteLine("\t\t\tdto.IsDeleted = data.IsDeleted;");
            writer.WriteLine();

            foreach (ChildSchema item in objSchema.Children)
            {
                writer.WriteLine("\t\t\tif(data.{0}List != null && data.{0}List.Count > 0)", item.Alias);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tdto.{0}List = new {0}DataDTOCollection();", item.Alias);
                writer.WriteLine("\t\t\t\tforeach ({0}Data item in data.{0}List)", item.Alias);
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tdto.{0}List.Add({0}Converter.ConvertToDataDTO(item));", item.Alias);
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();
            }

            writer.WriteLine("\t\t\treturn dto;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tpublic static {0}DTOCollection ConvertToDataDTOList({0}Collection dataList)", dataType.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}DTOCollection retList = new {0}DTOCollection();", dataType.Name);
            writer.WriteLine("\t\t\tretList.BlockCount = dataList.Block.Count;");
            writer.WriteLine("\t\t\tretList.BlockIndex = dataList.Block.Index;");
            writer.WriteLine("\t\t\tretList.BlockSize = dataList.Block.Size;");
            writer.WriteLine("\t\t\tforeach ({0} item in dataList)", dataType.Name);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tretList.Add({0}Converter.ConvertToDataDTO(item));", objSchema.Alias);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn retList;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }
    }
}
