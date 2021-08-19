using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Rcw.Data;

namespace SBGL
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
            DbContext.AddDataSource("xgmesweb", DbContext.DbType.Oracle, "192.168.36.153", "xgmes", "xgwebrpt", "xgwebrpt");
            DbContext.AddDataSource("zhgl", DbContext.DbType.SqlServer, "192.168.48.8", "cmsdb", "sa", "sa");
            Rcw.Data.DbContext.AddDataSource("sbgl", DbContext.DbType.MySql, "192.168.36.111", "lgsb", "root", "root");
           // Rcw.Data.DbContext.AddDataSource("sbgl", DbContext.DbType.MySql, "192.168.36.111", "root", "sbgl", "root");
            Application.Run(new Form1());
        }
    }
}
