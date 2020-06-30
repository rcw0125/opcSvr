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

namespace netcomm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public int hour = 0;     
        public IpData ipdata = new IpData();
        private void Form1_Load(object sender, EventArgs e)
        {
            setupIpList();
            dataGridView1.DataSource = ipdata;
        }
        public DateTime lasttime=DateTime.Now;
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var item in ipdata)
            {
                item.online = 0;
                item.offline = 0;
            }
            //后台工作开始进行
            backgroundWorker1.RunWorkerAsync();
        }
        #region timer:每小时上传一次检测结果
        public int commcount = 1;
        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;

            if (DateTime.Now.Hour != hour)
            {
                hour = DateTime.Now.Hour;
                commIPlist();
            }
            timer2.Enabled = true;
        }
        public void commIPlist()
        {
            string pre = DateTime.Now.ToString("MMdd");
            Int32 curcount = Convert.ToInt32(pre + "00000");
            curcount += commcount;
            foreach (var item in ipdata)
            {
                comm(item.ip, item.name, curcount);
            }
        }
        /// <summary>
        /// ping IP地址 写入到数据库
        /// </summary>
        /// <param name="ip"></param>
        public void comm(string ip, string name, Int32 count = 0)
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
                if (offline > 0)
                {
                    var db = new DbHelp("192.168.36.162", "cmsdb", "sa", "xgmes");
                    string sql = "insert into a_ip_comm(ip,online,offline,count,name) values('" + ip + "'," + online + "," + offline + "," + count + ",'" + name + "')";
                    db.ExeSql(sql);
                    commcount++;
                }

            }
            catch
            {

            }

        }
        #endregion

        #region ip地址列表初始化
        public void setupIpList()
        {
            addIpInfo("192.168.36.151", "MES服务器，数据库备用", 1);
            addIpInfo("192.168.36.153", "MES服务器，数据库在线", 2);
            addIpInfo("192.168.36.155", "MES服务器，应用备用", 3);
            addIpInfo("192.168.36.157", "MES服务器，应用在线", 4);
            addIpInfo("192.168.36.159", "MES服务器，实时数据库", 5);
            addIpInfo("192.168.36.169", "MES服务器，数据采集工程站", 6);
            addIpInfo("192.168.36.167", "MES服务器，数据采集机", 7);
            addIpInfo("192.168.36.168", "MES服务器，数据采集机", 8);
            addIpInfo("192.168.48.201", "服务器1，群集1：内网等应用", 9);
            addIpInfo("192.168.48.202", "服务器2，群集1：内网等应用", 10);
            addIpInfo("192.168.48.203", "服务器3，群集1：内网等应用", 11);
            addIpInfo("192.168.48.204", "服务器4，群集2：WinnCC等生产应用", 12);
            addIpInfo("192.168.48.205", "服务器5，群集2：WinnCC等生产应用", 13);
            addIpInfo("192.168.48.206", "服务器6，群集2：WinnCC等生产应用", 14);
            addIpInfo("192.168.48.207", "服务器7，群集3：MES通讯", 15);
            addIpInfo("192.168.48.208", "服务器8，群集3：MES通讯", 16);
            addIpInfo("192.168.48.209", "服务器9，虚拟机：群集管理器", 17);
            addIpInfo("192.168.48.200", "服务器10，群集4：群集管理器", 18);
            addIpInfo("192.168.48.22", "设备科计算机：数据库备份", 19);
            addIpInfo("192.168.48.230", "虚拟机：数据上传", 20);
            addIpInfo("192.168.48.232", "虚拟机：数据上传", 21);
            addIpInfo("192.168.48.239", "虚拟机：AOD数据上传", 22);
            addIpInfo("192.168.48.243", "虚拟机：3、4#机大包重量下传", 23);

        }
        public void addIpInfo(string ip, string name, int idx)
        {
            ipinfo curipinfo = new ipinfo();
            curipinfo.ip = ip;
            curipinfo.name = name;
            curipinfo.idx = idx;
            //ipList.Add(curipinfo);
            ipdata.Add(curipinfo);
        }

        #endregion

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;

            bw.ReportProgress(1, "开始检测IP地址");
            Ping ping = new Ping();
            int count = ipdata.Count;
            if (count < 1)
            {
                count = 20;
            }
            foreach (var item in ipdata)
            {
                for (int i = 0; i < 6; i++)
                {
                    PingReply pingReply = ping.Send(item.ip);
                    if (pingReply.Status == IPStatus.Success)
                    {
                        item.online++;
                    }
                    else
                    {
                        item.offline++;
                    }
                }
                int process = 1 + Convert.ToInt32(item.idx * 99 / count);
                bw.ReportProgress(process, item.name+"检测完毕");
            }
            bw.ReportProgress(100, "检测完毕");
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 3)
                {
                    if (e.Value != null && Convert.ToDouble(e.Value) > 0)
                    {
                        e.CellStyle.BackColor = Color.Red;
                    }
                }
            }
            catch
            {

            }
        }
    }

    public class IpData : BindingList<ipinfo>
    {     
        public void Refresh()
        {
            ListChangedEventArgs arg = new ListChangedEventArgs(ListChangedType.Reset, -1);
            this.OnListChanged(arg);
        }
    }
    public class ipinfo: INotifyPropertyChanged
    { 
        public int idx { get; set; }
        public string ip { get; set; }
        private int _online = 0;
        public int online
        {
            get
            {
                return _online;
            }
            set
            {
                _online = value;
                RaisePropertyChanged("online");

            }
        }
        private int _offline = 0;
        public int offline
        {
            get
            {
                return _offline;
            }
            set
            {
                _offline = value;
                RaisePropertyChanged("offline");

            }
        }
        public string name { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

    }
}
