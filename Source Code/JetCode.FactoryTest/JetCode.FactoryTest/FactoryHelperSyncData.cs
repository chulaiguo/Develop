using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using JetCode.BizSchema;
using JetCode.Factory;
namespace JetCode.FactoryTest
{
    public class FactoryHelperSyncData : FactoryBase
    {
        public FactoryHelperSyncData(MappingSchema mappingSchema)
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
            writer.WriteLine("namespace {0}.FacadeService.Utils", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic static class HelperSync");
            writer.WriteLine("\t{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            string dllName = string.Format("{0}.Data.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);

            MappingInsertOrder order = new MappingInsertOrder(base.MappingSchema);
            StringCollection insertOrderList = order.GetInsertOrder();

            foreach (string item in insertOrderList)
            {
                if (item == "CacheOffline" || item == "LogDeleteCard" || item == "ACDownloadTask" || item == "BDPanelCommand")
                    continue;

                this.WriteSyncData(writer, item, typeList);
            }
        }

        private void WriteSyncData(StringWriter writer, string tableName, SortedList<string, Type> typeList)
        {
            string typeKey = string.Format("{0}Data", tableName);
            if (!typeList.ContainsKey(typeKey))
                return;

            writer.WriteLine("\t\tpublic static void Sync{0}(D3000.Data.{0}Data src, E3000.Data.{0}Data dst)", tableName);
            writer.WriteLine("\t\t{");
         
            List<PropertyInfo> list = this.GetPropertyList(typeList[typeKey]);
            foreach (PropertyInfo field in list)
            {
                if (field.Name == "RowVersion")
                    continue;

                if (field.Name == "Active")
                {
                    if (tableName == "ACCardHolderBuildingMap" || tableName == "ACPanelFunctionCardMap" ||
                        tableName == "ACVisitor")
                    {
                        continue;
                    }
                }

                if (tableName == "ACPanel" && field.Name == "LastConnected")
                {
                    continue;
                }

                writer.WriteLine("\t\t\tdst.{0} = src.{0};", field.Name);
            }

            if (tableName == "ACCardHolderBuildingMap" || tableName == "ACPanelFunctionCardMap")
            {
                writer.WriteLine("\t\t\tdst.Active = dst.IsValidCard;");
            }

            if (tableName == "ACVisitor")
            {
                writer.WriteLine("\t\t\tdst.Active = dst.IsValidVisitor;");
            }
            
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private List<PropertyInfo> GetPropertyList(Type type)
        {
            List<PropertyInfo> retList = new List<PropertyInfo>();

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
            foreach (PropertyInfo info in properties)
            {
                if (info.Name == "IsDirty" || info.Name == "IsValid" || info.Name == "PKString"
                        || info.Name == "MarkAsDeleted" || info.Name == "TableName" || info.Name == "RowVersion")
                    continue;

                if (!info.CanWrite || !info.CanRead)
                    continue;

                if (info.PropertyType.IsValueType || info.PropertyType == typeof(string) || info.PropertyType == typeof(byte[]))
                {

                    retList.Add(info);
                }
            }

            return retList;
        }
    }
}
