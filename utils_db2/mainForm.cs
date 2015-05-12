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
    public partial class mainForm : Form
    {
        database _database = new database();
        utilities _utilities=new utilities();

        public mainForm()
        {
            InitializeComponent();
            _utilities.readUtilsDB(database._sqlConnection);

            dataGridView1.DataSource = _utilities.utilitiesList;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["file_data"].Visible = false; //we can not show binary data as image

            dataGridView1.Refresh();

            //utilities.Serialize(_utilities);
            _utilities.setImageData(2, helper.getAppPath + @"\cpumon.zip", database._sqlConnection);

            //test
            byte[] buf = _utilities.getFileData(2);
        }
    }
}
