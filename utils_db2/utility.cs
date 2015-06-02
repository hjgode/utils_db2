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
        public Device[] devices { get; set; }
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
            //build table of utils_id and device names
            devices = new Device[] { new Device() };

            operating_system = new Operating_System();
            file_data = null;
        }

        public utility(int i, string n, string desc, string a, string f, Device[] devs, Operating_System os, byte[] bData)
        {
            id = i;
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
            name = n;
            description = desc;
            author = a;
            file_link = f;
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

    }
}
