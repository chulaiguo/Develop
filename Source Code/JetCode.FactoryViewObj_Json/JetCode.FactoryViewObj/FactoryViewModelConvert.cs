using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryViewObj
{
    public class FactoryViewModelConvert : FactoryBase
    {
        public FactoryViewModelConvert(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using {0}.DTO;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.ViewModel", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            this.WriteToken(writer);
            this.WriteResult(writer);
            this.WriteDataObj(writer);
            this.WriteBizObj(writer);
            this.WriteEmailObj(writer);
        }

        private void WriteToken(StringWriter writer)
        {
            writer.WriteLine("\tpublic partial class SecurityToken");
            writer.WriteLine("\t{");

            //Serialize
            writer.WriteLine("\t\tpublic SecurityTokenDTO Serialize()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tSecurityTokenDTO dto = new SecurityTokenDTO(this.UserId, this.Password);");
            writer.WriteLine("\t\t\tif(this.PairKey != null && this.PairValue != null && this.PairKey.Length == this.PairValue.Length)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tdto.PairKey = new string[this.PairKey.Length];");
            writer.WriteLine("\t\t\t\tdto.PairValue = new string[this.PairValue.Length];");
            writer.WriteLine("\t\t\t\tfor (int i = 0; i < this.PairKey.Length; i++)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tdto.PairKey[i] = this.PairKey[i];");
            writer.WriteLine("\t\t\t\t\tdto.PairValue[i] = this.PairValue[i];");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\treturn dto;");
            writer.WriteLine("\t\t}");

            writer.WriteLine("\t}");
            writer.WriteLine();
        }

        private void WriteResult(StringWriter writer)
        {
            writer.WriteLine("\tpublic partial class Result");
            writer.WriteLine("\t{");

            //Deserialize
            writer.WriteLine("\t\tpublic Result(ResultDTO dto)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._ok = dto.OK;");
            writer.WriteLine("\t\t\tthis._error = dto.Error;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t}");
            writer.WriteLine();
        }

        private void WriteDataObj(StringWriter writer)
        {
            string dllName = string.Format("{0}.Data.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> pair in typeList)
            {
                if (pair.Key.EndsWith("Collection"))
                    continue;

                string className = pair.Key;
                if (pair.Key.EndsWith("Data"))
                {
                    className = pair.Key.Substring(0, pair.Key.Length - "Data".Length);
                }

                writer.WriteLine("\tpublic partial class {0}", className);
                writer.WriteLine("\t{");

                List<PropertyInfo> list = this.GetDataPropertyList(pair.Value);

                //Deserialize
                writer.WriteLine("\t\tpublic {0}({1}DTO dto)", className, pair.Key);
                writer.WriteLine("\t\t{");
                foreach (PropertyInfo field in list)
                {
                    if (!field.PropertyType.Name.EndsWith("DataCollection"))
                    {
                        writer.WriteLine("\t\t\tthis.{0} = dto.{0};", field.Name);
                    }
                }

                if (pair.Key.EndsWith("Data"))
                {
                    writer.WriteLine("\t\t\tthis.AcceptChanges();");
                    writer.WriteLine("\t\t\tif(dto.IsNew)");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tthis.MarkNew();");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine();
                    writer.WriteLine("\t\t\tif(dto.IsDeleted)");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tthis.MarkDeleted();");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine();
                    writer.WriteLine("\t\t\tif(dto.IsDirty)");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tthis.MarkDirty();");
                    writer.WriteLine("\t\t\t}");
                }

                foreach (PropertyInfo field in list)
                {
                    if (field.PropertyType.Name.EndsWith("DataCollection"))
                    {
                        string childrenName = field.PropertyType.Name.Substring(0, field.PropertyType.Name.Length - "DataCollection".Length);
                        writer.WriteLine("\t\t\tif(dto.{0} != null)", field.Name);
                        writer.WriteLine("\t\t\t{");
                        writer.WriteLine("\t\t\t\tthis.{0} = {1}.Deserialize(dto.{0});", field.Name, childrenName);
                        writer.WriteLine("\t\t\t}");
                    }
                }
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                if (typeList.ContainsKey(string.Format("{0}Collection", pair.Key)))
                {
                    writer.WriteLine("\t\tpublic static {0}[] Deserialize({1}DTO[] list)", className, pair.Key);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\t{0}[] retList = new {0}[list.Length];", className);
                    writer.WriteLine("\t\t\tfor (int i = 0; i < list.Length; i++)");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tretList[i] = new {0}(list[i]);", className);
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine("\t\t\treturn retList;");
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }

                //Serialize
                writer.WriteLine("\t\tpublic {0}DTO Serialize()", pair.Key);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\t{0}DTO dto = new {0}DTO();", pair.Key);
                foreach (PropertyInfo field in list)
                {
                    if (!field.PropertyType.Name.EndsWith("DataCollection"))
                    {
                        writer.WriteLine("\t\t\tdto.{0} = this.{0};", field.Name);
                    }
                }

                if (pair.Key.EndsWith("Data"))
                {
                    writer.WriteLine("\t\t\tdto.ObjectID = this.ObjectID;");
                    writer.WriteLine("\t\t\tdto.IsNew = this.IsNew;");
                    writer.WriteLine("\t\t\tdto.IsDirty = this.IsDirty;");
                    writer.WriteLine("\t\t\tdto.IsDeleted = this.IsDeleted;");
                }

                foreach (PropertyInfo field in list)
                {
                    if (field.PropertyType.Name.EndsWith("DataCollection"))
                    {
                        string childrenTypeName = field.PropertyType.Name.Substring(0, field.PropertyType.Name.Length - "DataCollection".Length);

                        writer.WriteLine("\t\t\tif(this.{0} != null)", field.Name);
                        writer.WriteLine("\t\t\t{");
                        writer.WriteLine("\t\t\t\tdto.{0} = {1}.Serialize(this.{0});", field.Name, childrenTypeName);
                        writer.WriteLine("\t\t\t}");
                    }
                }
                writer.WriteLine("\t\t\treturn dto;");
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                if (typeList.ContainsKey(string.Format("{0}Collection", pair.Key)))
                {
                    writer.WriteLine("\t\tpublic static {0}DTO[] Serialize({1}[] list)", pair.Key, className);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\t{0}DTO[] retList = new {0}DTO[list.Length];", pair.Key);
                    writer.WriteLine("\t\t\tfor (int i = 0; i < list.Length; i++)");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tretList[i] = list[i].Serialize();");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine("\t\t\treturn retList;");
                    writer.WriteLine("\t\t}");
                }

                writer.WriteLine("\t}");
            }

            writer.WriteLine();
        }

        private void WriteBizObj(StringWriter writer)
        {
            string dllName = string.Format("{0}.BizData.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> pair in typeList)
            {
                if (pair.Key.EndsWith("Collection"))
                    continue;

                writer.WriteLine("\tpublic partial class {0}", pair.Key);
                writer.WriteLine("\t{");

                List<PropertyInfo> list = this.GetBizPropertyList(pair.Value);

                //Deserialize
                writer.WriteLine("\t\tpublic {0}({0}DTO dto)", pair.Key);
                writer.WriteLine("\t\t{");
                foreach (PropertyInfo field in list)
                {
                    writer.WriteLine("\t\t\tthis.{0} = dto.{0};", field.Name);
                }
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                if (typeList.ContainsKey(string.Format("{0}Collection", pair.Key)))
                {
                    writer.WriteLine("\t\tpublic static {0}[] Deserialize({0}DTO[] list)", pair.Key);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\t{0}[] retList = new {0}[list.Length];", pair.Key);
                    writer.WriteLine("\t\t\tfor (int i = 0; i < list.Length; i++)");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tretList[i] = new {0}(list[i]);", pair.Key);
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine("\t\t\treturn retList;");
                    writer.WriteLine("\t\t}");
                }

                writer.WriteLine("\t}");
                writer.WriteLine();
            }

            writer.WriteLine();
        }

        private void WriteEmailObj(StringWriter writer)
        {
            string dllName = "Cheke.EmailData.dll";
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> pair in typeList)
            {
                if (pair.Key.EndsWith("Collection"))
                    continue;

                string className = pair.Key.Substring(0, pair.Key.Length - "Data".Length);
                writer.WriteLine("\tpublic partial class {0}", className);
                writer.WriteLine("\t{");

                List<PropertyInfo> list = this.GetEmailPropertyList(pair.Value);

                //Serialize
                writer.WriteLine("\t\tpublic {0}DTO Serialize()", pair.Key);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\t{0}DTO dto = new {0}DTO();", pair.Key);
                foreach (PropertyInfo field in list)
                {
                    if (field.PropertyType.Name.EndsWith("DataCollection"))
                    {
                        string childrenTypeName = field.PropertyType.Name.Substring(0, field.PropertyType.Name.Length - "Collection".Length);

                        writer.WriteLine("\t\t\tif(this.{0} != null)", field.Name);
                        writer.WriteLine("\t\t\t{");
                        writer.WriteLine("\t\t\t\tdto.{0} = new {1}DTO[this.{0}.Length];", field.Name, childrenTypeName);
                        writer.WriteLine("\t\t\t\tfor (int i = 0; i < this.{0}.Length; i++)", field.Name);
                        writer.WriteLine("\t\t\t\t{");
                        writer.WriteLine("\t\t\t\t\tdto.{0}[i] = this.{0}[i].Serialize();", field.Name);
                        writer.WriteLine("\t\t\t\t}");
                        writer.WriteLine("\t\t\t}");
                    }
                    else
                    {
                        writer.WriteLine("\t\t\tdto.{0} = this.{0};", field.Name);
                    }
                }
                writer.WriteLine("\t\t\treturn dto;");
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                writer.WriteLine("\t}");
            }

            writer.WriteLine();
        }

        private List<PropertyInfo> GetDataPropertyList(Type type)
        {
            List<PropertyInfo> retList = new List<PropertyInfo>();

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
            foreach (PropertyInfo info in properties)
            {
                if (info.Name == "IsDirty" || info.Name == "IsValid" || info.Name == "PKString"
                        || info.Name == "MarkAsDeleted" || info.Name == "TableName")
                    continue;

                if (!info.CanWrite || !info.CanRead)
                    continue;

                if (info.PropertyType.IsValueType || info.PropertyType == typeof(string) || info.PropertyType == typeof(byte[]))
                {
                    retList.Add(info);
                }

                if (info.PropertyType.Name.EndsWith("DataCollection"))
                {
                    retList.Add(info);
                }
            }

            return retList;
        }

        private List<PropertyInfo> GetBizPropertyList(Type type)
        {
            List<PropertyInfo> retList = new List<PropertyInfo>();

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
            foreach (PropertyInfo info in properties)
            {
                if (!info.CanWrite || !info.CanRead)
                    continue;

                if (info.PropertyType.IsValueType || info.PropertyType == typeof(string) || info.PropertyType == typeof(byte[]))
                {
                    retList.Add(info);
                }
            }

            return retList;
        }

        private List<PropertyInfo> GetEmailPropertyList(Type type)
        {
            List<PropertyInfo> retList = new List<PropertyInfo>();

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
            foreach (PropertyInfo info in properties)
            {
                if (info.PropertyType.IsValueType || info.PropertyType == typeof(string) || info.PropertyType == typeof(byte[]))
                {
                    retList.Add(info);
                }

                if (info.PropertyType.Name.EndsWith("DataCollection"))
                {
                    retList.Add(info);
                }
            }

            return retList;
        }
    }
}
