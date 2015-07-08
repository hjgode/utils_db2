using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;
using System.Data.SqlClient;
using System.IO;

namespace utils_db2
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

        /// <summary>
        /// a list of util_id values providing a tool for the category
        /// </summary>
        [XmlElement("util_ids")]
        public string util_ids { get; set; }

        [XmlIgnore]
        public List<int> util_IDs = new List<int>();

        /// <summary>
        /// list of util ids (int values)
        /// </summary>
        [XmlIgnore]
        List<int> utils_with_category = new List<int>();

        [XmlIgnore]
        public List<String> util_names_with_category = new List<string>();

        public Category()
        {
            cat_id = -1;
            name = "undef";
            description = "placeholder";
            util_ids = "-1 ";
        }
        public Category(int cat_ID, string sName, string sDescription, string uIDs)
            : base()
        {
            cat_id = cat_ID;
            name = sName;
            description = sDescription;
            util_ids = uIDs;
            util_IDs = splitUtil_ids(util_ids);
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

        //public category(int cat_ID, string sName, string sDescription, string sUtilsIDList)
        //    : base()
        //{
        //    cat_id = cat_ID;
        //    name = sName;
        //    description = sDescription;
        //    util_ids = sUtilsIDList;
        //    utils_with_category = getUtils();
        //}

        List<int> getUtils()
        {
            List<int> iList = new List<int>();
            string[] sList = util_ids.Split(new char[] { ' ' });
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


        public List<string> readUtilsFromDB(SqlConnection conn)
        {
            int iCnt = 0;
            List<string> sList = new List<string>();
            string sql = "select util_id, name, categories FROM utils;";
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
            //string sql = "select cat_id, name, description, util_ids FROM utils_categories;";
            string sql = "select cat_id, name, description, util_ids FROM utils_categories;";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Connection = conn;
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                int catID = rdr.GetInt32(0);            //cat_id
                string n = rdr.GetString(1).Trim();     //name
                string d = rdr.GetString(2).Trim();     //description
                string uIDs = rdr.GetString(3).Trim();     //util_ids
                categories_list.Add(new Category(catID, n, d, uIDs));
                iCnt++;
            }
            rdr.Close();
            cmd.Dispose();
            rdr.Dispose();

            /*
            //now read utils table and add utils_id to the categories
            sql = "select util_id, name, categories FROM utils;";
            cmd.Connection = conn;
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                int uid=rdr.GetInt32(0);
                string uname=rdr.GetString(1).Trim();
                string cats = rdr.GetString(2).Trim();//space delimitted list of cat_id's

                //list of cat_id in utils.categories
                string[] catListForUtil = cats.Split(new char[] { ' ' });
                List<int> catIListForUtil = getIntList(cats);

                //add util id to cat list
                foreach (Category C in categories_list)
                {
                    if (catIListForUtil.Contains(C.cat_id))
                        C.util_ids += " " + uid.ToString();
                }

            }
            rdr.Close();
            cmd.Dispose();
            rdr.Dispose();
            */

            return iCnt;
        }

        public List<Category> getCategoriesForUtil(int uID)
        {
            List<Category> cats = new List<Category>();
            foreach(Category C in categories_list){
                if (C.util_ids == null)
                    continue;
                string[] ids = C.util_ids.Split(new char[] { ' ' }); 
                foreach(string s in ids){
                    if (s==uID.ToString())
                        cats.Add(C);
                }
            }
            return cats;
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

    }
}
