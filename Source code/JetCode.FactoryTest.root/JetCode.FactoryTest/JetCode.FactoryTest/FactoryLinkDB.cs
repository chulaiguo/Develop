using System.IO;
using System.Text;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryTest
{
    public class FactoryLinkDB : FactoryBase
    {
        public FactoryLinkDB(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Data;");
            writer.WriteLine("using Cheke;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using Cheke.BusinessService;");

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.DataLink", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        private string GetD3000TableName(string tableName)
        {
            string d3000tableName = tableName;
            
            if (tableName == "ACHoliday")
            {
                d3000tableName = "ACPanelHolidayMap";
            }

            if (tableName == "ACOutputGroupDetail")
            {
                d3000tableName = "ACOutputGroupMainZoneMap";
            }

            if (tableName == "ACTimecodeDetail")
            {
                d3000tableName = "ACTimecodeIntervalMap";
            }

            return d3000tableName;
        }

        protected override void WriteContent(StringWriter writer)
        {
            writer.WriteLine("\tpublic class Link : IDBLinkAction");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tprivate readonly SecurityToken _token = null;");
            writer.WriteLine("");
            writer.WriteLine("\t\tpublic Link()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._token = new SecurityToken(this.AnonymousUser, this.AnonymousPassword, this.AppsToken);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tprotected SecurityToken Token");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this._token; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tprivate string AppsToken");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return \"HelloDataServiceEx\"; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tprivate string AnonymousUser");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return \"anonymous\"; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tprivate string AnonymousPassword");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return \"anonymous\"; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic int LinkDBInsert(BusinessBase entity, SecurityToken token)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (!this.IsLinkTable(entity.TableName))");
            writer.WriteLine("\t\t\t\treturn 1;");
            writer.WriteLine();
            writer.WriteLine("\t\t\tthis.LinkEntity(entity);");
            writer.WriteLine("\t\t\treturn 1;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic int LinkDBUpdate(BusinessBase entity, SecurityToken token)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (!this.IsLinkTable(entity.TableName))");
            writer.WriteLine("\t\t\t\treturn 1;");
            writer.WriteLine();
            writer.WriteLine("\t\t\tthis.LinkEntity(entity);");
            writer.WriteLine("\t\t\treturn 1;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic int LinkDBDelete(BusinessBase entity, SecurityToken token)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (!this.IsLinkTable(entity.TableName))");
            writer.WriteLine("\t\t\t\treturn 1;");
            writer.WriteLine();
            writer.WriteLine("\t\t\tthis.LinkEntity(entity);");
            writer.WriteLine("\t\t\treturn 1;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic int LinkDBDelete(DataTable table, SecurityToken token)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (!this.IsLinkTable(table.TableName))");
            writer.WriteLine("\t\t\t\treturn 1;");
            writer.WriteLine();
            writer.WriteLine("\t\t\tif (table.Columns.Count < 2)");
            writer.WriteLine("\t\t\t\treturn 1;");
            writer.WriteLine();
            writer.WriteLine("\t\t\tthis.LinkDelete(table);");
            writer.WriteLine("\t\t\treturn 1;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate void LinkEntity(BusinessBase entity)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tswitch (entity.TableName)");
            writer.WriteLine("\t\t\t{");
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                if (item.Alias == "ACUserEvent")
                    continue;

                if (item.Alias == "LogOnLineActivity" || item.Alias == "ACTempModeZone" || item.Alias == "LogPhoneLineActivity")
                    continue;

                if (item.Alias.EndsWith("Download"))
                    continue;

                string d3000TableName = this.GetD3000TableName(item.Alias);
                writer.WriteLine("\t\t\t\tcase D3000.Schema.{0}Schema.TableName:", d3000TableName);
                writer.WriteLine("\t\t\t\t\t{0}.Sync(entity as D3000.Data.{1}Data, this.Token);", item.Alias.Substring(2), d3000TableName);
                writer.WriteLine("\t\t\t\t\tbreak;");
            }

            //Misc
            writer.WriteLine("\t\t\t\tcase D3000.Schema.ACCardHolderSchema.TableName:");
            writer.WriteLine("\t\t\t\t\tMisc.SyncCardHolder(entity as D3000.Data.ACCardHolderData, this.Token);");
            writer.WriteLine("\t\t\t\t\tbreak;");
            writer.WriteLine("\t\t\t\tcase D3000.Schema.ACFunctionCardSchema.TableName:");
            writer.WriteLine("\t\t\t\t\tMisc.SyncFunctionCard(entity as D3000.Data.ACFunctionCardData, this.Token);");
            writer.WriteLine("\t\t\t\t\tbreak;");
            writer.WriteLine("\t\t\t\tcase D3000.Schema.BDTenantSchema.TableName:");
            writer.WriteLine("\t\t\t\t\tMisc.SyncTenant(entity as D3000.Data.BDTenantData, this.Token);");
            writer.WriteLine("\t\t\t\t\tbreak;");
            writer.WriteLine("\t\t\t\tcase D3000.Schema.UtilHolidaySchema.TableName:");
            writer.WriteLine("\t\t\t\t\tMisc.SyncHoliday(entity as D3000.Data.UtilHolidayData, this.Token);");
            writer.WriteLine("\t\t\t\t\tbreak;");

            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate void LinkDelete(DataTable table)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tswitch (table.TableName)");
            writer.WriteLine("\t\t\t{");
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                if (item.Alias == "ACUserEvent")
                    continue;

                if (item.Alias.EndsWith("Download"))
                    continue;

                string d3000TableName = this.GetD3000TableName(item.Alias);

                writer.WriteLine("\t\t\t\tcase D3000.Schema.{0}Schema.TableName:", d3000TableName);
                writer.WriteLine("\t\t\t\t\tforeach (DataRow row in table.Rows)");
                writer.WriteLine("\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\tvar recordPK = new Guid(row[0].ToString());");
                writer.WriteLine("\t\t\t\t\t\t{0}.SyncDelete(recordPK, this.Token);", item.Alias.Substring(2));
                writer.WriteLine("\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\tbreak;");
            }
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic bool IsLinkDBInsertEnabled");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return true; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tpublic bool IsLinkDBUpdateEnabled");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return true; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tpublic bool IsLinkDBDeleteEnabled");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return true; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate bool IsLinkTable(string tableName)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (tableName == \"ACHistory\" || tableName == \"ACAlarm\")");
            writer.WriteLine("\t\t\t\treturn false;");
            writer.WriteLine("");
            writer.WriteLine("\t\t\tif (tableName.StartsWith(\"AC\"))");
            writer.WriteLine("\t\t\t\treturn true;");
            writer.WriteLine("");
            writer.WriteLine("\t\t\tif (tableName == \"BDBuilding\" || tableName == \"BDTenant\")");
            writer.WriteLine("\t\t\t\treturn true;");
            writer.WriteLine("");
            writer.WriteLine("\t\t\treturn false;");
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t}");
        }
    }
}
