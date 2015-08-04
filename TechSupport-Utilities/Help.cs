using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;

namespace TechSupport_Utilities
{
    public partial class Help : Form
    {
        public string[] pages = new string[] {
            "index.html",
            "modelcode_firmware.htm",
            "software_version.htm",
            "reflash.htm",
        };
        public string[] pagecodes = new string[]{
            "start",
            "model_code",
            "model_code_done",
            "reflash",
        };
        public Help()
        {
            InitializeComponent();
            webBrowser1.ObjectForScripting = new ScriptInterface(this);
            navigate(pages[0]);
        }

        public void navigate(string sFile)
        {
            webBrowser1.Navigate(new Uri("file://" + helper.appPath + "web\\" + sFile));
        }

        [System.Runtime.InteropServices.ComVisibleAttribute(true)]
        public class ScriptInterface
        {
            public ScriptInterface(Help frm)
            {
                _helpform = frm;
            }
            Help _helpform = null;
            public void callMe(string s)
            {
                if (s == "modelcode")
                    _helpform.navigate(_helpform.pages[1]);
                if (s == "modelcode_done")
                    _helpform.navigate(_helpform.pages[2]);
                if (s == "reflash")
                    _helpform.navigate(_helpform.pages[3]);
                ;// Do something interesting
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }

    }
}
