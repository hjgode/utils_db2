using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace utils_db2
{
    static class Program
    {
        public static logger _logger;// = new logger(logger.logLevel.info);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            _logger = new logger(logger.logLevel.info);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new mainForm());
        }
    }
}
