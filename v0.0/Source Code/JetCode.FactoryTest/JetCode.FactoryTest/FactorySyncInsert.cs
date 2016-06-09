using System.Collections.Specialized;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;
namespace JetCode.FactoryTest
{
    public class FactorySyncInsert : FactoryBase
    {
        public FactorySyncInsert(MappingSchema mappingSchema)
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
            writer.WriteLine("\tpublic class SyncInsert : SyncBase");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tpublic SyncInsert(SecurityToken token)");
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

            //insert table
            writer.WriteLine("\t\tpublic void InsertData(D3000.Data.BDSiteServerData building)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis.WriteDebug(string.Format(\"Insert data begin...[at {0:yyyy/MM/dd HH:mm:ss}]\", DateTime.Now));");
            writer.WriteLine();
            foreach (string item in insertOrderList)
            {
                if (item.StartsWith("ZZ"))
                    continue;

                if (item == "CacheOffline" || item == "LogDeleteCard" || item == "ACDownloadTask" || item == "BDSiteCommand")
                    continue;

                if (item == "ACMasterCard")
                {
                    writer.WriteLine("\t\t\tthis.Insert{0}();", item);
                    continue;
                }

                writer.WriteLine("\t\t\tthis.Insert{0}(building);", item);
            }
            writer.WriteLine();
            writer.WriteLine("\t\t\tthis.WriteDebug(string.Format(\"Insert data end.[at {0:yyyy/MM/dd HH:mm:ss}]\", DateTime.Now));");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            foreach (string item in insertOrderList)
            {
                if (item.StartsWith("ZZ"))
                    continue;

                if (item == "CacheOffline" || item == "LogDeleteCard" || item == "ACDownloadTask" || item == "BDSiteCommand")
                    continue;

                if (item == "BDBuilding" || item == "ACCardHolderBuildingMap")
                    continue;

                this.WriteInsertData(writer, item);
            }
        }

        private string GetD3000TableName(string tableName)
        {
            return tableName;
        }

        private void WriteInsertData(StringWriter writer, string tableName)
        {
            string d3000TableName = this.GetD3000TableName(tableName);
            this.WriteInsertData(writer, tableName, tableName, d3000TableName);
        }

        private void WriteInsertData(StringWriter writer, string functionName, string tableName, string d3000TableName)
        {
            if (tableName == "ACMasterCard")
            {
                writer.WriteLine("\t\tprivate void Insert{0}()", functionName);
            }
            else
            {
                writer.WriteLine("\t\tprivate void Insert{0}(D3000.Data.BDSiteServerData building)", functionName);
            }
            
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis.WriteDebugMethodBegin();");
            writer.WriteLine();
            if (tableName == "ACMasterCard")
            {
                writer.WriteLine("\t\t\tD3000.Data.{0}DataCollection list = D3000.BasicServiceWrapper.{0}Wrapper.GetAll(this.Token);", d3000TableName);
            }
            else
            {
                writer.WriteLine("\t\t\tD3000.Data.{0}DataCollection list = D3000.BasicServiceWrapper.{0}Wrapper.GetByBDBuilding(building.BDBuildingPK, this.Token);", d3000TableName);   
            }
            
            writer.WriteLine("\t\t\tthis.WriteDebug(string.Format(\"Insert [{{0}}] {0}\", list.Count));", tableName);
            writer.WriteLine("\t\t\tforeach (D3000.Data.{0}Data item in list)", d3000TableName);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t{0}Data entity = new {0}Data();", tableName);
            writer.WriteLine("\t\t\t\tSyncData.Sync{0}(item, entity);", tableName);
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tResult result = {0}Wrapper.Save(entity, this.Token);", tableName);
            writer.WriteLine("\t\t\t\tthis.WriteSaveLog(entity, result);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tthis.WriteDebugMethodEnd();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }
    }
}
