using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryData
{
    public class FactoryDataView : FactoryBase
    {
        public FactoryDataView(MappingSchema mappingSchema, ObjectSchema objectSchema) 
            : base(mappingSchema, objectSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Data", this.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\t[Serializable]");
            writer.WriteLine("\tpublic partial class {0}View", base.ObjectSchema.Alias);
            writer.WriteLine("\t{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            this.WriteFields(writer);
            this.WriteProperties(writer);
            this.WriteConstruct(writer);
        }

        private void WriteFields(StringWriter writer)
        {
            Dictionary<PropertyInfo, bool> list = this.GetLocalAndJoinFields();
            foreach (KeyValuePair<PropertyInfo, bool> item in list)
            {
                if(item.Key.PropertyType == typeof(byte[]))
                    continue;

                string fieldName = string.Format("_{0}", base.LowerFirstLetter(item.Key.Name));
                Type fieldType = item.Key.PropertyType;
                bool isNullable = item.Value;

                //Fields
                if (fieldType == typeof(Guid))
                {
                    if (!item.Value)
                    {
                        writer.WriteLine("\t\tprivate {0} {1} = Guid.Empty;", fieldType, fieldName);
                    }
                    else
                    {
                        writer.WriteLine("\t\t//private {0} {1} = Guid.Empty;", fieldType, fieldName);
                    }
                }
                else if (fieldType == typeof(string))
                {
                    if (!isNullable)
                    {
                        writer.WriteLine("\t\tprivate {0} {1} = string.Empty;", fieldType, fieldName);
                    }
                    else
                    {
                        writer.WriteLine("\t\t//private {0} {1} = string.Empty;", fieldType, fieldName);
                    }
                }
                else if (fieldType == typeof(DateTime))
                {
                    if (!isNullable)
                    {
                        writer.WriteLine("\t\tprivate {0} {1} = new DateTime(1900, 1, 1);", fieldType, fieldName);
                    }
                    else
                    {
                        writer.WriteLine("\t\t//private {0} {1} = new DateTime(1900, 1, 1);", fieldType, fieldName);
                    }
                }
                else if (fieldType == typeof(bool))
                {
                    if (!isNullable)
                    {
                        writer.WriteLine("\t\tprivate {0} {1} = false;", fieldType, fieldName);
                    }
                    else
                    {
                        writer.WriteLine("\t\t//private {0} {1} = false;", fieldType, fieldName);
                    }
                }
                else
                {
                    if (!isNullable)
                    {
                        writer.WriteLine("\t\tprivate {0} {1} = 0;", fieldType, fieldName);
                    }
                    else
                    {
                        writer.WriteLine("\t\t//private {0} {1} = 0;", fieldType, fieldName);
                    }
                }
            }
            writer.WriteLine();
        }

        private void WriteProperties(StringWriter writer)
        {
            Dictionary<PropertyInfo, bool> list = this.GetLocalAndJoinFields();
            foreach (KeyValuePair<PropertyInfo, bool> item in list)
            {
                if (item.Key.PropertyType == typeof(byte[]))
                    continue;

                string fieldName = string.Format("_{0}", base.LowerFirstLetter(item.Key.Name));
                Type propertyType = item.Key.PropertyType;
                bool isNullable = item.Value;

                if (!isNullable)
                {
                    if(propertyType == typeof(DateTime))
                    {
                        writer.WriteLine("\t\tpublic {0} {1}", propertyType, item.Key.Name);
                        writer.WriteLine("\t\t{");
                        writer.WriteLine("\t\t\tget{{ return this.{0}.ToLocalTime(); }}", fieldName);
                        writer.WriteLine("\t\t\tset{{ this.{0} = value.ToUniversalTime(); }}", fieldName);
                        writer.WriteLine("\t\t}");
                    }
                    else
                    {
                        writer.WriteLine("\t\tpublic {0} {1}", propertyType, item.Key.Name);
                        writer.WriteLine("\t\t{");
                        writer.WriteLine("\t\t\tget{{ return this.{0}; }}", fieldName);
                        writer.WriteLine("\t\t\tset{{ this.{0} = value; }}", fieldName);
                        writer.WriteLine("\t\t}");
                    }
                }
                else
                {
                    if (propertyType == typeof(DateTime))
                    {
                        writer.WriteLine("//\t\tpublic {0} {1}", propertyType, item.Key.Name);
                        writer.WriteLine("//\t\t{");
                        writer.WriteLine("//\t\t\tget{{ return this.{0}.ToLocalTime(); }}", fieldName);
                        writer.WriteLine("//\t\t\tset{{ this.{0} = value.ToUniversalTime(); }}", fieldName);
                        writer.WriteLine("//\t\t}");
                    }
                    else
                    {
                        writer.WriteLine("//\t\tpublic {0} {1}", propertyType, item.Key.Name);
                        writer.WriteLine("//\t\t{");
                        writer.WriteLine("//\t\t\tget{{ return this.{0}; }}", fieldName);
                        writer.WriteLine("//\t\t\tset{{ this.{0} = value; }}", fieldName);
                        writer.WriteLine("//\t\t}");
                    }
                }
                writer.WriteLine();
            }
        }

        private void WriteConstruct(StringWriter writer)
        {
            writer.WriteLine("\t\tpublic {0}View()", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0}View({0}Data data)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            Dictionary<PropertyInfo, bool> list = this.GetLocalAndJoinFields();
            foreach (KeyValuePair<PropertyInfo, bool> item in list)
            {
                if (item.Key.PropertyType == typeof(byte[]))
                    continue;

                bool isNullable = item.Value;

                if (!isNullable)
                {
                    writer.WriteLine("\t\t\tthis.{0} = data.{0};", item.Key.Name);
                }
                else
                {
                    writer.WriteLine("\t\t\t//this.{0} = data.{0};", item.Key.Name);
                }
            }
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0}Data To{0}Data()", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}Data entity = new {0}Data();", base.ObjectSchema.Alias);
            foreach (KeyValuePair<PropertyInfo, bool> item in list)
            {
                if (item.Key.PropertyType == typeof(byte[]))
                    continue;

                bool isNullable = item.Value;

                if (!isNullable)
                {
                    writer.WriteLine("\t\t\tentity.{0} = this.{0};", item.Key.Name);
                }
                else
                {
                    writer.WriteLine("\t\t\t//entity.{0} = this.{0};", item.Key.Name);
                }
            }
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn entity;");
            writer.WriteLine("\t\t}");
        }

        #region WriteJoin Fileds

        private Dictionary<PropertyInfo, bool> GetLocalAndJoinFields()
        {
            Dictionary<PropertyInfo, bool> retList = new Dictionary<PropertyInfo, bool>();
            Type type = Utils.GetDataType(this.MappingSchema, this.ObjectSchema);
            if (type == null)
                return retList;

            SortedList<string, bool> sortedList = new SortedList<string, bool>();
            foreach (FieldSchema item in base.ObjectSchema.Fields)
            {
                if (sortedList.ContainsKey(item.Name))
                    continue;

                sortedList.Add(item.Name, item.IsNullable);
            }

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
            foreach (PropertyInfo info in properties)
            {
                if (info.Name == "IsDirty" || info.Name == "IsValid" || info.Name == "PKString"
                        || info.Name == "MarkAsDeleted" || info.Name == "TableName")
                    continue;

                if (!info.CanWrite || !info.CanRead)
                    continue;

                //Local field
                if (sortedList.ContainsKey(info.Name))
                {
                    retList.Add(info, sortedList[info.Name]);
                    continue;
                }

                //Join field
                if (info.PropertyType.IsValueType || info.PropertyType == typeof(string))
                {
                    
                    retList.Add(info, true);
                }
            }

            return retList;
        }
        #endregion
    }
}
