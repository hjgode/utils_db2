using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

using System.Xml.Serialization;
using System.IO;

namespace utils_db2
{
    [XmlRoot("utilities")]
    public class utilities
    {
        [XmlElement("utilities")]
        public List<utility> utilitiesList { get { return _utilities; } set { _utilities = value; } }

        [XmlIgnore]
        List<utility> _utilities=new List<utility>();

        [XmlIgnore]
        public List<Device> _devicesList = new List<Device>();

        public utilities()
        {
            _utilities = new List<utility>();
        }

        public int readUtilsDB(SqlConnection conn)
        {
            utilitiesList.Clear();
            int iRet = 0;
            string sql = "Select id, name, description, author, file_link, file_data from utils";
            //sql = "SELECT     dbo.utils.id, dbo.utils.name, dbo.utils.description, dbo.utils.author, dbo.utils.file_link, dbo.utils.file_data, dbo.utils_device.name AS Device "+
            //      "FROM         dbo.utils LEFT OUTER JOIN "+
            //      "dbo.utils_device ON dbo.utils.id = dbo.utils_device.util_id";

            SqlCommand cmd = new SqlCommand(sql, conn);

            //load the util_id<->device table
            if (_devicesList == null)
                _devicesList = new List<Device>();
            _devicesList.Clear();
            Device devTemp = new Device();
            _devicesList = devTemp.readList(conn);
            
            //load the util_id<->OS Names table
            Operating_System _os_Class = new Operating_System();
            _os_Class.readList(conn);

            //read util_db
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SequentialAccess); // CommandBehavior.SequentialAccess: read columns in order and every column only once!
            Device[] uDevices;
            while (rdr.Read())
            {
                int utilID = rdr.GetInt32(0);
                string name = rdr.GetString(1).Trim();
                string desc = rdr.GetString(2).Trim();
                string author = rdr.GetString(3).Trim();
                string filelink = rdr.GetString(4).Trim();
                byte[] filedata = null;

                //find devices attached to this util
                uDevices = devTemp.getDevicesForID(utilID);
                if (uDevices == null || uDevices.Length == 0)
                    uDevices = new Device[] { new Device(utilID, "undefined") };

                //load the binary data
                if (rdr.IsDBNull(5)) //is there any binary data?
                    filedata = null;
                else
                {
                    //read binary data
                    int bufferSize = 100;                   // Size of the BLOB buffer.
                    byte[] outbyte = new byte[bufferSize];  // The BLOB byte[] buffer to be filled by GetBytes.
                    long retval;                            // The bytes returned from GetBytes.
                    long startIndex = 0;                    // The starting position in the BLOB output.
                    MemoryStream ms = new MemoryStream();
                    BinaryWriter bw = new BinaryWriter(ms);
                    // Reset the starting byte for the new BLOB.
                    startIndex = 0;
                    // Read the bytes into outbyte[] and retain the number of bytes returned.
                    retval = rdr.GetBytes(5, startIndex, outbyte, 0, bufferSize);
                    // Continue reading and writing while there are bytes beyond the size of the buffer.
                    while (retval == bufferSize)
                    {
                        bw.Write(outbyte);
                        bw.Flush();
                        // Reposition the start index to the end of the last buffer and fill the buffer.
                        startIndex += bufferSize;
                        retval = rdr.GetBytes(5, startIndex, outbyte, 0, bufferSize);
                    }
                    bw.Close();
                    filedata = ms.ToArray();
                    ms.Close();
                }

                //add a new utility to our class
                utilitiesList.Add(new utility(utilID,name,desc,author,filelink,
                    uDevices,
                    _os_Class.getOsForId(utilID),
                    filedata
                    ));
                iRet++;
            }
            rdr.Close();
            //cmd = new SqlCommand("Select id,name,description,author,file_link from utils", conn);
            //rdr = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

            return iRet;
        }

        public void setImageData(int uID, string filename, SqlConnection conn)
        {
            byte[] byteData = GetByteData(filename);
            string sql = "UPDATE utils set file_data=@parm1 WHERE id=" + uID.ToString()+";";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("parm1", SqlDbType.Image, byteData.Length).Value = byteData;
            int iRes = cmd.ExecuteNonQuery();

            foreach(utility u in _utilities){
                if (u.id == uID)
                    u.file_data = byteData;
            }
        }

        public void setImageData(int uID, byte[] bData, SqlConnection conn)
        {
            byte[] byteData = bData;
            string sql = "UPDATE utils set file_data=@parm1 WHERE id=" + uID.ToString()+";";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("parm1", SqlDbType.Image, byteData.Length).Value = byteData;
            int iRes = cmd.ExecuteNonQuery();

            foreach(utility u in _utilities){
                if (u.id == uID)
                    u.file_data = byteData;
            }
        }

        public void setFile_Link(int uID, string sName, SqlConnection conn)
        {            
            string sql = "UPDATE utils set file_link=@parm1 WHERE id=" + uID.ToString()+";";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("parm1", SqlDbType.Text, sName.Length).Value = sName;
            int iRes = cmd.ExecuteNonQuery();

            foreach(utility u in _utilities){
                if (u.id == uID)
                    u.file_link = sName;
            }
        }

