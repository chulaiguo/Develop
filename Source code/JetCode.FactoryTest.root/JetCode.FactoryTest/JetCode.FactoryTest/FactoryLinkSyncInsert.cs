using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using JetCode.BizSchema;
using JetCode.Factory;
namespace JetCode.FactoryTest
{
    public class FactoryLinkSyncInsert : FactoryBase
    {
        public FactoryLinkSyncInsert(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("using System.IO;");
            writer.WriteLine("using System.Text;");
            writer.WriteLine("using System.Threading;");
            writer.WriteLine("using System.Threading.Tasks;");
            writer.WriteLine("using Cheke;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using {0}.BasicServiceWrapper;", base.ProjectName);
            writer.WriteLine("using {0}.Data;", base.ProjectName);
            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.DataSync", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic class Insert");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tprivate readonly string _logFinishFile = string.Empty;");
            writer.WriteLine("\t\tprivate readonly SortedList<string, string> _finishIndex = new SortedList<string, string>();");
            writer.WriteLine("");
            writer.WriteLine("\t\tprivate int _maxTaskCount = 20;");
            writer.WriteLine("\t\tprivate readonly SecurityToken _token = null;");
            writer.WriteLine("");
            writer.WriteLine("\t\tpublic Insert(string logFinishFile)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._logFinishFile = logFinishFile;");
            writer.WriteLine("\t\t\tif(File.Exists(this._logFinishFile))");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tstring[] lines = File.ReadAllLines(this._logFinishFile);");
            writer.WriteLine("\t\t\t\tforeach (string line in lines)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tif(string.IsNullOrEmpty(line))");
            writer.WriteLine("\t\t\t\t\t\tcontinue;");
            writer.WriteLine("");
            writer.WriteLine("\t\t\t\t\tif(this._finishIndex.ContainsKey(line))");
            writer.WriteLine("\t\t\t\t\t\tcontinue;");
            writer.WriteLine("");
            writer.WriteLine("\t\t\t\t\tthis._finishIndex.Add(line, line);");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\t\tthis._token = new SecurityToken(AnonymousUser, AnonymousPassword, AppsToken);");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tprivate SecurityToken Token");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return _token; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tprivate string AppsToken");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return \"HelloDataServiceEx\"; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tprivate string AnonymousUser");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return \"anonymous\"; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tprivate string AnonymousPassword");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return \"anonymous\"; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tpublic int MaxTaskCount");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return _maxTaskCount; }");
            writer.WriteLine("\t\t\tset { _maxTaskCount = value; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tprivate void WriteDebugMethodBegin()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tprivate void WriteDebugMethodEnd()");
            writer.WriteLine("\t\t{");
            writer.WriteLine();
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tprivate void WriteDebug(string message)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tConsole.WriteLine(message);");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tprivate void WriteError(string error)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tConsole.WriteLine(error);");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tprivate void WriteSaveLog(Result result)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (!result.OK)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.WriteDebug(result.ToString());");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tprivate bool IsFinishedInsert(string tableName)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn this._finishIndex.ContainsKey(tableName);");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\tprivate void FinishedInsert(string tableName)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tstring text = string.Format(\"{0}{1}\", tableName, Environment.NewLine);");
            writer.WriteLine("\t\t\tFile.AppendAllText(this._logFinishFile, text);");
            writer.WriteLine("\t\t\tif (!this._finishIndex.ContainsKey(tableName))");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis._finishIndex.Add(tableName, tableName);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine("");
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

            //Clear DB
            writer.WriteLine("\t\tpublic void ClearDatabase()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis.WriteDebug(string.Format(\"Clear database begin...[at {0:yyyy/MM/dd HH:mm:ss}]\", DateTime.Now));");
            writer.WriteLine("\t\t\tfor (int i = 0; i < 3; i++)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tstring error = BDBuildingWrapper.ExecuteNonQuerySql(this.GetClearDatabaseSql(), this.Token);");
            writer.WriteLine("\t\t\t\tif (string.IsNullOrEmpty(error))");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tthis.WriteDebug(string.Format(\"Clear database end.[at {0:yyyy/MM/dd HH:mm:ss}]\", DateTime.Now));");
            writer.WriteLine("\t\t\t\t\treturn;");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\t\t\tthis.WriteError(error);");
            writer.WriteLine("\t\t\t\tThread.Sleep(1000);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            //Sql
            writer.WriteLine("\t\tprivate string GetClearDatabaseSql()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tStringBuilder builder = new StringBuilder();");
            for (int i = insertOrderList.Count - 1; i >= 0; i--)
            {
                writer.WriteLine("\t\t\tbuilder.Append(\"DELETE FROM [{0}];\");", insertOrderList[i]);
            }
            writer.WriteLine("\t\t\treturn builder.ToString();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            //insert table
            writer.WriteLine("\t\tpublic void InsertData()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis.WriteDebug(string.Format(\"Insert data begin...[at {0:yyyy/MM/dd HH:mm:ss}]\", DateTime.Now));");
            writer.WriteLine();
            writer.WriteLine("\t\t\tthis.InsertBDBuilding();");
            writer.WriteLine("\t\t\tthis.InsertACPanel();");
            foreach (string item in insertOrderList)
            {
                if (item == "BDBuilding" || item == "ACPanel")
                    continue;

                if (item.EndsWith("Download") || item == "ACUserEvent")
                    continue;

                if (item.StartsWith("Log"))
                    continue;

                if (item == "LogOnLineActivity" || item == "ACTempModeZone")
                    continue;

                writer.WriteLine("\t\t\tthis.Insert{0}();", item);
            }
            writer.WriteLine();
            writer.WriteLine("\t\t\tthis.WriteDebug(string.Format(\"Insert data end.[at {0:yyyy/MM/dd HH:mm:ss}]\", DateTime.Now));");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            //Insert item
            foreach (string item in insertOrderList)
            {
                if (item.EndsWith("Download") || item == "ACUserEvent")
                    continue;

                if (item == "LogOnLineActivity" || item == "ACTempModeZone" || item == "LogPhoneLineActivity")
                    continue;

                this.WriteInsertData(writer, item);
            }

            //Clear download flag
            writer.WriteLine("\t\tpublic void ClearDownloadFlag()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tstring error = string.Empty;");
            foreach (string item in insertOrderList)
            {
                if (!item.EndsWith("Download"))
                    continue;

                writer.WriteLine("\t\t\terror = BDBuildingWrapper.ExecuteNonQuerySql(\"update {0} set Download = 0\", this.Token);", item);
                writer.WriteLine("\t\t\tif (!string.IsNullOrEmpty(error))");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tthis.WriteError(error);");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();
            }
            writer.WriteLine("\t\t}");
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

        private void WriteInsertData(StringWriter writer, string tableName)
        {
            string d3000TableName = this.GetD3000TableName(tableName);
            this.WriteInsertData(writer, tableName, tableName, d3000TableName);
            this.WriteInsertTask(writer, tableName, tableName, d3000TableName);
            this.WriteInsertTaskData(writer, tableName, tableName, d3000TableName);
            this.WriteSaveData(writer, tableName);
        }

        private void WriteInsertData(StringWriter writer, string functionName, string tableName, string d3000TableName)
        {
            writer.WriteLine("\t\tprivate void Insert{0}()", functionName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (this.IsFinishedInsert(\"{0}\"))", tableName);
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine();
            writer.WriteLine("\t\t\tthis.WriteDebugMethodBegin();");
            writer.WriteLine();
            writer.WriteLine("\t\t\tint count = D3000.BasicServiceWrapper.{0}Wrapper.GetAllCount(this.Token);", d3000TableName);
            writer.WriteLine("\t\t\tint pageCount = count/1000 + (count%1000 == 0 ? 0 : 1);");
            writer.WriteLine("\t\t\tfor (int i = 0; i < pageCount; i++)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tD3000.Data.{0}DataCollection list = D3000.BasicServiceWrapper.{0}Wrapper.GetAllPage(i, 1000,this.Token);", d3000TableName);
            writer.WriteLine("");
            writer.WriteLine("\t\t\t\tthis.WriteDebug(string.Format(\"Insert (Page:[{{0}}], Count:[{{1}}], Total Count=[{{2}}]) {0}\", i+1, list.Count, count));", tableName);
            writer.WriteLine("\t\t\t\tif(list.Count <= 100)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tInsert{0}Data(list);", functionName);
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\telse");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tInsert{0}Task(list);", functionName);
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tthis.WriteDebugMethodEnd();");
            writer.WriteLine("\t\t\tthis.FinishedInsert(\"{0}\");", tableName);
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteInsertTask(StringWriter writer, string functionName, string tableName, string d3000TableName)
        {
            writer.WriteLine("\t\tprivate void Insert{0}Task(D3000.Data.{1}DataCollection list)", functionName, d3000TableName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tvar taskList = new Task[this.MaxTaskCount];");
            writer.WriteLine("");
            writer.WriteLine("\t\t\tint count = list.Count/this.MaxTaskCount;");
            writer.WriteLine("\t\t\tfor (int i = 0; i < this.MaxTaskCount; i++)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tint begin = i*count;");
            writer.WriteLine("\t\t\t\tint end = (i + 1)*count;");
            writer.WriteLine("\t\t\t\tif(i == (this.MaxTaskCount - 1))//last one");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tend = list.Count;");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\t\t\tD3000.Data.{0}DataCollection dataList = new D3000.Data.{0}DataCollection();", d3000TableName);
            writer.WriteLine("\t\t\t\tfor (int j = begin; j < end; j++)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tdataList.Add(list[j]);");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\t\t\ttaskList[i] = new Task(Insert{0}Data, dataList);", functionName);
            writer.WriteLine("\t\t\t\ttaskList[i].Start();");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("");
            writer.WriteLine("\t\t\tTask.WaitAll(taskList);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteInsertTaskData(StringWriter writer, string functionName, string tableName, string d3000TableName)
        {
            writer.WriteLine("\t\tprivate void Insert{0}Data(object obj)", functionName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tD3000.Data.{0}DataCollection list = obj as D3000.Data.{0}DataCollection;", d3000TableName);
            writer.WriteLine("\t\t\tif (list != null)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tforeach (D3000.Data.{0}Data item in list)", d3000TableName);
            writer.WriteLine("\t\t\t\t{");

            if (tableName != "ACMasterCard" && tableName != "UsrAccount")
            {
                if (tableName == "ACCardHolderBuildingMap")
                {
                    writer.WriteLine("\t\t\t\t\tif(item.OnSiteSyncDB || item.CardTypeID == D3000.Utils.CardTypeConstant._VIRTUAL_KEY)");
                }
                else
                {
                    writer.WriteLine("\t\t\t\t\tif(item.OnSiteSyncDB)");
                }
                writer.WriteLine("\t\t\t\t\t\tcontinue;");
                writer.WriteLine();
            }

            writer.WriteLine("\t\t\t\t\t{0}Data entity = new {0}Data();", tableName);

            ObjectSchema obj = base.GetObjectByName(tableName);
            if (obj != null)
            {
                foreach (FieldSchema field in obj.Fields)
                {
                    if (field.Alias == "RowVersion" || field.Alias == "Deleted" || field.Alias == "Download")
                        continue;

                    if (field.IsPK)
                    {
                        writer.WriteLine("\t\t\t\t\tentity.{0}PK = item.{1}PK;", tableName, d3000TableName);
                        continue;
                    }

                    //Timezone
                    if (obj.Alias == "BDBuilding")
                    {
                        if (field.Alias == "TimezoneName")
                        {
                            writer.WriteLine(
                                "\t\t\t\t\tD3000.Utils.TimezoneConstant zoneType = D3000.Utils.TimezoneConstant.FindByID(item.TimezoneID);");

                            writer.WriteLine("\t\t\t\t\tentity.TimezoneName = zoneType.Description;");
                            writer.WriteLine("\t\t\t\t\tentity.HoursDiff = (short)zoneType.HoursDiff;");

                            continue;
                        }

                        if (field.Alias == "HoursDiff")
                        {
                            continue;
                        }
                    }

                    //Panel
                    if (obj.Alias == "ACPanel")
                    {
                        if (field.Alias == "UnitPhone")
                        {
                            writer.WriteLine(
                                "\t\t\t\t\tentity.UnitPhone = Misc.GetDialingPanelNumber(item.LongDistance, item.UnitPhone, item.UnitExt);");
                            continue;
                        }

                        if (field.Alias == "HistoryPhone")
                        {
                            writer.WriteLine(
                                "\t\t\t\t\tentity.HistoryPhone = Misc.GetDialingReceiverNumber(item.HistoryPhone, item.DialingPrefixPanel, item.DialingSuffixPanel);");
                            continue;
                        }

                        if (field.Alias == "AlarmPhone1")
                        {
                            writer.WriteLine(
                                "\t\t\t\t\tentity.AlarmPhone1 = Misc.GetDialingReceiverNumber(item.AlarmPhone1, item.DialingPrefixPanel, item.DialingSuffixPanel);");
                            continue;
                        }

                        if (field.Alias == "AlarmPhone2")
                        {
                            writer.WriteLine(
                                "\t\t\t\t\tentity.AlarmPhone2 = Misc.GetDialingReceiverNumber(item.AlarmPhone2, item.DialingPrefixPanel, item.DialingSuffixPanel);");
                            continue;
                        }
                    }

                    //Gateway
                    if (obj.Alias == "ACGateway")
                    {
                        if (field.Alias == "InPhone")
                        {
                            writer.WriteLine(
                                "\t\t\t\t\tentity.InPhone = Misc.GetDialingPanelNumber(item.InPhoneLongDistance, item.InPhone, item.InExt);");
                            continue;
                        }

                        if (field.Alias == "OutPhone")
                        {
                            writer.WriteLine(
                                "\t\t\t\t\tentity.OutPhone = Misc.GetDialingPanelNumber(item.OutPhoneLongDistance, item.OutPhone, item.OutExt);");
                            continue;
                        }

                        if (field.Alias == "HistoryPhone")
                        {
                            writer.WriteLine(
                                "\t\t\t\t\tentity.HistoryPhone = Misc.GetDialingReceiverNumber(item.HistoryPhone, item.DialingPrefixPanel, item.DialingSuffixPanel);");
                            continue;
                        }

                        if (field.Alias == "AlarmPhone1")
                        {
                            writer.WriteLine(
                                "\t\t\t\t\tentity.AlarmPhone1 = Misc.GetDialingReceiverNumber(item.AlarmPhone1, item.DialingPrefixPanel, item.DialingSuffixPanel);");
                            continue;
                        }

                        if (field.Alias == "AlarmPhone2")
                        {
                            writer.WriteLine(
                                "\t\t\t\t\tentity.AlarmPhone2 = Misc.GetDialingReceiverNumber(item.AlarmPhone2, item.DialingPrefixPanel, item.DialingSuffixPanel);");
                            continue;
                        }

                    }

                    //Start From
                    if (field.Alias == "StartFrom")
                    {
                        if (obj.Alias == "ACAccessLevelDetail" || obj.Alias == "ACOutputGroupDetail"
                        || obj.Alias == "ACMainZone")
                        {
                            writer.WriteLine(
                                "\t\t\t\t\tD3000.Utils.MainZoneConstant zoneType = D3000.Utils.MainZoneConstant.FindByID(item.MainZoneTypeID);");

                            writer.WriteLine("\t\t\t\tentity.StartFrom = zoneType.StartFrom;");
                            continue;
                        }

                        if (obj.Alias == "ACInput")
                        {
                            writer.WriteLine("\t\t\t\tentity.StartFrom = 21;");
                            continue;
                        }

                        if (obj.Alias == "ACSupervisory")
                        {
                            writer.WriteLine("\t\t\t\tentity.StartFrom = 31;");
                            continue;
                        }
                    }

                    //Other
                    if (field.Alias == "BDBaseBuildingPK")
                    {
                        writer.WriteLine("\t\t\t\t\tentity.BDBaseBuildingPK = item.BaseBDBuildingPK;");
                        continue;
                    }

                    if (field.Alias == "BDBaseAddress1")
                    {
                        writer.WriteLine("\t\t\t\t\tentity.BDBaseAddress1 = item.BaseAddress1;");
                        continue;
                    }


                    if (field.Alias == "Month")
                    {
                        writer.WriteLine("\t\t\t\t\tentity.Month = (byte)item.Date.Month;");
                        continue;
                    }

                    if (field.Alias == "Day")
                    {
                        writer.WriteLine("\t\t\t\t\tentity.Day = (byte)item.Date.Day;");
                        continue;
                    }

                    if (field.Alias == "Year")
                    {
                        writer.WriteLine("\t\t\t\t\tentity.Year = (short)item.Date.Year;");
                        continue;
                    }

                    if (field.Alias == "RedCard")
                    {
                        writer.WriteLine("\t\t\t\t\tentity.RedCard = item.FunctionTypeID == D3000.Utils.FunctionCardTypeConstant._RedCard;");
                        continue;
                    }

                    writer.WriteLine("\t\t\t\t\tentity.{0} = item.{0};", field.Alias);
                }

                if (obj.Alias == "ACAMFormat" || obj.Alias == "ACCardHolderBuildingMap" || obj.Alias == "ACInterval"
                    || obj.Alias == "ACMasterCard" || obj.Alias == "ACTimecode")
                {
                    writer.WriteLine("\t\t\t\t\tentity.Download = true;");
                }

                //Joined Field
                List<string> joinedList = GetJoinedField(obj);
                foreach (string item in joinedList)
                {
                    writer.WriteLine("\t\t\t\t\tentity.{0} = item.{0};", item);
                }
            }

            writer.WriteLine();
            writer.WriteLine("\t\t\t\t\tResult result = this.Save{0}(entity, this.Token);", tableName);
            writer.WriteLine("\t\t\t\t\tthis.WriteSaveLog(result);");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteSaveData(StringWriter writer, string tableName)
        {
            writer.WriteLine("\t\tprivate Result Save{0}({0}Data entity, SecurityToken token)", tableName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tfor (int i = 0; i < 10; i++)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\ttry");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\treturn {0}Wrapper.Save(entity, token);", tableName); 
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\tcatch (Exception ex)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tthis.WriteError(ex.Message);");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tthis.WriteDebug(string.Format(\"Try Save {0} [{{0}}] ...\", i+1));", tableName);
            writer.WriteLine("\t\t\t\tThread.Sleep(10000);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tthrow new ApplicationException(\"Save BDBuilding failed after try 10.\");");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private List<string> GetJoinedField(ObjectSchema obj)
        {
            List<string> retList = new List<string>();
            Type type = Utils.GetDataType(base.MappingSchema, obj);
            if (type == null)
                return retList;

            SortedList<string, string> fieldIndex = new SortedList<string, string>();
            foreach (FieldSchema field in obj.Fields)
            {
                if (fieldIndex.ContainsKey(field.Alias))
                    continue;

                fieldIndex.Add(field.Alias, field.Alias);
            }

            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);
            foreach (PropertyInfo info in propertyInfos)
            {
                if (!info.CanRead || !info.CanWrite)
                    continue;

                if (info.Name == "Download")
                    continue;

                if (fieldIndex.ContainsKey(info.Name))
                    continue;

                if (info.PropertyType.IsValueType || info.PropertyType == typeof(string))
                {
                    retList.Add(info.Name);
                }
            }

            return retList;
        }
    }
}
