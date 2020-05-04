using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using opcBase;

namespace OPCWcf
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
            //if (PlcSvr.GetInstance().opc_connected)
            //{
            //    Application.Run(new Form1());
            //}
           
        }
    }
}
