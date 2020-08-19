using Rcw.Method;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;

namespace opcBase
{
    public class ccmCut
    {

        public  ccmCutStrand ccmCutStrand_1 = new ccmCutStrand();
        public  ccmCutStrand ccmCutStrand_2 = new ccmCutStrand();
        public  ccmCutStrand ccmCutStrand_3 = new ccmCutStrand();
        public  ccmCutStrand ccmCutStrand_4 = new ccmCutStrand();
        public string CCMID = "S63";

        public string heatid { get; set; }
       
        public int arrive { get; set; }
        public int val_arrive { get; set; }
        public int start { get; set; }
        public int val_start { get; set; }
        public int stop { get; set; }
        public int val_stop { get; set; }
        /// <summary>
        /// 大包接缝
        /// </summary>
        public int ladlefeng { get; set; }
        public int val_ladlefeng { get; set; }

        public double getValue(int id)
        {
            return Convert.ToDouble(PlcSvr.GetInstance().getVal(id));
        }
        public int getValueInt(int id)
        {
            try
            {
                string val = PlcSvr.GetInstance().getVal(id);
                int i = Convert.ToInt16(val);
                return i;
            }
            catch(Exception ex)
            {
                return 0;
                throw new Exception(ex.ToString() + id + ":");
            }
           
        }

        public void updateData()
        {
            arrive = getValueInt(val_arrive);
            start = getValueInt(val_start);
            stop = getValueInt(val_stop);
            ladlefeng = getValueInt(val_ladlefeng);
            ccmCutStrand_1.updateData();
            ccmCutStrand_2.updateData();
            ccmCutStrand_3.updateData();
            ccmCutStrand_4.updateData();

        }

