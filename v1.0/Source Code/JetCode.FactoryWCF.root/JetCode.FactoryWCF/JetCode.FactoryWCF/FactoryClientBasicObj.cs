using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWCF
{
    public class FactoryClientBasicObj : FactoryBase
    {
        public FactoryClientBasicObj(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.ComponentModel;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("using Cheke.BusinessEntity;");

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
                writer.WriteLine("\tpublic partial class {0} : BusinessBase, INotifyPropertyChanged", item.Alias);
                writer.WriteLine("\t{");

                string typeKey = string.Format("{0}Data", item.Name);
                if(typeList.ContainsKey(typeKey))
                {
                    List<PropertyInfo> list = this.GetPropertyList(typeList[typeKey]);

                    writer.WriteLine("\t\t#region Fields");
                    this.WriteFields(writer, list);
                    this.WriteChildrenFields(writer, item);
                    writer.WriteLine("\t\t#endregion");
                    writer.WriteLine();

                    writer.WriteLine("\t\t#region Constructors");
                    this.WriteConstructor(writer, item, list);
                    writer.WriteLine("\t\t#endregion");
                    writer.WriteLine();

                    writer.WriteLine("\t\t#region Properties");
                    this.WriteProperties(writer, item, list);
                    this.WriteChildrenProperties(writer, item);
                    writer.WriteLine("\t\t#endregion");
                    writer.WriteLine();

                    writer.WriteLine("\t\t#region Methods");
                    this.WriteCloneChildrenMethod(writer, item);
                    this.WriteCopyFromMethod(writer, item, list);
                    writer.WriteLine("\t\t#endregion");
                    writer.WriteLine();
                }
                else
                {
                    writer.WriteLine("\t\t#region Fields");
                    this.WriteFields(writer, item);
                    this.WriteChildrenFields(writer, item);
                    writer.WriteLine("\t\t#endregion");
                    writer.WriteLine();

                    writer.WriteLine("\t\t#region Constructors");
                    this.WriteConstructor(writer, item);
                    writer.WriteLine("\t\t#endregion");
                    writer.WriteLine();

                    writer.WriteLine("\t\t#region Properties");
                    this.WriteProperties(writer, item);
                    this.WriteChildrenProperties(writer, item);
                    writer.WriteLine("\t\t#endregion");
                    writer.WriteLine();

                    writer.WriteLine("\t\t#region Methods");
                    this.WriteCloneChildrenMethod(writer, item);
                    this.WriteCopyFromMethod(writer, item, null);
                    writer.WriteLine("\t\t#endregion");
                    writer.WriteLine();
                }

                writer.WriteLine("\t\t#region Common Methods");
                this.WriteCommonMethod(writer, item);
                writer.WriteLine("\t\t#endregion");
                writer.WriteLine();

                writer.WriteLine("\t\t#region Rules");
                this.WriteRuleMethod(writer, item);
                writer.WriteLine("\t\t#endregion");
                writer.WriteLine();

                writer.WriteLine("\t\t#region AcceptChanges & IsDirty & IsValid");
                this.WriteAcceptChangesMethod(writer, item);
                this.WriteReplaceItemMethod(writer, item);
                this.WriteAcceptRowversionMethod(writer, item);
                this.WriteIsDirtyMethod(writer, item);
                this.WriteIsValidMethod(writer, item);
                writer.WriteLine("\t\t#endregion");

                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }

        private void WriteConstructor(StringWriter writer, ObjectSchema objSchema)
        {
            writer.WriteLine("\t\tpublic {0}()", objSchema.Alias);
            writer.WriteLine("\t\t{");
            foreach (FieldSchema item in objSchema.Fields)
            {
                Type dotNetType = Utilities.ToDotNetType(item.DataType);
                if (dotNetType == typeof(Guid))
                {
                    if (item.IsPK)
                    {
                        writer.WriteLine("\t\t\tthis._{0} = Guid.NewGuid();", base.LowerFirstLetter(item.Alias));
                    }
                    else
                    {
                        writer.WriteLine("\t\t\tthis._{0} = Guid.Empty;", base.LowerFirstLetter(item.Alias));
                    }
                    continue;
                }

                if (dotNetType == typeof(DateTime))
                {
                    writer.WriteLine("\t\t\tthis._{0} = DateTime.Now;", base.LowerFirstLetter(item.Alias));
                    continue;
                }

                if (dotNetType == typeof(string))
                {
                    writer.WriteLine("\t\t\tthis._{0} = string.Empty;", base.LowerFirstLetter(item.Alias));
                    continue;
                }

                if (dotNetType == typeof(bool))
                {
                    writer.WriteLine("\t\t\tthis._{0} = false;", base.LowerFirstLetter(item.Alias));
                    continue;
                }

                if (dotNetType == typeof(byte))
                {
                    writer.WriteLine("\t\t\tthis._{0} = (byte)0;", base.LowerFirstLetter(item.Alias));
                    continue;
                }

                if (dotNetType == typeof(short))
                {
                    writer.WriteLine("\t\t\tthis._{0} = (short)0;", base.LowerFirstLetter(item.Alias));
                    continue;
                }

                if (dotNetType == typeof(int))
                {
                    writer.WriteLine("\t\t\tthis._{0} = (int)0;", base.LowerFirstLetter(item.Alias));
                    continue;
                }

                if (dotNetType == typeof(long))
                {
                    writer.WriteLine("\t\t\tthis._{0} = (long)0;", base.LowerFirstLetter(item.Alias));
                    continue;
                }

                if (dotNetType == typeof(float))
                {
                    writer.WriteLine("\t\t\tthis._{0} = (float)0;", base.LowerFirstLetter(item.Alias));
                    continue;
                }

                if (dotNetType == typeof(double))
                {
                    writer.WriteLine("\t\t\tthis._{0} = (double)0;", base.LowerFirstLetter(item.Alias));
                    continue;
                }

                if (dotNetType == typeof(decimal))
                {
                    writer.WriteLine("\t\t\tthis._{0} = (decimal)0;", base.LowerFirstLetter(item.Alias));
                    continue;
                }
            }
            writer.WriteLine();
            writer.WriteLine("\t\t\tbase.MarkNew();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteConstructor(StringWriter writer, ObjectSchema objSchema, List<PropertyInfo> list)
        {
            writer.WriteLine("\t\tpublic {0}()", objSchema.Alias);
            writer.WriteLine("\t\t{");

            List<FieldSchema> pkList = objSchema.GetPKList();
            SortedList<string, FieldSchema> pkIndex = new SortedList<string, FieldSchema>();
            foreach (FieldSchema item in pkList)
            {
                pkIndex.Add(item.Alias, item);
            }

            foreach (PropertyInfo item in list)
            {
                Type dotNetType = item.PropertyType;
                if (dotNetType == typeof(Guid))
                {
                    if(pkIndex.ContainsKey(item.Name))
                    {
                        writer.WriteLine("\t\t\tthis._{0} = Guid.NewGuid();", base.LowerFirstLetter(item.Name));
                    }
                    else
                    {
                        writer.WriteLine("\t\t\tthis._{0} = Guid.Empty;", base.LowerFirstLetter(item.Name));
                    }
                    continue;
                }

                if (dotNetType == typeof(DateTime))
                {
                    writer.WriteLine("\t\t\tthis._{0} = DateTime.Now;", base.LowerFirstLetter(item.Name));
                    continue;
                }

                if (dotNetType == typeof(string))
                {
                    writer.WriteLine("\t\t\tthis._{0} = string.Empty;", base.LowerFirstLetter(item.Name));
                    continue;
                }

                if (dotNetType == typeof(bool))
                {
                    writer.WriteLine("\t\t\tthis._{0} = false;", base.LowerFirstLetter(item.Name));
                    continue;
                }

                if (dotNetType == typeof(byte))
                {
                    writer.WriteLine("\t\t\tthis._{0} = (byte)0;", base.LowerFirstLetter(item.Name));
                    continue;
                }

                if (dotNetType == typeof(short))
                {
                    writer.WriteLine("\t\t\tthis._{0} = (short)0;", base.LowerFirstLetter(item.Name));
                    continue;
                }

                if (dotNetType == typeof(int))
                {
                    writer.WriteLine("\t\t\tthis._{0} = (int)0;", base.LowerFirstLetter(item.Name));
                    continue;
                }

                if (dotNetType == typeof(long))
                {
                    writer.WriteLine("\t\t\tthis._{0} = (long)0;", base.LowerFirstLetter(item.Name));
                    continue;
                }

                if (dotNetType == typeof(float))
                {
                    writer.WriteLine("\t\t\tthis._{0} = (float)0;", base.LowerFirstLetter(item.Name));
                    continue;
                }

                if (dotNetType == typeof(double))
                {
                    writer.WriteLine("\t\t\tthis._{0} = (double)0;", base.LowerFirstLetter(item.Name));
                    continue;
                }

                if (dotNetType == typeof(decimal))
                {
                    writer.WriteLine("\t\t\tthis._{0} = (decimal)0;", base.LowerFirstLetter(item.Name));
                    continue;
                }
            }
            writer.WriteLine();
            writer.WriteLine("\t\t\tbase.MarkNew();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteFields(StringWriter writer, ObjectSchema obj)
        {
            foreach (FieldSchema item in obj.Fields)
            {
                writer.WriteLine("\t\tprivate {0} _{1};", Utilities.ToDotNetType(item.DataType).Name, base.LowerFirstLetter(item.Alias));
            }
            writer.WriteLine();
        }

        private void WriteProperties(StringWriter writer, ObjectSchema obj)
        {
            foreach (FieldSchema item in obj.Fields)
            {
                string overridetag = string.Empty;
                if (item.Alias == "CreatedOn" || item.Alias == "CreatedBy" || item.Alias == "ModifiedOn"
                    || item.Alias == "ModifiedBy" || item.Alias == "RowVersion")
                {
                    overridetag = "override";
                }

                writer.WriteLine("\t\tpublic  {0} {1} {2}", overridetag, Utilities.ToDotNetType(item.DataType).Name, item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget {{ return this._{0}; }}", base.LowerFirstLetter(item.Alias));
                if (overridetag.Length > 0)
                {
                    writer.WriteLine("\t\t\tset {{ this._{0} = value; }}", base.LowerFirstLetter(item.Alias));
                }
                else
                {
                    writer.WriteLine("\t\t\tset");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tif(this._{0} == value)", base.LowerFirstLetter(item.Alias));
                    writer.WriteLine("\t\t\t\t\treturn;");
                    writer.WriteLine();
                    writer.WriteLine("\t\t\t\tthis.CheckPropertyValue(\"{0}\", value);", item.Alias);
                    writer.WriteLine();
                    writer.WriteLine("\t\t\t\tthis._{0} = value;", base.LowerFirstLetter(item.Alias));
                    writer.WriteLine("\t\t\t\tbase.MarkDirty();");
                    writer.WriteLine("\t\t\t\tthis.OnPropertyChanged(\"{0}\");", item.Alias);
                    writer.WriteLine("\t\t\t}");
                }
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

            writer.WriteLine("\t\tpublic  override string TableName");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget {{ return \"{0}\"; }}", obj.Alias);
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteFields(StringWriter writer, List<PropertyInfo> list)
        {
            foreach (PropertyInfo item in list)
            {
                writer.WriteLine("\t\tprivate {0} _{1};", item.PropertyType.Name, base.LowerFirstLetter(item.Name));
            }
            writer.WriteLine();
        }

        private void WriteProperties(StringWriter writer, ObjectSchema objSchema, List<PropertyInfo> list)
        {
            SortedList<string, string> fieldIndex = new SortedList<string, string>();
            foreach (FieldSchema item in objSchema.Fields)
            {
                if(fieldIndex.ContainsKey(item.Alias))
                    continue;

                fieldIndex.Add(item.Alias, item.Alias);
            }

            this.WriteProperties(writer, objSchema);
            foreach (PropertyInfo item in list)
            {
                if(fieldIndex.ContainsKey(item.Name))
                    continue;

                writer.WriteLine("\t\tpublic {0} {1}", item.PropertyType.Name, item.Name);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget {{ return this._{0}; }}", base.LowerFirstLetter(item.Name));
                writer.WriteLine("\t\t\tset {{ this._{0} = value; }}", base.LowerFirstLetter(item.Name));
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

            writer.WriteLine();
        }

        private void WriteChildrenFields(StringWriter writer, ObjectSchema obj)
        {
            foreach (ChildSchema item in obj.Children)
            {
                writer.WriteLine("\t\tprivate {0}Collection _{1}List;", item.Alias, base.LowerFirstLetter(item.Alias));
            }
            
            writer.WriteLine();
        }

        private void WriteChildrenProperties(StringWriter writer, ObjectSchema obj)
        {
            foreach (ChildSchema item in obj.Children)
            {
                writer.WriteLine("\t\tpublic {0}Collection {0}List", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget {{ return this._{0}List; }}", base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\tset {{ this._{0}List = value; }}", base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

            writer.WriteLine();
        }


        private void WriteCloneChildrenMethod(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tprotected override void CloneChildren(BusinessBase obj)");
            writer.WriteLine("\t\t{");

            if (item.Children.Count > 0)
            {
                writer.WriteLine("\t\t\t{0} entity = obj as {0};", item.Alias);
                writer.WriteLine("\t\t\tif (entity== null)");
                writer.WriteLine("\t\t\t\treturn;");
                writer.WriteLine();
                foreach (ChildSchema child in item.Children)
                {
                    writer.WriteLine("\t\t\tif (entity.{0}List != null)", child.Alias);
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tthis.{0}List = entity.{0}List.Clone() as {0}Collection;", child.Alias);
                    writer.WriteLine("\t\t\t}");
                }
            }
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteCopyFromMethod(StringWriter writer, ObjectSchema item, List<PropertyInfo> list)
        {
            writer.WriteLine("\t\tpublic override void CopyFrom(BusinessBase item, bool all)");
            writer.WriteLine("\t\t{");

            writer.WriteLine("\t\t\tif (item == null)");
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t{0} entity = item as {0};", item.Alias);
            writer.WriteLine("\t\t\tif (entity == null)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tbase.CopyFrom(item);");
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();

            List<FieldSchema> pkList = item.GetPKList();
            SortedList<string, Type> pkIndex = new SortedList<string, Type>();
            pkIndex.Add("ObjectID", typeof(Guid));
            foreach (FieldSchema pk in pkList)
            {
                if(pkIndex.ContainsKey(pk.Alias))
                    continue;
               
                pkIndex.Add(pk.Alias, base.Utilities.ToDotNetType(pk.DataType));
            }

            if (list != null)
            {
                foreach (PropertyInfo info in list)
                {
                    if (pkIndex.ContainsKey(info.Name))
                        continue;

                    writer.WriteLine("\t\t\tthis._{0} = entity.{1};", base.LowerFirstLetter(info.Name), info.Name);
                }
            }
            else
            {
                foreach (FieldSchema field in item.Fields)
                {
                    if (pkIndex.ContainsKey(field.Alias))
                        continue;

                    writer.WriteLine("\t\t\tthis._{0} = entity.{1};", base.LowerFirstLetter(field.Alias), field.Alias);
                }
            }

            writer.WriteLine("\t\t\tif(all)");
            writer.WriteLine("\t\t\t{");
            foreach (KeyValuePair<string, Type> pair in pkIndex)
            {
                writer.WriteLine("\t\t\t\tthis._{0} = entity.{1};", base.LowerFirstLetter(pair.Key), pair.Key); 
            }
            writer.WriteLine("\t\t\t\tbase.MarkOld();");
            writer.WriteLine("\t\t\t\tif (entity.IsNew)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tbase.MarkNew();");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\tif (entity.IsDeleted)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tbase.MarkDeleted();");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\tif (entity.IsSelfDirty)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tbase.MarkDirty();");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t}");

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteAcceptChangesMethod(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic override void AcceptChanges()");
            writer.WriteLine("\t\t{");

            writer.WriteLine("\t\t\tbase.AcceptChanges();");
            writer.WriteLine();
            foreach (ChildSchema child in item.Children)
            {
                writer.WriteLine("\t\t\tif (this.{0}List != null)", child.Alias);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tthis.{0}List.AcceptChanges();", child.Alias);
                writer.WriteLine("\t\t\t}");
            }
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteReplaceItemMethod(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic override bool ReplaceItem(BusinessBase item)");
            writer.WriteLine("\t\t{");

            writer.WriteLine("\t\t\tif (base.ReplaceItem(item))");
            writer.WriteLine("\t\t\t\treturn true;");
            writer.WriteLine();
            foreach (ChildSchema child in item.Children)
            {
                writer.WriteLine("\t\t\tif (this.{0}List != null)", child.Alias);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tif(this.{0}List.ReplaceItem(item))", child.Alias);
                writer.WriteLine("\t\t\t\t\treturn true;");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();
            }
            writer.WriteLine("\t\t\treturn false;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteAcceptRowversionMethod(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic override void AcceptRowVersion(Dictionary<Guid, byte[]> rowVersionList, List<BusinessBase> changedList)");
            writer.WriteLine("\t\t{");

            writer.WriteLine("\t\t\tbase.AcceptRowVersion(rowVersionList, changedList);");
            writer.WriteLine();
            foreach (ChildSchema child in item.Children)
            {
                writer.WriteLine("\t\t\tif (this.{0}List != null)", child.Alias);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tthis.{0}List.AcceptRowVersion(rowVersionList, changedList);", child.Alias);
                writer.WriteLine("\t\t\t}");
            }
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }


        private void WriteIsValidMethod(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic override bool IsValid");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif (base.IsDeleted)");
            writer.WriteLine("\t\t\t\t\treturn true;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tif (!base.IsValid)"); 
            writer.WriteLine("\t\t\t\t\treturn false;");
            writer.WriteLine(); 
            foreach (ChildSchema child in item.Children)
            {
                writer.WriteLine("\t\t\t\tif (this.{0}List != null && !this.{0}List.IsValid)", child.Alias);
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\treturn false;");
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine();
            }
            writer.WriteLine("\t\t\treturn true;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteIsDirtyMethod(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic override bool IsDirty");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif (base.IsDirty)");
            writer.WriteLine("\t\t\t\t\treturn true;");
            writer.WriteLine();
            foreach (ChildSchema child in item.Children)
            {
                writer.WriteLine("\t\t\t\tif (this.{0}List != null && this.{0}List.IsDirty)", child.Alias);
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\treturn true;");
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine();
            }
            writer.WriteLine("\t\t\treturn false;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteCommonMethod(StringWriter writer, ObjectSchema item)
        {
            List<FieldSchema> pkList = item.GetPKList();
            writer.WriteLine("\t\tprivate bool Equals({0} obj)", item.Alias);
            writer.WriteLine("\t\t{");

            foreach (FieldSchema field in pkList)
            {
                writer.WriteLine("\t\t\tif (this.{0}.CompareTo(obj.{0}) != 0)", field.Alias);
                writer.WriteLine("\t\t\t\treturn false;");
                writer.WriteLine();
            }
            writer.WriteLine("\t\t\treturn true;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic override bool Equals(object obj)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn ((obj.GetType() == base.GetType()) && this.Equals(({0}) obj));", item.Alias);
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic new static bool Equals(object objA, object objB)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn ((objA.GetType() == objB.GetType()) && (({0}) objA).Equals(({0}) objB));", item.Alias);
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic override int GetHashCode()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn this.ToString().GetHashCode();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic override string ToString()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tstring str = \"{0}/\";", item.Alias);

            foreach (FieldSchema field in pkList)
            {
                writer.WriteLine("\t\t\tstr += string.Format(\"{0}:[{{0}}];\" ,this.{0}.ToString());", field.Alias);
            }
            writer.WriteLine("\t\t\treturn str;");
            writer.WriteLine("\t\t}");

        }

        private void WriteRuleMethod(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tprotected override void CheckRules()");
            writer.WriteLine("\t\t{");
            //writer.WriteLine("\t\t\tstring error = string.Empty;");
            //writer.WriteLine();
            //foreach (FieldSchema field in item.Fields)
            //{
            //    if (field.IsJoined)
            //        continue;

            //    Type fieldType = base.Utilities.ToDotNetType(field.DataType);
            //    if (fieldType == typeof(byte[]))
            //        continue;

            //    if (field.Alias == "CreatedOn" || field.Alias == "CreatedBy" || field.Alias == "ModifiedOn"
            //        || field.Alias == "ModifiedBy" || field.Alias == "RowVersion")
            //        continue;

            //    writer.WriteLine("\t\t\t//{0}", field.Alias);
            //    writer.WriteLine("\t\t\terror = this.CheckDBPropertyValue(\"{0}\", this.{0});", field.Alias);
            //    writer.WriteLine("\t\t\tif (error.Length > 0)");
            //    writer.WriteLine("\t\t\t{");
            //    writer.WriteLine("\t\t\t\tbase.BrokenRules.Add(\"{0}\", error);", field.Alias);
            //    writer.WriteLine("\t\t\t}");
            //    writer.WriteLine("\t\t\telse");
            //    writer.WriteLine("\t\t\t{");
            //    writer.WriteLine("\t\t\t\terror = this.CheckBizPropertyValue(\"{0}\", this.{0});", field.Alias);
            //    writer.WriteLine("\t\t\t\tif (error.Length > 0)");
            //    writer.WriteLine("\t\t\t\t{");
            //    writer.WriteLine("\t\t\t\t\tbase.BrokenRules.Add(\"{0}\", error);", field.Alias);
            //    writer.WriteLine("\t\t\t\t}");
            //    writer.WriteLine("\t\t\t}");
            //    writer.WriteLine();
            //}
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate void CheckPropertyValue(string name, object objValue)");
            writer.WriteLine("\t\t{");
            //writer.WriteLine("\t\t\tstring error = this.CheckDBPropertyValue(name, objValue);");
            //writer.WriteLine("\t\t\tif (error.Length > 0)");
            //writer.WriteLine("\t\t\t{");
            //writer.WriteLine("\t\t\t\tthrow new Exception(error);");
            //writer.WriteLine("\t\t\t}");
            //writer.WriteLine();
            //writer.WriteLine("\t\t\terror = this.CheckBizPropertyValue(name, objValue);");
            //writer.WriteLine("\t\t\tif (error.Length > 0)");
            //writer.WriteLine("\t\t\t{");
            //writer.WriteLine("\t\t\t\tthrow new Exception(error);");
            //writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
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

                if (!info.CanWrite || !info.CanRead)
                    continue;

                if (info.PropertyType.IsValueType || info.PropertyType == typeof(string)
                    || info.PropertyType == typeof(byte[]))
                {

                    retList.Add(info);
                }
            }

            return retList;
        }
    }
}
