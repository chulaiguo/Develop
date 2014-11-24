using System.Collections.Specialized;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryTest
{
    public class FactorySyncUpdate : FactoryBase
    {
        public FactorySyncUpdate(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("using Cheke;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using {0}.BasicServiceWrapper;", base.ProjectName);
            writer.WriteLine("using {0}.Data;", base.ProjectName);
            writer.WriteLine("using {0}.Schema;", base.ProjectName);
            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.SyncService.Core", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic class SyncUpdate : SyncBase");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tpublic SyncUpdate(SecurityToken token)");
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

            //update data
            writer.WriteLine("\t\tpublic void UpdateData(D3000.Data.BDSiteServerData building)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis.WriteDebug(string.Format(\"Update data begin...[at {0:yyyy/MM/dd HH:mm:ss}]\", DateTime.Now));");
            writer.WriteLine("\t\t\tthis.BeginUpdate(building);");
            writer.WriteLine();
            writer.WriteLine("\t\t\tthis.UpdateBDBuilding(building);");
            writer.WriteLine("\t\t\tthis.UpdateACPanel(building);");
            foreach (string item in insertOrderList)
            {
                if (item.StartsWith("ZZ"))
                    continue;

                if (item == "CacheOffline" || item == "LogDeleteCard" || item == "ACDownloadTask"
                    || item == "BDSiteCommand" || item == "BDSiteResponse" || item == "BDPanelCommand" || item == "ACTimeSheet")
                    continue;

                if (item == "ACHistory")
                    continue;

                if (item == "BDBuilding" || item == "ACPanel")
                    continue;

                writer.WriteLine("\t\t\tthis.Update{0}(building);", item);
            }
            writer.WriteLine();
            writer.WriteLine("\t\t\tthis.EndUpdate(building);");
            writer.WriteLine("\t\t\tthis.WriteDebug(string.Format(\"Update data end.[at {0:yyyy/MM/dd HH:mm:ss}]\", DateTime.Now));");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            foreach (string item in insertOrderList)
            {
                if (item.StartsWith("ZZ"))
                    continue;

                if (item == "CacheOffline" || item == "LogDeleteCard" || item == "ACDownloadTask"
                   || item == "BDSiteCommand" || item == "BDSiteResponse" || item == "BDPanelCommand" || item == "ACTimeSheet")
                    continue;

                if (item == "ACHistory" || item == "ACAlarm" || item == "BDBuilding"
                    || item == "ACCardHolderBuildingMap" || item == "BDVisitorPhoto")
                    continue;

                this.WriteUpdateData(writer, item);
            }
        }

        private string GetD3000TableName(string tableName)
        {
            return tableName;
        }

        private void WriteUpdateData(StringWriter writer, string tableName)
        {
            string d3000TableName = this.GetD3000TableName(tableName);
            this.WriteUpdateData(writer, tableName, tableName, d3000TableName);
        }


        private void WriteUpdateData(StringWriter writer, string functionName, string tableName, string d3000TableName)
        {
            writer.WriteLine("\t\tprivate void Update{0}(D3000.Data.BDSiteServerData building)", functionName);
            writer.WriteLine("\t\t{");

            writer.WriteLine("\t\t\tthis.WriteDebugMethodBegin(\"Update {0}\");", tableName);
            writer.WriteLine("\t\t\tD3000.Data.BDSiteSyncTableData sync = this.GetSyncTable(building, {0}Schema.TableName);", tableName);
            writer.WriteLine("\t\t\tDateTime begin = sync.DBLastSync.AddSeconds(-5);");
            writer.WriteLine("\t\t\tDateTime end = this.Now.AddSeconds(5);");
            writer.WriteLine("\t\t\tbool isUpdateTable = begin.Year > 1900;");
            writer.WriteLine();
            writer.WriteLine("\t\t\tbool success = true;");
            writer.WriteLine("\t\t\tSortedList<string, ACDownloadTaskData> downloadTaskIndex = new SortedList<string, ACDownloadTaskData>();");
            writer.WriteLine("\t\t\tD3000.Data.{0}DataCollection list;", d3000TableName);
            writer.WriteLine("\t\t\tif (!isUpdateTable)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t//clear table");
            writer.WriteLine("\t\t\t\tthis.WriteDebug(\"Clear {0} ...\");", tableName);
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tint failedCount = 0;");
            writer.WriteLine("\t\t\t\t{0}DataCollection deletedList = {0}Wrapper.GetAll(base.Token);", tableName);
            writer.WriteLine("\t\t\t\tforeach ({0}Data item in deletedList)", tableName);
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\titem.Delete();");
            writer.WriteLine("\t\t\t\t\tResult result = {0}Wrapper.Save(item, base.Token);", tableName);
            writer.WriteLine("\t\t\t\t\tif(!result.OK)");
            writer.WriteLine("\t\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t\tthis.WriteError(result.ToString());");
            writer.WriteLine("\t\t\t\t\t\tfailedCount++;");
            writer.WriteLine("\t\t\t\t\t}");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tthis.WriteDebug(string.Format(\"Delete {0}: [{{0}}] OK, [{{1}}] Failed.\", deletedList.Count, failedCount));", tableName);
            writer.WriteLine("\t\t\t\tif(failedCount > 0)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tthis.WriteDebugMethodEnd(\"Update {0}\");", tableName);
            writer.WriteLine("\t\t\t\t\treturn;");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\t//insert data");
            if (tableName == "ACMasterCard")
            {
                writer.WriteLine("\t\t\t\tlist = D3000.BasicServiceWrapper.{0}Wrapper.GetAll(this.Token);", d3000TableName);
            }
            else
            {
                writer.WriteLine("\t\t\t\tlist = D3000.BasicServiceWrapper.{0}Wrapper.GetByBDBuilding(building.BDBuildingPK, this.Token);", d3000TableName);
            }
            writer.WriteLine("\t\t\t\tthis.WriteDebug(string.Format(\"Insert [{{0}}] {0}\", list.Count));", tableName);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\telse");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t//delete table");
            writer.WriteLine("\t\t\t\tif (this.IsDeleteChanged(building, {0}Schema.TableName, begin))", d3000TableName);
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tD3000.Data.LogDBDeleteActivityDataCollection deletedList = D3000.BasicServiceWrapper.LogDBDeleteActivityWrapper.GetByLogDateTime(begin, end, {0}Schema.TableName, this.Token);", d3000TableName);
            writer.WriteLine("\t\t\t\t\tthis.WriteDebug(string.Format(\"Delete [{{0}}] {0}\", deletedList.Count));", tableName);
            writer.WriteLine("\t\t\t\t\tforeach (D3000.Data.LogDBDeleteActivityData item in deletedList)");
            writer.WriteLine("\t\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t\t{0}Data entity = {0}Wrapper.GetByPK(item.RecordPK, this.Token);", tableName);
            writer.WriteLine("\t\t\t\t\t\tif (entity == null)");
            writer.WriteLine("\t\t\t\t\t\t\tcontinue;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\t\t\tentity.Delete();");
            writer.WriteLine("\t\t\t\t\t\tResult result = {0}Wrapper.Save(entity, this.Token);", tableName);
            writer.WriteLine("\t\t\t\t\t\tif (!result.OK)");
            writer.WriteLine("\t\t\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t\t\tsuccess = false;");
            writer.WriteLine("\t\t\t\t\t\t\tthis.WriteError(result.ToString());");
            writer.WriteLine("\t\t\t\t\t\t\tcontinue;");
            writer.WriteLine("\t\t\t\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\t\t\tthis.NotifyDataChanged(entity, downloadTaskIndex);");
            writer.WriteLine("\t\t\t\t\t}");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\t\t\t//update table");
            writer.WriteLine("\t\t\t\tif (this.IsUpdateChanged(building, {0}Schema.TableName, begin))", d3000TableName);
            writer.WriteLine("\t\t\t\t{");
            if (tableName == "ACMasterCard")
            {
                writer.WriteLine("\t\t\t\t\tlist = D3000.BasicServiceWrapper.{0}Wrapper.GetByModifiedOn(begin, end, this.Token);", d3000TableName);
            }
            else
            {
                writer.WriteLine("\t\t\t\t\tlist = D3000.BasicServiceWrapper.{0}Wrapper.GetByModifiedOn(building.BDBuildingPK, begin, end, this.Token);", d3000TableName);
            }
            writer.WriteLine("\t\t\t\t\tthis.WriteDebug(string.Format(\"Update [{{0}}] {0}\", list.Count));", tableName);
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\telse");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tlist = new D3000.Data.{0}DataCollection();", d3000TableName);
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tforeach (D3000.Data.{0}Data item in list)", d3000TableName);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t{0}Data entity = null;", tableName);
            writer.WriteLine("\t\t\t\tif(isUpdateTable)");
            writer.WriteLine("\t\t\t\t{");
            if (tableName == "UtilSettingDetail")
            {
                writer.WriteLine("\t\t\t\t\tentity = UtilSettingDetailWrapper.GetByUK(item.UtilSettingCategoryPK, item.FieldName, this.Token);");
            }
            else
            {
                writer.WriteLine("\t\t\t\t\tentity = {0}Wrapper.GetByPK(item.{1}PK, this.Token);", tableName, d3000TableName);
                if (tableName == "BDSiteCommandFuture")
                {
                    writer.WriteLine("\t\t\t\t\tif(entity != null && entity.CommandID < 0)");
                    writer.WriteLine("\t\t\t\t\t\tcontinue;");
                }
            }
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tif(entity == null)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tentity = new {0}Data();", tableName);
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine();

            if (tableName == "UtilSettingDetail")
            {
                writer.WriteLine("\t\t\t\tentity.UtilSettingCategoryPK = item.UtilSettingCategoryPK;");
                writer.WriteLine("\t\t\t\tentity.FieldName = item.FieldName;");
                writer.WriteLine("\t\t\t\tentity.FieldValue = item.FieldValue;");
                writer.WriteLine("\t\t\t\tentity.Picture = item.Picture;");
                writer.WriteLine("\t\t\t\tentity.CreatedOn = item.CreatedOn;");
                writer.WriteLine("\t\t\t\tentity.CreatedBy = item.CreatedBy;");
                writer.WriteLine("\t\t\t\tentity.ModifiedOn = item.ModifiedOn;");
                writer.WriteLine("\t\t\t\tentity.ModifiedBy = item.ModifiedBy;");
            }
            else
            {
                writer.WriteLine("\t\t\t\tSyncData.Sync{0}(item, entity);", tableName);
            }
            writer.WriteLine("\t\t\t\tif (!entity.IsDirty)");
            writer.WriteLine("\t\t\t\t\tcontinue;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tResult result = {0}Wrapper.Save(entity, this.Token);", tableName);
            writer.WriteLine("\t\t\t\tif (!result.OK)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tsuccess = false;");
            writer.WriteLine("\t\t\t\t\tthis.WriteError(result.ToString());");
            writer.WriteLine("\t\t\t\t\tcontinue;");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tthis.NotifyDataChanged(entity, downloadTaskIndex);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tif (!this.CreateDownloadTask(downloadTaskIndex, begin).OK)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tsuccess = false;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tif(success)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.SaveSyncTable(sync, this.Now);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\t\tthis.WriteDebugMethodEnd(\"Update {0}\");", tableName);
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }
    }
}
