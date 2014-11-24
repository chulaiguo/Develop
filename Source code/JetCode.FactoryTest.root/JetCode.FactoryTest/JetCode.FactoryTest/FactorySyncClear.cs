using System.Collections.Specialized;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;
namespace JetCode.FactoryTest
{
    public class FactorySyncClear : FactoryBase
    {
        public FactorySyncClear(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.SyncService", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            MappingInsertOrder order = new MappingInsertOrder(base.MappingSchema);
            StringCollection insertOrderList = order.GetInsertOrder();

            //insert table
            writer.WriteLine("\t\tpublic string GetClearDatabaseSql()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tStringBuilder builder = new StringBuilder();");
            for (int i = insertOrderList.Count - 1; i >= 0; i--)
            {
                if (insertOrderList[i].StartsWith("ZZ"))
                    continue;

                writer.WriteLine("\t\t\tbuilder.Append(\"DELETE FROM [{0}];\");", insertOrderList[i]);
            }
            writer.WriteLine("\t\t\treturn builder.ToString();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }
    }
}
