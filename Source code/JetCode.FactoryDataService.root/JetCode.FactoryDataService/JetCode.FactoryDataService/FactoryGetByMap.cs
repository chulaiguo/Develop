using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryDataService
{
    public class FactoryGetByMap : FactoryBase
    {
        public FactoryGetByMap(MappingSchema mappingSchema)
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
                List<FieldSchema> pkList = obj.GetPKList();
                if(pkList.Count != 1)
                    continue;

                List<ObjectSchema> list = GetMapParents(obj);
                if(list.Count == 0)
                    continue;

                FieldSchema hostPKField = pkList[0];
                writer.WriteLine("\tpublic partial class {0}DataService", obj.Alias);
                writer.WriteLine("\t{");
                foreach (ObjectSchema mapSchema in list)
                {
                    ObjectSchema anotherParent = GetAnotherParent(obj, mapSchema);
                    if(anotherParent == null)
                        continue;

                    pkList = anotherParent.GetPKList();
                    if (pkList.Count != 1)
                        continue;

                    FieldSchema anotherParentPKField = pkList[0];

                    //GetByMapped...
                    writer.WriteLine("\t\tpublic {0}DataCollection GetByMapped{1}(Guid {2})", obj.Alias, anotherParent.Alias, anotherParentPKField.Alias);
                    writer.WriteLine("\t\t{");

                    //Sql
                    writer.WriteLine("\t\t\tstring sql = string.Format(\" WHERE [{{0}}].[{{1}}] IN (SELECT [{{2}}] FROM [{{3}}] WHERE [{{4}}] = @{{4}})\","
                    + " {0}Schema.TableAlias, {0}Schema.{1}, {2}Schema.{1}, {2}Schema.TableName, {2}Schema.{3});",
                        obj.Alias, hostPKField.Alias, mapSchema.Alias, anotherParentPKField.Alias);
                    writer.WriteLine();

                    //Parameter
                    writer.WriteLine("\t\t\tSqlParameter[] paras = new SqlParameter[1];");
                    writer.WriteLine("\t\t\tparas[0] = new SqlParameter(string.Format(\"@{{0}}\", {0}Schema.{1}), SqlDbType.UniqueIdentifier);", mapSchema.Alias, anotherParentPKField.Alias);
                    writer.WriteLine("\t\t\tparas[0].Value = {0};", anotherParentPKField.Alias);
                    writer.WriteLine();

                    //return
                    writer.WriteLine("\t\t\treturn this.GetCollection(sql, paras);");
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();

                    ////GetViewByMapped...
                    //writer.WriteLine("\t\tpublic {0}ViewCollection GetViewByMapped{1}(Guid {2})", obj.Alias, anotherParent.Alias, anotherParentPKField.Alias);
                    //writer.WriteLine("\t\t{");
                    //writer.WriteLine("\t\t\treturn new {0}ViewCollection(this.GetByMapped{1}({2}));", obj.Alias, anotherParent.Alias, anotherParentPKField.Alias);
                    //writer.WriteLine("\t\t}");
                    //writer.WriteLine();
               }
                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }

        private List<ObjectSchema> GetMapParents(ObjectSchema obj)
        {
            List<ObjectSchema> retList = new List<ObjectSchema>();
            foreach (ChildSchema item in obj.Children)
            {
                ObjectSchema childSchema = base.GetObjectByName(item.Name);
                if(!IsMapTable(childSchema))
                    continue;
                //List<FieldSchema> pkList = childSchema.GetPKList();
                //if (pkList.Count != 2)
                //    continue;

                retList.Add(childSchema);
            }

            return retList;
        }

        private ObjectSchema GetAnotherParent(ObjectSchema obj, ObjectSchema childSchema)
        {
            foreach (ParentSchema item in childSchema.Parents)
            {
                if(item.Alias == obj.Alias)
                    continue;

                return base.GetObjectByName(item.Name);
            }

            return null;
        }
    }
}
