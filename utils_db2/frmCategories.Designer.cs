namespace utils_db2
{
    partial class frmCategories
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lbCategoriesOfUtil = new System.Windows.Forms.ListBox();
            this.lbCategoriesAvailable = new System.Windows.Forms.ListBox();
            this.btnNewCat = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(333, 380);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 27);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(435, 380);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(96, 27);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 49);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(294, 20);
            this.textBox1.TabIndex = 19;
            // 
            // lbCategoriesOfUtil
            // 
            this.lbCategoriesOfUtil.FormattingEnabled = true;
            this.lbCategoriesOfUtil.Location = new System.Drawing.Point(12, 93);
            this.lbCategoriesOfUtil.Name = "lbCategoriesOfUtil";
            this.lbCategoriesOfUtil.Size = new System.Drawing.Size(196, 173);
            this.lbCategoriesOfUtil.TabIndex = 20;
            this.lbCategoriesOfUtil.DoubleClick += new System.EventHandler(this.lbCategoriesOfUtil_DoubleClick);
            // 
            // lbCategoriesAvailable
            // 
            this.lbCategoriesAvailable.FormattingEnabled = true;
            this.lbCategoriesAvailable.Location = new System.Drawing.Point(335, 93);
            this.lbCategoriesAvailable.Name = "lbCategoriesAvailable";
            this.lbCategoriesAvailable.Size = new System.Drawing.Size(196, 173);
            this.lbCategoriesAvailable.TabIndex = 20;
            this.lbCategoriesAvailable.DoubleClick += new System.EventHandler(this.lbCategoriesAvailable_DoubleClick);
            // 
            // btnNewCat
            // 
            this.btnNewCat.Location = new System.Drawing.Point(436, 271);
            this.btnNewCat.Name = "btnNewCat";
            this.btnNewCat.Size = new System.Drawing.Size(94, 24);
            this.btnNewCat.TabIndex = 21;
            this.btnNewCat.Text = "new";
            this.btnNewCat.UseVisualStyleBackColor = true;
            this.btnNewCat.Click += new System.EventHandler(this.btnNewCat_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(333, 272);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(95, 22);
            this.btnEdit.TabIndex = 22;
            this.btnEdit.Text = "edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // frmCategories
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 419);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnNewCat);
            this.Controls.Add(this.lbCategoriesAvailable);
            this.Controls.Add(this.lbCategoriesOfUtil);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Name = "frmCategories";
            this.Text = "frmCategories";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListBox lbCategoriesOfUtil;
        private System.Windows.Forms.ListBox lbCategoriesAvailable;
        private System.Windows.Forms.Button btnNewCat;
        private System.Windows.Forms.Button btnEdit;
    }
}