using System.Collections.Specialized;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryTest
{
    public class FactorySyncDelete : FactoryBase
    {
        public FactorySyncDelete(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using Cheke;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using {0}.BasicServiceWrapper;", base.ProjectName);
            writer.WriteLine("using {0}.Data;", base.ProjectName);
            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.SyncService.Core", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic class SyncDelete : SyncBase");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tpublic SyncDelete(SecurityToken token)");
            writer.WriteLine("\t\t\t: base(token)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
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

            //delete data
            writer.WriteLine("\t\tpublic void DeleteData(DateTime begin, DateTime end)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis.WriteDebug(string.Format(\"Delete data begin...[at {0:yyyy/MM/dd HH:mm:ss}]\", DateTime.Now));");
            writer.WriteLine("\t\t\tthis.WriteDebug(string.Format(\"Date Range: [{0:yyyy/MM/dd HH:mm:ss}-{1:yyyy/MM/dd HH:mm:ss}]\", begin, end));");
            writer.WriteLine();
            writer.WriteLine("\t\t\tD3000.Data.LogDBDeleteActivityDataCollection list = D3000.BasicServiceWrapper.LogDBDeleteActivityWrapper.GetByLogDateTime(begin, end, base.Token);");
            writer.WriteLine("\t\t\tthis.WriteDebug(string.Format(\"Sync delete [{0}] records.\", list.Count));");
            writer.WriteLine("\t\t\tforeach (D3000.Data.LogDBDeleteActivityData item in list)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tswitch (item.DBTableName)");
            writer.WriteLine("\t\t\t\t{");
            for (int i = insertOrderList.Count - 1; i >= 0; i--)
            {
                string tableName = insertOrderList[i];
                if (tableName.StartsWith("ZZ"))
                    continue;

                if (tableName == "CacheOffline" || tableName == "LogDeleteCard" || tableName == "ACDownloadTask" 
                    || tableName == "BDSiteCommand")
                    continue;

                string d3000TableName = this.GetD3000TableName(tableName);
                
                writer.WriteLine("\t\t\t\t\tcase D3000.Schema.{0}Schema.TableName:", d3000TableName);
                writer.WriteLine("\t\t\t\t\t\tthis.Delete{0}(item);", tableName);
                writer.WriteLine("\t\t\t\t\t\tbreak;");
            }
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tthis.WriteDebug(string.Format(\"Delete data end.[at {0:yyyy/MM/dd HH:mm:ss}]\", DateTime.Now));");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            //Delete Item
            for (int i = insertOrderList.Count - 1; i >= 0; i--)
            {
                string tableName = insertOrderList[i];
                if (tableName.StartsWith("ZZ"))
                    continue;

                if (tableName == "CacheOffline" || tableName == "LogDeleteCard" || tableName == "ACDownloadTask"
                    || tableName == "BDSiteCommand")
                    continue;
                
                this.WriteDeleteData(writer, tableName);
            }
        }

        private string GetD3000TableName(string tableName)
        {
            return tableName;
        }

        private void WriteDeleteData(StringWriter writer, string tableName)
        {
            writer.WriteLine("\t\tprivate void Delete{0}(D3000.Data.LogDBDeleteActivityData item)", tableName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}Data data = {0}Wrapper.GetByPK(item.RecordPK, base.Token);", tableName);
            writer.WriteLine("\t\t\tif(data != null)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tdata.Delete();");
            writer.WriteLine("\t\t\t\tResult result = {0}Wrapper.Save(data, base.Token);", tableName);
            writer.WriteLine("\t\t\t\tthis.WriteSaveLog(data, result);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }
    }
}
