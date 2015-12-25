using System;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
    public class FactoryRules : FactoryBase
    {
        public FactoryRules(MappingSchema mappingSchema, ObjectSchema objectSchema)
            : base(mappingSchema, objectSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using {0}.Schema;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Data", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            writer.WriteLine("\tpublic partial class {0}Data", base.ObjectSchema.Alias);
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tprotected override void CheckRules()");
            writer.WriteLine("\t\t{");

            foreach (FieldSchema field in base.ObjectSchema.Fields)
            {
                if(field.IsJoined)
                    continue;

                Type dotnetType = base.Utilities.ToDotNetType(field.DataType);
                if(dotnetType == typeof(Guid))
                {
                    if(!field.IsPK && !field.IsNullable)
                    {
                        writer.WriteLine("\t\t\tbase.BrokenRules.Assert({1}Schema.{0}, \"[{0}] can not be empty.\", this.{0} == Guid.Empty);", field.Alias, base.ObjectSchema.Alias);
                    }

                    continue;
                }

                if (field.DataType.ToLower() == "varchar" || field.DataType.ToLower() == "nvarchar"
                    || field.DataType.ToLower() == "text" || field.DataType.ToLower() == "ntext")
                {
                    //writer.WriteLine("\t\t\tbase.BrokenRules.Assert({2}Schema.{0}, \"The length of [{0}] is over {1}.\", this.{0}.Length > {1});", field.Alias, field.Size, base.ObjectSchema.Alias);

                    if(!field.IsNullable)
                    {
                        writer.WriteLine("\t\t\tbase.BrokenRules.Assert({1}Schema.{0}, \"[{0}] can not be empty.\", this.{0}.Length == 0);", field.Alias, base.ObjectSchema.Alias);
                    }

                    continue;
                }

                //if (field.DataType.ToLower() == "char" || field.DataType.ToLower() == "nchar")
                //{
                //    writer.WriteLine("\t\t\tbase.BrokenRules.Assert({2}Schema.{0}, \"The length of [{0}] is not equal to {1}.\",  this.{0}.Length > 0 && this.{0}.Length != {1});", field.Alias, field.Size base.ObjectSchema.Alias);

                //    if (!field.IsNullable)
                //    {
                //        writer.WriteLine("\t\t\tbase.BrokenRules.Assert({1}Schema.{0}, \"[{0}] can not be empty.\", this.{0}.Length == 0);", field.Alias base.ObjectSchema.Alias);
                //    }

                //    continue;
                //}
            }

            writer.WriteLine("\t\t}");
            writer.WriteLine("\t}");
        }
    }
}
