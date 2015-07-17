using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using utils_data;

namespace utils_db2
{
    public partial class frmNewUtility : Form
    {
        utilities _utilities = null;

        public frmNewUtility(ref utilities utls)
        {
            InitializeComponent();
            _utilities = utls;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Length > 0)
            {
                _utilities.addUtility(txtName.Text);
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("No empty utility name allowed");
                DialogResult = DialogResult.Cancel;
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
