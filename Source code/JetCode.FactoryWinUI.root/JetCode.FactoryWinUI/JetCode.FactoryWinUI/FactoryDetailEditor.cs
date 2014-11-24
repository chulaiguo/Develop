using System;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
    public class FactoryDetailEditor : FactoryBase
    {
        public FactoryDetailEditor(MappingSchema mappingSchema, ObjectSchema objectSchema)
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
            writer.WriteLine("namespace {0}.Manager.FormDetailEditor", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic partial class FormDetail{0} : FormDetailEditorBase", base.ObjectSchema.Alias);
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
            this.WriteParents(writer);
        }

        private void WriteConstruct(StringWriter writer)
        {
            writer.WriteLine("\t\tpublic FormDetail{0}()", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tInitializeComponent();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic FormDetail{0}(string userid, {0} entity)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t: base(userid, entity)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tInitializeComponent();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate {0} {0}", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget{{ return base.Entity as {0}; }}", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteDataBinding(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void DataBindingEntity()");
            writer.WriteLine("\t\t{");

            FieldSchemaCollection list = base.GetBindableFields();
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

            writer.WriteLine();
            foreach (ParentSchema item in base.ObjectSchema.Parents)
            {
                writer.WriteLine("\t\t\t//this.txt{0}.Text = this.{1}.XXX;", item.Alias, base.ObjectSchema.Alias);
            }

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteParents(StringWriter writer)
        {
            foreach (ParentSchema item in base.ObjectSchema.Parents)
            {
                writer.WriteLine("\t\tprivate void txt{0}_ButtonClick(object sender, ButtonPressedEventArgs e)", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\t//FormSelect{0} dlg = new FormSelect{0}(base.UserId);", item.Alias);
                writer.WriteLine("\t\t\t//if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)");
                writer.WriteLine("\t\t\t\t//return;");
                writer.WriteLine();
                writer.WriteLine("\t\t\t//this.{0}.{1} = dlg.{1};", base.ObjectSchema.Alias, item.Alias);
                writer.WriteLine("\t\t\t//this.txt{0}.Text = dlg.{0}.XXX;", item.Alias);
                writer.WriteLine("\t\t}");
            }
        }
    }
}
