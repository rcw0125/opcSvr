using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RestartApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button2_Click(null, null);
            label4.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {   if (MessageBox.Show("确认切换架构吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //取消自动模式
                checkBox1.Checked = false;
                timer_jiancha.Enabled = false;
                button1.Enabled = false;
                try
                {
                    AutoCloseClient();
                    AutoChangeAppServer();
                    button2_Click(null, null);
                }
                catch
                {
                    MessageBox.Show("出现异常，请手动重启！");

                }

                button1.Enabled = true;
                timer_jiancha.Enabled = true;
                sbCount = 0;
            }
            
                  
        }

        /// <summary>
        /// 关闭客户端
        /// </summary>
        public void AutoCloseClient()
        {
            Service service = new Service();
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            service.Update("insert into A_RESET_LOG( c_ip, c_type) values('192.168.36.157', '关闭开始') ");
            //insert into A_RESET_LOG(c_ts, c_ip, c_type) values('2019-02-20 14:00:43', '192.168.36.157', '关闭开始')
            service.Update("update A_RESET set value=value+1 ");
        }
        /// <summary>
        /// 切换架构
        /// </summary>
        public void AutoChangeAppServer()
        {
                     
            Process[] ps = Process.GetProcessesByName("AppServer");
            if (ps.Length != 2)
            {
                MessageBox.Show("服务的个数不对，请手动切换重启！");
                button1.Enabled = true;
                return;
            }
            else
            { 
                //5秒后杀掉进程
                Thread.Sleep(5000);
                if (ps[0].WorkingSet64 > ps[1].WorkingSet64)
                {
                    ps[0].Kill();
                    logQiehuan("手动切换成功");
                    MessageBox.Show("执行成功！");
                }
                else
                {
                    ps[1].Kill();
                    logQiehuan("手动切换成功");
                    MessageBox.Show("执行成功！");
                }

            }
        }

        /// <summary>
        /// 切换架构(锁表)
        /// </summary>
        public void AutoChangeAppServer_sb()
        {

            Process[] ps = Process.GetProcessesByName("AppServer");
            if (ps.Length != 2)
            {
                //MessageBox.Show("服务的个数不对，请手动切换重启！");
                //button1.Enabled = true;
                return;
            }
            else
            {
                
                if (ps[0].WorkingSet64 > ps[1].WorkingSet64)
                {
                    ps[0].Kill();
                    logQiehuan("报错切换成功");
                   // MessageBox.Show("执行成功！");
                }
                else
                {
                    ps[1].Kill();
                    logQiehuan("报错切换成功");
                   // MessageBox.Show("执行成功！");
                }

            }
        }


        public void logQiehuan(string type= "自动切换成功")
        {
            Service service = new Service();
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            service.Update("insert into A_RESET_LOG(c_ip, c_type,c_ts) values('192.168.36.157', '"+type+"','"+time+"') ");
            //insert into A_RESET_LOG(c_ts, c_ip, c_type) values('2019-02-20 14:00:43', '192.168.36.157', '关闭开始')
            //service.Update("update A_RESET set value=value+1 ");
        }

        /// <summary>
        /// 查询关闭情况
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {

            tabControl1_SelectedIndexChanged(null, null);


        }
        /// <summary>
        /// 15秒钟一次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                //if (CanAutoChange())
                //{
                //    AutoChangeAppServer();
                //}

                if (auto)
                {
                    autojisuan();
                }
            }
            catch
            {
                
            }
           
        }
        int appVal = 0;
        /// <summary>
        /// 是否可以自动切换
        /// </summary>
        /// <returns></returns>
        public bool CanAutoChange()
        {
            Service service = new Service();
            var ds = service.Query(" select val,ts from a_autoChangeApp");
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0 && ds != null)
            {
                if (ds.Tables[0].Rows[0]["val"].ToString() != appVal.ToString())
                {
                    if (appVal == 0)
                    {
                        appVal = Convert.ToInt32(ds.Tables[0].Rows[0]["val"].ToString());
                        return false;
                    }
                    else
                    {
                        appVal = Convert.ToInt32(ds.Tables[0].Rows[0]["val"].ToString());

                        return true;
                    }
                }
            }
            return false;
        }
        bool auto = false;
        /// <summary>
        /// 自动 按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                auto = true;
                calAppId();
                
            }
            else
            {
                auto = false;
                mainApp = 0;
                fuApp = 0;
                textBox1.Text = "";
                textBox2.Text = "";
            }
        }

        public Int64 mainApp = 0;
        public Int64 fuApp = 0;

        /// <summary>
        /// 计算是否有两个AppServer，是否可以自动
        /// </summary>
        public void calAppId()
        {
            Process[] ps = Process.GetProcessesByName("AppServer");
            if (ps.Length != 2)
            {
                checkBox1.Checked = false;
                MessageBox.Show("服务的个数不对，无法托管自动");              
                return;
            }
            else
            {             
                if (ps[0].WorkingSet64 > ps[1].WorkingSet64)
                {
                    mainApp= ps[0].Id;
                    fuApp = ps[1].Id;
                }
                else
                {
                    mainApp = ps[1].Id;
                    fuApp = ps[0].Id;
                }

            }
        }

        public void autojisuan()
        {
            Process[] ps = Process.GetProcessesByName("AppServer");
            if (ps.Length != 2)
            {
                // MessageBox.Show("服务的个数不对，无法自动");
                label4.Text = "服务的个数不对，无法自动";
                return;
            }
            else
            {
                label4.Text = "";
                // 两个服务都没有变
                textBox1.Text = mainApp.ToString();
                textBox2.Text = fuApp.ToString();
                if (ps[0].Id == mainApp && ps[1].Id == fuApp)
                {
                    return;
                }
                // 两个服务都没有变
                else if (ps[1].Id == mainApp && ps[0].Id == fuApp)
                {
                    return;
                }
                // 检测到存在原来的备用服务，说明服务已完成切换，
                // 这时的操作是：关闭客户端，
                //将另一个服务变成主pid，同时杀掉这个进程
                //记录成功切换
                else if (ps[0].Id == fuApp)
                {
                    AutoCloseClient();
                    //5秒后杀掉进程
                    Thread.Sleep(5000);
                    mainApp = ps[1].Id;
                    fuApp = 0;
                    ps[0].Kill();
                    logQiehuan();
                    button2_Click(null, null);
                    sbCount = 0;
                }
                // 检测到存在原来的备用服务，说明服务已完成切换，
                // 这时的操作是：关闭客户端，将另一个服务变成主pid，同时杀掉这个进程
                //记录成功切换
                else if (ps[1].Id == fuApp)
                {
                    AutoCloseClient();
                    //5秒后杀掉进程
                    Thread.Sleep(5000);
                    mainApp = ps[0].Id;
                    fuApp = 0;
                    ps[1].Kill();
                    logQiehuan();
                    button2_Click(null, null);
                    sbCount = 0;
                }
                // 自动切换后，另一个就是备用服务的pid           
                else if (ps[0].Id == mainApp)
                {
                    fuApp = ps[1].Id;
                }
                // 自动切换后，另一个就是备用服务的pid      
                else if (ps[1].Id == mainApp)
                {
                    fuApp = ps[0].Id;
                }
                else
                {
                    auto = false;
                    MessageBox.Show("已无法自动，请重新操作");
                }

            }

        }

        private void timer_jiancha_Tick(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                checkBox1.Checked = true;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage tabpag = tabControl1.SelectedTab;
            switch (tabpag.Name)
            {
                case "tabPage1":
                    calmx();
                    break;
                case "tabPage2":
                    calzs();
                    break;
                case "tabPage3":
                    //calsb();
                    break;
                case "tabPage4":
                    //querysb();
                    break;
                default: break;
            }
        }

        private void tabControl1_VisibleChanged(object sender, EventArgs e)
        {
           
        }
        /// <summary>
        /// 查询关闭明细
        /// </summary>
        public void calmx()
        {
            try
            {
                Service service = new Service();
                //select a.c_ts as 时刻,a.c_ip as IP地址,a.c_type as 操作,b.C_NAME 备注,a.c_name as 计算机名  from A_RESET_LOG a
                // string str = "select c_ts as 时刻,c_ip as IP地址,c_type as 操作  from A_RESET_LOG ";
                // str += " where c_ts> to_char(sysdate-0.003,'yyyy-MM-dd hh24:mi:ss') order by c_ts desc";

                //string str = "select a.c_ts as 时刻,a.c_ip as IP地址,a.c_type as 操作,b.C_NAME 备注,a.c_name as 计算机名  from A_RESET_LOG a ";
                //str += " left join A_COMPUTER b on a.c_ip=b.c_ip";
                //str += " where a.c_ts> to_char(sysdate-0.003,'yyyy-MM-dd hh24:mi:ss') order by a.c_ts desc";


                string str = "select * from log";
                var ds = service.Query(str);
                dataGridView1.DataSource = ds.Tables[0];
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    label1.Text = "最近一天关闭的客户端的个数为：" + ds.Tables[0].Rows.Count;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("出现异常" + ex.ToString());
            }
        }
        /// <summary>
        /// 查询操作次数
        /// </summary>
        public void calzs()
        {
            try
            {
                Service service = new Service();
                string str = "select c_ts, c_type from A_RESET_LOG where (c_type like '%切换成功') and c_ts > TO_CHAR(SYSDATE - 1, 'yyyy-MM-dd hh24:mi:ss') order by c_ts desc ";
                var ds = service.Query(str);
                dataGridView2.DataSource = ds.Tables[0];
                //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //{
                //    label1.Text = "最近一天关闭的客户端的个数为：" + ds.Tables[0].Rows.Count;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("出现异常" + ex.ToString());
            }
        }
        int sbCount = 0;
        List<dbsb> listdbsb = new List<dbsb>();
        /// <summary>
        /// 查看锁表情况
        /// </summary>
        public void calsb()
        {
            try
            {
                ServiceMain service = new ServiceMain();
                string str = "  SELECT L.ORACLE_USERNAME ,";
                str += "  DECODE(L.LOCKED_MODE, '0', 'none', '1', 'null 空',";
                str += "  '2', 'Row-S 行共享(RS)', '3', 'Row-X 行独占(RX)',";
                str += "  '4', 'Share 共享锁(S)', '5', 'S/Row-X 共享行独占(SRX)',";
                str += "   '6', 'exclusive 独占(X)', '未知') as sms,";
                str += "  O.OWNER , O.OBJECT_NAME ,  O.OBJECT_TYPE ,";
                str += "  S.MACHINE , S.CLIENT_INFO , S.PROGRAM , SA.SQL_TEXT ,";
                str += "  'alter system kill session ''' || S.SID || ',' || S.SERIAL# || ''';'  as jshh, ";
                str += "  'kill -9 ' || P.SPID as jsjc,'1' as c_ts ";
                str += "  FROM V$LOCKED_OBJECT L, DBA_OBJECTS O, V$SESSION S, V$PROCESS P, V$SQLAREA SA";      
                str += "  WHERE L.OBJECT_ID = O.OBJECT_ID  AND L.SESSION_ID = S.SID";
                str += "  AND S.PADDR = P.ADDR  AND S.SQL_ID = SA.SQL_ID";
                var ds = service.Query(str);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    listdbsb.Clear();
                    listdbsb = DT_To_List_dbsb(ds.Tables[0]);
                    //dataGridView3.DataSource = listdbsb;
                    //if (listdbsb.Count > 10)
                    //{
                    //    sbCount++;
                    //    save_suobiao();
                    //}
                    //else
                    //{
                    //    sbCount = 0;
                    //}
                    //label5.Text = "查询时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                }
                else
                {
                    sbCount = 0;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("出现异常" + ex.ToString());
            }
        }

        public void save_suobiao()
        {
            if (listdbsb.Count > 10)
            {
                foreach (var item in listdbsb)
                {
                    Service service = new Service();
                    string sql = "insert into ts_db_suobiao(oracle_username,sms,owner,object_name,object_type,machine,client_info,program,sql_text,jshh,jsjc)";
                    sql += "values('"+item.oracle_username+"','"+item.sms+"','"+item.owner+"','"+item.object_name+"','"+item.object_type+"','"+item.machine+"','";
                    sql += item.client_info + "','" + item.program + "','" + item.sql_text + "','" + item.jshh + "','" + item.jsjc + "')";
                    service.Update(sql);
                }
            } 
        }

        DateTime qidongshijian = DateTime.Now;
        List<shijian> listsj = new List<shijian>();
        /// <summary>
        /// 30秒检查一次锁表情况
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_suobiao_Tick(object sender, EventArgs e)
        {
            try
            {
                //if (auto)
                //{
                //    calsb();
                //}
                //else
                //{
                //    sbCount = 0;
                //}

                if (auto)
                {
                    label5.Text = "报错时间:" + qidongshijian.ToString("yyyy-MM-dd HH:mm:ss");
                    EventLog eventlog = new EventLog();
                    eventlog.Log = "System";
                    EventLogEntryCollection eventLogEntryCollection = eventlog.Entries;
                    string info = string.Empty;
                    foreach (EventLogEntry entry in eventLogEntryCollection)
                    {
                        //事件来源是Application Popup，时间大于上次时间，消息内容包括AppServer.exe
                        if (@"Application Popup" == entry.Source.ToString() && entry.TimeGenerated > qidongshijian && entry.Message.Contains("AppServer.exe"))
                        {
                            qidongshijian = entry.TimeGenerated;
                            //杀掉当前进程
                            AutoChangeAppServer_sb();
                            shijian sj = new shijian();
                            sj.type = entry.EntryType.ToString();
                            sj.time = entry.TimeGenerated.ToString("yyyy-MM-dd HH:mm:ss");
                            sj.source = entry.Source.ToString();
                            sj.message = entry.Message.ToString();
                            listsj.Add(sj);
                            dataGridView3.DataSource = null;
                            dataGridView3.DataSource = listsj;
                        }

                    }
                }

               

            }
            catch
            {

            }
           
            //if (sbCount >= 2)
            //{
            //    try
            //    {

            //        AutoChangeAppServer_sb();
            //    }
            //    catch
            //    {

            //    }
            //}
        }

        public List<dbsb> DT_To_List_dbsb(DataTable dt)
        {
            List<dbsb> sbgzlist = new List<dbsb>();
            if (dt == null)
            {
                return sbgzlist;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dbsb sb = new dbsb();
                sb.oracle_username = dt.Rows[i]["oracle_username"].ToString().Trim();
                sb.sms = dt.Rows[i]["sms"].ToString().Trim();
                sb.owner = dt.Rows[i]["owner"].ToString().Trim();
                sb.object_name = dt.Rows[i]["object_name"].ToString().Trim();
                sb.object_type = dt.Rows[i]["object_type"].ToString().Trim();
                sb.machine = dt.Rows[i]["machine"].ToString().Trim();
                sb.client_info = dt.Rows[i]["client_info"].ToString().Trim();
                sb.program = dt.Rows[i]["program"].ToString().Trim();
                sb.sql_text = dt.Rows[i]["sql_text"].ToString().Trim();
                sb.jshh = dt.Rows[i]["jshh"].ToString().Trim();
                sb.jsjc = dt.Rows[i]["jsjc"].ToString().Trim();
                sb.c_ts = dt.Rows[i]["c_ts"].ToString().Trim();

                sbgzlist.Add(sb);
            }
            return sbgzlist;
        }


        public void querysb()
        {
            try
            {
                Service service = new Service();
                string str = "   select * from ts_db_suobiao where c_ts>'"+DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd")+"'";
               
                var ds = service.Query(str);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //dataGridView4.DataSource = DT_To_List_dbsb(ds.Tables[0]);
                }
              

            }
            catch (Exception ex)
            {
                MessageBox.Show("出现异常" + ex.ToString());
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("不要关闭,关闭会被考核,请点“取消”", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
            {
                e.Cancel = true;
            }       
        }
       

        private void button3_Click(object sender, EventArgs e)
        {

            //logQiehuan("测试切换成功！");

            //try
            //{
            //    string user1path = @"D:\XGServer1\L3Users.dat";
            //    FileInfo user1 = new FileInfo(user1path);
            //    var lasttime = user1.LastWriteTime;

            //    string user2path = @"D:\XGServer2\L3Users.dat";
            //    FileInfo user2 = new FileInfo(user2path);
            //    if (user1.LastWriteTime == user2.LastWriteTime)
            //    {
            //        return;
            //    }
            //    else if (user1.LastWriteTime > user2.LastWriteTime)
            //    {
            //        user1.CopyTo(user2path, true);
            //    }
            //    else if (user1.LastWriteTime < user2.LastWriteTime)
            //    {
            //        user2.CopyTo(user1path, true);
            //    }
            //}
            //catch
            //{

            //}
        }
    }
}
