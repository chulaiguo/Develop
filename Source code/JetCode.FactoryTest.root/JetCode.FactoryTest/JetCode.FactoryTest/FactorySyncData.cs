using System.Collections.Specialized;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;
namespace JetCode.FactoryTest
{
    public class FactorySyncData : FactoryBase
    {
        public FactorySyncData(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System.Text;");
            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.SyncService.Core", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic static class SyncData");
            writer.WriteLine("\t{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            MappingInsertOrder order = new MappingInsertOrder(base.MappingSchema);
            StringCollection insertOrderList = order.GetInsertOrder();

            //Data
            foreach (string item in insertOrderList)
            {
                if(item.StartsWith("ZZ"))
                    continue;

                if (item == "CacheOffline" || item == "LogDeleteCard" || item == "ACDownloadTask"
                    || item == "BDSiteCommand" || item == "BDSiteResponse" || item == "BDPanelCommand" || item == "ACTimeSheet")
                    continue;

                this.WriteSyncData(writer, item);
            }

            //Clear SQL
            writer.WriteLine("\t\tpublic static string GetClearDatabaseSql(bool clearCacheOffline)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tStringBuilder builder = new StringBuilder();");
            for (int i = insertOrderList.Count - 1; i >= 0; i--)
            {
                if (insertOrderList[i].StartsWith("ZZ"))
                    continue;

                if (insertOrderList[i] == "CacheOffline")
                {
                    writer.WriteLine("\t\t\tif(clearCacheOffline)");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tbuilder.Append(\"DELETE FROM [{0}];\");", insertOrderList[i]);
                    writer.WriteLine("\t\t\t}");
                }
                else
                {
                    writer.WriteLine("\t\t\tbuilder.Append(\"DELETE FROM [{0}];\");", insertOrderList[i]);
                }
            }
            writer.WriteLine("\t\t\treturn builder.ToString();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private string GetD3000TableName(string tableName)
        {
            return tableName;
        }

        private void WriteSyncData(StringWriter writer, string tableName)
        {
            string d3000tableName = this.GetD3000TableName(tableName);
            this.WriteSyncData(writer, tableName, d3000tableName);
        }

        private void WriteSyncData(StringWriter writer, string tableName, string d3000tableName)
        {
            writer.WriteLine("\t\tpublic static void Sync{0}(D3000.Data.{1}Data src, E3000.Data.{0}Data dst)", tableName, d3000tableName);
            writer.WriteLine("\t\t{");
            ObjectSchema obj = base.GetObjectByName(tableName);
            if (obj != null)
            {
                foreach (FieldSchema field in obj.Fields)
                {
                    if (field.Alias == "RowVersion")
                        continue;

                    if (field.Alias == "Active")
                    {
                        if (obj.Alias == "ACCardHolderBuildingMap" || obj.Alias == "ACPanelFunctionCardMap" ||
                            obj.Alias == "ACVisitor")
                        {
                            continue;
                        }
                    }

                    if (obj.Alias == "ACPanel" && field.Alias == "LastConnected")
                    {
                        continue;
                    }

                    writer.WriteLine("\t\t\tdst.{0} = src.{0};", field.Alias);
                }

                if (obj.Alias == "ACCardHolderBuildingMap" || obj.Alias == "ACPanelFunctionCardMap")
                {
                    writer.WriteLine("\t\t\tdst.Active = dst.IsValidCard;");
                }

                if (obj.Alias == "ACVisitor")
                {
                    writer.WriteLine("\t\t\tdst.Active = dst.IsValidVisitor;");
                }
            }
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }
    }
}
