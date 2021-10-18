using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MESRTrans
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            gongxuConfig("2#转炉");
            gongxuConfig("3#转炉");
            gongxuConfig("4#转炉");
            gongxuConfig("3#铸机");
            gongxuConfig("4#铸机");
            gongxuConfig("5#铸机");
            gongxuConfig("6#铸机");
            try
            {
                getBofHeatid();
                getCcmHeatid();
            }
            catch
            { 
            
            }
           

        }

        List<heatinfo> listHeatInfo = new List<heatinfo>();

        public void gongxuConfig(string gx)
        {
            heatinfo bof = new heatinfo();
            bof.gongxu = gx;
            listHeatInfo.Add(bof);
        }

        public void getBofHeatid()
        {

            oraDbHelp service = new oraDbHelp();
            var ds = service.Query(" select * from GET_BOF_HEATID");
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0 && ds != null)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    heatinfo ht = new heatinfo();
                    ht.gongxu = item["gongxu"].ToString();
                    ht.heatid = item["heatid"].ToString();
                    ht.begintime = item["begintime"].ToString();
                    ht.endtime = item["endtime"].ToString();
                    //将未生产完时间置为空。
                    if (ht.endtime.StartsWith("1899"))
                    {
                        ht.endtime = "";
                    }
                    var curgongxu = listHeatInfo.Find(o => o.gongxu == ht.gongxu);
                    if (ht.heatid != curgongxu.heatid || curgongxu.endtime != ht.endtime)
                    {
                        curgongxu.heatid = ht.heatid;
                        curgongxu.begintime = ht.begintime;
                        curgongxu.endtime = ht.endtime;
                        string exeSql = "update ems_luci set heatid='"+curgongxu.heatid+"',begintime='"+curgongxu.begintime+"',endtime='"+curgongxu.endtime+"' where gongxu='"+curgongxu.gongxu+"'";
                        DbMySql.ExeSql(exeSql);
                    }


                }
            }
        }

        public void getCcmHeatid()
        {

            oraDbHelp service = new oraDbHelp();
            var ds = service.Query(" select * from GET_CCM_HEATID");
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0 && ds != null)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    heatinfo ht = new heatinfo();
                    ht.gongxu = item["gongxu"].ToString();
                    ht.heatid = item["heatid"].ToString();
                    ht.begintime = item["begintime"].ToString();
                    ht.endtime = item["endtime"].ToString();
                    //将未生产完时间置为空。
                    if (ht.endtime.StartsWith("1899"))
                    {
                        ht.endtime = "";
                    }
                    var curgongxu = listHeatInfo.Find(o => o.gongxu == ht.gongxu);
                    if (ht.heatid != curgongxu.heatid || curgongxu.endtime != ht.endtime)
                    {
                        curgongxu.heatid = ht.heatid;
                        curgongxu.begintime = ht.begintime;
                        curgongxu.endtime = ht.endtime;
                        string exeSql = "update ems_luci set heatid='" + curgongxu.heatid + "',begintime='" + curgongxu.begintime + "',endtime='" + curgongxu.endtime + "' where gongxu='" + curgongxu.gongxu + "'";
                        DbMySql.ExeSql(exeSql);
                    }


                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                getBofHeatid();
                getCcmHeatid();
            }
            catch
            {

            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("不要关闭,请点“取消”", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listHeatInfo;
        }
    }
}
