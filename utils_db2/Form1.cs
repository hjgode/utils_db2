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
    public partial class Form1 : Form
    {
        logger _logger = Program._logger;
        database _database;
        public Form1()
        {
            _logger.log("InitializeComponent()");
            InitializeComponent();
            _database = new database();
            if (_database._bConnected)
            {
                dataGridView1.DataSource = _database._bsUtils;
                formatColumns();
                listBoxAllDevices.DataSource = _database.readDeviceNameList();
                listBoxOperatingSystems.DataSource = _database._lstOSNames;
            }
        }

        void formatColumns()
        {
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["devices_id"].Visible = false;
            dataGridView1.Columns["operating_id"].Visible = false;
            dataGridView1.Columns["description"].Visible = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _database.Dispose();
            _logger.log("FormClosing");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            BindingSource bs = (BindingSource)dataGridView1.DataSource;
            bs.EndEdit();
            DataTable dt = (DataTable)bs.DataSource;

            // Just for test.... Try this with or without the EndEdit....
            DataTable changedTable = dt.GetChanges();
            _logger.log("changedTable.Rows: "+changedTable.Rows.Count.ToString());

            int rowsUpdated = _database._daUtils.Update(dt);
            _logger.log("updated " + rowsUpdated.ToString() + " rows");
        }

        /// <summary>
        /// update devices listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            int util_id = (int)row.Cells["id"].Value;
            //which devices_id?
            //DataTable dtUtils = _database.executeSQL("SELECT devices_id from utilities WHERE id=" + util_id);
            //DataRow drUtils = dtUtils.Rows[0];
            //int devices_id = (int)drUtils["devices_id"];
            ////which device are linked?
            //DataTable dtDevices = _database.executeSQL("Select device_id from devices WHERE devices_id="+devices_id);
            List<Device> listDevice = _database.getDevices(util_id);
            listBoxDevicesFor.DataSource = null;
            listBoxDevicesFor.Items.Clear();
            listBoxDevicesFor.DataSource = listDevice;

            txtDescription.Text = _database._description.getDescriptionforID(util_id);

            int operating_id = (int)row.Cells["operating_id"].Value;
            txtOperatingSystem.Text = _database._osname.getOSforID(operating_id);
        }
    }
}
