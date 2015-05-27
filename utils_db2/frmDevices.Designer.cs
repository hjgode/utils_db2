namespace utils_db2
{
    partial class frmDevices
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
            this.listBoxAvailableDevices = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtDEVname = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listBoxDevices = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxAvailableDevices
            // 
            this.listBoxAvailableDevices.FormattingEnabled = true;
            this.listBoxAvailableDevices.Location = new System.Drawing.Point(121, 131);
            this.listBoxAvailableDevices.Name = "listBoxAvailableDevices";
            this.listBoxAvailableDevices.Size = new System.Drawing.Size(323, 69);
            this.listBoxAvailableDevices.TabIndex = 18;
            this.listBoxAvailableDevices.DoubleClick += new System.EventHandler(this.listBoxAvailableDevices_DoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Devices:";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(331, 291);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 27);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(433, 291);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(96, 27);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtDEVname
            // 
            this.txtDEVname.Location = new System.Drawing.Point(121, 244);
            this.txtDEVname.Name = "txtDEVname";
            this.txtDEVname.Size = new System.Drawing.Size(323, 20);
            this.txtDEVname.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(432, 24);
            this.label1.TabIndex = 13;
            this.label1.Text = "label1";
            // 
            // listBoxDevices
            // 
            this.listBoxDevices.FormattingEnabled = true;
            this.listBoxDevices.Location = new System.Drawing.Point(121, 39);
            this.listBoxDevices.Name = "listBoxDevices";
            this.listBoxDevices.Size = new System.Drawing.Size(323, 69);
            this.listBoxDevices.TabIndex = 18;
            this.listBoxDevices.DoubleClick += new System.EventHandler(this.listBoxDevices_DoubleClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Known devices:";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(459, 242);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(69, 21);
            this.btnAdd.TabIndex = 19;
            this.btnAdd.Text = "add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // frmDevices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 331);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.listBoxDevices);
            this.Controls.Add(this.listBoxAvailableDevices);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtDEVname);
            this.Controls.Add(this.label1);
            this.Name = "frmDevices";
            this.Text = "frmDevices";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxAvailableDevices;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtDEVname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBoxDevices;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAdd;
    }
}