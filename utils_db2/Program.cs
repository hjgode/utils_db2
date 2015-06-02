﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace utils_db2
{
    static class Program
    {
        public static frmSplash frm;
        public static myLogger.logger _logger;// = new logger(logger.logLevel.info);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            _logger = new myLogger.logger(myLogger.logger.logLevel.info);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            frm = new frmSplash();
            frm.lblStatus.Text = "please wait...";
            frm.Show();

            Application.Run(new mainForm());
        }
    }
}
