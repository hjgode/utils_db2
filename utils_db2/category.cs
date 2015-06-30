﻿using System;
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
    public class category
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
        List<int> utils_with_category = new List<int>();

        [XmlIgnore]
        public List<String> util_names_with_category = new List<string>();

        public category()
        {
            cat_id = -1;
            name = "undef";
            description = "placeholder";
            util_ids = "-1 ";
        }

        public category(int cat_ID, string sName, string sDescription, string sUtilsIDList)
        {
            cat_id = cat_ID;
            name = sName;
            description = sDescription;
            util_ids = sUtilsIDList;
            utils_with_category = getUtils();
        }

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

    public class categories
    {
        [XmlElement("categories")]
        public List<category> categories_list { get; set; }

        public categories()
        {
            categories_list = new List<category>();
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
            string sql = "select cat_id, name, description, util_ids FROM utils_categories;";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Connection = conn;
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                int catID = rdr.GetInt32(0);
                string n = rdr.GetString(1).Trim();
                string d = rdr.GetString(2).Trim();
                string u = rdr.GetString(3).Trim();
                categories_list.Add(new category(catID,n,d,u));
                iCnt++;
            }
            rdr.Close();
            cmd.Dispose();
            return iCnt;
        }

        static public void Serialize(categories _cats, string sFile)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(categories));
            using (TextWriter writer = new StreamWriter(sFile))
            {
                serializer.Serialize(writer, _cats);
            }
        }

        static public categories Deserialize(string fileName)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(categories));
            TextReader reader = new StreamReader(fileName);
            object obj = deserializer.Deserialize(reader);
            categories XmlData = (categories)obj;
            reader.Close();
            return XmlData;
        }

    }
}
