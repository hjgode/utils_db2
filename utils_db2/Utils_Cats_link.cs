using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;
using System.Data.SqlClient;

namespace utils_db2
{
    public class Utils_Cats_link
    {
        [XmlElement("id")]
        public int id { get; set; }
        [XmlElement("util_id")]
        public int util_id { get; set; }
        [XmlElement("cat_id")]
        public int cat_id { get; set; }

        public Utils_Cats_link()
        {
            id = -1;
            util_id = -1;
            cat_id = -1;
        }
        public Utils_Cats_link(int ID, int UTIL_ID, int CAT_ID)
        {
            id = ID;
            util_id = UTIL_ID;
            cat_id = CAT_ID;
        }
    }
    public class Utils_Cats
    {
        [XmlElement("utils_cats_links")]
        public List<Utils_Cats_link> _utils_cats_links{get;set;}

        public Utils_Cats()
        {
            _utils_cats_links = new List<Utils_Cats_link>();
        }
        public int readUtils_Cats_Links()
        {
            int iRet = 0;
            int iID, iUTIL, iCAT;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = database._sqlConnection;
            cmd.CommandText = "SELECT id, util_id, cat_id from [utils_cats_link];";
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                iID = rdr.GetInt32(0);
                iUTIL = rdr.GetInt32(1);
                iCAT = rdr.GetInt32(2);
                _utils_cats_links.Add(new Utils_Cats_link(iID, iUTIL, iCAT));
                iRet++;
            }
            rdr.Close();
            cmd.Dispose();

            return iRet;
        }

    }
}
