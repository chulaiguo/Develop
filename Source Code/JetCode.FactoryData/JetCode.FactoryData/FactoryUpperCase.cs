using System;
using System.Collections.Generic;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryData
{
    public class FactoryUpperCase : FactoryBase
    {
        public FactoryUpperCase(MappingSchema mappingSchema)
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
                List<FieldSchema> fieldList = this.GetStringFields(item);
                if(fieldList.Count == 0)
                    continue;

                writer.WriteLine("\tpublic partial class {0}", item.Alias);
                writer.WriteLine("\t{");

                foreach (FieldSchema field in fieldList)
                {
                    writer.WriteLine("\t\tpublic override string {0}", field.Alias);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tget{{ return base.{0}.ToUpper(); }}", field.Alias);
                    //writer.WriteLine("\t\t\tset{{ base.{0} = value; }}", field.Alias);
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }

                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }

        private List<FieldSchema> GetStringFields(ObjectSchema objSchema)
        {
            List<FieldSchema> retList = new List<FieldSchema>();
            foreach (FieldSchema field in objSchema.Fields)
            {
                Type type = base.Utilities.ToDotNetType(field.DataType);
                if(type == typeof(string))
                {
                    retList.Add(field);
                }              
            }

            return retList;
        }
    }
}
