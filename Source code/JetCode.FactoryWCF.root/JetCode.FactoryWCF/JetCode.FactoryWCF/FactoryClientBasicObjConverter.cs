using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWCF
{
    public class FactoryClientBasicObjConverter : FactoryBase
    {
        public FactoryClientBasicObjConverter(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Text;");
            writer.WriteLine("using System.Collections;");
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
            string dllName = string.Format("{0}.Data.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);

            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                string dataTypeKey = string.Format("{0}Data", item.Alias);
                if (!typeList.ContainsKey(dataTypeKey))
                    continue;

                string viewTypeKey = string.Format("{0}View", item.Alias);
                if (!typeList.ContainsKey(viewTypeKey))
                    continue;

                writer.WriteLine("\tpublic partial class {0}", item.Alias);
                writer.WriteLine("\t{");
                this.WriteGetChangedDTO(writer, item, typeList[dataTypeKey]);
                this.WriteGetDeleteDTO(writer, item, typeList[dataTypeKey]);
                this.WriteDataDTOConvertToViewObj(writer, item, typeList[dataTypeKey]);
                this.WriteViewDTOConvertToViewObj(writer, item, typeList[viewTypeKey]);
                writer.WriteLine("\t}");
                writer.WriteLine();

                writer.WriteLine("\tpublic partial class {0}Collection", item.Alias);
                writer.WriteLine("\t{");
                this.WriteGetChangedDTOList(writer, item);
                this.WriteDataDTOConvertToViewObjList(writer, item);
                this.WriteViewDTOConvertToViewObjList(writer, item);
                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }

        private void WriteGetChangedDTO(StringWriter writer, ObjectSchema objSchema, Type dataType)
        {
            writer.WriteLine("\t\tpublic {0}DataDTO GetChangedDTO(string userId, DateTime date)", objSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif(!this.IsDirty)");
            writer.WriteLine("\t\t\t\treturn null;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t{0}DataDTO dto = new {0}DataDTO();", objSchema.Alias);
            PropertyInfo[] propertyInfoList = dataType.GetProperties();
            foreach (PropertyInfo info in propertyInfoList)
            {
                if (!info.CanWrite || !info.CanRead)
                    continue;

                if (info.PropertyType.IsValueType || info.PropertyType == typeof(string)
                    || info.PropertyType == typeof(byte[]))
                {
                    writer.WriteLine("\t\t\tdto.{0} = this._{1};", info.Name, base.LowerFirstLetter(info.Name));
                }
            }


            writer.WriteLine();
            writer.WriteLine("\t\t\tdto.IsNew = this.IsNew;");
            writer.WriteLine("\t\t\tdto.IsDeleted = this.IsDeleted;");
            writer.WriteLine("\t\t\tdto.IsSelfDirty = this.IsSelfDirty;");
            writer.WriteLine();
            writer.WriteLine("\t\t\tif(dto.IsNew)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tdto.CreatedOn = date;");
            writer.WriteLine("\t\t\t\tdto.CreatedBy = userId;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tdto.ModifiedOn = date;");
            writer.WriteLine("\t\t\tdto.ModifiedBy = userId;");
            writer.WriteLine();

            foreach (ChildSchema item in objSchema.Children)
            {
                writer.WriteLine("\t\t\tif (this.{0}List != null)", item.Alias);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tdto.{0}List = this.{0}List.GetChangedDTOList(userId, date);", item.Alias);
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();
            }

            writer.WriteLine("\t\t\treturn dto;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteGetChangedDTOList(StringWriter writer, ObjectSchema objSchema)
        {
            writer.WriteLine("\t\tpublic {0}DataDTOCollection GetChangedDTOList(string userId, DateTime date)", objSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}DataDTOCollection retList = new {0}DataDTOCollection();", objSchema.Alias);
            writer.WriteLine("\t\t\tforeach ({0} item in this._list)", objSchema.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t{0}DataDTO dto = item.GetChangedDTO(userId, date);", objSchema.Alias);
            writer.WriteLine("\t\t\t\tif(dto == null)");
            writer.WriteLine("\t\t\t\t\tcontinue;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tretList.Add(dto);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tforeach ({0} item in this._deletedList)", objSchema.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif(item.IsNew)");
            writer.WriteLine("\t\t\t\t\tcontinue;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tretList.Add(item.GetDeleteDTO());");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn retList;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteGetDeleteDTO(StringWriter writer, ObjectSchema objSchema, Type dataType)
        {
            writer.WriteLine("\t\tpublic {0}DataDTO GetDeleteDTO()", objSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}DataDTO dto = new {0}DataDTO();", objSchema.Alias);
            PropertyInfo[] propertyInfoList = dataType.GetProperties();
            foreach (PropertyInfo info in propertyInfoList)
            {
                if (!info.CanWrite || !info.CanRead)
                    continue;

                if (info.PropertyType.IsValueType || info.PropertyType == typeof(string)
                    || info.PropertyType == typeof(byte[]))
                {
                    writer.WriteLine("\t\t\tdto.{0} = this._{1};", info.Name, base.LowerFirstLetter(info.Name));
                }
            }


            writer.WriteLine();
            writer.WriteLine("\t\t\tdto.IsNew = false;");
            writer.WriteLine("\t\t\tdto.IsDeleted = true;");
            writer.WriteLine("\t\t\tdto.IsSelfDirty = true;");
            writer.WriteLine();

            writer.WriteLine("\t\t\treturn dto;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }


        private void WriteDataDTOConvertToViewObj(StringWriter writer, ObjectSchema objSchema, Type dataType)
        {
            writer.WriteLine("\t\tpublic {0}({0}DataDTO dto)", objSchema.Alias);
            writer.WriteLine("\t\t{");
            PropertyInfo[] propertyInfoList = dataType.GetProperties();
            foreach (PropertyInfo info in propertyInfoList)
            {
                if (!info.CanWrite || !info.CanRead)
                    continue;

                if (info.PropertyType.IsValueType || info.PropertyType == typeof(string)
                    || info.PropertyType == typeof(byte[]))
                {
                    writer.WriteLine("\t\t\tthis._{0} = dto.{1};", base.LowerFirstLetter(info.Name), info.Name);
                }
            }

            writer.WriteLine("\t\t\tif(dto.IsNew)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.MarkNew();");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\telse if(dto.IsDeleted)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.MarkDeleted();");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\telse if(dto.IsSelfDirty)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.MarkDirty();");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\telse");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.MarkOld();");
            writer.WriteLine("\t\t\t}");

            foreach (ChildSchema item in objSchema.Children)
            {
                writer.WriteLine("\t\t\tif(dto.{0}List != null && dto.{0}List.Count > 0)", item.Alias);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tthis.{0}List = new {0}Collection(dto.{0}List);", item.Alias);
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();
            }

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteViewDTOConvertToViewObj(StringWriter writer, ObjectSchema objSchema, Type viewType)
        {
            writer.WriteLine("\t\tpublic {0}({0}ViewDTO dto)", objSchema.Alias);
            writer.WriteLine("\t\t{");
            PropertyInfo[] propertyInfoList = viewType.GetProperties();
            foreach (PropertyInfo info in propertyInfoList)
            {
                if (!info.CanWrite || !info.CanRead)
                    continue;

                if (info.PropertyType.IsValueType || info.PropertyType == typeof(string)
                    || info.PropertyType == typeof(byte[]))
                {
                    writer.WriteLine("\t\t\tthis._{0} = dto.{1};", base.LowerFirstLetter(info.Name), info.Name);
                }
            }

            writer.WriteLine();
            writer.WriteLine("\t\t\tthis.MarkOld();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteDataDTOConvertToViewObjList(StringWriter writer, ObjectSchema objSchema)
        {
            writer.WriteLine("\t\tpublic {0}Collection({0}DataDTOCollection dtoList)", objSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis.BlockCount = dtoList.BlockCount;");
            writer.WriteLine("\t\t\tthis.BlockIndex = dtoList.BlockIndex;");
            writer.WriteLine("\t\t\tthis.BlockSize = dtoList.BlockSize;");
            writer.WriteLine("\t\t\tforeach ({0}DataDTO item in dtoList)", objSchema.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.Add(new {0}(item));", objSchema.Alias);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteViewDTOConvertToViewObjList(StringWriter writer, ObjectSchema objSchema)
        {
            writer.WriteLine("\t\tpublic {0}Collection({0}ViewDTOCollection dtoList)", objSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis.BlockCount = dtoList.BlockCount;");
            writer.WriteLine("\t\t\tthis.BlockIndex = dtoList.BlockIndex;");
            writer.WriteLine("\t\t\tthis.BlockSize = dtoList.BlockSize;");
            writer.WriteLine("\t\t\tforeach ({0}ViewDTO item in dtoList)", objSchema.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.Add(new {0}(item));", objSchema.Alias);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }
    }
}
