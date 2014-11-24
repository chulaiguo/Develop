using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryData
{
    public class FactoryDBRule : FactoryBase
    {
        public FactoryDBRule(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Utils", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\t[Serializable]");
            writer.WriteLine("\tpublic class DBRuleConstant");
            writer.WriteLine("\t{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            SortedList<string, int> oldIndex;

            string dllName = string.Format("{0}.Utils.dll", base.ProjectName);
            SortedList<string, Type> typeList = Utils.GetTypeList(base.ProjectName, dllName);
            string key = "DBRuleConstant";
            if(typeList.ContainsKey(key))
            {
                oldIndex = this.GetFieldList(typeList[key]);
            }
            else
            {
                oldIndex = new SortedList<string, int>();
            }

            SortedList<int, string> allIndex = this.GetAllIndex(oldIndex);
            foreach (KeyValuePair<int, string> pair in allIndex)
            {
                writer.WriteLine("\t\tpublic const int _{0} = {1:f0};", pair.Value, pair.Key);
            }

            writer.WriteLine();
            this.WriteMethods(writer, allIndex);
        }

        private void WriteMethods(StringWriter writer, SortedList<int, string> index)
        {
            writer.WriteLine("\t\tprivate readonly int _id = 0;");
            writer.WriteLine("\t\tpublic int ID");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return _id; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate readonly string _tableName = string.Empty;");
            writer.WriteLine("\t\tpublic string TableName");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return _tableName; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate readonly string _description = string.Empty;");
            writer.WriteLine("\t\tpublic string Description");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return _description; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate DBRuleConstant(int id, string tableName, string description)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._id = id;");
            writer.WriteLine("\t\t\tthis._tableName = tableName;");
            writer.WriteLine("\t\t\tthis._description = description;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic static DBRuleConstant[] GetAll()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tDBRuleConstant[] list = new DBRuleConstant[]");
            writer.WriteLine("\t\t\t{");
            foreach (KeyValuePair<int, string> pair in index)
            {
                writer.WriteLine("\t\t\t\tnew DBRuleConstant(_{0}, \"{0}\", \"{0}\"),", pair.Value);
            }
            writer.WriteLine("\t\t\t};");
            writer.WriteLine("\t\t\treturn list;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic static DBRuleConstant DefaultValue");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return new DBRuleConstant(0, \"Undefined\", \"Undefined\"); }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic static DBRuleConstant FindByID(int id)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tDBRuleConstant[] list = GetAll();");
            writer.WriteLine("\t\t\tforeach (DBRuleConstant item in list)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif (item.ID == id)");
            writer.WriteLine("\t\t\t\t\treturn item;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn DefaultValue;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic static DBRuleConstant FindByTableName(string tableName)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tDBRuleConstant[] list = GetAll();");
            writer.WriteLine("\t\t\tforeach (DBRuleConstant item in list)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif (string.Compare(item.TableName, tableName, 0) == 0)");
            writer.WriteLine("\t\t\t\t\treturn item;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn DefaultValue;");
            writer.WriteLine("\t\t}");
        }

        private SortedList<int, string> GetAllIndex(SortedList<string, int> oldIndex)
        {
            SortedList<string, int> index = new SortedList<string, int>();

            //New Index
            SortedList<string, string> newIndex = new SortedList<string, string>();
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                if (newIndex.ContainsKey(item.Name))
                    continue;

                newIndex.Add(item.Name, item.Name);
            }

            //delete items
            List<string> deleteList = new List<string>();
            foreach (KeyValuePair<string, int> pair in oldIndex)
            {
                if (!newIndex.ContainsKey(pair.Key))
                {
                    deleteList.Add(pair.Key);
                }
            }

            foreach (string item in deleteList)
            {
                oldIndex.Remove(item);
            }


            //Old
            int maxValue = 1000;
            foreach (KeyValuePair<string, int> pair in oldIndex)
            {
                if (pair.Value > maxValue)
                {
                    maxValue = pair.Value;
                }

                if (!index.ContainsKey(pair.Key))
                {
                    index.Add(pair.Key, pair.Value);
                }
            }

            //New
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                if (oldIndex.ContainsKey(item.Name))
                    continue;

                maxValue++;
                if (!index.ContainsKey(item.Name))
                {
                    index.Add(item.Name, maxValue);
                }
            }

            SortedList<int, string> sortedIndex = new SortedList<int, string>();
            foreach (KeyValuePair<string, int> pair in index)
            {
                if (pair.Key.StartsWith("Log") || pair.Key.StartsWith("Buf")
                   || pair.Key.StartsWith("ZZ"))
                    continue;

                //if (pair.Key.StartsWith("BDSite"))
                //    continue;

                //hello git

                if (pair.Key == "UtilSettingCategory" || pair.Key == "UtilSettingDetail"
                    || pair.Key == "ACHistory" || pair.Key == "ACAlarm")
                    continue;

                sortedIndex.Add(pair.Value, pair.Key);
            }

            return sortedIndex;
        }

        private SortedList<string, int> GetFieldList(Type type)
        {
            SortedList<string, int> retIndex = new SortedList<string, int>();

            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static);
            foreach (FieldInfo info in fields)
            {
                 if(retIndex.ContainsKey(info.Name))
                     continue;

                object obj = info.GetValue(null);
                if(obj == null)
                    continue;

                int value;
                if(!int.TryParse(obj.ToString(), out value))
                    continue;

                retIndex.Add(info.Name.Substring(1), value);
            }

            return retIndex;
        }
    }
}
