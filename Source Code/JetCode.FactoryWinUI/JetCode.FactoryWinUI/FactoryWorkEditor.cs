using System;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
    public class FactoryWorkEditor : FactoryBase
    {
        public FactoryWorkEditor(MappingSchema mappingSchema, ObjectSchema objectSchema)
            : base(mappingSchema, objectSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Windows.Forms;");
            writer.WriteLine("using DevExpress.XtraTab;");
            writer.WriteLine("using Cheke.WinCtrl;");
            writer.WriteLine("using {0}.ViewObj;", base.ProjectName);
            writer.WriteLine("using {0}.Schema;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Manager.FormWorkEditor", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic partial class FormWork{0} : FormWorkEditorBase", base.ObjectSchema.Alias);
            writer.WriteLine("\t{");
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            this.WriteLookupData(writer);
            this.WriteConstruct(writer);
            this.WriteOverride(writer);
            this.WriteEvents(writer);
            this.WriteDataBinding(writer);
            this.WriteParents(writer);
        }

        private void WriteLookupData(StringWriter writer)
        {
            foreach (ParentSchema item in base.ObjectSchema.Parents)
            {
                writer.WriteLine("\t\tprivate {0}Collection _{1}List = null;", item.Alias, base.LowerFirstLetter(item.Alias));
            }
            writer.WriteLine();
        }

        private void WriteConstruct(StringWriter writer)
        {
            writer.WriteLine("\t\tpublic FormWork{0}()", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tInitializeComponent();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic FormWork{0}(string userId, Control parent, {0} entity)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t: base(userId, parent, entity)");
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

        private void WriteOverride(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void UpdateUI(bool isDirty)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase.UpdateUI(isDirty);");
            writer.WriteLine();
            writer.WriteLine("\t\t\tif (this.{0}.IsNew)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t{");
            string special = "{0}";
            writer.WriteLine("\t\t\t\tthis.Caption = string.Format(\"New {0} Created At {1}\", DateTime.Now.ToLongTimeString());", base.ObjectSchema.Alias, special);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\telse");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.Caption = \"{0}\";//{0}Schema.XXX", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteEvents(StringWriter writer)
        {
            writer.WriteLine("\t\tprivate void xtraTabControl1_SelectedPageChanged(object sender, TabPageChangedEventArgs e)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (e.Page == this.tab{0})", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.ShowDeleteButton = true;");
            writer.WriteLine("\t\t\t\tthis.ShowNewButton = true;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\telse");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.ShowDeleteButton = false;");
            writer.WriteLine("\t\t\t\tthis.ShowNewButton = false;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tthis.DataBinding();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteDataBinding(StringWriter writer)
        {
            writer.WriteLine("\t\tprivate void tab{0}_Enter(object sender, EventArgs e)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (this.tab{0}.Tag == null)", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.tab{0}.Tag = true;", base.ObjectSchema.Alias);
            writer.WriteLine();
            FieldSchemaCollection list = base.GetBindableFields();
            foreach (FieldSchema item in list)
            {
                Type dotnetType = base.Utilities.ToDotNetType(item.DataType);
                if (dotnetType == typeof(bool))
                {
                    writer.WriteLine("\t\t\t\tthis.chk{0}.BindingData(this.{1}, {1}Schema.{0});", item.Alias, base.ObjectSchema.Alias);
                }
                else if (dotnetType == typeof(DateTime))
                {
                    writer.WriteLine("\t\t\t\tthis.date{0}.BindingData(this.{1}, {1}Schema.{0});", item.Alias, base.ObjectSchema.Alias);
                }
                else
                {
                    writer.WriteLine("\t\t\t\tthis.txt{0}.BindingData(this.{1}, {1}Schema.{0});", item.Alias, base.ObjectSchema.Alias);
                }
            }

            writer.WriteLine();
            foreach (ParentSchema item in base.ObjectSchema.Parents)
            {
                string uk = this.GetUKField(item.Name);
                if (uk.Length == 0)
                {
                    writer.WriteLine("\t\t\t//this.txt{0}.Text = this.{1}.XXX;", item.Alias, base.ObjectSchema.Alias);
                }
                else
                {
                    writer.WriteLine("\t\t\tthis.txt{0}.Text = this.{1}.{2}.ToString();", item.Alias, base.ObjectSchema.Alias, uk);
                }
            }
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteParents(StringWriter writer)
        {
            foreach (ParentSchema item in base.ObjectSchema.Parents)
            {
                writer.WriteLine("\t\tprivate void txt{0}_ButtonClick(object sender, ButtonPressedEventArgs e)", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tif (e.Button.Kind == ButtonPredefines.Delete)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tif (this.{0}.{1}PK == Guid.Empty)", base.ObjectSchema.Alias, item.Alias);
                writer.WriteLine("\t\t\t\t\treturn;");
                writer.WriteLine();
                writer.WriteLine("\t\t\t\tif (DialogResult.Yes != base.ShowQuestion(string.Format(\"Are you sure you want to clear the {{0}}?\", this.txt{0}.Title)))", item.Alias);
                writer.WriteLine("\t\t\t\t\treturn;");
                writer.WriteLine();
                writer.WriteLine("\t\t\t\tthis.{0}.{1}PK = Guid.Empty;", base.ObjectSchema.Alias, item.Alias);
                writer.WriteLine("\t\t\t\tthis.txt{0}.Text = string.Empty;", item.Alias);
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\telse");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tFormSelect{0} dlg = new FormSelect{0}(base.UserId);", item.Alias);
                writer.WriteLine("\t\t\t\tdlg.MultiSelect = false;");
                writer.WriteLine("\t\t\t\tif (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)");
                writer.WriteLine("\t\t\t\t\treturn;");
                writer.WriteLine();
                writer.WriteLine("\t\t\t\tif (this.{0}.{1}PK == dlg.{1}.{1}PK)", base.ObjectSchema.Alias, item.Alias);
                writer.WriteLine("\t\t\t\t\treturn;");
                writer.WriteLine();

                writer.WriteLine("\t\t\t\tthis.{0}.{1} = dlg.{1};", base.ObjectSchema.Alias, item.Alias);
                string uk = this.GetUKField(item.Name);
                if (uk.Length > 0)
                {
                    writer.WriteLine("\t\t\t\tthis.txt{0}.Text = dlg.{0}.{1}.ToString();", item.Alias, uk);

                }
                else
                {
                    writer.WriteLine("\t\t\t\t//this.txt{0}.Text = dlg.{0}.XXX;", item.Alias);
                }
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");

                writer.WriteLine();
            }
        }

        private string GetUKField(string name)
        {
            FieldSchemaCollection list = base.GetUKFields(name);
            if (list.Count == 0)
                return string.Empty;

            foreach (FieldSchema item in list)
            {
                Type dotnetType = base.Utilities.ToDotNetType(item.DataType);
                if (dotnetType == typeof(string))
                    return item.Alias;
            }

            return list[0].Alias;
        }
    }
}
