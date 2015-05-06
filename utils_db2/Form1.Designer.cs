namespace utils_db2
{
    partial class Form1
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnSave = new System.Windows.Forms.Button();
            this.listBoxAllDevices = new System.Windows.Forms.ListBox();
            this.listBoxDevicesFor = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtOperatingSystem = new System.Windows.Forms.TextBox();
            this.listBoxOperatingSystems = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.btnDeviceAdd = new System.Windows.Forms.Button();
            this.btnDeviceRemove = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 37);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(242, 145);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(173, 188);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(81, 30);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // listBoxAllDevices
            // 
            this.listBoxAllDevices.FormattingEnabled = true;
            this.listBoxAllDevices.Location = new System.Drawing.Point(531, 37);
            this.listBoxAllDevices.Name = "listBoxAllDevices";
            this.listBoxAllDevices.Size = new System.Drawing.Size(226, 56);
            this.listBoxAllDevices.TabIndex = 2;
            this.listBoxAllDevices.DoubleClick += new System.EventHandler(this.listBoxAllDevices_DoubleClick);
            // 
            // listBoxDevicesFor
            // 
            this.listBoxDevicesFor.FormattingEnabled = true;
            this.listBoxDevicesFor.Location = new System.Drawing.Point(279, 37);
            this.listBoxDevicesFor.Name = "listBoxDevicesFor";
            this.listBoxDevicesFor.Size = new System.Drawing.Size(218, 56);
            this.listBoxDevicesFor.TabIndex = 3;
            this.listBoxDevicesFor.DoubleClick += new System.EventHandler(this.listBoxDevicesFor_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Utility Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(276, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Device(s):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(528, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Available Device:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(276, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Operating System:";
            // 
            // txtOperatingSystem
            // 
            this.txtOperatingSystem.Location = new System.Drawing.Point(279, 126);
            this.txtOperatingSystem.Name = "txtOperatingSystem";
            this.txtOperatingSystem.Size = new System.Drawing.Size(218, 20);
            this.txtOperatingSystem.TabIndex = 6;
            // 
            // listBoxOperatingSystems
            // 
            this.listBoxOperatingSystems.FormattingEnabled = true;
            this.listBoxOperatingSystems.Location = new System.Drawing.Point(531, 126);
            this.listBoxOperatingSystems.Name = "listBoxOperatingSystems";
            this.listBoxOperatingSystems.Size = new System.Drawing.Size(226, 56);
            this.listBoxOperatingSystems.TabIndex = 2;
            this.listBoxOperatingSystems.DoubleClick += new System.EventHandler(this.listBoxOperatingSystems_DoubleClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(528, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(144, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Available Operating Systems:";
            // 
            // txtDescription
            // 
            this.txtDescription.AcceptsReturn = true;
            this.txtDescription.AcceptsTab = true;
            this.txtDescription.Location = new System.Drawing.Point(278, 207);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDescription.Size = new System.Drawing.Size(479, 123);
            this.txtDescription.TabIndex = 7;
            // 
            // btnDeviceAdd
            // 
            this.btnDeviceAdd.Location = new System.Drawing.Point(503, 37);
            this.btnDeviceAdd.Name = "btnDeviceAdd";
            this.btnDeviceAdd.Size = new System.Drawing.Size(22, 21);
            this.btnDeviceAdd.TabIndex = 8;
            this.btnDeviceAdd.Text = "<";
            this.btnDeviceAdd.UseVisualStyleBackColor = true;
            this.btnDeviceAdd.Click += new System.EventHandler(this.btnDeviceAdd_Click);
            // 
            // btnDeviceRemove
            // 
            this.btnDeviceRemove.Location = new System.Drawing.Point(503, 64);
            this.btnDeviceRemove.Name = "btnDeviceRemove";
            this.btnDeviceRemove.Size = new System.Drawing.Size(22, 21);
            this.btnDeviceRemove.TabIndex = 8;
            this.btnDeviceRemove.Text = "-";
            this.btnDeviceRemove.UseVisualStyleBackColor = true;
            this.btnDeviceRemove.Click += new System.EventHandler(this.btnDeviceRemove_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 348);
            this.Controls.Add(this.btnDeviceRemove);
            this.Controls.Add(this.btnDeviceAdd);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtOperatingSystem);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxDevicesFor);
            this.Controls.Add(this.listBoxOperatingSystems);
            this.Controls.Add(this.listBoxAllDevices);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ListBox listBoxAllDevices;
        private System.Windows.Forms.ListBox listBoxDevicesFor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtOperatingSystem;
        private System.Windows.Forms.ListBox listBoxOperatingSystems;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button btnDeviceAdd;
        private System.Windows.Forms.Button btnDeviceRemove;
    }
}

