using System;
using System.Windows.Forms;
using JetCode.BizSchema;

namespace JetCode.MapperConfigure
{
    public partial class FormJoinDetail : Form
    {
        private MappingSchema _mapSchema = null;
        private ObjectSchema _objectSchema = null;

        public FormJoinDetail()
        {
            InitializeComponent();
        }

        public FormJoinDetail(MappingSchema mapSchema, ObjectSchema objectSchema)
        {
            InitializeComponent();
            this._mapSchema = mapSchema;
            this._objectSchema = objectSchema;
        }

        private void FormJoinDetail_Load(object sender, EventArgs e)
        {
            //Ref table
            foreach(ObjectSchema item in this._mapSchema.Objects)
            {
                if (item.IsMultiPK)
                    continue;

                this.cmbRefTable.Items.Add(item.Name);
            }
            this.cmbRefTable.SelectedIndex = 0;

            //Key Table
            this.cmbKeyAlias.Items.Add(this._objectSchema.Alias);
            foreach (JoinSchema item in this._objectSchema.Joins)
            {
                this.cmbKeyAlias.Items.Add(item.RefAlias);
            }
            this.cmbKeyAlias.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!this.IsValid())
            {
                MessageBox.Show("JoinCommand is invalid. Please retry it again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                JoinSchema join = this.GetJoinSchema();
                this._objectSchema.Joins.Add(join);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool IsValid()
        {
            if(this.txtkeyName.Text.Trim().Length == 0)
                return false;

            if (this.cmbKeyAlias.Text == this.cmbRefTable.Text)
                return false;

            return true;
        }

        private void cmbKeyAlias_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbKeyFields.Items.Clear();
            JoinSchema join = this._objectSchema.Joins[this.cmbKeyAlias.Text];
            if (join == null)
            {
                this.txtkeyName.Text = this._objectSchema.Name;
                foreach (FieldSchema item in this._objectSchema.Fields)
                {
                    if (item.IsJoined)
                        continue;

                    this.cmbKeyFields.Items.Add(item.Name);
                }
            }
            else
            {
                foreach (FieldSchema item in this._mapSchema.Objects[join.RefTable].Fields)
                {
                    if (item.IsJoined)
                        continue;

                    this.cmbKeyFields.Items.Add(item.Name);
                }

                this.txtkeyName.Text = join.RefTable;
            }

            this.cmbKeyFields.SelectedIndex = 0;
        }

        private void cmbRefTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            ObjectSchema schema = this._mapSchema.Objects[this.cmbRefTable.Text];
            this.txtRefAlias.Text = schema.Alias;

            //RefField
            foreach (FieldSchema item in schema.Fields)
            {
                if (!item.IsPK)
                    continue;

                this.txtRefField.Text = item.Name;
                break;
            }
        }

        private void txtRefField_TextChanged(object sender, EventArgs e)
        {
            this.cmbKeyFields.Text = this.txtRefField.Text;
        }

        private void cmbKeyFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateJoinCommand();
        }

        private void txtRefAlias_TextChanged(object sender, EventArgs e)
        {
            this.UpdateJoinCommand();
        }

        private void UpdateJoinCommand()
        {
            JoinSchema join = this.GetJoinSchema();
            this.txtJoinCommand.Text = join.JoinCommand;
        }

        private JoinSchema GetJoinSchema()
        {
            JoinSchema join = new JoinSchema();
            join.KeyTable = this.txtkeyName.Text;
            join.KeyAlias = this.cmbKeyAlias.Text;
            join.KeyField = this.cmbKeyFields.Text;

            join.RefTable = this.cmbRefTable.Text;
            join.RefAlias = this.txtRefAlias.Text.Trim();
            join.RefField = this.txtRefField.Text;

            return join;
        }
    }
}