﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//using OPCAutomation;
using opcBase;
using Rcw.Method;

namespace OPCWcf
{
    public partial class Form1 : Form
    {       
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }
        /// <summary>
        /// 水流量变量地址起始位置
        /// </summary>
        int sllwz = 44;
        /// <summary>
        /// 末端电磁搅拌变量地址起始位置
        /// </summary>
        int mdwz = 28;
        /// <summary>
        /// 煤气回收变量起始位置
        /// </summary>
        int mqhswz = 11;
        /// <summary>
        /// 3#、4#CCM大包重量起始地址
        /// </summary>
        int dbzlwz = 76;
        /// <summary>
        /// 大包剩钢起始位置
        /// </summary>
        int ladleweightwz = 78;
        meiqihuishou bof1 = new meiqihuishou();
        meiqihuishou bof2 = new meiqihuishou();
        meiqihuishou bof3 = new meiqihuishou();
        meiqihuishou bof4 = new meiqihuishou();
        List<meiqihuishou> listMqhs = new List<meiqihuishou>();

        dabaoshenggang dabaoshenggang5 = new dabaoshenggang();
         private void Form1_Load(object sender, EventArgs e)
        {


            //string sql = "select xuhao as id,L1name as name,scanrate,datatype from  L1OPC_TAG where used=1 and type=" + 0 + " order by id ";
            ////DbMySql.GetDataTable(sql);
            ////var dt = new sqlDbHelp().Query(sql);
            //var dtb = DbMySql.GetDataTable(sql);
            //var count = dtb.Rows.Count;
            setupDbsg();
            setupMqhs();
            setCasterWeightInfo();
            setCasterinfo();
           
            DateTime dt = DateTime.Now;
            label22.Text = dt.ToString("yyyy-MM-dd HH:mm:ss");
            //服务运行中
            serviceList sl = new serviceList();
            sl.Open();
            KepServer.GetInstance();



            //激活两个timer
            timer_mqhs.Enabled = true;
            timer_sll.Enabled = true;

            //2021-03-11 取消了大包剩钢的计算
            // timer_dabaoshenggang.Enabled = true;

        }

        /// <summary>
        /// 设置5#机大包剩钢参数
        /// </summary>
        public void setupDbsg()
        {
            ccm5dabaoshenggang.GetInstance().valid_status = ladleweightwz;
            //dabaoshenggang5.valid_status = ladleweightwz;
            //dabaoshenggang5.valid_tundishweight = ladleweightwz + 1;
            //dabaoshenggang5.valid_ladelweight = ladleweightwz + 2;
            //dabaoshenggang5.valid_A_weight = ladleweightwz + 3;
            //dabaoshenggang5.valid_B_weight = ladleweightwz + 4;
            //dabaoshenggang5.valid_genzongzhi1 = ladleweightwz + 5;
            //dabaoshenggang5.valid_genzongzhi2 = ladleweightwz + 6;
            //dabaoshenggang5.valid_genzongzhi3 = ladleweightwz + 7;
            //dabaoshenggang5.valid_genzongzhi4 = ladleweightwz + 8;
            ccm5dabaoshenggang.GetInstance().valid_A_weight = ladleweightwz + 2;
            ccm5dabaoshenggang.GetInstance().valid_B_weight = ladleweightwz + 3;
            ccm5dabaoshenggang.GetInstance().valid_ladelweight = ladleweightwz + 4;
            ccm5dabaoshenggang.GetInstance().valid_tundishweight = ladleweightwz + 5;
            
            
            ccm5dabaoshenggang.GetInstance().valid_genzongzhi1 = ladleweightwz + 6;
            ccm5dabaoshenggang.GetInstance().valid_genzongzhi2 = ladleweightwz + 7;
            ccm5dabaoshenggang.GetInstance().valid_genzongzhi3 = ladleweightwz + 8;
            ccm5dabaoshenggang.GetInstance().valid_genzongzhi4 = ladleweightwz + 9;
        }