        public void setupDateConfig(string ccmid,int startweizhi,double zhupichangdu)
        {
            CCMID = ccmid;
            //设置流数据
            ccmCutStrand_1.setupDataConfig(CCMID, 1);
            ccmCutStrand_2.setupDataConfig(CCMID, 2);
            ccmCutStrand_3.setupDataConfig(CCMID, 3);
            ccmCutStrand_4.setupDataConfig(CCMID, 4);
            val_arrive = startweizhi;
            val_start = startweizhi + 1;
            val_stop= startweizhi + 2;
            val_ladlefeng= startweizhi + 3;
            ccmCutStrand_1.valid_cutstatus= startweizhi + 4;
            ccmCutStrand_1.valid_strandstatus = startweizhi + 5;
            ccmCutStrand_1.valid_billetLength = startweizhi + 6;
            ccmCutStrand_1.valid_trackvalue = startweizhi + 7;
            ccmCutStrand_1.valid_speed = startweizhi + 8;
            ccmCutStrand_1.valid_track = startweizhi + 9;
            ccmCutStrand_1.zhupichangdu = zhupichangdu;

            ccmCutStrand_2.valid_cutstatus = startweizhi + 10;
            ccmCutStrand_2.valid_strandstatus = startweizhi + 11;
            ccmCutStrand_2.valid_billetLength = startweizhi + 12;
            ccmCutStrand_2.valid_trackvalue = startweizhi + 13;
            ccmCutStrand_2.valid_speed = startweizhi + 14;
            ccmCutStrand_2.valid_track = startweizhi + 15;
            ccmCutStrand_2.zhupichangdu = zhupichangdu;

            ccmCutStrand_3.valid_cutstatus = startweizhi + 16;
            ccmCutStrand_3.valid_strandstatus = startweizhi + 17;
            ccmCutStrand_3.valid_billetLength = startweizhi + 18;
            ccmCutStrand_3.valid_trackvalue = startweizhi + 19;
            ccmCutStrand_3.valid_speed = startweizhi + 20;
            ccmCutStrand_3.valid_track = startweizhi + 21;
            ccmCutStrand_3.zhupichangdu = zhupichangdu;

            ccmCutStrand_4.valid_cutstatus = startweizhi + 22;
            ccmCutStrand_4.valid_strandstatus = startweizhi + 23;
            ccmCutStrand_4.valid_billetLength = startweizhi + 24;
            ccmCutStrand_4.valid_trackvalue = startweizhi + 25;
            ccmCutStrand_4.valid_speed = startweizhi + 26;
            ccmCutStrand_4.valid_track = startweizhi + 27;
            ccmCutStrand_4.zhupichangdu = zhupichangdu;
        }
        /// <summary>
        /// 获取各流速度和跟踪值（每1s接受一个数据）
        /// </summary>
        public void getSpeedAndTrack()
        {
            try
            {
                double L1S = ccmCutStrand_1.getSpeed();
                double L2S = ccmCutStrand_2.getSpeed();
                double L3S = ccmCutStrand_3.getSpeed();
                double L4S = ccmCutStrand_4.getSpeed();
                if (L1S > 0 || L2S > 0 || L3S > 0 || L4S > 0)
                {
                    double L1T = ccmCutStrand_1.getTrack();
                    double L2T = ccmCutStrand_2.getTrack();
                    double L3T = ccmCutStrand_3.getTrack();
                    double L4T = ccmCutStrand_4.getTrack();
                    ccmCutStrand_1.speed = L1S;
                    ccmCutStrand_1.track = L1T;
                    ccmCutStrand_2.speed = L2S;
                    ccmCutStrand_2.track = L2T;
                    ccmCutStrand_3.speed = L3S;
                    ccmCutStrand_3.track = L3T;
                    ccmCutStrand_4.speed = L4S;
                    ccmCutStrand_4.track = L4T;
                    #region 根据跟踪值，计算钢坯位置
                    calZhuPiWeizhi(ccmCutStrand_1);
                    calZhuPiWeizhi(ccmCutStrand_2);
                    calZhuPiWeizhi(ccmCutStrand_3);
                    calZhuPiWeizhi(ccmCutStrand_4);
                    #endregion
                    oraDbHelp service = new oraDbHelp();
                    service.connectionString = "Data Source = 192.168.48.117/XGMES; User Id = XGMES; Password =XGMES;";
                    string exeSql = " insert into CUT_Speed_DATA(CCMID,SPEED_1ST,TRACK_1ST,SPEED_2ST,TRACK_2ST,SPEED_3ST,TRACK_3ST,SPEED_4ST,TRACK_4ST) ";
                    exeSql += " values('" + CCMID + "', " + L1S + ", " + L1T + ", " + L2S + ", " + L2T + ", " + L3S + ", " + L3T + ", " + L4S + ", " + L4T + ") ";
                    service.Update(exeSql);

                    #region  向数据库保存铸坯跟踪数据
                    string sqlstr = " insert into cut_strand_info_his (ccmid,strandid,heatid,starttrack,endtrack,zhupiStart,zhupiEnd,speed) ";
                    sqlstr += " values('" + CCMID + "', 1, '" + ccmCutStrand_1.heatid + "', " + ccmCutStrand_1.pitouTrack + "," + ccmCutStrand_1.track + ", " + ccmCutStrand_1.zhupiStart + ", " + ccmCutStrand_1.zhupiEnd + ", " + ccmCutStrand_1.speed + ") ";
                    sqlstr += " ,('" + CCMID + "', 2, '" + ccmCutStrand_2.heatid + "', " + ccmCutStrand_2.pitouTrack + "," + ccmCutStrand_2.track + ", " + ccmCutStrand_2.zhupiStart + ", " + ccmCutStrand_2.zhupiEnd + ", " + ccmCutStrand_2.speed + ") ";
                    sqlstr += " ,('" + CCMID + "', 3, '" + ccmCutStrand_3.heatid + "', " + ccmCutStrand_3.pitouTrack + "," + ccmCutStrand_3.track + ", " + ccmCutStrand_3.zhupiStart + ", " + ccmCutStrand_3.zhupiEnd + ", " + ccmCutStrand_3.speed + ") ";
                    sqlstr += " ,('" + CCMID + "', 4, '" + ccmCutStrand_4.heatid + "', " + ccmCutStrand_4.pitouTrack + "," + ccmCutStrand_4.track + ", " + ccmCutStrand_4.zhupiStart + ", " + ccmCutStrand_4.zhupiEnd + ", " + ccmCutStrand_4.speed + ") ";
                    try
                    {
                        var db = new sqlDbHelp();
                        db.ExeSql(sqlstr);
                    }
                    catch (Exception ex)
                    {
                        SysLog.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-- 铸机：" + CCMID + ";insert into cut_strand_info_his方法出错" + ex.ToString() + sqlstr);
                    }
                    #endregion


                }
            }
            catch
            { 
            
            }                 
        }

