using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryTest
{
    public class FactoryLinkItem : FactoryBase
    {
        public FactoryLinkItem(MappingSchema mappingSchema, ObjectSchema item)
            : base(mappingSchema, item)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using {0}.BasicServiceWrapper;", base.ProjectName);
            writer.WriteLine("using {0}.Data;", base.ProjectName);

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
            string d3000TableName = this.GetD3000TableName(base.ObjectSchema.Alias);

            writer.WriteLine("\tpublic static class {0}", base.ObjectSchema.Alias.Substring(2));
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tpublic static void Sync(D3000.Data.{0}Data data, Cheke.SecurityToken token)", d3000TableName);
            writer.WriteLine("\t\t{");
            if (base.ObjectSchema.Alias == "ACMasterCard")
            {
                 writer.WriteLine("\t\t\tif(data == null)");
            }
            else
            {
                 writer.WriteLine("\t\t\tif(data == null)");
            }
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine();

            if (base.ObjectSchema.Alias.StartsWith("AC"))
            {
                writer.WriteLine("\t\t\tif(data.IsNew)");
                writer.WriteLine("\t\t\t{");

                if (base.ObjectSchema.Alias == "ACCardHolderBuildingMap")
                {
                    writer.WriteLine("\t\t\t\t{0}Data old = {0}Wrapper.GetBySitecodeEncode(data.BDBuildingPK, data.Sitecode, data.Encoded, token);", base.ObjectSchema.Alias);
                }
                else if (base.ObjectSchema.Alias == "ACPanelFunctionCardMap")
                {
                    writer.WriteLine("\t\t\t\t{0}Data old = {0}Wrapper.GetBySitecodeEncode(data.ACPanelPK, data.Sitecode, data.Encoded, token);", base.ObjectSchema.Alias);
                }
                else if (base.ObjectSchema.Alias == "ACMasterCard")
                {
                    writer.WriteLine("\t\t\t\t{0}Data old = {0}Wrapper.GetBySitecodeEncode(data.Sitecode, data.Encoded, token);", base.ObjectSchema.Alias);
                }
                else if (base.ObjectSchema.Alias == "ACPanel")
                {
                    writer.WriteLine("\t\t\t\t{0}Data old = {0}Wrapper.GetByUnitID(data.UnitID, token);", base.ObjectSchema.Alias);
                }
                else if (base.ObjectSchema.Alias == "ACHoliday")
                {
                    writer.WriteLine("\t\t\t\t{0}Data old = {0}Wrapper.GetByUK(data.ACPanelPK, (short)data.Date.Year, (byte)data.Date.Month, (byte)data.Date.Day, token);", base.ObjectSchema.Alias);
                }
                else
                {
                    writer.WriteLine("\t\t\t\t{0}Data old = {0}Wrapper.GetByUK({1}token);", base.ObjectSchema.Alias, this.GetUKFieldInputPara("data"));
                }
                
                writer.WriteLine("\t\t\t\tif(old != null)");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\told.Delete();");
                writer.WriteLine("\t\t\t\t\tSave(old, token);");
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("\t\t\t}");

  
                writer.WriteLine("\t\t\telse if (data.IsDeleted)");
                writer.WriteLine("\t\t\t{");
                if (!this.HasDeletedField())
                {
                    writer.WriteLine("\t\t\t\tSyncDelete(data.{0}PK, token);", d3000TableName);
                    writer.WriteLine("\t\t\t\treturn;");
                }
                else
                {
                    if (this.HasDownloadField())
                    {
                        writer.WriteLine("\t\t\t\tif (!data.Active || data.IsE150)");
                    }
                    else
                    {
                        if (base.ObjectSchema.Alias == "ACMasterCard")
                        {
                            writer.WriteLine("\t\t\t\tif (!ACPanelWrapper.HasD150Panel(token))");
                        }
                        else
                        {
                            writer.WriteLine("\t\t\t\tif (!data.Active || !ACPanelWrapper.HasD150Panel(data.BDBuildingPK, token))");
                        }
                    }
                    writer.WriteLine("\t\t\t\t{");
                    writer.WriteLine("\t\t\t\t\t{0}Data old = {0}Wrapper.GetByPK(data.{1}PK, token);",
                                        base.ObjectSchema.Alias, d3000TableName);
                    writer.WriteLine("\t\t\t\t\tif (old != null)");
                    writer.WriteLine("\t\t\t\t\t{");
                    writer.WriteLine("\t\t\t\t\t\told.Delete();");
                    writer.WriteLine("\t\t\t\t\t\tSave(old, token);");
                    writer.WriteLine("\t\t\t\t\t}");
                    writer.WriteLine();
                    writer.WriteLine("\t\t\t\t\treturn;");
                    writer.WriteLine("\t\t\t\t}");
                }
                writer.WriteLine("\t\t\t}");
              
                writer.WriteLine();
            }

            if (base.ObjectSchema.Alias == "ACCardHolderBuildingMap")
            {
                writer.WriteLine("\t\t\tif (data.CardTypeID == D3000.Utils.CardTypeConstant._VIRTUAL_KEY)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\t{0}Data old = {0}Wrapper.GetByPK(data.{1}PK, token);",
                                 base.ObjectSchema.Alias, d3000TableName);
                writer.WriteLine("\t\t\t\tif (old != null)");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tif (data.Active)");
                writer.WriteLine("\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\told.Deleted = true;");
                writer.WriteLine("\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\telse");
                writer.WriteLine("\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\told.Delete();");
                writer.WriteLine("\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\tSave(old, token);");
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t\t\treturn;");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();
            }

            writer.WriteLine("\t\t\t{0}Data entity = null;", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tif(!data.IsNew)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tentity = {0}Wrapper.GetByPK(data.{1}PK, token);", base.ObjectSchema.Alias, d3000TableName);
            writer.WriteLine("\t\t\t\tif (entity == null && data.IsDeleted)");
            writer.WriteLine("\t\t\t\t\treturn;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tif(entity == null)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tentity = new {0}Data();", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();

            foreach (FieldSchema field in base.ObjectSchema.Fields)
            {
                if (field.Alias == "RowVersion" || field.Alias == "Deleted" || field.Alias == "Download")
                    continue;

                if(field.IsPK)
                {
                    writer.WriteLine("\t\t\tentity.{0} = data.{1}PK;", field.Alias, d3000TableName);
                    continue;
                }

                //Timezone
                if (base.ObjectSchema.Alias == "BDBuilding")
                {
                    if (field.Alias == "TimezoneName")
                    {
                        writer.WriteLine(
                            "\t\t\tD3000.Utils.TimezoneConstant zoneType = D3000.Utils.TimezoneConstant.FindByID(data.TimezoneID);");

                        writer.WriteLine("\t\t\tentity.TimezoneName = zoneType.Description;");
                        writer.WriteLine("\t\t\tentity.HoursDiff = (short)zoneType.HoursDiff;");

                        continue;
                    }

                    if (field.Alias == "HoursDiff")
                    {
                        continue;
                    }
                }

                //Panel
                if (base.ObjectSchema.Alias == "ACPanel")
                {
                    if (field.Alias == "UnitPhone")
                    {
                        writer.WriteLine(
                            "\t\t\tentity.UnitPhone = Misc.GetDialingPanelNumber(data.LongDistance, data.UnitPhone, data.UnitExt);");
                        continue;
                    }

                    if (field.Alias == "HistoryPhone")
                    {
                        writer.WriteLine(
                            "\t\t\tentity.HistoryPhone = Misc.GetDialingReceiverNumber(data.HistoryPhone, data.DialingPrefixPanel, data.DialingSuffixPanel);");
                        continue;
                    }

                    if (field.Alias == "AlarmPhone1")
                    {
                        writer.WriteLine(
                            "\t\t\tentity.AlarmPhone1 = Misc.GetDialingReceiverNumber(data.AlarmPhone1, data.DialingPrefixPanel, data.DialingSuffixPanel);");
                        continue;
                    }

                    if (field.Alias == "AlarmPhone2")
                    {
                        writer.WriteLine(
                            "\t\t\tentity.AlarmPhone2 = Misc.GetDialingReceiverNumber(data.AlarmPhone2, data.DialingPrefixPanel, data.DialingSuffixPanel);");
                        continue;
                    }
                }

                //Gateway
                if (base.ObjectSchema.Alias == "ACGateway")
                {
                    if (field.Alias == "InPhone")
                    {
                        writer.WriteLine(
                            "\t\t\tentity.InPhone = Misc.GetDialingPanelNumber(data.InPhoneLongDistance, data.InPhone, data.InExt);");
                        continue;
                    }

                    if (field.Alias == "OutPhone")
                    {
                        writer.WriteLine(
                            "\t\t\tentity.OutPhone = Misc.GetDialingPanelNumber(data.OutPhoneLongDistance, data.OutPhone, data.OutExt);");
                        continue;
                    }

                    if (field.Alias == "HistoryPhone")
                    {
                        writer.WriteLine(
                            "\t\t\tentity.HistoryPhone = Misc.GetDialingReceiverNumber(data.HistoryPhone, data.DialingPrefixPanel, data.DialingSuffixPanel);");
                        continue;
                    }

                    if (field.Alias == "AlarmPhone1")
                    {
                        writer.WriteLine(
                            "\t\t\tentity.AlarmPhone1 = Misc.GetDialingReceiverNumber(data.AlarmPhone1, data.DialingPrefixPanel, data.DialingSuffixPanel);");
                        continue;
                    }

                    if (field.Alias == "AlarmPhone2")
                    {
                        writer.WriteLine(
                            "\t\t\tentity.AlarmPhone2 = Misc.GetDialingReceiverNumber(data.AlarmPhone2, data.DialingPrefixPanel, data.DialingSuffixPanel);");
                        continue;
                    }

                }

                //Start From
                if(field.Alias == "StartFrom")
                {
                    if (base.ObjectSchema.Alias == "ACAccessLevelDetail" || base.ObjectSchema.Alias == "ACOutputGroupDetail"
                    || base.ObjectSchema.Alias == "ACMainZone")
                    {
                        writer.WriteLine(
                            "\t\t\tD3000.Utils.MainZoneConstant zoneType = D3000.Utils.MainZoneConstant.FindByID(data.MainZoneTypeID);");

                        writer.WriteLine("\t\t\tentity.StartFrom = zoneType.StartFrom;");
                        continue;
                    }

                    if(base.ObjectSchema.Alias == "ACInput")
                    {
                        writer.WriteLine("\t\t\tentity.StartFrom = 21;");
                        continue;
                    }

                    if (base.ObjectSchema.Alias == "ACSupervisory")
                    {
                        writer.WriteLine("\t\t\tentity.StartFrom = 31;");
                        continue;
                    }
                }

                //Other
                if (field.Alias == "BDBaseBuildingPK")
                {
                    writer.WriteLine("\t\t\tentity.BDBaseBuildingPK = data.BaseBDBuildingPK;");
                    continue;
                }

                if (field.Alias == "BDBaseAddress1")
                {
                    writer.WriteLine("\t\t\tentity.BDBaseAddress1 = data.BaseAddress1;");
                    continue;
                }

                if (field.Alias == "RedCard")
                {
                    writer.WriteLine("\t\t\tentity.RedCard = data.FunctionTypeID == D3000.Utils.FunctionCardTypeConstant._RedCard;");
                    continue;
                }

                if(field.Alias == "Month")
                {
                    writer.WriteLine("\t\t\tentity.Month = (byte)data.Date.Month;");
                }
                else if (field.Alias == "Day")
                {
                    writer.WriteLine("\t\t\tentity.Day = (byte)data.Date.Day;");
                }
                else if (field.Alias == "Year")
                {
                    writer.WriteLine("\t\t\tentity.Year = (short)data.Date.Year;");
                }
                else
                {
                    writer.WriteLine("\t\t\tentity.{0} = data.{0};", field.Alias);
                }
            }

            //Joined Field
            List<string> joinedList = GetJoinedField();
            foreach (string item in joinedList)
            {
                writer.WriteLine("\t\t\tentity.{0} = data.{0};", item);
            }
            
            if (this.HasDownloadField())
            {
                writer.WriteLine();
                if(base.ObjectSchema.Alias == "ACGateway")
                {
                    writer.WriteLine("\t\t\tif (!data.Active)");
                }
                else
                {
                    writer.WriteLine("\t\t\tif (!data.Active || data.IsE150)");
                }
               
                writer.WriteLine("\t\t\t{");
                if (this.HasDeletedField())
                {
                    writer.WriteLine("\t\t\t\tentity.Deleted = false;");
                }
                writer.WriteLine("\t\t\t\tentity.Download = false;");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\telse");
                writer.WriteLine("\t\t\t{");
                if (this.HasDeletedField())
                {
                    writer.WriteLine("\t\t\t\tentity.Deleted = data.IsDeleted;");
                }
                writer.WriteLine("\t\t\t\tentity.Download = entity.IsDirty;");
                writer.WriteLine("\t\t\t}");
            }
            else
            {
                if (this.HasDeletedField())
                {
                    writer.WriteLine();
                    writer.WriteLine("\t\t\tentity.Deleted = data.IsDeleted;");
                }
            }
           
            writer.WriteLine("\t\t\tSave(entity, token);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic static void SyncDelete(Guid {0}PK, Cheke.SecurityToken token)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}Data entity = {0}Wrapper.GetByPK({0}PK, token);", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tif (entity == null)");
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine();

            if (!this.HasDeletedField())
            {
                writer.WriteLine("\t\t\tentity.Delete();");
            }
            else
            {
                if (this.HasDownloadField())
                {
                    writer.WriteLine("\t\t\tif (!entity.Active || entity.IsE150)");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tentity.Delete();");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine("\t\t\telse");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tentity.Deleted = true;");
                    writer.WriteLine("\t\t\t\tentity.Download = true;");
                    writer.WriteLine("\t\t\t}");
                }
                else
                {
                    if (base.ObjectSchema.Alias == "ACMasterCard")
                    {
                        writer.WriteLine("\t\t\tif (!ACPanelWrapper.HasD150Panel(token))");
                    }
                    else
                    {
                        writer.WriteLine(
                            "\t\t\tif (!entity.Active || !ACPanelWrapper.HasD150Panel(entity.BDBuildingPK, token))");
                    }

                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tentity.Delete();");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine("\t\t\telse");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tentity.Deleted = true;");
                    writer.WriteLine("\t\t\t}");
                }
            }

            writer.WriteLine("\t\t\tSave(entity, token);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate static void Save({0}Data entity, Cheke.SecurityToken token)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (!entity.IsDirty)");
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine("");
            writer.WriteLine("\t\t\t{0}Wrapper.SaveUnderTransaction(entity, token);", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t}");
        }

        private bool HasDeletedField()
        {
            foreach (FieldSchema field in base.ObjectSchema.Fields)
            {
                if (field.Alias == "Deleted")
                    return true;
            }

            return false;
        }

        private bool HasDownloadField()
        {
            foreach (FieldSchema field in base.ObjectSchema.Fields)
            {
                if (field.Alias == "Download")
                    return true;
            }

            return false;
        }

        private string GetUKFieldInputPara(string prefix)
        {

            foreach (IndexSchema index in base.ObjectSchema.Indexs)
            {
                if (index.IsUniqueConstraint)
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (string key in index.Keys)
                    {
                        builder.AppendFormat("{0}.{1}, ", prefix, key);
                    }

                    return builder.ToString();
                }
            }

            return string.Empty;
        }

        private List<string> GetJoinedField()
        {
            List<string> retList = new List<string>();
            Type type = Utils.GetDataType(base.MappingSchema, base.ObjectSchema);
            if (type == null)
                return retList;

            SortedList<string, string> fieldIndex = new SortedList<string, string>();
            foreach (FieldSchema field in base.ObjectSchema.Fields)
            {
                if(fieldIndex.ContainsKey(field.Alias))
                    continue;

                fieldIndex.Add(field.Alias, field.Alias);
            }

            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);
            foreach (PropertyInfo info in propertyInfos)
            {
                if(!info.CanRead || !info.CanWrite)
                    continue;

                if(info.Name == "Download")
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
