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

            gongxuConfig("2号转炉");
            gongxuConfig("3号转炉");
            gongxuConfig("4号转炉");
            gongxuConfig("34号铸机");
           
            gongxuConfig("5号铸机");
            gongxuConfig("6号铸机");
            gongxuConfig("7号铸机");
            gongxuConfig("135号精炼");
            gongxuConfig("2号精炼");
            gongxuConfig("脱硫站");
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




        public void getLfHeatid()
        {

            oraDbHelp service = new oraDbHelp();
            var ds = service.Query(" select * from GET_LF_HEATID");
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

        public void getDesHeatid()
        {

            oraDbHelp service = new oraDbHelp();
            var ds = service.Query(" select * from GET_DES_HEATID");
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

        int day = 0;
        public void getdianliangAndchanliang()
        {
            int curday = DateTime.Now.Day;
            if (curday != day)
            {
                int hour = DateTime.Now.Hour;
                if (hour == 7 || hour == 8)
                {
                    try
                    {
                        oraDbHelp service = new oraDbHelp();
                        var ds = service.Query(" select * from nengyuan_day where logtime='" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "'");
                        if (ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0 && ds != null)
                        {
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                Decimal chanliang = Convert.ToDecimal(item["tg"]);
                                Decimal dianhao = Convert.ToDecimal(item["dianhao"]);
                                string exeSql = "update dianliang_cfg set note='" + curday + "日数据已更新',val=" + chanliang + "where name='炼钢总产量'";
                                DbMySql.ExeSql(exeSql);
                                exeSql = "update dianliang_cfg set note='" + curday + "日数据已更新',val=" + dianhao + "where name='炼钢总电量'";
                                DbMySql.ExeSql(exeSql);
                                day = curday;
                            }
                        }
                    }
                    catch
                    {

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
                getLfHeatid();
                getDesHeatid();
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

        private void timer2_Tick(object sender, EventArgs e)
        {
            getdianliangAndchanliang();
        }
    }
}
