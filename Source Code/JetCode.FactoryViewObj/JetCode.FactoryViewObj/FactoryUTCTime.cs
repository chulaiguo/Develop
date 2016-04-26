using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryViewObj
{
    public class FactoryUTCTime : FactoryBase
    {
        public FactoryUTCTime(MappingSchema mappingSchema)
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
            writer.WriteLine("namespace {0}.ViewObj", base.ProjectName);
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
                writer.WriteLine("\tpublic partial class {0}", item.Alias);
                writer.WriteLine("\t{");

                writer.WriteLine("\t\tpublic override DateTime CreatedOn");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tDateTime utc = new DateTime(base.CreatedOn.Ticks, DateTimeKind.Utc);");
                writer.WriteLine("\t\t\t\treturn utc.ToLocalTime();");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\tset");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tbase.CreatedOn = value.ToUniversalTime();");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                writer.WriteLine("\t\tpublic override DateTime ModifiedOn");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tDateTime utc = new DateTime(base.ModifiedOn.Ticks, DateTimeKind.Utc);");
                writer.WriteLine("\t\t\t\treturn utc.ToLocalTime();");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\tset");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tbase.ModifiedOn = value.ToUniversalTime();");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                if (item.Alias == "LogDBEditActivity")
                {
                    writer.WriteLine("\t\tpublic override DateTime LogDateTime");
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tget");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tDateTime utc = new DateTime(base.LogDateTime.Ticks, DateTimeKind.Utc);");
                    writer.WriteLine("\t\t\t\treturn utc.ToLocalTime();");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine("\t\t\tset");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tbase.LogDateTime = value.ToUniversalTime();");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }

                if (item.Alias == "LogDBDeleteActivity")
                {
                    writer.WriteLine("\t\tpublic override DateTime LogDateTime");
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tget");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tDateTime utc = new DateTime(base.LogDateTime.Ticks, DateTimeKind.Utc);");
                    writer.WriteLine("\t\t\t\treturn utc.ToLocalTime();");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine("\t\t\tset");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tbase.LogDateTime = value.ToUniversalTime();");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }

                if (item.Alias == "BDSiteSyncTable")
                {
                    writer.WriteLine("\t\tpublic override DateTime DBLastSync");
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tget");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tDateTime utc = new DateTime(base.DBLastSync.Ticks, DateTimeKind.Utc);");
                    writer.WriteLine("\t\t\t\treturn utc.ToLocalTime();");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine("\t\t\tset");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tbase.DBLastSync = value.ToUniversalTime();");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }

                if (item.Alias == "BDSitePanelSyncTask")
                {
                    writer.WriteLine("\t\tpublic override DateTime TaskLastSync");
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tget");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tDateTime utc = new DateTime(base.TaskLastSync.Ticks, DateTimeKind.Utc);");
                    writer.WriteLine("\t\t\t\treturn utc.ToLocalTime();");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine("\t\t\tset");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tbase.TaskLastSync = value.ToUniversalTime();");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }

                writer.WriteLine("\t}");
            }
        }
    }
}