        public int setName(int uID, string sName, SqlConnection conn)
        {
            int iRes = 0;
            string sql = "UPDATE utils set description=@parm1 WHERE id=" + uID.ToString() + ";";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("parm1", SqlDbType.Text, sName.Length).Value = sName;
            iRes = cmd.ExecuteNonQuery();
            
            foreach (utility u in _utilities)
                if (u.id == uID)
                    u.name = sName;

            cmd.Dispose();
            return iRes;
        }

        public int setDevices(int uID, Device[] devs, SqlConnection conn)
        {
            int iRes = 0;
            //read all from utils_device with uID
            SqlCommand cmd = new SqlCommand("SELECT util_id,name from utils_device WHERE util_id=" + uID.ToString(), conn);
            //iRes = cmd.ExecuteNonQuery();
            SqlDataReader dr = cmd.ExecuteReader();
            List<Device> dbDevicesForID = new List<Device>();
            while (dr.Read())
            {
                dbDevicesForID.Add(new Device(uID, dr["name"].ToString().Trim()));
            }
            dr.Close();

            List<Device> devToAdd = devs.ToList<Device>();
            foreach (Device db in dbDevicesForID)
            {
                foreach (Device dNew in devToAdd)
                    if (dNew.name.Equals(db.name))
                        devToAdd.Remove(dNew);
            }
            //add all devs with uID
            iRes = 0;
            foreach (Device d in devToAdd)
            {
                cmd.Parameters.Clear();
                cmd.Dispose();
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO utils_device (util_id, name) VALUES (@uid1, @name);";
                cmd.Parameters.Add("uid1", SqlDbType.Int, sizeof(int)).Value = d.util_id;
                cmd.Parameters.Add("name", SqlDbType.Text, d.name.Length).Value = d.name;
                iRes += cmd.ExecuteNonQuery();
            }
            cmd.Dispose();

            _devicesList.Clear();
            Device devTemp = new Device();
            _devicesList = devTemp.readList(conn);

            //publish the new list to the object
            foreach (utility u in _utilities)
            {
                if (u.id == uID)
                    u.devices = devs;
            }
            return iRes;
        }

        public void setOperatingsystem(int uID, Operating_System os, SqlConnection conn)
        {            
            //is util_id already listed?
            int iRes = 0;
            string sql = "Select * from utils_operating_systems WHERE utils_id=" + uID.ToString()+";";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {

                cmd.CommandText = "UPDATE utils_operating_systems set name=@parm1 WHERE utils_id=" + uID.ToString() + ";";
                cmd.Parameters.Add("parm1", SqlDbType.Text, os.name.Length).Value = os.name;
                rdr.Close();
                iRes = cmd.ExecuteNonQuery();
            }
            else
            {
                cmd.CommandText = "INSERT INTO utils_operating_systems (utils_id, name) VALUES (@parm1, @parm2)" + ";";
                cmd.Parameters.Add("parm1", SqlDbType.Int, sizeof(int)).Value = uID;
                cmd.Parameters.Add("parm2", SqlDbType.Text, os.name.Length).Value = os.name;
                rdr.Close();
                iRes = cmd.ExecuteNonQuery();
            }
            cmd.Dispose();

            //publish the new list to the object
            foreach (utility u in _utilities)
            {
                if (u.id == uID)
                    u.operating_system = os;
            }
        }

        public static byte[] GetByteData(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            byte[] photo = br.ReadBytes((int)fs.Length);

            br.Close();
            fs.Close();

            return photo;
        }

        public byte[] getFileData(int uID){
            foreach(utility u in _utilities){
                if (u.id == uID)
                    return u.file_data;
            }
            return null;
        }

        public utility getUtilityByID(int id)
        {
            foreach (utility utl in _utilities)
                if (utl.id == id)
                    return utl;
            return null;
        }

        public int updateDescription(utility utl, SqlConnection conn)
        {
            string sql = "UPDATE utils set description=@parm1 WHERE id=" + utl.id.ToString() + ";";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("parm1", SqlDbType.Text, utl.description.Length).Value = utl.description;
            int iRes = cmd.ExecuteNonQuery();
            foreach (utility u in _utilities)
                if (u.id == utl.id)
                    u.description = utl.description;

            return iRes;
        }

        public int updateAuthor(utility utl, SqlConnection conn)
        {
            string sql = "UPDATE utils set author=@parm1 WHERE id=" + utl.id.ToString() + ";";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("parm1", SqlDbType.Char, utl.author.Length).Value = utl.author;
            int iRes = cmd.ExecuteNonQuery();
            foreach (utility u in _utilities)
                if (u.id == utl.id)
                    u.author = utl.author;

            return iRes;
        }

        static public void Serialize(utilities _utils, string sFile)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(utilities));
            using (TextWriter writer = new StreamWriter(sFile))
            {
                serializer.Serialize(writer, _utils);
            }
        }

        static public utilities Deserialize(string fileName)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(utilities));
            TextReader reader = new StreamReader(fileName);
            object obj = deserializer.Deserialize(reader);
            utilities XmlData = (utilities)obj;
            reader.Close();
            return XmlData;
        }
    }

}
