using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;
using System;

namespace JetCode.FactoryTest
{
    public class FactoryItem : FactoryBase
    {
        public FactoryItem(MappingSchema mappingSchema, ObjectSchema item)
            : base(mappingSchema, item)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.ComponentModel.DataAnnotations;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("using {0}.ViewObj;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.WebExpress.Models", base.ProjectName);
            writer.WriteLine("{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            writer.WriteLine("\tpublic class {0}Mode", base.ObjectSchema.Alias);
            writer.WriteLine("\t{");

            //Properties
            foreach (FieldSchema field in base.ObjectSchema.Fields)
            {
                if (!this.IsValidField(field))
                    continue;

                if (field.IsPK)
                {
                    writer.WriteLine("\t\t[Key]");
                }

                Type fieldType = base.Utilities.ToDotNetType(field.DataType);
                if (fieldType != typeof(Guid))
                {
                    if (!field.IsNullable)
                    {
                        writer.WriteLine("\t\t[Required]");
                    }

                    if (fieldType == typeof (string))
                    {
                        if (field.DataType == "char" || field.DataType == "nchar")
                        {
                            writer.WriteLine(
                                "\t\t[StringLength({0}, ErrorMessage = \"The {{0}} must be at least {{2}} characters long.\", MinimumLength = {0})]",
                                field.Size);
                        }
                        else
                        {
                            writer.WriteLine("\t\t[StringLength({0})]", field.Size);
                        }

                        int size;
                        if (int.TryParse(field.Size, out size))
                        {
                            if (size >= 128)
                            {
                                writer.WriteLine("\t\t[DataType(DataType.MultilineText)]");
                            }
                        }
                    }

                    writer.WriteLine("\t\t[Display(Name = \"{0}\")]", field.Alias);
                }

                writer.WriteLine("\t\tpublic {0} {1} {{ get; set; }}", fieldType.Name, field.Alias);
                writer.WriteLine();
            }

            //Methods
            writer.WriteLine("\t\tpublic static {0}Mode FromData({0} data)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}Mode entity = new {0}Mode();", base.ObjectSchema.Alias);
            foreach (FieldSchema field in base.ObjectSchema.Fields)
            {
                if (!this.IsValidField(field))
                    continue;

                writer.WriteLine("\t\t\tentity.{0} = data.{0};", field.Alias);
            }
            writer.WriteLine("\t\t\treturn entity;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic static List<{0}Mode> FromDataList({0}Collection dataList)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tList<{0}Mode> retList = new List<{0}Mode>();", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tforeach({0} item in dataList)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tretList.Add(FromData(item));");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\treturn retList;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic void EditData({0} data)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            foreach (FieldSchema field in base.ObjectSchema.Fields)
            {
                if (!this.IsValidField(field))
                    continue;

                if (field.IsPK)
                    continue;

                writer.WriteLine("\t\t\tdata.{0} = this.{0};", field.Alias);
            }
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t}");
            writer.WriteLine();
        }

        private bool IsValidField(FieldSchema field)
        {
            //if (field.IsPK)
            //    return false;

            if (field.Alias == "CreatedOn" || field.Alias == "CreatedBy"
                || field.Alias == "ModifiedOn" || field.Alias == "ModifiedBy"
                || field.Alias == "RowVersion")
                return false;

            return true;
        }
    }
}
