using System.IO;
using JetCode.Factory;
using JetCode.BizSchema;

namespace JetCode.FactoryWinUI
{
    public class FactoryFormSelectDesign : FactoryBase
    {
        public FactoryFormSelectDesign(MappingSchema mappingSchema, ObjectSchema objectSchema)
            : base(mappingSchema, objectSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Manager.FormSelect", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic partial class FormSelect{0}", base.ObjectSchema.Alias);
            writer.WriteLine("\t{");

            writer.WriteLine("\t\t/// <summary>");
            writer.WriteLine("\t\t/// Required designer variable.");
            writer.WriteLine("\t\t/// </summary>");
            writer.WriteLine("\t\tprivate System.ComponentModel.IContainer components = null;");
            writer.WriteLine();

            writer.WriteLine("\t\t/// <summary>");
            writer.WriteLine("\t\t/// Clean up any resources being used.");
            writer.WriteLine("\t\t/// </summary>");
            writer.WriteLine("\t\t/// <param name=\"disposing\">true if managed resources should be disposed; otherwise, false.</param>");
            writer.WriteLine("\t\tprotected override void Dispose(bool disposing)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (disposing && (components != null))");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tcomponents.Dispose();");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tbase.Dispose(disposing);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        protected override void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        protected override void WriteContent(StringWriter writer)
        {
            this.WriteInitializeComponent(writer);
        }

        private void WriteInitializeComponent(StringWriter writer)
        {
            writer.WriteLine("\t\t#region Windows Form Designer generated code");
            writer.WriteLine();
            writer.WriteLine("\t\t/// <summary>");
            writer.WriteLine("\t\t/// Required method for Designer support - do not modify");
            writer.WriteLine("\t\t/// the contents of this method with the code editor.");
            writer.WriteLine("\t\t/// </summary>");
            writer.WriteLine("\t\tprivate void InitializeComponent()");
            writer.WriteLine("\t\t{");

            this.WriteDetail(writer);

            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();
        }

        private void WriteDetail(StringWriter writer)
        {
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.pnlSearch)).BeginInit();");
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.pnlButtons)).BeginInit();");
            writer.WriteLine("\t\t\tthis.SuspendLayout();");

            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\t//pnlSearch");
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\tthis.pnlSearch.TabIndex = 0;");
            writer.WriteLine("\t\t\tthis.pnlSearch.Visible = false;");
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\t//pnlButtons");
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\tthis.pnlButtons.TabIndex = 0;");
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\t// FormSelect{0}", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t//");
            writer.WriteLine("\t\t\tthis.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);");
            writer.WriteLine("\t\t\tthis.Name = \"FormSelect{0}\";", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\tthis.Text = \"Select {0}\";", base.ObjectSchema.Alias);
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.pnlSearch)).EndInit();");
            writer.WriteLine("\t\t\t((System.ComponentModel.ISupportInitialize)(this.pnlButtons)).EndInit();");
            writer.WriteLine("\t\t\tthis.ResumeLayout(false);");

        }
    }
}
