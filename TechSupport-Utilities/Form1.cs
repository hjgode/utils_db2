using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TechSupport_Utilities
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
