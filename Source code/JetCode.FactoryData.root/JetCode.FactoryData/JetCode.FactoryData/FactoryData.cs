using System;
using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryData
{
    public class FactoryData : FactoryBase
    {
        public FactoryData(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Data;");
            writer.WriteLine("using System.Text;");
            writer.WriteLine("using Cheke.BusinessEntity;");

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Data", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {   
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                writer.WriteLine("\t[Serializable]");
                writer.WriteLine("\tpublic partial class {0}Data : BusinessBase", item.Alias);
                writer.WriteLine("\t{");

                this.WriteItemFields(writer, item);
                this.WriteItemConstructor(writer, item);
                this.WriteItemProperties(writer, item);
                this.WriteItemTableName(writer, item);
                this.WriteItemChildren(writer, item);
                this.WriteItemParent(writer, item);

                this.WriteItemCopyFromMethod(writer, item);
                this.WriteItemCloneChildrenMethod(writer, item);
                this.WriteItemAcceptChangesMethod(writer, item);
                this.WriteItemEqualMethod(writer, item);
                this.WriteItemGetHashCodeMethod(writer, item);
                this.WriteItemToStringMethod(writer, item);
                this.WriteItemIsDirty(writer, item);
                this.WriteItemIsValid(writer, item);
                this.WriteItemBindingEvent(writer, item);
                
                writer.WriteLine("\t}");
            }

            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                writer.WriteLine("\t[Serializable]");
                writer.WriteLine("\tpublic class {0}DataCollection : BusinessCollectionBase", item.Alias);
                writer.WriteLine("\t{");

                this.WriteListConstructor(writer, item);
                this.WriteListAdd(writer, item);
                this.WriteListAddRange(writer, item);
                this.WriteListRemove(writer, item);
                this.WriteListInsert(writer, item);
                this.WriteListContains(writer, item);
                this.WriteListContainsDeleted(writer, item);
                this.WriteListIndex(writer, item);
                this.WriteListOnValidate(writer, item);

                writer.WriteLine("\t}");
            }
        }

        #region Item
        private void WriteItemFields(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\t#region Fields");
            foreach(FieldSchema item in obj.Fields)
            {
                writer.WriteLine("\t\tprivate {0} _{1};", Utilities.ToDotNetType(item.DataType).Name, base.LowerFirstLetter(item.Alias));
            }
            writer.WriteLine();
            foreach (ChildSchema item in obj.Children)
            {
                writer.WriteLine("\t\tprivate {0}DataCollection _{1}List;", item.Alias, base.LowerFirstLetter(item.Alias));
            }
            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();
        }

        private void WriteItemConstructor(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\t#region Constructors");

            writer.WriteLine("\t\tpublic {0}Data()", obj.Alias);
            writer.WriteLine("\t\t{");
            foreach (FieldSchema item in obj.Fields)
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
            }
            writer.WriteLine();
            writer.WriteLine("\t\t\tbase.MarkNew();");
            writer.WriteLine("\t\t\tthis.InitMemberVariables();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0}Data({0}Data data)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis.CopyFrom(data, true);");
            writer.WriteLine("\t\t\tthis.CloneChildren(data);");
            writer.WriteLine("\t\t}");

            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();
        }

        private void WriteItemProperties(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\t#region Properties");
            //Field
            foreach (FieldSchema item in obj.Fields)
            {
                if (item.Alias == "RowVersion" || item.Alias == "CreatedOn" || item.Alias == "CreatedBy"
                    || item.Alias == "ModifiedOn" || item.Alias == "ModifiedBy")
                {
                    writer.WriteLine("\t\tpublic override {0} {1}", Utilities.ToDotNetType(item.DataType).Name,
                        item.Alias);
                }
                else
                {
                    writer.WriteLine("\t\tpublic virtual {0} {1}", Utilities.ToDotNetType(item.DataType).Name,
                       item.Alias);
                }

                writer.WriteLine("\t\t{");

                writer.WriteLine("\t\t\tget");
                writer.WriteLine("\t\t\t{");
                if (Utilities.ToDotNetType(item.DataType) == typeof (DateTime))
                {
                    writer.WriteLine("\t\t\t\tSystem.DateTime utcTime = new System.DateTime(this._{0}.Ticks, DateTimeKind.Utc);", base.LowerFirstLetter(item.Alias));
                    writer.WriteLine("\t\t\t\treturn utcTime.ToLocalTime();");
                }
                else if (Utilities.ToDotNetType(item.DataType) == typeof(string))
                {
                    writer.WriteLine("\t\t\t\treturn this._{0} == null ? null : this._{0}.Trim();", base.LowerFirstLetter(item.Alias));
                }
                else
                {
                    writer.WriteLine("\t\t\t\treturn this._{0};", base.LowerFirstLetter(item.Alias));
                }
                writer.WriteLine("\t\t\t}");

                writer.WriteLine("\t\t\tset");
                writer.WriteLine("\t\t\t{");
                if (item.IsJoined || item.Alias == "RowVersion" || item.Alias == "CreatedOn" || item.Alias == "CreatedBy"
                    || item.Alias == "ModifiedOn" || item.Alias == "ModifiedBy")
                {
                    if (Utilities.ToDotNetType(item.DataType) == typeof (DateTime))
                    {
                        writer.WriteLine("\t\t\t\t this._{0} = value.ToUniversalTime();", base.LowerFirstLetter(item.Alias));
                    }
                    else if (Utilities.ToDotNetType(item.DataType) == typeof(string))
                    {
                        writer.WriteLine("\t\t\t\t this._{0} = value == null ? null : value.Trim();", base.LowerFirstLetter(item.Alias));
                    }
                    else
                    {
                        writer.WriteLine("\t\t\t\tthis._{0} = value;", base.LowerFirstLetter(item.Alias)); 
                    }
                }
                else
                {
                    writer.WriteLine("\t\t\t\tEntityEventArgs e = new EntityEventArgs(\"{0}\", value);", item.Alias);
                    writer.WriteLine("\t\t\t\tif (this._{0} != value)", base.LowerFirstLetter(item.Alias));
                    writer.WriteLine("\t\t\t\t{");
                    writer.WriteLine("\t\t\t\t\tthis.OnBeforeChanged(e);");
                    writer.WriteLine("\t\t\t\t\tif (!e.Cancel)");
                    writer.WriteLine("\t\t\t\t\t{");

                    if (Utilities.ToDotNetType(item.DataType) == typeof (DateTime))
                    {
                        writer.WriteLine("\t\t\t\t\t\tthis._{0} = value.ToUniversalTime();", base.LowerFirstLetter(item.Alias));
                    }
                    else if (Utilities.ToDotNetType(item.DataType) == typeof(string))
                    {
                        writer.WriteLine("\t\t\t\t\t\tthis._{0} = value == null ? null : value.Trim();", base.LowerFirstLetter(item.Alias));
                    }
                    else
                    {
                        writer.WriteLine("\t\t\t\t\t\tthis._{0} = value;", base.LowerFirstLetter(item.Alias));
                    }
                    
                    writer.WriteLine("\t\t\t\t\t\tbase.MarkDirty();");
                    writer.WriteLine("\t\t\t\t\t\tthis.OnAfterChanged(e);");
                    writer.WriteLine("\t\t\t\t\t\tthis.On{0}Changed(EventArgs.Empty);", item.Alias);
                    writer.WriteLine("\t\t\t\t\t}");
                    writer.WriteLine("\t\t\t\t}");
                }
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }
        
            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();
        }

        private void WriteItemTableName(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\t#region TableName");
            writer.WriteLine("\t\tpublic override string TableName");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn \"{0}\";", obj.Alias);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();
        }

        private void WriteItemChildren(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\t#region Children");
            foreach (ChildSchema item in obj.Children)
            {
                writer.WriteLine("\t\tpublic virtual {0}DataCollection {0}List", item.Alias);
                writer.WriteLine("\t\t{");

                writer.WriteLine("\t\t\tget");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\treturn this._{0}List;", base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t}");

                writer.WriteLine("\t\t\tset");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tthis._{0}List = value;", base.LowerFirstLetter(item.Alias));
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }
            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();

        }

        private void WriteItemParent(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\t#region Parents");
            //Joined Parent
            SortedList<string, List<FieldSchema>> joinedParents = this.GetJoinedParents(obj);
            foreach (KeyValuePair<string, List<FieldSchema>> pair in joinedParents)
            {
                List<FieldSchema> list = pair.Value;
                writer.WriteLine("\t\tpublic virtual {0}Data {0}", pair.Key);
                writer.WriteLine("\t\t{");

                writer.WriteLine("\t\t\tget");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\t{0}Data parent = new {0}Data();", pair.Key);
                foreach (FieldSchema item in list)
                {
                    writer.WriteLine("\t\t\t\tparent.{0} = this.{1};", item.Name, item.Alias);
                }
                writer.WriteLine("\t\t\t\treturn parent;");
                writer.WriteLine("\t\t\t}");

                writer.WriteLine("\t\t\tset");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tif(value == null)");
                writer.WriteLine("\t\t\t\t\treturn;");
                writer.WriteLine();
                foreach (FieldSchema item in list)
                {
                    writer.WriteLine("\t\t\t\tthis.{0} = value.{1};", item.Alias, item.Name);
                }
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

            //Local Parent
            foreach (ParentSchema item in obj.Parents)
            {
                if (joinedParents.ContainsKey(item.Alias))
                    continue;

                writer.WriteLine("\t\tpublic virtual {0}Data {0}", item.Alias);
                writer.WriteLine("\t\t{");

                writer.WriteLine("\t\t\tget");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\t{0}Data parent = new {0}Data();", item.Alias);
                writer.WriteLine("\t\t\t\tparent.{0} = this.{1};", item.RemoteColumn, item.LocalColumn);
                writer.WriteLine("\t\t\t\treturn parent;");
                writer.WriteLine("\t\t\t}");

                writer.WriteLine("\t\t\tset");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tif(value == null)");
                writer.WriteLine("\t\t\t\t\treturn;");
                writer.WriteLine();
                writer.WriteLine("\t\t\t\tthis.{0} = value.{1};", item.LocalColumn, item.RemoteColumn);
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();
        }

        private SortedList<string, List<FieldSchema>> GetJoinedParents(ObjectSchema obj)
        {
            
            SortedList<string, List<FieldSchema>> retIndex = new SortedList<string, List<FieldSchema>>();
            foreach (FieldSchema item in obj.Fields)
            {
                if (!item.IsJoined)
                    continue;

                FieldSchema parentPK = this.GetTablePK(item.TableAlias);
                if(parentPK == null)
                    continue;

                if (!retIndex.ContainsKey(item.TableAlias))
                {
                    retIndex.Add(item.TableAlias, new List<FieldSchema>());
                    retIndex[item.TableAlias].Add(parentPK);
                }

                if (parentPK.Name != item.Name)
                {
                    retIndex[item.TableAlias].Add(item);
                }
            }

            return retIndex;
        }

        private FieldSchema GetTablePK(string tableName)
        {
            ObjectSchema obj = base.GetObjectByName(tableName);
            if (obj == null)
                return null;

            List<FieldSchema> fields = obj.GetPKList();
            if (fields.Count != 1)
                return null;

            return fields[0];
        }

        private void WriteItemCopyFromMethod(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tpublic override void CopyFrom(BusinessBase entity, bool all)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}Data data = entity as {0}Data;", obj.Alias);
            writer.WriteLine("\t\t\tif (data == null)");
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine();

            foreach (FieldSchema item in obj.Fields)
            {
                if (item.IsPK)
                    continue;

                writer.WriteLine("\t\t\tthis.{0} = data.{0};", item.Alias);
            }

            writer.WriteLine("\t\t\tif (all)");
            writer.WriteLine("\t\t\t{");
            foreach (FieldSchema item in obj.Fields)
            {
                if (item.IsPK)//set PK
                {
                    writer.WriteLine("\t\t\t\tthis.{0} = data.{0};", item.Alias);
                }
            }
            writer.WriteLine("\t\t\t\tbase._objectID = data.ObjectID;");
            writer.WriteLine("\t\t\t\tbase.MarkOld();");
            writer.WriteLine("\t\t\t\tif (data.IsNew)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tbase.MarkNew();");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\tif (data.IsDeleted)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tbase.MarkDeleted();");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\tif (data.IsSelfDirty)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tbase.MarkDirty();");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t}");

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteItemCloneChildrenMethod(StringWriter writer, ObjectSchema obj)
        {
            if (obj.Children.Count == 0)
                return;

            writer.WriteLine("\t\tprotected override void CloneChildren(BusinessBase entity)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}Data data = entity as {0}Data;", obj.Alias);
            writer.WriteLine("\t\t\tif (data == null)");
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine();

            foreach (ChildSchema item in obj.Children)
            {
                writer.WriteLine("\t\t\tif (data.{0}List != null)", item.Alias);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tthis.{0}List = new {0}DataCollection(data.{0}List);", item.Alias);
                writer.WriteLine("\t\t\t}");
            }

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteItemAcceptChangesMethod(StringWriter writer, ObjectSchema obj)
        {
            if (obj.Children.Count == 0)
                return;

            writer.WriteLine("\t\tpublic override void AcceptChanges()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase.AcceptChanges();");
            writer.WriteLine();

            foreach (ChildSchema item in obj.Children)
            {
                writer.WriteLine("\t\t\tif (this.{0}List != null)", item.Alias);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tthis.{0}List.AcceptChanges();", item.Alias);
                writer.WriteLine("\t\t\t}");
            }

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteItemEqualMethod(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tpublic override bool Equals(object obj)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn ((obj.GetType() == this.GetType()) && this.InternalEquals(obj as {0}Data));", obj.Alias);
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate bool InternalEquals({0}Data obj)", obj.Alias);
            writer.WriteLine("\t\t{");

            foreach (FieldSchema item in obj.Fields)
            {
                if(!item.IsPK)
                    continue;

                writer.WriteLine("\t\t\tif (this.{0}.CompareTo(obj.{0}) != 0)", item.Alias);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\treturn false;");
                writer.WriteLine("\t\t\t}");
            }
            writer.WriteLine("\t\t\treturn true;");

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteItemGetHashCodeMethod(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tpublic override int GetHashCode()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn this.ToString().GetHashCode();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteItemBindingEvent(StringWriter writer, ObjectSchema obj)
        {
           writer.WriteLine("\t\t#region Binding Events");
            //Field
            foreach (FieldSchema item in obj.Fields)
            {
                if(item.IsJoined)
                    continue;

                writer.WriteLine("\t\t[field: NonSerialized, NotUndoable]");
                writer.WriteLine("\t\tpublic event EventHandler {0}Changed;", item.Alias);
               
                writer.WriteLine("\t\tprivate void On{0}Changed(EventArgs e)", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tif ((this.{0}Changed != null))", item.Alias);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tthis.{0}Changed(this, e);", item.Alias);
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();
        }

        private void WriteItemToStringMethod(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tpublic override string PKString");
            writer.WriteLine("\t\t{");

            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tStringBuilder builder = new StringBuilder();");
            foreach (FieldSchema item in obj.Fields)
            {
                if (!item.IsPK)
                    continue;

                writer.WriteLine("\t\t\t\tbuilder.AppendFormat(\"{{0}}|\", this.{0});", item.Alias);
            }
            writer.WriteLine("\t\t\t\treturn builder.ToString().TrimEnd('|');");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic override string ToString()");
            writer.WriteLine("\t\t{");

            writer.WriteLine("\t\t\tStringBuilder builder = new StringBuilder();");
            writer.WriteLine("\t\t\tbuilder.Append(\"{0}:\");", obj.Alias);
            foreach (FieldSchema item in obj.Fields)
            {
                if (!item.IsPK)
                    continue;

                writer.WriteLine("\t\t\tbuilder.AppendFormat(\"{{0}}|\", this.{0});", item.Alias);
            }
            writer.WriteLine("\t\t\treturn builder.ToString().TrimEnd('|');");

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteItemIsDirty(StringWriter writer, ObjectSchema obj)
        {
            if(obj.Children.Count == 0)
                return;

            writer.WriteLine("\t\tpublic override bool IsDirty");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif(base.IsDirty)");
            writer.WriteLine("\t\t\t\t\treturn true;");
            writer.WriteLine();
            
            foreach (ChildSchema item in obj.Children)
            {
                writer.WriteLine("\t\t\t\tif (this.{0}List != null && this.{0}List.IsDirty)", item.Alias);
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\treturn true;");
                writer.WriteLine("\t\t\t\t}");
            }

            writer.WriteLine();
            writer.WriteLine("\t\t\t\treturn false;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteItemIsValid(StringWriter writer, ObjectSchema obj)
        {
            if (obj.Children.Count == 0)
                return;

            writer.WriteLine("\t\tpublic override bool IsValid");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif(!base.IsValid)");
            writer.WriteLine("\t\t\t\t\treturn false;");
            writer.WriteLine();

            foreach (ChildSchema item in obj.Children)
            {
                writer.WriteLine("\t\t\t\tif (this.{0}List != null && !this.{0}List.IsValid)", item.Alias);
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\treturn false;");
                writer.WriteLine("\t\t\t\t}");
            }

            writer.WriteLine();
            writer.WriteLine("\t\t\t\treturn true;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }
        #endregion

        #region List
        private void WriteListConstructor(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\t#region Constructors");

            writer.WriteLine("\t\tpublic {0}DataCollection()", item.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase._itemType = typeof({0}Data);", item.Alias);
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0}DataCollection({0}DataCollection list)", item.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase._itemType = typeof({0}Data);", item.Alias);
            writer.WriteLine("\t\t\tforeach({0}Data item in list)", item.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.Add(new {0}Data(item));", item.Alias);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tSystem.Collections.ArrayList deletedList = list.GetDeletedList();");
            writer.WriteLine("\t\t\tforeach({0}Data item in deletedList)", item.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t{0}Data entity = new {0}Data(item);", item.Alias);
            writer.WriteLine("\t\t\t\tthis.Add(entity);");
            writer.WriteLine("\t\t\t\tthis.Remove(entity);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic override string TableName");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget {{ return \"{0}\"; }}", item.Alias);
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();
        }

        private void WriteListAdd(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic void Add({0}Data obj)", item.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase.List.Add(obj);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteListAddRange(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic void AddRange({0}DataCollection list)", item.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tforeach({0}Data item in list)", item.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.Add(item);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteListRemove(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic void Remove({0}Data obj)", item.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase.List.Remove(obj);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteListInsert(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic void Insert(int index, {0}Data obj)", item.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t base.List.Insert(index, obj);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteListContains(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic bool Contains({0}Data item)", item.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tforeach ({0}Data data in base.List)", item.Name);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif (data.Equals(item))");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t return true;");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\treturn false;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteListContainsDeleted(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic bool ContainsDeleted({0}Data item)", item.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tforeach ({0}Data data in  this._deletedList)", item.Name);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif (data.Equals(item))");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t return true;");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\treturn false;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteListOnValidate(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tprotected override void OnValidate(object item)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tSystem.Type t = item.GetType();");
            writer.WriteLine("\t\t\tif (t != base._itemType && !t.IsSubclassOf(base._itemType))");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthrow new ArgumentException(\"The item must be a type of {0}Data\");", item.Alias);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteListIndex(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic new {0}Data this[int index]", item.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn ({0}Data) base.List[index];", item.Name);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tset");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tbase.List[index] = value;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }
        #endregion
    }
}