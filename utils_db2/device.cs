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
        [XmlElement("util_id")]
        public int util_id { get; set; }
        [XmlElement("name")]
        public string name { get; set; }

        public Device(int utilID, string n)
        {
            util_id = utilID;
            name = n;
        }
        public Device()
        {
            name = "undefined";
            util_id = -1;
        }
        public Device(string devName)
        {
            name = devName;
            util_id = -1;
        }

        public override string ToString()
        {
            return util_id.ToString()+ ":" + name;
        }

        public static Device addNewDevice2DB(string name, SqlConnection conn)
        {
            Device dev = new Device(name);

            SqlCommand cmd = new SqlCommand("INSERT INTO [utils_device] (util_id, name) VALUES (-1, @PARM1) "+
                " SELECT SCOPE_IDENTITY() As TheId;", conn);
            cmd.Parameters.Add("PARM1", SqlDbType.Text, name.Length).Value = name;
            cmd.Connection = conn;
            object o = cmd.ExecuteScalar();

            //if (o != null)
            //{
            //    int iRet = Convert.ToInt32(o);
            //    dev.device_id = iRet;
            //    cmd.CommandText = "UPDATE utils_device set device_id=@did WHERE name=@name;";
            //    cmd.Parameters.Clear();
            //    cmd.Parameters.Add("did", SqlDbType.Int).Value = iRet;
            //    cmd.Parameters.Add("name", SqlDbType.Text).Value = name;
            //    iRet = cmd.ExecuteNonQuery();
            //}
            //else
            //    dev.device_id = -1;

            Program._logger.log("addNewDevice2DB: " + dev.ToString());

            _lstDevices.Add(dev);
            return dev;
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

            SqlCommand cmd = new SqlCommand("select util_id, name from [utils_device];", conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            _lstDevices.Clear();
            while (rdr.Read())
            {
                _lstDevices.Add(new Device(rdr.GetInt32(0), rdr.GetString(1).Trim()));
            }
            rdr.Close();
            return _lstDevices;
        }

        /// <summary>
        /// read utils->devices link table
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static List<Device> readListUnique(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand("select DISTINCT name from [utils_device];", conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            List<Device> devs = new List<Device>();
            while (rdr.Read())
            {
                devs.Add(new Device(rdr.GetString(0).Trim()));
            }
            rdr.Close();
            return devs;
        }

        public Device deviceName(int uID)
        {
            Device _dev = new Device(-1, "undefined");
            if (_lstDevices.Count > 0)
            {
                foreach (Device d in _lstDevices)
                {
                    if (d.util_id == uID)
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
}
