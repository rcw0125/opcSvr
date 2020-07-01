using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace L2Ctr
{
    public partial class Form1 : Form
    {
        public List<remark> remarkList = null;
        public Form1()
        {
            InitializeComponent();
        }
        public int id = 0;

        public bool shangchuan = true;
        public bool autotx = false;
        public bool autocomm = false;

        public int hour = 0;
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MessageBox.Show("程序将最小化到系统托盘区");

            e.Cancel = true; // 取消关闭窗体
            this.Hide();
            this.ShowInTaskbar = false;//取消窗体在任务栏的显示
            this.notifyIcon1.Visible = true;//显示托盘图标
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Show();
                this.ShowInTaskbar = true;
                this.notifyIcon1.Visible = false;
            }
        }
        public string path = "";
        private void Form1_Load(object sender, EventArgs e)
        {
            remarkList = new List<remark>();
            //设置了通讯监控路径，才开始监控
            if (ConfigurationManager.AppSettings["path"]!=null&&ConfigurationManager.AppSettings["path"].ToString() != "")
            {
                autotx = true;
            }
            if (autotx)
            {
                path = @"D:\" + ConfigurationManager.AppSettings["path"];
                DirectoryInfo TheFolder = new DirectoryInfo(path);
                label1.Text = path + "开始监控（100秒不动，重启通讯）";
                remark rmk = new remark();
                rmk.id = (++id).ToString();
                rmk.des = "监控启动";
                rmk.time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                remarkList.Add(rmk);
            }
            else
            {
                dataGridView1.Visible = false;
                label2.Text = "";
                label1.Text = "";
            }

            if (GetLocalIP() == "192.168.48.163")
            {
                autocomm = true;
            }
            else
            {
                timer2.Enabled = false;
            }
            
            dataGridView1.DataSource = remarkList;
           
        }
        public DateTime lasttime=DateTime.Now;
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            timer1.Enabled = false;
            if (autotx)
            {
                DirectoryInfo TheFolder = new DirectoryInfo(path);
                foreach (FileInfo NextFile in TheFolder.GetFiles("L2CommError*"))
                {
                    if (NextFile.LastWriteTime > lasttime)
                    {
                        lasttime = NextFile.LastWriteTime;
                    }
                }
                label2.Text = lasttime.ToString("hh:mm:ss");

                if ((DateTime.Now - lasttime).TotalSeconds > 100)
                {
                    CleanMainExe("L2Comm");
                    Thread.Sleep(1000);
                    ReStartExe("L2Comm.bat");
                    //重启通讯后，上次修改时间改为当前时间
                    lasttime = DateTime.Now;
                    Thread.Sleep(5000);
                }
            }

            if (shangchuan)
            {
                if (DateTime.Now.Hour != hour)
                {
                    hour = DateTime.Now.Hour;
                    dianjian(hour);
                }

            }
            timer1.Enabled = true;
        }


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
        /// <summary>
        /// 重启主程序 (bat 与exe程序是不一样的)
        /// </summary>
        private void ReStartExe(string mainExe)
        {
             
            Process proc = null;
            try
            {
               
                proc = new Process();
                proc.StartInfo.WorkingDirectory =  path + @"\";
                proc.StartInfo.FileName = mainExe;
                proc.StartInfo.Arguments = string.Format("10");//this is argument          
                proc.Start();
               // proc.WaitForExit();
                remark rmk = new remark();
                rmk.id = (++id).ToString();
                rmk.des = "自动重启";
                rmk.time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                remarkList.Add(rmk);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = remarkList;
            }
            catch (Exception ex)
            {
               
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


        public string GetLocalIP()
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

        private void button1_Click(object sender, EventArgs e)
        {
            dianjian(DateTime.Now.Hour,"手动");
        }

        public void dianjian(int hour,string type="自动")
        {
            try
            {
                var db = new DbHelp("192.168.36.162", "cmsdb", "sa", "xgmes");
                string sql = "insert into a_ip_jl(ip,name,hour,type) values('" + GetLocalIP() + "','" + GetComputerName() + "'," + hour + ",'" + type + "')";
                db.ExeSql(sql);
                label1.Text = DateTime.Now.ToString("HH:mm:ss")+ "上传成功！";
            }
            catch
            {
                shangchuan = false;
            }
            
        }


        public int commcount = 1;
        public void commIPlist()
        {
            string pre = DateTime.Now.ToString("MMdd");
            Int32 curcount = Convert.ToInt32(pre + "00000");
            curcount += commcount;
           
            comm("192.168.48.201", "服务器1，群集1：内网等应用", curcount);
            comm("192.168.48.202", "服务器2，群集1：内网等应用", curcount);
            comm("192.168.48.203", "服务器3，群集1：内网等应用", curcount);
            comm("192.168.48.204", "服务器4，群集2：WinnCC等生产应用", curcount);
            comm("192.168.48.205", "服务器5，群集2：WinnCC等生产应用", curcount);
            comm("192.168.48.206", "服务器6，群集2：WinnCC等生产应用", curcount);
            comm("192.168.48.207", "服务器7，群集3：MES通讯", curcount);
            comm("192.168.48.208", "服务器8，群集3：MES通讯", curcount);
            comm("192.168.48.209", "服务器9，虚拟机：群集管理器", curcount);
            comm("192.168.48.200", "服务器0，群集4：群集管理器", curcount);

            comm("192.168.36.151", "MES服务器，数据库备用", curcount);
            comm("192.168.36.153", "MES服务器，数据库在线", curcount);
            comm("192.168.36.155", "MES服务器，应用备用", curcount);
            comm("192.168.36.157", "MES服务器，应用在线", curcount);
            comm("192.168.36.159", "MES服务器，实时数据库", curcount);
            comm("192.168.36.169", "MES服务器，数据采集工程站", curcount);
            comm("192.168.36.167", "MES服务器，数据采集机", curcount);
            comm("192.168.36.168", "MES服务器，数据采集机", curcount);

        }
        /// <summary>
        /// ping IP地址 写入到数据库
        /// </summary>
        /// <param name="ip"></param>
        public void comm(string ip,string name,Int32 count=0)
        {


            try
            {
                int online = 0, offline = 0;
                Ping ping = new Ping();
                for (int i = 0; i < 6; i++)
                {
                    PingReply pingReply = ping.Send(ip);
                    if (pingReply.Status == IPStatus.Success)
                    {
                        online++;

                    }
                    else
                    {
                        offline++;
                    }
                }
                var db = new DbHelp("192.168.36.162", "cmsdb", "sa", "xgmes");
                string sql = "insert into a_ip_comm(ip,online,offline,count,name) values('" + ip + "'," + online + "," + offline + ","+count+",'"+name+"')";
                db.ExeSql(sql);

            }
            catch
            {

            }

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            if (autocomm)
            {
                commcount++;
                commIPlist();
            }
            timer2.Enabled = true;
        }
    }
}
