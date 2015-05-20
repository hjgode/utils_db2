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

        int _current_UtilID = -1;
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            int util_id = (int)row.Cells["id"].Value;
            _current_UtilID = util_id;

            int iColumn = e.ColumnIndex;
            //open Details Form
            if (row.Cells[iColumn].OwningColumn.HeaderText == "description")
            {
                frmDescription frm = new frmDescription(_utilities.getUtilityByID(util_id));
                if (frm.ShowDialog() == DialogResult.OK){
                    _utilities.updateDescription(frm._utility, database._sqlConnection);
                    dataGridView1.Refresh();
                }
                frm.Dispose();
            }

            if (row.Cells[iColumn].OwningColumn.HeaderText == "author")
            {
                frmAuthor frm = new frmAuthor(_utilities.getUtilityByID(util_id));
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    _utilities.updateAuthor(frm._utility, database._sqlConnection);
                    dataGridView1.Refresh();
                }
                frm.Dispose();
            }

            if (row.Cells[iColumn].OwningColumn.HeaderText == "file_link")
            {
                frmFile frm = new frmFile(_utilities.getUtilityByID(util_id));
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    _utilities.setImageData(util_id, frm._utility.file_data, database._sqlConnection);
                    _utilities.setFile_Link(util_id, frm._utility.file_link, database._sqlConnection);
                    dataGridView1.Refresh();
                }
                frm.Dispose();
            }
        }

    }
}
