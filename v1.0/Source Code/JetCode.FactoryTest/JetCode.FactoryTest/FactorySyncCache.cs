using System.Collections.Specialized;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;
namespace JetCode.FactoryTest
{
    public class FactorySyncCache : FactoryBase
    {
        public FactorySyncCache(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using Cheke;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using {0}.BasicServiceWrapper;", base.ProjectName);
            writer.WriteLine("using {0}.Data;", base.ProjectName);
            writer.WriteLine("using {0}.SyncService.Helper;", base.ProjectName);
            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.SyncService.Core", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic static class SyncCache");
            writer.WriteLine("\t{");
            writer.WriteLine();
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            StringCollection list = this.GetTableList();

            //insert table
            writer.WriteLine("\t\tpublic static void UpdateCacheOffline(CacheOfflineData item, SecurityToken token)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tswitch (item.DataType)");
            writer.WriteLine("\t\t\t{");
            foreach (string item in list)
            {
                writer.WriteLine("\t\t\t\tcase D3000.Schema.{0}Schema.TableName:", item);
                writer.WriteLine("\t\t\t\t\tSyncCache.Update{0}Cache(item, token);", item);
                writer.WriteLine("\t\t\t\t\tbreak;");
            }
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            this.WriteSyncTable(list, writer);
        }

        private StringCollection GetTableList()
        {
            MappingInsertOrder order = new MappingInsertOrder(base.MappingSchema);
            StringCollection insertOrderList = order.GetInsertOrder();

            StringCollection list = new StringCollection();
            foreach (string item in insertOrderList)
            {
                if (item == "CacheOffline" || item == "LogDeleteCard" || item == "ACDownloadTask"
                    || item == "BDSiteCommand" || item == "BDSiteResponse" || item == "BDPanelCommand")
                    continue;

                list.Add(item);
            }

            return list;
        }

        private void WriteSyncTable(StringCollection list, StringWriter writer)
        {
            foreach (string item in list)
            {
                writer.WriteLine("\t\tprivate static void Update{0}Cache(CacheOfflineData item, SecurityToken token)", item);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tD3000.Data.{0}Data data = new D3000.Data.{0}Data();", item);
                writer.WriteLine("\t\t\tSerializeHelper.DeserializeObject(data, item.ObjectStream);");
                writer.WriteLine("");
                writer.WriteLine("\t\t\tD3000.Data.{0}Data entity;", item);
                writer.WriteLine("\t\t\tif (!item.IsInsertAction)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tentity = D3000.BasicServiceWrapper.{0}Wrapper.GetByPK(item.RecordPK, token);", item);
                writer.WriteLine("\t\t\t\tif (item.IsDeleteAction)");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tif (entity == null)");
                writer.WriteLine("\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\tCacheOfflineWrapper.DeleteByPK(item.CacheOfflinePK, token);");
                writer.WriteLine("\t\t\t\t\t\treturn;");
                writer.WriteLine("\t\t\t\t\t}");
                writer.WriteLine("");
                writer.WriteLine("\t\t\t\t\tentity.Delete();");
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("\t\t\t\telse");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tif (entity == null)");
                writer.WriteLine("\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\tentity = new D3000.Data.{0}Data();", item);
                writer.WriteLine("\t\t\t\t\t\tentity.{0}PK = item.RecordPK;", item);
                writer.WriteLine("\t\t\t\t\t\tentity.CopyFrom(data, false);");
                writer.WriteLine("\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\telse");
                writer.WriteLine("\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\tbyte[] rowversion = entity.RowVersion;");
                writer.WriteLine("\t\t\t\t\t\tentity.CopyFrom(data, false);");
                writer.WriteLine("\t\t\t\t\t\tentity.RowVersion = rowversion;");
                writer.WriteLine("\t\t\t\t\t\tentity.MarkDirty();");
                writer.WriteLine("\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\telse");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tentity = new D3000.Data.{0}Data();", item);
                writer.WriteLine("\t\t\t\tentity.{0}PK = item.RecordPK;", item);
                writer.WriteLine("\t\t\t\tentity.CopyFrom(data, false);");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("");

                writer.WriteLine("\t\t\tif (!entity.IsDeleted)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tif (entity.IsNew)");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tentity.CreatedBy = string.Empty;");
                writer.WriteLine("\t\t\t\t\tentity.CreatedOn = System.DateTime.Now;");
                writer.WriteLine("\t\t\t\t\tentity.ModifiedBy = string.Empty;");
                writer.WriteLine("\t\t\t\t\tentity.ModifiedOn = System.DateTime.Now;");
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("\t\t\t\telse");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tentity.ModifiedBy = string.Empty;");
                writer.WriteLine("\t\t\t\t\tentity.ModifiedOn = System.DateTime.Now;");
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("\t\t\t}");

                writer.WriteLine("");
                writer.WriteLine("\t\t\tResult result = D3000.BasicServiceWrapper.{0}Wrapper.Save(entity, token);", item);
                writer.WriteLine("\t\t\tif (result.OK)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tCacheOfflineWrapper.DeleteByPK(item.CacheOfflinePK, token);");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\telse");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tUtils.WriteError(result.ToString());");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");

            }
        }
    }
}
