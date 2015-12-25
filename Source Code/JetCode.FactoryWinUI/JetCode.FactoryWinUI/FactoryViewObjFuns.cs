using System;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
        public class FactoryViewObjFuns : FactoryBase
    {
        public FactoryViewObjFuns(MappingSchema mappingSchema)
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

                writer.WriteLine("\t\tpublic void AcceptSelfChanges()");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tif (this.IsSelfDirty)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tbase.MarkOld();");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                if (!item.IsMultiPK)
                {
                    writer.WriteLine("\t\tpublic static {0} FindItemByPK({0}Collection list, Guid {1}PK)", item.Alias, base.LowerFirstLetter(item.Alias));
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tforeach ({0} item in list)", item.Alias);
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tif (item.{0}PK == {1}PK)", item.Alias, base.LowerFirstLetter(item.Alias));
                    writer.WriteLine("\t\t\t\t\treturn item;");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine();
                    writer.WriteLine("\t\t\treturn null;");
                    writer.WriteLine("\t\t}");
                }

                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }
    }
}
