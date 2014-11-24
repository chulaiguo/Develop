using System.IO;
using System.Text;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryDataService
{
    public class FactoryInactiveFilter : FactoryBase
    {
        public FactoryInactiveFilter(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using {0}.Data;", base.ProjectName);
            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.DataService", base.ProjectName);
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
                FieldSchema field = this.GetLogicDeleteField(item);
                if(field == null)
                    continue;

                writer.WriteLine("\tpublic partial class {0}DataService", item.Alias);
                writer.WriteLine("\t{");

                writer.WriteLine("\t\tprotected override bool Selectable(BusinessBase entity)");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\t{0}Data data = entity as {0}Data;", item.Alias);
                writer.WriteLine("\t\t\tif(data == null)");
                writer.WriteLine("\t\t\t\treturn false;");
                writer.WriteLine();
                writer.WriteLine("\t\t\tif(data.{0} && !this.SecurityToken.IncludeInactive)", field.Alias);
                writer.WriteLine("\t\t\t\treturn false;");
                writer.WriteLine();
                writer.WriteLine("\t\t\treturn true;");
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                writer.WriteLine("\t\tprotected override void BeforeSave(BusinessBase entity)");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\t{0}Data data = entity as {0}Data;", item.Alias);
                writer.WriteLine("\t\t\tif(data == null)");
                writer.WriteLine("\t\t\t\treturn;");
                writer.WriteLine();
                writer.WriteLine("\t\t\tif (!data.IsNew)");
                writer.WriteLine("\t\t\t\treturn;");
                writer.WriteLine();
                writer.WriteLine("\t\t\t{0}Data old = this._dal.GetByPK({1});", item.Alias, this.GetPKInputParameters(item));
                writer.WriteLine("\t\t\tif(old == null)");
                writer.WriteLine("\t\t\t\treturn;");
                writer.WriteLine();
                writer.WriteLine("\t\t\tdata.RowVersion = old.RowVersion;");
                writer.WriteLine("\t\t\tdata.AcceptSelfChanges();");
                writer.WriteLine("\t\t\tdata.MarkDirty();");
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }

        protected string GetPKInputParameters(ObjectSchema objSchema)
        {
            StringBuilder builder = new StringBuilder();
            foreach (FieldSchema item in objSchema.Fields)
            {
                if (!item.IsPK)
                    continue;

                builder.AppendFormat(" data.{0},", item.Name);
            }

            return builder.ToString().TrimEnd(',');
        }
    }
}