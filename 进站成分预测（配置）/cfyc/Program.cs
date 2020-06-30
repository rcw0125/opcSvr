using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using Rcw.Data;

namespace cfyc
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            DbContext.AddDataSource("ip", DbContext.DbType.Oracle, "192.168.36.151", "xgmes", "sbgl", "sbgl");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (existIp())
            {
                Application.Run(new Form1());
            }
            else
            {
                MessageBox.Show("IP不允许："+GetLocalIP());
                return;
            }       

        }
        /// <summary>
        /// 数据库是否存在该Ip的信息
        /// </summary>
        /// <returns></returns>
        public static bool existIp()
        {

            StringBuilder sbu = new StringBuilder();
            sbu.Clear();
            sbu.Append(" select count(*) from A_Z_JZCF_IP where ip='" + GetLocalIP() + "' ");
            var ds = DbContext.GetDataTable(sbu.ToString());
            if (ds.Rows.Count > 0 && ds != null)
            {
                if (ds.Rows[0][0].ToString() != "0")
                {
                    return true;
                }

            }
            return false;
        }
        public static string GetLocalIP()
        {
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                return "";
            }
            catch
            {
                return "";
            }

        }
    }
}
