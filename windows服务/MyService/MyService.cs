using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace MyService
{
    partial class MyService : ServiceBase
    {
        public MyService()
        {
            InitializeComponent();
        }

        public int initvalue = 0;

        public int count = 0;

        /// <summary>
        /// 服务启动
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            try
            {              
                timer1.Enabled = true;
                timer2.Enabled = true;
                Log(GetLocalIP(), getMac(), GetComputerName(), count.ToString(), "开机");
                // 先检查数据库是否存在该IP ，不存在，则记录
                if (!existIp())
                {
                    LogComputer(GetLocalIP());
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("创建服务时失败");

            }
          
        }

        #region 数据库是否存在该Ip的信息
        /// <summary>
        /// 数据库是否存在该Ip的信息
        /// </summary>
        /// <returns></returns>
        public bool existIp()
        {

            StringBuilder sbu = new StringBuilder();
            sbu.Clear();
            sbu.Append(" select count(*) from A_COMPUTER where c_ip='" + GetLocalIP() + "' ");
            var ds = DbHelper.Query(sbu.ToString());
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0 && ds != null)
            {
                if (ds.Tables[0].Rows[0][0].ToString() != "0")
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region  关闭主程序

        /// <summary>
        /// 关闭主程序
        /// </summary>
        /// <param name="mainExe"></param>
        private void CleanMainExe(string mainExe)
        {
            if (mainExe == "")
            {
                return;
            }
            Process[] processes = Process.GetProcesses();
            for (int i = 0; i < processes.Length; i++)
            {
                if (processes[i].ProcessName.ToString().Contains(mainExe.Replace(".exe", "")))
                {
                    processes[i].Kill();
                }
            }
        }
        #endregion

  

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
        }


        #region 5秒钟检测一次是否MES服务发生故障

        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                queryValue();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 向数据库查询是否需要重启，并执行
        /// </summary>
        public void queryValue()
        {
            #region 1、检查是否打开MES客户端，没有客户端，则返回
            if (!existMes())
            {
                initvalue = 0;
                return;
            }
            #endregion

            #region 2、向数据库查询监测值是否发生变化，发生变化，则关闭MES客户端
            StringBuilder sbu = new StringBuilder();
            sbu.Clear();
            sbu.Append(" select value from A_RESET ");
            var ds = DbHelper.Query(sbu.ToString());
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0 && ds != null)
            {
                if (ds.Tables[0].Rows[0]["value"].ToString() != initvalue.ToString())
                {

                    if (initvalue == 0)
                    {
                        initvalue = Convert.ToInt32(ds.Tables[0].Rows[0]["value"].ToString());
                    }
                    else
                    {
                        initvalue = Convert.ToInt32(ds.Tables[0].Rows[0]["value"].ToString());
                        CleanMainExe("XGMESMain.exe");
                        Thread.Sleep(1000);
                        //存在，则表示关闭进程失败
                        if (existMes())
                        {
                            LogReset(GetLocalIP(), "失败", GetComputerName());
                        }
                        else//不存在，则表示关闭进程成功
                        {
                            LogReset(GetLocalIP(), "成功", GetComputerName());
                        }

                    }

                }
            }
            #endregion


        }
        #endregion


        #region 是否打开了MES程序
        /// <summary>
        /// 是否打开了MES程序
        /// </summary>
        /// <returns></returns>
        public bool existMes()
        {
            string mainExe = "XGMESMain.exe";
            Process[] processes = Process.GetProcesses();
            for (int i = 0; i < processes.Length; i++)
            {
                if (processes[i].ProcessName.ToString().Contains(mainExe.Replace(".exe", "")))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region 记录客户端的运行情况
        /// <summary>
        /// 记录客户端的运行情况 2个小时一次   A_Client_Log
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="cpu_id"></param>
        /// <param name="name"></param>
        /// <param name="timeCount"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public string Log(string ip, string cpu_id, string name, string timeCount, string account)
        {
            StringBuilder sbu = new StringBuilder();
            sbu.Append(" insert into A_Client_Log(C_IP, C_CPU_ID, C_NAME, C_TimeCount,C_Account) ");
            sbu.Append("values('" + ip + "',");
            sbu.Append("'" + cpu_id + "',");
            sbu.Append("'" + name + "',");
            sbu.Append("'" + timeCount + "',");
            sbu.Append("'" + account + "')");
            int result = DbHelper.ExecuteSql(sbu.ToString());
            return result.ToString();
        }
        #endregion


        #region 数据库中，没有该iP信息，则上传IP
        public string LogComputer(string ip)
        {
            StringBuilder sbu = new StringBuilder();
            sbu.Append(" insert into A_COMPUTER(C_IP) ");
            sbu.Append("values('" + ip + "')");
            int result = DbHelper.ExecuteSql(sbu.ToString());
            return result.ToString();
        }
        #endregion

        #region 记录客户端关闭情况
        /// <summary>
        /// 记录客户端关闭情况
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string LogReset(string ip, string type, string name)
        {
            StringBuilder sbu = new StringBuilder();
            sbu.Append(" insert into A_RESET_LOG(C_IP,C_NAME,C_TYPE) ");
            sbu.Append("values('" + ip + "','" + name + "','" + type + "')");
            int result = DbHelper.ExecuteSql(sbu.ToString());
            return result.ToString();
        }
        #endregion

        #region 获取IP地址、MAC地址，计算机名等方法

        #region 获取iP地址
        public static string GetLocalIP()
        {
            try
            {
                string ip = "";
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (IpEntry.AddressList[i].ToString().StartsWith("192.168.36") || IpEntry.AddressList[i].ToString().StartsWith("192.168.48") || (IpEntry.AddressList[i].ToString().StartsWith("192.168.2")))
                        {
                            return IpEntry.AddressList[i].ToString();
                        }
                        ip = IpEntry.AddressList[i].ToString();


                    }
                }
                return ip;
            }
            catch
            {
                return "";
            }

        }
        #endregion
        /// <summary>
        /// 变为获取Mac地址
        /// </summary>
        /// <returns></returns>
        public string getMac()
        {

            try
            {
                string strMac = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        strMac = mo["MacAddress"].ToString();
                    }
                }
                moc = null;
                mc = null;
                return strMac;
            }
            catch
            {
                return "unknown";
            }

        }
        public string GetComputerName()
        {
            try
            {
                return System.Environment.GetEnvironmentVariable("ComputerName");
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }
        #endregion


        int hour = 0;
        /// <summary>
        /// 定时（6分钟），13点、23点上传一次开机情况
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (DateTime.Now.Hour != hour)
                {
                    hour = DateTime.Now.Hour;               
                    if (hour == 13 || hour == 23 || hour == 5)
                    {                    
                        string strExist = "";
                        if (existMes())
                        {
                            strExist = "使用";
                        }
                        else
                        {
                            strExist = "无";
                        }
                        Log(GetLocalIP(), getMac(), GetComputerName(), hour.ToString(), strExist);
                    }               
                }
                
            }
            catch
            {

            }

           
        }

        #region 不再执行 定时（1分钟）检测有多少客户端

        public int shangchuan = 0;
        /// <summary>
        ///  定时（1分钟）检测有多少客户端
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer3_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                StringBuilder sbu = new StringBuilder();
                sbu.Clear();
                sbu.Append(" select shangchuan from A_SHANGCHUAN ");
                var ds = DbHelper.Query(sbu.ToString());
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0 && ds != null)
                {
                    if (ds.Tables[0].Rows[0]["shangchuan"].ToString() != shangchuan.ToString())
                    {
                        shangchuan = Convert.ToInt32(ds.Tables[0].Rows[0]["shangchuan"].ToString());
                        string strExist = "";
                        if (existMes())
                        {
                            strExist = "使用";
                        }
                        else
                        {
                            strExist = "无";
                        }
                        Log(GetLocalIP(), getMac(), GetComputerName(), "主动上传", strExist);
                    }
                }
            }
            catch
            {
            }
        }

        #endregion

    }
}
