using System;
using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryData
{
    public class FactoryDecimalRound : FactoryBase
    {
        public FactoryDecimalRound(MappingSchema mappingSchema)
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
                List<FieldSchema> fieldList = this.GetDecimalFields(item);
                if(fieldList.Count == 0)
                    continue;

                writer.WriteLine("\tpublic partial class {0}", item.Alias);
                writer.WriteLine("\t{");

                foreach (FieldSchema field in fieldList)
                {
                    writer.WriteLine("\t\tpublic override decimal {0}", field.Alias);
                    writer.WriteLine("\t\t{");

                    string dataType = field.DataType.ToLower();
                    string dataSize = field.Size.ToLower();
                    writer.WriteLine("\t\t\tget{{ return base.{0}; }}", field.Alias);

                    if (dataType == "smallmoney" || dataType == "money")
                    {
                        writer.WriteLine("\t\t\tset{{ base.{0} = Math.Round(value, 2); }}", field.Alias);
                    }
                    else if (dataType.StartsWith("decimal"))
                    {
                        int scale;
                        if (int.TryParse(dataSize.Substring(dataSize.Length - 2, 1), out scale))
                        {
                            writer.WriteLine("\t\t\tset{{ base.{0} = Math.Round(value, {1}); }}", field.Alias, scale);
                        }
                        else
                        {
                            writer.WriteLine("\t\t\tset{{ base.{0} = value; }}", field.Alias);
                        }
                    }
                    else
                    {
                        writer.WriteLine("\t\t\tset{{ base.{0} = value; }}", field.Alias);
                    }
                    writer.WriteLine("\t\t}");
                }

                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }

        private List<FieldSchema> GetDecimalFields(ObjectSchema objSchema)
        {
            List<FieldSchema> retList = new List<FieldSchema>();
            foreach (FieldSchema field in objSchema.Fields)
            {
                Type type = base.Utilities.ToDotNetType(field.DataType);
                if(type == typeof(float) || type == typeof(double) || type == typeof(decimal))
                {
                    retList.Add(field);
                }
            }

            return retList;
        }
    }
}
