using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryDataService
{
    public class FactoryCRUDLite : FactoryBase
    {
        public FactoryCRUDLite(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Data;");
            writer.WriteLine("using System.Data.SqlClient;");
            writer.WriteLine("using System.Collections.ObjectModel;");

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.DAL", base.ProjectName);
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
                writer.WriteLine("\tpublic partial class DAL{0} : DALBase", obj.Alias);
                writer.WriteLine("\t{");

                this.WriteConstructor(writer, obj);

                this.WriteInsert(writer, obj);
                this.WriteUpdate(writer, obj);

                this.WriteFetchEntity(writer, obj);
                this.WriteGetByPK(writer, obj);
                this.WriteGetAll(writer, obj);
                this.WriteGetByParent(writer, obj);

                this.WriteDeleteByPK(writer, obj);
                this.WriteDeleteAll(writer, obj);
                this.WriteDeleteByParent(writer, obj);

                writer.WriteLine();
                writer.WriteLine("\t}");
            }
        }

        private void WriteConstructor(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tprivate string _tableName = \"{0}\";", obj.Name);
            writer.WriteLine();

            writer.WriteLine("\t\tpublic string TableName");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._tableName; }");
            writer.WriteLine("\t\t\tset { this._tableName = value; }");
            writer.WriteLine("\t\t}");

            writer.WriteLine();
        }

        private void WriteInsert(StringWriter writer, ObjectSchema obj)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("INSERT INTO [{{0}}] (");
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
            writer.WriteLine("\t\t\tstring sql = string.Format(\"{0}\", this.TableName);", sql);
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
            sqlBuilder.AppendFormat("UPDATE [{{0}}] SET ");
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
            writer.WriteLine("\t\t\tstring sql = string.Format(\"{0}\", this.TableName);", sql);
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
            writer.WriteLine("\t\tprivate ObservableCollection<{0}Data> FetchData(string sqlWhere, SqlParameter[] paras)", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tObservableCollection<{0}Data> retList = new ObservableCollection<{0}Data>();", obj.Alias);
            writer.WriteLine();
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < obj.Fields.Count; i++)
            {
                FieldSchema item = obj.Fields[i];
                if (i == obj.Fields.Count - 1)
                {
                    builder.AppendFormat("{0}", item.Alias);
                }
                else
                {
                    builder.AppendFormat("{0},", item.Alias);
                }
            }
            writer.WriteLine("\t\t\tstring sql = string.Format(\"SELECT {0} FROM [{{0}}] {{1}}\", this.TableName, sqlWhere);", builder);
            writer.WriteLine("\t\t\tDataSet dataSet = this.ExecuteDataset(sql, paras);");
            writer.WriteLine("\t\t\tif (dataSet.Tables.Count == 0)");
            writer.WriteLine("\t\t\t\treturn retList;");
            writer.WriteLine();
            writer.WriteLine("\t\t\tforeach (DataRow row in dataSet.Tables[0].Rows)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t{0}Data data = new {0}Data();", obj.Name);
            writer.WriteLine();
            for (int i = 0; i < obj.Fields.Count; i++)
            {
                FieldSchema item = obj.Fields[i];

                writer.WriteLine("\t\t\t\tobject obj{0} = row[{1}];", item.Alias, i);

                Type fieldType = Utilities.ToDotNetType(item.DataType);
                if (fieldType.IsValueType)
                {
                    if (fieldType == typeof(Guid))
                    {
                        writer.WriteLine("\t\t\t\tdata.{0} = Guid.Empty;", item.Alias);
                    }
                    else if (fieldType == typeof(DateTime))
                    {
                        writer.WriteLine("\t\t\t\tdata.{0} = DateTime.Now;", item.Alias);
                    }
                    else if (fieldType == typeof(bool))
                    {
                        writer.WriteLine("\t\t\t\tdata.{0} = false;", item.Alias);
                    }
                    else
                    {
                        writer.WriteLine("\t\t\t\tdata.{0} = 0;", item.Alias);
                    }
                    
                    writer.WriteLine("\t\t\t\tif (obj{0} != null)", item.Alias);
                    writer.WriteLine("\t\t\t\t{");
                    writer.WriteLine("\t\t\t\t\t{0} value;", fieldType.Name);
                    writer.WriteLine("\t\t\t\t\tif ({0}.TryParse(obj{1}.ToString(), out value))", fieldType.Name, item.Alias);
                    writer.WriteLine("\t\t\t\t\t{");
                    writer.WriteLine("\t\t\t\t\t\tdata.{0} = value;", item.Alias);
                    writer.WriteLine("\t\t\t\t\t}");
                    writer.WriteLine("\t\t\t\t}");
                }
                else
                {
                    if (fieldType == typeof(string))
                    {
                        writer.WriteLine("\t\t\t\tif (obj{0} != null)", item.Alias);
                        writer.WriteLine("\t\t\t\t{");
                        writer.WriteLine("\t\t\t\t\tdata.{0} = obj{0}.ToString();", item.Alias);
                        writer.WriteLine("\t\t\t\t}");
                        writer.WriteLine("\t\t\t\telse");
                        writer.WriteLine("\t\t\t\t{");
                        writer.WriteLine("\t\t\t\t\tdata.{0} = string.Empty;", item.Alias);
                        writer.WriteLine("\t\t\t\t}");
                    }
                    else
                    {
                        writer.WriteLine("\t\t\t\tdata.{0} = obj{0} as {1};", item.Alias, fieldType);
                    }
                }

                writer.WriteLine();
            }
            writer.WriteLine("\t\t\t\tretList.Add(data);");
            writer.WriteLine("\t\t\t}");

            writer.WriteLine();
            writer.WriteLine("\t\t\treturn retList;");
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
                sqlBuilder.AppendFormat(" [{0}] = @{0} AND", item.Name);
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

            writer.WriteLine("\t\t\tObservableCollection<{0}Data> retList = this.FetchData(sql, paras);", obj.Alias);
            writer.WriteLine("\t\t\tif(retList.Count > 0)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn retList[0];");
            writer.WriteLine("\t\t\t}");
           
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn null;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteGetAll(StringWriter writer, ObjectSchema obj)
        {
            writer.WriteLine("\t\tpublic ObservableCollection<{0}Data> GetAll()", obj.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn this.FetchData(\"\", null);");
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

                writer.WriteLine("\t\tpublic ObservableCollection<{0}Data> GetBy{1}({2} {3})", obj.Alias, item.Alias, 
                                 base.Utilities.ToDotNetType(field.DataType), field.Name);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tstring sqlWhere = \" WHERE [{0}] = @{0}\";", field.Name);
                writer.WriteLine("\t\t\tSqlParameter[] paras = new SqlParameter[1];");
                writer.WriteLine("\t\t\tparas[0] = new SqlParameter(\"@{0}\", {1});", field.Name, this.ToSqlDBType(field));
                writer.WriteLine("\t\t\tparas[0].Value = {0};", field.Name);
                writer.WriteLine("\t\t\treturn this.FetchData(sqlWhere, paras);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }
        }


        private void WriteDeleteByPK(StringWriter writer, ObjectSchema obj)
        {
            FieldSchema rowVersion = obj.GetRowVersion();
            List<FieldSchema> pkList = obj.GetPKList();

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("DELETE FROM [{{0}}] WHERE ");
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
            writer.WriteLine("\t\t\tstring sql = string.Format(\"{0}\", this.TableName);", sql);

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
            writer.WriteLine("\t\t\tstring sql = string.Format(\"DELETE FROM [{0}]\", this.TableName);");
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
                writer.WriteLine("\t\t\tstring sql = string.Format(\"{0}\", this.TableName);", this.GetDeleteByParentSql(item, obj));
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
                builder.AppendFormat("DELETE [{{0}}] FROM [{{0}}] [{0}] ", obj.Alias);
                for (int i = path.Count - 1; i >= 0; i--)
                {
                    if (i == path.Count - 1)
                    {
                        builder.AppendFormat("INNER JOIN [{0}] ON [{1}].[{2}] = [{0}].[{3}] ",
                                             path[i].Name, obj.Alias, path[i].LocalColumn, path[i].RemoteColumn); 
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