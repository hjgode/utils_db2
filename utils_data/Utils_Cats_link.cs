using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace utils_data
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
        myLogger.logger _logger = new myLogger.logger(myLogger.logger.logLevel.debug);

        [XmlElement("utils_cats_links")]
        public List<Utils_Cats_link> _utils_cats_links{get;set;}

        public Utils_Cats()
        {
            _utils_cats_links = new List<Utils_Cats_link>();
        }

        static public void Serialize(Utils_Cats _utils, string sFile)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Utils_Cats));
            using (TextWriter writer = new StreamWriter(sFile))
            {
                serializer.Serialize(writer, _utils);
            }
        }

        static public Utils_Cats Deserialize(string fileName)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(Utils_Cats));
            TextReader reader = new StreamReader(fileName);
            object obj = deserializer.Deserialize(reader);
            Utils_Cats XmlData = (Utils_Cats)obj;
            reader.Close();
            return XmlData;
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

        /// <summary>
        /// read all utils_cats_link data
        /// </summary>
        /// <returns></returns>
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

        int deleteCatsForUtil(int uID)
        {
            int iRet = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = database._sqlConnection;
            try
            {
                //delete existing data???
                //cmd.CommandText = "IF EXISTS (SELECT * FROM [utils_cats_link] WHERE util_id=@util) DELETE FROM [utils_cats_link] WHERE util_id=@util);";
                cmd.CommandText = "DELETE FROM [utils_cats_link] WHERE util_id=@util;";
                cmd.Parameters.Add("util", SqlDbType.Int, sizeof(int)).Value=uID;
                iRet = cmd.ExecuteNonQuery();
            }
            catch (Exception ex) {
                _logger.log("Exception in deleteCatsForUtil() for " + uID.ToString() +". "+ex.Message);
            }
            cmd.Parameters.Clear();
            cmd.Dispose();

            return iRet;
        }
        public int saveUtils_Cats_LinksToDB()
        {
            int iRet = 0;

            //delete existing data???
            if( this._utils_cats_links.Count!=0 )
                deleteCatsForUtil(this._utils_cats_links[0].util_id);

            //add non existing data
            SqlCommand cmd = new SqlCommand();
            SqlTransaction transaction;
            transaction = database._sqlConnection.BeginTransaction();
            try
            {
                cmd.Transaction = transaction;
                cmd.Connection = database._sqlConnection;
                cmd.CommandText = "IF NOT EXISTS (SELECT * FROM [utils_cats_link] WHERE util_id=@util AND cat_id=@cat) INSERT INTO [utils_cats_link] VALUES(@util, @cat);";

                foreach (Utils_Cats_link UCL in _utils_cats_links)
                {
                    cmd.Parameters.Add("util", SqlDbType.Int).Value = UCL.util_id;
                    cmd.Parameters.Add("cat", SqlDbType.Int).Value = UCL.cat_id;
                    try
                    {
                        iRet = cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        //we will get errors for existing data!
                        _logger.log("SqlException in saveUtils_Cats_LinksToDB(): " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        _logger.log("Exception in saveUtils_Cats_LinksToDB(): " + ex.Message);
                    }
                    finally
                    {
                        cmd.Parameters.Clear();
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                iRet = -1;
            }
            finally
            {
            }

            cmd.Dispose();

            return iRet;
        }
    }
}
