using System;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
    public class FactoryDetailEditor2 : FactoryBase
    {
        public FactoryDetailEditor2(MappingSchema mappingSchema, ObjectSchema objectSchema)
            : base(mappingSchema, objectSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Windows.Forms;");
            writer.WriteLine("using Cheke.WinCtrl;");
            writer.WriteLine("using DevExpress.XtraEditors;");
            writer.WriteLine("using DevExpress.XtraEditors.Controls;");
            writer.WriteLine("using {0}.ViewObj;", base.ProjectName);
            writer.WriteLine("using {0}.Schema;", base.ProjectName);
            writer.WriteLine("using {0}.Manager.FormSelect;", base.ProjectName);
            writer.WriteLine("using {0}.Manager.GridDecorator;", base.ProjectName);

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
            this.WriteChildren(writer);
        }

        private void WriteConstruct(StringWriter writer)
        {
            foreach (ChildSchema item in this.ObjectSchema.Children)
            {
                writer.WriteLine("\t\tprivate Grid{0}Decorator _decorate{0} = null;", item.Alias);
            }
            writer.WriteLine();

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

            writer.WriteLine("\t\tprotected override void InitializeDecorator()");
            writer.WriteLine("\t\t{");
            foreach (ChildSchema item in this.ObjectSchema.Children)
            {
                writer.WriteLine("\t\t\tthis._decorate{0} = new Grid{0}Decorator(base.UserId, this.grd{0});", item.Alias);
                writer.WriteLine("\t\t\tthis._decorate{0}.Initialize();", item.Alias);
                writer.WriteLine();
            }
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
                string uk = this.GetUKField(item.Name);
                if(uk.Length == 0)
                {
                    writer.WriteLine("\t\t\t//this.txt{0}.Text = this.{1}.XXX;", item.Alias, base.ObjectSchema.Alias);
                }
                else
                {
                    writer.WriteLine("\t\t\tthis.txt{0}.Text = this.{1}.{2}.ToString();", item.Alias, base.ObjectSchema.Alias, uk);
                }
            }

            writer.WriteLine("\t\t}");
            writer.WriteLine();
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

        private void WriteChildren(StringWriter writer)
        {
            writer.WriteLine("\t\tprivate void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (this.xtraTabControl1.SelectedTabPage == this.tabDetail)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.ShowNewButton = true;");
            writer.WriteLine("\t\t\t\tthis.ShowDeleteButton = true;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\telse");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.ShowNewButton = false;");
            writer.WriteLine("\t\t\t\tthis.ShowDeleteButton = false;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tthis.xtraTabControl1.SelectedTabPage.Focus();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            foreach (ChildSchema item in this.ObjectSchema.Children)
            {
                writer.WriteLine("\t\tprivate void tab{0}_Enter(object sender, System.EventArgs e)", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tif (this.tab{0}.Tag == null)", item.Alias);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tif (this.{0}.IsNew)", base.ObjectSchema.Alias);
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tthis._decorate{0}.DataSource = null;", item.Alias);
                writer.WriteLine("\t\t\t\t\treturn;");
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t\t\tthis.tab{0}.Tag = true;", item.Alias);
                writer.WriteLine();
                writer.WriteLine("\t\t\t\tif (this.{0}.{1}List == null || base.IsRefreshData)", base.ObjectSchema.Alias, item.Alias);
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t this.{0}.{1}List = {1}.GetBy{0}(this.{0}.{0}PK);", base.ObjectSchema.Alias, item.Alias);
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t\t\tthis._decorate{0}.{1} = this.{1};", item.Alias, base.ObjectSchema.Alias);
                writer.WriteLine("\t\t\t\tthis._decorate{0}.DataSource = this.{1}.{0}List;", item.Alias, base.ObjectSchema.Alias);
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

        }
    }
}
