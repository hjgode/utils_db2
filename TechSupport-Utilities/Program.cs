using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;


//namespace TechSupport_Utilities
//{
    public static class Program
    {
        public static myLogger.logger _logger;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            _logger = new myLogger.logger(myLogger.logger.logLevel.info);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TechSupport_Utilities.mainForm());
        }
    }
//}
