using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace inputscrap
{
    public partial class Form1 : Form
    {
        List<scrap> listScrap = new List<scrap>();
        Entities db = new Entities();
        Entities_mes mesdb = new Entities_mes();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            wulogtime = dateTimePicker1.Text;    
            callScrapWeight();
        }

        string wulogtime = "";
        public void callScrapWeight()
        {
            listScrap.Clear();
            try
            {

                string logtime = wulogtime;
                //从物流系统查询当日自产废钢
                
                string strsql = "select decode(sum((suttle-deduction)/1000),null,0,sum((suttle-deduction)/1000)) as weight,'自产废钢' as name from M_MEASURE_T ";
                       strsql += " where to_char(suttletime,'yyyy-MM-dd')='" + logtime + "' and taskcode in  ('13008', '11018', '13015', '13006', '11011','11010','13011')";
                var zcscrap = db.Database.SqlQuery<scrap>(strsql).FirstOrDefault();
                var zcweight = 0d;
                if (zcscrap != null)
                {
                    zcweight = zcscrap.weight;
                }
                listScrap.Add(zcscrap);


                //从物流系统查询当日内回收废钢              
                strsql = " select decode(sum((suttle-deduction)/1000),null,0,sum((suttle-deduction)/1000)) as weight,'内回收废钢' as name from M_MEASURE_T ";
                strsql += " where to_char(suttletime,'yyyy-MM-dd')='" + logtime + "' and  (((materialname = '甩废' or materialname = '切废' or materialname = '自产废钢') and targetname = '炼钢') or taskcode in  ('12026','12029'))";
                var hsscrap = db.Database.SqlQuery<scrap>(strsql).FirstOrDefault();
                var hsweight = 0d;
                if (hsscrap != null)
                {
                    hsweight = hsscrap.weight;
                }
                listScrap.Add(hsscrap);

                //从物流系统查询当日进厂废钢            
                strsql = " select decode(sum((suttle-deduction)/1000),null,0,sum((suttle-deduction)/1000)) as weight,'进厂外购废钢' as name from M_MEASURE_T ";
                strsql += " where to_char(suttletime,'yyyy-MM-dd')='" + logtime + "' and   targetname = '储运生铁废钢库'";
                var indata = db.Database.SqlQuery<scrap>(strsql).FirstOrDefault();
                var inweight = 0d;
                if (indata != null)
                {
                    inweight = indata.weight;
                }
                listScrap.Add(indata);

                //             select decode(sum((suttle-deduction)/ 1000),null,0,sum((suttle - deduction) / 1000)) as weight,'外购制钢生铁' as name
                //from M_MEASURE_T  where to_char(suttletime, 'yyyy-MM-dd') = '2019-11-22' and targetname = '储运生铁废钢库' and materialname = '外购制钢生铁'
                //从物流系统查询当日外购制钢生铁      
                strsql = " select decode(sum((suttle-deduction)/1000),null,0,sum((suttle-deduction)/1000)) as weight,'外购制钢生铁' as name from M_MEASURE_T ";
                strsql += " where to_char(suttletime,'yyyy-MM-dd')='" + logtime + "' and   targetname = '储运生铁废钢库' and materialname = '外购制钢生铁'";
                var stdata = db.Database.SqlQuery<scrap>(strsql).FirstOrDefault();
                var stweight = 0d;
                if (stdata != null)
                {
                    stweight = stdata.weight;
                }
                listScrap.Add(stdata);


                //保存到MES数据库

                var mes_log = mesdb.TS_INSCRAP.Where(o => o.LOGTIME == logtime).FirstOrDefault();
                if (mes_log == null)
                {
                    TS_INSCRAP inscrap = new TS_INSCRAP();
                    inscrap.LOGTIME = logtime;
                    inscrap.ZCWEIGHT = (decimal)zcweight;
                    inscrap.HSWEIGHT = (decimal)hsweight;
                    inscrap.INWEIGHT = (decimal)inweight;
                    inscrap.WEIGHT = (decimal)(inweight+ zcweight + hsweight);
                    inscrap.STWEIGHT= (decimal)stweight; 
                    mesdb.TS_INSCRAP.Add(inscrap);
                    mesdb.SaveChanges();
                    label1.Text = "导入时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = listScrap;
            }
            catch (Exception ex)
            {
                string a = ex.ToString();


            }
           
        }
        /// <summary>
        /// 每半小时执行一次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.Hour == 0)
            {
                wulogtime= DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"); 
                callScrapWeight();
            }
            //精炼炉电耗分档计算 不再计算
            //if (DateTime.Now.Hour == 1 || DateTime.Now.Hour == 5 || DateTime.Now.Hour == 9 || DateTime.Now.Hour == 13 || DateTime.Now.Hour == 17 || DateTime.Now.Hour == 21)
            //{
            //   // calDianHao();
            //}
            ////大于7点10
            //if (DateTime.Now.Hour == 7 && DateTime.Now.Minute>10)
            //{
            //    //计算能源数据
            //    calXiaoHao(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            //}
           
        }
        List<CLF_HEAT_GEARS> listGears = new List<CLF_HEAT_GEARS>();
        List<CLF_BASE_DATA> listLFbase = new List<CLF_BASE_DATA>();
        List<fdjs> fdjslist = new List<fdjs>();
        List<CLF_HEAT_GEARS_ITEM> listGearsItem = new List<CLF_HEAT_GEARS_ITEM>();
        List<lfstatus> statuslist = new List<lfstatus>();

        public void calDianHao()
        {
            try
            {

                listGearsItem.Clear();
                #region 查询需要计算分档加热时间的炉次
                listLFbase.Clear();
                string sqlbase = " select* from clf_base_data where productiondate < sysdate - 0.05";
                sqlbase += " and productiondate > sysdate - 2 and treatno not in (select treatno from CLF_HEAT_GEARS_item )";
                listLFbase = mesdb.Database.SqlQuery<CLF_BASE_DATA>(sqlbase).ToList();
                #endregion

                #region 查询出档位变化记录
                listGears.Clear();
                listGears = mesdb.Database.SqlQuery<CLF_HEAT_GEARS>("select * from CLF_HEAT_GEARS a where logtime >sysdate -5").ToList();
                #endregion

                #region 对每一炉进行计算
                foreach (var baseitem in listLFbase)
                {
                    try
                    {

                        #region 查询该处理号的加热开始、结束的状态
                        statuslist.Clear();
                        string strSql = "select status,change_time as logtime,treatno from clf_process_satus where treatno = '" + baseitem.TREATNO + "' and(status = '2' or status = '3') order by change_time";
                        statuslist = mesdb.Database.SqlQuery<lfstatus>(strSql).ToList();
                        quchonglfstatus(statuslist);
                        //各状态所对应的档位
                        foreach (var item in statuslist)
                        {
                            var curGears = listGears.Where(o => o.LOGTIME <= item.logtime && o.TREATNO.StartsWith(item.treatno.ToString().Substring(0, 1)))
                                    .OrderByDescending(o => o.LOGTIME).FirstOrDefault();
                            if (curGears != null)
                            {
                                item.gears = (int)curGears.GEARS;
                            }
                            else
                            {
                                item.gears = 0;
                            }
                        }
                        if (statuslist.Count == 0)
                        {
                            continue;
                        }
                        #endregion

                        #region 查询加热过程中，档位是否有变化， 若有，将档位变化加入到statuslist中
                        DateTime kstime, jstime;
                        try
                        {
                            kstime = statuslist.Where(o => o.status == 2).OrderBy(o => o.logtime).FirstOrDefault().logtime;
                            jstime = statuslist.Where(o => o.status == 3).OrderByDescending(o => o.logtime).FirstOrDefault().logtime;
                        }
                        catch
                        {
                            continue;
                        }
                        //根据加热开始时间，最后加热结束时间，查询是否有档位变化
                        var listTemp = listGears.Where(o => o.LOGTIME < jstime && o.LOGTIME > kstime && o.TREATNO == baseitem.TREATNO).ToList();
                        foreach (var item in listTemp)
                        {
                            lfstatus lfsts = new lfstatus();
                            lfsts.logtime = (DateTime)item.LOGTIME;
                            lfsts.gears = (int)item.GEARS;
                            statuslist.Add(lfsts);
                        }
                        #endregion

                        #region  根据加热状态变化和档位变化，计算各档位加热时间
                        fdjslist.Clear();
                        statuslist = statuslist.OrderBy(o => o.logtime).ToList();

                        int curgears = 0;
                        DateTime? curtime = null;
                        foreach (var item in statuslist)
                        {
                            //加热开始
                            if (item.status == 2)
                            {
                                curgears = item.gears;
                                curtime = item.logtime;
                            }
                            //加热中途换挡
                            else if (item.status == 0)
                            {
                                //没有加热开始信号，就不计算
                                if (curtime == null)
                                {
                                    continue;
                                }
                                var curfd = fdjslist.Where(o => o.gears == curgears).FirstOrDefault();
                                if (curfd == null)
                                {
                                    fdjs fd = new fdjs();
                                    fd.gears = curgears;
                                    fd.val = (item.logtime - (DateTime)curtime).TotalSeconds;
                                    fdjslist.Add(fd);
                                }
                                else
                                {
                                    curfd.val += (item.logtime - (DateTime)curtime).TotalSeconds;
                                }
                                curgears = item.gears;
                                curtime = item.logtime;
                            }
                            //加热结束时，判断是否有加热开始信号，没有则不计算
                            else if (item.status == 3)
                            {
                                //如果第一个状态是加热结束，则不计算
                                if (curtime == null)
                                {
                                    continue;
                                }
                                var curfd = fdjslist.Where(o => o.gears == curgears).FirstOrDefault();
                                if (curfd == null)
                                {
                                    fdjs fd = new fdjs();
                                    fd.gears = curgears;
                                    fd.val = (item.logtime - (DateTime)curtime).TotalSeconds;
                                    fdjslist.Add(fd);
                                }
                                else
                                {
                                    curfd.val += (item.logtime - (DateTime)curtime).TotalSeconds;
                                }
                                //将开始时间置为null；
                                curtime = null;
                            }
                        }
                        #endregion

                        #region 将计算的结果保存到listGearsItem
                        try
                        {
                            CLF_HEAT_GEARS_ITEM gearsitem = new CLF_HEAT_GEARS_ITEM();
                            gearsitem.TREATNO = baseitem.TREATNO;
                            gearsitem.HEATID = baseitem.HEATID;
                            gearsitem.PRODUCTIONTIME = ((DateTime)baseitem.PRODUCTIONDATE).ToString("yyyy-MM-dd HH:mm:ss");
                            //加热次数 结束信号的个数
                            gearsitem.HEATCOUNT = statuslist.Where(o => o.status == 3).Count();
                            gearsitem.STEELGRADE = baseitem.STEELGRADE;
                            gearsitem.HEAD = baseitem.HEADFURNACE;
                            if (baseitem.TEAM == "1")
                            {
                                gearsitem.TEAM = "甲班";
                            }
                            else if (baseitem.TEAM == "2")
                            {
                                gearsitem.TEAM = "乙班";
                            }
                            else if (baseitem.TEAM == "3")
                            {
                                gearsitem.TEAM = "丙班";
                            }
                            else if (baseitem.TEAM == "4")
                            {
                                gearsitem.TEAM = "丁班";
                            }
                            if (fdjslist.Count > 0)
                            {
                                gearsitem.GEARS1 = fdjslist[0].gears.ToString();
                                gearsitem.TIME1 = Math.Round((double)fdjslist[0].val / 60, 1).ToString();
                            }
                            if (fdjslist.Count > 1)
                            {
                                gearsitem.GEARS2 = fdjslist[1].gears.ToString();
                                gearsitem.TIME2 = Math.Round((double)fdjslist[1].val / 60, 1).ToString();
                            }
                            if (fdjslist.Count > 2)
                            {
                                gearsitem.GEARS3 = fdjslist[2].gears.ToString();
                                gearsitem.TIME3 = Math.Round((double)fdjslist[2].val / 60, 1).ToString();
                            }
                            if (fdjslist.Count > 3)
                            {
                                gearsitem.GEARS4 = fdjslist[3].gears.ToString();
                                gearsitem.TIME4 = Math.Round((double)fdjslist[3].val / 60, 1).ToString();
                            }
                            double totaltime = 0;
                            foreach (var item1 in fdjslist)
                            {
                                totaltime += item1.val;
                            }
                            gearsitem.TOTALTIME = Math.Round(totaltime / 60, 1).ToString();

                            listGearsItem.Add(gearsitem);
                        }
                        catch
                        {
                            continue;
                        }

                        #endregion
                    }
                    catch
                    {
                        continue;
                    }



                }
                #endregion
                //将数据保存到数据库
                mesdb.CLF_HEAT_GEARS_ITEM.AddRange(listGearsItem);
                mesdb.SaveChanges();
            }
            catch
            {

            }

        }

        public void quchonglfstatus(List<lfstatus> liststatus)
        {
            if (liststatus.Count > 2&&liststatus[0].status==2&&liststatus[1].status==2)
            {
                liststatus.RemoveAt(0);
                quchonglfstatus(liststatus);
            }    
        }


        private void button2_Click(object sender, EventArgs e)
        {
            calDianHao();
            calDianHao();
   
        }

        XGDBEntities xgrt = new XGDBEntities();
        private void button3_Click(object sender, EventArgs e)
        {
            var listweight = xgrt.CCM5_GrossWeight.Where(o=>o.Permit_Delete_Flag==1).OrderBy(o => o.aa_id).ToList();
            dataGridView2.DataSource = listweight;

            if (listweight.Count == 1)
            {
                //select* from clf_ladle_weight where heatid = (select heatid from cccm_base_data where treatno = 
                //    (select max(treatno) from cccm_base_data where ccmid = 'S65' ))
                string strsql = "select * from clf_ladle_weight where heatid = (select heatid from cccm_base_data ";
                strsql += " where treatno = (select max(treatno) from cccm_base_data where ccmid = 'S65' ))";
                var lfweight = mesdb.Database.SqlQuery<CLF_LADLE_WEIGHT>(strsql).AsQueryable().FirstOrDefault();
                if (lfweight != null)
                {
                    #region 当前炉号的重量记录里，连铸机没有毛重时，对数据进行处理
                    if (lfweight.CCMGROSSWEIGHT == null)
                    {
                        decimal pz = 0;
                        if (listweight[0].GrossWeight > listweight[0].GrossWeightB)
                        {
                            lfweight.CCMGROSSWEIGHT = Convert.ToDecimal(listweight[0].GrossWeight);
                            lfweight.CCMLASTTAREWEIGHT = Convert.ToDecimal(listweight[0].GrossWeightB);
                            lfweight.CCMINPUTAREWEIGHT = Convert.ToDecimal(listweight[0].TareWeight);
                            lfweight.AREA = "A";
                            pz = Convert.ToDecimal(listweight[0].GrossWeightB);
                        }
                        else
                        {
                            lfweight.CCMGROSSWEIGHT = Convert.ToDecimal(listweight[0].GrossWeightB);
                            lfweight.CCMLASTTAREWEIGHT = Convert.ToDecimal(listweight[0].GrossWeight);
                            lfweight.CCMINPUTAREWEIGHT = Convert.ToDecimal(listweight[0].TareWeight);
                            lfweight.AREA = "B";
                            pz = Convert.ToDecimal(listweight[0].GrossWeight);
                        }
                        try
                        {
                            mesdb.Entry<CLF_LADLE_WEIGHT>(lfweight).State = System.Data.Entity.EntityState.Modified;
                            mesdb.SaveChanges();
                        }
                        catch
                        {

                        }
                       
                        if (pz > 10)
                        {
                            string preheatid = (Convert.ToInt64(lfweight.HEATID) - 1).ToString();
                            var preweight = mesdb.CLF_LADLE_WEIGHT.Where(o => o.HEATID == preheatid).FirstOrDefault();
                            preweight.CCMTAREWEIGHT = pz;
                            mesdb.SaveChanges();
                        }
                        xgrt.CCM5_GrossWeight.Remove(listweight[0]);
                        xgrt.SaveChanges();
                    }
                    #endregion
                }
                else
                {
                    listweight[0].Permit_Delete_Flag = 0;
                    xgrt.Entry<CCM5_GrossWeight>(listweight[0]).State = System.Data.Entity.EntityState.Modified;
                    xgrt.SaveChanges();
                }

            }
            else
            {
                foreach (var item in listweight)
                {
                    //select* from CLF_LADLE_WEIGHT where heatid is not null and ccmgrossweight is null
                    //string strsql = "select * from clf_ladle_weight where heatid = (select heatid from cccm_base_data ";
                    //strsql += " where treatno = (select max(treatno) from cccm_base_data where ccmid = 'S65' ))";
                    var lfweight = mesdb.CLF_LADLE_WEIGHT.Where(o => o.HEATID != null && o.CCMGROSSWEIGHT == null).OrderBy(o => o.HEATID).FirstOrDefault();
                    if (lfweight != null)
                    {
                        #region 当前炉号的重量记录里对数据进行处理

                        decimal pz = 0;
                        if (listweight[0].GrossWeight > listweight[0].GrossWeightB)
                        {
                            lfweight.CCMGROSSWEIGHT = Convert.ToDecimal(listweight[0].GrossWeight);
                            lfweight.CCMLASTTAREWEIGHT = Convert.ToDecimal(listweight[0].GrossWeightB);
                            lfweight.CCMINPUTAREWEIGHT = Convert.ToDecimal(listweight[0].TareWeight);
                            lfweight.AREA = "A";
                            pz = Convert.ToDecimal(listweight[0].GrossWeightB);
                        }
                        else
                        {
                            lfweight.CCMGROSSWEIGHT = Convert.ToDecimal(listweight[0].GrossWeightB);
                            lfweight.CCMLASTTAREWEIGHT = Convert.ToDecimal(listweight[0].GrossWeight);
                            lfweight.AREA = "B";
                            lfweight.CCMINPUTAREWEIGHT = Convert.ToDecimal(listweight[0].TareWeight);
                            pz = Convert.ToDecimal(listweight[0].GrossWeight);
                        }
                        try
                        {
                            mesdb.Entry<CLF_LADLE_WEIGHT>(lfweight).State = System.Data.Entity.EntityState.Modified;
                            mesdb.SaveChanges();
                        }
                        catch
                        {

                        }

                        if (pz > 10)
                        {
                            string preheatid = (Convert.ToInt64(lfweight.HEATID) - 1).ToString();
                            var preweight = mesdb.CLF_LADLE_WEIGHT.Where(o => o.HEATID == preheatid).FirstOrDefault();
                            preweight.CCMTAREWEIGHT = pz;
                            mesdb.SaveChanges();
                        }
                        xgrt.CCM5_GrossWeight.Remove(item);
                        xgrt.SaveChanges();
                        #endregion
                    }


                }

            }

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //button3_Click(null, null);
        }

        cmsdbEntities cmsdb = new cmsdbEntities();
        private void button4_Click(object sender, EventArgs e)
        {        
                // string logtime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                string logtime = dateTimePicker2.Text.ToString();
                calXiaoHao(logtime);

        }

        public void calXiaoHao(string logtime)
        {
            try
            {              
               //如果记录已存在，则不再执行
                if (cmsdb.dt_ems.Where(o => o.logtime == logtime).Count() > 0)
                {
                    return;
                }
                var cmslist = mesdb.Database.SqlQuery<cms>("select code,value from INDICATORVALUES where bizdate='" + logtime + "'").ToList();
                dt_ems cur_ems = new dt_ems();
                cur_ems.logtime = logtime;
                cur_ems.tg_chanliang = Math.Round(Convert.ToDouble(cmslist.Where(o => o.code == "TG_001").FirstOrDefault().value), 1);
                cur_ems.tg_shuihao = Math.Round(Convert.ToDouble(cmslist.Where(o => o.code == "TG_ShuiHao").FirstOrDefault().value), 1);
                cur_ems.tg_dianhao = Math.Round(Convert.ToDouble(cmslist.Where(o => o.code == "TG_DianHao").FirstOrDefault().value), 1);
                cur_ems.tg_jiaolumeiqi = Math.Round(Convert.ToDouble(cmslist.Where(o => o.code == "TG_JiaoLuMeiQi").FirstOrDefault().value), 1);
                cur_ems.tg_zhuanlumeiqi = Math.Round(Convert.ToDouble(cmslist.Where(o => o.code == "TG_ZhuanLuMeiQi").FirstOrDefault().value), 1);
                cur_ems.tg_danqi = Math.Round(Convert.ToDouble(cmslist.Where(o => o.code == "TG_DanQi").FirstOrDefault().value), 1);
                cur_ems.tg_yangqi = Math.Round(Convert.ToDouble(cmslist.Where(o => o.code == "TG_YangQi").FirstOrDefault().value), 1);
                cur_ems.tg_yaqi = Math.Round(Convert.ToDouble(cmslist.Where(o => o.code == "TG_YaQi").FirstOrDefault().value), 1);
                cur_ems.tg_zhengqiwaigong = Math.Round(Convert.ToDouble(cmslist.Where(o => o.code == "TG_ZhengQiWaiGong").FirstOrDefault().value), 1);
                cur_ems.tg_meiqihuishou = Math.Round(Convert.ToDouble(cmslist.Where(o => o.code == "TG_MeiQiHuiShou").FirstOrDefault().value), 1);
                try
                {
                    cur_ems.dl_zhengqiwaigong = Math.Round(Convert.ToDouble(cmslist.Where(o => o.code == "TG_dlgrhzq").FirstOrDefault().value), 1);
                }
                catch
                {
                    cur_ems.dl_zhengqiwaigong = 0;
                }
                //TG_dlgrhzq
                cur_ems.bxg_chanliang = Math.Round(Convert.ToDouble(cmslist.Where(o => o.code == "BXG_001").FirstOrDefault().value), 1);
                cur_ems.bxg_shuihao = Math.Round(Convert.ToDouble(cmslist.Where(o => o.code == "BXG_ShuiHao").FirstOrDefault().value), 1);
                cur_ems.bxg_dianhao = Math.Round(Convert.ToDouble(cmslist.Where(o => o.code == "BXG_DianHao").FirstOrDefault().value), 1);
                cur_ems.bxg_jiaolumeiqi = Math.Round(Convert.ToDouble(cmslist.Where(o => o.code == "BXG_JiaoLuMeiQi").FirstOrDefault().value), 1);
                cur_ems.bxg_zhuanlumeiqi = Math.Round(Convert.ToDouble(cmslist.Where(o => o.code == "BXG_ZhuanLuMeiQi").FirstOrDefault().value), 1);
                cur_ems.bxg_danqi = Math.Round(Convert.ToDouble(cmslist.Where(o => o.code == "BXG_DanQi").FirstOrDefault().value), 1);
                cur_ems.bxg_yangqi = Math.Round(Convert.ToDouble(cmslist.Where(o => o.code == "BXG_YangQi").FirstOrDefault().value), 1);
                cur_ems.bxg_yaqi = Math.Round(Convert.ToDouble(cmslist.Where(o => o.code == "BXG_YaQi").FirstOrDefault().value), 1);
                cur_ems.bxg_zhengqiwaigong = Math.Round(Convert.ToDouble(cmslist.Where(o => o.code == "BXG_ZhengQiWaiGong").FirstOrDefault().value), 1);
                if (cur_ems.bxg_chanliang > 100)
                {
                    cur_ems.bxg_flag = 1;
                }
                else
                {
                    cur_ems.bxg_flag = 0;
                }
                cmsdb.dt_ems.Add(cur_ems);
                cmsdb.SaveChanges();
                dataGridView3.DataSource = cmslist;
                calDayXiaoHao(logtime);
            }
            catch
            {


            }
        }

        public void calDayXiaoHao(string logtime)
        {
            double day = Convert.ToDouble(logtime.Substring(8, 2));
            int totalDay = calDayOfMonth(logtime.Substring(5, 2));
            var cur_ems = cmsdb.dt_ems.Where(o => o.logtime == logtime).FirstOrDefault();
            var ems_config = cmsdb.dt_ems_config.ToList();
            foreach (var item in ems_config)
            {
                dt_ems_day cur_ems_day = new dt_ems_day();
                cur_ems_day.xuhao = item.xuhao;
                if (item.name == "碳钢产量" || item.name == "蒸汽外供")
                {
                    cur_ems_day.name = item.name + "(" + item.zhibiao_sc + ")";
                }
                else
                {
                    cur_ems_day.name = item.name;
                }
                if (item.name == "碳钢产量" || item.name == "蒸汽外供")
                {
                    cur_ems_day.zhibiao_sc = (Math.Round(day * (Convert.ToDouble(item.zhibiao_sc)) * 1.0 / totalDay,2)).ToString();
                }
                else
                {
                    cur_ems_day.zhibiao_sc = item.zhibiao_sc;
                }
                cur_ems_day.zhibiao_cw = item.zhibiao_cw;
                if (item.name == "碳钢产量")
                {
                    cur_ems_day.yongliang = cur_ems.tg_chanliang - cur_ems.bxg_chanliang * cur_ems.bxg_flag;
                }
                else if (item.name == "水耗")
                {
                    cur_ems_day.yongliang = cur_ems.tg_shuihao - cur_ems.bxg_shuihao * cur_ems.bxg_flag;
                }
                else if (item.name == "电")
                {
                    cur_ems_day.yongliang = cur_ems.tg_dianhao + cur_ems.bxg_dianhao * (1-cur_ems.bxg_flag)+item.diansun;
                }
                else if (item.name == "焦炉煤气")
                {
                    cur_ems_day.yongliang = cur_ems.tg_jiaolumeiqi - cur_ems.bxg_jiaolumeiqi * cur_ems.bxg_flag;
                }
                else if (item.name == "转炉煤气")
                {
                    cur_ems_day.yongliang = cur_ems.tg_zhuanlumeiqi - cur_ems.bxg_zhuanlumeiqi * cur_ems.bxg_flag;
                }
                else if (item.name == "氮气")
                {
                    cur_ems_day.yongliang = cur_ems.tg_danqi - cur_ems.bxg_danqi * cur_ems.bxg_flag;
                }
                else if (item.name == "氧气")
                {
                    cur_ems_day.yongliang = cur_ems.tg_yangqi - cur_ems.bxg_yangqi * cur_ems.bxg_flag;
                }
                else if (item.name == "氩气")
                {
                    cur_ems_day.yongliang = cur_ems.tg_yaqi - cur_ems.bxg_yaqi * cur_ems.bxg_flag;
                }
                else if (item.name == "蒸汽外供")
                {
                    cur_ems_day.yongliang = cur_ems.tg_zhengqiwaigong - cur_ems.bxg_zhengqiwaigong * cur_ems.bxg_flag-cur_ems.dl_zhengqiwaigong;
                }
                else if (item.name == "煤气回收")
                {
                    cur_ems_day.yongliang = cur_ems.tg_meiqihuishou;
                }
                cur_ems_day.logtime = logtime;
                cur_ems_day.danjia = item.danjia;

                cmsdb.dt_ems_day.Add(cur_ems_day);
                cmsdb.SaveChanges();
            }
            if (cur_ems.bxg_flag == 0)
            {
               // dt_ems_day 

            }

           

        }
        /// <summary>
        /// 二月份 返回29天
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public int calDayOfMonth(string month)
        {
            if (month == "01" || month == "03" || month == "05" || month == "07" || month == "08" || month == "10" || month == "12")
            {
                return 31;
            }
            else if (month == "04" || month == "06" || month == "09" || month == "11")
            {
                return 30;
            }
            else
            {
                return 29;
            }
        }
    }
}
