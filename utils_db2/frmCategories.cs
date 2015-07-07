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
    public partial class frmCategories : Form
    {
        utility _utility;
        Categories _categories;

        public frmCategories(utility utl)
        {
            InitializeComponent();
            _utility = utl;
            label1.Text = _utility.name;
            textBox1.Text = _utility._categories;

            _categories = new Categories();
            _categories.readCatsFromDB(database._sqlConnection);
            foreach (Category C in _categories.categories_list)
            {
                lbCategoriesAvailable.Items.Add(C);
                if (_utility._categories.Trim().Split().Contains<string>(C.cat_id.ToString()))
                {
                    lbCategoriesOfUtil.Items.Add(C);
                    _utility._category_list.Add(C);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void lbCategoriesOfUtil_DoubleClick(object sender, EventArgs e)
        {
            if (lbCategoriesOfUtil.SelectedIndex == -1)
                return;
            lbCategoriesOfUtil.Items.Remove(lbCategoriesOfUtil.SelectedItem);
        }

        private void lbCategoriesAvailable_DoubleClick(object sender, EventArgs e)
        {
            if (lbCategoriesAvailable.SelectedIndex == -1)
                return;
            if (lbCategoriesOfUtil.Items.Contains(lbCategoriesAvailable.SelectedItem))
                return;
            lbCategoriesOfUtil.Items.Add(lbCategoriesAvailable.SelectedItem);

            textBox1.Text = "";
            foreach(object o in lbCategoriesOfUtil.Items){
                textBox1.Text += " " + ((Category)o).cat_id.ToString();
            }
        }
    }
}
