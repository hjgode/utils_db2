using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace utils_db2
{
    class database:IDisposable
    {
        string sqlALL = "SELECT dbo.utilities.name, dbo.device.name AS device, dbo.operating_systems.name AS OS, dbo.utilities.description, dbo.utilities.author, dbo.utilities.file_link " +
                        "FROM dbo.utilities LEFT OUTER JOIN " +
                        "dbo.operating_systems ON dbo.utilities.operating_id = dbo.operating_systems.operating_id LEFT OUTER JOIN " +
                        "dbo.devices ON dbo.utilities.devices_id = dbo.devices.devices_id LEFT OUTER JOIN " +
                        "dbo.device ON dbo.devices.device_id = dbo.device.device_id ";

        logger _logger = Program._logger;
        string _u = "supportstaff-rw";
        string _p = "rqySGX4D";
        string _c = "";
        private static SqlConnection _sqlConnection = new SqlConnection();
        public bool _bConnected = false;

        DataTable _dtUtils = null;
        public SqlDataAdapter _daUtils = null;
        public System.Windows.Forms.BindingSource _bsUtils = null;

        DataTable _dtDevicesNames = null;
        DataTable _dtDevicesLinkTable = null;

        List<Device> _ListDevices;

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
            _daUtils = new SqlDataAdapter("select * from utilities", _sqlConnection);// (sqlALL, sqlConnection);// ("select * from utilities", sqlConnection);
            SqlCommandBuilder sqlBuilder = new SqlCommandBuilder(_daUtils);
            _daUtils.Fill(_dtUtils);
            _bsUtils = new System.Windows.Forms.BindingSource();
            _bsUtils.DataSource = _dtUtils;

            readDeviceNameList();

            readDeviceLinkList();
        }

        /// <summary>
        /// fill dtDevices DataTable
        /// </summary>
        /// <returns></returns>
        public List<Device> readDeviceNameList()
        {
            _ListDevices = new List<Device>(); ;
            _dtDevicesNames = new DataTable();
            SqlCommand cmdReadDevices = new SqlCommand("select * from device", _sqlConnection);
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
            SqlCommand cmdReadDevices = new SqlCommand("select * from devices", _sqlConnection);
            _dtDevicesLinkTable.Load(cmdReadDevices.ExecuteReader());
            return _dtDevicesLinkTable;
        }

        public List<Device> getDevices(int util_id)
        {
            List<Device> devicesList = new List<Device>();
            //look thru utilities
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
 * Insert into dbo.utilities (name) values ('Microsoft DevHealth'), ( 'cpumon'), ( 'PhoneStateChangeLogger'), ( 'BatteryLog'), ( 'WWANCollector'), ( 'DebugLog'), ( 'NetValidate'), ( 'RILLogger'), ( 'BatteryLog'), ( 'BTmon'), ( 'memuse'), ( 'connMgrLog'), ( 'deviceMon'), ( 'PowerMsgLog'), ( 'rb_logger'), ( 'Intermec DevHealth');
USE [SupportStaff]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

BEGIN TRY
    DROP TABLE [dbo].[utilities];
END TRY
BEGIN CATCH
END CATCH

GO

CREATE TABLE [dbo].[utilities](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nchar](32) NOT NULL
) ON [PRIMARY]

GO

declare @crlf char(2)
select @crlf = char(13)+char(10)

Insert into dbo.utilities (name) values ('Microsoft DevHealth'), ( 'cpumon'), ( 'PhoneStateChangeLogger'), ( 'BatteryLog'), ( 'WWANCollector'), ( 'DebugLog'), ( 'NetValidate'), ( 'RILLogger'), ( 'BatteryLog'), ( 'BTmon'), ( 'memuse'), ( 'connMgrLog'), ( 'deviceMon'), ( 'PowerMsgLog'), ( 'rb_logger'), ( 'Intermec DevHealth');
GO

*/
