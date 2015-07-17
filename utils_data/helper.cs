using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils_data
{
    class helper
    {
        static string appPath = "";
        public static string getAppPath
        {
            get
            {
                if (appPath == "")
                {
                    string AppPath;
                    AppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                    if (!AppPath.EndsWith(@"\"))
                        AppPath += @"\";
                    Uri uri = new Uri(AppPath);
                    AppPath = uri.AbsolutePath;
                    appPath = AppPath;
                }
                return appPath;
            }
        }
    }
    public class mySettings
    {
        public static string getConnectionString()
        {
            return Properties.Settings.Default.connectstring;
        }
    }

}

