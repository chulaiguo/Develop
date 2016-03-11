using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWebAPI
{
    public class FactoryDTOConvert : FactoryBase
    {
        public FactoryDTOConvert(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using Cheke;");
            writer.WriteLine("using Cheke.EmailData;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using {0}.BizData;", base.ProjectName);
            writer.WriteLine("using {0}.Data;", base.ProjectName);
            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.DTO", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            this.WriteTokenDTO(writer);
            this.WriteResultDTO(writer);
            this.WriteDataDTO(writer);
            this.WriteBizDTO(writer);
            this.WriteEmailDTO(writer);
        }

        private void WriteTokenDTO(StringWriter writer)
        {
            writer.WriteLine("\tpublic partial class SecurityTokenDTO");
            writer.WriteLine("\t{");

            //Deserialize
            writer.WriteLine("\t\tpublic SecurityToken Deserialize()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tSecurityToken data = new SecurityToken(this.UserId, this.Password);");
            writer.WriteLine("\t\t\tif(this.PairKey != null && this.PairValue != null && this.PairKey.Length == this.PairValue.Length)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tfor (int i = 0; i < this.PairKey.Length; i++)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tdata.AddPairValue(this.PairKey[i], this.PairValue[i]);");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\treturn data;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate object GetParameter(int index)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (this.Parameters == null || index < 0 || index >= this.Parameters.Length)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn null;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn this.Parameters[index];");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic string GetParameterJson(int index)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tstring json = this.GetParameter(index).ToString();");
            writer.WriteLine("\t\t\tif (json.StartsWith(\"{\") || json.StartsWith(\"[\") || json.StartsWith(\"\\\"\"))");
            writer.WriteLine("\t\t\t\treturn json;");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn \"\\\"\" + json + \"\\\"\"; ");
            writer.WriteLine("\t\t}");

            writer.WriteLine("\t}");
            writer.WriteLine();
        }

        private void WriteResultDTO(StringWriter writer)
        {
            writer.WriteLine("\tpublic partial class ResultDTO");
            writer.WriteLine("\t{");

            //Serialize
            writer.WriteLine("\t\tpublic static ResultDTO Serialize(Result result)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tResultDTO dto = new ResultDTO();");
            writer.WriteLine("\t\t\tdto.OK = result.OK;");
            writer.WriteLine("\t\t\tdto.Error = result.ToString();");
            writer.WriteLine("\t\t\treturn dto;");
            writer.WriteLine("\t\t}");

            writer.WriteLine("\t}");
            writer.WriteLine();
        }

        private void WriteDataDTO(StringWriter writer)
        {
            string dllName = string.Format("{0}.Data.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> pair in typeList)
            {
                if (pair.Key.EndsWith("Collection"))
                    continue;

                writer.WriteLine("\tpublic partial class {0}DTO", pair.Key);
                writer.WriteLine("\t{");

                List<PropertyInfo> list = this.GetDataPropertyList(pair.Value);
               
                //Deserialize
                writer.WriteLine("\t\tpublic {0} Deserialize()", pair.Key);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\t{0} data = new {0}();", pair.Key);
                foreach (PropertyInfo field in list)
                {
                    if (!field.PropertyType.Name.EndsWith("DataCollection"))
                    {
                        writer.WriteLine("\t\t\tdata.{0} = this.{0};", field.Name);
                    }
                }

                if (pair.Key.EndsWith("Data"))
                {
                    writer.WriteLine("\t\t\tdata.AcceptChanges();");
                    writer.WriteLine("\t\t\tif(this.IsNew)");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tdata.MarkNew();");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine();
                    writer.WriteLine("\t\t\tif(this.IsDeleted)");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tdata.Delete();");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine();
                    writer.WriteLine("\t\t\tif(this.IsDirty)");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tdata.MarkDirty();");
                    writer.WriteLine("\t\t\t}");
                }

                foreach (PropertyInfo field in list)
                {
                    if (field.PropertyType.Name.EndsWith("DataCollection"))
                    {
                        writer.WriteLine("\t\t\tif(this.{0} != null)", field.Name);
                        writer.WriteLine("\t\t\t{");
                        writer.WriteLine("\t\t\t\tdata.{0} = new {1}();", field.Name, field.PropertyType.Name);
                        writer.WriteLine("\t\t\t\tfor (int i = 0; i < this.{0}.Length; i++)", field.Name);
                        writer.WriteLine("\t\t\t\t{");
                        writer.WriteLine("\t\t\t\t\tdata.{0}.Add(this.{0}[i].Deserialize());", field.Name);
                        writer.WriteLine("\t\t\t\t}");
                        writer.WriteLine("\t\t\t}");
                    }
                }
                writer.WriteLine("\t\t\treturn data;");
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                if (typeList.ContainsKey(string.Format("{0}Collection", pair.Key)))
                {
                    writer.WriteLine("\t\tpublic static {0}Collection Deserialize({0}DTO[] list)", pair.Key);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\t{0}Collection retList = new {0}Collection();", pair.Key);
                    writer.WriteLine("\t\t\tfor (int i = 0; i < list.Length; i++)");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tretList.Add(list[i].Deserialize());");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine("\t\t\treturn retList;");
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }

                //Serialize
                writer.WriteLine("\t\tpublic static {0}DTO Serialize({0} data)", pair.Key);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\t{0}DTO dto = new {0}DTO();", pair.Key);
                foreach (PropertyInfo field in list)
                {
                    if (!field.PropertyType.Name.EndsWith("DataCollection"))
                    {
                        writer.WriteLine("\t\t\tdto.{0} = data.{0};", field.Name);
                    }
                }

                if (pair.Key.EndsWith("Data"))
                {
                    writer.WriteLine("\t\t\tdto.ObjectID = data.ObjectID;");
                    writer.WriteLine("\t\t\tdto.IsNew = data.IsNew;");
                    writer.WriteLine("\t\t\tdto.IsDirty = data.IsDirty;");
                    writer.WriteLine("\t\t\tdto.IsDeleted = data.IsDeleted;");
                }

                foreach (PropertyInfo field in list)
                {
                    if (field.PropertyType.Name.EndsWith("DataCollection"))
                    {
                        string childrenTypeName = field.PropertyType.Name.Substring(0, field.PropertyType.Name.Length - "Collection".Length);

                        writer.WriteLine("\t\t\tif(data.{0} != null)", field.Name);
                        writer.WriteLine("\t\t\t{");
                        writer.WriteLine("\t\t\t\tdto.{0} = {1}DTO.Serialize(data.{0});", field.Name, childrenTypeName);
                        writer.WriteLine("\t\t\t}");
                    }
                }
                writer.WriteLine("\t\t\treturn dto;");
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                if (typeList.ContainsKey(string.Format("{0}Collection", pair.Key)))
                {
                    writer.WriteLine("\t\tpublic static {0}DTO[] Serialize({0}Collection list)", pair.Key);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\t{0}DTO[] retList = new {0}DTO[list.Count];", pair.Key);
                    writer.WriteLine("\t\t\tfor (int i = 0; i < list.Count; i++)");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tretList[i] = Serialize(list[i]);");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine("\t\t\treturn retList;");
                    writer.WriteLine("\t\t}");
                }

                writer.WriteLine("\t}");
            }

            writer.WriteLine();
        }

        private void WriteBizDTO(StringWriter writer)
        {
            string dllName = string.Format("{0}.BizData.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> pair in typeList)
            {
                if (pair.Key.EndsWith("Collection"))
                    continue;

                writer.WriteLine("\tpublic partial class {0}DTO", pair.Key);
                writer.WriteLine("\t{");

                List<PropertyInfo> list = this.GetBizPropertyList(pair.Value);
               
                //Serialize
                writer.WriteLine("\t\tpublic static {0}DTO Serialize({0} data)", pair.Key);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\t{0}DTO dto = new {0}DTO();", pair.Key);
                foreach (PropertyInfo field in list)
                {
                    writer.WriteLine("\t\t\tdto.{0} = data.{0};", field.Name);
                }
                writer.WriteLine("\t\t\treturn dto;");
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                if (typeList.ContainsKey(string.Format("{0}Collection", pair.Key)))
                {
                    writer.WriteLine("\t\tpublic static {0}DTO[] Serialize({0}Collection list)", pair.Key);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\t{0}DTO[] retList = new {0}DTO[list.Count];", pair.Key);
                    writer.WriteLine("\t\t\tfor (int i = 0; i < list.Count; i++)");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tretList[i] = Serialize(list[i]);");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine("\t\t\treturn retList;");
                    writer.WriteLine("\t\t}");
                }

                writer.WriteLine("\t}");
                writer.WriteLine();
            }

            writer.WriteLine();
        }

        private void WriteEmailDTO(StringWriter writer)
        {
            string dllName = "Cheke.EmailData.dll";
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            foreach (KeyValuePair<string, Type> pair in typeList)
            {
                if (pair.Key.EndsWith("Collection"))
                    continue;

                writer.WriteLine("\tpublic partial class {0}DTO", pair.Key);
                writer.WriteLine("\t{");

                List<PropertyInfo> list = this.GetEmailPropertyList(pair.Value);
              
                //Deserialize
                writer.WriteLine("\t\tpublic {0} Deserialize()", pair.Key);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\t{0} data = new {0}();", pair.Key);
                foreach (PropertyInfo field in list)
                {
                    if (field.PropertyType.Name.EndsWith("DataCollection"))
                    {
                        writer.WriteLine("\t\t\tif(this.{0} != null)", field.Name);
                        writer.WriteLine("\t\t\t{");
                        if (field.CanWrite)
                        {
                            writer.WriteLine("\t\t\t\tdata.{0} = new {1}[this.{0}.Length]", field.Name, field.PropertyType.Name);
                            writer.WriteLine("\t\t\t\tfor (int i = 0; i < this.{0}.Length; i++)", field.Name);
                            writer.WriteLine("\t\t\t\t{");
                            writer.WriteLine("\t\t\t\t\tdata.{0}[i] = this.{0}[i].Deserialize();", field.Name);
                            writer.WriteLine("\t\t\t\t}");
                        }
                        else
                        {
                            writer.WriteLine("\t\t\t\tfor (int i = 0; i < this.{0}.Length; i++)", field.Name);
                            writer.WriteLine("\t\t\t\t{");
                            writer.WriteLine("\t\t\t\t\tdata.{0}.Add(this.{0}[i].Deserialize());", field.Name);
                            writer.WriteLine("\t\t\t\t}");
                        }
                        writer.WriteLine("\t\t\t}");
                    }
                    else
                    {
                        writer.WriteLine("\t\t\tdata.{0} = this.{0};", field.Name);
                    }
                }
                writer.WriteLine("\t\t\treturn data;");
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
