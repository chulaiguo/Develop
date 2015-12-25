using System;
using System.Windows.Forms;
using JetCode.BizSchema;
using JetCode.BizSchema.Factory;

namespace JetCode.MapperConfigure
{
    public partial class FormMain : Form
    {
        private MappingSchema _mapSchema = null;

        public FormMain()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.menuConnect.PerformClick();
        }

        private void menuConnect_Click(object sender, EventArgs e)
        {
            try
            {
                FormDBConn dlg = new FormDBConn();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._mapSchema = SchemaFactory.GetMappingSchema(dlg.ConnString, dlg.Database);
                    this.FillObjectList();
                    this.EnableUI(true);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                this.EnableUI(false);
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void menuOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;
                    this._mapSchema = SchemaFactory.GetMappingSchema(this.openFileDialog1.FileName);
                    this.FillObjectList();
                    this.EnableUI(true);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                this.EnableUI(false);
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void menuSave_Click(object sender, EventArgs e)
        {
            if (this._mapSchema == null)
                return;

            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string xmlFile = this.saveFileDialog1.FileName;
                this._mapSchema.SaveToFile(xmlFile);
            }
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lstObjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lstObjects.Text.Length == 0)
                return;

            this.Cursor = Cursors.WaitCursor;
            this.gridFieldCtrl1.BindingData(this._mapSchema, this.lstObjects.Text);
            this.gridJoinCtrl1.BindingData(this._mapSchema, this.lstObjects.Text);
            this.gridChildCtrl1.BindingData(this._mapSchema, this.lstObjects.Text);
            this.gridParentCtrl1.BindingData(this._mapSchema, this.lstObjects.Text);
            this.gridIndexCtrl1.BindingData(this._mapSchema, this.lstObjects.Text);

            this.tableCtrl1.Init(this._mapSchema, this.lstObjects.Text);
            this.Cursor = Cursors.Default;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this._mapSchema == null)
                return;

            if (MessageBox.Show("Are you sure to close this form ?", "Question", MessageBoxButtons.YesNo,
                               MessageBoxIcon.Question) != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void FillObjectList()
        {
            if (this._mapSchema != null)
            {
                this.lstObjects.Items.Clear();
                foreach (ObjectSchema schema1 in this._mapSchema.Objects)
                {
                    this.lstObjects.Items.Add(schema1.Name);
                }

                this.lstObjects.SelectedIndex = 0;
            }
        }

        private void EnableUI(bool enabled)
        {
            this.menuSave.Enabled = enabled;
            this.toolSave.Enabled = enabled;
            this.menuMainTools.Enabled = enabled;

            this.tableCtrl1.Enabled = enabled;
        }

        internal void UpdateUI()
        {
            switch (this.tabControl1.SelectedIndex)
            {
                case 0:
                    this.gridFieldCtrl1.UpdateUI();
                    break;
                case 1:
                    this.gridJoinCtrl1.UpdateUI();
                    break;
                case 2:
                    this.gridChildCtrl1.UpdateUI();
                    break;
                case 3:
                    this.gridParentCtrl1.UpdateUI();
                    break;
                case 4:
                    this.gridIndexCtrl1.UpdateUI();
                    break;
                default:
                    break;
            }
        }


        #region Tools

        private void menuSynchronize_Click(object sender, EventArgs e)
        {
            try
            {
                FormDBConn dlg = new FormDBConn();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;
                    MappingSynchronize mappingSynchronize = new MappingSynchronize(this._mapSchema);
                    mappingSynchronize.Synchronize(dlg.ConnString, dlg.Database);
                    this.FillObjectList();
                    this.EnableUI(true);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                this.EnableUI(false);
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void addNewSchemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FormDBConn dlg = new FormDBConn();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;
                    MappingSynchronize mappingSynchronize = new MappingSynchronize(this._mapSchema);
                    mappingSynchronize.AddNewSchema(dlg.ConnString, dlg.Database);
                    this.FillObjectList();
                    this.EnableUI(true);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                this.EnableUI(false);
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void menuCheckErrors_Click(object sender, EventArgs e)
        {
            FormCheckJoins dlg = new FormCheckJoins(this._mapSchema);
            dlg.ShowDialog();
        }

        private void mappingNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMappingName dlg = new FormMappingName(this._mapSchema);
            dlg.ShowDialog();
        }

        #endregion

        #region Help
        private void menuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("About");
        }
        #endregion

        private void lstObjects_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                int index = this.lstObjects.SelectedIndex;
                ObjectSchema obj = this._mapSchema.GetObjectByName(this.lstObjects.Text);
                if (obj == null)
                    return;

                this._mapSchema.Objects.Remove(obj);

                if(index == this.lstObjects.Items.Count - 1)
                {
                    this.lstObjects.SelectedIndex = index - 1;
                }
                else
                {
                    this.lstObjects.SelectedIndex = index + 1;
                }
                
                this.lstObjects.Items.RemoveAt(index);
            }
        }

    }
}