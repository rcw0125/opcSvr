using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;

namespace L2Ctr
{
    public partial class PingForm : Form
    {
        public PingForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comm("192.168.36.5");
            comm("192.168.36.80");

        }

        public void comm(string ip)
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
                string sql = "insert into a_ip_comm(ip,online,offline) values('" + ip + "',"  + online + "," + offline + ")";
                db.ExeSql(sql);
                
            }
            catch
            {
               
            }

        }
    }
}
