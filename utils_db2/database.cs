using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

#region OLD_DB_DESIGN
/*
 * database
 *      supportstaff-rw
 * tables
 *      dbo.utils
                CREATE TABLE [dbo].[utils](
	                [id] [int] IDENTITY(1,1) NOT NULL,      ==> pointer into utils_devices->utils_id
                                                            ==> pointer into utils_operating_systems->utils_id
	                [name] [nchar](80) NOT NULL
 	                [description] [text] NOT NULL,
	                [author] [nchar](80) NOT NULL,
	                [file_link] [nchar](254) NOT NULL
                ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
 * 
 *      dbo.utils_operating_systems
                CREATE TABLE [dbo].[utils_operating_systems](
	                [id] [int] IDENTITY(1,1) NOT NULL,
	                [utils_id] [int] NOT NULL,
	                [name] [nchar](80) NOT NULL
                ) ON [PRIMARY]
 *      dbo.utils_device
                CREATE TABLE [dbo].[utils_device](
                    [id] [int] IDENTITY(1,1) NOT NULL,
	                [util_id] [int] IDENTITY(1,1) NOT NULL,   <== utils.id
	                [name] [nchar](80) NOT NULL
                ) ON [PRIMARY]
 * constraints
 * */
#endregion

namespace utils_db2
{
    class database:IDisposable
    {
        myLogger.logger _logger = Program._logger;
        string _u = "supportstaff-rw";
        string _p = "rqySGX4D";
        string _c = "";
        public static SqlConnection _sqlConnection = new SqlConnection();
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

        public database(String connectString){
            _logger.log("init database");
            _c = String.Format(connectString, _u, _p);
            _bConnected = connect();

            //test
            if (_bConnected)
                readDataUtils();
            else
            {
                _logger.log("read data failed");
                throw new Exception("database init() failed");
            }
        }

        public database(String connectString, bool prefetchData){
            _logger.log("init database");
            _c = String.Format(connectString, _u, _p);
            _bConnected = connect();

            //test
            if (_bConnected && prefetchData)
                readDataUtils();
            if(!_bConnected)
            {
                _logger.log("not connected");
                throw new Exception("database connect failed");
            }
        }
        void readDataUtils()
        {
            _dtUtils = new DataTable();
            //SqlCommand sqlCmd = new SqlCommand("select * from utilities_view", sqlConnection);
            //dt.Load(sqlCmd.ExecuteReader());
            _daUtils = new SqlDataAdapter("select * from utils", _sqlConnection);// (sqlALL, sqlConnection);// ("select * from utils", sqlConnection);
            SqlCommandBuilder sqlBuilder = new SqlCommandBuilder(_daUtils);
            _daUtils.Fill(_dtUtils);
            _bsUtils = new System.Windows.Forms.BindingSource();
            _bsUtils.DataSource = _dtUtils;

            readDeviceNameList();

            //readDeviceLinkList();

            _lstOSNames = _osname.readList(_sqlConnection);

            _descriptions = _description.readList(_sqlConnection);

        }

        /// <summary>
        /// fill dtDevices DataTable
        /// </summary>
        /// <returns></returns>
        public List<Device> readDeviceNameList()
        {
            _ListDevices = new List<Device>();
            _dtDevicesNames = new DataTable();
            SqlCommand cmdReadDevices = new SqlCommand("select util_id, name from utils_device", _sqlConnection);
            _dtDevicesNames.Load(cmdReadDevices.ExecuteReader());
            foreach (DataRow dr in _dtDevicesNames.Rows)
            {
                int uID = -1; string n = "undef";
                if (int.TryParse(dr["util_id"].ToString(), out uID))
                    uID=int.Parse(dr["util_id"].ToString());
                n = dr["name"].ToString().Trim();

                _ListDevices.Add(new Device(uID, n));
            }
            return _ListDevices;
        }

