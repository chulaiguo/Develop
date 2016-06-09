using System;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
    public class FactoryDetailMapEditor : FactoryBase
    {
        public FactoryDetailMapEditor(MappingSchema mappingSchema, ObjectSchema objectSchema)
            : base(mappingSchema, objectSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using Cheke.WinCtrl;");
            writer.WriteLine("using {0}.ViewObj;", base.ProjectName);
            writer.WriteLine("using {0}.Schema;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Manager.FormDetailMap", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic partial class FormDetail{0} : FormDetailMapBase", base.ObjectSchema.Alias);
            writer.WriteLine("\t{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            this.WriteConstruct(writer);
            this.WriteDataBinding(writer);
        }

        private void WriteConstruct(StringWriter writer)
        {
            writer.WriteLine("\t\tpublic FormDetail{0}()", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tInitializeComponent();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic FormDetail{0}(string userid, {0} entity, bool editable)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t: base(userid, entity, editable)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tInitializeComponent();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0} {0}", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget{{ return base.Entity as {0}; }}", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteDataBinding(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void DataBinding()");
            writer.WriteLine("\t\t{");

            FieldSchemaCollection list = base.GetBindableFields();
            foreach (ParentSchema item in base.ObjectSchema.Parents)
            {
                FieldSchemaCollection children = base.GetReadableUKFields(item.Name);
                foreach (FieldSchema child in children)
                {
                    if (list.Find(child.Name) == null)
                    {
                        child.IsJoined = true;
                        list.Add(child);
                    }
                }
            }
            foreach (FieldSchema item in list)
            {
                Type dotnetType = base.Utilities.ToDotNetType(item.DataType);
                if (dotnetType == typeof (bool))
                {
                    writer.WriteLine("\t\t\tthis.chk{0}.BindingData(this.{1}, {1}Schema.{0});", item.Alias,
                                     base.ObjectSchema.Alias);
                }
                else if (dotnetType == typeof (DateTime))
                {
                    writer.WriteLine("\t\t\tthis.date{0}.BindingData(this.{1}, {1}Schema.{0});", item.Alias,
                                     base.ObjectSchema.Alias);
                }
                else
                {
                    writer.WriteLine("\t\t\tthis.txt{0}.BindingData(this.{1}, {1}Schema.{0});", item.Alias,
                                     base.ObjectSchema.Alias);
                }
            }            

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }
    }
}
