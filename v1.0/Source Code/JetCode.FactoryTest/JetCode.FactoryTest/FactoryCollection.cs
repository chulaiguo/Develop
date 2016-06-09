using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;
using System;

namespace JetCode.FactoryTest
{
    public class FactoryCollection : FactoryBase
    {
        public FactoryCollection(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using REST.Data;");
            writer.WriteLine("using REST.Schema;");
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
            //writer.WriteLine("switch (entity.TableName)");
            //writer.WriteLine("{");
            //foreach (ObjectSchema obj in base.MappingSchema.Objects)
            //{
            //    if(obj.Alias.StartsWith("ZZ") || obj.Alias.StartsWith("Log")
            //        || obj.Alias.StartsWith("BDSite") || obj.Alias.StartsWith("BDParking")
            //        || obj.Alias.StartsWith("BDVisitor") || obj.Alias.StartsWith("ACVisitor"))
            //        continue;

            //    if (obj.Alias == "ACAlarm" || obj.Alias == "ACHistory" || obj.Alias == "ACTimeSheet"
            //        || obj.Alias == "UtilSettingCategory" || obj.Alias == "UtilSettingDetail")
            //        continue;

            //    writer.WriteLine("\t\tRefresh{0}();", obj.Alias);
            //}
            //writer.WriteLine("}");

            //writer.WriteLine("\t\t\tswitch (tableName)");
            //writer.WriteLine("\t\t\t{");
            //foreach (ObjectSchema obj in base.MappingSchema.Objects)
            //{
            //    if (obj.Alias.StartsWith("ZZ") || obj.Alias.StartsWith("Log")
            //        || obj.Alias.StartsWith("BDSite") || obj.Alias.StartsWith("BDParking")
            //        || obj.Alias.StartsWith("BDVisitor") || obj.Alias.StartsWith("ACVisitor"))
            //        continue;

            //    if (obj.Alias == "ACAlarm" || obj.Alias == "ACHistory" || obj.Alias == "ACTimeSheet"
            //        || obj.Alias == "UtilSettingCategory" || obj.Alias == "UtilSettingDetail")
            //        continue;

            //    writer.WriteLine("\t\t\t\tcase \"{0}\":", obj.Alias.ToUpper());
            //    writer.WriteLine("\t\t\t\t\tRefresh{0}();", obj.Alias);
            //    writer.WriteLine("\t\t\t\t\tbreak;");
            //}
            //writer.WriteLine("\t\t\t}");

            foreach (ObjectSchema obj in base.MappingSchema.Objects)
            {
                if (obj.Alias.StartsWith("ZZ") || obj.Alias.StartsWith("Log")
                    || obj.Alias.StartsWith("BDSite") || obj.Alias.StartsWith("BDParking")
                    || obj.Alias.StartsWith("BDVisitor") || obj.Alias.StartsWith("ACVisitor"))
                    continue;

                if (obj.Alias == "ACAlarm" || obj.Alias == "ACHistory" || obj.Alias == "ACTimeSheet"
                    || obj.Alias == "UtilSettingCategory" || obj.Alias == "UtilSettingDetail")
                    continue;

                writer.WriteLine("\t\tprivate static void Refresh{0}()", obj.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tWriteLine(\"Sync {0} begin...\");", obj.Alias);
                writer.WriteLine();
                writer.WriteLine("\t\t\tint index = 1;");
                writer.WriteLine();
                writer.WriteLine("\t\t\tint count = {0}Wrapper.GetAllCount(_Token);", obj.Alias);
                writer.WriteLine("\t\t\tWriteLine(string.Format(\"{0} Count = {{0}}\", count));", obj.Alias);
                writer.WriteLine();
                writer.WriteLine("\t\t\tconst int pageSize = 1000;");
                writer.WriteLine("\t\t\tint pageCount = (count / pageSize) + (count % pageSize == 0 ? 0 : 1);");
                writer.WriteLine("\t\t\tfor (int i = 0; i < pageCount; i++)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\t{0}DataCollection list = {0}Wrapper.GetAllPage(i, pageSize, _Token);", obj.Alias);
                writer.WriteLine("\t\t\t\tforeach ({0}Data item in list)", obj.Alias);
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\titem.MarkDirty();");
                writer.WriteLine("\t\t\t\t\tResult r = {0}Wrapper.Save(item, _Token);", obj.Alias);
                writer.WriteLine("\t\t\t\t\tif (!r.OK)");
                writer.WriteLine("\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\tWriteLine(r.ToString());");
                writer.WriteLine("\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\telse");
                writer.WriteLine("\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\tWriteLine(string.Format(\"{0} OK. {{0}}\", index++));", obj.Alias);
                writer.WriteLine("\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("\t\t\t}");

                writer.WriteLine();
                writer.WriteLine("\t\t\tWriteLine(\"Sync {0} end.\");", obj.Alias);
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

            //writer.WriteLine("\t\tprivate void DeleteData(DateTime begin, DateTime end)");
            //writer.WriteLine("\t\t{");
            //writer.WriteLine("\t\t\t LogDBDeleteActivityDataCollection list = LogDBDeleteActivityWrapper.GetByLogDateTime(begin, end, base.Token);");
            //writer.WriteLine("\t\t\tforeach (LogDBDeleteActivityData item in list)");
            //writer.WriteLine("\t\t\t{");
            //writer.WriteLine("\t\t\t\tswitch (item.DBTableName)");
            //writer.WriteLine("\t\t\t\t{");

            //MappingInsertOrder order = new MappingInsertOrder(base.MappingSchema);
            //StringCollection insertOrderList = order.GetInsertOrder();
            //for (int i = insertOrderList.Count - 1; i >= 0; i--)
            //{
            //    if (insertOrderList[i].StartsWith("ZZ"))
            //        continue;

            //    WriteDeleteData(writer, insertOrderList[i]);
            //}

            //writer.WriteLine("\t\t\t\t}");
            //writer.WriteLine("\t\t\t}");
            //writer.WriteLine("\t\t}");
            //writer.WriteLine();

            //foreach (string item in insertOrderList)
            //{
            //    if (item.StartsWith("ZZ") || item.StartsWith("Log"))
            //        continue;

            //    this.WriteUpdateData(writer, item);
            //}

            //writer.WriteLine("\t\tprotected Result InsertTable(string tableName)");
            //writer.WriteLine("\t\t{");
            //writer.WriteLine("\t\t\tswitch (tableName)");
            //writer.WriteLine("\t\t\t{");
            //foreach (string item in insertOrderList)
            //{
            //    writer.WriteLine("\t\t\t\tcase \"{0}\":", item);
            //    writer.WriteLine("\t\t\t\t\treturn this.Insert{0}();", item);
            //}
            //writer.WriteLine("\t\t\t\tdefault:");
            //writer.WriteLine("\t\t\t\t\treturn new Result(tableName + \" does't exist.\");");
            //writer.WriteLine("\t\t\t}");
            //writer.WriteLine("\t\t}");
            //writer.WriteLine();
           

            //this.WriteSettingCategory(writer);

            //this.WriteDBRule(writer);
        }

        private void WriteDeleteData(StringWriter writer, string tableName)
        {
            writer.WriteLine("\t\t\t\t\tcase {0}Schema.TableName:", tableName);
            writer.WriteLine("\t\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t\tI{0}BasicService backupSvr = base.BackupServiceFactory.Get{0}Service(base.Token);", tableName);
            writer.WriteLine("\t\t\t\t\t\t{0}Data data = backupSvr.GetByPK(item.RecordPK);",tableName);
            writer.WriteLine("\t\t\t\t\t\tif(data != null)");
            writer.WriteLine("\t\t\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t\t\tdata.Delete();");
            writer.WriteLine("\t\t\t\t\t\t\tResult result = backupSvr.Save(data);");
            writer.WriteLine("\t\t\t\t\t\t\tthis.WriteSaveLog(result);");
            writer.WriteLine("\t\t\t\t\t\t}");
            writer.WriteLine("\t\t\t\t\t\tbreak;");
            writer.WriteLine("\t\t\t\t\t}");
        }

        private void WriteInsertData(StringWriter writer, string tableName)
        {
            writer.WriteLine("\t\tprivate void Insert{0}()", tableName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t_SysLog.WriteDebugMethodBegin();");
            writer.WriteLine("\t\t\tI{0}BasicService backup = base.BackupServiceFactory.Get{0}Service(base.Token);", tableName);
            writer.WriteLine("\t\t\t{0}DataCollection list = {0}Wrapper.GetAll(base.Token);", tableName);
            writer.WriteLine("\t\t\t_SysLog.WriteDebug(string.Format(\"Insert [{{0}}] {0}\", list.Count));", tableName);
            writer.WriteLine("\t\t\tforeach ({0}Data item in list)", tableName);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\titem.MarkNew();");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tResult result = backup.Save(item);");
            writer.WriteLine("\t\t\t\tthis.WriteSaveLog(result);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\t_SysLog.WriteDebugMethodEnd();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteUpdateData(StringWriter writer, string tableName)
        {

            writer.WriteLine("\t\tprivate void Update{0}(DateTime begin, DateTime end)", tableName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t_SysLog.WriteDebugMethodBegin();");
            writer.WriteLine("\t\t\tI{0}BasicService backupSvr = base.BackupServiceFactory.Get{0}Service(base.Token);", tableName);
            writer.WriteLine("\t\t\t{0}DataCollection list = {0}Wrapper.GetByModifiedOn(begin, end, base.Token);", tableName);
            writer.WriteLine("\t\t\t_SysLog.WriteDebug(string.Format(\"Update [{{0}}] {0}\", list.Count));", tableName);
            writer.WriteLine("\t\t\tforeach ({0}Data item in list)", tableName);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t{0}Data old = backupSvr.GetByPK(item.{0}PK);", tableName);
            writer.WriteLine("\t\t\t\tif(old != null)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\titem.RowVersion = old.RowVersion;");
            writer.WriteLine("\t\t\t\t\titem.MarkDirty();");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\telse");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\titem.MarkNew();");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tResult result = backupSvr.Save(item);");
            writer.WriteLine("\t\t\t\tthis.WriteSaveLog(result);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\t_SysLog.WriteDebugMethodEnd();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }


        private void WriteInsertTable(StringWriter writer, string tableName)
        {
            writer.WriteLine("\t\tprotected virtual Result Insert{0}()", tableName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tNew.{0}Collection newList = new New.{0}Collection();", tableName);
            writer.WriteLine();
            writer.WriteLine("\t\t\tOld.{0}Collection oldList = Old.{0}.GetAll();", tableName);
            writer.WriteLine("\t\t\tforeach (Old.{0} item in oldList)", tableName);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tNew.{0} entity = new New.{0}();", tableName);
            writer.WriteLine("\t\t\t\tentity.CopyFrom(item, false);");

            ObjectSchema obj = base.GetObjectByName(tableName);
            if (obj != null)
            {
                foreach (FieldSchema item in obj.Fields)
                {
                    if (!item.IsPK)
                        continue;

                    writer.WriteLine("\t\t\t\tentity.{0} = item.{0};", item.Name);
                }
            }

            writer.WriteLine("\t\t\t\tnewList.Add(entity);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\treturn newList.Save();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteClearTable(StringWriter writer, string tableName)
        {
            writer.WriteLine("\t\tprivate Result Clear{0}()", tableName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tNew.{0}Collection list = New.{0}.GetAll();", tableName);
            writer.WriteLine("\t\t\tlist.Clear();");
            writer.WriteLine("\t\t\treturn list.Save();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }
    }
}
