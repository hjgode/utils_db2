﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;
using System.IO;

namespace utils_db2
{
    public class utility
    {
        myLogger.logger _logger = Program._logger;

        [XmlElement("id")]
        public int id { get; set; }
        [XmlElement("util_id")]
        public int util_id { get; set; }

        [XmlElement("name")]
        public string name { get; set; }
        [XmlElement("description")]
        public string description { get; set; }
        [XmlElement("author")]
        public string author { get; set; }
        [XmlElement("file_link")]
        public string file_link { get; set; }
        [XmlElement("devices")]
        public Device[] devices { get; set; }
        [XmlElement("operating_system")]
        public Operating_System operating_system { get; set; }

        [XmlElement("file_data")]
        public byte[] file_data { get; set; }

        /// <summary>
        /// will hold a string with category IDs this utility can be usefull
        /// </summary>
        [XmlElement("categories")]
        public string _categories { get; set; }

        [XmlIgnore]
        public List<Category> _category_list { get; set; }

        public utility()
        {
            id = -1;
            util_id = -1;
            name = "undefined";
            description = "undefined";
            author = "undefined";
            file_link = "undefined";
            //build table of utils_id and device names
            devices = new Device[] { new Device() };

            operating_system = new Operating_System();
            file_data = null;

            _categories = "-1";
            _category_list = new List<Category>();
            _category_list.Add(new Category());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i">util_id</param>
        /// <param name="n">name</param>
        /// <param name="desc">description</param>
        /// <param name="a">author</param>
        /// <param name="f">file_link</param>
        /// <param name="cats"></param>
        public utility(int i, string n, string desc, string a, string f, string cats)
            : base()
        {
            id = i;
            util_id = i;
            name = n;
            description = desc;
            author = a;
            file_link = f;

            _categories = cats;
        }

        public utility(int i, string n, string desc, string a, string f, byte[] fData, string cats)
            : base()
        {
            id = i;
            util_id = i;
            name = n;
            description = desc;
            author = a;
            file_link = f;
            file_data = fData;
            _categories = cats;
        }

        /*
        public utility(int i, string n, string desc, string a, string f, Device[] devs, Operating_System os, byte[] bData, string cats)
            : base()
        {
            id = i;
            util_id = i;
            name = n;
            description = desc;
            author = a;
            file_link = f;
            devices = devs;
            operating_system = os;
            file_data = bData;

            _categories = cats;
        }

        public utility(int i, string n, string desc, string a, string f):base()
        {
            id = i;
            util_id = i;
            name = n;
            description = desc;
            author = a;
            file_link = f;
            file_data = null;
        }
        */

        public byte[] getFileData()
        {
            return this.file_data;
        }

        public void setFileData(Byte[] bData)
        {
            this.file_data = bData;
        }

        public override string ToString()
        {
            return this.name;
        }

        public int writeFileData(string sFilename)
        {
            int iRet = -1;
            try
            {
                FileStream fs = new FileStream(sFilename, FileMode.CreateNew);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(this.file_data);
                bw.Flush();
                fs.Flush();
                bw.Close();
                fs.Close();
                iRet = 0;
            }
            catch (IOException ex)
            {
                _logger.log("IOException in writeFileData: " + ex.Message);
            }
            catch (Exception ex)
            {
                _logger.log("Exception in writeFileData: " + ex.Message);
            }
            return iRet;
        }

        List<int> getCatList()
        {
            List<int> iList = new List<int>();
            string[] aList = this._categories.Split(new char[] { ' ' });
            foreach (string S in aList)
            {
                int V;
                if(Int32.TryParse(S,out V))
                    iList.Add(V);
            }
            return iList;
        }

        void cat_ids_to_categories(List<Category> cats)
        {
            _category_list.Clear();
            List<int> iList = getCatList();
            foreach (Category C in cats)
            {
                if(iList.Contains(C.cat_id))
                    _category_list.Add(C);
            }
        }

        public int addCategory(Category cat)
        {
            int iRet = 0;
            if (_category_list.Contains(cat))
                return iRet;
            this._categories += " " + cat.cat_id.ToString();
            this._category_list.Add(cat);
            return iRet+1;
        }

        public int readCategoriesForUtil()
        {
            int iCnt = 0;
            if (_category_list == null)
                _category_list = new List<Category>();
            else
                _category_list.Clear();
            Categories cats= new Categories();
            cats.readCatsFromDB(database._sqlConnection);
            foreach (Category C in cats.categories_list)
            {
                if (C.util_ids == null)
                    C.util_ids = "";
                if (C.util_ids.Contains(this.util_id.ToString()))
                    _category_list.Add(C);
            }
            return iCnt;
        }

        
    }
}
