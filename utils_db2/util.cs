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
        public utilities()
        {
            _utilities = new List<utility>();
        }
        public int addUtil(utility ut)
        {
            int iRet = 0;
            utilitiesList.Add(ut);
            return iRet;
        }

        public int readUtilsDB(SqlConnection conn)
        {
            utilitiesList.Clear();
            int iRet = 0;
            string sql = "Select id, name, description, author, file_link, file_data from utils";
            SqlCommand cmd = new SqlCommand(sql, conn);

            Devices _devicesClass = new Devices();
            _devicesClass.readList(conn);

            Operating_System _os_Class = new Operating_System();
            _os_Class.readList(conn);

            SqlDataReader rdr = cmd.ExecuteReader();
            Devices[] uDevices;
            while (rdr.Read())
            {
                int utilID = rdr.GetInt32(0);
                string name = rdr.GetString(1).Trim();
                string desc = rdr.GetString(2).Trim();
                string author = rdr.GetString(3).Trim();
                string filelink = rdr.GetString(4).Trim();
                byte[] filedata = null;

                uDevices = _devicesClass.getDevicesForID(utilID);
                if (uDevices == null || uDevices.Length == 0)
                    uDevices = new Devices[] { new Devices(0, utilID) };


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
                utilitiesList.Add(new utility(utilID,name,desc,author,filelink,
                    uDevices,
                    _os_Class.getOsForId(utilID),
                    filedata
                    ));
                /*
                utilitiesList.Add(new utility(
                    rdr.GetInt32(0),//id
                    rdr.GetString(1).Trim(),//name
                    rdr.GetString(2).Trim(),//description
                    rdr.GetString(3).Trim(),//author
                    rdr.GetString(4).Trim(),//file_link
                    uDevices,               //extern table devices
                    _os_Class.getOsForId(rdr.GetInt32(0)),//extern table operating system
                    getImageBytes(ref rdr, 5)
                    ));
                */
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

        static byte[] GetByteData(string filePath)
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

        static public void Serialize(utilities _utils)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(utilities));
            using (TextWriter writer = new StreamWriter(helper.getAppPath+ @"utils.xml"))
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

    public class utility
    {
        [XmlElement("id")]
        public int id { get; set; }
        [XmlElement("name")]
        public string name { get; set; }
        [XmlElement("description")]
        public string description { get; set; }
        [XmlElement("author")]
        public string author { get; set; }
        [XmlElement("file_link")]
        public string file_link { get; set; }
        [XmlElement("devices")]
        public Devices[] devices { get; set; }
        [XmlElement("operating_system")]
        public Operating_System operating_system { get; set; }
        
        [XmlElement("file_data")]
        public byte[] file_data { get; set; }

        public utility()
        {
            id = -1;
            name = "undefined";
            description = "undefined";
            author = "undefined";
            file_link = "undefined";
            devices = new Devices[]{new Devices()};
            operating_system = new Operating_System();
            file_data = null;
        }
        public utility(int i, string n, string desc, string a, string f, Devices[] devs, Operating_System os, byte[] bData)
        {
            id = i;
            name = n;
            description=desc;
            author = a;
            file_link = f;
            devices = devs;
            operating_system = os;
            file_data = bData;
        }
        public utility(int i, string n, string desc, string a, string f)
        {
            id = i;
            name = n;
            description=desc;
            author = a;
            file_link = f;
        }

        public byte[] getFileData()
        {
            return this.file_data;
        }
    }    
}
