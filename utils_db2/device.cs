using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Xml.Serialization;

namespace utils_db2
{
    public class Device
    {
        [XmlElement("device_id")]
        public int device_id { get; set; }
        [XmlElement("name")]
        public string name { get; set; }

        public Device(int i, string n)
        {
            device_id = i;
            name = n;
        }
        public Device()
        {
            name = "undefined";
            device_id = -1;
        }
        public override string ToString()
        {
            return name;
        }
        List<Device> _lstDevices = new List<Device>();

        /// <summary>
        /// read utils->devices link table
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public List<Device> readList(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand("select device_id,name from [utils_device];", conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            _lstDevices.Clear();
            while (rdr.Read())
            {
                _lstDevices.Add(new Device(rdr.GetInt32(0), rdr.GetString(1).Trim()));
            }
            rdr.Close();
            return _lstDevices;
        }

        public Device deviceName(int id)
        {
            Device _dev = new Device(-1,"undefined");
            if (_lstDevices.Count > 0)
            {
                foreach (Device d in _lstDevices)
                {
                    if (d.device_id == id)
                        _dev = d;
                }
            }
            return _dev;
        }
    }

    public class Devices
    {
        [XmlElement("utils_id")]
        /// <summary>
        /// link to utils.id
        /// </summary>
        public int utils_id { get; set; }

        [XmlElement("device_id")]
        /// <summary>
        /// link to Device->device_id
        /// </summary>
        public int device_id { get; set; }

        public Devices(int device_device_id, int util_devices_id)
        {
            device_id= device_device_id;
            utils_id = util_devices_id;
        }
        public Devices()
        {
            device_id = 0;
            utils_id = 0;
        }

        List<Devices> _lstDevices = new List<Devices>();

        /// <summary>
        /// read utils->devices link table
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public List<Devices> readList(SqlConnection conn)
        {
            //read list of id and name
            Device _dev= new Device();
            List<Device> lstDeviceNames = _dev.readList(conn);

            SqlCommand cmd = new SqlCommand("select utils_id,device_id from [utils_devices];", conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            _lstDevices.Clear();
            while (rdr.Read())
            {
                _lstDevices.Add(new Devices(rdr.GetInt32(0), rdr.GetInt32(1)));
            }
            rdr.Close();
            return _lstDevices;
        }

        public Devices[] getDevicesForID(int utilID)
        {
            List<Devices> devs = new List<Devices>();
            if (_lstDevices.Count > 0)
            {
                foreach (Devices d in _lstDevices)
                {
                    if (d.utils_id == utilID)
                        devs.Add(new Devices(d.device_id, d.utils_id));
                }
            }
            return devs.ToArray();
        }


    }
}
