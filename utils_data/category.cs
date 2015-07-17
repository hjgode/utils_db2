using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;
using System.Data.SqlClient;
using System.IO;

namespace utils_data
{
    /*
    CREATE TABLE [dbo].[utils_categories](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[cat_id] [int] NOT NULL,
	[name] [nchar](130) NOT NULL,
	[description] [ntext] NOT NULL,
	[util_ids] [nchar](80) NOT NULL,
     CONSTRAINT [PK_utils_categories] PRIMARY KEY CLUSTERED 
    (
	    [id] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

    GO

    ALTER TABLE [dbo].[utils_categories] ADD  CONSTRAINT [DF_utils_categories_util_ids]  DEFAULT ((0)) FOR [util_ids]
    GO
    */
    public class Category
    {
        [XmlElement("cat_id")]
        public int cat_id { get; set; }
        [XmlElement("name")]
        public string name { get; set; }
        [XmlElement("description")]
        public string description { get; set; }

        [XmlIgnore]
        public List<int> util_IDs = new List<int>();

        /// <summary>
        /// list of util ids (int values)
        /// </summary>
        [XmlIgnore]
        List<int> utils_with_category = new List<int>();

        [XmlIgnore]
        public List<String> util_names_with_category = new List<string>();

        myLogger.logger _logger = new myLogger.logger(myLogger.logger.logLevel.debug);

        public Category()
        {
            cat_id = -1;
            name = "undef";
            description = "placeholder";
        }
        public Category(int cat_ID, string sName, string sDescription)
            : base()
        {
            cat_id = cat_ID;
            name = sName;
            description = sDescription;
        }

        List<int> splitUtil_ids(string s)
        {
            List<int> iList = new List<int>();
            string[] sList = s.Split(new char[] { ' ' });
            int iT;
            foreach(string S in sList){
                if(int.TryParse(S,out iT))
                    iList.Add(iT);
            }
            return iList;
        }

        public int saveToDB()
        {
            int iRet = 0;
            string sql = "UPDATE utils_categories SET description=@desc WHERE cat_id=@catid;";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Connection = database._sqlConnection;
            try
            {
                cmd.Parameters.Add("catid", System.Data.SqlDbType.Int, sizeof(int)).Value = this.cat_id;
                cmd.Parameters.Add("desc", System.Data.SqlDbType.NVarChar, this.description.Length).Value = this.description;
                iRet = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
            }
            cmd.Parameters.Clear();
            cmd.Dispose();

            return iRet;
        }

        public List<string> readUtilNamesFromDB(SqlConnection conn)
        {
            int iCnt = 0;
            List<string> sList = new List<string>();
            string sql = "select util_id, name FROM utils;";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Connection = conn;
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                int utilID = rdr.GetInt32(0);
                if (utils_with_category.Contains(utilID))
                {
                    string n = rdr.GetString(1).Trim();
                    sList.Add(n);
                }
                iCnt++;
            }
            rdr.Close();
            cmd.Dispose();
            util_names_with_category = sList;
            return sList;
        }

