using System;
using System.Windows.Forms;
using JetCode.BizSchema;

namespace JetCode.MapperConfigure.Controls
{
    public partial class GridFieldCtrl : UserControl
    {
        private MappingSchema _mapSchema = null;
        private string _objectName = string.Empty;

        public GridFieldCtrl()
        {
            InitializeComponent();
        }

        public void BindingData(MappingSchema mapSchema, string objectName)
        {
            this._mapSchema = mapSchema;
            this._objectName = objectName;
            this.bindingSource1.DataSource = null;
            this.bindingSource1.DataSource = this._mapSchema.Objects[this._objectName].Fields;

            this.bindingNavigatorAddNewItem.Enabled = true;
        }

        public void UpdateUI()
        {
            this.bindingSource1.ResetBindings(false);
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            FormFieldDetail dlg = new FormFieldDetail(this._mapSchema, this._mapSchema.Objects[this._objectName]);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.bindingSource1.DataSource = null;
                this.bindingSource1.DataSource = this._mapSchema.Objects[this._objectName].Fields;
            }
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            FieldSchema field = this.GetSelectField();
            if (field == null)
                return;

            if (MessageBox.Show("Are you sure to delete this field?", "Question", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.dataGridView1.Rows.Remove(this.dataGridView1.SelectedRows[0]);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            this.bindingNavigatorDeleteItem.Enabled = this.CanDeleteField();
        }

        private FieldSchema GetSelectField()
        {
            if (this.dataGridView1.SelectedRows.Count == 0)
                return null;

            DataGridViewCell cell = this.dataGridView1.SelectedRows[0].Cells["Name"];
            if (cell == null)
                return null;

            return this._mapSchema.Objects[this._objectName].Fields[cell.Value.ToString()];
        }

        private bool CanDeleteField()
        {
            FieldSchema field = this.GetSelectField();
            if (field == null || !field.IsJoined)
                return false;

            return true;
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1)
                return;

            //IsPK
            DataGridViewCell cell = this.dataGridView1.Rows[e.RowIndex].Cells["IsPK"];
            if (cell != null)
            {
                if (cell.Value.ToString() == true.ToString())
                {
                    e.CellStyle.ForeColor = System.Drawing.Color.Red;
                }
            }

            //IsJoin
            cell = this.dataGridView1.Rows[e.RowIndex].Cells["IsJoined"];
            if (cell != null)
            {
                if (cell.Value.ToString() == true.ToString())
                {
                    e.CellStyle.ForeColor = System.Drawing.Color.Green;
                }
            }
        }
    }
}