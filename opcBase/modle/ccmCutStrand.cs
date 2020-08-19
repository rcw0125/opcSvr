using Rcw.Method;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opcBase
{
    public  class ccmCutStrand
    {
        public string CCMID = "S63";
        public string heatid { get; set; }
      
        public List<ladleFeng> listLadleFeng = new List<ladleFeng>();
        /// <summary>
        /// 火切机位置（结晶器到火切机的长度）
        /// </summary>
        public double zhupichangdu = 24.8;
        public double zhupiStart { get; set; }
        public double zhupiEnd { get; set; }
        //坯头跟踪值
        public double pitouTrack { get; set; }
        public int strandid { get; set; }
        
        //切割状态
        public int cutstatus  { get; set; }
        //切割长度
        public double billetLength { get; set; }
        //切割时的跟踪值
        public double trackvalue { get; set; }

        //跟踪值
        public double track { get; set; }
        //拉速
        public double speed { get; set; }
        //流状态
        public int strandstatus { get; set; }
        public int valid_cutstatus { get; set; }
      
        public int valid_billetLength { get; set; }
       
        public int valid_trackvalue { get; set; }

     
        public int valid_track { get; set; }
        
        public int valid_speed { get; set; }
       
        public int valid_strandstatus { get; set; }

        /// <summary>
        /// 设置铸机，流号
        /// </summary>
        /// <param name="ccmid"></param>
        /// <param name="strand_id"></param>
        public void setupDataConfig(string ccmid,int strand_id)
        {

            CCMID = ccmid;
            strandid = strand_id;
        }
        public double getValue(int id)
        {
            return Convert.ToDouble(PlcSvr.GetInstance().getVal(id));
        }
        public int getValueInt(int id)
        {
            try
            {
                int i = Convert.ToInt16(PlcSvr.GetInstance().getVal(id));
                return i;
            }
            catch
            {
                return 0;
            }
            
        }
        //
      
        //更新数据
        public void updateData()
        {
            cutstatus = getValueInt(valid_cutstatus);
            billetLength = getValue(valid_billetLength);
            trackvalue = getValue(valid_trackvalue);
            speed = getValue(valid_speed);
            strandstatus = getValueInt(valid_strandstatus);
            track= getValue(valid_track);
        }
        /// <summary>
        /// 各流切割状态的变化
        /// </summary>
        /// <param name="cutStatus"></param>
        public void acceptCutStatus(int cutStatus)
        {
            //将流号，切割状态、跟踪值保存到数据库
            double billet_length = getValue(valid_billetLength);
             double track_value= getValue(valid_trackvalue);
            double track = getValue(valid_track);
            string curheatid = heatid;
            double yuanPiTouTrack = pitouTrack;
            double yuanPiWeiTrack = 0;
            double dabaofeng = 0;
            try
            {
                oraDbHelp service = new oraDbHelp();
                service.connectionString = "Data Source = 192.168.48.117/XGMES; User Id = XGMES; Password =XGMES;";
                string exeSql = " insert into CUT_STRAND_DATA(CCMID, STRANDID, STATUS, BILLETLENGTH, TRACKVALUE) ";
                exeSql += " values('" + CCMID + "', " + strandid + ", " + cutStatus + ", " + billet_length + ", " + track_value + ") ";
                service.Update(exeSql);


                if (cutStatus == 3)
                {
                    #region 切割时，铸坯位置的跟踪计算
                    if (strandstatus == 1)
                    {
                        //正常浇注模式，坯头跟踪值，为跟踪值-固定值                  
                        pitouTrack = track - zhupichangdu;
                        zhupiStart = zhupichangdu;
                    }
                    else
                    {
                        //拉尾坯时，坯头跟踪值，原坯头跟踪值+定尺
                        pitouTrack = pitouTrack + billet_length;
                        zhupiStart = zhupichangdu;
                        //坯尾的位置：固定长度--（实际红坯的长度）
                        zhupiEnd = zhupichangdu - (track - pitouTrack);
                    }
                    #endregion
                    yuanPiWeiTrack = pitouTrack;
                    #region 切割时，对大包缝的判断
                    #endregion
                    if (listLadleFeng.Count > 0)
                    {
                        
                        dabaofeng = listLadleFeng[0].startTrack;                       
                       //如果小于大包缝的跟踪，则使用大包缝的炉号
                        if (yuanPiWeiTrack < listLadleFeng[0].startTrack)
                        {
                            curheatid = listLadleFeng[0].heatid;
                        }
                        //大包缝在中间时
                        else if (yuanPiWeiTrack >= listLadleFeng[0].startTrack && yuanPiTouTrack < listLadleFeng[0].startTrack)
                        {
                           
                                double zhongdian = (yuanPiWeiTrack - yuanPiTouTrack) / 2 + yuanPiTouTrack;
                                if (zhongdian < listLadleFeng[0].startTrack)
                                {
                                    curheatid = listLadleFeng[0].heatid;
                                }
                                else
                                {
                                    //如果存在两个大包缝，则用第二个
                                    if (listLadleFeng.Count > 1)
                                    {
                                        curheatid = listLadleFeng[1].heatid;
                                    }
                                }
                            #region 在数据库中将此状态置为1
                            //update cut_ladlefenginfo set status=1,u_ts='1' where heatid='' and strandid=1 
                            try
                            {
                                var db = new sqlDbHelp();
                                string sql = "update cut_ladlefenginfo set status=1,u_ts='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where heatid='" + listLadleFeng[0].heatid + "' and strandid=" + strandid + "";
                                db.ExeSql(sql);
                            }
                            catch (Exception ex)
                            {
                                SysLog.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-- 铸机：" + CCMID + ";流号：" + strandid + ";acceptCutStatus-update cut_ladlefenginfo方法出错" + ex.ToString());
                            }
                            #endregion
                            //移除第一个大包缝
                            listLadleFeng.RemoveAt(0);
                            //如果有第二个大包缝，则继续移除
                            if (listLadleFeng.Count > 0)
                            {
                                if (yuanPiWeiTrack >= listLadleFeng[0].startTrack)
                                {
                                    listLadleFeng.RemoveAt(0);
                                }
                            }
                        }
                    }   
                    try
                    {
                        var dbhelp = new sqlDbHelp();
                        string inSql = "insert into cut_info(ccmid,strandid,heatid,toutrack,weitrack,fengtrack) ";
                        inSql += "values('" + CCMID + "', " + strandid + ", '" + curheatid + "', " + yuanPiTouTrack + "," + yuanPiWeiTrack + ", " + dabaofeng + ")";
                        dbhelp.ExeSql(inSql);
                    }
                    catch (Exception ex)
                    {
                        SysLog.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-- 铸机：" + CCMID + ";流号：" + strandid + ";acceptCutStatus-insert into cut_info方法出错" + ex.ToString());
                    }
                }
                
            }
            catch (Exception ex)
            {
                SysLog.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-- 铸机：" + CCMID + ";流号：" + strandid + ";acceptCutStatus方法出错" + ex.ToString());
            }

         

       
        }

        public double getSpeed()
        {
            return getValue(valid_speed);
        }
        public double getTrack()
        {
            return getValue(valid_track);
        }
        /// <summary>
        /// 各流是否开浇的状态
        /// </summary>
        /// <param name="strandStatus"></param>
        public void acceptStrandStatus(int strand_Status)
        {
            //将流号，状态保存到数据库
            oraDbHelp service = new oraDbHelp();
            service.connectionString = "Data Source = 192.168.48.117/XGMES; User Id = XGMES; Password =XGMES;";
            //insert into CUT_STRANDSTATUS_DATA(CCMID,STRANDID,STRANDSTATUS)
            strandstatus = strand_Status;
            //流开浇时，将铸坯的坯头和坯尾位置置为0
            if (strand_Status == 1)
            {
                zhupiStart = 0;
                zhupiEnd = 0;
                pitouTrack = 0;
            }
            string exeSql = " insert into CUT_STRANDSTATUS_DATA(CCMID,STRANDID,STRANDSTATUS) ";
            exeSql += " values('"+CCMID+"', "+strandid+", "+ strand_Status + ") ";
            service.Update(exeSql);
        }




    }
}
