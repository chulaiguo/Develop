using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryTest
{
    public class FactoryGetByModifiedOn : FactoryBase
    {
        public FactoryGetByModifiedOn(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Text;");
            writer.WriteLine("using System.Data;");
            writer.WriteLine("using System.Data.SqlClient;");
            writer.WriteLine("using {0}.Data;", base.ProjectName);
            writer.WriteLine("using {0}.Schema;", base.ProjectName);
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
                if(obj.Alias.StartsWith("AC"))
                  WriteGetByModifiedOn_D3000(writer, obj.Name);
            }
        }

        private void WriteGetByModifiedOn(StringWriter writer, string tableName)
        {
            writer.WriteLine("\tpublic partial class {0}DataService", tableName);
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tpublic {0}DataCollection GetByModifiedOn(DateTime begin, DateTime end)", tableName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tstring sqlModifiedOn = string.Format(\" WHERE  [{{0}}].[{{1}}] >= @Min{{1}} AND [{{0}}].[{{1}}] <= @Max{{1}} \", {0}Schema.TableAlias, {0}Schema.ModifiedOn);", tableName);
            writer.WriteLine();
            writer.WriteLine("\t\t\tSqlParameter[] paras = new SqlParameter[2];");
            writer.WriteLine("\t\t\tparas[0] = new SqlParameter(string.Format(\"@Min{{0}}\", {0}Schema.ModifiedOn), SqlDbType.DateTime);", tableName);
            writer.WriteLine("\t\t\tparas[0].Value = begin;");
            writer.WriteLine("\t\t\tparas[1] = new SqlParameter(string.Format(\"@Max{{0}}\", {0}Schema.ModifiedOn), SqlDbType.DateTime);", tableName);
            writer.WriteLine("\t\t\tparas[1].Value = end;");
            writer.WriteLine();
            writer.WriteLine("\t\t\tthis._dal.ClearSortBy();");
            writer.WriteLine("\t\t\tthis._dal.AddSortBy({0}Schema.ModifiedOn, true);", tableName);
            writer.WriteLine("\t\t\treturn this.GetCollection(sqlModifiedOn, paras);");
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t}");
            writer.WriteLine();
        }

        private void WriteGetByModifiedOn_D3000(StringWriter writer, string tableName)
        {
            writer.WriteLine("\tpublic partial class {0}DataService", tableName);
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tpublic {0}DataCollection GetByModifiedOn(Guid buildinPK, DateTime begin, DateTime end)", tableName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tStringBuilder builder = new StringBuilder();");
            writer.WriteLine("\t\t\tbuilder.Append(\" WHERE \");");
            writer.WriteLine("\t\t\tbuilder.AppendFormat(\"[{{0}}].[{{1}}] = @{{1}}\", {0}Schema.TableAlias, {0}Schema.BDBuildingPK);", tableName);
            writer.WriteLine("\t\t\tbuilder.AppendFormat(\" AND [{{0}}].[{{1}}] >= @Min{{1}} AND [{{0}}].[{{1}}] <= @Max{{1}}\", {0}Schema.TableAlias, {0}Schema.ModifiedOn);", tableName);
            writer.WriteLine();
            writer.WriteLine("\t\t\tSqlParameter[] paras = new SqlParameter[3];");
            writer.WriteLine("\t\t\tparas[0] = new SqlParameter(string.Format(\"@{{0}}\", {0}Schema.BDBuildingPK), SqlDbType.UniqueIdentifier);", tableName);
            writer.WriteLine("\t\t\tparas[0].Value = buildinPK;");
            writer.WriteLine("\t\t\tparas[1] = new SqlParameter(string.Format(\"@Min{{0}}\", {0}Schema.ModifiedOn), SqlDbType.DateTime);", tableName);
            writer.WriteLine("\t\t\tparas[1].Value = begin;");
            writer.WriteLine("\t\t\tparas[2] = new SqlParameter(string.Format(\"@Max{{0}}\", {0}Schema.ModifiedOn), SqlDbType.DateTime);", tableName);
            writer.WriteLine("\t\t\tparas[2].Value = end;");
            writer.WriteLine();
            writer.WriteLine("\t\t\tthis._dal.ClearSortBy();");
            writer.WriteLine("\t\t\tthis._dal.AddSortBy({0}Schema.ModifiedOn, true);", tableName);
            writer.WriteLine("\t\t\treturn this.GetCollection(builder.ToString(), paras);");
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t}");
            writer.WriteLine();
        }
    }
}
