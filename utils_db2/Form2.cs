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
    public partial class Form2 : Form
    {
        database _database = new database();
        util _util=new util();

        public Form2()
        {
            InitializeComponent();
            _util.readUtilsDB(database._sqlConnection);

            dataGridView1.DataSource = _util.utilities;
            dataGridView1.Refresh();
        }
    }
}
