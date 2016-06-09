namespace JetCode.MapperConfigure.Controls
{
    partial class FieldCtrl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.txtFieldAlias = new System.Windows.Forms.TextBox();
            this.lblColumnName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblFieldType = new System.Windows.Forms.Label();
            this.chkIsPK = new System.Windows.Forms.CheckBox();
            this.chkIsNull = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(16, 12);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // txtFieldAlias
            // 
            this.txtFieldAlias.Enabled = false;
            this.txtFieldAlias.Location = new System.Drawing.Point(199, 9);
            this.txtFieldAlias.Name = "txtFieldAlias";
            this.txtFieldAlias.Size = new System.Drawing.Size(134, 20);
            this.txtFieldAlias.TabIndex = 2;
            this.txtFieldAlias.Leave += new System.EventHandler(this.txtFieldAlias_Leave);
            // 
            // lblColumnName
            // 
            this.lblColumnName.AutoSize = true;
            this.lblColumnName.Location = new System.Drawing.Point(50, 12);
            this.lblColumnName.Name = "lblColumnName";
            this.lblColumnName.Size = new System.Drawing.Size(70, 13);
            this.lblColumnName.TabIndex = 3;
            this.lblColumnName.Text = "ColumnName";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(167, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Alias";
            // 
            // lblFieldType
            // 
            this.lblFieldType.AutoSize = true;
            this.lblFieldType.Location = new System.Drawing.Point(349, 12);
            this.lblFieldType.Name = "lblFieldType";
            this.lblFieldType.Size = new System.Drawing.Size(56, 13);
            this.lblFieldType.TabIndex = 6;
            this.lblFieldType.Text = "Field Type";
            // 
            // chkIsPK
            // 
            this.chkIsPK.AutoSize = true;
            this.chkIsPK.Enabled = false;
            this.chkIsPK.Location = new System.Drawing.Point(455, 12);
            this.chkIsPK.Name = "chkIsPK";
            this.chkIsPK.Size = new System.Drawing.Size(48, 17);
            this.chkIsPK.TabIndex = 7;
            this.chkIsPK.Text = "IsPK";
            this.chkIsPK.UseVisualStyleBackColor = true;
            // 
            // chkIsNull
            // 
            this.chkIsNull.AutoSize = true;
            this.chkIsNull.Enabled = false;
            this.chkIsNull.Location = new System.Drawing.Point(509, 11);
            this.chkIsNull.Name = "chkIsNull";
            this.chkIsNull.Size = new System.Drawing.Size(62, 17);
            this.chkIsNull.TabIndex = 8;
            this.chkIsNull.Text = "IsNULL";
            this.chkIsNull.UseVisualStyleBackColor = true;
            // 
            // FieldCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkIsNull);
            this.Controls.Add(this.chkIsPK);
            this.Controls.Add(this.lblFieldType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblColumnName);
            this.Controls.Add(this.txtFieldAlias);
            this.Controls.Add(this.checkBox1);
            this.Name = "FieldCtrl";
            this.Size = new System.Drawing.Size(583, 32);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox txtFieldAlias;
        private System.Windows.Forms.Label lblColumnName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblFieldType;
        private System.Windows.Forms.CheckBox chkIsPK;
        private System.Windows.Forms.CheckBox chkIsNull;
    }
}
