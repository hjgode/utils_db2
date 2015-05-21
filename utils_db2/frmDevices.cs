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
    public partial class frmDevices : Form
    {
        public utility _utility;
        public frmDevices(utility utl)
        {
            InitializeComponent();
            _utility = utl;
            label1.Text = _utility.name;

        }
    }
}
