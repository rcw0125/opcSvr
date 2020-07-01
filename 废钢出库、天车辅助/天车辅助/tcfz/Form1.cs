using Rcw.Method;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tcfz
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            lbts3.Text = "";
            lbts4.Text = "";
            button1_Click(null, null);
        }
        public string apiurl = "http://192.168.48.234/api/tc/";
        //public string apiurl = "http://localhost:53763/api/tc/";
        private void button1_Click(object sender, EventArgs e)
        {
            PlayVoice("欢迎使用");
            try
            {
                getccmweight();
                getlfplan();
                getccmplan();
                getccmtime();
            }
            catch
            {

            }
          
        }

        public void getccmweight()
        {
            string url =apiurl+ "getCcmWeight";
            List<ccm_weight> listweight = WebOper.HttpGet(url, typeof(List<ccm_weight>)) as List<ccm_weight>;
            foreach (var item in listweight)
            {
                if (item.ccmid == "3")
                {
                    lbccmweight3.Text = item.weight;
                    continue;
                }

                if (item.ccmid == "4")
                {
                    lbccmweight4.Text = item.weight;
                }

            }

            ycgw("S63", Convert.ToDouble(lbccmweight3.Text), lbts3);
            ycgw("S64", Convert.ToDouble(lbccmweight4.Text), lbts4);


        }
        int i3 = 0;
        int i4 = 0;
        /// <summary>
        /// 预测工位
        /// </summary>
        /// <param name="ccmid"></param>
        /// <param name="weight"></param>
        /// <param name="lb"></param>
        public void ycgw(string ccmid,double weight, DevExpress.XtraEditors.LabelControl lb)
        {
            if (weight > 18 || weight < 10)
            {
                if (ccmid == "S63")
                {
                    i3 = 0;
                }
                else
                {
                    i4 = 0;
                }
                lb.Text = "";
                return;
            }
            if (lb.Text.Trim() != "")
            {
                if (ccmid == "S63")
                {
                    if (i3 % 6 == 0)
                    {
                        PlayVoice(lb.Text.Trim());
                    }
                   
                }
                else
                {
                    if (i4 % 6 == 0)
                    {
                        PlayVoice(lb.Text.Trim());
                    }
                }   
                return;
            }
            if (ccmid == "S63")
            {
                i3++;
            }
            else
            {
                i4++;
            }
            string url = apiurl + "getCcmyc?ccmid="+ccmid;
            List<ccmyc> listplan = WebOper.HttpGet(url, typeof(List<ccmyc>)) as List<ccmyc>;
            if (listplan[0].name != "无")
            {
                lb.Text = listplan[0].name + ",请准备;";
                PlayVoice(lb.Text.Trim());
            }


        }

        public List<lfplan> listlf1 = new List<lfplan>();
        public List<lfplan> listlf2 = new List<lfplan>();

        public void getlfplan()
        {
            string url = apiurl + "getlfplan";
            List<lfplan> listplan = WebOper.HttpGet(url, typeof(List<lfplan>)) as List<lfplan>;
            gridControl1.DataSource = null;
            gridControl2.DataSource = null;
            listlf1.Clear();
            listlf2.Clear();
            foreach (var item in listplan)
            {
                if (item.code.Contains("S41"))
                {
                    item.code = item.code.Substring(3, 1);
                    listlf1.Add(item);
                    continue;
                }

                if (item.code.Contains("S42"))
                {
                    item.code = item.code.Substring(3, 1);
                    listlf2.Add(item);                 
                }

            }

            gridControl1.DataSource = listlf1;
            gridControl2.DataSource = listlf2;
        }

        public void getccmtime()
        {
            tbccmtime3.Text = "";
            tbccmtime4.Text = "";
            string url = apiurl + "getccmtime";
            List<ccmtime> listtime = WebOper.HttpGet(url, typeof(List<ccmtime>)) as List<ccmtime>;

            if (listtime != null && listtime.Count == 2)
            {
                tbccmtime3.Text = listtime[0].remaintime;
                tbccmtime4.Text = listtime[1].remaintime;

            }
           
        }

        public List<ccmplan> listccm = new List<ccmplan>();
        public void getccmplan()
        {
            if (!((Convert.ToDouble(lbccmweight3.Text) > 3 && Convert.ToDouble(lbccmweight3.Text) < 18)||(Convert.ToDouble(lbccmweight4.Text) >3  && Convert.ToDouble(lbccmweight4.Text) < 18)))
            {
                gridControl3.DataSource = null;
                return;
            }

            string msg3 = "";
            string msg4 = "";
            string url = apiurl + "getccmplan";
            List<ccmplan> listplan = WebOper.HttpGet(url, typeof(List<ccmplan>)) as List<ccmplan>;
            gridControl3.DataSource = null;
           
            //lbts3.Text = "";
            //lbts4.Text = "";

            listccm.Clear();
            foreach (var item in listplan)
            {
                listccm.Add(item);
                if (item.position.Contains("3"))
                {
                    if (item.lfid != "0")
                    {
                        msg3 +=  item.lfid.Substring(0, 1) + "号精炼炉出站，请准备;";
                        continue;
                    }
                    else
                    {
                        msg3 +=  item.bofid.Substring(0, 1) + "号转炉出钢请准备;";
                        continue;
                    }
                   
                }

                if (item.position.Contains("4"))
                {
                    if (item.lfid != "0")
                    {
                        msg4 += item.lfid.Substring(0, 1) + "号精炼炉出站，请准备;";
                    }

                    else
                    {
                        msg4 += item.bofid.Substring(0, 1) + "号转炉出钢请,准备;";

                    }

                }

            }

            gridControl3.DataSource = listccm;
            if (Convert.ToDouble(lbccmweight3.Text) > 10 && Convert.ToDouble(lbccmweight3.Text) < 18)
            {       
                if (msg3 != "")
                {
                    lbts3.Text = msg3;
                    PlayVoice(msg3);
                }
            }

            if (Convert.ToDouble(lbccmweight4.Text) > 10 && Convert.ToDouble(lbccmweight4.Text) < 18)
            {            
                if (msg4 != "")
                {
                    lbts4.Text = msg4;
                    PlayVoice(msg4);
                }
            }

          
        }


        /// <summary>
        /// 读声音
        /// </summary>
        /// <param name="msg"></param>
        private void PlayVoice(string msg)
        {          
            if (!string.IsNullOrEmpty(msg))
            {
                SoundReading sr = new SoundReading(msg);
                sr.Voice();
            }
        }

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            e.Appearance.BackColor = Color.LightGreen;
        }

        private void gridView2_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            e.Appearance.BackColor = Color.LightGreen;
        }

        private void gridView3_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            e.Appearance.BackColor = Color.LightGreen;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;           
            try
            {
                getccmweight();
                //getlfplan();
                //getccmplan();
            }
            catch
            {

            }
            timer1.Enabled = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            try
            {

                getlfplan();
                getccmplan();
                getccmtime();
            }
            catch
            {

            }
            timer2.Enabled = true;
        }
    }
}