        /// <summary>
        /// 根据变量ID获取变量的值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double getVal(int id)
         {
            try
            {

                return Convert.ToDouble(KepServer.GetInstance().getVal(id));
            }
            catch
            {
                return 0;
            }
             
         }
        
                     
        //15分钟执行一次
         private void timer1_Tick(object sender, EventArgs e)
         {
             /// 计算连铸机水流量、末端电磁搅拌数据
             getCasterInfo();
         }
         #region 计算5#连铸机水流量、4#机末端电磁搅拌数据
         List<casterInfo> listCasterInfo = new List<casterInfo>();
         casterInfo casterInfo5 = new casterInfo();
         casterInfo casterInfo4 = new casterInfo();
         public void setCasterinfo()
         {
             casterInfo4.code = "S64";
             listCasterInfo.Add(casterInfo4);
             casterInfo5.code = "S65";
             listCasterInfo.Add(casterInfo5);
         }


         /// <summary>
         /// 计算连铸机水流量、末端电磁搅拌数据
         /// </summary>
         public void getCasterInfo()
         {
             //查询连铸机生产状态
             string sql = "select code,heatid,status from cccm_unit_mag where code='S65' or code='S64'";
             oraDbHelp service = new oraDbHelp();
             DataSet ds = service.Query(sql);           
             if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
             {
                 foreach (DataRow item in ds.Tables[0].Rows)
                 {
                     if (item["code"].ToString() == "S65")
                     {
                         if (item["status"].ToString() == "2")
                         {
                             //计算水流量
                             calWater(item["heatid"].ToString());
                         }
                         casterInfo5.heatid = item["heatid"].ToString();
                         casterInfo5.status = item["status"].ToString();
                     }
                     else
                     {
                         if (item["status"].ToString() == "2")
                         {
                             //计算末端电磁搅拌
                             calMddj(item["heatid"].ToString());
                         }
                         casterInfo4.heatid = item["heatid"].ToString();
                         casterInfo4.status = item["status"].ToString();
                     }
                 }
             }
             DateTime dt = DateTime.Now;
             label22.Text = dt.ToString("yyyy-MM-dd HH:mm:ss");

         }
        
         /// <summary>
         /// 4#机末端电磁搅拌
         /// </summary>
         /// <param name="heatid"></param>
         public void calMddj(string heatid)
         {
             //1-4流实际电流大于1，则插入
             if (getVal(mdwz + 12) > 1 || getVal(mdwz + 13) > 1 || getVal(mdwz + 14) > 0 || getVal(mdwz + 15) > 0)
             {
                 string sql = "INSERT INTO CCCM_MDDJ_DATA(HEATID,S1P,S2P,S3P,S4P,L1P,L2P,L3P,L4P,S1D,S2D,S3D,S4D,L1D,L2D,L3D,L4D) VALUES('" + heatid + "',";
                 sql += getVal(mdwz) + "," + getVal(mdwz + 1) + "," + getVal(mdwz + 2) + "," + getVal(mdwz + 3) + "," + getVal(mdwz + 4) + "," + getVal(mdwz + 5) + ",";
                 sql += getVal(mdwz + 6) + "," + getVal(mdwz + 7) + "," + getVal(mdwz + 8) + "," + getVal(mdwz + 9) + "," + getVal(mdwz + 10) + "," + getVal(mdwz + 11) + ",";
                 sql += getVal(mdwz + 12) + "," + getVal(mdwz + 13) + "," + getVal(mdwz + 14) + "," + getVal(mdwz + 15) + ")";
                 oraDbHelp service = new oraDbHelp();
                 service.Update(sql);
             }
         }
         public void getMddjData()
         {
             textBox5.Text = getVal(mdwz + 8).ToString();
             textBox6.Text = getVal(mdwz + 9).ToString();
             textBox7.Text = getVal(mdwz + 10).ToString();
             textBox8.Text = getVal(mdwz + 11).ToString();
             textBox9.Text = getVal(mdwz + 12).ToString();
             textBox10.Text = getVal(mdwz + 13).ToString();
             textBox11.Text = getVal(mdwz + 14).ToString();
             textBox12.Text = getVal(mdwz + 15).ToString();

             textBox13.Text = getVal(mdwz).ToString();
             textBox14.Text = getVal(mdwz + 1).ToString();
             textBox15.Text = getVal(mdwz + 2).ToString();
             textBox16.Text = getVal(mdwz + 3).ToString();
             textBox17.Text = getVal(mdwz + 4).ToString();
             textBox18.Text = getVal(mdwz + 5).ToString();
             textBox19.Text = getVal(mdwz + 6).ToString();
             textBox20.Text = getVal(mdwz + 7).ToString();
         }


