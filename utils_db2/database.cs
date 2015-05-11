﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

/*
 * database
 *      supportstaff-rw
 * tables
 *      dbo.utils
                CREATE TABLE [dbo].[utils](
	                [id] [int] IDENTITY(1,1) NOT NULL,
	                [name] [nchar](80) NOT NULL,
	                [devices_id] [int] NOT NULL,            ==> pointer into utils_devices->devices_id
	                [operating_id] [int] NOT NULL,          ==> pointer into utils_operating_systems->operating_id
	                [description] [text] NOT NULL,
	                [author] [nchar](80) NOT NULL,
	                [file_link] [nchar](254) NOT NULL
                ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
 * 
 *      dbo.utils_operating_systems
                CREATE TABLE [dbo].[utils_operating_systems](
	                [id] [int] IDENTITY(1,1) NOT NULL,
	                [operating_id] [int] NOT NULL,
	                [name] [nchar](80) NOT NULL
                ) ON [PRIMARY]
 *      dbo.utils_devices
 *          many-to-many junction table
                CREATE TABLE [dbo].[utils_devices](
	                [id] [int] IDENTITY(1,1) NOT NULL,
	                [devices_id] [int] NOT NULL,            <== utils.devices_id
	                [device_id] [int] NOT NULL              ==> pointer into utils_device->device_id
                ) ON [PRIMARY]
 *      dbo.utils_device
                CREATE TABLE [dbo].[utils_device](
	                [device_id] [int] IDENTITY(1,1) NOT NULL,   <== utils.operating_id
	                [name] [nchar](80) NOT NULL
                ) ON [PRIMARY]
 * constraints
 * */

namespace utils_db2
{
    class database:IDisposable
    {
        string sqlALL = "SELECT dbo.utils.name, dbo.utils_device.name AS utils_device, dbo.utils_operating_systems.name AS OS, dbo.utils.description, dbo.utils.author, dbo.utils.file_link " +
                        "FROM dbo.utils LEFT OUTER JOIN " +
                        "dbo.utils_operating_systems ON dbo.utils.operating_id = dbo.utils_operating_systems.operating_id LEFT OUTER JOIN " +
                        "dbo.utils_devices ON dbo.utils.devices_id = dbo.utils_devices.devices_id LEFT OUTER JOIN " +
                        "dbo.utils_device ON dbo.utils_devices.device_id = dbo.utils_device.device_id ";

        logger _logger = Program._logger;
        string _u = "supportstaff-rw";
        string _p = "rqySGX4D";
        string _c = "";
        private static SqlConnection _sqlConnection = new SqlConnection();
        public bool _bConnected = false;

        DataTable _dtUtils = null;
        public SqlDataAdapter _daUtils = null;
        public System.Windows.Forms.BindingSource _bsUtils = null;

        //device names and link list
        DataTable _dtDevicesNames = null;
        DataTable _dtDevicesLinkTable = null;
        List<Device> _ListDevices;

        //os names
        public List<OS_name> _lstOSNames = null;
        public OS_name _osname = new OS_name();

        //descriptions
        public List<descriptionText> _descriptions = null;
        public descriptionText _description = new descriptionText();

        public database(){
            _logger.log("init database");
            _c = String.Format(Properties.Settings.Default.connectstring, _u,_p);
            _bConnected = connect();

            //test
            if (_bConnected)
                readDataUtils();
            else
                _logger.log("read data failed");
        }

        void readDataUtils()
        {
            //SqlDataReader dr;
            _dtUtils = new DataTable();
            //SqlCommand sqlCmd = new SqlCommand("select * from utilities_view", sqlConnection);
            //dt.Load(sqlCmd.ExecuteReader());
            _daUtils = new SqlDataAdapter("select * from utils", _sqlConnection);// (sqlALL, sqlConnection);// ("select * from utils", sqlConnection);
            SqlCommandBuilder sqlBuilder = new SqlCommandBuilder(_daUtils);
            _daUtils.Fill(_dtUtils);
            _bsUtils = new System.Windows.Forms.BindingSource();
            _bsUtils.DataSource = _dtUtils;

            readDeviceNameList();

            readDeviceLinkList();

            _lstOSNames = _osname.readList(_sqlConnection);

            _descriptions = _description.readList(_sqlConnection);

        }

