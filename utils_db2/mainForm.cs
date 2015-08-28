using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Reflection;
using utils_data;

namespace utils_db2
{
    public partial class mainForm : Form
    {
        database _database=null;
        utilities _utilities=null;
        myLogger.logger _logger = Program._logger;

        BindingSource source = new BindingSource();

        public mainForm()
        {

            Application.DoEvents();

            InitializeComponent();

            int iRes = openDB();
            
            Program.frm.Close();

            if (iRes != 0)
                MessageBox.Show("Database did not load. Check network connection.");
        }

        int openDB()
        {
            int iRes = 0;
            try
            {
                string connect = Properties.Settings.Default.connectstring;

                _logger.log("Open database...");
                _database = new database(connect);

                //read the helper lists first and update the utilities objects
                //devices
                Devices devices = new Devices();
                devices.readDevicesFromDB(database._sqlConnection);
                
                //categories
                Categories categories = new Categories();
                categories.readCatsFromDB(database._sqlConnection);

                //OS
                Operating_System operating_system = new Operating_System();
                operating_system.readList(database._sqlConnection);

                //utils_cats_link
                Utils_Cats utils_cats = new Utils_Cats();
                utils_cats.readUtils_Cats_Links();

                //read the utils
                _utilities = new utilities();
                _utilities.readUtilsDB(database._sqlConnection);

                foreach (utility U in _utilities.utilitiesList)
                {
                    //update devices
                    U.devices = devices.getDevicesForID(U.util_id);
                    //update operating_system
                    U.operating_system = operating_system.getOsForId(U.util_id);

                    //categories
                    List<Utils_Cats_link> cat_ids = utils_cats.getCatsForUtil(U.util_id);
                    U._category_list.Clear();
                    List<Category> category_for_util = new List<Category>();
                    category_for_util = categories.getCategoryByUtil(U.util_id, categories.categories_list, utils_cats._utils_cats_links);

                    U._category_list = category_for_util;
                    _logger.log("Util: "+ U.util_id.ToString() + ": <LIST> " + Categories.asString(U._category_list));

                    ///U._categories = Categories.getCategoriesAsString(U._category_list);
                    ///_logger.log("Util: "+ U.util_id.ToString() + ": <string> " + U._categories);

                    //save changes back to utils table
                    ///U.saveCategoriesToDB();
                }


                source = new BindingSource();
                source.DataSource = _utilities.utilitiesList;
                dataGridView1.DataSource = source;

//                dataGridView1.DataSource = _utilities.utilitiesList;

                dataGridView1.Columns["id"].Visible = false;
                //dataGridView1.Columns["util_id"].Visible = true;
                try
                {
                    dataGridView1.Columns["file_data"].Visible = false; //we can not show binary data as image
                }
                catch (Exception) { }
                dataGridView1.Columns.Add("devices", "devices");
                
                dataGridView1.Columns["devices"].DataPropertyName = "devices";

                dataGridView1.Columns.Add("categories", "categories");
                dataGridView1.Columns["categories"].DataPropertyName = "_category_list";

                dataGridView1.Refresh();

                //utilities.Serialize(_utilities);
                //_utilities.setImageData(2, helper.getAppPath + @"\cpumon.zip", database._sqlConnection);

                //test
                //byte[] buf = _utilities.getFileData(2);
                _logger.log("database loaded and ready");
            }
            catch (Exception ex)
            {
                _logger.log("Exception in mainForm init: " + ex.Message);
                iRes = -1;
            }
            return iRes;
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
                frmFile frm = new frmFile(_utilities.getUtilityByID(util_id), ref _utilities);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    _utilities.setImageData(util_id, frm._utility.file_data, database._sqlConnection);
                    _utilities.setFile_Link(util_id, frm._utility.file_link, database._sqlConnection);
                    dataGridView1.Refresh();
                }
                frm.Dispose();
            }

