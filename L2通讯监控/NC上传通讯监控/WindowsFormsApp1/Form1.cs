using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
            path = @"D:\"+ ConfigurationManager.AppSettings["path"];
            DirectoryInfo TheFolder = new DirectoryInfo(path);
            label1.Text =path+"120分钟不动，重启通讯";
            remark rmk = new remark();
            rmk.id = (++id).ToString();
            rmk.des = "监控启动";
            rmk.time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            remarkList.Add(rmk);
            dataGridView1.DataSource = remarkList;
           
        }
        public DateTime lasttime=DateTime.Now;
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            DirectoryInfo TheFolder = new DirectoryInfo(path);
            foreach (FileInfo NextFile in TheFolder.GetFiles("MES_Data*"))
            {
                if (NextFile.LastWriteTime > lasttime)
                {
                    lasttime = NextFile.LastWriteTime;
                }
            }
            label2.Text = lasttime.ToString("hh:mm:ss");

            if ((DateTime.Now - lasttime).TotalMinutes > 120)
            {
                CleanMainExe("NCCaller");
                Thread.Sleep(1000);
                ReStartExe("NCCaller.exe");
                //重启通讯后，上次修改时间改为当前时间
                lasttime = DateTime.Now;
                Thread.Sleep(5000);
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

    }
}
