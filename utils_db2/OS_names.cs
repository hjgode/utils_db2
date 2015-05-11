using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace utils_db2
{
    class OS_name
    {
        public int id = 0; //auto!
        public int utils_id = 0;
        public string name = "undefined";

        public OS_name()
        {
        }

        public OS_name(int util_id, string n)
        {
            utils_id = util_id;
            name = n;
        }
        
        List<OS_name> _OS_names = new List<OS_name>();

        public List<OS_name> readList(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand("select utils_id,name from [utils_operating_systems];", conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            _OS_names.Clear();
            while (rdr.Read())
            {
                _OS_names.Add(new OS_name(rdr.GetInt32(0), rdr.GetString(1)));
            }
            rdr.Close();            
            return _OS_names;
        }

        public override string ToString()
        {
            return this.name;
        }

        public string getOSforID(int op_id)
        {
            if (_OS_names != null && _OS_names.Count > 0)
            {
                for (int i = 0; i < _OS_names.Count; i++)
                {
                    if (_OS_names[i].utils_id == op_id)
                    {
                        return _OS_names[i].name;
                    }
                }
            }
            return "";
        }
    }

    /// <summary>
    /// Windows CE 5
    /// Windows Mobile 6
    /// Windows Embedded Handheld 6.5
    /// </summary>
    public class Operating_System
    {
        public int id { get; set; }
        public string name { get; set; }
        public Operating_System()
        {
            id = -1;
            name = "undefined";
        }
        public Operating_System(int i, string n)
        {
            id = i;
            name = n;
        }

        public override string  ToString()
        {
            return name;
        }

        List<Operating_System> _operatingSystemsList = new List<Operating_System>();

        public List<Operating_System> readList(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand("select utils_id,name from [utils_operating_systems];", conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            _operatingSystemsList.Clear();
            while (rdr.Read())
            {
                _operatingSystemsList.Add(new Operating_System(rdr.GetInt32(0), rdr.GetString(1)));
            }
            rdr.Close();
            return _operatingSystemsList;
        }

        public Operating_System getOsForId(int id)
        {
            foreach (Operating_System o in _operatingSystemsList)
                if (o.id == id)
                    return o;
            return new Operating_System();
        }
    }

}
