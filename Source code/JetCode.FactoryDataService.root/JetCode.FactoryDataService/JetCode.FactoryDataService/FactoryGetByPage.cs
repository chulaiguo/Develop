using System;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryDataService
{
    public class FactoryGetByPage : FactoryBase
    {
        public FactoryGetByPage(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Text;");
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
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                if(item.Alias == "ACHistory")
                    continue;

                if (item.Alias.StartsWith("Log") || item.Alias.StartsWith("ZZ"))
                    continue;

                writer.WriteLine("\tpublic partial class {0}DataService", item.Alias);
                writer.WriteLine("\t{");
                writer.WriteLine("\t\tpublic int GetAllCount()");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tstring sql = string.Format(\"SELECT COUNT(*) FROM [{{0}}]\", {0}Schema.TableName);", item.Alias);
                writer.WriteLine("");
                writer.WriteLine("\t\t\tobject obj = this._dal.ExecuteScalar(sql, null);");
                writer.WriteLine("\t\t\tif (obj == null)");
                writer.WriteLine("\t\t\t\treturn 0;");
                writer.WriteLine("");
                writer.WriteLine("\t\t\tint count;");
                writer.WriteLine("\t\t\tif (!int.TryParse(obj.ToString(), out count))");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\treturn 0;");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("");
                writer.WriteLine("\t\t\treturn count;");
                writer.WriteLine("\t\t}");
                writer.WriteLine("");
                writer.WriteLine("\t\tpublic {0}DataCollection GetAllPage(int pageIndex, int pageSize)", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tStringBuilder builder = new StringBuilder();");
                writer.WriteLine("\t\t\tbuilder.Append(\"SELECT * FROM ( \");");
                writer.WriteLine("\t\t\tbuilder.AppendFormat(\"SELECT ROW_NUMBER() OVER( ORDER BY [{0}].[{1}]) as rowid, {2} FROM [{3}] [{4}] {5} \",");
                writer.WriteLine("\t\t\t\t{0}Schema.TableAlias, {0}Schema.ModifiedOn,", item.Alias);
                writer.WriteLine("\t\t\t\tthis._dal.SQLColumns, {0}Schema.TableName,", item.Alias);
                writer.WriteLine("\t\t\t\t{0}Schema.TableAlias, this._dal.SQLLeftJoins);", item.Alias);
                writer.WriteLine("");
                writer.WriteLine("\t\t\tbuilder.Append(\") T \");");
                writer.WriteLine("\t\t\tbuilder.AppendFormat(\"WHERE rowid BETWEEN {0} AND {1}\", pageIndex * pageSize + 1, (pageIndex + 1) * pageSize);");
                writer.WriteLine("");
                writer.WriteLine("\t\t\treturn this._dal.GetCollectionExBy(builder.ToString(), null);");
                writer.WriteLine("\t\t}");
                writer.WriteLine("\t}");

            }
        }
    }
}
