using System;
using System.Windows.Forms;
using JetCode.BizSchema;

namespace JetCode.MapperConfigure.Controls
{
    public partial class FieldCtrl : UserControl
    {
        private FieldSchema _fieldSchema = null;
        private ObjectSchema _dstObject = null;

        public FieldCtrl()
        {
            InitializeComponent();
        }

        public FieldCtrl(FieldSchema fieldSchema, ObjectSchema dstObject)
        {
            InitializeComponent();

            this._fieldSchema = fieldSchema;
            this._dstObject = dstObject;
        }

        public void Init()
        {
            string alias = this.GetFieldAlias(this._fieldSchema);
            this.checkBox1.Checked = alias.Length == 0 ? false : true;
            this.lblColumnName.Text = this._fieldSchema.Name;
            this.txtFieldAlias.Text = alias.Length == 0 ? this._fieldSchema.Name : alias;
            this.lblFieldType.Text = this._fieldSchema.DataType;
            this.chkIsPK.Checked = this._fieldSchema.IsPK;
            this.chkIsNull.Checked = this._fieldSchema.IsNullable;

            if(this.IsFKColumn())
            {
                this.checkBox1.Checked = true;
                this.Enabled = false;
            }
        }

        public void Save()
        {
            if (!this.checkBox1.Checked || !this.Enabled)
                return;

            FieldSchema field = this._fieldSchema.Clone();
            field.Alias = this.txtFieldAlias.Text;
            field.IsJoined = true;
            field.IsPK = false;

            this._dstObject.Fields.Add(field);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.txtFieldAlias.Enabled = this.checkBox1.Checked;
        }

        private string GetFieldAlias(FieldSchema field)
        {
            foreach (FieldSchema item in this._dstObject.Fields)
            {
                if (item.Name == field.Name && item.TableName == field.TableName)
                {
                    return item.Alias;
                }
            }

            return string.Empty;
        }

        private bool IsFKColumn()
        {
            foreach (FieldSchema item in this._dstObject.Fields)
            {
                if (item.IsJoined)
                    continue;

                if (this._fieldSchema.IsPK && item.Name == this._fieldSchema.Name)
                {
                    return true;
                }
            }

            return false;
        }

        private void txtFieldAlias_Leave(object sender, EventArgs e)
        {
            this.txtFieldAlias.Text = this.txtFieldAlias.Text.Trim();
            if(this.txtFieldAlias.Text.Length == 0)
            {
                this.txtFieldAlias.Text = this.lblColumnName.Text;
            }
        }
    }
}