            if (row.Cells[iColumn].OwningColumn.HeaderText == "operating_system")
            {
                frmOperatingSystems frm = new frmOperatingSystems(_utilities.getUtilityByID(util_id));
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    _utilities.setOperatingsystem(util_id, frm._utility.operating_system, database._sqlConnection);
                    dataGridView1.Refresh();
                }
                frm.Dispose();
            }

            if (row.Cells[iColumn].OwningColumn.HeaderText == "name")
            {
                frmName frm = new frmName(_utilities.getUtilityByID(util_id));
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    _utilities.setName(util_id, frm._utility.name , database._sqlConnection);
                    dataGridView1.Refresh();
                }
                frm.Dispose();
            }

            if (row.Cells[iColumn].OwningColumn.HeaderText == "devices")
            {
                frmDevices frm = new frmDevices(_utilities.getUtilityByID(util_id), ref _utilities);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    _utilities.setDevices(util_id, frm._devices, database._sqlConnection);
                    dataGridView1.Refresh();
                }
                frm.Dispose();
            }

            if (row.Cells[iColumn].OwningColumn.HeaderText.EndsWith("categories"))
            {
                utility U = _utilities.getUtilityByID(util_id);
                frmCategories frm = new frmCategories(ref U);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    //_utilities.setDevices(util_id, frm._devices, database._sqlConnection);
                    dataGridView1.Refresh();
                }
                frm.Dispose();
            }
        }

        private void mnuSaveXML_Click(object sender, EventArgs e)
        {
            if (_utilities == null || _utilities.utilitiesList.Count==0)
            {
                MessageBox.Show("no utilities to save");
                return;
            }
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.RestoreDirectory = true;
            ofd.CheckPathExists = true;
            ofd.OverwritePrompt=true;
            ofd.Filter = "xml files|*.xml|all files|*.*";
            ofd.FilterIndex = 0;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    utilities.Serialize(_utilities, ofd.FileName);
                    // Categories, Links, Devices, OS_Systems lists???
                }
                catch (Exception ex) {
                    MessageBox.Show("Exception saving file: " + ex.Message);
                }
            }

            ofd.Dispose();
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            if(_database!=null)
                _database.Dispose();
            Application.Exit();
        }

        private void mnuLoadXML_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.RestoreDirectory = true;
            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;
            ofd.Filter = "xml files|*.xml|all files|*.*";
            ofd.FilterIndex = 0;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _utilities = utilities.Deserialize(ofd.FileName);
                    dataGridView1.DataSource = _utilities.utilitiesList;
                    dataGridView1.Columns["id"].Visible = false;
                    dataGridView1.Columns["file_data"].Visible = false; //we can not show binary data as image

                    dataGridView1.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception reading file: " + ex.Message);
                }
            }
            ofd.Dispose();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            DataGridViewRow row = grid.Rows[e.RowIndex];
            DataGridViewColumn col = grid.Columns[e.ColumnIndex];
            if (row.DataBoundItem != null && col.DataPropertyName=="devices")// .Contains("."))
            {
                string[] props = col.DataPropertyName.Split('.');
                PropertyInfo propInfo = row.DataBoundItem.GetType().GetProperty("devices");
                object val = propInfo.GetValue(row.DataBoundItem, null);
                Device[] d = (Device[])val;
                string s = "";
                foreach (Device dev in d)
                    s += dev.name.Trim() + "+";
                e.Value = s.Trim(new char[]{'+'});
            }
        }

        private void btnNewUtility_Click(object sender, EventArgs e)
        {
            frmNewUtility frm = new frmNewUtility(ref _utilities);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //refresh
                source.ResetBindings(false);
                //dataGridView1.Refresh();
            }
            frm.Dispose();
        }

        class util_file
        {
            public string file;
            public utility u;
            public util_file(utility utl, string sFile)
            {
                u = utl;
                file = sFile;
            }
        }
        private void mnuUpdateAllFiles_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog ofd = new FolderBrowserDialog();
            ofd.ShowNewFolderButton = false;
            ofd.Description = "Select folder with zipped utilities";
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            string sPath = ofd.SelectedPath;
            
            string[] files_disk = System.IO.Directory.GetFiles(ofd.SelectedPath, "*.zip");

            List<util_file> files_db = new List<util_file>();
            foreach (utility u in _utilities.utilitiesList)
            {
                string sName = u.file_link;
                files_db.Add(new util_file(u, sName));
            }

            //compare and update
            foreach (util_file u in files_db)
            {
                if (u.file.Length == 0)
                    continue;
                foreach (string f in files_disk)
                {
                    if (f.EndsWith(u.file)){
                        ;//update file data
                        _logger.log("updating: " + u.file);
                        u.u.file_data = utilities.GetByteData(f);
                        _utilities.setImageData(u.u.id, u.u.file_data, database._sqlConnection);
                        //_utilities.setFile_Link(util_id, frm._utility.file_link, database._sqlConnection);

                    }
                }
            }
            dataGridView1.Refresh();
        }

    }
}
