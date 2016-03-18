using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryViewObj
{
    public class FactoryViewModel : FactoryBase
    {
        public FactoryViewModel(MappingSchema mappingSchema)
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
            this.WriteBusinessBase(writer);
            this.WriteDataObj(writer);
            this.WriteBizObj(writer);
            this.WriteEmailObj(writer);
        }

        private void WriteToken(StringWriter writer)
        {
            writer.WriteLine("\tpublic partial class SecurityToken");
            writer.WriteLine("\t{");

            writer.WriteLine("\t\tpublic SecurityToken(string userid, string password)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._userId = userid;");
            writer.WriteLine("\t\t\tthis._password = password;");
            writer.WriteLine("\t\t\tthis._secret = string.Empty;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic SecurityToken(string userid, string password, string secret)");
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
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate string _password = string.Empty;");
            writer.WriteLine("\t\tpublic string Password");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._password;}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate string _secret = string.Empty;");
            writer.WriteLine("\t\tpublic string Secret");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._secret;}");
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

            writer.WriteLine("\t}");
            writer.WriteLine();
        }

        private void WriteResult(StringWriter writer)
        {
            writer.WriteLine("\tpublic partial class Result");
            writer.WriteLine("\t{");

            writer.WriteLine("\t\tprivate bool _ok = false;");
            writer.WriteLine("\t\tpublic bool OK");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._ok;}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate string _error = string.Empty;");
            writer.WriteLine("\t\tpublic string Error");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._error;}");
            writer.WriteLine("\t\t}");

            writer.WriteLine("\t}");
            writer.WriteLine();
        }

        private void WriteBusinessBase(StringWriter writer)
        {
            writer.WriteLine("\tpublic partial class BusinessBase");
            writer.WriteLine("\t{");

            writer.WriteLine("\t\tpublic BusinessBase()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._isNew = true;");
            writer.WriteLine("\t\t\tthis._isDirty = true;");
            writer.WriteLine("\t\t\tthis._objectID = Guid.NewGuid();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate Guid _objectID = Guid.Empty;");
            writer.WriteLine("\t\tprivate bool _isNew = false;");
            writer.WriteLine("\t\tprivate bool _isDeleted = false;");
            writer.WriteLine("\t\tprivate bool _isDirty = false;");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic Guid ObjectID");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._objectID;}");
            writer.WriteLine("\t\t\tset { this._objectID = value;}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic bool IsNew");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._isNew;}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic bool IsDirty");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._isDirty;}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tpublic bool IsDeleted");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._isDeleted;}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic void MarkClean()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._isNew = false;");
            writer.WriteLine("\t\t\tthis._isDirty = false;");
            writer.WriteLine("\t\t\tthis._isDeleted = false;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic void AcceptChanges()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis.MarkClean();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic void MarkDirty()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._isDirty = true;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic void MarkNew()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._isNew = true;");
            writer.WriteLine("\t\t\tthis._isDirty = true;");
            writer.WriteLine("\t\t\tthis._isDeleted = false;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic void MarkDeleted()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._isNew = false;");
            writer.WriteLine("\t\t\tthis._isDirty = true;");
            writer.WriteLine("\t\t\tthis._isDeleted = true;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected void RaisePropertyChanged(string propertyName)");
            writer.WriteLine("\t\t{");
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
                if(pair.Key.EndsWith("Collection"))
                    continue;

                if (pair.Key.EndsWith("Data"))
                {
                    string className = pair.Key.Substring(0, pair.Key.Length - "Data".Length);
                    writer.WriteLine("\tpublic partial class {0} : BusinessBase", className);
                }
                else
                {
                    writer.WriteLine("\tpublic partial class {0}", pair.Key);
                }
                writer.WriteLine("\t{");

                List<PropertyInfo> list = this.GetDataPropertyList(pair.Value);
                foreach (PropertyInfo field in list)
                {
                    if (field.PropertyType.Name.EndsWith("DataCollection"))
                    {
                        string childrenTypeName = field.PropertyType.Name.Substring(0, field.PropertyType.Name.Length - "DataCollection".Length);
                        writer.WriteLine("\t\tprivate {0}[] _{1};", childrenTypeName, base.LowerFirstLetter(field.Name));
                    }
                    else
                    {
                        if (field.PropertyType == typeof (Guid))
                        {
                            if (pair.Key.EndsWith("Data") && field.Name == pair.Key.Substring(0, pair.Key.Length - "Data".Length) + "PK")
                            {
                                writer.WriteLine("\t\tprivate {0} _{1} = Guid.NewGuid();", field.PropertyType.FullName, base.LowerFirstLetter(field.Name));
                            }
                            else
                            {
                                writer.WriteLine("\t\tprivate {0} _{1} = Guid.Empty;", field.PropertyType.FullName, base.LowerFirstLetter(field.Name));
                            }
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
                        string childrenTypeName = field.PropertyType.Name.Substring(0, field.PropertyType.Name.Length - "DataCollection".Length);
                        writer.WriteLine("\t\tpublic {0}[] {1}", childrenTypeName, field.Name);
                        writer.WriteLine("\t\t{");
                        writer.WriteLine("\t\t\tget {{ return this._{0}; }}", base.LowerFirstLetter(field.Name));
                        writer.WriteLine("\t\t\tset {{ this._{0} = value; }}", base.LowerFirstLetter(field.Name));
                        writer.WriteLine("\t\t}");
                        writer.WriteLine();
                    }
                    else
                    {
                        if (pair.Key.EndsWith("Data"))
                        {
                            writer.WriteLine("\t\tpublic {0} {1}", field.PropertyType.FullName, field.Name);
                            writer.WriteLine("\t\t{");
                            writer.WriteLine("\t\t\tget {{ return this._{0}; }}", base.LowerFirstLetter(field.Name));
                            writer.WriteLine("\t\t\tset");
                            writer.WriteLine("\t\t\t{");
                            writer.WriteLine("\t\t\t\tthis._{0} = value;", base.LowerFirstLetter(field.Name));
                            writer.WriteLine("\t\t\t\tthis.MarkDirty();");
                            writer.WriteLine("\t\t\t\tthis.RaisePropertyChanged(\"{0}\");", field.Name);
                            writer.WriteLine("\t\t\t}");
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
                foreach (PropertyInfo field in list)
                {
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
                    writer.WriteLine("\t\tpublic {0} {1}", field.PropertyType.FullName, field.Name);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tget {{ return this._{0}; }}", base.LowerFirstLetter(field.Name));
                    writer.WriteLine("\t\t\tset {{ this._{0} = value; }}", base.LowerFirstLetter(field.Name));
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }

                writer.WriteLine("\t}");
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
                foreach (PropertyInfo field in list)
                {
                    if (field.PropertyType.Name.EndsWith("DataCollection"))
                    {
                        string childrenTypeName = field.PropertyType.Name.Substring(0, field.PropertyType.Name.Length - "DataCollection".Length);
                        writer.WriteLine("\t\tprivate {0}[] _{1};", childrenTypeName, base.LowerFirstLetter(field.Name));
                    }
                    else
                    {
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
                }
                writer.WriteLine();

                foreach (PropertyInfo field in list)
                {
                    if (field.PropertyType.Name.EndsWith("DataCollection"))
                    {
                        string childrenTypeName = field.PropertyType.Name.Substring(0, field.PropertyType.Name.Length - "DataCollection".Length);
                        writer.WriteLine("\t\tpublic {0}[] {1}", childrenTypeName, field.Name);
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
