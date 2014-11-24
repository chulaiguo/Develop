using System;
using System.Data;
using System.Data.SqlClient;

namespace JetCode.DBSchema.Factory
{
    public class MSSQLSchemaFactory
    {
        private string _dbName = string.Empty;
        private string _connectionString = string.Empty;

        public MSSQLSchemaFactory(string database, string connectionString)
        {
            this._dbName = database;
            this._connectionString = connectionString;
        }

        public MSSQLSchemaFactory(string server, string database, string user, string pswd, bool byWindows)
        {
            this._dbName = database;
            if (byWindows)
            {
                this._connectionString = "Data Source=" + server + ";Initial Catalog=" + database + ";Integrated Security=SSPI;";
            }
            else
            {
                this._connectionString = "Data Source=" + server + ";Initial Catalog=" + database + ";User ID=" + user + ";Password=" +
                                         pswd + ";";
            }
        }

        public DatabaseSchema LoadDatabaseSchema()
        {
            DatabaseSchema schema = new DatabaseSchema(this._dbName);
            this.LoadTableSchema(schema.Tables);

            return schema;
        }

        private DataTable GetDataTabe(string sql)
        {
            DataSet dataset = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(sql, this._connectionString);
            adapter.Fill(dataset, "TableName");

            return dataset.Tables["TableName"];
        }

        private void LoadTableSchema(TableSchemaCollection tables)
        {
            string sql = "SELECT Table_Schema, Table_Name FROM Information_Schema.Tables "
                         + " WHERE Table_Type = 'Base Table' AND Table_Name <> 'dtproperties' AND Table_Name <> 'sysdiagrams'";

            DataTable table = this.GetDataTabe(sql);
            foreach (DataRow row in table.Rows)
            {
                string tableName = row["TABLE_NAME"].ToString();

                TableSchema schema = new TableSchema(tableName);
                this.LoadColumnSchema(tableName, schema.Columns);
                this.LoadRelationshipSchema(tableName, schema.Relationship);
                this.LoadTableIndexes(tableName, schema.Indexs);

                tables.Add(schema);
            }
        }

        private void LoadColumnSchema(string tableName, ColumnSchemaCollection list)
        {
            string sql = string.Format("sp_MShelpcolumns N'{0}', null, 'id', 1", tableName);

            DataTable table = this.GetDataTabe(sql);
            foreach (DataRow row in table.Rows)
            {
                string name = row["col_name"].ToString();
                ColumnSchema schema = new ColumnSchema(name);

                //IsPK
                if (((int) row["col_flags"]) >= 4)
                {
                    schema.IsPK = true;
                }
                else
                {
                    schema.IsPK = false;
                }

                //type
                schema.DataType = row["col_typename"].ToString();
                if (row["col_prec"] == DBNull.Value)
                {
                    schema.Size = row["col_len"].ToString();
                }
                else
                {
                    string typeName = row["col_basetypename"].ToString();
                    if ((!typeName.EndsWith("int") && (typeName != "real")) && (typeName != "float"))
                    {
                        schema.Size = string.Format("{0} ({1},{2})", row["col_len"], row["col_prec"], row["col_scale"]);
                    }
                    else
                    {
                        schema.Size = row["col_len"].ToString();
                    }
                }

                //IsNull
                schema.IsNullable = (bool) row["col_null"];

                list.Add(schema);
            }
        }

        private void LoadRelationshipSchema(string tableName, RelationshipSchemaCollection list)
        {
            string sql = string.Format("sp_MStablerefs N'{0}', N'actualtables', N'both', null", tableName);

            DataTable table = this.GetDataTabe(sql);
            foreach (DataRow row in table.Rows)
            {
                string name = row["Constraint"].ToString();
                RelationshipSchema schema = new RelationshipSchema(name);
                if (row["PK_Table"].ToString() != tableName)
                {
                    schema.IsParent = true;
                    schema.PKTableName = row["FK_Table"].ToString();
                    schema.FKTableName = row["PK_Table"].ToString();
                    schema.LocalColumnName = row["cKeyCol1"].ToString();
                    schema.RemoteColumnName = row["cRefCol1"].ToString();
                }
                else
                {
                    schema.IsParent = false;
                    schema.PKTableName = row["PK_Table"].ToString();
                    schema.FKTableName = row["FK_Table"].ToString();
                    schema.RemoteColumnName = row["cKeyCol1"].ToString();
                    schema.LocalColumnName = row["cRefCol1"].ToString();
                }

                list.Add(schema);
            }
        }

        private void LoadTableIndexes(string tableName, DBIndexSchemaCollection list)
        {
            string sql = string.Format("sp_MShelpindex N'{0}', null, 1", tableName);

            DataTable table = this.GetDataTabe(sql);
            foreach (DataRow row in table.Rows)
            {
                DBIndexSchema schema = new DBIndexSchema(row["name"].ToString());
                if ((((int)row["status"]) & 0x800) > 0)
                {
                    schema.IsPrimaryKey = true;
                }
                else if ((((int)row["status"]) & 0x1000) > 0)
                {
                    schema.IsUniqueConstraint = true;
                }
                else
                {
                }

                for (int index = 4; index < 20; index++ )
                {
                    if(row[index] == DBNull.Value)
                        break;

                    schema.Keys.Add(row[index].ToString());
                }
                
                list.Add(schema);
            }
        }

    }
}