using System;
using System.Data.Sql;
using System.Windows.Forms;

namespace JetCode.MapperConfigure
{
    public partial class FormDBConn : Form
    {
        private string _connString = string.Empty;
        private string _database = string.Empty;

        public FormDBConn()
        {
            InitializeComponent();
        }

        public string Database
        {
            get { return this._database; }
        }

        public string ConnString
        {
            get { return this._connString; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if(this.cmbServer.Text.Trim().Length == 0)
            {
                this.cmbServer.Focus();
                return;
            }

            if (this.txtDatabase.Text.Trim().Length == 0)
            {
                this.txtDatabase.Focus();
                return;
            }

            this._database = this.txtDatabase.Text;
            if (this.chkByWindows.Checked)
            {
                this._connString = "Data Source=" + this.cmbServer.Text + ";Initial Catalog=" + this.txtDatabase.Text + ";Integrated Security=SSPI;";
            }
            else
            {
                this._connString = "Data Source=" + this.cmbServer.Text + ";Initial Catalog=" + this.txtDatabase.Text + ";User ID=" + this.txtUser.Text + ";Password=" + this.txtPassword.Text + ";";
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void chkByWindows_CheckedChanged(object sender, EventArgs e)
        {
            this.txtUser.Enabled = !this.chkByWindows.Checked;
            this.txtPassword.Enabled = !this.chkByWindows.Checked;
        }

        private void cmbServer_DropDown(object sender, EventArgs e)
        {
            if (this.cmbServer.Tag != null)
                return;

            this.Cursor = Cursors.WaitCursor;
            System.Data.DataTable table = SqlDataSourceEnumerator.Instance.GetDataSources();
            foreach (System.Data.DataRow row in table.Rows)
            {
                this.cmbServer.Items.Add(row["ServerName"]);
            }
            this.cmbServer.Tag = 1;
            this.Cursor = Cursors.Default;
        }

        private void txtDatabase_TextChanged(object sender, EventArgs e)
        {
            this.txtUser.Text = this.txtDatabase.Text;
        }
    }
}