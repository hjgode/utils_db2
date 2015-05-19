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
    public partial class frmDescription : Form
    {
        public utility _utility = null;
        public frmDescription(utility utl)
        {
            InitializeComponent();
            _utility = utl;
            label1.Text = _utility.name;
            textBox1.Text = _utility.description;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _utility.description = textBox1.Text.Trim();
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

    }
}
