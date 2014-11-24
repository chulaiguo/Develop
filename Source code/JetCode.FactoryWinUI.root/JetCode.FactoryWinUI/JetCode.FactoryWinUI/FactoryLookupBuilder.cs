using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
    public class FactoryLookupBuilder : FactoryBase
    {
        public FactoryLookupBuilder(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected  override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using DevExpress.XtraEditors.Controls;");
            writer.WriteLine("using DevExpress.XtraEditors.Repository;");
            writer.WriteLine("using {0}.Schema;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Manager.Utilities", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tinternal static class LookUpEditBuilder");
            writer.WriteLine("\t{");
        }

        protected override void WriteContent(StringWriter writer)
        {
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                writer.WriteLine("\t\tinternal static void Setup{0}(RepositoryItemLookUpEdit lookUpEdit, object dataSource, string valueMember)", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tif (lookUpEdit.Columns.Count == 0)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tLookUpColumnInfo col = new LookUpColumnInfo();");
                writer.WriteLine("\t\t\t\tcol.FieldName = \"XXX\";//{0}Schema.XXX;", item.Alias);
                writer.WriteLine("\t\t\t\tcol.Caption = \"{0}\";", item.Alias);
                writer.WriteLine("\t\t\t\tlookUpEdit.Columns.Add(col);");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();

                writer.WriteLine("\t\t\tlookUpEdit.DropDownRows = 10;");
                writer.WriteLine("\t\t\tlookUpEdit.PopupWidth = 100;");
                writer.WriteLine();

                writer.WriteLine("\t\t\tlookUpEdit.ValueMember = valueMember;");
                writer.WriteLine("\t\t\tlookUpEdit.DisplayMember = \"XXX\";//{0}Schema.XXX;", item.Alias);
                writer.WriteLine();

                writer.WriteLine("\t\t\tlookUpEdit.DataSource = dataSource;");
                writer.WriteLine("\t\t\tlookUpEdit.BestFit();");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
    }
}
