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
        [XmlElement("util_id")]
        public int util_id { get; set; }

        public Device(int utilID, int devID, string n)
        {
            util_id = utilID;
            device_id = devID;
            name = n;
        }
        public Device()
        {
            name = "undefined";
            device_id = -1;
            util_id = -1;
        }

        public override string ToString()
        {
            return name;
        }
        
        public static List<Device> _lstDevices = new List<Device>();

        /// <summary>
        /// read utils->devices link table
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public List<Device> readList(SqlConnection conn)
        {
            if (_lstDevices.Count > 0)
                return _lstDevices;

            SqlCommand cmd = new SqlCommand("select util_id, device_id, name from [utils_device];", conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            _lstDevices.Clear();
            while (rdr.Read())
            {
                _lstDevices.Add(new Device(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetString(2).Trim()));
            }
            rdr.Close();
            return _lstDevices;
        }

        public Device deviceName(int id)
        {
            Device _dev = new Device(-1,-1,"undefined");
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

        public Device[] getDevicesForID(int uID)
        {
            List<Device> lst = new List<Device>();
            foreach (Device d in _lstDevices)
            {
                if (d.util_id == uID)
                    lst.Add(d);
            }
            return lst.ToArray();
        }
    }

    /// <summary>
    /// maintain all devices in linking database
    /// </summary>
    //public class Devices
    //{
    //    SqlConnection _conn { get; set; }

    //    public Devices(SqlConnection conn)
    //    {
    //        _conn = conn;
    //        readList(_conn);
    //    }

    //    public List<Device> _lstDevices = new List<Device>();

    //    /// <summary>
    //    /// read utils->devices link table
    //    /// </summary>
    //    /// <param name="conn"></param>
    //    /// <returns></returns>
    //    List<Device> readList(SqlConnection conn)
    //    {
    //        //read list of id and name
    //        //Device _dev= new Device();
    //        //List<Device> lstDeviceNames = _dev.readList(conn);

    //        SqlCommand cmd = new SqlCommand("select util_id, device_id, name from [utils_device];", conn);
    //        SqlDataReader rdr = cmd.ExecuteReader();
    //        _lstDevices.Clear();
    //        while (rdr.Read())
    //        {
    //            int uID = -1, devID = -1; string n = "undef";
    //            try { uID = rdr.GetInt32(0); }
    //            catch (Exception) { }
    //            try { devID = rdr.GetInt32(1); }
    //            catch (Exception) { }
    //            try { n = rdr.GetString(2).Trim(); }
    //            catch (Exception) { }

    //            _lstDevices.Add(new Device(uID, devID, n));
    //        }
    //        rdr.Close();
    //        return _lstDevices;
    //    }

    //    public Device[] getDevicesForID(int utilID)
    //    {
    //        List<Device> devs = new List<Device>();
    //        if (_lstDevices.Count == 0)
    //            _lstDevices = readList(_conn);

    //        if (_lstDevices.Count > 0)
    //        {
    //            foreach (Device d in _lstDevices)
    //            {
    //                if (d.util_id == utilID)
    //                    devs.Add(new Device(d.util_id, d.device_id, d.name));
    //            }
    //        }
    //        return devs.ToArray();
    //    }
    //    public override string ToString()
    //    {
    //        string s = "";
    //        foreach (Device d in _lstDevices)
    //            s += d.name+" ";
    //        return s.Trim();
    //    }

    //}
}
