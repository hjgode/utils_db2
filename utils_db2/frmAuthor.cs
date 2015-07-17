using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using utils_data;

namespace utils_db2
{
    public partial class frmAuthor : Form
    {
        public utility _utility = null;
        public frmAuthor(utility utl)
        {
            InitializeComponent();
            _utility = utl;
            textBox1.Text = _utility.author;
            label1.Text = _utility.name;
            fillKnownNames();
        }

        void fillKnownNames()
        {
            if (database._sqlConnection == null)
                return;
            SqlCommand cmd= new SqlCommand("select distinct author from utils;", database._sqlConnection);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                string a = rdr[0].ToString().Trim();
                if(a.Length>0)
                    listBox1.Items.Add(a);
            }
            rdr.Close();
            cmd.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _utility.author = textBox1.Text.Trim();
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;
            textBox1.Text = listBox1.SelectedItem.ToString();
        }
    }
}
