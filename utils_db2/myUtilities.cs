using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils_db2
{
    public class myUtilities
    {
        myLogger.logger _logger = Program._logger;
        //database _database;
        public utilities _utilities;
        public Categories _categories;
        public Operating_System _operating_system;
        public Utils_Cats _utils_cats;

        public myUtilities()
        {
        }
        public int openDB()
        {
            int iRes = 0;
            try
            {
                //string connect = Properties.Settings.Default.connectstring;

                //_logger.log("Open database...");
                //database _database = theDatabase;

                //read the helper lists first and update the utilities objects
                //devices
                Devices devices = new Devices();
                devices.readDevicesFromDB(database._sqlConnection);

                //categories
                _categories = new Categories();
                _categories.readCatsFromDB(database._sqlConnection);

                //OS
                _operating_system = new Operating_System();
                _operating_system.readList(database._sqlConnection);

                //utils_cats_link
                _utils_cats = new Utils_Cats();
                _utils_cats.readUtils_Cats_Links();

                //read the utils
                _utilities = new utilities();
                _utilities.readUtilsDB(database._sqlConnection);

                foreach (utility U in _utilities.utilitiesList)
                {
                    //update devices
                    U.devices = devices.getDevicesForID(U.util_id);
                    //update operating_system
                    U.operating_system = _operating_system.getOsForId(U.util_id);

                    //categories
                    List<Utils_Cats_link> cat_ids = _utils_cats.getCatsForUtil(U.util_id);
                    U._category_list.Clear();
                    List<Category> category_for_util = new List<Category>();
                    category_for_util = _categories.getCategoryByUtil(U.util_id, _categories.categories_list, _utils_cats._utils_cats_links);

                    U._category_list = category_for_util;
                    _logger.log("Util: " + U.util_id.ToString() + ": <LIST> " + Categories.asString(U._category_list));

                    ///U._categories = Categories.getCategoriesAsString(U._category_list);
                    ///_logger.log("Util: "+ U.util_id.ToString() + ": <string> " + U._categories);

                    //save changes back to utils table
                    ///U.saveCategoriesToDB();
                }


                //source = new BindingSource();
                //source.DataSource = _utilities.utilitiesList;
                //dataGridView1.DataSource = source;

                ////                dataGridView1.DataSource = _utilities.utilitiesList;

                //dataGridView1.Columns["id"].Visible = false;
                ////dataGridView1.Columns["util_id"].Visible = true;
                //try
                //{
                //    dataGridView1.Columns["file_data"].Visible = false; //we can not show binary data as image
                //}
                //catch (Exception) { }
                //dataGridView1.Columns.Add("devices", "devices");

                //dataGridView1.Columns["devices"].DataPropertyName = "devices";

                //dataGridView1.Columns.Add("categories", "categories");
                //dataGridView1.Columns["categories"].DataPropertyName = "_category_list";

                //dataGridView1.Refresh();

                _logger.log("database loaded and ready");
            }
            catch (Exception ex)
            {
                _logger.log("Exception in mainForm init: " + ex.Message);
                iRes = -1;
            }
            return iRes;
        }
    }
}
