using System;
using System.Windows.Forms;
using JetCode.BizSchema;

namespace JetCode.MapperConfigure.Controls
{
    public partial class GridJoinCtrl : UserControl
    {
        private MappingSchema _mapSchema = null;
        private string _objectName = string.Empty;

        public GridJoinCtrl()
        {
            InitializeComponent();
        }

        public void BindingData(MappingSchema mapSchema, string objectName)
        {
            this._mapSchema = mapSchema;
            this._objectName = objectName;
            this.bindingSource1.DataSource = null;
            this.bindingSource1.DataSource = this._mapSchema.Objects[this._objectName].Joins;

            this.bindingNavigatorAddNewItem.Enabled = true;
            this.bindingNavigatorDeleteItem.Enabled = true;
        }

        public void UpdateUI()
        {
            this.bindingSource1.ResetBindings(false);
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            FormJoinDetail dlg = new FormJoinDetail(this._mapSchema, this._mapSchema.Objects[this._objectName]);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.bindingSource1.DataSource = null;
                this.bindingSource1.DataSource = this._mapSchema.Objects[this._objectName].Joins;
            }
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if (!this.CanDeleteJoin())
            {
                MessageBox.Show("You can not delete this join.\r\nMaybe joined fields or other joins are using it.", "Delete Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Are you sure to delete this join?", "Question", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.dataGridView1.Rows.Remove(this.dataGridView1.SelectedRows[0]);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1)
                return;

            ObjectSchema objSchema = this._mapSchema.Objects[this._objectName];
            if(objSchema == null)
                return;

            JoinSchema join = objSchema.Joins[e.RowIndex];
            if (join == null)
                return;

            //modify alias in Fields
            foreach (FieldSchema item in objSchema.Fields)
            {
                if(!item.IsJoined)
                    continue;

                if(objSchema.Joins[item.TableAlias] != null)
                    continue;

                item.TableAlias = join.RefAlias;
            }

            //modify alias in Joins
            foreach (JoinSchema item in objSchema.Joins)
            {
                if (item == join)
                    continue;

                if (item.KeyTable != join.RefTable)
                    continue;

                item.KeyAlias = join.RefAlias;
            }
        }

        private JoinSchema GetSelectJoin()
        {
            if (this.dataGridView1.SelectedRows.Count == 0)
                return null;

            DataGridViewCell cell = this.dataGridView1.SelectedRows[0].Cells["RefAlias"];
            if (cell == null)
                return null;

            return this._mapSchema.Objects[this._objectName].Joins[cell.Value.ToString()];
        }

        private bool CanDeleteJoin()
        {
            JoinSchema join = this.GetSelectJoin();
            if (join == null)
                return false;

            ObjectSchema objectSchema = this._mapSchema.Objects[this._objectName];
            foreach (FieldSchema item in objectSchema.Fields)
            {
                if (item.TableAlias == join.RefAlias)
                    return false;
            }

            foreach (JoinSchema item in objectSchema.Joins)
            {
                if (item == join)
                    continue;

                if (item.KeyTable == join.RefTable)
                    return false;
            }

            return true;
        }
    }
}