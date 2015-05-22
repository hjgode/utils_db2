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
        utilities _utilities;
        public frmDevices(utility utl, ref utilities utls)
        {
            InitializeComponent();
            _utility = utl;
            _utilities = utls;

            label1.Text = _utility.name;
            
            fillUtlitiesListbox();

            readDevsForUtil();
        }

        void fillUtlitiesListbox()
        {
            listBoxAvailableDevices.Items.Clear();
            foreach (utility u in _utilities.utilitiesList)
                listBoxAvailableDevices.Items.Add(u);
        }
        void readDevsForUtil()
        {
            listBoxDevices.Items.Clear();
            foreach (Devices d in _utility.devices)
                listBoxDevices.Items.Add(d);
        }
    }
}
