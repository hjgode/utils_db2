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
        public int operating_id = 0;
        public string name = "undefined";

        public OS_name()
        {
        }

        public OS_name(int os_id, string n)
        {
            operating_id = os_id;
            name = n;
        }
        List<OS_name> _OS_names = new List<OS_name>();
        public List<OS_name> readList(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand("select operating_id,name from [utils_operating_systems];", conn);
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
                    if (_OS_names[i].operating_id == op_id)
                    {
                        return _OS_names[i].name;
                    }
                }
            }
            return "";
        }
    }

}
