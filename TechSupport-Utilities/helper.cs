using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Net;
using System.Net.NetworkInformation;

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

        public class ConnectTest:IDisposable
        {
            Thread connectTestThread = null;
            IPAddress ip = getIP();
            
            public ConnectTest()
            {
                connectTestThread=new Thread(new ThreadStart(myConnectThread));
                connectTestThread.Name = "connectTestThread";
                connectTestThread.Start();
            }
            public void Dispose()
            {
                if (connectTestThread != null)
                {
                    connectTestThread.Abort();
                }
            }
            void myConnectThread(){
                bool runThread = true;
                Ping ping = new Ping();
                string s = "Hello World";
                byte[] b = Encoding.ASCII.GetBytes(s);
                int timeout = 2000;
                PingOptions po = new PingOptions(64, false);
                do
                {
                    try
                    {
                        PingReply pr = ping.Send(ip, timeout);
                        if (pr.Status == IPStatus.Success)
                            onUpdateHandler(new MyEventArgs(true));
                        else
                            onUpdateHandler(new MyEventArgs(false));
                    }
                    catch (ThreadAbortException ex)
                    {
                        runThread = false;
                    }
                    catch (Exception ex)
                    {
                        ;
                    }
                    Thread.Sleep(5000);
                } while (runThread);
            }
            public class MyEventArgs : EventArgs
            {
                public bool bSuccess = false;
                public MyEventArgs(bool b)
                {
                    this.bSuccess = b;
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
        }//class connectTest

    }
    public class mySettings{
        public static string getConnectionString(){
            return Properties.Settings.Default.connectstring;
        }
    }
}
