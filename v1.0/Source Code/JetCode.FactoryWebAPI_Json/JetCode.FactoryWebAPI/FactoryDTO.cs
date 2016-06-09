using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWebAPI
{
    public class FactoryDTO : FactoryBase
    {
        public FactoryDTO(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
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
        }

        private void WriteTokenDTO(StringWriter writer)
        {
            writer.WriteLine("\tpublic partial class SecurityTokenDTO");
            writer.WriteLine("\t{");

            writer.WriteLine("\t\tpublic SecurityTokenDTO()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tpublic SecurityTokenDTO(string userid, string password)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._userId = userid;");
            writer.WriteLine("\t\t\tthis._password = password;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tpublic SecurityTokenDTO(string userid, string password, string secret)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._userId = userid;");
            writer.WriteLine("\t\t\tthis._password = password;");
            writer.WriteLine("\t\t\tthis._secret = secret;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate string _userId = string.Empty;");
            writer.WriteLine("\t\tpublic string UserId");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._userId;}");
            writer.WriteLine("\t\t\tset { this._userId = value;}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate string _password = string.Empty;");
            writer.WriteLine("\t\tpublic string Password");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._password;}");
            writer.WriteLine("\t\t\tset { this._password = value;}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate string _secret = string.Empty;");
            writer.WriteLine("\t\tpublic string Secret");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._secret;}");
            writer.WriteLine("\t\t\tset { this._secret = value;}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate string[] _pairKey = null;");
            writer.WriteLine("\t\tpublic string[] PairKey");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._pairKey;}");
            writer.WriteLine("\t\t\tset { this._pairKey = value;}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate string[] _pairValue = null;");
            writer.WriteLine("\t\tpublic string[] PairValue");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._pairValue;}");
            writer.WriteLine("\t\t\tset { this._pairValue = value;}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate string _parameterNames = string.Empty;");
            writer.WriteLine("\t\tpublic string ParameterNames");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._parameterNames;}");
            writer.WriteLine("\t\t\tset { this._parameterNames = value;}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate object[] _parameters = null;");
            writer.WriteLine("\t\tpublic object[] Parameters");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._parameters;}");
            writer.WriteLine("\t\t\tset { this._parameters = value;}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            //Deserialize
            writer.WriteLine("\t\tpublic void AttachParameters(string[] paraNames, object[] paraValues)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif(paraNames == null || paraValues == null || paraNames.Length != paraValues.Length)");
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine();
            writer.WriteLine("\t\t\tthis.Parameters = new object[paraValues.Length];");
            writer.WriteLine("\t\t\tfor (int i = 0; i < paraValues.Length; i++)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.Parameters[i] = paraValues[i];");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tif (i == 0)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tthis.ParameterNames = string.Format(\"{0}\", paraNames[i]);");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\telse");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tthis.ParameterNames = string.Format(\"{0}|{1}\", this.ParameterNames, paraNames[i]);");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");

            writer.WriteLine("\t}");
            writer.WriteLine();
        }

        private void WriteResultDTO(StringWriter writer)
        {
            writer.WriteLine("\tpublic partial class ResultDTO");
            writer.WriteLine("\t{");

            writer.WriteLine("\t\tprivate bool _ok = false;");
            writer.WriteLine("\t\tpublic bool OK");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._ok;}");
            writer.WriteLine("\t\t\tset { this._ok = value;}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate string _error = string.Empty;");
            writer.WriteLine("\t\tpublic string Error");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._error;}");
            writer.WriteLine("\t\t\tset { this._error = value;}");
            writer.WriteLine("\t\t}");
            
            writer.WriteLine("\t}");
            writer.WriteLine();
        }

        private void WriteDataDTO(StringWriter writer)
        {
            SortedList<string, Type> typeList = Utils.GetDataTypeList(base.ProjectName);
            foreach (KeyValuePair<string, Type> pair in typeList)
            {
                if(pair.Key.EndsWith("Collection"))
                    continue;

                writer.WriteLine("\tpublic partial class {0}DTO", pair.Key);
                writer.WriteLine("\t{");

                if (pair.Key.EndsWith("Data"))
                {
                    writer.WriteLine("\t\tprivate Guid _objectID = Guid.Empty;");
                    writer.WriteLine("\t\tprivate bool _isNew = false;");
                    writer.WriteLine("\t\tprivate bool _isDeleted = false;");
                    writer.WriteLine("\t\tprivate bool _isDirty = false;");
                }

                List<PropertyInfo> list = this.GetDataPropertyList(pair.Value);
                foreach (PropertyInfo field in list)
                {
                    if (field.PropertyType.Name.EndsWith("DataCollection"))
                    {
                        string childrenTypeName = field.PropertyType.Name.Substring(0, field.PropertyType.Name.Length - "Collection".Length);
                        writer.WriteLine("\t\tprivate {0}DTO[] _{1};", childrenTypeName, base.LowerFirstLetter(field.Name));
                    }
                    else
                    {
                        if (field.PropertyType == typeof (Guid))
                        {
                            writer.WriteLine("\t\tprivate {0} _{1} = Guid.Empty;", field.PropertyType.FullName, base.LowerFirstLetter(field.Name));
                        }
                        else if (field.PropertyType == typeof(DateTime))
                        {
                            writer.WriteLine("\t\tprivate {0} _{1} = DateTime.Now;", field.PropertyType.FullName, base.LowerFirstLetter(field.Name));
                        }
                        else if (field.PropertyType == typeof(string))
                        {
                            writer.WriteLine("\t\tprivate {0} _{1} = string.Empty;", field.PropertyType.FullName, base.LowerFirstLetter(field.Name));
                        }
                        else
                        {
                            writer.WriteLine("\t\tprivate {0} _{1};", field.PropertyType.FullName, base.LowerFirstLetter(field.Name));
                        }
                    }
                }
                writer.WriteLine();

                foreach (PropertyInfo field in list)
                {
                    if (field.PropertyType.Name.EndsWith("DataCollection"))
                    {
                        string childrenTypeName = field.PropertyType.Name.Substring(0, field.PropertyType.Name.Length - "Collection".Length);
                        writer.WriteLine("\t\tpublic {0}DTO[] {1}", childrenTypeName, field.Name);
                        writer.WriteLine("\t\t{");
                        writer.WriteLine("\t\t\tget {{ return this._{0}; }}", base.LowerFirstLetter(field.Name));
                        writer.WriteLine("\t\t\tset {{ this._{0} = value; }}", base.LowerFirstLetter(field.Name));
                        writer.WriteLine("\t\t}");
                        writer.WriteLine();
                    }
                    else
                    {
                        writer.WriteLine("\t\tpublic {0} {1}", field.PropertyType.FullName, field.Name);
                        writer.WriteLine("\t\t{");
                        writer.WriteLine("\t\t\tget {{ return this._{0}; }}", base.LowerFirstLetter(field.Name));
                        writer.WriteLine("\t\t\tset {{ this._{0} = value; }}", base.LowerFirstLetter(field.Name));
                        writer.WriteLine("\t\t}");
                        writer.WriteLine();
                    }
                }

                if (pair.Key.EndsWith("Data"))
                {
                    writer.WriteLine("\t\tpublic Guid ObjectID");
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tget { return this._objectID;}");
                    writer.WriteLine("\t\t\tset { this._objectID = value;}");
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();

                    writer.WriteLine("\t\tpublic bool IsNew");
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tget { return this._isNew;}");
                    writer.WriteLine("\t\t\tset { this._isNew = value;}");
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();

                    writer.WriteLine("\t\tpublic bool IsDirty");
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tget { return this._isDirty;}");
                    writer.WriteLine("\t\t\tset { this._isDirty = value;}");
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                    writer.WriteLine("\t\tpublic bool IsDeleted");
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tget { return this._isDeleted;}");
                    writer.WriteLine("\t\t\tset { this._isDeleted = value;}");
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }

                writer.WriteLine("\t}");
            }

            writer.WriteLine();
        }

        private void WriteBizDTO(StringWriter writer)
        {
            SortedList<string, Type> typeList = Utils.GetBizDataTypeList(base.ProjectName);
            foreach (KeyValuePair<string, Type> pair in typeList)
            {
                if (pair.Key.EndsWith("Collection"))
                    continue;

                writer.WriteLine("\tpublic partial class {0}DTO", pair.Key);
                writer.WriteLine("\t{");

                List<PropertyInfo> list = this.GetBizPropertyList(pair.Value);
                foreach (PropertyInfo field in list)
                {
                    if (field.PropertyType.Name.EndsWith("Collection"))
                    {
                        string childrenTypeName = field.PropertyType.Name.Substring(0, field.PropertyType.Name.Length - "Collection".Length);
                        writer.WriteLine("\t\tprivate {0}DTO[] _{1};", childrenTypeName, base.LowerFirstLetter(field.Name));
                        continue;
                    }

                    if (field.PropertyType == typeof(Guid))
                    {
                        writer.WriteLine("\t\tprivate {0} _{1} = Guid.Empty;", field.PropertyType.FullName, base.LowerFirstLetter(field.Name));
                    }
                    else if (field.PropertyType == typeof(DateTime))
                    {
                        writer.WriteLine("\t\tprivate {0} _{1} = DateTime.Now;", field.PropertyType.FullName, base.LowerFirstLetter(field.Name));
                    }
                    else if (field.PropertyType == typeof(string))
                    {
                        writer.WriteLine("\t\tprivate {0} _{1} = string.Empty;", field.PropertyType.FullName, base.LowerFirstLetter(field.Name));
                    }
                    else
                    {
                        writer.WriteLine("\t\tprivate {0} _{1};", field.PropertyType.FullName, base.LowerFirstLetter(field.Name));
                    }
                }
                writer.WriteLine();

                foreach (PropertyInfo field in list)
                {
                    if (field.PropertyType.Name.EndsWith("Collection"))
                    {
                        string childrenTypeName = field.PropertyType.Name.Substring(0,
                            field.PropertyType.Name.Length - "Collection".Length);
                        writer.WriteLine("\t\tpublic {0}DTO[] {1}", childrenTypeName, field.Name);
                        writer.WriteLine("\t\t{");
                        writer.WriteLine("\t\t\tget {{ return this._{0}; }}", base.LowerFirstLetter(field.Name));
                        writer.WriteLine("\t\t\tset {{ this._{0} = value; }}", base.LowerFirstLetter(field.Name));
                        writer.WriteLine("\t\t}");
                        writer.WriteLine();
                    }
                    else
                    {
                        writer.WriteLine("\t\tpublic {0} {1}", field.PropertyType.FullName, field.Name);
                        writer.WriteLine("\t\t{");
                        writer.WriteLine("\t\t\tget {{ return this._{0}; }}", base.LowerFirstLetter(field.Name));
                        writer.WriteLine("\t\t\tset {{ this._{0} = value; }}", base.LowerFirstLetter(field.Name));
                        writer.WriteLine("\t\t}");
                        writer.WriteLine();
                    }
                }

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
                if (!info.CanRead)
                    continue;

                if (!info.CanWrite)
                {
                    if (info.PropertyType.Name.EndsWith("Collection"))
                    {
                        retList.Add(info);
                    }

                    continue;
                }

                if (info.PropertyType.IsValueType || info.PropertyType == typeof(string) || info.PropertyType == typeof(byte[]))
                {
                    retList.Add(info);
                }
            }

            return retList;
        }
    }
}