        /// <summary>
        /// 5#机水流量
        /// </summary>
        /// <param name="heatid"></param>
        public void calWater(string heatid)
         {
            
             double s1 = Math.Round(getVal(sllwz) + getVal(sllwz + 1) + getVal(sllwz + 2) + getVal(sllwz + 3) + getVal(sllwz + 4) + getVal(sllwz + 5) + getVal(sllwz + 6) + getVal(sllwz + 7), 1);
             double s2 = Math.Round(getVal(sllwz + 8) + getVal(sllwz + 9) + getVal(sllwz + 10) + getVal(sllwz + 11) + getVal(sllwz + 12) + getVal(sllwz + 13) + getVal(sllwz + 14) + getVal(sllwz + 15), 1);
             double s3 = Math.Round(getVal(sllwz + 16) + getVal(sllwz + 17) + getVal(sllwz + 18) + getVal(sllwz + 19) + getVal(sllwz + 20) + getVal(sllwz + 21) + getVal(sllwz + 22) + getVal(sllwz + 23), 1);
             double s4 = Math.Round(getVal(sllwz + 24) + getVal(sllwz + 25) + getVal(sllwz + 26) + getVal(sllwz + 27) + getVal(sllwz + 28) + getVal(sllwz + 29) + getVal(sllwz + 30) + getVal(sllwz + 31), 1);
             //插入数据
             string sql = "INSERT INTO CCCM_MBER_DATA(heatid,CS1,cs2,CS3,cs4) VALUES('" + heatid + "'," + s1 + "," + s2 + "," + s3 + "," + s4 + ")";
             oraDbHelp service = new oraDbHelp();
             service.Update(sql);
         }
         public void getWaterData()
         {
             double s1 = Math.Round(getVal(sllwz) + getVal(sllwz + 1) + getVal(sllwz + 2) + getVal(sllwz + 3) + getVal(sllwz + 4) + getVal(sllwz + 5) + getVal(sllwz + 6) + getVal(sllwz + 7), 1);
             double s2 = Math.Round(getVal(sllwz + 8) + getVal(sllwz + 9) + getVal(sllwz + 10) + getVal(sllwz + 11) + getVal(sllwz + 12) + getVal(sllwz + 13) + getVal(sllwz + 14) + getVal(sllwz + 15), 1);
             double s3 = Math.Round(getVal(sllwz + 16) + getVal(sllwz + 17) + getVal(sllwz + 18) + getVal(sllwz + 19) + getVal(sllwz + 20) + getVal(sllwz + 21) + getVal(sllwz + 22) + getVal(sllwz + 23), 1);
             double s4 = Math.Round(getVal(sllwz + 24) + getVal(sllwz + 25) + getVal(sllwz + 26) + getVal(sllwz + 27) + getVal(sllwz + 28) + getVal(sllwz + 29) + getVal(sllwz + 30) + getVal(sllwz + 31), 1);
             textBox1.Text = s1.ToString();
             textBox2.Text = s2.ToString();
             textBox3.Text = s3.ToString();
             textBox4.Text = s4.ToString();
             dataGridView3.DataSource = null;
             dataGridView3.DataSource = listCasterInfo;
         }
        #endregion

