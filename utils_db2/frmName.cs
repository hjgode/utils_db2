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
    public partial class frmName : Form
    {
        public utility _utility = null;
        public frmName(utility utl)
        {
            _utility = utl;
            InitializeComponent();
            label1.Text = _utility.name;
            lblID.Text = _utility.id.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _utility.name = txtUtilName.Text.Trim();
            DialogResult = DialogResult.OK;
        }

    }
}
