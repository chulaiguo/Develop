using System;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
    public class FactoryDBRule : FactoryBase
    {
        public FactoryDBRule(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using Cheke.BusinessEntity;");

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
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                writer.WriteLine("\tpublic partial class {0}Data", item.Alias);
                writer.WriteLine("\t{");

                foreach (FieldSchema field in item.Fields)
                {
                    if (field.IsJoined)
                        continue;

                    if (field.DataType.ToLower() == "uniqueidentifier")
                    {
                        if (!field.IsNullable)
                        {
                            writer.WriteLine(
                                "\t\tprivate static readonly Rule _Rule_{0} = new Rule(\"{0}\", \"It can't be empty.\");",
                                field.Alias);
                        }

                        continue;
                    }

                    if (field.DataType.ToLower() == "varchar" || field.DataType.ToLower() == "nvarchar")
                    {

                        if (!field.IsNullable)
                        {
                            writer.WriteLine(
                                "\t\tprivate static readonly Rule _Rule_{0} = new Rule(\"{0}\", \"It can't be empty and its max length is {1}.\");",
                                field.Alias, field.Size);
                        }
                        else
                        {
                            writer.WriteLine(
                                "\t\tprivate static readonly Rule _Rule_{0} = new Rule(\"{0}\", \"The max length is {1}.\");", field.Alias,
                                field.Size);
                        }

                        continue;
                    }

                    if (field.DataType.ToLower() == "char" || field.DataType.ToLower() == "nchar")
                    {
                        if (!field.IsNullable)
                        {
                            writer.WriteLine(
                                "\t\tprivate static readonly Rule _Rule_{0} = new Rule(\"{0}\", \"It can't be empty and its length is equal to {1}.\");",
                                field.Alias, field.Size);
                        }
                        else
                        {
                            writer.WriteLine(
                                "\t\tprivate static readonly Rule _Rule_{0} = new Rule(\"{0}\", \"The length is equal to {1}.\");",
                                field.Alias, field.Size);
                        }

                        continue;
                    }
                }

                writer.WriteLine();
                this.WriteCheckDBRule(writer, item);

                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }

        protected void WriteCheckDBRule(StringWriter writer, ObjectSchema item)
        {
            writer.WriteLine("\t\tprotected override void CheckDBRules()");
            writer.WriteLine("\t\t{");
            foreach (FieldSchema field in item.Fields)
            {
                if (field.IsJoined)
                    continue;

                if (field.DataType.ToLower() == "uniqueidentifier")
                {
                    if (!field.IsNullable)
                    {
                        writer.WriteLine("\t\t\tbase.BrokenRules.Assert(_Rule_{0}, this._{1} == Guid.Empty);",
                            field.Alias, base.LowerFirstLetter(field.Alias));
                    }

                    continue;
                }

                if (field.DataType.ToLower() == "varchar" || field.DataType.ToLower() == "nvarchar")
                {
                    if (!field.IsNullable)
                    {
                        writer.WriteLine("\t\t\tbase.BrokenRules.Assert(_Rule_{0}, this._{1}.Length > {2} || this._{1}.Length == 0);",
                            field.Alias, base.LowerFirstLetter(field.Alias), field.Size);
                    }
                    else
                    {
                        writer.WriteLine("\t\t\tbase.BrokenRules.Assert(_Rule_{0}, this._{1}.Length > {2});",
                            field.Alias, base.LowerFirstLetter(field.Alias), field.Size);
                    }

                    continue;
                }

                if (field.DataType.ToLower() == "char" || field.DataType.ToLower() == "nchar")
                {
                    if (!field.IsNullable)
                    {
                        writer.WriteLine("\t\t\tbase.BrokenRules.Assert(_Rule_{0}, this._{1}.Length != {2});",
                            field.Alias, base.LowerFirstLetter(field.Alias), field.Size);
                    }
                    else
                    {
                        writer.WriteLine("\t\t\tbase.BrokenRules.Assert(_Rule_{0}, this._{1}.Length > 0 &&  this._{1}.Length != {2});",
                            field.Alias, base.LowerFirstLetter(field.Alias), field.Size);
                    }

                    continue;
                }
            }
            writer.WriteLine("\t\t}");
        }
    }
}
