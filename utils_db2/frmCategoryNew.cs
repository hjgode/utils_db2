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
    public partial class frmCategoryNew : Form
    {
        Categories _categories;

        public frmCategoryNew(ref Categories cats)
        {
            InitializeComponent();
            _categories = cats;
            lbxKnownCats.DataSource = _categories.categories_list;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
                return;
            if (textBox2.Text.Length == 0)
                return;
            Category c = new Category(0, textBox1.Text, textBox2.Text);
            int iRes = _categories.addCategory(ref c);
            if (iRes > 0)
            {
                MessageBox.Show(c.cat_id.ToString() + "\n" + c.name + "\n" + c.description, "Added new category");
            }
            this.DialogResult = DialogResult.OK;
        }
    }
}