        #region 设置煤气回收参数
        /// <summary>
        /// 设置煤气回收参数
        /// </summary>
        public void setupMqhs()
        {
            listMqhs.Add(bof1);
            //listMqhs.Add(bof2);
            listMqhs.Add(bof3);
            listMqhs.Add(bof4);
            bof1.bofid = "S22";
            bof1.tqcount = 0;
            bof1.tqsk = 0;
            bof1.tqflag = 0;
            bof1.tapid = mqhswz; //出钢信号地址
            bof1.blowflagid = mqhswz + 1; //吹炼信号
            bof1.blowtimeid = mqhswz + 2;     //吹氧时间
            bof1.o_hangliangid = mqhswz + 3;  //煤气氧含量      
            bof1.fjid = mqhswz + 4;           //风机回收
            bof1.co5id = mqhswz + 15;
            bof1.co8id = mqhswz + 16;

            //bof2.bofid = "S21";
            //bof2.tqcount = 0;
            //bof2.tqsk = 0;
            //bof2.tqflag = 0;
            //bof2.tapid = mqhswz + 5;
            //bof2.blowtimeid = mqhswz+6;
            //bof2.o_hangliangid = mqhswz + 7;
            //bof2.blowflagid = mqhswz + 8;
            //bof2.fjid = mqhswz + 9;
            //bof2.co5id = mqhswz + 20;
            //bof2.co8id = mqhswz + 21;

            bof3.bofid = "S23";
            bof3.tqcount = 0;
            bof3.tqsk = 0;
            bof3.tqflag = 0;
            bof3.tapid = mqhswz + 5; //出钢信号地址
            bof3.blowflagid = mqhswz + 6; //吹炼信号
            bof3.blowtimeid = mqhswz + 7;     //吹氧时间
            bof3.o_hangliangid = mqhswz + 8;  //煤气氧含量      
            bof3.fjid = mqhswz + 9;           //风机回收
            bof3.co5id = mqhswz + 15;
            bof3.co8id = mqhswz + 16;

            bof4.bofid = "S24";
            bof4.tqcount = 0;
            bof4.tqsk = 0;
            bof4.tqflag = 0;
            bof4.tapid = mqhswz + 10; //出钢信号地址
            bof4.blowflagid = mqhswz + 11; //吹炼信号
            bof4.blowtimeid = mqhswz + 12;     //吹氧时间
            bof4.o_hangliangid = mqhswz + 13;  //煤气氧含量      
            bof4.fjid = mqhswz + 14;           //风机回收
            bof4.co5id = mqhswz + 15;
            bof4.co8id = mqhswz + 16;
        }


        #endregion

        public void getCaster5LadleInfo()
        {

            if (getVal(ladleweightwz) > 0.1)
            {
                textBox33.Text = "浇注中";
            }
            else
            {
                textBox33.Text = "已停浇";
            }


            textBox28.Text = getVal(ladleweightwz + 5).ToString();
            textBox29.Text = getVal(ladleweightwz + 4).ToString();
            textBox30.Text = getVal(ladleweightwz + 2).ToString();
            textBox31.Text = getVal(ladleweightwz + 3).ToString();

            textBox23.Text = getVal(ladleweightwz + 6).ToString();
            textBox24.Text = getVal(ladleweightwz + 7).ToString();
            textBox25.Text = getVal(ladleweightwz + 8).ToString();
            textBox26.Text = getVal(ladleweightwz + 9).ToString();

            textBox27.Text = ccm5dabaoshenggang.GetInstance().status;
            textBox32.Text = ccm5dabaoshenggang.GetInstance().tundishweight.ToString();
            textBox34.Text = ccm5dabaoshenggang.GetInstance().genzongzhi1.ToString();
            textBox35.Text = ccm5dabaoshenggang.GetInstance().genzongzhi2.ToString();
            textBox36.Text = ccm5dabaoshenggang.GetInstance().genzongzhi3.ToString();
            textBox37.Text = ccm5dabaoshenggang.GetInstance().genzongzhi4.ToString();
            textBox38.Text = ccm5dabaoshenggang.GetInstance().dabaobi;


        }
       

         private void timer_mqhs_Tick(object sender, EventArgs e)
         {
            try
            {
                bof1.calHssk();
                //bof2.calHssk();
                bof3.calHssk();
                bof4.calHssk();
            }
            catch
            {

            }
                    
         }
         public void getMqhsData()
         {
             dataGridView1.DataSource = null;
             dataGridView1.DataSource = listMqhs;
         }

         private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
         {
             TabPage tabpag = tabControl1.SelectedTab;
             switch (tabpag.Name)
             {
                 case "tabPage1":
                     getWaterData();
                     break;
                 case "tabPage2":
                     getMddjData();
                     break;
                 case "tabPage3":
                     getMqhsData();
                     break;
                 case "tabPage4":
                     getCasterWeightData();
                     break;
                //case "tabPage5":
                //    getWcfStatus();
                //    break;
                case "tabPage7":
                    getCaster5LadleInfo();
                    break;
                case "tabPage8":
                    //getCaster3CutInfo();
                    break;
                default: break;
             }
         }

