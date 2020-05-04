using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OPCAutomation;

namespace opc
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        meiqihuishou bof1 = new meiqihuishou();
        meiqihuishou bof2 = new meiqihuishou();
        meiqihuishou bof3 = new meiqihuishou();
        meiqihuishou bof4 = new meiqihuishou();
        List<meiqihuishou> listMqhs = new List<meiqihuishou>();
      
         private void Form1_Load(object sender, EventArgs e)
         {           
             setupMqhs();
             setCasterWeightInfo();
             setCasterinfo();
             DateTime dt = DateTime.Now;
             label22.Text = dt.ToString("yyyy-MM-dd HH:mm:ss");
             
         }
      
         private void button3_Click(object sender, EventArgs e)
         {
             //OPCItem bItem = KepGroup.OPCItems.GetOPCItem(listTag[2].itmHandleServer);
             //bItem.Write(700);
         }

        

        
         /// <summary>
         /// 根据变量ID获取变量的值
         /// </summary>
         /// <param name="id"></param>
         /// <returns></returns>
         public double getVal(int id)
         {
             return Math.Round(OPCHelp.GetInstance().getVal(id),2);
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
             Service service = new Service();
             DataSet ds = service.Query(sql);
            
             if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
             {
                 foreach (DataRow item in ds.Tables[0].Rows)
                 {
                     if (item["code"].ToString() == "S65")
                     {
                         if (item["status"].ToString() == "2")
                         {
                             calWater(item["heatid"].ToString());
                         }
                         casterInfo5.heatid = item["heatid"].ToString();
                         casterInfo5.status = item["status"].ToString();
                     }
                     else
                     {
                         if (item["status"].ToString() == "2")
                         {
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
             if (getVal(49) > 1 || getVal(50) > 1 || getVal(51) > 0 || getVal(52) > 0)
             {
                 string sql = "INSERT INTO CCCM_MDDJ_DATA(HEATID,S1P,S2P,S3P,S4P,L1P,L2P,L3P,L4P,S1D,S2D,S3D,S4D,L1D,L2D,L3D,L4D) VALUES('" + heatid + "',";
                 sql += getVal(37) + "," + getVal(38) + "," + getVal(39) + "," + getVal(40) + "," + getVal(41) + "," + getVal(42) + "," + getVal(43) + "," + getVal(44) + "," + getVal(45) + "," + getVal(46) + "," + getVal(47) + "," + getVal(48) + "," + getVal(49) + "," + getVal(50) + "," + getVal(51) + "," + getVal(52) + ")";
                 Service service = new Service();
                 service.Update(sql);
             }
         }
         public void getMddjData()
         {
             textBox5.Text = getVal(45).ToString();
             textBox6.Text = getVal(46).ToString();
             textBox7.Text = getVal(47).ToString();
             textBox8.Text = getVal(48).ToString();
             textBox9.Text = getVal(49).ToString();
             textBox10.Text = getVal(50).ToString();
             textBox11.Text = getVal(51).ToString();
             textBox12.Text = getVal(52).ToString();

             textBox13.Text = getVal(37).ToString();
             textBox14.Text = getVal(38).ToString();
             textBox15.Text = getVal(39).ToString();
             textBox16.Text = getVal(40).ToString();
             textBox17.Text = getVal(41).ToString();
             textBox18.Text = getVal(42).ToString();
             textBox19.Text = getVal(43).ToString();
             textBox20.Text = getVal(44).ToString();
         }
         /// <summary>
         /// 5#机水流量
         /// </summary>
         /// <param name="heatid"></param>
         public void calWater(string heatid)
         {
             double s1 = Math.Round(getVal(5) + getVal(6) + getVal(7) + getVal(8) + getVal(9) + getVal(10) + getVal(11) + getVal(12),1);
             double s2 = Math.Round(getVal(13) + getVal(14) + getVal(15) + getVal(16) + getVal(17) + getVal(18) + getVal(19) + getVal(20),1);
             double s3 = Math.Round(getVal(21) + getVal(22) + getVal(23) + getVal(24) + getVal(25) + getVal(26) + getVal(27) + getVal(28),1);
             double s4 = Math.Round(getVal(29) + getVal(30) + getVal(31) + getVal(32) + getVal(33) + getVal(34) + getVal(35) + getVal(36), 1);
             //插入数据
             string sql = "INSERT INTO CCCM_MBER_DATA(heatid,CS1,cs2,CS3,cs4) VALUES('" + heatid + "'," + s1 + "," + s2 + "," + s3 + "," + s4 + ")";
             Service service = new Service();
             service.Update(sql);

         }
         public void getWaterData()
         {
             double s1 = Math.Round(getVal(5) + getVal(6) + getVal(7) + getVal(8) + getVal(9) + getVal(10) + getVal(11) + getVal(12), 1);
             double s2 = Math.Round(getVal(13) + getVal(14) + getVal(15) + getVal(16) + getVal(17) + getVal(18) + getVal(19) + getVal(20), 3);
             double s3 = Math.Round(getVal(21) + getVal(22) + getVal(23) + getVal(24) + getVal(25) + getVal(26) + getVal(27) + getVal(28), 3);
             double s4 = Math.Round(getVal(29) + getVal(30) + getVal(31) + getVal(32) + getVal(33) + getVal(34) + getVal(35) + getVal(36), 3);
             textBox1.Text = s1.ToString();
             textBox2.Text = s2.ToString();
             textBox3.Text = s3.ToString();
             textBox4.Text = s4.ToString();
             dataGridView3.DataSource = null;
             dataGridView3.DataSource = listCasterInfo;
         }
         #endregion

        
         public void setupMqhs()
         {
             listMqhs.Add(bof1);
             listMqhs.Add(bof2);
             listMqhs.Add(bof3);
             listMqhs.Add(bof4);
             int i = 53;
             bof1.bofid = "S21";
             bof1.tqcount = 0;
             bof1.tqsk = 0;
             bof1.tqflag = 0;
             bof1.tapid = i;
             bof1.blowtimeid = i+1;
             bof1.o_hangliangid = i + 2;
             bof1.blowflagid = i + 3;
             bof1.fjid = i + 4;
             bof1.co5id = i + 20;
             bof1.co8id = i + 21;

             bof2.bofid = "S22";
             bof2.tqcount = 0;
             bof2.tqsk = 0;
             bof2.tqflag = 0;
             bof2.tapid = i + 5;
             bof2.blowtimeid = i+6;
             bof2.o_hangliangid = i + 7;
             bof2.blowflagid = i + 8;
             bof2.fjid = i + 9;
             bof2.co5id = i + 20;
             bof2.co8id = i + 21;

             bof3.bofid = "S23";
             bof3.tqcount = 0;
             bof3.tqsk = 0;
             bof3.tqflag = 0;
             bof3.tapid = i + 10;
             bof3.blowtimeid = i + 11;
             bof3.o_hangliangid = i + 12;
             bof3.blowflagid = i + 13;
             bof3.fjid = i + 14;
             bof3.co5id = i + 20;
             bof3.co8id = i + 21;

             bof4.bofid = "S24";
             bof4.tqcount = 0;
             bof4.tqsk = 0;
             bof4.tqflag = 0;
             bof4.tapid = i + 15;
             bof4.blowtimeid = i + 16;
             bof4.o_hangliangid = i + 17;
             bof4.blowflagid = i + 18;
             bof4.fjid = i + 19;
             bof4.co5id = i + 20;
             bof4.co8id = i + 21;
         }

         private void timer_mqhs_Tick(object sender, EventArgs e)
         {
             bof1.calHssk();
             bof2.calHssk();
             bof3.calHssk();
             bof4.calHssk();        
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
                 default: break;
             }
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
             int i = 75;
             casterweight3.code = "S63";
             casterweight3.L1ValId = i;
             casterweight4.code = "S64";
             casterweight4.L1ValId = i+1;
         }
         public void loadLadleWeight()
         {
             string sql = "select code,round(steelweight,2) as weight from cccm_unit_mag where code='S63' or code='S64'";
             Service service = new Service();
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
             dataGridView2.DataSource = null;
             casterweight3.L1weight = getVal(casterweight3.L1ValId).ToString();
             casterweight4.L1weight = getVal(casterweight4.L1ValId).ToString();
             listCasterWeight.Clear();
             listCasterWeight.Add(casterweight3);
             listCasterWeight.Add(casterweight4);
             dataGridView2.DataSource = listCasterWeight;
           
         }
        

    }
}
