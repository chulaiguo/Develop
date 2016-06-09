using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryTest
{
    public class FactoryMicroData : FactoryBase
    {
        public FactoryMicroData(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Runtime.Serialization;");

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.MicroData", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            SortedList<string, Type> dataTypeList = Utils.GetDataTypeList(this.ProjectName);
            foreach (ObjectSchema obj in base.MappingSchema.Objects)
            {
                if (obj.Alias == "UsrAccount" || obj.Alias == "CacheOffline")
                    continue;

                string key = string.Format("{0}Data", obj.Alias);
                if(!dataTypeList.ContainsKey(key))
                    continue;

                this.WriteItem(writer, obj, dataTypeList[key]);
                //this.WriteCollection(writer, obj);
            }
        }

        private void WriteItem(StringWriter writer, ObjectSchema item, Type type)
        {
            //writer.WriteLine("\t[DataContract(Namespace=\"http://www.cheke.com/\")]");
            writer.WriteLine("\t[DataContract]");
            writer.WriteLine("\tpublic partial class {0}", item.Alias);
            writer.WriteLine("\t{");

            List<PropertyInfo> list = this.GetPropertyList(type);
            foreach (PropertyInfo field in list)
            {
                if (field.PropertyType == typeof(Guid))
                {
                    writer.WriteLine("\t\tprivate {0} _{1} = Guid.Empty;", field.PropertyType.FullName, field.Name);
                }
                else if (field.PropertyType == typeof(string))
                {
                    writer.WriteLine("\t\tprivate {0} _{1} = string.Empty;", field.PropertyType.FullName, field.Name);
                }
                else if (field.PropertyType == typeof(DateTime))
                {
                    writer.WriteLine("\t\tprivate {0} _{1} = new DateTime(1900, 1, 1);", field.PropertyType.FullName, field.Name);
                }
                else if (field.PropertyType == typeof(bool))
                {
                    writer.WriteLine("\t\tprivate {0} _{1} = false;", field.PropertyType.FullName, field.Name);
                }
                else if (field.PropertyType == typeof(byte[]))
                {
                    writer.WriteLine("\t\tprivate {0} _{1} = null;", field.PropertyType.FullName, field.Name);
                }
                else
                {
                    writer.WriteLine("\t\tprivate {0} _{1} = 0;", field.PropertyType.FullName, field.Name);
                }

                writer.WriteLine("\t\t[DataMember]");
                writer.WriteLine("\t\tpublic {0} {1}", field.PropertyType.FullName, field.Name);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget{{ return this._{0}; }}", field.Name);
                writer.WriteLine("\t\t\tset{{ this._{0} = value; }}", field.Name);
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

            writer.WriteLine("\t}");
            writer.WriteLine();
        }

        private void WriteCollection(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t[DataContract]");
            writer.WriteLine("\t[KnownType(typeof(E3000.MicroData.{0}))]", item.Alias);
            writer.WriteLine("\tpublic partial class {0}Collection : IList", item.Alias);
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tprivate readonly ArrayList _list = new ArrayList();");
            writer.WriteLine("");
            writer.WriteLine("\t\t[DataMember]");
            writer.WriteLine("\t\tpublic ArrayList InnerList");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._list; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tpublic IEnumerator GetEnumerator()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn this._list.GetEnumerator();");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tpublic void CopyTo(Array array, int index)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._list.CopyTo(array, index);");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tpublic int Count");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._list.Count; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tpublic object SyncRoot");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._list.SyncRoot; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tpublic bool IsSynchronized");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._list.IsSynchronized; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tpublic int Add(object value)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn this._list.Add(value);");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tpublic bool Contains(object value)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn this._list.Contains(value);");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tpublic void Clear()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._list.Clear();");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tpublic int IndexOf(object value)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn this._list.IndexOf(value);");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tpublic void Insert(int index, object value)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._list.Insert(index, value);");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tpublic void Remove(object value)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._list.Remove(value);");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tpublic void RemoveAt(int index)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._list.RemoveAt(index);");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tpublic object this[int index]");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._list[index]; }");
            writer.WriteLine("\t\t\tset { this._list[index] = value; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tpublic bool IsReadOnly");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._list.IsReadOnly; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t\tpublic bool IsFixedSize");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._list.IsFixedSize; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t}");
        }

        private List<PropertyInfo> GetPropertyList(Type type)
        {
            List<PropertyInfo> retList = new List<PropertyInfo>();
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
            foreach (PropertyInfo info in properties)
            {
                if (info.Name == "IsDirty" || info.Name == "IsValid" || info.Name == "PKString"
                        || info.Name == "MarkAsDeleted" || info.Name == "TableName")
                    continue;

                if (info.Name == "ModifiedOn" || info.Name == "ModifiedBy"
                  || info.Name == "CreatedOn" || info.Name == "CreatedBy"
                  || info.Name == "RowVersion")
                    continue;

                if(!info.PropertyType.IsValueType && info.PropertyType != typeof(string)
                    && info.PropertyType != typeof(byte[]))
                    continue;

                if (!info.CanWrite || !info.CanRead)
                    continue;

                retList.Add(info);
            }

            return retList;
        }
    }
}
