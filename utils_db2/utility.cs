using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;
using System.IO;

namespace utils_db2
{
    public class utility
    {
        myLogger.logger _logger = Program._logger;

        [XmlElement("id")]
        public int id { get; set; }
        [XmlElement("util_id")]
        public int util_id { get; set; }

        [XmlElement("name")]
        public string name { get; set; }
        [XmlElement("description")]
        public string description { get; set; }
        [XmlElement("author")]
        public string author { get; set; }
        [XmlElement("file_link")]
        public string file_link { get; set; }
        [XmlElement("devices")]
        public Device[] devices { get; set; }
        [XmlElement("operating_system")]
        public Operating_System operating_system { get; set; }

        [XmlElement("file_data")]
        public byte[] file_data { get; set; }

        [XmlElement("categories")]
        public List<category> _category_list=new List<category>();

        public utility()
        {
            id = -1;
            util_id = -1;
            name = "undefined";
            description = "undefined";
            author = "undefined";
            file_link = "undefined";
            //build table of utils_id and device names
            devices = new Device[] { new Device() };

            operating_system = new Operating_System();
            file_data = null;
        }

        public utility(int i, string n, string desc, string a, string f, Device[] devs, Operating_System os, byte[] bData)
        {
            id = i;
            util_id = i;
            name = n;
            description = desc;
            author = a;
            file_link = f;
            devices = devs;
            operating_system = os;
            file_data = bData;
        }

        public utility(int i, string n, string desc, string a, string f)
        {
            id = i;
            util_id = i;
            name = n;
            description = desc;
            author = a;
            file_link = f;
            file_data = null;
        }

        public byte[] getFileData()
        {
            return this.file_data;
        }

        public void setFileData(Byte[] bData)
        {
            this.file_data = bData;
        }

        public override string ToString()
        {
            return this.name;
        }

        public int writeFileData(string sFilename)
        {
            int iRet = -1;
            try
            {
                FileStream fs = new FileStream(sFilename, FileMode.CreateNew);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(this.file_data);
                bw.Flush();
                fs.Flush();
                bw.Close();
                fs.Close();
                iRet = 0;
            }
            catch (IOException ex)
            {
                _logger.log("IOException in writeFileData: " + ex.Message);
            }
            catch (Exception ex)
            {
                _logger.log("Exception in writeFileData: " + ex.Message);
            }
            return iRet;
        }

        public int readCategoriesForUtil()
        {
            int iCnt = 0;
            _category_list.Clear();
            categories cats= new categories();
            cats.readCatsFromDB(database._sqlConnection);
            foreach (category C in cats.categories_list)
            {
                if (C.util_ids.Contains(this.util_id.ToString()))
                    _category_list.Add(C);
            }
            return iCnt;
        }
    }
}
