using System.Collections.Specialized;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
    public class FactoryTestData : FactoryBase
    {
        public FactoryTestData(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using {0}.ViewObj;", base.ProjectName);
            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.TestData", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic class Program");
            writer.WriteLine("\t{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            this.WriteMain(writer);
            this.WriteShowHelpMenu(writer);
            this.WriteLoadData(writer);
            this.WriteLoadFromExcel(writer);
            this.WriteConvertData(writer);
            this.WriteInsertAll(writer);
            this.WriteDeleteAll(writer);
        }

        private void WriteMain(StringWriter writer)
        {
            writer.WriteLine("\t\tstatic void Main(string[] args)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (args.Length != 1)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tShowHelpMenu();");
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tswitch (args[0].ToLower())");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tcase \"/?\":");
            writer.WriteLine("\t\t\t\tcase \"/help\":");
            writer.WriteLine("\t\t\t\t\tShowHelpMenu();");
            writer.WriteLine("\t\t\t\t\tbreak;");

            writer.WriteLine("\t\t\t\tcase \"/i\":");
            writer.WriteLine("\t\t\t\t\tConsole.WriteLine(\"Start to create test data ....\");");
            writer.WriteLine("\t\t\t\t\ttry");
            writer.WriteLine("\t\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t\tResult r = InsertAll();");
            writer.WriteLine("\t\t\t\t\t\tif(r.OK)");
            writer.WriteLine("\t\t\t\t\t\t\tConsole.WriteLine(\"Finished creating test data ....\");");
            writer.WriteLine("\t\t\t\t\t}");
            writer.WriteLine("\t\t\t\t\tcatch (Exception ex)");
            writer.WriteLine("\t\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t\tConsole.WriteLine(ex.Message);");
            writer.WriteLine("\t\t\t\t\t\tif (ex.InnerException != null)");
            writer.WriteLine("\t\t\t\t\t\t\tConsole.WriteLine(ex.InnerException.Message);");
            writer.WriteLine("\t\t\t\t\t}");
            writer.WriteLine("\t\t\t\t\tbreak;");

            writer.WriteLine("\t\t\t\tcase \"/u\":");
            writer.WriteLine("\t\t\t\t\tConsole.WriteLine(\"Start to delete test data ....\");");
            writer.WriteLine("\t\t\t\t\ttry");
            writer.WriteLine("\t\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t\tResult r = DeleteAll();");
            writer.WriteLine("\t\t\t\t\t\tif(r.OK)");
            writer.WriteLine("\t\t\t\t\t\t\tConsole.WriteLine(\"Finished deleting test data ....\");");
            writer.WriteLine("\t\t\t\t\t}");
            writer.WriteLine("\t\t\t\t\tcatch (Exception ex)");
            writer.WriteLine("\t\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t\tConsole.WriteLine(ex.Message);");
            writer.WriteLine("\t\t\t\t\t\tif (ex.InnerException != null)");
            writer.WriteLine("\t\t\t\t\t\t\tConsole.WriteLine(ex.InnerException.Message);");
            writer.WriteLine("\t\t\t\t\t}");
            writer.WriteLine("\t\t\t\t\tbreak;");

            writer.WriteLine("\t\t\t\tdefault:");
            writer.WriteLine("\t\t\t\t\tShowHelpMenu();");
            writer.WriteLine("\t\t\t\t\tbreak;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tConsole.ReadLine();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteShowHelpMenu(StringWriter writer)
        {
            writer.WriteLine("\t\tprivate static void ShowHelpMenu()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tConsole.WriteLine(\"USAGE: {0}.TestData.exe [options]\");", base.ProjectName);
            writer.WriteLine("\t\t\tConsole.WriteLine(\"Options:\");");
            writer.WriteLine("\t\t\tConsole.WriteLine(\"\\t/? or /help\\tDisplay this usage message.\");");
            writer.WriteLine("\t\t\tConsole.WriteLine(\"\\t/u\\t\\tUn-load data from the database\");");
            writer.WriteLine("\t\t\tConsole.WriteLine(\"\\t/i\\t\\tInstall data into the database\");");
            writer.WriteLine("");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteLoadData(StringWriter writer)
        {
            writer.WriteLine("\t\tprivate static SortedList<string, BusinessCollectionBase> LoadData()");
            writer.WriteLine("\t\t{");

            writer.WriteLine("\t\t\tSortedList<string, BusinessCollectionBase> sortedList = new SortedList<>(string, BusinessCollectionBase);");
            writer.WriteLine("\t\t\tstring fileName = string.Format(@\"{{0}}\\{{1}}.xsl\", Application.StartupPath, {0}", base.ProjectName);
            writer.WriteLine("\t\t\tstring[] sheetList = Cheke.Excel.ExcelSheetReader.GetExcelSheetsList(fileName);");
            writer.WriteLine("\t\t\tforeach (string sheet in sheetList)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif (!sheet.EndsWith(\"$\"))");
            writer.WriteLine("\t\t\t\t\tcontinue;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tstring tableName = sheet.TrimEnd('$');");
            writer.WriteLine("\t\t\t\tBusinessCollectionBase list = Activator.CreateInstance(typeof({0})) as BusinessCollectionBase;");
            writer.WriteLine("\t\t\t\tif (list == null)");
            writer.WriteLine("\t\t\t\t\tcontinue;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tDataTable dt = Cheke.Excel.ExcelSheetReader.LoadSheetIntoDataSet(fileName, sheet, true);");
            writer.WriteLine("\t\t\t\tLoadFromExcel(dt, list);");
            writer.WriteLine("\t\t\t\tsortedList.Add(tableName, list);");
            writer.WriteLine("\t\t\t}");

            writer.WriteLine("\t\t\treturn sortedList;");
            writer.WriteLine("\t\t}");
        }

        private void WriteLoadFromExcel(StringWriter writer)
        {
            writer.WriteLine("\t\tprivate static void LoadFromExcel(DataTable dt, BusinessCollectionBase list)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tPropertyInfo[] properties = list.GetItemType().GetProperties();");
            writer.WriteLine("\t\t\tHashtable propertyList = new Hashtable();");
            writer.WriteLine("\t\t\tforeach (PropertyInfo item in properties)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif (!item.CanRead || !item.CanWrite)");
            writer.WriteLine("\t\t\t\t\tcontinue;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tif (!item.PropertyType.IsValueType && item.PropertyType != typeof(string))");
            writer.WriteLine("\t\t\t\t\tcontinue;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tpropertyList.Add(item.Name, item);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tforeach (DataRow row in dt.Rows");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tobject entity = Activator.CreateInstance(list.GetItemType());");
            writer.WriteLine("\t\t\t\tforeach (DataColumn column in dt.Columns)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tPropertyInfo property = propertyList[column.ColumnName] as PropertyInfo;");
            writer.WriteLine("\t\t\t\t\tif (property == null)");
            writer.WriteLine("\t\t\t\t\t\tcontinue;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\t\tstring colValue = row[column.ColumnName].ToString().Trim();");
            writer.WriteLine("\t\t\t\t\tproperty.SetValue(entity, Convert(property, colValue), null);");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\tlist.Add(entity);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
        }

        private void WriteConvertData(StringWriter writer)
        {
            writer.WriteLine("\t\tprivate static object Convert(PropertyInfo property, string colValue)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tswitch (property.PropertyType.Name)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tcase \"Boolean\":");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tbool result;");
            writer.WriteLine("\t\t\t\t\tif (bool.TryParse(colValue, out result))");
            writer.WriteLine("\t\t\t\t\t\treturn result;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\t\treturn colValue == \"T\" || colValue == \"Y\" || colValue == \"1\";");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\tcase \"Byte\":");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tbyte result;");
            writer.WriteLine("\t\t\t\t\tif (byte.TryParse(colValue, out result))");
            writer.WriteLine("\t\t\t\t\t\treturn result;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\t\treturn (byte)0;");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\tcase \"Int16\":");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tshort result;");
            writer.WriteLine("\t\t\t\t\tif (short.TryParse(colValue, out result))");
            writer.WriteLine("\t\t\t\t\t\treturn result;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\t\treturn (short)0;");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\tcase \"Int32\":");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tint result;");
            writer.WriteLine("\t\t\t\t\tif (int.TryParse(colValue, out result))");
            writer.WriteLine("\t\t\t\t\t\treturn result;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\t\treturn 0;");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\tcase \"Int64\":");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tlong result;");
            writer.WriteLine("\t\t\t\t\tif (long.TryParse(colValue, out result))");
            writer.WriteLine("\t\t\t\t\t\treturn result;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\t\treturn (long)0;");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\tcase \"Decimal\":");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tdecimal result;");
            writer.WriteLine("\t\t\t\t\tif (decimal.TryParse(colValue, out result))");
            writer.WriteLine("\t\t\t\t\t\treturn result;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\t\treturn (decimal)0;");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\tcase \"Single\":");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tfloat result;");
            writer.WriteLine("\t\t\t\t\tif (float.TryParse(colValue, out result))");
            writer.WriteLine("\t\t\t\t\t\treturn result;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\t\treturn (float)0;");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\tcase \"Double\":");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tdouble result;");
            writer.WriteLine("\t\t\t\t\tif (double.TryParse(colValue, out result))");
            writer.WriteLine("\t\t\t\t\t\treturn result;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\t\treturn (double)0;");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\tcase \"DateTime\":");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tDateTime result;");
            writer.WriteLine("\t\t\t\t\tif (DateTime.TryParse(colValue, out result))");
            writer.WriteLine("\t\t\t\t\t\treturn result;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t\t\treturn new DateTime(1900, 1, 1);");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\tcase \"Guid\":");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\ttry");
            writer.WriteLine("\t\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t\treturn new Guid(colValue);");
            writer.WriteLine("\t\t\t\t\t}");
            writer.WriteLine("\t\t\t\t\tcatch");
            writer.WriteLine("\t\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t\treturn Guid.Empty;");
            writer.WriteLine("\t\t\t\t\t}");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t\tdefault:");
            writer.WriteLine("\t\t\t\t\treturn colValue;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
        }

        private void WriteInsertAll(StringWriter writer)
        {
            writer.WriteLine("\t\tprivate static Result InsertAll()");
            writer.WriteLine("\t\t{");

            writer.WriteLine("\t\t\tSortedList<string, BusinessCollectionBase> sortedList = LoadData();");

            MappingInsertOrder order = new MappingInsertOrder(base.MappingSchema);
            StringCollection list = order.GetInsertOrder();
            writer.WriteLine("\t\t\tResult r = new Result(true);");
            for (int i = 0; i < list.Count - 1; i++)
            {
                writer.WriteLine("\t\t\tif(sortedList.Containkey({0})");
                writer.WriteLine("\t\t\t");
                writer.WriteLine("\t\t\t");
                writer.WriteLine();
                writer.WriteLine("\t\t\t{0}Collection list = {0}.GetAll();", list[i]);
                writer.WriteLine("\t\t\tlist.Clear();");
                writer.WriteLine("\t\t\tr = list.Save();");
                writer.WriteLine("\t\t\tif(!r.OK)");
                writer.WriteLine("\t\t\t{");
                string error = "string.Format(\"" + list[i] + ": {0}\"";
                writer.WriteLine("\t\t\t\tConsole.WriteLine({0}, r.ToString()));", error);
                writer.WriteLine("\t\t\t\treturn r;");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\telse");
                writer.WriteLine("\t\t\t{");
                string message = "string.Format(\"" + list[i] + " data has been created successfully ... {0}\"";
                writer.WriteLine("\t\t\t\tConsole.WriteLine({0}, DateTime.Now.ToLongTimeString()));", message);
                writer.WriteLine("\t\t\t}");
            }

            writer.WriteLine("\t\t\treturn r;");
            writer.WriteLine("\t\t}");
        }

        private void WriteDeleteAll(StringWriter writer)
        {
            writer.WriteLine("\t\tprivate static Result DeleteAll()");
            writer.WriteLine("\t\t{");

            MappingInsertOrder order = new MappingInsertOrder(base.MappingSchema);
            StringCollection list = order.GetInsertOrder();
            writer.WriteLine("\t\t\tResult r = new Result(true);");
            for (int i = list.Count - 1; i >= 0; i--)
            {
                writer.WriteLine("\t\t\t{0}Collection list = {0}.GetAll();", list[i]);
                writer.WriteLine("\t\t\tlist.Clear();");
                writer.WriteLine("\t\t\tr = list.Save();");
                writer.WriteLine("\t\t\tif(!r.OK)");
                writer.WriteLine("\t\t\t{");
                string error = "string.Format(\"" + list[i] + ": {0}\"";
                writer.WriteLine("\t\t\t\tConsole.WriteLine({0}, r.ToString()));", error);
                writer.WriteLine("\t\t\t\treturn r;");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\telse");
                writer.WriteLine("\t\t\t{");
                string message = "string.Format(\"" + list[i] +" data has been deleted successfully ... {0}\"";
                writer.WriteLine("\t\t\t\tConsole.WriteLine({0}, DateTime.Now.ToLongTimeString()));", message);
                writer.WriteLine("\t\t\t}");
            }

            writer.WriteLine("\t\t\treturn r;");
            writer.WriteLine("\t\t}");
        }
    }
}
