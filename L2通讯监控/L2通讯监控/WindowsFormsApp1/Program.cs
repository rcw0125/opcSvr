using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace L2Ctr
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SingleApplication.Run(new Form1());
            //SingleApplication.Run(new PingForm());

        }
    }
}
