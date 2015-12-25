using System;
using System.Windows.Forms;
using JetCode.BizSchema;

namespace JetCode.MapperConfigure.Controls
{
    public partial class TableAliasCtrl : UserControl
    {
        private MappingSchema _mapSchema = null;
        private ObjectSchema _objectSchema = null;

        public TableAliasCtrl()
        {
            InitializeComponent();
        }

        public void Init(MappingSchema mapSchema, string tableName)
        {
            this._mapSchema = mapSchema;
            this._objectSchema = this._mapSchema.Objects[tableName];
            this.textBox1.Text = this._objectSchema.Alias;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            string tableAlias = this.textBox1.Text.Trim();
            tableAlias = tableAlias.Replace(" ", "_");
            this.textBox1.Text = tableAlias;

            if(!this.IsValid(tableAlias))
            {
                MessageBox.Show("The Alias is invalid, please input anthor alias.", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            else
            {
                this.ModifyAlias(tableAlias);

                //update UI
                FormMain frm = this.GetMainFrom(this);
                if(frm != null)
                {
                    frm.UpdateUI();
                }
            }

            this.Cursor = Cursors.Default;
        }

        private bool IsValid(string alias)
        {
            if (alias.Length == 0)
                return false;

            if(!char.IsLetter(alias[0]))
                return false;

            foreach(ObjectSchema item in this._mapSchema.Objects)
            {
                if (item == this._objectSchema)
                    continue;

                if (item.Name == alias || item.Alias == alias)
                    return false;
            }

            foreach(JoinSchema item in this._objectSchema.Joins)
            {
                if (item.KeyTable == this._objectSchema.Name)
                    continue;

                if (item.RefAlias == alias)
                    return false;
            }

            return true;
        }

        private void ModifyAlias(string alias)
        {
            this._objectSchema.Alias = alias;

            //update field
            foreach (FieldSchema item in this._objectSchema.Fields)
            {
                if (item.IsJoined)
                    continue;

                item.TableAlias = alias;
            }

            //update join
            foreach (JoinSchema item in this._objectSchema.Joins)
            {
                if (item.KeyTable == this._objectSchema.Name)
                {
                    item.KeyAlias = alias;
                }
            }

            //update others children & parent
            foreach (ObjectSchema item in this._mapSchema.Objects)
            {
                if (item == this._objectSchema)
                    continue;

                foreach (ChildSchema child in item.Children)
                {
                    if (child.Name != this._objectSchema.Name)
                        continue;

                    child.Alias = alias;
                }

                foreach (ParentSchema parent in item.Parents)
                {
                    if (parent.Name != this._objectSchema.Name)
                        continue;

                    parent.Alias = alias;
                }
            }
        }



        private FormMain GetMainFrom(Control ctrl)
        {
            if (ctrl == null || ctrl.Parent == null)
            {
                return null;
            }

            if (ctrl.Parent.GetType().IsSubclassOf(typeof(Form)))
            {
                return ctrl.Parent as FormMain;
            }
            else
            {
                return GetMainFrom(ctrl.Parent);
            }
        }
    }
}