using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryTest
{
    public class FactoryUpdateCache : FactoryBase
    {
        public FactoryUpdateCache(MappingSchema mappingSchema)
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
            writer.WriteLine("namespace {0}.UpdateCache", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            writer.WriteLine("\tpublic static class LogChanged");
            writer.WriteLine("\t{");

            writer.WriteLine("\t\tpublic static void RecordLog(BusinessBase entity, string changeType, DateTime changeTime)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tswitch (entity.TableName)");
            writer.WriteLine("\t\t\t{");
            SortedList<string, ObjectSchema> index = new SortedList<string, ObjectSchema>();
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                if (item.Alias.StartsWith("Log") || item.Alias.StartsWith("ZZ"))
                    continue;

                if(index.ContainsKey(item.Alias))
                    continue;

                index.Add(item.Alias, item);
            }

            foreach (KeyValuePair<string, ObjectSchema> pair in index)
            {
                writer.WriteLine("\t\t\t\tcase {0}Schema.TableName:", pair.Key);
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t{0}Data data = entity as {0}Data;", pair.Key);
                writer.WriteLine("\t\t\t\t\tif (data != null)");
                writer.WriteLine("\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\tUpdateLog(data.BDBuildingPK, entity.TableName, changeType, changeTime);");
                writer.WriteLine("\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("\t\t\t\tbreak;"); 
            }
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t}");
            writer.WriteLine();
        }
    }
}
