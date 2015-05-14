using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryDataService
{
    public class FactoryCRUD : FactoryBase
    {
        public FactoryCRUD(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Data;");
            writer.WriteLine("using System.Text;");
            writer.WriteLine("using System.Data.SqlClient;");
            writer.WriteLine("using Cheke.DataAccess;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using {0}.Data;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.CRUD", base.ProjectName);
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
                writer.WriteLine("\tpublic class {0}CRUD : DataAccessBase", obj.Alias);
                writer.WriteLine("\t{");

                this.WriteConstructor(writer, obj);

                this.WriteSQLColumns(writer, obj);
                this.WriteSQLLeftJoins(writer, obj);
                this.WriteSQLSelect(writer, obj);
                this.WriteSQLCount(writer, obj);

                this.WriteInsert(writer, obj);
                this.WriteUpdate(writer, obj);

                this.WriteFetchEntity(writer, obj);
                this.WriteFetchCollection(writer, obj);
                this.WriteGetEntityBy(writer, obj);
                this.WriteGetCollectionBy(writer, obj);

                this.WriteGetRowVersion(writer, obj);
                this.WriteGetByPK(writer, obj);
                this.WriteGetAll(writer, obj);
                this.WriteGetByParent(writer, obj);

                this.WriteGetLogByPK(writer, obj);
                this.WriteGetLogByParent(writer, obj);

                this.WriteDeleteByPK(writer, obj);
                this.WriteDeleteAll(writer, obj);
                this.WriteDeleteByParent(writer, obj);

                writer.WriteLine();
                writer.WriteLine("\t}");
            }
        }

        private void WriteConstructor(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tpublic {0}CRUD(string connectionString) : base(connectionString)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine();
        }

        private void WriteSQLLeftJoins(StringWriter writer, ObjectSchema obj)
        {
            StringBuilder joinsBuilder = new StringBuilder();
            foreach (JoinSchema item in obj.Joins)
            {
                joinsBuilder.AppendFormat("LEFT JOIN [{0}] [{1}] ON {2} ", item.RefTable, item.RefAlias, item.JoinCommand);
            }

            writer.WriteLine("\t\tpublic string SQLLeftJoins");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn \"{0}\";", joinsBuilder.ToString().TrimEnd(' '));
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");

            writer.WriteLine();
        }

        private void WriteSQLColumns(StringWriter writer, ObjectSchema obj)
        {
            StringBuilder fieldsBuilder = new StringBuilder();
            foreach (FieldSchema item in obj.Fields)
            {
                fieldsBuilder.AppendFormat("[{0}].[{1}] AS [{2}],", item.TableAlias, item.Name, item.Alias);
            }

            writer.WriteLine("\t\tpublic string SQLColumns");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn \"{0}\";", fieldsBuilder.ToString().TrimEnd(','));
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");

            writer.WriteLine();
        }

        private void WriteSQLSelect(StringWriter writer, ObjectSchema obj)
        {
            StringBuilder fieldsBuilder = new StringBuilder();
            foreach (FieldSchema item in obj.Fields)
            {
                fieldsBuilder.AppendFormat("[{0}].[{1}] AS [{2}],", item.TableAlias, item.Name, item.Alias);
            }

            StringBuilder joinsBuilder = new StringBuilder();
            foreach (JoinSchema item in obj.Joins)
            {
                joinsBuilder.AppendFormat("LEFT JOIN [{0}] [{1}] ON {2} ", item.RefTable, item.RefAlias, item.JoinCommand);
            }

            writer.WriteLine("\t\tprotected override string SQLSelect");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn \"SELECT {0} \"", fieldsBuilder.ToString().TrimEnd(','));
            writer.WriteLine("\t\t\t\t\t+ \" FROM [{0}] [{1}] \"", obj.Name, obj.Alias);
            writer.WriteLine("\t\t\t\t\t+ \" {0}\";", joinsBuilder.ToString().TrimEnd(' '));
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");

            writer.WriteLine();
        }

        private void WriteSQLCount(StringWriter writer, ObjectSchema obj)
        {
            StringBuilder joinsBuilder = new StringBuilder();
            foreach (JoinSchema item in obj.Joins)
            {
                joinsBuilder.AppendFormat("LEFT JOIN [{0}] [{1}] ON {2} ", item.RefTable, item.RefAlias, item.JoinCommand);
            }

            writer.WriteLine("\t\tprotected override string SQLCount");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn \"SELECT COUNT(*) FROM [{0}] [{1}] \"", obj.Name, obj.Alias);
            writer.WriteLine("\t\t\t\t\t+ \" {0}\";", joinsBuilder.ToString().TrimEnd(' '));
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }


        private void WriteInsert(StringWriter writer, ObjectSchema obj)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("INSERT INTO [{0}] (", obj.Name);
            foreach (FieldSchema item in obj.Fields)
            {
                if(item.IsJoined)
                    continue;

                if (System.String.Compare(item.DataType, "timestamp", System.StringComparison.OrdinalIgnoreCase) == 0)
                    continue;

                sqlBuilder.AppendFormat("[{0}],", item.Name);
            }
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1);
            sqlBuilder.AppendFormat(") VALUES(");
            foreach (FieldSchema item in obj.Fields)
            {
                if (item.IsJoined)
                    continue;

                if (System.String.Compare(item.DataType, "timestamp", System.StringComparison.OrdinalIgnoreCase) == 0)
                    continue;

                sqlBuilder.AppendFormat("@{0},", item.Name);
            }
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1);
            sqlBuilder.AppendFormat(")");
            string sql = sqlBuilder.ToString();

            writer.WriteLine("\t\tpublic int Insert({0}Data entity)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tstring sql = \"{0}\";", sql);
            writer.WriteLine("\t\t\tSqlParameter[] paras = new SqlParameter[{0}];", this.GetInsertFieldCount(obj));
            int i = 0;
            foreach (FieldSchema item in obj.Fields)
            {
                if (item.IsJoined)
                    continue;

                if (System.String.Compare(item.DataType, "timestamp", System.StringComparison.OrdinalIgnoreCase) == 0)
                    continue;

                string sqlTypeSize = this.ToSqlDBTypeSize(item);
                if (sqlTypeSize.Length == 0)
                {
                    writer.WriteLine("\t\t\tparas[{0}] = new SqlParameter(\"@{1}\", {2});", i, item.Name, this.ToSqlDBType(item));
                }
                else
                {
                    writer.WriteLine("\t\t\tparas[{0}] = new SqlParameter(\"@{1}\", {2}, {3});", i, item.Name, this.ToSqlDBType(item), sqlTypeSize);
                }
                if (item.DataType == "uniqueidentifier")
                {
                    writer.WriteLine("\t\t\tif (entity.{0} == Guid.Empty)", item.Name);
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tparas[{0}].Value = DBNull.Value;", i);
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine("\t\t\telse");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tparas[{0}].Value = entity.{1};", i, item.Name);
                    writer.WriteLine("\t\t\t}");
                }
                else
                {
                    writer.WriteLine("\t\t\tparas[{0}].Value = entity.{1};", i, item.Name);
                }
                i++;
            }

            writer.WriteLine("\t\t\treturn base.ExecuteNonQuery(sql, paras);");

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteUpdate(StringWriter writer, ObjectSchema obj)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("UPDATE [{0}] SET ", obj.Name);
            string rowVersion = string.Empty;
            StringCollection pkList = new StringCollection();
            foreach (FieldSchema item in obj.Fields)
            {
                if (item.IsJoined)
                    continue;

                if (System.String.Compare(item.DataType, "timestamp", System.StringComparison.OrdinalIgnoreCase) == 0)
                {
                    rowVersion = item.Name;
                    continue;
                }

                if (item.IsPK)
                {
                    pkList.Add(item.Name);
                    continue;
                }

                sqlBuilder.AppendFormat("[{0}] = @{0},", item.Name);
            }
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1);
            sqlBuilder.Append("  WHERE");
            foreach (string pk in pkList)
            {
                sqlBuilder.AppendFormat(" [{0}] = @{0} AND", pk);
            }

            if (rowVersion.Length > 0)
            {
                sqlBuilder.AppendFormat(" [{0}] = @{0}", rowVersion);
            }
            else
            {
                sqlBuilder.Remove(sqlBuilder.Length - 3, 3);
            }
            string sql = sqlBuilder.ToString();

            writer.WriteLine("\t\tpublic int Update({0}Data entity)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tstring sql = \"{0}\";", sql);
            writer.WriteLine("\t\t\tSqlParameter[] paras = new SqlParameter[{0}];", this.GetUpdateFieldCount(obj));

            int i = 0;
            foreach (FieldSchema item in obj.Fields)
            {
                if (item.IsJoined)
                    continue;

                string sqlTypeSize = this.ToSqlDBTypeSize(item);
                if (sqlTypeSize.Length == 0)
                {
                    writer.WriteLine("\t\t\tparas[{0}] = new SqlParameter(\"@{1}\", {2});", i, item.Name, this.ToSqlDBType(item));
                }
                else
                {
                    writer.WriteLine("\t\t\tparas[{0}] = new SqlParameter(\"@{1}\", {2}, {3});", i, item.Name, this.ToSqlDBType(item), sqlTypeSize);
                }
                if (item.DataType == "uniqueidentifier")
                {
                    writer.WriteLine("\t\t\tif (entity.{0} == Guid.Empty)", item.Name);
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tparas[{0}].Value = DBNull.Value;", i);
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine("\t\t\telse");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tparas[{0}].Value = entity.{1};", i, item.Name);
                    writer.WriteLine("\t\t\t}");
                }
                else
                {
                    writer.WriteLine("\t\t\tparas[{0}].Value = entity.{1};", i, item.Name);
                }
                i++;
            }

            writer.WriteLine("\t\t\treturn base.ExecuteNonQuery(sql, paras);");

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private int GetInsertFieldCount(ObjectSchema obj)
        {
            int ret = 0;
            foreach (FieldSchema item in obj.Fields)
            {
                if (item.IsJoined)
                    continue;

                if (System.String.Compare(item.DataType, "timestamp", System.StringComparison.OrdinalIgnoreCase) == 0)
                    continue;

                ret++;
            }

            return ret;
        }

        private int GetUpdateFieldCount(ObjectSchema obj)
        {
            int ret = 0;
            foreach (FieldSchema item in obj.Fields)
            {
                if (item.IsJoined)
                    continue;

                ret++;
            }

            return ret;
        }

        private void WriteFetchEntity(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tprivate {0}Data CreateDataByReader(SafeDataReader reader)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}Data data = new {0}Data {{", obj.Alias);
            for (int i = 0; i < obj.Fields.Count; i++)
            {
                FieldSchema item = obj.Fields[i];
                if (i == obj.Fields.Count - 1)
                {
                    writer.WriteLine("\t\t\t\t{0} = ({1}) reader[\"{0}\"]", item.Alias, Utilities.ToDotNetType(item.DataType).Name);
                }
                else
                {
                    writer.WriteLine("\t\t\t\t{0} = ({1}) reader[\"{0}\"],", item.Alias, Utilities.ToDotNetType(item.DataType).Name);
                }
            }
           
            writer.WriteLine("\t\t\t};");
            writer.WriteLine("\t\t\tdata.AcceptChanges();");
            writer.WriteLine("\t\t\treturn data;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate {0}Data FetchEntity(string where, SqlParameter[] paras)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}Data data = null;", obj.Alias);
            writer.WriteLine("\t\t\tusing (SafeDataReader reader = base.ExecuteDataReader(where, paras))");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif (reader.Read())");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tdata = this.CreateDataByReader(reader);");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\treader.Close();");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\treturn data;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate {0}Data FetchEntityEx(string sql, SqlParameter[] paras)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}Data data = null;", obj.Alias);
            writer.WriteLine("\t\t\tusing (SafeDataReader reader = base.ExecuteDataReaderEx(sql, paras))");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif (reader.Read())");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tdata = this.CreateDataByReader(reader);");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\treader.Close();");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\treturn data;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteFetchCollection(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tprivate {0}DataCollection FetchCollection(string where, SqlParameter[] paras)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}DataCollection list = new {0}DataCollection();", obj.Alias);
            writer.WriteLine("\t\t\tusing (SafeDataReader reader = base.ExecuteDataReader(where, paras))");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\twhile(reader.Read())");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t{0}Data data = this.CreateDataByReader(reader);", obj.Alias);
            writer.WriteLine("\t\t\t\t\tlist.Add(data);");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\treader.Close();");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\treturn list;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate {0}DataCollection FetchCollectionEx(string sql, SqlParameter[] paras)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}DataCollection list = new {0}DataCollection();", obj.Alias);
            writer.WriteLine("\t\t\tusing (SafeDataReader reader = base.ExecuteDataReaderEx(sql, paras))");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\twhile(reader.Read())");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t{0}Data data = this.CreateDataByReader(reader);", obj.Alias);
            writer.WriteLine("\t\t\t\t\tlist.Add(data);");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\treader.Close();");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\treturn list;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteGetEntityBy(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tpublic {0}Data GetEntityBy(string sqlWhere, SqlParameter[] paras)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn this.FetchEntity(sqlWhere, paras);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0}Data GetEntityExBy(string sql, SqlParameter[] paras)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn this.FetchEntityEx(sql, paras);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteGetCollectionBy(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tpublic {0}DataCollection GetCollectionBy(string sqlWhere, SqlParameter[] paras)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn this.FetchCollection(sqlWhere, paras);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0}DataCollection GetCollectionExBy(string sql, SqlParameter[] paras)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn this.FetchCollectionEx(sql, paras);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }


        private void WriteGetRowVersion(StringWriter writer, ObjectSchema obj)
        {
            FieldSchema rowVersion = obj.GetRowVersion();
            if (rowVersion == null)
                return;

            List<FieldSchema> pkList = obj.GetPKList();

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("SELECT [{0}] AS [{1}] FROM [{2}] WHERE ", rowVersion.Name, rowVersion.Name, obj.Name);
            foreach (FieldSchema item in pkList)
            {
                sqlBuilder.AppendFormat(" [{0}] = @{0} AND", item.Name);
            }
            sqlBuilder.Remove(sqlBuilder.Length - 3, 3);

            string sql = sqlBuilder.ToString();

            string pkParams = this.GetPKDeclareParameters(obj);
            writer.WriteLine("\t\tpublic byte[] GetRowVersion({0})", pkParams);

            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tstring sql = \"{0}\";", sql);
            writer.WriteLine("\t\t\tSqlParameter[] paras = new SqlParameter[{0}];", pkList.Count);

            int i = 0;
            foreach (FieldSchema item in pkList)
            {
                writer.WriteLine("\t\t\tparas[{0}] = new SqlParameter(\"@{1}\", {2}, {3});", i, item.Name, this.ToSqlDBType(item), item.Size);
                writer.WriteLine("\t\t\tparas[{0}].Value = {1};", i, item.Name);
                i++;
            }

            writer.WriteLine("\t\t\treturn (byte[]) base.ExecuteScalar(sql, paras);");

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteGetByPK(StringWriter writer, ObjectSchema obj)
        {
            List<FieldSchema> pkList = obj.GetPKList();

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat(" WHERE ");
            foreach (FieldSchema item in pkList)
            {
                sqlBuilder.AppendFormat(" [{0}].[{1}] = @{1} AND", item.TableName, item.Name);
            }
            sqlBuilder.Remove(sqlBuilder.Length - 3, 3);

            string sql = sqlBuilder.ToString();

            string pkParams = this.GetPKDeclareParameters(obj);
            writer.WriteLine("\t\tpublic {0}Data GetByPK({1})", obj.Alias, pkParams);

            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tstring sql = \"{0}\";", sql);
            writer.WriteLine("\t\t\tSqlParameter[] paras = new SqlParameter[{0}];", pkList.Count);

            int i = 0;
            foreach (FieldSchema item in pkList)
            {
                writer.WriteLine("\t\t\tparas[{0}] = new SqlParameter(\"@{1}\", {2});", i, item.Name, this.ToSqlDBType(item));
                writer.WriteLine("\t\t\tparas[{0}].Value = {1};", i, item.Name);
                i++;
            }

            writer.WriteLine("\t\t\treturn this.FetchEntity(sql, paras);");

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteGetAll(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tpublic {0}DataCollection GetAll()", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn this.FetchCollection(\"\", null);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteGetByParent(StringWriter writer, ObjectSchema obj)
        {
            foreach (ParentSchema item in obj.Parents)
            {
                FieldSchema field = obj.Fields[item.LocalColumn];
                if(field == null)
                    continue;

                writer.WriteLine("\t\tpublic {0}DataCollection GetBy{1}({2} {3})", obj.Alias, item.Alias, 
                                 base.Utilities.ToDotNetType(field.DataType), field.Name);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tstring sqlWhere = \" WHERE [{0}].[{1}] = @{1}\";", field.TableAlias, field.Name);
                writer.WriteLine("\t\t\tSqlParameter[] paras = new SqlParameter[1];");
                writer.WriteLine("\t\t\tparas[0] = new SqlParameter(\"@{0}\", {1});", field.Name, this.ToSqlDBType(field));
                writer.WriteLine("\t\t\tparas[0].Value = {0};", field.Name);
                writer.WriteLine("\t\t\treturn this.FetchCollection(sqlWhere, paras);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }
        }


        private void WriteGetLogByPK(StringWriter writer, ObjectSchema obj)
        {
            List<FieldSchema> pkList = obj.GetPKList();
            StringBuilder sqlWhereBuilder = new StringBuilder();
            foreach (FieldSchema item in pkList)
            {
                sqlWhereBuilder.AppendFormat(" [{0}].[{1}] = @{1} AND", item.TableName, item.Name);
            }
            sqlWhereBuilder.Remove(sqlWhereBuilder.Length - 3, 3);
            string sqlWhere = sqlWhereBuilder.ToString();

            string pkParams = this.GetPKDeclareParameters(obj);
            writer.WriteLine("\t\tpublic DataTable GetLogByPK({0})", pkParams);

            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tstring sql = \"SELECT {0} FROM [{1}] WHERE {2}\";", this.GetLogColumns(obj), obj.Name, sqlWhere);
            writer.WriteLine("\t\t\tSqlParameter[] paras = new SqlParameter[{0}];", pkList.Count);

            int i = 0;
            foreach (FieldSchema item in pkList)
            {
                writer.WriteLine("\t\t\tparas[{0}] = new SqlParameter(\"@{1}\", {2});", i, item.Name, this.ToSqlDBType(item));
                writer.WriteLine("\t\t\tparas[{0}].Value = {1};", i, item.Name);
                i++;
            }

            writer.WriteLine("\t\t\treturn this.ExecuteDataTable(sql, paras);");

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteGetLogByParent(StringWriter writer, ObjectSchema obj)
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

                writer.WriteLine("\t\tpublic DataTable GetLogBy{0}({1} {2})", item.Alias,
                                 base.Utilities.ToDotNetType(field.DataType), base.LowerFirstLetter(field.Name));
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tstring sql = \"{0}\";", this.GetDeleteByParentSql(item, obj));

                writer.WriteLine("\t\t\tstring oldValue = \"DELETE [{0}]\";", obj.Name);
                writer.WriteLine("\t\t\tstring newValue = \"SELECT {0}\";", this.GetLogColumns(obj));
                writer.WriteLine("\t\t\tsql = sql.Replace(oldValue, newValue).TrimEnd().TrimEnd(';').Replace(\",\", \"UNION\");");

                writer.WriteLine("\t\t\tSqlParameter[] paras = new SqlParameter[1];");
                writer.WriteLine("\t\t\tparas[0] = new SqlParameter(\"@{0}\", {1});", field.Name, this.ToSqlDBType(field));
                writer.WriteLine("\t\t\tparas[0].Value = {0};", base.LowerFirstLetter(field.Name));
                writer.WriteLine("\t\t\treturn base.ExecuteDataTable(sql, paras);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }
        }

        private string GetLogColumns(ObjectSchema obj)
        {
            List<FieldSchema> pkList = obj.GetPKList();

            StringBuilder sqlPKBuilder = new StringBuilder();
            foreach (FieldSchema item in pkList)
            {
                sqlPKBuilder.AppendFormat(" [{0}].[{1}] AS [{1}],", item.TableName, item.Name);
            }

            foreach (FieldSchema item in obj.Fields)
            {
                if (item.Name == "ModifiedOn")
                {
                    return string.Format("{0} [{1}].[{2}] AS [{2}]", sqlPKBuilder, obj.Alias, item.Name);
                }
            }

            return sqlPKBuilder.ToString().TrimEnd(',');
        }


        private void WriteDeleteByPK(StringWriter writer, ObjectSchema obj)
        {
            FieldSchema rowVersion = obj.GetRowVersion();
            List<FieldSchema> pkList = obj.GetPKList();

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("DELETE FROM [{0}] WHERE ", obj.Name);
            foreach (FieldSchema item in pkList)
            {
                sqlBuilder.AppendFormat(" [{0}] = @{0} AND", item.Name);
            }

            if (rowVersion != null)
            {
                sqlBuilder.AppendFormat(" [{0}] = @{0}", rowVersion.Name);
            }
            else
            {
                sqlBuilder.Remove(sqlBuilder.Length - 3, 3);
            }

            string sql = sqlBuilder.ToString();
            string pkInputParams = this.GetPKDeclareParameters(obj);
            if (rowVersion != null)
            {
                pkInputParams = string.Format("{0}, byte[] {1}", pkInputParams, rowVersion.Name);
            }
            writer.WriteLine("\t\tpublic int DeleteByPK({0})", pkInputParams);

            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tstring sql = \"{0}\";", sql);

            if (rowVersion != null)
            {
                writer.WriteLine("\t\t\tSqlParameter[] paras = new SqlParameter[{0}];", pkList.Count + 1);
            }
            else
            {
                writer.WriteLine("\t\t\tSqlParameter[] paras = new SqlParameter[{0}];", pkList.Count);
            }

            int i = 0;
            foreach (FieldSchema item in pkList)
            {
                writer.WriteLine("\t\t\tparas[{0}] = new SqlParameter(\"@{1}\", {2});", i, item.Name, this.ToSqlDBType(item));
                writer.WriteLine("\t\t\tparas[{0}].Value = {1};", i, item.Name);
                i++;
            }

            if (rowVersion != null)
            {
                writer.WriteLine("\t\t\tparas[{0}] = new SqlParameter(\"@{1}\", {2}, {3});", i, rowVersion.Name, this.ToSqlDBType(rowVersion), rowVersion.Size);
                writer.WriteLine("\t\t\tparas[{0}].Value = {1};", i, rowVersion.Name);

            }

            writer.WriteLine("\t\t\treturn base.ExecuteNonQuery(sql, paras);");

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteDeleteAll(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tpublic int DeleteAll()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tstring sql = \"DELETE FROM [{0}]\";", obj.Name);
            writer.WriteLine("\t\t\treturn base.ExecuteNonQuery(sql, null);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteDeleteByParent(StringWriter writer, ObjectSchema obj)
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

                writer.WriteLine("\t\tpublic int DeleteBy{0}({1} {2})", item.Alias, 
                                 base.Utilities.ToDotNetType(field.DataType), base.LowerFirstLetter(field.Name));
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tstring sql = \"{0}\";", this.GetDeleteByParentSql(item, obj));
                writer.WriteLine("\t\t\tSqlParameter[] paras = new SqlParameter[1];");
                writer.WriteLine("\t\t\tparas[0] = new SqlParameter(\"@{0}\", {1});", field.Name, this.ToSqlDBType(field));
                writer.WriteLine("\t\t\tparas[0].Value = {0};", base.LowerFirstLetter(field.Name));
                writer.WriteLine("\t\t\treturn base.ExecuteNonQuery(sql, paras);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }
        }

        #region Deletion path

        private string GetDeleteByParentSql(ParentSchema topParent, ObjectSchema obj)
        {
            List<List<ParentSchema>> listPath = this.GetDeletePath(topParent, obj);
            if (listPath.Count == 0)
                return string.Empty;

            StringBuilder builder = new StringBuilder();
            foreach (List<ParentSchema> path in listPath)
            {
                builder.AppendFormat("DELETE [{0}] FROM [{0}] ", obj.Name);
                for (int i = path.Count - 1; i >= 0; i--)
                {
                    if (i == path.Count - 1)
                    {
                        builder.AppendFormat("INNER JOIN [{0}] ON [{1}].[{2}] = [{0}].[{3}] ",
                                             path[i].Name, obj.Name, path[i].LocalColumn, path[i].RemoteColumn); 
                    }
                    else
                    {
                        builder.AppendFormat("INNER JOIN [{0}] ON [{1}].[{2}] = [{0}].[{3}] ",
                                             path[i].Name, path[i + 1].Name, path[i].LocalColumn, path[i].RemoteColumn);
                    }
                }
                builder.AppendFormat("WHERE [{0}].[{1}] = @{1};", topParent.Name, topParent.RemoteColumn);
            }

            return builder.ToString();
        }

        private List<List<ParentSchema>> GetDeletePath(ParentSchema topParent, ObjectSchema obj)
        {
            List<List<ParentSchema>> listPath = new List<List<ParentSchema>>();
            Stack<ParentSchema> path = new Stack<ParentSchema>();
            foreach (ParentSchema parent in obj.Parents)
            {
                this.GetDeletePath(topParent, parent, path, listPath);
            }

            return listPath;
        }

        private void GetDeletePath(ParentSchema topParent, ParentSchema parent, Stack<ParentSchema> path, List<List<ParentSchema>> listPath)
        {
            path.Push(parent);

            if (parent.Name == topParent.Name)
            {
                List<ParentSchema> deletePath = new List<ParentSchema>();
                foreach (ParentSchema schema in path)
                {
                    deletePath.Add(schema);
                }
                listPath.Add(deletePath);
            }
            else
            {
                ObjectSchema objSchema = base.GetObjectByName(parent.Name);
                if (objSchema != null)
                {
                    foreach (ParentSchema item in objSchema.Parents)
                    {
                        this.GetDeletePath(topParent, item, path, listPath);
                    }
                }
            }

            path.Pop();
        }

        #endregion

        #region Helper functions

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
        private string ToSqlDBType(FieldSchema field)
        {
            switch (field.DataType.ToLower())
            {
                case "tinyint":
                    return "SqlDbType.TinyInt";
                case "smallint":
                    return "SqlDbType.SmallInt";
                case "int":
                    return "SqlDbType.Int";
                case "bigint":
                    return "SqlDbType.BigInt";
                case "bit":
                    return "SqlDbType.Bit";
                case "decimal":
                case "numeric":
                    return "SqlDbType.Decimal";
                case "smallmoney":
                    return "SqlDbType.SmallMoney";
                case "money":
                    return "SqlDbType.Money";
                case "float":
                    return "SqlDbType.Float";
                case "real":
                    return "SqlDbType.Real";
                case "smalldatetime":
                    return "SqlDbType.SmallDateTime";
                case "datetime":
                    return "SqlDbType.DateTime";
                case "char":
                    return "SqlDbType.Char";
                case "varchar":
                    return "SqlDbType.VarChar";
                case "text":
                    return "SqlDbType.Text";
                case "nchar":
                    return "SqlDbType.NChar";
                case "nvarchar":
                    return "SqlDbType.NVarChar";
                case "ntext":
                    return "SqlDbType.NText";
                case "uniqueidentifier":
                    return "SqlDbType.UniqueIdentifier";
                case "binary":
                    return "SqlDbType.Binary";
                case "image":
                    return "SqlDbType.Image";
                case "varbinary":
                    return "SqlDbType.VarBinary";
                case "timestamp":
                    return "SqlDbType.Timestamp";
                case "sql_variant":
                    return "SqlDbType.Variant";
                default:
                    return string.Empty;
            }
        }

        private string ToSqlDBTypeSize(FieldSchema field)
        {
            switch (field.DataType.ToLower())
            {
                case "char":
                case "varchar":
                case "text":
                case "nchar":
                case "nvarchar":
                case "ntext":
                case "binary":
                case "varbinary":
                    return field.Size;
                default:
                    return string.Empty;
            }
        }
        #endregion
    }
}