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

        public mainForm()
        {
            InitializeComponent();


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
            db = new database(connect, false);
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

        void readDataThread()
        {
            Invoke(new Action(() => toolStripStatusLabel2.Text = "loading..."));
            _utilities.readUtilsDB(database._sqlConnection);

            Invoke(new Action(()=> lbUtilities.DataSource = _utilities.utilitiesList));// db._bsUtils.DataSource;
            //lbUtilities.DisplayMember = "name";
            Invoke(new Action(()=> lbUtilities.Refresh()));

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
    }
}
