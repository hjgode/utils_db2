using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using utils_db2;

namespace TechSupport_Utilities
{
    public partial class mainForm : Form
    {
        helper.ConnectTest connectTest=null;

        database db=null;
        utilities _utilities;

        categories _categories=new categories();

        public mainForm()
        {
            InitializeComponent();

            lbUtilities.SelectionMode = SelectionMode.MultiExtended;

            connectTest = new helper.ConnectTest();
            connectTest.updateEvent += new helper.ConnectTest.updateEventHandler(connectTest_updateEvent);
            System.Net.IPAddress ip= helper.getIP();
            if (ip != System.Net.IPAddress.Loopback)
            {
                toolStripStatusLabel1.Text = ip.ToString();
            }
            else
            {
                toolStripStatusLabel1.Text = "offline";
            }

        }

        void db_updateEvent(object sender, database.MyEventArgs eventArgs)
        {
            if (eventArgs.bIsOpen)
                toolStripStatusLabel2.Text = "online";
            else
                toolStripStatusLabel2.Text = "offline";
            System.Diagnostics.Debug.WriteLine("sqlserver: " + eventArgs.state.ToString());
        }

        void connectTest_updateEvent(object sender, helper.ConnectTest.MyEventArgs eventArgs)
        {
            if(eventArgs.bSuccess)
                toolStripStatusLabel1.Text = "online";
            else
                toolStripStatusLabel1.Text = "offline";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (connectTest != null)
                connectTest.Dispose();
            if (db != null)
                db.Dispose();
        }

        private void mnuConnect_Click(object sender, EventArgs e)
        {
            animateProgress();

            System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ThreadStart(thStartConnect));
            th.Start();
            toolStripStatusLabel2.Text = "connecting...";
        }

        void thStartConnect()
        {
            string connect = Properties.Settings.Default.connectstring;
            try
            {
                db = new database(connect, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to connect to server.\n" + ex.Message);
                return;
            }
            db.updateEvent += new database.updateEventHandler(db_updateEvent);
            if (db._bConnected)
                Invoke(new Action(() => toolStripStatusLabel2.Text = "connected"));
            else
                Invoke(new Action(() => toolStripStatusLabel2.Text = "---"));

            Invoke(new Action(() => updateData())); //starts another thread
        }

        void updateData()
        {
            if (db._bConnected)
            {
                lbUtilities.Items.Clear();
                
                _utilities = new utilities();
                Cursor.Current = Cursors.WaitCursor;

                System.Threading.Thread thLoadData = new System.Threading.Thread(new System.Threading.ThreadStart(readDataThread));
                thLoadData.Start();
            }
        }

        /// <summary>
        /// read data by DB
        /// </summary>
        void readDataThread()
        {
            Invoke(new Action(() => toolStripStatusLabel2.Text = "loading..."));
            _utilities.readUtilsDB(database._sqlConnection);

            Invoke(new Action(()=> lbUtilities.DataSource = _utilities.utilitiesList));// db._bsUtils.DataSource;
            //lbUtilities.DisplayMember = "name";
            Invoke(new Action(()=> lbUtilities.Refresh()));

            //read categories
            _categories.readCatsFromDB(database._sqlConnection);
            Invoke(new Action(() => lbCategories.DataSource = _categories.categories_list));

            Cursor.Current = Cursors.Default;
            Invoke(new Action(() => toolStripStatusLabel2.Text = lbUtilities.Items.Count.ToString()));

            stopAnimate();
        }

        static int iProg=0;
        Timer timer = new Timer();
        void animateProgress()
        {
            iProg = 0;
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer_Tick);
            toolStripProgressBar1.Maximum = 100;
            timer.Enabled = true;
        }
        void stopAnimate()
        {
            timer.Enabled = false;
            Invoke (new Action(()=> toolStripProgressBar1.Value = 0));
        }
        void timer_Tick(object sender, EventArgs e)
        {
            stepProgress(10);
        }
        void stepProgress(int step)
        {
            iProg += step;
            if (iProg >= 100)
                iProg = 0;
            Invoke(new Action(() => toolStripProgressBar1.Value = iProg));
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int iCnt = 0;
            for (int i=0; i<lbUtilities.Items.Count; i++){
                utility U=(utility)lbUtilities.Items[i];
                if (U.description.ToLower().Contains(txtSearch.Text.ToLower()))
                {
                    lbUtilities.SetSelected(i, true);
                    iCnt++;
                }
                else
                    lbUtilities.SetSelected(i, false);                  
            }
            tsLblResult.Text = "found " + iCnt.ToString() + " items";
        }

        private void lbUtilities_DoubleClick(object sender, EventArgs e)
        {
            if (lbUtilities.SelectedIndex == -1){
                txtDescription.Text = "";
                return;
            }

            var list = (ListBox)sender;

            int itemIndex = list.IndexFromPoint(((MouseEventArgs)e).Location);
            if (itemIndex != -1)
            {
                // This is your double clicked item
                utility item = (utility)list.Items[itemIndex];
                txtDescription.Text = item.description;
            }
        }

        private void lbUtilities_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbUtilities.SelectedIndex == -1)
                return;
            utility item = (utility)lbUtilities.Items[lbUtilities.SelectedIndex];
            txtDescription.Text = item.description;
            //get file?
            if (item.file_link.Length > 0)
            {
                panelFile.Visible = true;
                lblFilename.Text = item.file_link;
            }
            else
            {
                panelFile.Visible = false;
                lblFilename.Text = "";
            }
                //byte[] data = _utilities.getFileData(item.util_id);
        }

        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            if (lbUtilities.SelectedIndex == -1)
                return;
            utility item = (utility)lbUtilities.Items[lbUtilities.SelectedIndex];
            byte[] data = _utilities.getFileData(item.util_id);
            if (data != null)
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.RestoreDirectory = true;
                ofd.CheckPathExists = true;
                ofd.OverwritePrompt = true;
                ofd.Filter = "zip files|*.zip|all files|*.*";
                ofd.FilterIndex = 0;
                ofd.FileName = item.file_link;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        item.writeFileData(ofd.FileName);
                        MessageBox.Show("Saved " + data.Length.ToString() + " bytes to '" + ofd.FileName + "'");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Exception writing file: " + ex.Message);
                    }
                }
                ofd.Dispose();
            }
            else
            {
                MessageBox.Show("No data");
            }
        }

        private void lbCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbCategories.SelectedIndex == -1)
                return;
            category this_cat = (category) lbCategories.SelectedItem;
            txtCatDescription.Text = this_cat.description;
            lbUtilitiesByCategory.DataSource = this_cat.readUtilsFromDB(database._sqlConnection);
            lbUtilitiesByCategory.Refresh();
        }

        private void lbUtilitiesByCategory_DoubleClick(object sender, EventArgs e)
        {
            if (lbUtilitiesByCategory.SelectedIndex == -1 || lbUtilities.Items.Count==0)
                return;
            string sUtil = (string)lbUtilitiesByCategory.SelectedItem;
            //find utility in lbUtilities and select it
            int iFound = -1;
            for (int i = 0; i < lbUtilities.Items.Count; i++)
            {
                utility u=(utility)lbUtilities.Items[i];
                if (u.name.Trim() == sUtil)
                {
                    iFound = i;
                    lbUtilities.SelectedItems.Clear();
                    lbUtilities.SelectedIndex = iFound;
                    tabControl1.SelectedTab = tabUtilities;
                    return;
                }
            }
        }
    }
}
