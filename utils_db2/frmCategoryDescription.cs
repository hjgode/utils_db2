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
    public partial class frmCategoryDescription : Form
    {
        public Category _category = null;
        public frmCategoryDescription(ref Category cat)
        {
            InitializeComponent();
            _category = cat;
            label1.Text = _category.name;
            textBox1.Text = _category.description;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _category.description = textBox1.Text.Trim();
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

    }
}
