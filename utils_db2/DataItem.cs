using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;

namespace utils_db2
{
    [Serializable]
    class DataItem
    {
        public int id { get; set; }
        public string name { set; get; }
        public Device[] devices { get; set; }
        public Operating_System operating_systsm { get; set; }
        public string description { get; set; }
        public string author;
        public byte[] file_link;
    }


    /// <summary>
    /// to store a generic category like
    /// network
    /// battery
    /// general
    /// </summary>
    class category
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
