using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryDataService
{
    public class FactoryDataServiceInterface : FactoryBase
    {
        public FactoryDataServiceInterface(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using Cheke;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using {0}.Data;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.IDataService", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                writer.WriteLine("\tpublic partial interface I{0}DataService", item.Alias);
                writer.WriteLine("\t{");
                writer.WriteLine("\t\tResult Purge({0}Data entity);", item.Alias);
                writer.WriteLine("\t\tResult Save({0}Data entity);", item.Alias);
                writer.WriteLine("\t\tResult Save({0}DataCollection list);", item.Alias);
                //writer.WriteLine("\t\tbool SaveUnderTransaction({0}Data entity);", item.Alias);
                //writer.WriteLine("\t\tbool SaveUnderTransaction({0}DataCollection list);", item.Alias);
                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }
    }
}
