using System;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;
using System.Text;

namespace JetCode.FactoryDataService
{
    public class FactoryGetByUK : FactoryBase
    {
        public FactoryGetByUK(MappingSchema mappingSchema)
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
            bool hasUK = false;
            int index = -1;
            foreach (IndexSchema item in objSchema.Indexs)
            {
                if (!item.IsUniqueConstraint)
                    continue;

                FieldSchemaCollection list = new FieldSchemaCollection();
                foreach (string key in item.Keys)
                {
                    FieldSchema field = objSchema.Fields.Find(key);
                    if (field == null)
                        break;

                    list.Add(field);
                }

                if(!hasUK)
                {
                    hasUK = true;
                    writer.WriteLine("\tpublic partial class {0}DataService", objSchema.Alias);
                    writer.WriteLine("\t{");
                }

                if (list.Count == 1)
                {
                    this.PrintMethod(-1, writer, objSchema, list);
                }
                else
                {
                    index++;
                    this.PrintMethod(index, writer, objSchema, list);
                }
                
            }

            if(hasUK)
            {
                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }

        private void PrintMethod(int index, StringWriter writer, ObjectSchema objSchema, FieldSchemaCollection list)
        {
            if(index == -1)
            {
                writer.WriteLine("\t\tpublic {0}Data GetBy{1}({2})", objSchema.Alias, list[0].Name, this.GetParas(list));
            }
            else if (index == 0)
            {
                writer.WriteLine("\t\tpublic {0}Data GetByUK({1})", objSchema.Alias, this.GetParas(list));
            }
            else
            {
                writer.WriteLine("\t\tpublic {0}Data GetByUK{1}({2})", objSchema.Alias, index, this.GetParas(list));
            }
            writer.WriteLine("\t\t{");

            //Sql
            string and = " WHERE ";
            foreach (FieldSchema field in list)
            {
                writer.WriteLine("\t\t\tstring sql{1} = string.Format(\"{2} [{{0}}].[{{1}}] = @{{1}}\", {0}Schema.TableAlias, {0}Schema.{1}); ", field.TableAlias, field.Name, and);
                and = " AND ";
            }
            writer.WriteLine();

            //Parameter
            writer.WriteLine("\t\t\tSqlParameter[] paras = new SqlParameter[{0}];", list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                FieldSchema field = list[i];
                if (field == null)
                    continue;

                writer.WriteLine("\t\t\tparas[{0}] = new SqlParameter(string.Format(\"@{{0}}\", {1}Schema.{2}), {3});", i, field.TableAlias, field.Name, this.ToSqlDBType(field));
                writer.WriteLine("\t\t\tparas[{0}].Value = {1};", i, field.Alias);
            }
            writer.WriteLine();

            //GetEntity
            StringBuilder builder = new StringBuilder();
            foreach (Object obj in list)
            {
                FieldSchema field = obj as FieldSchema;
                if (field == null)
                    continue;

                builder.AppendFormat("sql{0} + ", field.Name);
            }

            string sqlWhere = builder.ToString().TrimEnd().TrimEnd('+');
            writer.WriteLine("\t\t\treturn this.GetEntity({0}, paras);", sqlWhere);

            writer.WriteLine("\t\t}");
        }

        private string GetParas(FieldSchemaCollection list)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < list.Count; i++)
            {
                FieldSchema field = list[i];
                if (field == null)
                    continue;

                Type fieldType = base.Utilities.ToDotNetType(field.DataType);
                builder.AppendFormat(" {0} {1},", fieldType.FullName, field.Alias);
            }

            return builder.ToString().TrimEnd(',');
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
