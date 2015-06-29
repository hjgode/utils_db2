using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace utils_db2
{
    public partial class frmFile : Form
    {
        public utility _utility = null;
        utilities _utilities = null;

        byte[] filedata=null;
        string filename = "";

        public frmFile(utility utl, ref utilities utls)
        {
            InitializeComponent();
            _utility = utl;
            _utilities = utls;

            label1.Text = _utility.name;
            txtFile.Text = _utility.file_link;

            //on demmand read of byte data
            if (_utility.file_data == null)
            {
                byte[] fData = _utilities.getFileData(_utility.util_id);
                _utility.setFileData(fData);
            }
            
            if (_utility.file_link.Length > 0)
                txtFileLink.Text = _utility.file_link;
            else
                txtFileLink.Text = "";

            if (_utility.file_data != null)
            {
                filedata = _utility.file_data;
                if (_utility.file_data.Length > 0)
                    txtFileDetails.Text = _utility.file_data.Length.ToString();
                else
                    txtFileDetails.Text = "";
            }
            else
                txtFileDetails.Text = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _utility.file_data = filedata;
            _utility.file_link = filename;
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.RestoreDirectory = true;
            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;
            ofd.Filter = "zip files|*.zip|all files|*.*";
            ofd.FilterIndex = 0;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    filedata = utilities.GetByteData(ofd.FileName);
                    filename = System.IO.Path.GetFileName(ofd.FileName);
                    txtFile.Text = ofd.FileName;
                    txtFileLink.Text = filename;
                    txtFileDetails.Text = filedata.Length.ToString();
                }
                catch (Exception ex) {
                    MessageBox.Show("Exception reading file: " + ex.Message);
                }
            }
            ofd.Dispose();
        }

        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.RestoreDirectory = true;
            ofd.CheckPathExists = true;
            ofd.OverwritePrompt = true;
            ofd.Filter = "zip files|*.zip|all files|*.*";
            ofd.FilterIndex = 0;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _utility.writeFileData(ofd.FileName);
                    txtFile.Text = ofd.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception writing file: " + ex.Message);
                }
            }
            ofd.Dispose();
        }
    }
}
