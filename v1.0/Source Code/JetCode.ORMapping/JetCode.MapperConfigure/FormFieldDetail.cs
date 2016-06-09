using System;
using System.Windows.Forms;
using JetCode.BizSchema;
using JetCode.MapperConfigure.Controls;

namespace JetCode.MapperConfigure
{
    public partial class FormFieldDetail : Form
    {
        private MappingSchema _mapSchema = null;
        private ObjectSchema _objectSchema = null;

        public FormFieldDetail()
        {
            InitializeComponent();
        }

        public FormFieldDetail(MappingSchema mapSchema, ObjectSchema objectSchema)
        {
            InitializeComponent();
            this._mapSchema = mapSchema;
            this._objectSchema = objectSchema;
        }

        private void FormFieldDetail_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            if (this._objectSchema == null || this._objectSchema.Joins.Count == 0)
            {
                MessageBox.Show("Please add Joined table firstly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.Close();
            }
            else
            {
                foreach (JoinSchema item in this._objectSchema.Joins)
                {
                    ObjectSchema objSchema = this._mapSchema.Objects[item.RefTable];
                    if (objSchema == null)
                        continue;

                    ObjectSchema srcObject = objSchema.Clone();
                    srcObject.Alias = item.RefAlias;
                    foreach (FieldSchema field in srcObject.Fields)
                    {
                        if (field.IsJoined)
                            continue;

                        field.TableAlias = srcObject.Alias;
                    }


                    TabPage page = new TabPage();
                    page.Text = item.RefAlias;
                    page.BorderStyle = BorderStyle.Fixed3D;
                    FieldContainerCtrl container = new FieldContainerCtrl(srcObject, this._objectSchema);
                    container.Init();
                    container.Dock = DockStyle.Fill;
                    container.Parent = page;

                    this.tabControl1.Controls.Add(page);
                }
            }

            this.Cursor = Cursors.Default;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            foreach (TabPage item in this.tabControl1.Controls)
            {
                FieldContainerCtrl container = item.Controls[0] as FieldContainerCtrl;
                if(container != null)
                {
                    container.Save();
                }
            }
            this.Cursor = Cursors.Default;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}