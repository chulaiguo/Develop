using System;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;
using System.Text;

namespace JetCode.FactoryWinUI
{
    public class FactoryFixDecimal : FactoryBase
    {
        public FactoryFixDecimal(MappingSchema mappingSchema)
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
            foreach (ObjectSchema obj in base.MappingSchema.Objects)
            {
                writer.WriteLine("\tpublic partial class {0}", obj.Alias);
                writer.WriteLine("\t{");

                foreach (FieldSchema field in obj.Fields)
                {
                    if(field.IsJoined)
                        continue;

                    Type dotnetType = base.Utilities.ToDotNetType(field.DataType);
                    if(dotnetType != typeof(decimal))
                        continue;

                    string[] splits = field.Size.Split(',');
                    if(splits.Length == 0)
                        continue;

                    byte precision;
                    if(!byte.TryParse(splits[1].TrimEnd(')'), out precision))
                        continue;

                    if(precision < 1)
                        continue;

                    StringBuilder precisionBuilder = new StringBuilder();
                    precisionBuilder.Append("0.");
                    for (int i = 0; i < precision - 1; i++)
                    {
                        precisionBuilder.Append("0");
                    }
                    precisionBuilder.Append("1M");

                    writer.WriteLine("\t\tpublic override decimal {0}", field.Alias);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tget {{ return base.{0}; }}", field.Alias);
                    writer.WriteLine("\t\t\tset");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tif(Math.Abs(base.{0} - value) >= {1})", field.Alias, precisionBuilder);
                    writer.WriteLine("\t\t\t\t{");
                    writer.WriteLine("\t\t\t\t\tbase.{0} = value;", field.Alias);
                    writer.WriteLine("\t\t\t\t}");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine("\t\t}");
                }

                writer.WriteLine("\t}");
            }
        }
    }
}
