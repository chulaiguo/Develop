using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWCF
{
    public class FactoryWCFFacadeConverter : FactoryBase
    {
        public FactoryWCFFacadeConverter(MappingSchema mappingSchema)
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
            writer.WriteLine("using {0}.BizData;", base.ProjectName);
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
            string dllName = string.Format("{0}.BizData.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> pair in typeList)
            {
                if(pair.Key.EndsWith("Collection"))
                    continue;

                writer.WriteLine("\tpublic static class {0}Converter", pair.Key);
                writer.WriteLine("\t{");

                this.WriteConvertToBiz(writer, pair.Value);
                writer.WriteLine();
                this.WriteConvertToDTO(writer, pair.Value);

                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }

        private void WriteConvertToBiz(StringWriter writer, Type bizType)
        {
            writer.WriteLine("\t\tpublic static {0} ConvertToBiz({0}DTO dto)", bizType.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0} data = new {0}();", bizType.Name);

            PropertyInfo[] propertyInfoList = bizType.GetProperties();
            foreach (PropertyInfo info in propertyInfoList)
            {
                if (!info.CanWrite || !info.CanRead)
                    continue;

                 writer.WriteLine("\t\t\tdata.{0} = dto.{0};", info.Name);
            }

            writer.WriteLine("\t\t\treturn data;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic static {0}Collection ConvertToBizList({0}DTOCollection dtoList)", bizType.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}Collection retList = new {0}Collection();", bizType.Name);
            writer.WriteLine("\t\t\tforeach ({0}DTO item in dtoList)", bizType.Name);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t{0} entity = {0}Converter.ConvertToBiz(item);", bizType.Name);
            writer.WriteLine("\t\t\t\tretList.Add(entity);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn retList;");
            writer.WriteLine("\t\t}");
        }

        private void WriteConvertToDTO(StringWriter writer, Type bizType)
        {
            writer.WriteLine("\t\tpublic static {0}DTO ConvertToDTO({0} data)", bizType.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}DTO dto = new {0}DTO();", bizType.Name);
            PropertyInfo[] propertyInfoList = bizType.GetProperties();
            foreach (PropertyInfo info in propertyInfoList)
            {
                if (!info.CanWrite || !info.CanRead)
                    continue;

                writer.WriteLine("\t\t\tdto.{0} = data.{0};", info.Name);
            }

            writer.WriteLine();
            writer.WriteLine("\t\t\treturn dto;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic static {0}DTOCollection ConvertToDTOList({0}Collection dataList)", bizType.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}DTOCollection retList = new {0}DTOCollection();", bizType.Name);
            writer.WriteLine("\t\t\tforeach ({0} item in dataList)", bizType.Name);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tretList.Add({0}Converter.ConvertToDTO(item));", bizType.Name);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn retList;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }
    }
}
