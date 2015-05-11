using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace utils_db2
{
    public class descriptionText
    {
        public int util_id = 0;
        public string txt = "";
        public descriptionText(int id, string t)
        {
            txt = t;
            util_id = id;
        }

        public descriptionText()
        {
        }

        public override string ToString()
        {
            return txt;
        }

        List<descriptionText> _descriptions = new List<descriptionText>();

        public List<descriptionText> readList(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand("select id, description from [utils];", conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            _descriptions.Clear();
            while (rdr.Read())
            {
                _descriptions.Add(new descriptionText(rdr.GetInt32(0), rdr.GetString(1)));
            }
            rdr.Close();
            return _descriptions;
        }

        public string getDescriptionforID(int utl_id)
        {
            if (_descriptions != null && _descriptions.Count > 0)
            {
                for (int i = 0; i < _descriptions.Count; i++)
                {
                    if (_descriptions[i].util_id == utl_id)
                    {
                        return _descriptions[i].txt;
                    }
                }
            }
            return "";
        }
    }
}
