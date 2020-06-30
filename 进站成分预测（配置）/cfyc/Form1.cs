using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Rcw.Data;

namespace cfyc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DbContext.AddDataSource("cfyc", DbContext.DbType.Oracle, "192.168.36.153", "xgmes", "xgmes", "xgmes");
        }

        List<L_Alloy_Config> dataList = new List<L_Alloy_Config>();
        private void button1_Click(object sender, EventArgs e)
        {
             dataList = L_Alloy_Config.GetList("1=1 order by code_des ");
            gridControl1.DataSource = dataList;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataList.Update();
            MessageBox.Show("操作成功！");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        List<L_Alloy_Rate> rateList = new List<L_Alloy_Rate>();
        private void button3_Click(object sender, EventArgs e)
        {
            rateList = L_Alloy_Rate.GetList("1=1 order by steelgrade");
            gridControl2.DataSource = rateList;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            rateList.Update();
            MessageBox.Show("操作成功！");
        }
    }
}
