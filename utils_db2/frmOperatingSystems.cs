using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;

namespace utils_db2
{
    public partial class frmOperatingSystems : Form
    {
        public utility _utility = null;
        public Operating_System _currentOS = null;

        public frmOperatingSystems(utility utl)
        {
            InitializeComponent();
            _utility = utl;

            label1.Text = _utility.name;
            _currentOS = _utility.operating_system;
            txtOSname.Text = _utility.operating_system.ToString();
            fillKnownNames();
        }

        void fillKnownNames()
        {
            listBox1.Items.Clear();
            Operating_System osclass = new Operating_System();
            List<Operating_System> osList=new List<Operating_System>();

            osList = osclass.readList(database._sqlConnection);
            foreach (Operating_System os in osList)
                listBox1.Items.Add(os);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _utility.operating_system = _currentOS;
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;
            _currentOS = (Operating_System)listBox1.SelectedItem;
            txtOSname.Text = _currentOS.ToString();
        }
    }
}