        public override string ToString()
        {
            return name;
        }
    }

    public class Categories
    {
        [XmlElement("categories")]
        public List<Category> categories_list { get; set; }

        myLogger.logger _logger = new myLogger.logger(myLogger.logger.logLevel.debug);

        public Categories()
        {
            categories_list = new List<Category>();
        }

        public static string getCategoriesAsString(List<Category> cats)
        {
            string s = " ";
            foreach (Category C in cats)
            {
                s += C.ToString() + " ";
            }
            return s;
        }

        //public List<category> getCatForUtil(int uID)
        //{
        //    List<category> cat_list = new List<category>();
        //    foreach (category C in categories_list)
        //    {
                
        //    }
        //    return cat_list;
        //}

        /// <summary>
        /// read all categories into categories object
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public int readCatsFromDB(SqlConnection conn)
        {
            int iCnt = 0;
            categories_list.Clear();
            //string sql = "select cat_id, name, description, util_ids FROM utils_categories;";
            string sql = "select cat_id, name, description FROM utils_categories;";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Connection = conn;
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                int catID = rdr.GetInt32(0);            //cat_id
                string n = rdr.GetString(1).Trim();     //name
                string d = rdr.GetString(2).Trim();     //description
                categories_list.Add(new Category(catID, n, d));
                iCnt++;
            }
            rdr.Close();
            cmd.Dispose();
            rdr.Dispose();

            return iCnt;
        }

        public string getCatName(int cID)
        {
            string s = "undef";
            foreach (Category C in this.categories_list)
            {
                if (C.cat_id == cID)
                    return C.name;
            }
            return s;
        }

        public List<Category> getCategoryByUtil(int uID, List<Category> catList, List<Utils_Cats_link> utilcatsList)
        {
            List<Category> returnList = new List<Category>();

            List<Utils_Cats_link> cats_list = new List<Utils_Cats_link>();
            
            Category c = new Category(0, "undef", "Please select valid category");
            foreach (Utils_Cats_link ucl in utilcatsList)
            {
                if (ucl.util_id == uID)
                    returnList.Add(this.getCategoryByCatID (ucl.cat_id));
            }
            return returnList;
        }
        public Category getCategoryByCatID(int cID)
        {
            foreach (Category c in this.categories_list)
            {
                if (c.cat_id == cID)
                    return c;
            }
            return new Category();
        }

        List<int> getIntList(string space_delimitted)
        {
            List<int> iList = new List<int>();
            string[] sList = space_delimitted.Split(new char[] { ' ' });
            foreach (string s in sList)
            {
                int i = -1;
                int uID = -1;
                if (int.TryParse(s, out i))
                {
                    uID = i;
                    iList.Add(uID);
                }
            }
            return iList;
        }

        public override string ToString()
        {
            string s = "";
            foreach (Category C in this.categories_list)
                s += "|" + C.ToString();
            return s;
        }

        public static string asString(List<Category> Cats)
        {
            string s = "";
            foreach (Category C in Cats)
                s += "|" + C.ToString();
            return s;
        }

        static public void Serialize(Categories _cats, string sFile)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Categories));
            using (TextWriter writer = new StreamWriter(sFile))
            {
                serializer.Serialize(writer, _cats);
            }
        }

        static public Categories Deserialize(string fileName)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(Categories));
            TextReader reader = new StreamReader(fileName);
            object obj = deserializer.Deserialize(reader);
            Categories XmlData = (Categories)obj;
            reader.Close();
            return XmlData;
        }

        public int addCategory(ref Category cat)
        {
            int iRet = 0;
            int iNewCatID = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT MAX(cat_id) FROM utils_categories;";
            cmd.Connection = database._sqlConnection;
            iNewCatID = (int)cmd.ExecuteScalar();
            cat.cat_id = iNewCatID+1;
            try
            {
                cmd.CommandText = "INSERT INTO utils_categories (cat_id, name, description) VALUES(@catid,@name,@description);";
                cmd.Parameters.Add("catid", System.Data.SqlDbType.Int, sizeof(int)).Value = cat.cat_id;
                cmd.Parameters.Add("name", System.Data.SqlDbType.NVarChar, cat.name.Length).Value = cat.name;
                cmd.Parameters.Add("description", System.Data.SqlDbType.NVarChar, cat.description.Length).Value = cat.description;
                iRet = cmd.ExecuteNonQuery();
                if (iRet == 1)
                    iRet = cat.cat_id;
                categories_list.Add(cat);
            }catch(Exception ex){
                _logger.log(String.Format("Exception in addCategory() for {0}, {1}. \n{2}\n", cat.cat_id, cat.name, ex.Message));
            }
            cmd.Parameters.Clear();
            cmd.Dispose();
            return iRet;
        }
    }
}