        /// <summary>
        /// fill dtDevices DataTable
        /// </summary>
        /// <returns></returns>
        public List<Device> readDeviceNameList()
        {
            _ListDevices = new List<Device>(); ;
            _dtDevicesNames = new DataTable();
            SqlCommand cmdReadDevices = new SqlCommand("select * from utils_device", _sqlConnection);
            _dtDevicesNames.Load(cmdReadDevices.ExecuteReader());
            foreach (DataRow dr in _dtDevicesNames.Rows)
            {
                _ListDevices.Add(new Device(int.Parse(dr["device_id"].ToString()),dr["name"].ToString()));
            }
            return _ListDevices;
        }

        public DataTable readDeviceLinkList()
        {
            _ListDevices = new List<Device>(); ;
            _dtDevicesLinkTable = new DataTable();
            SqlCommand cmdReadDevices = new SqlCommand("select * from utils_devices", _sqlConnection);
            _dtDevicesLinkTable.Load(cmdReadDevices.ExecuteReader());
            return _dtDevicesLinkTable;
        }

        public List<Device> getDevices(int util_id)
        {
            List<Device> devicesList = new List<Device>();
            //look thru utils
            foreach (DataRow dr in _dtUtils.Rows)
            {
                if ((int)dr["id"] == util_id)// find utils record for ID
                {
                    int devicesID=(int)dr["devices_id"];
                    //look thru devices link list
                    foreach (DataRow drDeviceLink in _dtDevicesLinkTable.Rows)
                    {
                        if ((int)drDeviceLink["devices_id"] == devicesID)
                        {
                            //get name of device
                            devicesList.Add(getDevice((int)drDeviceLink["device_id"]));
                        }
                    }
                }
            }
            return devicesList;
        }

        Device getDevice(int device_id)
        {
            Device dev= new Device(0,"");
            foreach (Device d in _ListDevices)
            {
                if (d.device_id == device_id)
                    dev = d;
            }
            return dev;
        }

        public DataTable executeSQL(string s)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(s, _sqlConnection);
            dt.Load(cmd.ExecuteReader());
            return dt;
        }

        public int executeSQLnoQuery(string s)
        {
            SqlCommand cmd = new SqlCommand(s, _sqlConnection);
            int iRes =cmd.ExecuteNonQuery();
            return iRes;
        }

        /*
        private static void ReadOrderData(string connectionString)
        {
            string queryString =
                "SELECT OrderID, CustomerID FROM dbo.Orders;";

            using (SqlConnection connection =
                       new SqlConnection(connectionString))
            {
                SqlCommand command =
                    new SqlCommand(queryString, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data. 
                while (reader.Read())
                {
                    Console.WriteLine(String.Format("{0}, {1}",
                        reader[0], reader[1]));
                }

                // Call Close when done reading.
                reader.Close();
            }
        }
        */
        private bool connect()
        {
            try
            {
                _sqlConnection.ConnectionString = _c;
                _sqlConnection.Open();
                return true;
            }
            catch (SqlException ex)
            {
                _logger.log("connect() exception: "+ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                _logger.log("connect() exception: " + ex.Message);
                return false;
            }
        }

        public void Dispose()
        {
            if (_sqlConnection != null)
            {
                _logger.log("close database");
                _sqlConnection.Close();
            }
        }
    }
}
/*
 * Insert into dbo.utils (name) values ('Microsoft DevHealth'), ( 'cpumon'), ( 'PhoneStateChangeLogger'), ( 'BatteryLog'), ( 'WWANCollector'), ( 'DebugLog'), ( 'NetValidate'), ( 'RILLogger'), ( 'BatteryLog'), ( 'BTmon'), ( 'memuse'), ( 'connMgrLog'), ( 'deviceMon'), ( 'PowerMsgLog'), ( 'rb_logger'), ( 'Intermec DevHealth');
USE [SupportStaff]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

BEGIN TRY
    DROP TABLE [dbo].[utils];
END TRY
BEGIN CATCH
END CATCH

GO

CREATE TABLE [dbo].[utils](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nchar](32) NOT NULL
) ON [PRIMARY]

GO

declare @crlf char(2)
select @crlf = char(13)+char(10)

Insert into dbo.utils (name) values ('Microsoft DevHealth'), ( 'cpumon'), ( 'PhoneStateChangeLogger'), ( 'BatteryLog'), ( 'WWANCollector'), ( 'DebugLog'), ( 'NetValidate'), ( 'RILLogger'), ( 'BatteryLog'), ( 'BTmon'), ( 'memuse'), ( 'connMgrLog'), ( 'deviceMon'), ( 'PowerMsgLog'), ( 'rb_logger'), ( 'Intermec DevHealth');
GO

*/
