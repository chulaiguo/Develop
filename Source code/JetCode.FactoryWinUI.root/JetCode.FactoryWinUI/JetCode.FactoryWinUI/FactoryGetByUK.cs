using System;
using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;
using System.Collections;
using System.Text;

namespace JetCode.FactoryWinUI
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
                List<Stack> list = new List<Stack>();
                Stack currentStack = new Stack();
                list.Add(currentStack);
                this.GetIndex(obj, currentStack, list);

                if (!this.IsWriteClass(list))
                    continue;

                writer.WriteLine("\tpublic partial class {0}DataService", obj.Alias);
                writer.WriteLine("\t{");
                for (int i = 0; i < list.Count; i++)
                {
                    if(list[i].Count <= 1)
                        continue;

                    this.PrintMethod(i + 1, writer, obj, list[i]);
                    writer.WriteLine();
                }
                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }

        private bool IsWriteClass(List<Stack> list)
        {
            foreach (Stack item in list)
            {
                if(item.Count > 1)
                    return true;
            }

            return false;
        }

        private void PrintMethod(int index, StringWriter writer, ObjectSchema objSchema, Stack item)
        {
            object[] list = item.ToArray();
            if(list.Length == 0)
                return;

            writer.WriteLine("\t\tpublic {0}Data GetByUK{1}({2})", objSchema.Alias, index, this.GetParas(list));
            writer.WriteLine("\t\t{");

            //Sql
            string and = " WHERE ";
            foreach (Object obj in list)
            {
                FieldSchema field = obj as FieldSchema;
                if (field == null)
                    continue;

                writer.WriteLine("\t\t\tstring sql{1} = string.Format(\"{2} [{{0}}] = @{{0}}\", {0}Schema.{1}); ", field.TableAlias, field.Name, and);
                and = " AND ";
            }
            writer.WriteLine();

            //Parameter
            writer.WriteLine("\t\t\tSqlParameter[] paras = new SqlParameter[{0}];", item.Count);
            for (int i = 0; i < list.Length; i++)
            {
                FieldSchema field = list[i] as FieldSchema;
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


        private void GetIndex(ObjectSchema objSchema, Stack currentStack, List<Stack> list)
        {
            int index = 0;
            Stack originalStack = currentStack.Clone() as Stack;
            foreach (IndexSchema item in objSchema.Indexs)
            {
                if (!item.IsUniqueConstraint)
                    continue;

                if(index >= 1)
                {
                    currentStack = originalStack.Clone() as Stack;
                    list.Add(currentStack);
                }

                FieldSchemaCollection pkFileds = this.AddReadableIndex(objSchema, item, currentStack);
                if (pkFileds.Count == 1)
                {
                    ObjectSchema parentSchema = this.GetParentSchema(objSchema, pkFileds[0]);
                    if (parentSchema == null)
                        continue;

                    this.GetIndex(parentSchema, currentStack, list);
                }

                index++;
            }
        }

        private ObjectSchema GetParentSchema(ObjectSchema objSchema, FieldSchema field)
        {
            foreach (ParentSchema parent in objSchema.Parents)
            {
                if (parent.LocalColumn != field.Name)
                    continue;

                ObjectSchema parentSchema = this.MappingSchema.Objects.Find(parent.Name);
                if (parentSchema == null)
                    continue;

                return parentSchema;
            }

            return null;
        }

        private FieldSchemaCollection AddReadableIndex(ObjectSchema objSchema, IndexSchema item, Stack currentStack)
        {
            FieldSchemaCollection retList= new FieldSchemaCollection();
            foreach (string key in item.Keys)
            {
                FieldSchema field = objSchema.Fields.Find(key);
                if (field == null)
                    break;

                if (field.DataType != "uniqueidentifier" || retList.Count > 0)
                {
                    currentStack.Push(field);
                }
                else
                {
                    retList.Add(field);
                }
            }

            return retList;
        }

        private string GetParas(object[] list)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < list.Length; i++)
            {
                FieldSchema field = list[i] as FieldSchema;
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