        public void calZhuPiWeizhi(ccmCutStrand cutStrand)
        {
            if (cutStrand.speed > 0)
            {
                if (cutStrand.strandstatus == 1)
                {                  
                    //开浇后，铸坯长度，就是等于跟踪值-坯头的跟踪值                 
                    cutStrand.zhupiStart = cutStrand.track - cutStrand.pitouTrack;
                    cutStrand.zhupiEnd = 0;
                }
                else if (cutStrand.strandstatus == 0)
                {                   
                    //当坯尾位置小于铸坯火切机位置时，坯尾一直向前滚动
                    if (cutStrand.zhupiEnd < cutStrand.zhupichangdu)
                    {
                        cutStrand.zhupiEnd = cutStrand.zhupiEnd + (cutStrand.speed / 60);
                        cutStrand.zhupiStart = cutStrand.zhupiStart + (cutStrand.speed / 60);
                    }
                    
                }
            }
        }


        /// <summary>
        /// 连铸机到达开浇停浇状态
        /// </summary>
        /// <param name="status"></param>
        public void acceptCasterStatus(int status)
        {
            if (status == 2)
            {
                ////暂停5秒后
                //Thread.Sleep(5000);
                //保存到数据库
                //炉号先变更
                heatid = "";
                oraDbHelp service = new oraDbHelp();

                #region 1、读取生产计划 2、传输给切割 3、删除
                try
                {
                    string selectSql = " select a.guid,a.ccmid,a.heatid,b.steelgrade,b.length,c.weight,d.tundish_heatnum from cccm_download_heatid a ,cplan_tapping b , ";
                    selectSql += " csteel_data c, cccm_process_data d where a.heatid = b.heatid and a.heatid = c.heatid and a.heatid = d.heatid(+) ";
                    selectSql += " and a.ccmid = '" + CCMID + "'  order by a.c_ts desc";
                    var steelGradeInfo = service.Query(selectSql);
                    if (steelGradeInfo != null && steelGradeInfo.Tables.Count > 0 && steelGradeInfo.Tables[0].Rows.Count > 0)
                    {
                        int length = 0;
                        try
                        {
                            length = Convert.ToInt32(steelGradeInfo.Tables[0].Rows[0]["length"]);
                        }
                        catch { }

                        double weight = 0;
                        try
                        {
                            weight = Convert.ToDouble(steelGradeInfo.Tables[0].Rows[0]["weight"]);
                        }
                        catch { }

                        int heatnum = 0;
                        try
                        {
                            heatnum = Convert.ToInt32(steelGradeInfo.Tables[0].Rows[0]["tundish_heatnum"]);
                        }
                        catch { }

                        string insertSql = "insert into CUT_PRODUCTPLAN(heatid, ccmid, length, steelgrade, netweight, tundish_heatnum) values";
                        insertSql += "('" + steelGradeInfo.Tables[0].Rows[0]["heatid"].ToString() + "', '" + steelGradeInfo.Tables[0].Rows[0]["ccmid"].ToString()
                            + "', " + length + ", '" + steelGradeInfo.Tables[0].Rows[0]["steelgrade"].ToString() + "', " + weight + ", " + heatnum + ")";
                        service.connectionString = "Data Source = 192.168.48.117/XGMES; User Id = XGMES; Password =XGMES;";
                        service.Update(insertSql);

                        foreach (DataRow item in steelGradeInfo.Tables[0].Rows)
                        {
                            service.connectionString = "Data Source = 192.168.36.153/XGMES; User Id = XGMES; Password =XGMES;";
                            string deleteSql = "delete from cccm_download_heatid where guid='" + item["guid"].ToString() + "'";
                            service.Update(deleteSql);
                        }
                        #region 将炉号赋给大包的heatid,同时赋给各个流的下一个炉号
                        heatid = steelGradeInfo.Tables[0].Rows[0]["heatid"].ToString();
                        //if(ccmCutStrand_1.heatid=="")
                        //ccmCutStrand_1.nextheatid = heatid;
                        //ccmCutStrand_2.nextheatid = heatid;
                        //ccmCutStrand_3.nextheatid = heatid;
                        //ccmCutStrand_4.nextheatid = heatid;
                        #endregion


                    }
                }
                catch
                { 
                
                }
                
                #endregion

                #region 传输开浇信号
                #endregion
                service.connectionString = "Data Source = 192.168.48.117/XGMES; User Id = XGMES; Password =XGMES;";
                string exeSql = " insert into CUT_CCMSTATUS_DATA(CCMID,CCMSTATUS) values('" + CCMID + "', " + status + ") ";
                service.Update(exeSql);
            
            }
            else
            {
                //保存到数据库
                oraDbHelp service = new oraDbHelp();
                service.connectionString = "Data Source = 192.168.48.117/XGMES; User Id = XGMES; Password =XGMES;";
                string exeSql = " insert into CUT_CCMSTATUS_DATA(CCMID,CCMSTATUS) values('" + CCMID + "', " + status + ") ";
                service.Update(exeSql);
            }
            
        }

