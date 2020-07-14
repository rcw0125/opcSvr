using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Rcw.Data;

namespace SBGL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<lgsb_sbdjjl_table> datalist = null;

        List<lgsb_sbdjjl_table> deldatalist =new List<lgsb_sbdjjl_table> ();

        #region 设备点检去重项（在执行）
        private void button1_Click(object sender, EventArgs e)
        {
            // var data = DbContext.GetDataTable("select djdate,a.* from lgsb_sbdjjl_table a where djdate like '2019-02-24%' order by sblist_id,djdate ");
            string riqi = dateTimePicker1.Text.ToString() + "%";
            loadData(riqi);
        }

        //设备去重项，不计算岗位点检
        public void loadData(string riqi)
        {
            datalist = lgsb_sbdjjl_table.GetList("djdate like @djdate  order by djdate desc ", riqi);
            label1.Text = datalist.Count.ToString();
            dataGridView1.DataSource = datalist;
        }

        public void jisuan()
        {
            if (datalist.Count == 0)
            {
                return;
            }
            else
            {
                deldatalist.Clear();
                foreach (var item in datalist)
                {
                    if (item.DataState != DataRowState.Deleted)
                    {
                        foreach (var iz in datalist)
                        {
                            if (iz.DataState != DataRowState.Deleted)
                            {
                                //设备名称，状态，描述，点检人，点检路线
                                if (item.id != iz.id && item.sblist_id == iz.sblist_id && item.status == iz.status && item.comment == iz.comment && item.djr == iz.djr && item.sbdjlx_id == iz.sbdjlx_id)
                                {

                                    //岗位点检 如果两条记录在5条之内，则删除
                                    if (item.sbdjlb_id == 14)
                                    {
                                        if ((item.id - iz.id < 5) || (iz.id - item.id < 5))
                                        {
                                            iz.DataState = DataRowState.Deleted;
                                        }

                                    }
                                    else
                                    {
                                        iz.DataState = DataRowState.Deleted;
                                    }
                                }
                            }

                        }

                    }
                }
                foreach (var item in datalist)
                {
                    if (item.DataState == DataRowState.Deleted)
                    {
                        deldatalist.Add(item);
                    }

                }
                if (deldatalist != null)
                {
                    label1.Text = deldatalist.Count.ToString();
                    dataGridView1.DataSource = deldatalist;
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {

            jisuan();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            deldatalist.Update();
        }
        #endregion

        #region 自动生成周期计划（已废弃）
        public void sczqjh()
        {
            int sblist_id, sbjxdw_id, sbjxbzu_id, sbjxzq;
            string sblistname, sblistcode, jxnr;
            //select DISTINCT id,sblist_id,sblistname,sblistcode,sbjxdw_id,sbjxbzu_id,sbjxzq,jhdate,jxnr,syts,jxlb,STATUS,scdate   
            // from(select * from lgsb_sbjxjh_table   where sbjxzq > 0   order by jhdate desc) a
            // GROUP BY sblistcode, sbjxbzu_id
            //string sql = "select SUBSTRING_INDEX(group_concat(sblistcode,sbjxbzu_id order by jhdate asc),',',1) as  ";
            //sql += "  rowid,id,sblist_id,sblistname,sblistcode,sbjxdw_id,sbjxbzu_id,sbjxzq,jhdate,jxnr,syts,jxlb,STATUS,scdate  ";
            //sql += "  from lgsb_sbjxjh_table where sbjxzq>0 GROUP BY sblistcode,sbjxbzu_id";
            string sql = "select DISTINCT id,sblist_id,sblistname,sblistcode,sbjxdw_id,sbjxbzu_id,sbjxzq,jhdate,jxnr,syts,jxlb,STATUS,scdate   ";
            sql += "  from(select * from lgsb_sbjxjh_table   where sbjxzq > 0   order by jhdate desc) a   ";
            sql += "  GROUP BY sblistcode, sbjxbzu_id ";
            var data = DbContext.GetDataTable(sql);
            //dataGridView1.DataSource = data;
            if (data != null && data.Rows.Count > 0)
            {

                for (int i = 0; i < data.Rows.Count; i++)
                {
                    //如果该计划已经完成，则检查周期
                    if (Convert.ToInt32(data.Rows[i]["status"]) == 1)
                    {
                        //计划完成时间+周期 与当前时间对比
                        var scdate = Convert.ToDateTime(data.Rows[i]["scdate"].ToString()).AddDays(Convert.ToInt32(data.Rows[i]["sbjxzq"].ToString()));
                        //超过周期，则添加一条
                        if (DateTime.Now > scdate)
                        {
                            sblist_id = Convert.ToInt32(data.Rows[i]["sblist_id"].ToString());
                            sbjxdw_id = Convert.ToInt32(data.Rows[i]["sbjxdw_id"].ToString());
                            sbjxbzu_id = Convert.ToInt32(data.Rows[i]["sbjxbzu_id"].ToString());
                            sbjxzq = Convert.ToInt32(data.Rows[i]["sbjxzq"].ToString());
                            sblistname = data.Rows[i]["sblistname"].ToString();
                            sblistcode = data.Rows[i]["sblistcode"].ToString();
                            jxnr = data.Rows[i]["jxnr"].ToString();
                            saveJhjx(sblist_id, sblistname, sblistcode, sbjxdw_id, sbjxbzu_id, sbjxzq, jxnr, 0, 1, 0);
                        }
                    }
                }

            }
        }
        public void saveJhjx(int sblist_id, string sblistname, string sblistcode, int sbjxdw_id, int sbjxbzu_id, int sbjxzq, string jxnr, int syts, int jxlb, int status)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string sql = "INSERT into lgsb_sbjxjh_table(sblist_id, sblistname, sblistcode, sbjxdw_id,";
            sql += "sbjxbzu_id, sbjxzq,scdate, jhdate, jxnr, syts, jxlb, STATUS) VALUES( ";
            sql += sblist_id + ",'" + sblistname + "','" + sblistcode + "'," + sbjxdw_id + "," + sbjxbzu_id + "," + sbjxzq + ",'" + date + "','" + date + "','" + jxnr + "'," + syts + "," + jxlb + "," + status + ")";
            DbContext.ExeSql(sql);
            //INSERT into lgsb_sbjxjh_table(sblist_id, sblistname, sblistcode, sbjxdw_id,
            // sbjxbzu_id, sbjxzq, jhdate, jxnr, syts, jxlb, STATUS)
            //VALUES(715, '3#LF炉-高压室-电气-全部-全部-全部普通', '053-016-002-000-000-0P', 18, 454, 1, '2019-04-30', 'ceshi', 0, 0, 0)
        }
        #endregion
  
        string curday = "2019-06-02";
              
        public List<sbdjjl> DT_To_List(DataTable dt)
        {
            List<sbdjjl> sbdjlist = new List<sbdjjl>();
            if (dt == null)
            {
                return sbdjlist;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sbdjjl sbdj = new sbdjjl();
                sbdj.sblist_id = dt.Rows[i]["sblist_id"].ToString().Trim();
                sbdj.sblistname = dt.Rows[i]["sblistname"].ToString().Trim();
                sbdj.djlx= dt.Rows[i]["djlx"].ToString().Trim().Replace(" ", "");
                sbdj.djdw = dt.Rows[i]["djdw"].ToString().Trim().Replace(" ","");
                sbdj.djbz = dt.Rows[i]["djbz"].ToString().Trim().Replace(" ", "");
                sbdj.djzq = dt.Rows[i]["djzq"].ToString().Trim().Replace(" ", "");
                sbdj.djlb = dt.Rows[i]["djlb"].ToString().Trim().Replace(" ", "");
                sbdj.status = dt.Rows[i]["status"].ToString() == "1" ? "正常": "异常";
                sbdj.comment = dt.Rows[i]["comment"].ToString().Trim();
                sbdj.djr = dt.Rows[i]["djr"].ToString().Trim();
                //点检日期为当前查询日期
                sbdj.djrq = curday;
                sbdjlist.Add(sbdj);
            }
            return sbdjlist;
        }


        #region 设备月报判定 （已不用）
        /// <summary>
        /// 生成设备点检月报
        /// </summary>
        public void scyb()
        {
            cleardata();
            foreach (var item in sbdjlist)
            {
                jsdj_item(item);
            }
            //计算月点检次数
            jsycount();
        }
        /// <summary>
        /// 清空当日数据
        /// </summary>
        public void cleardata()
        {

            string sql = "";
            string day = curday.Substring(8, 2);
            sql = " update ts_sbdjyb set status" + day + "=null,comment" + day + "=null,djr" + day + "=null,count" + day + "='0'";
            sql += " where logtime = '" + curday.Substring(0, 7) + "'";
            DbContext.ExeSql("xgmesweb", sql);
        }
        public void jsdj_item(sbdjjl sbdj_item)
        {
            string day = curday.Substring(8, 2);

            if (existjl(sbdj_item))
            {
                //更新数据
                updateday(sbdj_item);
            }
            else
            {
                //插入数据                                    
                insertday(sbdj_item);
            }
        }

        public bool existjl(sbdjjl item)
        {
            //select count(0) from lgsb_sbdjtj_table where logtime='2019-06' and sblist_id='' 
            //and djlx = '' and djdw = '' and djbz = ''
            string sql = " select count(0) from ts_sbdjyb where logtime='" + item.djrq.Substring(0, 7) + "' and sbid='" + item.sblist_id + "'  ";
            sql += " and djlx = '" + item.djlx + "' and djdw = '" + item.djdw + "' and djbz = '" + item.djbz + "'";
            var data = DbContext.GetDataTable("xgmesweb", sql);
            if (data.Rows[0][0].ToString() == "0")
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        #region  插入数据
        /// <summary>
        /// 向对应字段（天数）插入数据
        /// </summary>
        /// <param name="item"></param>
        public void insertday(sbdjjl item)
        {
            string count = "2";
            if (item.djzq == "每班12小时")
            {
                count = "1";
            }
            string sql = "";
            string day = curday.Substring(8, 2);
            sql = " insert into ts_sbdjyb(logtime, sbid, sbname, djlx, djdw, djbz, djzq, djlb,  ";
            sql += "status" + day + ", comment" + day + ", djr" + day + ", count" + day + ")";
            sql += "  VALUES('" + item.djrq.Substring(0, 7) + "', '" + item.sblist_id + "', '" + item.sblistname + "', '" + item.djlx + "', '" + item.djdw + "', '";
            sql += item.djbz + "', '" + item.djzq + "', '" + item.djlb + "', '" + item.status + "', '" + item.comment + "', '" + item.djr + "', '" + count + "') ";
            DbContext.ExeSql("xgmesweb", sql);
        }
        #endregion
        #region 更新数据

        /// <summary>
        /// 更新指定字段（当天）的数据
        /// </summary>
        /// <param name="item"></param>
        public void updateday(sbdjjl item)
        {
            string count = "2";
            if (item.djzq == "每班12小时")
            {
                count = "1";
            }
            //查询已点检的次数
            string sql = "";
            string day = curday.Substring(8, 2);
            sql = " select count" + day + " from ts_sbdjyb where logtime='" + item.djrq.Substring(0, 7) + "' and sbid='" + item.sblist_id + "'  ";
            sql += " and djlx = '" + item.djlx + "' and djdw = '" + item.djdw + "' and djbz = '" + item.djbz + "'";
            var data = DbContext.GetDataTable("xgmesweb", sql);
            if (data != null && data.Rows.Count > 0)
            {
                count = (Convert.ToInt16(data.Rows[0][0].ToString()) + Convert.ToInt16(count)).ToString();
            }
            //更新点检情况
            sql = " update ts_sbdjyb set status" + day + "='" + item.status + "',comment" + day + "='";
            sql += item.comment + "',djr" + day + "='" + item.djr + "',count" + day + "='" + count + "' ";
            sql += " where logtime = '" + item.djrq.Substring(0, 7) + "' and sbid = '" + item.sblist_id + "' and djlx = '" + item.djlx + "' ";
            sql += " and djdw = '" + item.djdw + "' and djbz = '" + item.djbz + "'";
            DbContext.ExeSql("xgmesweb", sql);
        }
        #endregion

        public void jsycount()
        {
            string sql = "";
            string day = curday.Substring(8, 2);
            //计算每班12小时的次数
            sql = " update TS_SBDJYB set ycount= ( ";
            for (int i = 1; i <= 31; i++)
            {
                if (i < 10)
                {
                    sql += " case when count0" + i + "='1' then '0' when count0" + i + "='0' then '0' else '1' end ";

                }
                else
                {
                    sql += " case when count" + i + "='1' then '0' when count" + i + "='0' then '0' else '1' end ";

                }
                if (i != 31)
                {
                    sql += " + ";
                }
            }
            sql += " ) where djzq='每班12小时' and logtime='" + curday.Substring(0, 7) + "'";
            DbContext.ExeSql("xgmesweb", sql);

            sql = " update TS_SBDJYB set ycount= ( ";
            for (int i = 1; i <= 31; i++)
            {
                if (i < 10)
                {
                    sql += " case when count0" + i + "='1' then '0' when count0" + i + "='0' then '0' else '1' end ";

                }
                else
                {
                    sql += " case when count" + i + "='1' then '0' when count" + i + "='0' then '0' else '1' end ";

                }
                if (i != 31)
                {
                    sql += " + ";
                }
            }
            sql += " ) where djzq <> '每班12小时' and logtime='" + curday.Substring(0, 7) + "'";
            DbContext.ExeSql("xgmesweb", sql);

        }



        #endregion


        #region 点检明细导入到mes数据库(已废弃)

        List<sbdjjl> sbdjlist = new List<sbdjjl>();

        /// <summary>
        /// 清空当日明细数据,然后记录每条数据
        /// </summary>
        public void scitemdata(string day)
        {
            string sql = " select a.sblist_id,a.sblistname,a.status,a.comment,a.djr,b.name as djlx ,c.name as djdw,  ";
            sql += "              d.name as djbz,e.name as djlb,f.name as djzq  ";
            sql += "      from lgsb_sbdjjl_table a,lgsb_sbdjbz_sbdjlx_table b, lgsb_user_company_table c, ";
            sql += "              lgsb_user_part_table d, lgsb_sbdjbz_sbdjlb_table e,lgsb_sbdjbz_sbdjzq_table f  ";
            sql += "        where a.sbdjlx_id = b.id and a.sbdjdw_id = c.id and a.sbdjbzu_id = d.id  ";
            sql += "              and a.sbdjlb_id = e.id and a.sbdjzq_id = f.id  ";
            sql += "             and a.djdate like '" + day + "%' order by a.djdate  ";
            //sql += "  GROUP BY sblistcode, sbjxbzu_id ";
            var data = DbContext.GetDataTable(sql);
            sbdjlist.Clear();
            sbdjlist = DT_To_List(data);

            sql = " delete from TS_SBDJYB_item where logtime='" + day + "'";
            DbContext.ExeSql("xgmesweb", sql);

            foreach (var item in sbdjlist)
            {
                insertitemday(item);
            }
        }
        /// <summary>
        /// 记录点检明细数据
        /// </summary>
        /// <param name="item"></param>
        public void insertitemday(sbdjjl item)
        {

            string sql = "";
            string day = curday;
            sql = " insert into ts_sbdjyb_item(logtime, sbid, sbname, djlx, djdw, djbz, djzq, djlb,status,note, djr)  ";
            sql += "  VALUES('" + item.djrq + "', '" + item.sblist_id + "', '" + item.sblistname + "', '" + item.djlx + "', '" + item.djdw + "', '";
            sql += item.djbz + "', '" + item.djzq + "', '" + item.djlb + "', '" + item.status + "', '" + item.comment + "', '" + item.djr + "')";
            DbContext.ExeSql("xgmesweb", sql);
        }
        #endregion


        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                string riqi = DateTime.Now.ToString("yyyy-MM-dd") + "%";
                loadData(riqi);
                jisuan();
                deldatalist.Update();
            }
            catch
            {

            }

            //当前时间为5点多，自动计算生成周期计划
            if (DateTime.Now.Hour == 5)
            {
                curday = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                string preday = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");     
                //计划检修超期计算
                exc_jx_expire();         
                if (DateTime.Now.Day == 1)
                {
                    calYueSbgzbx(DateTime.Now.AddDays(-1).ToString("yyyy-MM"));                     
                    yuecaljhjx();          
                }            
                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                {                     
                    calSbgzbx();                
                    caljhjx();
                    caldjzb();
                    // 计算设备点检周报
                    //int zhou = (DateTime.Now.Day - 1) / 7 + 1;                
                    //string insql = "call xgmes.jisuan_sbzb('" + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + "','" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "'," + zhou + ") ";
                    //DbContext.ExeSql("xgmesweb", insql);
                }
            }         
        }

        #region 检修计划超期更改为超期状态
        /// <summary>
        /// 检修计划超期更改为超期状态
        /// </summary>
        public void exc_jx_expire()
        {
            //20191221修改，超期后不再增加新的，影响周计划数量
            //string strsql = "insert into lgsb_sbjxjh_table(sblist_id, sblistname, sblistcode, sbjxdw_id, sbjxbzu_id, sbjxzq, jxnr, status, jxlb)";
            //strsql += " select sblist_id, sblistname, sblistcode, sbjxdw_id, sbjxbzu_id, sbjxzq, jxnr, status, jxlb";
            //strsql += " from lgsb_sbjxjh_table where status = 0 and jhdate<'" + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + "' ";
            //DbContext.ExeSql(strsql);
            //strsql = "update lgsb_sbjxjh_table set jhdate='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where status=0 and jhdate='0000-00-00'";
            //DbContext.ExeSql(strsql);
            string strsql = "update lgsb_sbjxjh_table set status=3 where status=0 and jhdate<'" + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + "'";
            DbContext.ExeSql(strsql);

            //            insert into lgsb_sbjxjh_table(sblist_id, sblistname, sblistcode, sbjxdw_id, sbjxbzu_id, sbjxzq, jxnr, status, jxlb)
            //select sblist_id, sblistname, sblistcode, sbjxdw_id, sbjxbzu_id, sbjxzq, jxnr, status, jxlb
            //from lgsb_sbjxjh_table
            //where status = 0 and jhdate<'2019-11-02'
        }
        #endregion

        #region 计算周设备故障报修统计
        //周设备故障报修统计
        List<sbgz> zhouListSbgz = new List<sbgz>();
        List<sbgzdw> listGzdw = new List<sbgzdw>();
        List<sbgztj> listGztj = new List<sbgztj>();
        /// <summary>
        /// 计算设备故障报修
        /// </summary>
        public void calSbgzbx()
        {
            //查询故障数据
            zhouListSbgz.Clear();
            string strsql = "select djtime,djrdw,jsrdw,clrdw,yzrdw,datediff(jstime,djtime) as sjc,zt as status from lgsb_sbgz_table where ";
            strsql += " djtime > '" + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + " 00:00:00'";
            strsql += "  and djtime < '" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + " 23:59:59' order by djtime desc";
            var data = DbContext.GetDataTable(strsql);
            zhouListSbgz = DT_To_List_gzbx(data);

            #region 获取设置所有登记单位

            //获取登记单位
            //foreach (var gzitem in zhouListSbgz)
            //{
            //    //从登记人单位添加单位
            //    if (listGzdw.Where(o => o.dw == gzitem.djrdw).FirstOrDefault() == null)
            //    {
            //        sbgzdw addgzdw = new sbgzdw();
            //        addgzdw.dw = gzitem.djrdw;
            //        listGzdw.Add(addgzdw);
            //    }
            //    //从接收人单位添加单位
            //    if (gzitem.jsrdw != null && gzitem.jsrdw != "")
            //    {
            //        if (listGzdw.Where(o => o.dw == gzitem.jsrdw).FirstOrDefault() == null)
            //        {
            //            sbgzdw addgzdw = new sbgzdw();
            //            addgzdw.dw = gzitem.djrdw;
            //            listGzdw.Add(addgzdw);
            //        }
            //    }
            //}
            
            listGzdw.Clear();
            if (listGzdw.Where(o => o.dw == "冶炼车间").FirstOrDefault() == null)
            {
                sbgzdw addgzdw = new sbgzdw();
                addgzdw.dw = "冶炼车间";
                listGzdw.Add(addgzdw);
            }
            if (listGzdw.Where(o => o.dw == "精炼车间").FirstOrDefault() == null)
            {
                sbgzdw addgzdw = new sbgzdw();
                addgzdw.dw = "精炼车间";
                listGzdw.Add(addgzdw);
            }
            if (listGzdw.Where(o => o.dw == "连铸车间").FirstOrDefault() == null)
            {
                sbgzdw addgzdw = new sbgzdw();
                addgzdw.dw = "连铸车间";
                listGzdw.Add(addgzdw);
            }
            if (listGzdw.Where(o => o.dw == "冶炼维修车间").FirstOrDefault() == null)
            {
                sbgzdw addgzdw = new sbgzdw();
                addgzdw.dw = "冶炼维修车间";
                listGzdw.Add(addgzdw);
            }
            if (listGzdw.Where(o => o.dw == "连铸维修车间").FirstOrDefault() == null)
            {
                sbgzdw addgzdw = new sbgzdw();
                addgzdw.dw = "连铸维修车间";
                listGzdw.Add(addgzdw);
            }
            if (listGzdw.Where(o => o.dw == "运行车间").FirstOrDefault() == null)
            {
                sbgzdw addgzdw = new sbgzdw();
                addgzdw.dw = "运行车间";
                listGzdw.Add(addgzdw);
            }

            #endregion

            #region 对故障报修各个指标进行计算
            listGztj.Clear();
            foreach (var item in listGzdw)
            {
                //冶炼二车间不再计算
                //if (item.dw == "冶炼二车间")
                //{
                //    continue;
                //}
                sbgztj curtj = new sbgztj();
                curtj.dw = item.dw;
                //查询该单位的登记数量
                curtj.djsl = zhouListSbgz.Where(o => o.djrdw == item.dw).Count();
                //查询该单位的处理数量
                curtj.clsl = zhouListSbgz.Where(o => o.clrdw == item.dw).Count();
                //谁登记谁验证,2为已处理
                curtj.yyzsl = zhouListSbgz.Where(o => o.djrdw == item.dw && o.status >= 2).Count();
                //查询该单位的验证数量
                curtj.yzsl = zhouListSbgz.Where(o => o.yzrdw == item.dw && o.status > 2).Count();
                //查询该单位的已接收数量
                curtj.jssl = zhouListSbgz.Where(o => o.jsrdw == item.dw).Count();
                //计算验证率
                if (curtj.yyzsl > 0)
                {
                    curtj.yzrate = Math.Round(curtj.yzsl * 100 / curtj.yyzsl, 1);
                }
                else
                {
                    curtj.yzrate = 0;
                }

                if (curtj.dw == "冶炼维修车间")
                {
                    //查询该单位的待处理数量
                    curtj.dclsl = zhouListSbgz
                         .Where(o => o.status == 0 && (o.djrdw == item.dw || o.djrdw == "冶炼车间"  || o.djrdw == "精炼车间")).Count();
                    //应接收数据
                    curtj.yjssl = zhouListSbgz
                         .Where(o => (o.djrdw == item.dw || o.djrdw == "冶炼车间"  || o.djrdw == "精炼车间")).Count();
                    if (curtj.yjssl > 0)
                    {
                        curtj.ywsl = zhouListSbgz
                            .Where(o => (o.djrdw == item.dw || o.djrdw == "冶炼车间"  || o.djrdw == "精炼车间") && o.sjc > 2)
                            .Count();
                        curtj.ywrate = Math.Round(curtj.ywsl * 100.0 / curtj.yjssl, 1);
                        //应接收的减去延误的就是及时接收率
                        curtj.jsrate = Math.Round((curtj.yjssl - curtj.ywsl) * 100.0 / curtj.yjssl, 1);
                    }
                }
                else if (curtj.dw == "连铸维修车间")
                {
                    curtj.dclsl = zhouListSbgz
                         .Where(o => o.status == 0 && (o.djrdw == item.dw || o.djrdw == "连铸车间")).Count();
                    curtj.yjssl = zhouListSbgz
                         .Where(o => (o.djrdw == item.dw || o.djrdw == "连铸车间")).Count();
                    if (curtj.yjssl > 0)
                    {
                        curtj.ywsl = zhouListSbgz
                            .Where(o => (o.djrdw == item.dw || o.djrdw == "连铸车间") && o.sjc > 2)
                            .Count();
                        curtj.ywrate = Math.Round(curtj.ywsl * 100.0 / curtj.yjssl, 1);
                        //应接收的减去延误的就是及时接收率
                        curtj.jsrate = Math.Round((curtj.yjssl - curtj.ywsl) * 100.0 / curtj.yjssl, 1);
                    }
                }
                else if (curtj.dw == "运行车间")
                {
                    curtj.dclsl = zhouListSbgz
                         .Where(o => o.status == 0 && (o.djrdw == item.dw)).Count();
                    curtj.yjssl = zhouListSbgz
                         .Where(o => (o.djrdw == item.dw)).Count();
                    if (curtj.yjssl > 0)
                    {
                        curtj.ywsl = zhouListSbgz
                            .Where(o => (o.djrdw == item.dw) && o.sjc > 2)
                            .Count();
                        curtj.ywrate = Math.Round(curtj.ywsl * 100.0 / curtj.yjssl, 1);
                        //应接收的减去延误的就是及时接收率
                        curtj.jsrate = Math.Round((curtj.yjssl - curtj.ywsl) * 100.0 / curtj.yjssl, 1);
                    }
                }


                //查询该单位的延误率（超两天未接收数量/生成数量） 
                //1.没有接收的，无法判定接收单位
                //2.只能从已接收的里计算延误的
                if (curtj.jssl > 0)
                {

                    // curtj.ywrate = Math.Round(zhouListSbgz.Where( o => o.jsrdw == item.dw && o.sjc > 2).Count() * 100.0 / curtj.jssl, 1);
                    //及时处理率
                    curtj.clrate = Math.Round(curtj.clsl * 100.0 / curtj.jssl, 1);
                }
                else
                {
                    // curtj.ywrate = 0;
                    curtj.clrate = 0;
                }
                listGztj.Add(curtj);
            }
            #endregion

            #region 将计算结果保存到数据库
            string insql = "";
            int zhou = (DateTime.Now.AddDays(-1).Day - 1) / 7 + 1;
            string sjd = DateTime.Now.AddDays(-7).Day + "--" + DateTime.Now.AddDays(-1).Day + "日";
            foreach (var item in listGztj)
            {
                //if (item.dw == "冶炼一车间")
                //{
                //    item.dw = "冶炼车间";
                //}
                //insql = "insert into ts_sbbxzb(djdw,logtime,zhou,sjd,djsl,yjssl,clsl,dclsl,yyzsl,yzsl,yzrate,ywsl,ywrate,jsrate,clrate,jssl) ";
                //insql += "values('" + item.dw + "','" + DateTime.Now.ToString("yyyy-MM") + "','" + zhou.ToString() + "','" + sjd + "'," + item.djsl + "," + item.yjssl + "," + item.clsl;
                //insql += "," + item.dclsl + "," + item.yyzsl + "," + item.yzsl + "," + item.yzrate + "," + item.ywsl + "," + item.ywrate + "," + item.jsrate + "," + item.clrate + "," + item.jssl + ")";                
                //DbContext.ExeSql("xgmesweb", insql);

                insql = "insert into sb_bx(djdw,logtime,zhou,sjd,djsl,yjssl,clsl,dclsl,yyzsl,yzsl,yzrate,ywsl,ywrate,jsrate,clrate,jssl) ";
                insql += "values('" + item.dw + "','" + DateTime.Now.AddDays(-1).ToString("yyyy-MM") + "','" + zhou.ToString() + "','" + sjd + "'," + item.djsl + "," + item.yjssl + "," + item.clsl;
                insql += "," + item.dclsl + "," + item.yyzsl + "," + item.yzsl + "," + item.yzrate + "," + item.ywsl + "," + item.ywrate + "," + item.jsrate + "," + item.clrate + "," + item.jssl + ")";
                DbContext.ExeSql("zhgl", insql);
            }
            #endregion


        }
        #endregion

        #region 计算周设备检修统计
        List<sbjxjhtj> listjxtj = new List<sbjxjhtj>();
        List<sbjxjh> zhouListSbjx = new List<sbjxjh>();
        /// <summary>
        /// 计算设备计划报修
        /// </summary>
        public void caljhjx()
        {
            //查询检修数据
            zhouListSbjx.Clear();
            string sql = "select sbjxdw_id as jxdw,status,count(sbjxdw_id) as count from lgsb_sbjxjh_table where jhdate>='" + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + "' ";
            sql += "and jhdate<='" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "' GROUP BY sbjxdw_id,status";
            var datajx = DbContext.GetDataTable(sql);
            zhouListSbjx = DT_To_List_jxjh(datajx);
            #region 计算检修兑现率
            listjxtj.Clear();
            sbjxjhtj yxtj = new sbjxjhtj();
            yxtj.jxdw = "运行车间";
            yxtj.sbsl = zhouListSbjx.Where(o => o.jxdw == "运行车间").Sum(o => o.count);
            yxtj.sjsl = zhouListSbjx.Where(o => o.jxdw == "运行车间" && o.status == 1).Sum(o => o.count);
            if (yxtj.sbsl == 0)
            {
                yxtj.rate = 0;
            }
            else
            {
                yxtj.rate = Math.Round(yxtj.sjsl * 100.0 / yxtj.sbsl, 1);
            }
            listjxtj.Add(yxtj);
            sbjxjhtj lztj = new sbjxjhtj();
            lztj.jxdw = "连铸维修车间";
            lztj.sbsl = zhouListSbjx.Where(o => o.jxdw == "连铸维修车间").Sum(o => o.count);
            lztj.sjsl = zhouListSbjx.Where(o => o.jxdw == "连铸维修车间" && o.status == 1).Sum(o => o.count);
            if (lztj.sbsl == 0)
            {
                lztj.rate = 0;
            }
            else
            {
                lztj.rate = Math.Round(lztj.sjsl * 100.0 / lztj.sbsl, 1);
            }
            listjxtj.Add(lztj);

            sbjxjhtj yltj = new sbjxjhtj();
            yltj.jxdw = "冶炼维修车间";
            yltj.sbsl = zhouListSbjx.Where(o => o.jxdw == "冶炼维修车间").Sum(o => o.count);
            yltj.sjsl = zhouListSbjx.Where(o => o.jxdw == "冶炼维修车间" && o.status == 1).Sum(o => o.count);
            if (yltj.sbsl == 0)
            {
                yltj.rate = 0;
            }
            else
            {
                yltj.rate = Math.Round(yltj.sjsl * 100.0 / yltj.sbsl, 1);
            }
            listjxtj.Add(yltj);
            #endregion

            #region 将数据保存到数据库
            ////////////////////////////////保存到数据库
            string insql = "";
            int zhou = (DateTime.Now.AddDays(-1).Day - 1) / 7 + 1;
            string sjd = DateTime.Now.AddDays(-7).Day + "--" + DateTime.Now.AddDays(-1).Day + "日";
            foreach (var item in listjxtj)
            {
                //insql = "insert into ts_sbjxzb(djdw,logtime,zhou,sjd,sbsl,sjsl,rate) ";
                //insql += "values('" + item.jxdw + "','" + DateTime.Now.ToString("yyyy-MM") + "','" + zhou.ToString() + "','" + sjd + "'," + item.sbsl + "," + item.sjsl + "," + item.rate + ")";
                ////DbContext.ExeSql("xgmesweb", insql);

                insql = "insert into sb_jx(djdw,logtime,zhou,sjd,sbsl,sjsl,rate) ";
                insql += "values('" + item.jxdw + "','" + DateTime.Now.ToString("yyyy-MM") + "','" + zhou.ToString() + "','" + sjd + "'," + item.sbsl + "," + item.sjsl + "," + item.rate + ")";
                DbContext.ExeSql("zhgl", insql);
            }
            #endregion


        }

        #endregion

        #region 计算月报（故障报修）

        List<sbgz> yueListSbgz = new List<sbgz>();
        List<sbgzdw> yuelistGzdw = new List<sbgzdw>();
        // List<sbgztj> yuelistGztj = new List<sbgztj>();
        /// <summary>
        /// 计算设备故障报修月报
        /// </summary>
        public void calYueSbgzbx(string logtime)
        {
            //查询故障数据
            yueListSbgz.Clear();
            listGztj.Clear();
            string stryuesql = "select djtime,djrdw,jsrdw,clrdw,yzrdw,datediff(jstime,djtime) as sjc,zt as status from lgsb_sbgz_table where ";
            stryuesql += " djtime like '" +logtime + "%'";
            var data1 = DbContext.GetDataTable(stryuesql);
            yueListSbgz = DT_To_List_gzbx(data1);

            #region 获取、设置登记单位
           
            if (yuelistGzdw.Where(o => o.dw == "冶炼车间").FirstOrDefault() == null)
            {
                sbgzdw addgzdw = new sbgzdw();
                addgzdw.dw = "冶炼车间";
                yuelistGzdw.Add(addgzdw);
            }

            if (yuelistGzdw.Where(o => o.dw == "精炼车间").FirstOrDefault() == null)
            {
                sbgzdw addgzdw = new sbgzdw();
                addgzdw.dw = "精炼车间";
                yuelistGzdw.Add(addgzdw);
            }

            if (yuelistGzdw.Where(o => o.dw == "连铸车间").FirstOrDefault() == null)
            {
                sbgzdw addgzdw = new sbgzdw();
                addgzdw.dw = "连铸车间";
                yuelistGzdw.Add(addgzdw);
            }

            if (yuelistGzdw.Where(o => o.dw == "冶炼维修车间").FirstOrDefault() == null)
            {
                sbgzdw addgzdw = new sbgzdw();
                addgzdw.dw = "冶炼维修车间";
                yuelistGzdw.Add(addgzdw);
            }
            if (yuelistGzdw.Where(o => o.dw == "连铸维修车间").FirstOrDefault() == null)
            {
                sbgzdw addgzdw = new sbgzdw();
                addgzdw.dw = "连铸维修车间";
                yuelistGzdw.Add(addgzdw);
            }
            if (yuelistGzdw.Where(o => o.dw == "运行车间").FirstOrDefault() == null)
            {
                sbgzdw addgzdw = new sbgzdw();
                addgzdw.dw = "运行车间";
                yuelistGzdw.Add(addgzdw);
            }
            #endregion


            //yuelistGztj.Clear();
            foreach (var item in yuelistGzdw)
            {
                sbgztj curtj = new sbgztj();
                curtj.dw = item.dw;
                //查询该单位的登记数量
                curtj.djsl = yueListSbgz.Where(o => o.djrdw == item.dw).Count();
                //查询该单位的处理数量
                curtj.clsl = yueListSbgz.Where(o => o.clrdw == item.dw).Count();
                //谁登记谁验证,2为已处理
                curtj.yyzsl = yueListSbgz.Where(o => o.djrdw == item.dw && o.status >= 2).Count();
                //查询该单位的验证数量
                curtj.yzsl = yueListSbgz.Where(o => o.yzrdw == item.dw && o.status > 2).Count();
                //查询该单位的已接收数量
                curtj.jssl = yueListSbgz.Where(o => o.jsrdw == item.dw).Count();
                //计算验证率
                if (curtj.yyzsl > 0)
                {
                    curtj.yzrate = Math.Round(curtj.yzsl * 100 / curtj.yyzsl, 1);
                }
                else
                {
                    curtj.yzrate = 0;
                }

                #region 对接收率、延误率进行计算（除以应接收数量）
                if (curtj.dw == "冶炼维修车间")
                {
                    //查询该单位的待处理数量
                    curtj.dclsl = yueListSbgz
                         .Where(o => o.status == 0 && (o.djrdw == item.dw || o.djrdw == "冶炼车间" || o.djrdw == "精炼车间")).Count();
                    //应接收数据
                    curtj.yjssl = yueListSbgz
                         .Where(o => (o.djrdw == item.dw || o.djrdw == "冶炼车间" || o.djrdw == "精炼车间")).Count();
                    if (curtj.yjssl > 0)
                    {
                        curtj.ywsl = yueListSbgz
                            .Where(o => (o.djrdw == item.dw || o.djrdw == "冶炼车间"  || o.djrdw == "精炼车间") && o.sjc > 2)
                            .Count();
                        curtj.ywrate = Math.Round(curtj.ywsl * 100.0 / curtj.yjssl, 1);
                        //应接收的减去延误的就是及时接收率
                        curtj.jsrate = Math.Round((curtj.yjssl - curtj.ywsl) * 100.0 / curtj.yjssl, 1);
                    }
                }
                else if (curtj.dw == "连铸维修车间")
                {
                    curtj.dclsl = yueListSbgz
                         .Where(o => o.status == 0 && (o.djrdw == item.dw || o.djrdw == "连铸车间")).Count();
                    curtj.yjssl = yueListSbgz
                         .Where(o => (o.djrdw == item.dw || o.djrdw == "连铸车间")).Count();
                    if (curtj.yjssl > 0)
                    {
                        curtj.ywsl = yueListSbgz
                            .Where(o => (o.djrdw == item.dw || o.djrdw == "连铸车间") && o.sjc > 2)
                            .Count();
                        curtj.ywrate = Math.Round(curtj.ywsl * 100.0 / curtj.yjssl, 1);
                        //应接收的减去延误的就是及时接收率
                        curtj.jsrate = Math.Round((curtj.yjssl - curtj.ywsl) * 100.0 / curtj.yjssl, 1);
                    }
                }
                else if (curtj.dw == "运行车间")
                {
                    curtj.dclsl = yueListSbgz
                         .Where(o => o.status == 0 && (o.djrdw == item.dw)).Count();
                    curtj.yjssl = yueListSbgz
                         .Where(o => (o.djrdw == item.dw)).Count();
                    if (curtj.yjssl > 0)
                    {
                        curtj.ywsl = yueListSbgz
                            .Where(o => (o.djrdw == item.dw) && o.sjc > 2)
                            .Count();
                        curtj.ywrate = Math.Round(curtj.ywsl * 100.0 / curtj.yjssl, 1);
                        //应接收的减去延误的就是及时接收率
                        curtj.jsrate = Math.Round((curtj.yjssl - curtj.ywsl) * 100.0 / curtj.yjssl, 1);
                    }
                }
                #endregion

                //查询该单位的延误率（超两天未接收数量/生成数量） 
                //1.没有接收的，无法判定接收单位
                //2.只能从已接收的里计算延误的
                if (curtj.jssl > 0)
                {

                    // curtj.ywrate = Math.Round(zhouListSbgz.Where( o => o.jsrdw == item.dw && o.sjc > 2).Count() * 100.0 / curtj.jssl, 1);
                    //及时处理率
                    curtj.clrate = Math.Round(curtj.clsl * 100.0 / curtj.jssl, 1);
                }
                else
                {
                    // curtj.ywrate = 0;
                    curtj.clrate = 0;
                }
                listGztj.Add(curtj);
            }

            string insql = "";
            int zhou = 0;
            string sjd = "";
            foreach (var item in listGztj)
            {
                insql = "insert into sb_bx(djdw,logtime,zhou,sjd,djsl,yjssl,clsl,dclsl,yyzsl,yzsl,yzrate,ywsl,ywrate,jsrate,clrate,jssl) ";
                insql += "values('" + item.dw + "','" + logtime + "','" + zhou.ToString() + "','" + sjd + "'," + item.djsl + "," + item.yjssl + "," + item.clsl;
                insql += "," + item.dclsl + "," + item.yyzsl + "," + item.yzsl + "," + item.yzrate + "," + item.ywsl + "," + item.ywrate + "," + item.jsrate + "," + item.clrate + "," + item.jssl + ")";
                DbContext.ExeSql("zhgl", insql);
            }

        }

        #endregion

        #region 计算月报（设备检修）

        List<sbjxjh> yueListSbjx = new List<sbjxjh>();
        List<sbjxjhtj> yuelistjxtj = new List<sbjxjhtj>();
        /// <summary>
        /// 计算设备计划报修（月）
        /// </summary>
        public void yuecaljhjx()
        {
            yueListSbjx.Clear();
            string sqlyue = "select sbjxdw_id as jxdw,status,count(sbjxdw_id) as count from lgsb_sbjxjh_table where jhdate like '" + DateTime.Now.AddDays(-7).ToString("yyyy-MM");
            sqlyue += "%' GROUP BY sbjxdw_id,status";
            var datajxyue = DbContext.GetDataTable(sqlyue);
            yueListSbjx = DT_To_List_jxjh(datajxyue);

            yuelistjxtj.Clear();
            sbjxjhtj yxtj = new sbjxjhtj();
            yxtj.jxdw = "运行车间";
            yxtj.sbsl = yueListSbjx.Where(o => o.jxdw == "运行车间").Sum(o => o.count);
            yxtj.sjsl = yueListSbjx.Where(o => o.jxdw == "运行车间" && o.status == 1).Sum(o => o.count);
            if (yxtj.sbsl == 0)
            {
                yxtj.rate = 0;
            }
            else
            {
                yxtj.rate = Math.Round(yxtj.sjsl * 100.0 / yxtj.sbsl, 1);
            }
            yuelistjxtj.Add(yxtj);
            sbjxjhtj lztj = new sbjxjhtj();
            lztj.jxdw = "连铸维修车间";
            lztj.sbsl = yueListSbjx.Where(o => o.jxdw == "连铸维修车间").Sum(o => o.count);
            lztj.sjsl = yueListSbjx.Where(o => o.jxdw == "连铸维修车间" && o.status == 1).Sum(o => o.count);
            if (lztj.sbsl == 0)
            {
                lztj.rate = 0;
            }
            else
            {
                lztj.rate = Math.Round(lztj.sjsl * 100.0 / lztj.sbsl, 1);
            }
            yuelistjxtj.Add(lztj);

            sbjxjhtj yltj = new sbjxjhtj();
            yltj.jxdw = "冶炼维修车间";
            yltj.sbsl = yueListSbjx.Where(o => o.jxdw == "冶炼维修车间").Sum(o => o.count);
            yltj.sjsl = yueListSbjx.Where(o => o.jxdw == "冶炼维修车间" && o.status == 1).Sum(o => o.count);
            if (yltj.sbsl == 0)
            {
                yltj.rate = 0;
            }
            else
            {
                yltj.rate = Math.Round(yltj.sjsl * 100.0 / yltj.sbsl, 1);
            }
            yuelistjxtj.Add(yltj);
            ////////////////////////////////保存到数据库
            string insql = "";
            int zhou = 0;
            string sjd = "";
            foreach (var item in yuelistjxtj)
            {             
                insql = "insert into sb_jx(djdw,logtime,zhou,sjd,sbsl,sjsl,rate) ";
                //logtime为上个月
                insql += "values('" + item.jxdw + "','" + DateTime.Now.AddDays(-1).ToString("yyyy-MM") + "','" + zhou.ToString() + "','" + sjd + "'," + item.sbsl + "," + item.sjsl + "," + item.rate + ")";
                DbContext.ExeSql("zhgl", insql);
            }

        }


        #endregion

        #region datatable转list
        /// <summary>
        /// 设备故障报修datatable转list
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<sbgz> DT_To_List_gzbx(DataTable dt)
        {
            List<sbgz> sbgzlist = new List<sbgz>();
            if (dt == null)
            {
                return sbgzlist;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sbgz sbgz = new sbgz();
                sbgz.djtime = (DateTime)dt.Rows[i]["djtime"];
                if (dt.Rows[i]["djrdw"].ToString() == "冶炼一车间" || dt.Rows[i]["djrdw"].ToString() == "冶炼二车间")
                {
                    sbgz.djrdw = "冶炼车间";
                }
                else
                {
                    sbgz.djrdw = dt.Rows[i]["djrdw"].ToString();
                }
                sbgz.jsrdw = dt.Rows[i]["jsrdw"].ToString();
                sbgz.clrdw = dt.Rows[i]["clrdw"].ToString();
                sbgz.yzrdw = dt.Rows[i]["yzrdw"].ToString();
                sbgz.status = Convert.ToInt16(dt.Rows[i]["status"].ToString());
                if (dt.Rows[i]["sjc"] != null && dt.Rows[i]["sjc"].ToString() != "" && dt.Rows[i]["sjc"].ToString() != null)
                {
                    sbgz.sjc = Convert.ToInt16(dt.Rows[i]["sjc"].ToString());
                }
                else
                {
                    if (sbgz.djtime < DateTime.Now.AddDays(-2.5))
                    {
                        sbgz.sjc = 3;
                    }
                }
                sbgzlist.Add(sbgz);
            }
            return sbgzlist;
        }

        /// <summary>
        /// 设备检修计划转datatable转list
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<sbjxjh> DT_To_List_jxjh(DataTable dt)
        {
            List<sbjxjh> sbgzlist = new List<sbjxjh>();
            if (dt == null)
            {
                return sbgzlist;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sbjxjh sbjx = new sbjxjh();
                if (dt.Rows[i]["jxdw"].ToString() == "13")
                {
                    sbjx.jxdw = "运行车间";
                }
                else if (dt.Rows[i]["jxdw"].ToString() == "17")
                {
                    sbjx.jxdw = "连铸维修车间";
                }
                else if (dt.Rows[i]["jxdw"].ToString() == "18")
                {
                    sbjx.jxdw = "冶炼维修车间";
                }

                sbjx.count = Convert.ToInt32(dt.Rows[i]["count"].ToString());
                if (dt.Rows[i]["status"].ObjToBool() == true)
                {
                    sbjx.status = 1;
                }
                else
                {
                    sbjx.status = 0;
                }
                sbgzlist.Add(sbjx);
            }
            return sbgzlist;
        }

        #endregion

        #region 计算点检周报
        /// <summary>
        /// 计算点检周报
        /// </summary>
        public void caldjzb()
        {

            try
            {
                string yue = DateTime.Now.AddDays(-1).ToString("yyyy-MM");
                string endday = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                string beginday = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                int zhou = (DateTime.Now.AddDays(-1).Day - 1) / 7 + 1;//第几周
                string sjd = DateTime.Now.AddDays(-7).Day + "--" + DateTime.Now.AddDays(-1).Day + "日";//时间段

                #region 计算点检周报
                //查询数据
                string sql = "select sbdjdw_id, djcishu, ydjcishu,(select count(distinct(comment)) from lgsb_sbdjjl_table where djdate >= '" + beginday + "'  ";
                sql += "     and djdate < '" + endday + " 23:59:59' and sbdjdw_id = d.sbdjdw_id and status = 0) as faxianwt,(select count(distinct(sbdjjl.comment))  from lgsb_sbdjjl_table sbdjjl inner  ";
                sql += "     join lgsb_user_part_table banzu on sbdjjl.sbdjbzu_id = banzu.id  where sbdjjl.djdate >= '" + beginday + "' ";
                sql += "     and sbdjjl.djdate < '" + endday + " 23:59:59' and sbdjjl.sbdjdw_id = d.sbdjdw_id and sbdjjl.status = 0 and banzu.name like '%管理%') as faxianglwt  ";
                sql += "       from(select sbdjdw_id, sum( case when djcishu > ydjcishu then ydjcishu else djcishu end) as djcishu, sum(ydjcishu) as ydjcishu  ";
                sql += "              from(   ";
                sql += "             select a.sblist_id, a.sbdjdw_id, a.sbdjbzu_id, a.djcishu, b.cishu as ydjcishu from  ";
                sql += "              (select sblist_id, sbdjdw_id, sbdjbzu_id, sbdjzq_id, count(0) as djcishu   ";
                sql += "             from lgsb_sbdjjl_table where djdate >= '" + beginday + "'  ";
                sql += "              and djdate < '" + endday + " 23:59:59' GROUP BY sblist_id, sbdjdw_id, sbdjbzu_id, sbdjzq_id, sbdjlb_id) a, lgsb_sbdjbz_sbdjzq_table b   ";
                sql += "            where a.sbdjzq_id = b.id) c group by sbdjdw_id) d  ";
                //sql += "  GROUP BY sblistcode, sbjxbzu_id ";
                var data = DbContext.GetDataTable(sql);
                //dataGridView2.DataSource = data;

                foreach (DataRow item in data.Rows)
                {
                    #region 计算各车间数据，并保存到数据库
                    //                insert into sb_dj(djdw, logtime, sjd, zhou, ydjsl, sdjsl, wtsl, glwtsl)
                    //values('冶炼维修车间', '2020-03', '11', '1', 3, 4, 5, 6)
                    string djdw = getDanweiName(item["sbdjdw_id"].ToString());
                    if (djdw == "")
                    {
                        continue;
                    }
                    int cjzb = 1;//管理人员点检发现问题车间指标（没有问题数量）
                    int ljwtsl = 0;//累计问题数量
                    int glwt = Convert.ToInt16(item["faxianglwt"]);//管理发现问题
                    int djcishu = Convert.ToInt16(item["djcishu"]);//实际点检次数
                    int ydjcishu = Convert.ToInt16(item["ydjcishu"]);//实际点检次数
                    #region 计算点检率
                    if (djdw == "连铸车间")
                    {
                        ydjcishu = 336;
                    }
                    else if (djdw == "冶炼车间")
                    {
                        ydjcishu = 170;
                    }
                    double djrate = Math.Round((djcishu * 100.0 / ydjcishu), 1);//点检率
                    if (djrate > 100)
                    {
                        djrate = 100;
                    }
                    #endregion

                    #region 计算维修车间管理人员发现问题指标
                    int curlj = 0;
                    double wtrate = 0;
                    if (djdw == "运行车间" || djdw == "冶炼维修车间" || djdw == "连铸维修车间")
                    {
                        if (djdw == "运行车间")
                        {
                            cjzb = 35;
                        }
                        else if (djdw == "冶炼维修车间")
                        {
                            cjzb = 70;
                        }
                        else if (djdw == "连铸维修车间")
                        {
                            cjzb = 60;
                        }

                        string ljsql = "select case when max(glwtsl_lj) is null then 0 else max(glwtsl_lj) end  as leiji  from sb_dj ";
                        ljsql += "where logtime='" + yue + "' and djdw='" + djdw + "'";
                        var ljdata = DbContext.GetDataTable("zhgl", ljsql);
                        if (ljdata != null && ljdata.Rows.Count > 0)
                        {
                            ljwtsl = Convert.ToInt16(ljdata.Rows[0][0].ToString());
                        }
                        curlj = ljwtsl + glwt;
                        //发现问题指标计算
                        wtrate = Math.Round((curlj * 100.0 / cjzb), 1);
                    }
                    #endregion

                    string insql = " insert into sb_dj(djdw, logtime, sjd, zhou, ydjsl, sdjsl, wtsl, glwtsl,cjzb,djrate,wtrate,glwtsl_lj)";
                    insql += " values('" + djdw + "', '" + yue + "', '" + sjd + "', '" + zhou + "', " + ydjcishu + ", " + djcishu + ", " + item["faxianwt"].ToString() + ", " + glwt + "," + cjzb + "," + djrate + "," + wtrate + "," + curlj + ")";
                    DbContext.ExeSql("zhgl", insql);
                    #endregion

                }
                #endregion

                #region 计算分类周报

                //            select sbdjlb_id, djsl,(select count(distinct(comment)) from lgsb_sbdjjl_table where djdate >= '2020-02-24'
                //and djdate < '2020-03-02' and status = 0 and sbdjlb_id = a.sbdjlb_id ) as wtsl from
                //     (select sbdjlb_id, count(0) as djsl from lgsb_sbdjjl_table where djdate >= '2020-02-24'
                //     and djdate < '2020-03-02' group by sbdjlb_id) a
                string flsql = "  select sbdjlb_id, djsl,(select count(distinct(comment)) from lgsb_sbdjjl_table where djdate >= '" + beginday + "' ";
                flsql += "  and djdate < '" + endday + " 23:59:59' and status = 0 and sbdjlb_id = a.sbdjlb_id ) as wtsl from  ";
                flsql += "  (select sbdjlb_id, count(0) as djsl from lgsb_sbdjjl_table where djdate >= '" + beginday + "'  ";
                flsql += "  and djdate < '" + endday + " 23:59:59' group by sbdjlb_id) a  ";
                var fldata = DbContext.GetDataTable(flsql);
                foreach (DataRow item in fldata.Rows)
                {
                    string djfl = "";
                    if (item["sbdjlb_id"].ToString() == "11")
                    {
                        djfl = "维修点检";
                    }
                    else if (item["sbdjlb_id"].ToString() == "12")
                    {
                        djfl = "管理点检";
                    }
                    else if (item["sbdjlb_id"].ToString() == "14")
                    {
                        djfl = "岗位点检";
                    }
                    if (djfl == "")
                    {
                        continue;
                    }
                    string insql = " insert into sb_dj_fl(logtime, sjd, zhou, djfl, djsl, wtsl)";
                    insql += " values('" + yue + "', '" + sjd + "', '" + zhou + "','" + djfl + "', " + item["djsl"].ToString() + ", " + item["wtsl"].ToString() + ")";
                    DbContext.ExeSql("zhgl", insql);
                }
                #endregion

            }
            catch
            {

            }

        }

        public string getDanweiName(string id)
        {
            string name = "";
            if (id == "8")
            {
                name = "冶炼车间";
            }
            else if (id == "10")
            {
                name = "精炼车间";
            }
            else if (id == "11")
            {
                name = "连铸车间";
            }
            else if (id == "13")
            {
                name = "运行车间";
            }
            else if (id == "17")
            {
                name = "连铸维修车间";
            }
            else if (id == "18")
            {
                name = "冶炼维修车间";
            }
            return name;
        }
        #endregion
        private void button7_Click(object sender, EventArgs e)
        {
            // calYueSbgzbx(dateTimePicker1.Value.ToString("yyyy-MM"));
            ////每天执行
            //exc_jx_expire();

            calYueSbgzbx(DateTime.Now.AddDays(-7).ToString("yyyy-MM"));
            if (DateTime.Now.Day == 1)
            {
                calYueSbgzbx(DateTime.Now.AddDays(-1).ToString("yyyy-MM"));
                //yuecaljhjx();
            }
            //if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            //{
            //    calSbgzbx();
            //    caljhjx();
            //    caldjzb();
            //}
        }


    }
}
