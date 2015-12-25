using System;
using System.Windows.Forms;
using JetCode.BizSchema;

namespace JetCode.MapperConfigure
{
    public partial class FormMappingName : Form
    {
        private MappingSchema _mapSchema = null;

        public FormMappingName()
        {
            InitializeComponent();
        }

        public FormMappingName(MappingSchema mapSchema)
        {
            InitializeComponent();
            this._mapSchema = mapSchema;
        }

        private void FormMappingName_Load(object sender, EventArgs e)
        {
            this.txtMappingName.Text = this._mapSchema.Name;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string name = this.txtMappingName.Text.Trim();
            if(name.Length == 0)
            {
                MessageBox.Show("Mapping name is invalid.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.txtMappingName.Focus();
                return;
            }

            this._mapSchema.Name = name;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}