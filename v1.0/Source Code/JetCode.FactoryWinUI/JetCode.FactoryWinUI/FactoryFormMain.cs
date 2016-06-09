using System;
using System.IO;
using JetCode.BizSchema;
using JetCode.Factory;

namespace JetCode.FactoryWinUI
{
    public class FactoryFormMain : FactoryBase
    {
        public FactoryFormMain(MappingSchema mappingSchema)
            : base(mappingSchema)
        {
        }

        protected override void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Windows.Forms;");
            writer.WriteLine("using Cheke.WinCtrl;");
            writer.WriteLine("using Cheke.WinCtrl.Login;");
            writer.WriteLine("using DevExpress.XtraBars;");
            writer.WriteLine("using DevExpress.XtraNavBar;");
            writer.WriteLine("using {0}.Manager.FormWorkList;", base.ProjectName);

            writer.WriteLine();
        }

        protected override void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Manager", base.ProjectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic partial class FormMain : FormMainBase");
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
            this.WriteDefaultMenu(writer);
            this.WriteListLinks(writer);
        }

        private void WriteConstruct(StringWriter writer)
        {
            writer.WriteLine("\t\tprivate static FormMain _Instance = null;");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic FormMain()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tInitializeComponent();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic FormMain(string userid)");
            writer.WriteLine("\t\t\t: base(userid)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tInitializeComponent();");
            writer.WriteLine("\t\t\t_Instance = this;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected override Control WorkArea");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return this.splitContainerMain.Panel2; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tinternal static FormMain Instance");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget { return _Instance; }");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteDefaultMenu(StringWriter writer)
        {
            writer.WriteLine("\t\t#region menu Windows");

            writer.WriteLine("\t\tprivate void menuWindowsList_GetItemData(object sender, EventArgs e)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis.menuWindowsList.Strings.Clear();");
            writer.WriteLine();
            writer.WriteLine("\t\t\tforeach (Form item in Application.OpenForms)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif (item.Parent == this.WorkArea)");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\tthis.menuWindowsList.Strings.Add(item.Text);");
            writer.WriteLine("\t\t\t\t\tif (this.WorkArea.Controls.GetChildIndex(item) == 0)");
            writer.WriteLine("\t\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t\tthis.menuWindowsList.ItemIndex = this.menuWindowsList.Strings.Count - 1;");
            writer.WriteLine("\t\t\t\t\t}");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");

            writer.WriteLine("\t\tprivate void menuWindowsList_ListItemClick(object sender, ListItemClickEventArgs e)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tforeach (Form item in Application.OpenForms)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif (item.Text == this.menuWindowsList.Strings[e.Index])");
            writer.WriteLine("\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\titem.BringToFront();");
            writer.WriteLine("\t\t\t\t}");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");

            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();

            writer.WriteLine("\t\t#region menu File");
            writer.WriteLine("\t\tprivate void menuFileClose_ItemClick(object sender, ItemClickEventArgs e)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase.CloseTopMostChild();");
            writer.WriteLine("\t\t}");

            writer.WriteLine("\t\tprivate void menuFileCloseAll_ItemClick(object sender, ItemClickEventArgs e)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase.CloseAllChildren();");
            writer.WriteLine("\t\t}");

            writer.WriteLine("\t\tprivate void menuExit_ItemClick(object sender, ItemClickEventArgs e)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis.Close();");
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();

            writer.WriteLine("\t\t#region menu Dictionary");
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                if (!item.Alias.StartsWith("Util"))
                    continue;

                writer.WriteLine("\t\tprivate void menu{0}_ItemClick(object sender, ItemClickEventArgs e)", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tbase.ShowChildForm(typeof (FormWork{0}List));", item.Alias);
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }
            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();

            writer.WriteLine("\t\t#region menu Tools");
            writer.WriteLine("\t\tprivate void menuChangePassword_ItemClick(object sender, ItemClickEventArgs e)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tFormChangePassword dlg = new FormChangePassword(base.UserId, new LoginService());");
            writer.WriteLine("\t\t\tdlg.ShowDialog();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tprivate void menuWhoAmI_ItemClick(object sender, ItemClickEventArgs e)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase.ShowMessage(string.Format(\"I am {0}.\", base.UserId));");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\tprivate void menuUserAccount_ItemClick(object sender, ItemClickEventArgs e)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t//base.ShowChildForm(typeof (FormWorkUsrAccountList));");
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();

            writer.WriteLine("\t\t#region menu Help");
            writer.WriteLine("\t\tprivate void menuAbout_ItemClick(object sender, ItemClickEventArgs e)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tFormAboutBox dlg = new FormAboutBox();");
            writer.WriteLine("\t\t\tdlg.ShowDialog();");
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();
        }

        private void WriteListLinks(StringWriter writer)
        {
            writer.WriteLine("\t\t#region General");
            foreach (ObjectSchema item in base.MappingSchema.Objects)
            {
                if (item.GetPKList().Count == 2)
                    continue;

                writer.WriteLine("\t\tprivate void nbi{0}_LinkClicked(object sender, NavBarLinkEventArgs e)", item.Alias);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tbase.ShowChildForm(typeof (FormWork{0}List));", item.Alias);
                writer.WriteLine("\t\t}");
            }
            writer.WriteLine("\t\t#endregion");
        }
    }
}