        //public DataTable readDeviceLinkList()
        //{
        //    _ListDevices = new List<Device>(); ;
        //    _dtDevicesLinkTable = new DataTable();
        //    SqlCommand cmdReadDevices = new SqlCommand("select * from utils_devices", _sqlConnection);
        //    _dtDevicesLinkTable.Load(cmdReadDevices.ExecuteReader());
        //    return _dtDevicesLinkTable;
        //}

        public List<Device> getDevices(int util_id)
        {
            List<Device> devicesList = new List<Device>();
            //look thru utils
            foreach (DataRow dr in _dtUtils.Rows)
            {
                if ((int)dr["util_id"] == util_id)// find utils record for ID
                {
                    int utilsID=(int)dr["util_id"];
                    //look thru devices link list
                    foreach (DataRow drDeviceLink in _dtDevicesLinkTable.Rows)
                    {
                        if ((int)drDeviceLink["utils_id"] == utilsID)
                        {
                            //get name of device
                            devicesList.Add(getDevice((int)drDeviceLink["util_id"]));
                        }
                    }
                }
            }
            return devicesList;
        }

        /// <summary>
        /// update util_devices for util_id to cover all devices listed
        /// </summary>
        /// <param name="util_id">update which util</param>
        /// <param name="devs">list of devices</param>
        /// <returns></returns>
        public int updateDeviceListForUtil(int util_id, Device[] devs)
        {
            int iCnt = 0;
            List<Device> lstDevicesOld = getDevices(util_id);
            //compare lists
            List<Device> lstDevicesToAdd = new List<Device>();

            //find devices not already in list
            foreach (Device d in devs)
            {
                if (!lstDevicesOld.Contains(d)) //device not yet listed
                    lstDevicesToAdd.Add(d);
            }
            int iRes = 0;
            //clear existing entries for util_id
            string sDelete = "DELETE FROM utils_devices where utils_id=" + util_id.ToString();

            iRes = executeSQLnoQuery(sDelete);

            if (lstDevicesToAdd.Count > 0)
            {
                for (int i = 0; i < lstDevicesToAdd.Count; i++)
                {
                    //insert utils_devices
                    // "Insert into utils_devices.devices_id, utils_devices.device_id VALUES (2,2);"
                    string sSql = "Insert into utils_devices (utils_id, name) VALUES (" +
                        util_id.ToString() + ", '" + lstDevicesToAdd[i].name +"'"+
                        ");";
                    iRes = executeSQLnoQuery(sSql);
                    iCnt += iRes;
                }
            }
            return iRes;
        }

        Device getDevice(int util_id)
        {
            Device dev= new Device();
            foreach (Device d in _ListDevices)
            {
                if (d.util_id == util_id)
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
                _sqlConnection.StateChange += new StateChangeEventHandler(_sqlConnection_StateChange);
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

        void _sqlConnection_StateChange(object sender, StateChangeEventArgs e)
        {
            bool bOK=false;
            ConnectionState st = (ConnectionState)e.CurrentState;
            switch (st)
            {
                case ConnectionState.Broken:
                case ConnectionState.Closed:
                    bOK = false;
                    break;
                case ConnectionState.Connecting:
                case ConnectionState.Executing:
                case ConnectionState.Fetching:
                case ConnectionState.Open:
                    bOK = true;
                    break;
            }
            onUpdateHandler(new MyEventArgs(bOK,st));
        }
        public class MyEventArgs : EventArgs
        {
            public bool bIsOpen = false;
            public ConnectionState state = ConnectionState.Closed;
            public MyEventArgs(bool b, ConnectionState st)
            {
                this.bIsOpen = b;
                this.state = st;
            }
        }
        public delegate void updateEventHandler(object sender, MyEventArgs eventArgs);
        public event updateEventHandler updateEvent;
        void onUpdateHandler(MyEventArgs args)
        {
            //anyone listening?
            if (this.updateEvent == null)
                return;
            MyEventArgs a = args;
            this.updateEvent(this, a);
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
