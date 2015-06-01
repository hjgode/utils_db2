using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;

namespace TechSupport_Utilities
{
    class helper
    {
        public static IPAddress getIP()
        {
            string s = Properties.Settings.Default.connectstring;
            int iS = s.IndexOf("Data Source=");
            string sIP="";
            IPAddress ip = IPAddress.Loopback;
            if (iS != -1)
            {
                iS += "Data Source=".Length;
                int iE = s.IndexOf(";",iS);
                sIP = s.Substring(iS, iE - iS);
                try
                {
                    ip = IPAddress.Parse(sIP);
                }
                catch (Exception)
                {
                }
            }
            return ip;
        }


    }
}
