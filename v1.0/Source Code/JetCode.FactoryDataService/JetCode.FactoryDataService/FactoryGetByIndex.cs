using System;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryDataService
{
    public class FactoryGetByIndex : FactoryBase
    {
        public FactoryGetByIndex(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Data;");
            writer.WriteLine("using {0}.Data;", base.ProjectName);
            writer.WriteLine("using {0}.Schema;", base.ProjectName);
            writer.WriteLine("using System.Data.SqlClient;");

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.DataService", base.ProjectName);
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
                this.WriteOriginalUK(writer, obj);
            }
        }

        private void WriteOriginalUK(StringWriter writer, ObjectSchema objSchema)
        {
            bool hasIndex = false;
            foreach (IndexSchema item in objSchema.Indexs)
            {
                if (item.IsUniqueConstraint)
                    continue;

                FieldSchemaCollection list = new FieldSchemaCollection();
                foreach (string key in item.Keys)
                {
                    FieldSchema field = objSchema.Fields.Find(key);
                    if (field == null)
                        break;

                    list.Add(field);
                }

                if(list.Count != 1)
                    continue;

                Type fieldType = base.Utilities.ToDotNetType(list[0].DataType);
                if(fieldType == typeof(Guid))
                    continue;

                if (!hasIndex)
                {
                    hasIndex = true;
                    writer.WriteLine("\tpublic partial class {0}DataService", objSchema.Alias);
                    writer.WriteLine("\t{");
                }

                this.PrintMethod(writer, objSchema, list[0]);
            }

            if (hasIndex)
            {
                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }

        private void PrintMethod(StringWriter writer, ObjectSchema objSchema, FieldSchema field)
        {
            Type fieldType = base.Utilities.ToDotNetType(field.DataType);
            if(fieldType == typeof(DateTime))
            {
                writer.WriteLine("\t\tpublic {0}DataCollection GetBy{1}(DateTime begin, DateTime end)", objSchema.Alias, field.Name);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tstring sql = string.Format(\" WHERE [{{0}}].[{{1}}] >= @Min{{1}} AND [{{0}}].[{{1}}] <= @Max{{1}}\", {0}Schema.TableAlias, {0}Schema.{1}); ", objSchema.Alias, field.Name);
                writer.WriteLine();
                writer.WriteLine("\t\t\tSqlParameter[] paras = new SqlParameter[2];");
                writer.WriteLine("\t\t\tparas[0] = new SqlParameter(string.Format(\"@Min{{0}}\", {0}Schema.{1}), {2});", objSchema.Alias, field.Name, this.ToSqlDBType(field));
                writer.WriteLine("\t\t\tparas[0].Value = begin;");
                writer.WriteLine("\t\t\tparas[1] = new SqlParameter(string.Format(\"@Max{{0}}\", {0}Schema.{1}), {2});", objSchema.Alias, field.Name, this.ToSqlDBType(field));
                writer.WriteLine("\t\t\tparas[1].Value = end;");
                writer.WriteLine();
                writer.WriteLine("\t\t\tthis._dal.ClearSortBy();");
                writer.WriteLine("\t\t\tthis._dal.AddSortBy({0}Schema.{1}, false);", objSchema.Alias, field.Name);
                writer.WriteLine("\t\t\treturn this.GetCollection(sql, paras);");
                writer.WriteLine("\t\t}");
            }
            else if(fieldType == typeof(string))
            {
                writer.WriteLine("\t\tpublic {0}DataCollection GetByLike{1}({2} {1})", objSchema.Alias, field.Name, fieldType.FullName);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tstring sql = string.Format(\" WHERE [{{0}}].[{{1}}] LIKE @{{1}} \", {0}Schema.TableAlias, {0}Schema.{1}); ", objSchema.Alias, field.Name);
                writer.WriteLine();
                writer.WriteLine("\t\t\tSqlParameter[] paras = new SqlParameter[1];");
                writer.WriteLine("\t\t\tparas[0] = new SqlParameter(string.Format(\"@{{0}}\", {0}Schema.{1}), {2});", objSchema.Alias, field.Name, this.ToSqlDBType(field));
                writer.WriteLine("\t\t\tparas[0].Value = string.Format(\"{{0}}%\", {0});", field.Name);
                writer.WriteLine();
                writer.WriteLine("\t\t\treturn this.GetCollection(sql, paras);");
                writer.WriteLine("\t\t}");
            }
            else
            {
                writer.WriteLine("\t\tpublic {0}DataCollection GetBy{1}({2} {1})", objSchema.Alias, field.Name, fieldType.FullName);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tstring sql = string.Format(\" WHERE [{{0}}].[{{1}}] = @{{1}} \", {0}Schema.TableAlias, {0}Schema.{1}); ", objSchema.Alias, field.Name);
                writer.WriteLine();
                writer.WriteLine("\t\t\tSqlParameter[] paras = new SqlParameter[1];");
                writer.WriteLine("\t\t\tparas[0] = new SqlParameter(string.Format(\"@{{0}}\", {0}Schema.{1}), {2});", objSchema.Alias, field.Name, this.ToSqlDBType(field));
                writer.WriteLine("\t\t\tparas[0].Value = {0};", field.Name);
                writer.WriteLine();
                writer.WriteLine("\t\t\treturn this.GetCollection(sql, paras);");
                writer.WriteLine("\t\t}");
            }
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
    }
}
