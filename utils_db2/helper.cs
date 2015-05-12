using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils_db2
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
}
