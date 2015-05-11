using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace utils_db2
{
    public class util
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string author { get; set; }
        public string file_link { get; set; }

        public Devices[] devices { get; set; }
        public Operating_System operating_system { get; set; }

        public util()
        {
            id = -1;
            name = "undefined";
            description = "undefined";
            author = "undefined";
            file_link = "undefined";
            devices = null;
            operating_system = new Operating_System();
        }
        public util(int i, string n, string desc, string a, string f, Devices[] devs, Operating_System os)
        {
            id = i;
            name = n;
            description=desc;
            author = a;
            file_link = f;
            devices = devs;
            operating_system = os;
        }
        public util(int i, string n, string desc, string a, string f)
        {
            id = i;
            name = n;
            description=desc;
            author = a;
            file_link = f;
        }
        public List<util> utilities = new List<util>();

        public int addUtil(util ut)
        {
            int iRet = 0;
            utilities.Add(ut);
            return iRet;
        }

        public int readUtilsDB(SqlConnection conn)
        {
            utilities.Clear();
            int iRet = 0;
            string sql = "Select id,name,description,author,file_link from utils";
            SqlCommand cmd = new SqlCommand(sql, conn);

            Devices _devicesClass=new Devices();
            _devicesClass.readList(conn);

            Operating_System _os_Class=new Operating_System();
            _os_Class.readList(conn);

            SqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                utilities.Add(new util(
                    rdr.GetInt32(0),//id
                    rdr.GetString(1),//name
                    rdr.GetString(2),//description
                    rdr.GetString(3),//author
                    rdr.GetString(4),//file_link
                    _devicesClass.getDevicesForID(rdr.GetInt32(0)),
                    _os_Class.getOsForId(rdr.GetInt32(0))
                    ));
                iRet++;
            }
            rdr.Close();
            return iRet;
        }
    }    
}