        public void getWcfStatus()
        {
            #region 各个服务启动
            try
            {
                if (!PlcSvr.GetInstance().opc_connected)
                {
                    label23.Text = "net服务连接启动！";
                    label23.ForeColor = Color.Red;
                }
                else
                {
                    label23.Text = "net服务连接已启动！";
                }
                if (!WinccBof.GetInstance().opc_connected)
                {
                    label24.Text = "135服务连接没有启动！";
                    label24.ForeColor = Color.Red;
                }
                else
                {
                    label24.Text = "135服务连接已启动！";
                }
                if (!WinccBof_B.GetInstance().opc_connected)
                {
                    label25.Text = "48135服务连接没有启动！";
                    label25.ForeColor = Color.Red;
                }
                else
                {
                    label25.Text = "48135服务连接已启动！";
                }

                if (!WinccCcm.GetInstance().opc_connected)
                {
                    label26.Text = "125服务连接没有启动！";
                    label26.ForeColor = Color.Red;
                }
                else
                {
                    label26.Text = "125服务连接已启动！";
                }
                if (!WinccCcm_B.GetInstance().opc_connected)
                {
                    label27.Text = "48125服务连接没有启动！";
                    label27.ForeColor = Color.Red;
                }
                else
                {
                    label27.Text = "48125服务连接已启动！";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


            #endregion
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1_SelectedIndexChanged(null, null);
        }

         private void Form1_FormClosing(object sender, FormClosingEventArgs e)
         {
            if (MessageBox.Show("不要关闭,服务正在运行中,请点“取消”", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
            //e.Cancel = true; // 取消关闭窗体
            //this.Hide();
            //this.ShowInTaskbar = false;//取消窗体在任务栏的显示
            //this.notifyIcon1.Visible = true;//显示托盘图标
        }

         private void button2_Click(object sender, EventArgs e)
         {
             bof1.tqcount++;
         }

         private void checkBox1_CheckedChanged(object sender, EventArgs e)
         {
             if (checkBox1.Checked)
             {
                 timer_ladelweight.Enabled = true;
             }
             else
             {
                 timer_ladelweight.Enabled = false;
             }
         }

         private void timer_ladelweight_Tick(object sender, EventArgs e)
         {
             loadLadleWeight();
         }
         List<casterWeight> listCasterWeight = new List<casterWeight>();
         casterWeight casterweight3 = new casterWeight();
         casterWeight casterweight4 = new casterWeight();
      
         public void setCasterWeightInfo()
         {
             casterweight3.code = "S63";
             casterweight3.L1ValId = dbzlwz;
             casterweight4.code = "S64";
             casterweight4.L1ValId = dbzlwz + 1;
         }
         public void loadLadleWeight()
         {
             string sql = "select code,round(steelweight,2) as weight from cccm_unit_mag where code='S63' or code='S64'";
             oraDbHelp service = new oraDbHelp();
             DataSet ds = service.Query(sql);
             List<casterInfo> listCasterInfo = new List<casterInfo>();
             if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
             {
                 foreach (DataRow item in ds.Tables[0].Rows)
                 {                 
                     if (item["code"].ToString() == "S63")
                     {
                         casterweight3.curweight = Convert.ToDouble(item["weight"]);
                         casterweight3.downLoadWeight();
                         casterweight3.ts = DateTime.Now.ToString("HH:mm:ss");
                     }
                     else
                     {
                         casterweight4.curweight = Convert.ToDouble(item["weight"]);
                         casterweight4.downLoadWeight();
                         casterweight4.ts = DateTime.Now.ToString("HH:mm:ss");
                     }
                 }
             }
         }
         public void getCasterWeightData()
         {
             loadLadleWeight();
             dataGridView2.DataSource = null;
             casterweight3.L1weight = getVal(casterweight3.L1ValId).ToString();
             casterweight4.L1weight = getVal(casterweight4.L1ValId).ToString();
             listCasterWeight.Clear();
             listCasterWeight.Add(casterweight3);
             listCasterWeight.Add(casterweight4);
             dataGridView2.DataSource = listCasterWeight;
           
         }


        #region 大包剩钢、托盘等
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Show();
                this.ShowInTaskbar = true;
                this.notifyIcon1.Visible = false;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string s = KepServer.GetInstance().getVal(124);
            MessageBox.Show(s);
        }

        private void timer_dabaoshenggang_Tick(object sender, EventArgs e)
        {
            try
            {
                dabaoshenggang5.calData();
            }
            catch
            {

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (getVal(ladleweightwz) > 0.1)
            {
                ccm5dabaoshenggang.GetInstance().calData(1);
            }
        }
        #endregion




        #region 不用的 铸机切割优化等

        public void getCaster3CutInfo()
        {
            //ccm3cut.GetInstance();
            //ccm3cut.GetInstance().updateData();
            //textBox39.Text = ccm3cut.GetInstance().arrive.ToString();
            //textBox40.Text = ccm3cut.GetInstance().start.ToString();
            //textBox41.Text = ccm3cut.GetInstance().stop.ToString();
            //textBox42.Text = ccm3cut.GetInstance().ladlefeng.ToString();
            //List<ccmCutStrand> listcutstrand = new List<ccmCutStrand>();
            //listcutstrand.Add(ccm3cut.GetInstance().ccmCutStrand_1);
            //listcutstrand.Add(ccm3cut.GetInstance().ccmCutStrand_2);
            //listcutstrand.Add(ccm3cut.GetInstance().ccmCutStrand_3);
            //listcutstrand.Add(ccm3cut.GetInstance().ccmCutStrand_4);
            //dataGridView4.DataSource = null;
            //dataGridView4.DataSource = listcutstrand;

        }


        private void button4_Click(object sender, EventArgs e)
        {
            //ccm3cut.GetInstance();
            //ccm3cut.GetInstance().ccmCutStrand_1.acceptCutStatus(4);
            //ccm3cut.GetInstance().getSpeedAndTrack();
            //ccm3cut.GetInstance().ccmCutStrand_2.acceptStrandStatus(1);
            //ccm3cut.GetInstance().acceptCasterStatus(2);
            //ccm3cut.GetInstance().acceptLadlefeng(3);
        }

        private void timer_cut_Tick(object sender, EventArgs e)
        {
            //ccm3cut.GetInstance().getSpeedAndTrack();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //ucCaster1.setupConfig();
            //ucCaster1.strand1.zhupiStart = ccm3cut.GetInstance().ccmCutStrand_1.zhupiStart;
            //ucCaster1.strand1.zhupiEnd = ccm3cut.GetInstance().ccmCutStrand_1.zhupiEnd;
            //ucCaster1.strand1.starttrack = ccm3cut.GetInstance().ccmCutStrand_1.pitouTrack;
            //ucCaster1.strand1.endtrack= ccm3cut.GetInstance().ccmCutStrand_1.track;
            //ucCaster1.strand1.listfeng= ccm3cut.GetInstance().ccmCutStrand_1.listLadleFeng;

            //ucCaster1.strand2.zhupiStart = ccm3cut.GetInstance().ccmCutStrand_2.zhupiStart;
            //ucCaster1.strand2.zhupiEnd = ccm3cut.GetInstance().ccmCutStrand_2.zhupiEnd;
            //ucCaster1.strand2.starttrack = ccm3cut.GetInstance().ccmCutStrand_2.pitouTrack;
            //ucCaster1.strand2.endtrack = ccm3cut.GetInstance().ccmCutStrand_2.track;
            //ucCaster1.strand2.listfeng = ccm3cut.GetInstance().ccmCutStrand_2.listLadleFeng;

            //ucCaster1.strand3.zhupiStart = ccm3cut.GetInstance().ccmCutStrand_3.zhupiStart;
            //ucCaster1.strand3.zhupiEnd = ccm3cut.GetInstance().ccmCutStrand_3.zhupiEnd;
            //ucCaster1.strand3.starttrack = ccm3cut.GetInstance().ccmCutStrand_3.pitouTrack;
            //ucCaster1.strand3.endtrack = ccm3cut.GetInstance().ccmCutStrand_3.track;
            //ucCaster1.strand3.listfeng = ccm3cut.GetInstance().ccmCutStrand_3.listLadleFeng;

            //ucCaster1.strand4.zhupiStart = ccm3cut.GetInstance().ccmCutStrand_4.zhupiStart;
            //ucCaster1.strand4.zhupiEnd = ccm3cut.GetInstance().ccmCutStrand_4.zhupiEnd;
            //ucCaster1.strand4.starttrack = ccm3cut.GetInstance().ccmCutStrand_4.pitouTrack;
            //ucCaster1.strand4.endtrack = ccm3cut.GetInstance().ccmCutStrand_4.track;
            //ucCaster1.strand4.listfeng = ccm3cut.GetInstance().ccmCutStrand_4.listLadleFeng;
            //ucCaster1.Redraw();
        }
        #endregion



     
    }
}
