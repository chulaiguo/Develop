using System;
using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;
using System.Collections;
using System.Text;

namespace JetCode.FactoryDataService
{
    public class FactoryGetByUK_Old : FactoryBase
    {
        public FactoryGetByUK_Old(MappingSchema mappingSchema)
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
                //if (obj.Alias != "BDTentBadgeTemplate")
                //    continue;

                List<FieldSchemaCollection> list = new List<FieldSchemaCollection>();
                this.GetOriginalMethods(obj, list);

                if (!this.IsWriteClass(list))
                    continue;

                //Process
                SortedList<string, List<FieldSchemaCollection>> sortedList =
                    new SortedList<string, List<FieldSchemaCollection>>();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Count <= 0)
                        continue;

                    foreach (FieldSchema field in list[i])
                    {
                        if(sortedList.ContainsKey(field.Alias))
                            continue;

                        if (field.DataType == "uniqueidentifier")
                        {
                            FieldSchemaCollection fields = new FieldSchemaCollection();
                            fields.Add(field);

                            Stack stack = new Stack();
                            stack.Push(fields);
                            this.ProcessMethod(stack, obj, fields);
                            sortedList.Add(field.Alias, GetValidFields(stack));
                        }
                    }
                }

                SortedList<int, int> methodsCount = new SortedList<int, int>();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Count <= 0)
                        continue;

                    int count = 1;
                    foreach (FieldSchema field in list[i])
                    {
                        if(sortedList.ContainsKey(field.Alias))
                        {
                            count *= sortedList[field.Alias].Count;
                        }
                    }

                    methodsCount.Add(i, count);
                }

                //Write
                writer.WriteLine("\tpublic partial class {0}DataService", obj.Alias);
                writer.WriteLine("\t{");
                this.WriteOriginalUK(writer, obj);
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Count <= 0)
                        continue;

                    if(!methodsCount.ContainsKey(i))
                        continue;

                    //Init
                    FieldSchemaCollection[] methodList = new FieldSchemaCollection[methodsCount[i]];
                    for (int j = 0; j < methodsCount[i]; j++)
                    {
                        methodList[j] = new FieldSchemaCollection();
                    }

                    //Fill readable fields
                    foreach (FieldSchema field in list[i])
                    {
                        if(!sortedList.ContainsKey(field.Alias))
                        {
                            for (int j = 0; j < methodsCount[i]; j++)
                            {
                                methodList[j].Add(field);
                            }
                        }
                    }

                    //Fill PK fields
                    foreach (FieldSchema field in list[i])
                    {
                        if (sortedList.ContainsKey(field.Alias))
                        {
                            List<FieldSchemaCollection> childMethodList = sortedList[field.Alias];
                            while (childMethodList.Count < methodsCount[i])
                            {
                                childMethodList.AddRange(childMethodList);
                            }

                            for (int j = 0; j < methodsCount[i]; j++)
                            {
                                methodList[j].AddRange(childMethodList[j]);
                            }
                        }
                    }

                    //Write methods
                    for (int k = 0; k < methodList.Length; k++)
                    {
                        this.PrintMethod(k + 1 + i, writer, obj, methodList[k]);
                        writer.WriteLine();
                    }
                }

                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }

        private void WriteOriginalUK(StringWriter writer, ObjectSchema objSchema)
        {
            foreach (IndexSchema item in objSchema.Indexs)
            {
                if (!item.IsUniqueConstraint)
                    continue;

                bool isFK = true;
                FieldSchemaCollection list = new FieldSchemaCollection();
                foreach (string key in item.Keys)
                {
                    FieldSchema field = objSchema.Fields.Find(key);
                    if (field == null)
                        break;

                    isFK = this.IsFK(field.Name, objSchema.Parents);
                    if (!isFK)
                    {
                        break;
                    }

                    list.Add(field);
                }

                if (isFK)
                {
                    this.PrintMethod(0, writer, objSchema, list);
                }
            }
        }

        private bool IsFK(string fieldName, ParentSchemaCollection list)
        {
            foreach (ParentSchema parent in list)
            {
                if (parent.LocalColumn == fieldName)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsWriteClass(List<FieldSchemaCollection> list)
        {
            foreach (FieldSchemaCollection fields in list)
            {
                if(fields.Count > 0)
                    return true;
            }

            return false;
        }

        private List<FieldSchemaCollection> GetValidFields(Stack stack)
        {
            List<FieldSchemaCollection> retList = new List<FieldSchemaCollection>();
            object[] arr = stack.ToArray();
            for (int i = 0; i < arr.Length; i++)
            {
                FieldSchemaCollection fieldList = arr[i] as FieldSchemaCollection;
                if (fieldList == null)
                    continue;

                if (!this.IsFieldListValid(fieldList))
                    continue;

                retList.Add(fieldList);
            }

            return retList;
        }

        private bool IsFieldListValid(FieldSchemaCollection fieldList)
        {
            foreach (FieldSchema item in fieldList)
            {
                if (item.DataType == "uniqueidentifier")
                    return false;
            }

            return true;
        }

        private void PrintMethod(int index, StringWriter writer, ObjectSchema objSchema, FieldSchemaCollection list)
        {
            if(index == 0)
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

        private void GetOriginalMethods(ObjectSchema objSchema, List<FieldSchemaCollection> list)
        {
            foreach (IndexSchema item in objSchema.Indexs)
            {
                if (!item.IsUniqueConstraint)
                    continue;

                FieldSchemaCollection fieldList = new FieldSchemaCollection();
                list.Add(fieldList);
                foreach (string key in item.Keys)
                {
                    FieldSchema field = objSchema.Fields.Find(key);
                    if (field == null)
                        break;

                    fieldList.Add(field);
                }
            }
        }

        private void ProcessMethod(Stack stack, ObjectSchema objSchema, FieldSchemaCollection fieldList)
        {
            foreach (FieldSchema field in fieldList)
            {
               if(field.DataType != "uniqueidentifier")
                   continue;

               ObjectSchema parentSchema = this.GetParentSchema(objSchema, field);
               if (parentSchema == null)
                   continue;
           
                List<FieldSchemaCollection> children = new List<FieldSchemaCollection>();
                this.GetOriginalMethods(parentSchema, children);
                foreach (FieldSchemaCollection child in children)
                {
                    FieldSchemaCollection clone = this.CloneFieldList(fieldList, field);
                    clone.AddRange(child);
                    stack.Push(clone);

                    this.ProcessMethod(stack, parentSchema, clone);
                }
            }
        }

        private FieldSchemaCollection CloneFieldList(FieldSchemaCollection fieldList, FieldSchema exclude)
        {
            FieldSchemaCollection retList = new FieldSchemaCollection();
            foreach (FieldSchema item in fieldList)
            {
                if(item.Name == exclude.Name)
                    continue;

                retList.Add(item.Clone());
            }

            return retList;
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
