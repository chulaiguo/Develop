using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;
using System;

namespace JetCode.FactoryTest
{
    public class FactorySettingCategory : FactoryBase
    {
        public FactorySettingCategory(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Utils", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            SortedList<string, ObjectSchema> sortedList = new SortedList<string, ObjectSchema>();
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                if (!item.Name.StartsWith("ZZ"))
                    continue;

                if (sortedList.ContainsKey(item.Name))
                    continue;

                sortedList.Add(item.Name, item);
            }

            writer.WriteLine("\tpublic class SettingCategoryConstant");
            writer.WriteLine("\t{");

            int id = 1000;
            foreach (KeyValuePair<string, ObjectSchema> pair in sortedList)
            {
                id++;

                writer.WriteLine("\t\tpublic const int _{0} = {1};", pair.Value.Name, id);
            }
            writer.WriteLine();

            writer.WriteLine("\t\tprivate readonly int _id = 0;");
            writer.WriteLine("\t\tpublic int ID");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return _id; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate readonly string _description = string.Empty;");
            writer.WriteLine("\t\tpublic string Description");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return _description; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate SettingCategoryConstant(int id, string description)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._id = id;");
            writer.WriteLine("\t\t\tthis._description = description;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic static SettingCategoryConstant[] GetAll()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tSettingCategoryConstant[] list = new SettingCategoryConstant[]");
            writer.WriteLine("\t\t\t{");
            foreach (KeyValuePair<string, ObjectSchema> pair in sortedList)
            {
                writer.WriteLine("\t\t\t\tnew SettingCategoryConstant(_{0}, \"{0}\", \"{1}\"),", pair.Value.Name, pair.Value.Alias);
            }
            writer.WriteLine("\t\t\t};");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic static SettingCategoryConstant DefaultValue");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return new SettingCategoryConstant(0, \"Undefined\"); }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic static SettingCategoryConstant FindByID(int id)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tSettingCategoryConstant[] list = GetAll();");
            writer.WriteLine("\t\t\tforeach (SettingCategoryConstant item in list)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif (item.ID == id)");
            writer.WriteLine("\t\t\t\t\treturn item;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\t return DefaultValue;");
            writer.WriteLine("\t\t}");

            writer.WriteLine("\t}");
        }
    }
}
