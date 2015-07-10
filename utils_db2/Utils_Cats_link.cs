using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;
using System.Data.SqlClient;
using System.Data;

namespace utils_db2
{
    public class Utils_Cats_link
    {
        [XmlElement("util_id")]
        public int util_id { get; set; }
        [XmlElement("cat_id")]
        public int cat_id { get; set; }

        [XmlIgnore]
        static Categories myCategories=null;

        public Utils_Cats_link()
        {
            util_id = -1;
            cat_id = -1;
            myCategories = new Categories();
            if(myCategories==null)  //singleton call
                myCategories.readCatsFromDB(database._sqlConnection);
        }

        public Utils_Cats_link(int UTIL_ID, int CAT_ID):base()
        {
            util_id = UTIL_ID;
            cat_id = CAT_ID;
        }

        /// <summary>
        /// get the name of the category
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = "undef";
            foreach(Category C in myCategories.categories_list){
                if(C.cat_id==this.cat_id)
                    s=C.name;
            }
            return s;
        }
    }

    public class Utils_Cats
    {
        myLogger.logger _logger = Program._logger;

        [XmlElement("utils_cats_links")]
        public List<Utils_Cats_link> _utils_cats_links{get;set;}

        public Utils_Cats()
        {
            _utils_cats_links = new List<Utils_Cats_link>();
        }

        public static string getAsString(List<Utils_Cats_link> cat_list)
        {
            string s = "";
            foreach (Utils_Cats_link UCL in cat_list)
            {
                s += UCL.cat_id.ToString() + " ";
            }
            return s;
        }
        public List<Utils_Cats_link> getCatsForUtil(int uID)
        {
            int iRet = 0;
            int iUTIL, iCAT;
            List<Utils_Cats_link> cat_list = new List<Utils_Cats_link>();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = database._sqlConnection;
            cmd.CommandText = "SELECT util_id, cat_id from [utils_cats_link] WHERE util_id="+uID.ToString()+";";
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                iUTIL = rdr.GetInt32(0);
                iCAT = rdr.GetInt32(1);
                Utils_Cats_link utils_cats = new Utils_Cats_link(iUTIL, iCAT);
                //DO NOT ADD already existing link
                if (!cat_list.Contains(utils_cats))
                {
                    cat_list.Add(utils_cats);
                    iRet++;
                }
            }
            rdr.Close();
            cmd.Dispose();

            return cat_list;
        }

        public int readUtils_Cats_Links()
        {
            int iRet = 0;
            int iUTIL, iCAT;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = database._sqlConnection;
            cmd.CommandText = "SELECT util_id, cat_id from [utils_cats_link];";
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                iUTIL = rdr.GetInt32(0);
                iCAT = rdr.GetInt32(1);
                //DO NOT ADD already existing link
                if (!_utils_cats_links.Contains(new Utils_Cats_link(iUTIL, iCAT)))
                {
                    _utils_cats_links.Add(new Utils_Cats_link(iUTIL, iCAT));
                    iRet++;
                }
            }
            rdr.Close();
            cmd.Dispose();

            return iRet;
        }

        public int saveUtils_Cats_LinksToDB()
        {
            int iRet = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = database._sqlConnection;

            cmd.CommandText = "INSERT INTO VALUES(@util, @cat);";
            
            foreach(Utils_Cats_link UCL in _utils_cats_links){
                cmd.Parameters.Add("util", SqlDbType.Int).Value = UCL.util_id;
                cmd.Parameters.Add("cat", SqlDbType.Int).Value = UCL.cat_id;
                try
                {
                    iRet = cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    //we will get errors for existing data!
                    _logger.log("SqlException in saveUtils_Cats_LinksToDB(): "+ex.Message);
                }
                catch (Exception ex) {
                    _logger.log("Exception in saveUtils_Cats_LinksToDB(): " + ex.Message);
                }
                finally
                {
                    cmd.Parameters.Clear();
                }
            }
            cmd.Dispose();

            return iRet;
        }
    }
}