        /// <summary>
        /// 大包缝，可以理解为各流开浇时刻，将流的下一炉号赋给当前炉号
        /// </summary>
        /// <param name="status"></param>
        public void acceptLadlefeng(int status)
        {
            //保存到数据库
            double L1T = ccmCutStrand_1.getTrack();
            double L2T = ccmCutStrand_2.getTrack();
            double L3T = ccmCutStrand_3.getTrack();
            double L4T = ccmCutStrand_4.getTrack();
            oraDbHelp service = new oraDbHelp();
            service.connectionString = "Data Source = 192.168.48.117/XGMES; User Id = XGMES; Password =XGMES;";
            string exeSql = " insert into CUT_LADLEFENG_DATA(CCMID,LADLEFENG,TRACK_1ST,TRACK_2ST,TRACK_3ST,TRACK_4ST) ";
            exeSql += " values('" + CCMID + "'," + status + "," + L1T + "," + L2T + "," + L3T + "," + L4T + ") ";
            service.Update(exeSql);

            #region 流炉号变更

            if (ccmCutStrand_1.heatid == "" || ccmCutStrand_1.heatid == null)
            {
                oraDbHelp service1 = new oraDbHelp();
                string selectSql = " select heatid from(select heatid, PRODUCTIONDATE from cccm_base_data  where ccmid = '" + CCMID + "' order by PRODUCTIONDATE desc)  where rownum <= 2";
                var steelGradeInfo = service1.Query(selectSql);
                if (steelGradeInfo != null && steelGradeInfo.Tables.Count > 0 && steelGradeInfo.Tables[0].Rows.Count > 0)
                {
                    ccmCutStrand_1.heatid = steelGradeInfo.Tables[0].Rows[1]["heatid"].ToString();
                    ccmCutStrand_2.heatid = steelGradeInfo.Tables[0].Rows[1]["heatid"].ToString();
                    ccmCutStrand_3.heatid = steelGradeInfo.Tables[0].Rows[1]["heatid"].ToString();
                    ccmCutStrand_4.heatid = steelGradeInfo.Tables[0].Rows[1]["heatid"].ToString();
                }
            }
                    

            #region 记录大包缝数据
            // insert into cut_ladlefenginfo(ccmid,strandid,heatid,endtrack) 
            //values('S63', 1, '99', 33.3)
            try
            {
                var db = new sqlDbHelp();
                string sql = "";
                
                if (ccmCutStrand_1.strandstatus == 1)
                {
                    ladleFeng s1ladlefeng = new ladleFeng();
                    s1ladlefeng.heatid = ccmCutStrand_1.heatid;
                    s1ladlefeng.startTrack = L1T;
                    ccmCutStrand_1.listLadleFeng.Add(s1ladlefeng);
                    try
                    {
                        sql = " insert into cut_ladlefenginfo(ccmid,strandid,heatid,endtrack) ";
                        sql += " values('" + CCMID + "', 1, '" + s1ladlefeng.heatid + "', " + s1ladlefeng.startTrack + ") ";
                        db.ExeSql(sql);
                    }
                    catch(Exception ex)
                    {
                        SysLog.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-- 铸机：" + CCMID + ";insert into cut_ladlefenginfo方法出错1" + ex.ToString());
                    }

                    

                }

                if (ccmCutStrand_2.strandstatus == 1)
                {
                    ladleFeng s2ladlefeng = new ladleFeng();
                    s2ladlefeng.heatid = ccmCutStrand_2.heatid;
                    s2ladlefeng.startTrack = L2T;
                    ccmCutStrand_2.listLadleFeng.Add(s2ladlefeng);
                    
                    try
                    {
                        sql = " insert into cut_ladlefenginfo(ccmid,strandid,heatid,endtrack) ";
                        sql += " values('" + CCMID + "', 2, '" + s2ladlefeng.heatid + "', " + s2ladlefeng.startTrack + ") ";
                        db.ExeSql(sql);
                    }
                    catch (Exception ex)
                    {
                        SysLog.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-- 铸机：" + CCMID + ";insert into cut_ladlefenginfo方法出错2" + ex.ToString());
                    }
                }

                if (ccmCutStrand_3.strandstatus == 1)
                {
                    ladleFeng s3ladlefeng = new ladleFeng();
                    s3ladlefeng.heatid = ccmCutStrand_3.heatid;
                    s3ladlefeng.startTrack = L3T;
                    ccmCutStrand_3.listLadleFeng.Add(s3ladlefeng);
                    try
                    {
                        sql = " insert into cut_ladlefenginfo(ccmid,strandid,heatid,endtrack) ";
                        sql += " values('" + CCMID + "', 3, '" + s3ladlefeng.heatid + "', " + s3ladlefeng.startTrack + ") ";
                        db.ExeSql(sql);
                    }
                    catch (Exception ex)
                    {
                        SysLog.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-- 铸机：" + CCMID + ";insert into cut_ladlefenginfo方法出错3" + ex.ToString());
                    }

                }

                if (ccmCutStrand_4.strandstatus == 1)
                {
                    ladleFeng s4ladlefeng = new ladleFeng();
                    s4ladlefeng.heatid = ccmCutStrand_4.heatid;
                    s4ladlefeng.startTrack = L4T;
                    ccmCutStrand_4.listLadleFeng.Add(s4ladlefeng);
                    try
                    {
                        sql = " insert into cut_ladlefenginfo(ccmid,strandid,heatid,endtrack) ";
                        sql += " values('" + CCMID + "', 4, '" + s4ladlefeng.heatid + "', " + s4ladlefeng.startTrack + ") ";
                        db.ExeSql(sql);
                    }
                    catch (Exception ex)
                    {
                        SysLog.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-- 铸机：" + CCMID + ";insert into cut_ladlefenginfo方法出错4" + ex.ToString());
                    }

                }
                                                       
            }
            catch (Exception ex)
            {
                SysLog.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-- 铸机：" + CCMID + ";insert into cut_ladlefenginfo方法出错5" + ex.ToString());
            }
            #endregion

            if (heatid == "" || heatid == null)
            {
                oraDbHelp service1 = new oraDbHelp();          
                string selectSql = " select heatid from(select heatid, PRODUCTIONDATE from cccm_base_data  where ccmid = '"+CCMID+"' order by PRODUCTIONDATE desc)  where rownum = 1";            
                var steelGradeInfo = service1.Query(selectSql);
                if (steelGradeInfo != null && steelGradeInfo.Tables.Count > 0 && steelGradeInfo.Tables[0].Rows.Count > 0)
                {
                    heatid = steelGradeInfo.Tables[0].Rows[0]["heatid"].ToString();
                }
            }

            ccmCutStrand_1.heatid = heatid;
            ccmCutStrand_2.heatid = heatid;
            ccmCutStrand_3.heatid = heatid;
            ccmCutStrand_4.heatid = heatid;

            #endregion

            
        }
    }
}
