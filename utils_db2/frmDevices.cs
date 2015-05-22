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
        public Device[] _devices;

        public frmDevices(utility utl, ref utilities utls)
        {
            InitializeComponent();

            _utility = utl;
            _utilities = utls;

            label1.Text = _utility.name;
            
            fillDevicesListbox();

            readDevsForUtil();
        }

        void fillDevicesListbox()
        {
            listBoxAvailableDevices.Items.Clear();
            foreach (Device d in _utilities._devicesList)
                listBoxAvailableDevices.Items.Add(d);
        }
        void readDevsForUtil()
        {
            listBoxDevices.Items.Clear();
            List<Device> dList = new List<Device>();
            foreach (Device d in _utility.devices)
            {
                listBoxDevices.Items.Add(d);
                dList.Add(d);
            }
            _devices = dList.ToArray();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (listBoxDevices.Items.Count == 0)
                _devices = new Device[] { new Device(_utility.id, -1, "undefined") };
            else
            {
                List<Device> lst=new List<Device>();
                foreach (var v in listBoxDevices.Items){
                    Device d=(Device)v;
                    lst.Add(new Device(_utility.id, d.device_id, d.name));
                }
                _devices = lst.ToArray();
            }
            DialogResult = DialogResult.OK;
        }
    }
}
