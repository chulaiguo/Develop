using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryData
{
    public class FactoryViewObj : FactoryBase
    {
        public FactoryViewObj(MappingSchema mappingSchema)
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
                writer.WriteLine("\tpublic partial class {0} :  {0}Data, Cheke.BusinessEntity.IPersist", item.Alias);
                writer.WriteLine("\t{");

                this.WriteItemConstructor(writer, item);
                this.WriteItemParent(writer, item);
                this.WriteItemChildren(writer, item);
                this.WriteItemCloneChildrenMethod(writer, item);
                this.WriteItemSave(writer, item);

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
                this.WriteListSave(writer, item);

                writer.WriteLine("\t}");
            }
        }

        #region Item

        private void WriteItemConstructor(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\t#region Constructors");

            writer.WriteLine("\t\tpublic {0}()", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0}({0}Data data)", obj.Alias);
            writer.WriteLine("\t\t\tbase(data)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t}");

            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();
        }

        private void WriteItemChildren(StringWriter writer, ObjectSchema obj)
        {
            foreach (ChildSchema item in obj.Children)
            {
                writer.WriteLine("\t\tpublic new virtual {0}Collection {0}List", item.Alias);
                writer.WriteLine("\t\t{");

                writer.WriteLine("\t\t\tget");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\treturn (({0}Collection)(base.{0}List));", item.Alias);
                writer.WriteLine("\t\t\t}");

                writer.WriteLine("\t\t\tset");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tbase.{0}List = value;", item.Alias);
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

            writer.WriteLine();
        }

        private void WriteItemParent(StringWriter writer, ObjectSchema obj)
        {
            //Parent
            foreach (ParentSchema item in obj.Parents)
            {
                List<string> list = GetJoinedParentField(obj, item);
                writer.WriteLine("\t\tpublic new virtual {0} {0}", item.Alias);
                writer.WriteLine("\t\t{");

                writer.WriteLine("\t\t\tget");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\t{0} parent = new {0}();", item.Alias);
                writer.WriteLine("\t\t\t\tparent.{0} = this.{1};", item.RemoteColumn, item.LocalColumn);
                foreach (string joinField in list)
                {
                    writer.WriteLine("\t\t\t\tparent.{0} = this.{0};", joinField);
                }
                writer.WriteLine("\t\t\t\treturn parent;");
                writer.WriteLine("\t\t\t}");

                writer.WriteLine("\t\t\tset");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tif(value == null)");
                writer.WriteLine("\t\t\t\t\treturn;");
                writer.WriteLine();
                writer.WriteLine("\t\t\t\tthis.{0} = value.{1};", item.LocalColumn, item.RemoteColumn);
                foreach (string joinField in list)
                {
                    writer.WriteLine("\t\t\t\tthis.{0} = value.{0};", joinField);
                }
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

        }

        private List<string> GetJoinedParentField(ObjectSchema obj, ParentSchema parent)
        {
            List<string> retList = new List<string>();
            foreach (FieldSchema item in obj.Fields)
            {
                if (!item.IsJoined)
                    continue;

                if (item.TableAlias != parent.Alias)
                    continue;

                retList.Add(item.Alias);
            }

            return retList;
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
                writer.WriteLine("\t\t\t\tthis.{0}List = new {0}Collection(data.{0}List);", item.Alias);
                writer.WriteLine("\t\t\t}");
            }

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteItemSave(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tpublic virtual Cheke.BusinessEntity.Result Save(SecurityToken token)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (!this.IsDirty)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn new Result(true);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\t{0}Data item = new {0}Data(this);", obj.Alias);
            writer.WriteLine("\t\t\titem.GetChanges();");
            writer.WriteLine("\t\t\treturn {0}Wrapper.Save(item, token);", obj.Alias);
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic virtual Cheke.BusinessEntity.Result Save()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn this.Save(Cheke.Identity.Token);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }
        #endregion

        #region List
        private void WriteListConstructor(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\t#region Constructors");

            writer.WriteLine("\t\tpublic partial class {0}Collection : {0}DataCollection, Cheke.BusinessEntity.IPersist", item.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase._itemType = typeof({0});", item.Alias);
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0}Collection({0}DataCollection list)", item.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase._itemType = typeof({0});", item.Alias);
            writer.WriteLine("\t\t\tforeach({0}Data item in list)", item.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.Add(new {0}(item));", item.Alias);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tSystem.Collections.ArrayList deletedList = list.GetDeletedList();");
            writer.WriteLine("\t\t\tforeach({0}Data item in deletedList)", item.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t{0} entity = new {0}(item);", item.Alias);
            writer.WriteLine("\t\t\t\tthis.Add(entity);");
            writer.WriteLine("\t\t\t\tthis.Remove(entity);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();
        }

        private void WriteListAdd(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic void Add({0} obj)", item.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase.List.Add(obj);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteListAddRange(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic void AddRange({0}Collection list)", item.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tforeach({0} item in list)", item.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.Add(item);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteListRemove(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic void Remove({0} obj)", item.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase.List.Remove(obj);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteListInsert(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic void Insert(int index, {0} obj)", item.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t base.List.Insert(index, obj);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteListContains(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic bool Contains({0} item)", item.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tforeach ({0} data in base.List)", item.Name);
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
            writer.WriteLine("\t\tpublic bool ContainsDeleted({0} item)", item.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tforeach ({0} data in  this._deletedList)", item.Name);
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

        private void WriteListIndex(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tpublic new {0} this[int index]", item.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn ({0}) base.List[index];", item.Name);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tset");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tbase.List[index] = value;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteListSave(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tpublic virtual Cheke.BusinessEntity.Result Save(SecurityToken token)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (!this.IsDirty)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn new Result(true);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\t{0}DataCollection list = new {0}DataCollection(this);", obj.Alias);
            writer.WriteLine("\t\t\tlist.GetChanges();");
            writer.WriteLine("\t\t\treturn {0}Wrapper.Save(list, token);", obj.Alias);
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic virtual Cheke.BusinessEntity.Result Save()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn this.Save(Cheke.Identity.Token);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }
        #endregion
    }
}