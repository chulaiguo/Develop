using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using JetCode.BizSchema;
using JetCode.Factory;
using System;

namespace JetCode.FactoryTest
{
    public class FactoryDiff : FactoryBase
    {
        public FactoryDiff(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void BeginWrite(StringWriter writer)
        {
           
        }

        protected override void WriteUsing(StringWriter writer)
        {
           
        }

        protected override void EndWrite(StringWriter writer)
        {

        }

        protected override void WriteContent(StringWriter writer)
        {
            //3
            //Old
            SortedList < string, Type > list = Utils.GetDataTypeList(base.ProjectName);
            SortedList<string, Type> oldTypeIndex = new SortedList<string, Type>();
            foreach (KeyValuePair<string, Type> pair in list)
            {
          
                if(!pair.Key.EndsWith("Data"))
                    continue;

                string name = pair.Key.Substring(0, pair.Key.Length - 4);
                oldTypeIndex.Add(name, pair.Value);
            }

            //Current
            SortedList<string, ObjectSchema> currentTypeIndex = new SortedList<string, ObjectSchema>();
            foreach (ObjectSchema obj in base.MappingSchema.Objects)
            {
                currentTypeIndex.Add(obj.Name, obj);
            }

            //Diff
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, ObjectSchema> pair in currentTypeIndex)
            {
                builder.AppendLine(string.Format("{0}: ", pair.Key));
                if (!oldTypeIndex.ContainsKey(pair.Key))
                {
                    builder.AppendLine(string.Format("++++++++++++++++++++ Is New Table.+++++++++++++++++++"));
                }
                else
                {
                    string diff = DiffTableField(oldTypeIndex[pair.Key], pair.Value);
                    if (string.IsNullOrEmpty(diff))
                    {
                        builder.AppendLine(string.Format("======================================== OK!"));
                    }
                    else
                    {
                        builder.AppendLine(diff);
                    }
                }
            }

            foreach (KeyValuePair<string, Type> pair in oldTypeIndex)
            {
                if (!currentTypeIndex.ContainsKey(pair.Key))
                {
                    builder.AppendLine(string.Format("{0}: ", pair.Key));
                    builder.AppendLine(string.Format("------------------- Is Deleted Table.--------------------"));
                }
            }

            writer.WriteLine(builder.ToString());
        }

        private string DiffTableField(Type old, ObjectSchema current)
        {
            SortedList<string, PropertyInfo> oldIndex = new SortedList<string, PropertyInfo>();
            PropertyInfo[] oldInfo = old.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (PropertyInfo info in oldInfo)
            {
                if(!info.PropertyType.IsValueType && info.PropertyType != typeof(string) && info.Name != "RowVersion")
                    continue;

                if(!info.CanWrite || !info.CanRead)
                    continue;

                if (oldIndex.ContainsKey(info.Name))
                    continue;

                oldIndex.Add(info.Name, info);
            }

            SortedList<string, FieldSchema> currentIndex = new SortedList<string, FieldSchema>();
            foreach (FieldSchema field in current.Fields)
            {
                if (currentIndex.ContainsKey(field.Name))
                    continue;

                currentIndex.Add(field.Name, field);
            }

            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, FieldSchema> pair in currentIndex)
            {
                if (!oldIndex.ContainsKey(pair.Key))
                {
                    builder.AppendLine(string.Format("+++++ {0}: Field Inserted;+++++", pair.Key));
                }
                else
                {
                    PropertyInfo oldProperty = oldIndex[pair.Key];
                    if (oldProperty.PropertyType != base.Utilities.ToDotNetType(pair.Value.DataType))
                    {
                        builder.AppendLine(string.Format("***** {0}: Field Type Modified;*****", pair.Key));
                    }
                }
            }

            foreach (KeyValuePair<string, PropertyInfo> pair in oldIndex)
            {
                if (!currentIndex.ContainsKey(pair.Key))
                {
                    builder.AppendLine(string.Format("----- {0}: Field Deleted;-----", pair.Key));
                }
            }

            return builder.ToString();
        }
        
    }
}
