namespace JetCode.MapperConfigure
{
    partial class FormJoinDetail
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbRefTable = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRefAlias = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbKeyFields = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtRefField = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbKeyAlias = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtkeyName = new System.Windows.Forms.TextBox();
            this.lblJoinCommand = new System.Windows.Forms.Label();
            this.txtJoinCommand = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbRefTable
            // 
            this.cmbRefTable.DropDownHeight = 200;
            this.cmbRefTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRefTable.FormattingEnabled = true;
            this.cmbRefTable.IntegralHeight = false;
            this.cmbRefTable.Location = new System.Drawing.Point(9, 37);
            this.cmbRefTable.Name = "cmbRefTable";
            this.cmbRefTable.Size = new System.Drawing.Size(138, 21);
            this.cmbRefTable.Sorted = true;
            this.cmbRefTable.TabIndex = 0;
            this.cmbRefTable.SelectedIndexChanged += new System.EventHandler(this.cmbRefTable_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            // 
            // txtRefAlias
            // 
            this.txtRefAlias.Location = new System.Drawing.Point(9, 90);
            this.txtRefAlias.Name = "txtRefAlias";
            this.txtRefAlias.Size = new System.Drawing.Size(138, 20);
            this.txtRefAlias.TabIndex = 2;
            this.txtRefAlias.TextChanged += new System.EventHandler(this.txtRefAlias_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Alias";
            // 
            // cmbKeyFields
            // 
            this.cmbKeyFields.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKeyFields.FormattingEnabled = true;
            this.cmbKeyFields.Location = new System.Drawing.Point(13, 139);
            this.cmbKeyFields.Name = "cmbKeyFields";
            this.cmbKeyFields.Size = new System.Drawing.Size(138, 21);
            this.cmbKeyFields.Sorted = true;
            this.cmbKeyFields.TabIndex = 7;
            this.cmbKeyFields.SelectedIndexChanged += new System.EventHandler(this.cmbKeyFields_SelectedIndexChanged);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(180, 247);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(265, 247);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtRefField);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtRefAlias);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbRefTable);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(159, 174);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Ref Table";
            // 
            // txtRefField
            // 
            this.txtRefField.Location = new System.Drawing.Point(9, 141);
            this.txtRefField.Name = "txtRefField";
            this.txtRefField.ReadOnly = true;
            this.txtRefField.Size = new System.Drawing.Size(138, 20);
            this.txtRefField.TabIndex = 10;
            this.txtRefField.TextChanged += new System.EventHandler(this.txtRefField_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Ref Field";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Key Fields";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(177, 155);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "=";
            // 
            // cmbKeyAlias
            // 
            this.cmbKeyAlias.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKeyAlias.FormattingEnabled = true;
            this.cmbKeyAlias.Location = new System.Drawing.Point(13, 36);
            this.cmbKeyAlias.Name = "cmbKeyAlias";
            this.cmbKeyAlias.Size = new System.Drawing.Size(138, 21);
            this.cmbKeyAlias.Sorted = true;
            this.cmbKeyAlias.TabIndex = 11;
            this.cmbKeyAlias.SelectedIndexChanged += new System.EventHandler(this.cmbKeyAlias_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Alias";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cmbKeyFields);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtkeyName);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.cmbKeyAlias);
            this.groupBox2.Location = new System.Drawing.Point(196, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(160, 173);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Key Table";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 73);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Name";
            // 
            // txtkeyName
            // 
            this.txtkeyName.Location = new System.Drawing.Point(13, 89);
            this.txtkeyName.Name = "txtkeyName";
            this.txtkeyName.ReadOnly = true;
            this.txtkeyName.Size = new System.Drawing.Size(138, 20);
            this.txtkeyName.TabIndex = 2;
            // 
            // lblJoinCommand
            // 
            this.lblJoinCommand.AutoSize = true;
            this.lblJoinCommand.Location = new System.Drawing.Point(13, 202);
            this.lblJoinCommand.Name = "lblJoinCommand";
            this.lblJoinCommand.Size = new System.Drawing.Size(76, 13);
            this.lblJoinCommand.TabIndex = 14;
            this.lblJoinCommand.Text = "Join Command";
            // 
            // txtJoinCommand
            // 
            this.txtJoinCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtJoinCommand.Location = new System.Drawing.Point(12, 218);
            this.txtJoinCommand.Name = "txtJoinCommand";
            this.txtJoinCommand.ReadOnly = true;
            this.txtJoinCommand.Size = new System.Drawing.Size(344, 20);
            this.txtJoinCommand.TabIndex = 15;
            // 
            // FormJoinDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 282);
            this.Controls.Add(this.txtJoinCommand);
            this.Controls.Add(this.lblJoinCommand);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormJoinDetail";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Join Detail";
            this.Load += new System.EventHandler(this.FormJoinDetail_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbRefTable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRefAlias;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbKeyFields;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbKeyAlias;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtkeyName;
        private System.Windows.Forms.Label lblJoinCommand;
        private System.Windows.Forms.TextBox txtJoinCommand;
        private System.Windows.Forms.TextBox txtRefField;
    }
}