using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryTest
{
    public class FactoryLog : FactoryBase
    {
        public FactoryLog(MappingSchema mappingSchema)
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
            writer.WriteLine("namespace {0}.DBLog", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            writer.WriteLine("\tpublic class DBEditLog : IDBEditLog");
            writer.WriteLine("\t{");

            writer.WriteLine("\t\tpublic int LogDelete(BusinessBase entity, SecurityToken token)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tswitch (entity.TableName)");
            writer.WriteLine("\t\t\t{");
            SortedList<string, ObjectSchema> index = new SortedList<string, ObjectSchema>();
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                if (item.Alias.StartsWith("Log"))
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
                writer.WriteLine("\t\t\t\t\t\tInsertDeleteLog(data, data.StrStorePK, token);");
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
