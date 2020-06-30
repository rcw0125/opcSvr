using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Configuration.Install;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Management;

namespace installService
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string dept = "";
        string user = "";
        string note = "";
        private void Form1_Load(object sender, EventArgs e)
        {
            tbIP.Text = GetLocalIP();
            //            select c_ip, c_dept, c_user from(select c_ip, c_dept, c_user from a_ip where c_ip = '192.168.48.6' order by c_ts desc)
            //where rownum = 1
            string sqlstr = " select c_ip, c_dept, c_user,c_note from(select c_ip, c_dept, c_user,c_note from a_ip where c_ip = '"+ tbIP.Text.Trim() + "' order by c_ts desc) ";
            sqlstr += " where rownum = 1 ";
            var dt = DbHelper.Query(sqlstr);
            if (dt != null&&dt.Tables.Count>0&&dt.Tables[0].Rows.Count>0)
            {
                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    comboBox1.Text = dr["c_dept"].ToString();
                    dept= dr["c_dept"].ToString();
                    tbsyr.Text= dr["c_user"].ToString();
                    user= dr["c_user"].ToString();
                    comboBox2.Text= dr["c_note"].ToString();
                    note = dr["c_note"].ToString();
                }
            }
           
        }

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

        /// <summary>
        /// 操作系统版本
        /// </summary>
        public string GetOS_Version()
        {
            string str = "Windows 10";
            try
            {
                string hdId = string.Empty;
                ManagementClass hardDisk = new ManagementClass(WindowsAPIType.Win32_OperatingSystem.ToString());
                ManagementObjectCollection hardDiskC = hardDisk.GetInstances();
                foreach (ManagementObject m in hardDiskC)
                {
                    str = m[WindowsAPIKeys.Name.ToString()].ToString().Split('|')[0].Replace("Microsoft", "");
                    break;
                }
            }
            catch
            {

            }
            return str;
        }

        /// <summary>
        /// 查找cpu的名称，主频, 核心数
        /// </summary>
        /// <returns></returns>
        public Tuple<string, string> GetCPU()
        {
            Tuple<string, string> result = null;
            try
            {
                string str = string.Empty;
                ManagementClass mcCPU = new ManagementClass(WindowsAPIType.Win32_Processor.ToString());
                ManagementObjectCollection mocCPU = mcCPU.GetInstances();
                foreach (ManagementObject m in mocCPU)
                {
                    string name = m[WindowsAPIKeys.Name.ToString()].ToString();
                    string[] parts = name.Split('@');
                    result = new Tuple<string, string>(parts[0].Split('-')[0] + "处理器", parts[1]);
                    break;
                }

            }
            catch
            {

            }
            return result;
        }

        #endregion


        string serviceFilePath = System.AppDomain.CurrentDomain.BaseDirectory + @"ms\MyService.exe";
        string serviceName = "MyService";
        private void button1_Click(object sender, EventArgs e)
        {

            //判断服务是否存在，存在则卸载。
            if (!this.IsServiceExisted(serviceName))
            {
                //安装服务
                this.InstallService(serviceFilePath);
                ServiceStart(serviceName);
            }
            if (comboBox1.Text.Trim() == "")
            {
                MessageBox.Show("请选择所在单位！");
                return;
            }
            if (comboBox2.Text.Trim() == "")
            {
                MessageBox.Show("请选择备注内容！");
                return;
            }
            if (tbIP.Text.Contains("192.168.48") || tbIP.Text.Contains("192.168.36"))
            {
                if (comboBox1.Text.Trim() != dept || tbsyr.Text.Trim() != user||comboBox2.Text.Trim()!=note)
                {
                    StringBuilder sbu = new StringBuilder();
                    string cpustr = "";
                    var cpu = GetCPU();
                    if (cpu != null)
                    {
                        cpustr = cpu.Item1 + cpu.Item2;
                    }
                    sbu.Append(" insert into a_ip(c_ip, c_mac, c_name, c_dept,c_note,c_cpu,c_os, c_user) ");
                    sbu.Append("values('" + tbIP.Text + "',");
                    sbu.Append("'" + getMac() + "',");
                    sbu.Append("'" + GetComputerName() + "',");
                    sbu.Append("'" + comboBox1.Text + "',");
                    sbu.Append("'" + comboBox2.Text.Trim() + "',");
                    sbu.Append("'" + cpustr + "',");
                    sbu.Append("'" + GetOS_Version() + "',");
                    sbu.Append("'" + tbsyr.Text.Trim() + "')");
                    int result = DbHelper.ExecuteSql(sbu.ToString());
                    dept = comboBox1.Text.Trim();
                    user = tbsyr.Text.Trim();
                    note= comboBox2.Text.Trim();
                    MessageBox.Show("上传成功！");
                }
                else
                {
                    MessageBox.Show("操作完成！");
                }
                
            }
            else
            {
                MessageBox.Show("没有获取到有效的IP地址,无法注册！");
            }
           
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string path = "" + @"I:\2019归档\1、MES系统\MES服务监控\windows服务48.9\installService\bin\Debug\ms\MyService.exe";
            UninstallService(path);
        }
        //判断服务是否存在
        private bool IsServiceExisted(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController sc in services)
            {
                if (sc.ServiceName.ToLower() == serviceName.ToLower())
                {
                    return true;
                }
                
            }
            return false;
        }

        //安装服务
        private void InstallService(string serviceFilePath)
        {
            using (AssemblyInstaller installer = new AssemblyInstaller())
            {
                installer.UseNewContext = true;
                installer.Path = serviceFilePath;
                IDictionary savedState = new Hashtable();
                installer.Install(savedState);
                installer.Commit(savedState);
            }
        }

        //卸载服务
        private void UninstallService(string serviceFilePath)
        {
            using (AssemblyInstaller installer = new AssemblyInstaller())
            {
                installer.UseNewContext = true;
                installer.Path = serviceFilePath;
                installer.Uninstall(null);
            }
        }
        //启动服务
        private void ServiceStart(string serviceName)
        {
            using (ServiceController control = new ServiceController(serviceName))
            {
                if (control.Status == ServiceControllerStatus.Stopped)
                {
                    control.Start();
                }
            }
        }

        //停止服务
        private void ServiceStop(string serviceName)
        {
            using (ServiceController control = new ServiceController(serviceName))
            {
                if (control.Status == ServiceControllerStatus.Running)
                {
                    control.Stop();
                }
            }
        }

      
    }

    /// <summary>
    /// windows api 名称
    /// </summary>
    public enum WindowsAPIType
    {
        /// <summary>
        /// 内存
        /// </summary>
        Win32_PhysicalMemory,
        /// <summary>
        /// cpu
        /// </summary>
        Win32_Processor,
        /// <summary>
        /// 硬盘
        /// </summary>
        win32_DiskDrive,
        /// <summary>
        /// 电脑型号
        /// </summary>
        Win32_ComputerSystemProduct,
        /// <summary>
        /// 分辨率
        /// </summary>
        Win32_DesktopMonitor,
        /// <summary>
        /// 显卡
        /// </summary>
        Win32_VideoController,
        /// <summary>
        /// 操作系统
        /// </summary>
        Win32_OperatingSystem

    }

    public enum WindowsAPIKeys
    {
        /// <summary>
        /// 名称
        /// </summary>
        Name,
        /// <summary>
        /// 显卡芯片
        /// </summary>
        VideoProcessor,
        /// <summary>
        /// 显存大小
        /// </summary>
        AdapterRAM,
        /// <summary>
        /// 分辨率宽
        /// </summary>
        ScreenWidth,
        /// <summary>
        /// 分辨率高
        /// </summary>
        ScreenHeight,
        /// <summary>
        /// 电脑型号
        /// </summary>
        Version,
        /// <summary>
        /// 硬盘容量
        /// </summary>
        Size,
        /// <summary>
        /// 内存容量
        /// </summary>
        Capacity,
        /// <summary>
        /// cpu核心数
        /// </summary>
        NumberOfCores
    }
}
