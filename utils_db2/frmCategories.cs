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

        public frmCategories(ref utility utl)
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
                //if (_utility._categories.Trim().Split().Contains<string>(C.cat_id.ToString()))
                //{
                //    lbCategoriesOfUtil.Items.Add(C);
                //    _utility._category_list.Add(C);
                //}
            }
            foreach (Category C in _utility._category_list)
            {
                lbCategoriesOfUtil.Items.Add(C);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _utility.saveCategoriesToDB();
            this.DialogResult = DialogResult.OK;
        }

        private void lbCategoriesOfUtil_DoubleClick(object sender, EventArgs e)
        {
            if (lbCategoriesOfUtil.SelectedIndex == -1)
                return;
            Category catToRemove = (Category) lbCategoriesOfUtil.SelectedItem;
            lbCategoriesOfUtil.Items.Remove(lbCategoriesOfUtil.SelectedItem);

            _utility._category_list.Remove(catToRemove);
            _utility._categories = updateCatIdString();
        }

        private void lbCategoriesAvailable_DoubleClick(object sender, EventArgs e)
        {
            if (lbCategoriesAvailable.SelectedIndex == -1)
                return;
            if (lbCategoriesOfUtil.Items.Contains(lbCategoriesAvailable.SelectedItem))
                return;

            Category catToAdd = (Category) lbCategoriesAvailable.SelectedItem;
            lbCategoriesOfUtil.Items.Add(lbCategoriesAvailable.SelectedItem);

            _utility._category_list.Add(catToAdd);
            _utility._categories = updateCatIdString();
        }

        string updateCatIdString()
        {
            textBox1.Text = "";
            foreach(object o in lbCategoriesOfUtil.Items){
                textBox1.Text += " " + ((Category)o).cat_id.ToString();
            }
            return textBox1.Text;
        }
    }
}
