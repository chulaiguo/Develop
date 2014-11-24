using System.IO;
using System.Text;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryDataService
{
    public class FactoryDataServiceBase : FactoryBase
    {
        public FactoryDataServiceBase(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Data;");
            writer.WriteLine("using System.Data.SqlClient;");
            writer.WriteLine("using Cheke;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using Cheke.BusinessService;");
            writer.WriteLine("using Cheke.DataAccess;");
            writer.WriteLine("using {0}.Data;", base.ProjectName);
            writer.WriteLine("using {0}.CRUD;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.DataServiceBase", base.ProjectName);
            writer.WriteLine("{");
           
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            foreach (ObjectSchema obj in base.MappingSchema.Objects)
            {
                writer.WriteLine("\tpublic abstract class {0}DataServiceBase : ServiceBase", obj.Alias);
                writer.WriteLine("\t{");

                this.WriteFields(writer, obj);
                this.WriteConstructor(writer, obj);
                this.WriteProperties(writer, obj);
                this.WriteCreateChildrenService(writer, obj);

                this.WriteDeleteEntity(writer, obj);
                this.WriteDeleteByParent(writer, obj);
                this.WritePurgeEntity(writer, obj);
                this.WritePurgeByParent(writer, obj);

                this.WriteInsertEntity(writer, obj);
                this.WriteUpdateEntity(writer, obj);
                this.WriteUpdateChildren(writer, obj);

                this.WriteGetRowVersion(writer, obj);

                this.WriteGetByPK(writer, obj);
                this.WriteGetAll(writer, obj);
                this.WriteGetByParent(writer, obj);
                this.WriteProcessing(writer, obj);

                this.WriteSaveEntity(writer, obj);
                this.WriteSaveList(writer, obj);

                writer.WriteLine();
                writer.WriteLine("\t}");
            }

        }

        private void WriteFields(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tprotected {0}CRUD _dal;", obj.Alias);
            writer.WriteLine();
        }

        private void WriteConstructor(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tpublic {0}DataServiceBase(string connectionString, SecurityToken token) : base(connectionString, token)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._dal = new {0}CRUD(connectionString);", obj.Alias);
            writer.WriteLine("\t\t\tfor (int i = 0; i < token.GetSortFields().Count; i++)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tSortField field = token.GetSortFields()[i];");
            writer.WriteLine("\t\t\t\tthis._dal.AddSortBy(field.FieldAlias, field.ASC);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteProperties(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tprotected override string TableName");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn \"{0}\";", obj.Name);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteCreateChildrenService(StringWriter writer, ObjectSchema obj)
        {
            foreach (ChildSchema item in obj.Children)
            {
                writer.WriteLine("\t\tprotected abstract {0}DataServiceBase Get{0}DataServiceBase(SecurityToken token);", item.Alias);
            }

            writer.WriteLine();
        }


        private void WriteDeleteEntity(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tprotected override int DeleteEntity(BusinessBase entity)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}Data data = ({0}Data) entity;", obj.Alias);
            writer.WriteLine("\t\t\tif(data == null)");
            writer.WriteLine("\t\t\t\treturn 0;");
            writer.WriteLine();
            foreach (ChildSchema child in obj.Children)
            {
                if (child.CascadeDelete)
                {
                    writer.WriteLine("\t\t\tthis.Get{0}DataServiceBase(base.SecurityToken).DeleteBy{1}(data);", child.Alias, obj.Alias);
                }
            }

            FieldSchema rowVersion = obj.GetRowVersion();
            if (rowVersion == null)
            {
                writer.WriteLine("\t\t\treturn this._dal.DeleteByPK(data.{0}PK, data.RowVersion);", this.GetPKInputParameters(obj, "data"));
            }
            else
            {
                writer.WriteLine("\t\t\treturn this._dal.DeleteByPK({0}, data.{1});", this.GetPKInputParameters(obj, "data"), rowVersion.Alias);
            }
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteDeleteByParent(StringWriter writer, ObjectSchema obj)
        {
            ParentSchemaCollection list = this.GetParentList(obj);
            foreach (ParentSchema item in list)
            {
                ObjectSchema objSchema = base.MappingSchema.Objects[item.Name];
                if(objSchema == null)
                    continue;

                FieldSchema field = objSchema.Fields[item.RemoteColumn];
                if (field == null)
                    continue;

                writer.WriteLine("\t\tinternal int DeleteBy{0}(BusinessBase entity)", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\t{0}Data data = ({0}Data) entity;", item.Alias);
                writer.WriteLine("\t\t\tif(data == null)");
                writer.WriteLine("\t\t\t\treturn 0;");
                writer.WriteLine();

                foreach (ChildSchema child in obj.Children)
                {
                    writer.WriteLine("\t\t\tthis.Get{0}DataServiceBase(base.SecurityToken).DeleteBy{1}(entity);", child.Alias, item.Alias);
                }
               
                writer.WriteLine("\t\t\tbool isLogDeleteEnabled = ServiceBase._DBLog.IsLogDeleteEnabled;");
                writer.WriteLine("\t\t\tbool isLinkDBDeleteEnabled = ServiceBase._DBLink.IsLinkDBDeleteEnabled;");
                writer.WriteLine("\t\t\tif (!isLogDeleteEnabled && !isLinkDBDeleteEnabled)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\treturn this._dal.DeleteBy{0}(data.{1});", item.Alias, this.GetPKInputParameters(objSchema));
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t\tDataTable list = this._dal.GetLogBy{0}(data.{1});", item.Alias, this.GetPKInputParameters(objSchema));
                writer.WriteLine("\t\t\tlist.TableName = this._dal.TableName;");
                writer.WriteLine("\t\t\tint ret = this._dal.DeleteBy{0}(data.{1});", item.Alias, this.GetPKInputParameters(objSchema));
                writer.WriteLine("\t\t\tif (isLogDeleteEnabled)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tServiceBase._DBLog.LogDelete(list, base.SecurityToken);");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t\tif (isLinkDBDeleteEnabled)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tServiceBase._DBLink.LinkDBDelete(list, base.TrustedToken);");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t\treturn ret;");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }
        }

        private void WritePurgeEntity(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tpublic virtual Result Purge({0}Data entity)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn base.Purge(entity);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected override int PurgeEntity(BusinessBase entity)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}Data data = ({0}Data) entity;", obj.Alias);
            writer.WriteLine("\t\t\tif(data == null)");
            writer.WriteLine("\t\t\t\treturn 0;");
            writer.WriteLine();
            foreach (ChildSchema child in obj.Children)
            {
                if (child.CascadeDelete)
                {
                    writer.WriteLine("\t\t\tthis.Get{0}DataServiceBase(base.SecurityToken).PurgeBy{1}(data);", child.Alias, obj.Alias);
                }
            }

            FieldSchema rowVersion = obj.GetRowVersion();
            if (rowVersion == null)
            {
                writer.WriteLine("\t\t\treturn this._dal.DeleteByPK(data.{0}PK, data.RowVersion);", this.GetPKInputParameters(obj, "data"));
            }
            else
            {
                writer.WriteLine("\t\t\treturn this._dal.DeleteByPK({0}, data.{1});", this.GetPKInputParameters(obj, "data"), rowVersion.Alias);
            }
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WritePurgeByParent(StringWriter writer, ObjectSchema obj)
        {
            ParentSchemaCollection list = this.GetParentList(obj);
            foreach (ParentSchema item in list)
            {
                ObjectSchema objSchema = base.MappingSchema.Objects[item.Name];
                if (objSchema == null)
                    continue;

                FieldSchema field = objSchema.Fields[item.RemoteColumn];
                if (field == null)
                    continue;

                writer.WriteLine("\t\tinternal int PurgeBy{0}(BusinessBase entity)", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\t{0}Data data = ({0}Data) entity;", item.Alias);
                writer.WriteLine("\t\t\tif(data == null)");
                writer.WriteLine("\t\t\t\treturn 0;");
                writer.WriteLine();

                foreach (ChildSchema child in obj.Children)
                {
                    writer.WriteLine("\t\t\tthis.Get{0}DataServiceBase(base.SecurityToken).PurgeBy{1}(entity);", child.Alias, item.Alias);
                }

                writer.WriteLine("\t\t\treturn this._dal.DeleteBy{0}(data.{1});", item.Alias, this.GetPKInputParameters(objSchema));
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }
        }


        private void WriteInsertEntity(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tprotected override int InsertEntity(BusinessBase entity)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}Data data = ({0}Data) entity;", obj.Alias);
            writer.WriteLine("\t\t\tif(data == null)");
            writer.WriteLine("\t\t\t\treturn 0;");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn this._dal.Insert(data);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteUpdateEntity(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tprotected override int UpdateEntity(BusinessBase entity)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}Data data = ({0}Data) entity;", obj.Alias);
            writer.WriteLine("\t\t\tif(data == null)");
            writer.WriteLine("\t\t\t\treturn 0;");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn this._dal.Update(data);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteUpdateChildren(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tprotected override void UpdateChildren(BusinessBase entity)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}Data data = ({0}Data) entity;", obj.Alias);
            writer.WriteLine("\t\t\tif(data == null)");
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine();
            foreach (ChildSchema child in obj.Children)
            {
                writer.WriteLine("\t\t\tif (data.{0}List != null)", child.Alias);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\t this.Get{0}DataServiceBase(base.SecurityToken).SaveUnderTransaction(data.{0}List);", child.Alias);
                writer.WriteLine("\t\t\t}");
            }
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }


        private void WriteGetRowVersion(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tinternal Result GetRowVersion({0}DataCollection list)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn base.GetRowVersion(list);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected override Result GetRowVersion(BusinessBase entity)");
            writer.WriteLine("\t\t{");
            FieldSchema rowVersion = obj.GetRowVersion();
            if (rowVersion == null)
            {
                writer.WriteLine("\t\t\treturn new Result(true);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                return;
            }

            writer.WriteLine("\t\t\t{0}Data data = ({0}Data) entity;", obj.Alias);
            writer.WriteLine("\t\t\tif(data == null || !data.IsDirty)");
            writer.WriteLine("\t\t\t\treturn new Result(true);");
            writer.WriteLine();

            writer.WriteLine("\t\t\tResult result = new Result(true);");
            writer.WriteLine("\t\t\tif (entity.IsDeleted)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tresult.Add(data.ObjectID, new byte[0]);");
            writer.WriteLine("\t\t\t\treturn result;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\t\tif (entity.IsSelfDirty)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tresult.Add(data.ObjectID, this._dal.GetRowVersion({0}));", this.GetPKInputParameters(obj, "data"));
            writer.WriteLine("\t\t\t}");

            foreach (ChildSchema child in obj.Children)
            {
                writer.WriteLine("\t\t\tif (data.{0}List != null && data.{0}List.Count > 0)", child.Alias);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tresult.Add(this.Get{0}DataServiceBase(base.SecurityToken).GetRowVersion(data.{0}List));", child.Alias);
                writer.WriteLine("\t\t\t}");
            }

            writer.WriteLine("\t\t\treturn result;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }


        private void WriteGetByPK(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tpublic virtual {0}Data GetByPK({1})", obj.Alias, this.GetPKDeclareParameters(obj));
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (base.IsTrusted)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn this.Processing(this._dal.GetByPK({0}));", this.GetPKInputParameters(obj));
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tif (!base.IsAuthorizedSelect)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn null;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn this.Processing(({0}Data) base.Filter(this._dal.GetByPK({1})));", obj.Alias, this.GetPKInputParameters(obj));
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected virtual {0}Data GetEntity(string sqlWhere, SqlParameter[] paras)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (base.IsTrusted)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn this.Processing(this._dal.GetEntityBy(sqlWhere, paras));");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tif (!base.IsAuthorizedSelect)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn null;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn this.Processing(({0}Data) base.Filter(this._dal.GetEntityBy(sqlWhere, paras)));", obj.Alias);
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteGetAll(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tpublic virtual {0}DataCollection GetAll()", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (base.IsTrusted)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn this.Processing(this._dal.GetAll());");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tif (!base.IsAuthorizedSelect)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn new {0}DataCollection();", obj.Alias);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn this.Processing(({0}DataCollection) base.Filter(this._dal.GetAll()));", obj.Alias);
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0}ViewCollection GetViewAll()", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn new {0}ViewCollection(this.GetAll());", obj.Alias);
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected virtual {0}DataCollection GetCollection(string sqlWhere, SqlParameter[] paras)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (base.IsTrusted)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn this.Processing(this._dal.GetCollectionBy(sqlWhere, paras));");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tif (!base.IsAuthorizedSelect)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn new {0}DataCollection();", obj.Alias);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\t return this.Processing(({0}DataCollection) base.Filter(this._dal.GetCollectionBy(sqlWhere, paras)));", obj.Alias);
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteGetByParent(StringWriter writer, ObjectSchema obj)
        {
            foreach (ParentSchema parent in obj.Parents)
            {
                FieldSchema field = obj.Fields[parent.LocalColumn];
                if (field == null)
                    continue;

                writer.WriteLine("\t\tpublic virtual {0}DataCollection GetBy{1}({2} {3})", obj.Alias, parent.Alias, 
                                 base.Utilities.ToDotNetType(field.DataType), field.Name);
                writer.WriteLine("\t\t{");

                writer.WriteLine("\t\t\tif (base.IsTrusted)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\treturn this.Processing(this._dal.GetBy{0}({1}));", parent.Alias, field.Name);
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t\tif (!base.IsAuthorizedSelect)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\treturn null;");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t\treturn this.Processing(({0}DataCollection) base.Filter(this._dal.GetBy{1}({2})));", obj.Alias, parent.Alias, field.Name);
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                writer.WriteLine("\t\tpublic {0}ViewCollection GetViewBy{1}({2} {3})", obj.Alias, parent.Alias,
                               base.Utilities.ToDotNetType(field.DataType), field.Name);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\treturn new {0}ViewCollection(this.GetBy{1}({2}));", obj.Alias, parent.Alias, field.Name);
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }
        }

        private void WriteProcessing(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tprotected virtual {0}Data Processing({0}Data entity)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn entity;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected virtual {0}DataCollection Processing({0}DataCollection list)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}DataCollection retList = new {0}DataCollection();", obj.Alias);
            writer.WriteLine("\t\t\tforeach({0}Data item in list)", obj.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t{0}Data data = this.Processing(item);", obj.Alias);
            writer.WriteLine("\t\t\t\tif(data != null)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tretList.Add(data);");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn retList;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }
       

        private void WriteSaveEntity(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tpublic virtual Result Save({0}Data entity)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn base.Save(entity);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic virtual void SaveUnderTransaction({0}Data entity)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase.SaveUnderTransaction(entity);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteSaveList(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tpublic virtual Result Save({0}DataCollection list)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn base.Save(list);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic virtual void SaveUnderTransaction({0}DataCollection list)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase.SaveUnderTransaction(list);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        #region Helper functions

        private string GetPKInputParameters(ObjectSchema obj)
        {
            return this.GetPKInputParameters(obj, string.Empty);
        }

        private string GetPKInputParameters(ObjectSchema obj, string preName)
        {
            StringBuilder builder = new StringBuilder();
            foreach (FieldSchema item in obj.Fields)
            {
                if (!item.IsPK)
                    continue;

                if (preName.Length > 0)
                {
                    builder.AppendFormat(" {0}.{1},", preName, item.Name);
                }
                else
                {
                    builder.AppendFormat(" {0},", item.Name);
                }
            }

            return builder.ToString().TrimEnd(',');
        }

        private string GetPKDeclareParameters(ObjectSchema obj)
        {
            StringBuilder builder = new StringBuilder();
            foreach (FieldSchema item in obj.Fields)
            {
                if (!item.IsPK)
                    continue;

                builder.AppendFormat(" {0} {1},", this.Utilities.ToDotNetType(item.DataType), item.Name);
            }

            return builder.ToString().TrimEnd(',');
        }

        private ParentSchemaCollection GetParentList(ObjectSchema obj)
        {
            ParentSchemaCollection list = new ParentSchemaCollection();
            this.MappingSchema.GetParentList(obj, list);
            return list;
        }
        #endregion
    }
}