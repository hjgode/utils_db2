using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace myLogger
{
    public class logger
    {
        string sFile = "";
        System.IO.StreamWriter sw = null;
        logLevel _logLevel = logLevel.info;

        //make single instance
        static System.IO.StreamWriter _streamwriter = null;
        static System.IO.StreamWriter getStreamwriter(string filename) {
            if (_streamwriter == null)
            {
                _streamwriter = new System.IO.StreamWriter(filename + "utils_data.log", true);
                _streamwriter.AutoFlush = true;
            }
            return _streamwriter;
        }
        public enum logLevel
        {
            info,
            warning,
            error,
            debug,
        }
        public logger(logLevel ll) {
            _logLevel = ll;
            sFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            sFile = new Uri(sFile).LocalPath;
            if (!sFile.EndsWith(@"\"))
                sFile += @"\";
            sw = getStreamwriter(sFile);// new System.IO.StreamWriter(sFile + "utils_data.log", true);
        }
        public void log(string s)
        {
            System.Diagnostics.Debug.WriteLine(s);
            switch (_logLevel)
            {
                case logLevel.info:
                    this.info(s);
                    break;
                case logLevel.warning:
                    this.warning(s);
                    break;
                case logLevel.error:
                    this.error(s);
                    break;
                case logLevel.debug:
                    this.debug(s);
                    break;
            }
        }

        void info(string s){
            writeTimeStamp();
            sw.WriteLine(s);
        }
        void warning(string s)
        {
            writeTimeStamp();
            sw.WriteLine(s);
        }
        void error(string s)
        {
            writeTimeStamp();
            sw.WriteLine(s);
        }
        void debug(string s)
        {
            writeTimeStamp();
            sw.WriteLine(s);
        }
        void writeTimeStamp()
        {
            DateTime dt = DateTime.Now;
            sw.Write( String.Format("{0}{1}{2}{3}{4}{5} ",
                dt.Year.ToString("0000"), dt.Month.ToString("00"), dt.Day.ToString("00"),
                dt.Hour.ToString("00"), dt.Minute.ToString("00"), dt.Second.ToString("00")));
        }
    }
}
