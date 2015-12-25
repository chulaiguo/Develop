using System;
using System.Windows.Forms;
using JetCode.BizSchema;

namespace JetCode.MapperConfigure
{
    public partial class FormCheckJoins : Form
    {
        private MappingSchema _mapSchema = null;

        public FormCheckJoins()
        {
            InitializeComponent();
        }

        public FormCheckJoins(MappingSchema mapSchema)
        {
            InitializeComponent();
            this._mapSchema = mapSchema;
        }

        private void FormCheckJoins_Load(object sender, EventArgs e)
        {
            MappingCheckErrors errors = new MappingCheckErrors(this._mapSchema);
            string error = errors.CheckErrors();
            if (error.Length == 0)
            {
                this.textBox1.Text = "No errors.";
            }
            else
            {
                this.textBox1.Text = error;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}