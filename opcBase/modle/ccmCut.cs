using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opcBase
{
    public class ccmCut
    {
        public  ccmCutStrand ccmCutStrand_1 = new ccmCutStrand();
        public  ccmCutStrand ccmCutStrand_2 = new ccmCutStrand();
        public  ccmCutStrand ccmCutStrand_3 = new ccmCutStrand();
        public  ccmCutStrand ccmCutStrand_4 = new ccmCutStrand();
        public string CCMID = "S63";

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

        public void setupDateConfig(string ccmid,int startweizhi)
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

            ccmCutStrand_2.valid_cutstatus = startweizhi + 10;
            ccmCutStrand_2.valid_strandstatus = startweizhi + 11;
            ccmCutStrand_2.valid_billetLength = startweizhi + 12;
            ccmCutStrand_2.valid_trackvalue = startweizhi + 13;
            ccmCutStrand_2.valid_speed = startweizhi + 14;
            ccmCutStrand_2.valid_track = startweizhi + 15;

            ccmCutStrand_3.valid_cutstatus = startweizhi + 16;
            ccmCutStrand_3.valid_strandstatus = startweizhi + 17;
            ccmCutStrand_3.valid_billetLength = startweizhi + 18;
            ccmCutStrand_3.valid_trackvalue = startweizhi + 19;
            ccmCutStrand_3.valid_speed = startweizhi + 20;
            ccmCutStrand_3.valid_track = startweizhi + 21;

            ccmCutStrand_4.valid_cutstatus = startweizhi + 22;
            ccmCutStrand_4.valid_strandstatus = startweizhi + 23;
            ccmCutStrand_4.valid_billetLength = startweizhi + 24;
            ccmCutStrand_4.valid_trackvalue = startweizhi + 25;
            ccmCutStrand_4.valid_speed = startweizhi + 26;
            ccmCutStrand_4.valid_track = startweizhi + 27;
        }
        /// <summary>
        /// 获取各流速度和跟踪值（每1s接受一个数据）
        /// </summary>
        public void getSpeedAndTrack()
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
                oraDbHelp service = new oraDbHelp();
                service.connectionString = "Data Source = 192.168.48.117/XGMES; User Id = XGMES; Password =XGMES;";        
                string exeSql = " insert into CUT_Speed_DATA(CCMID,SPEED_1ST,TRACK_1ST,SPEED_2ST,TRACK_2ST,SPEED_3ST,TRACK_3ST,SPEED_4ST,TRACK_4ST) ";
                exeSql += " values('" + CCMID + "', " + L1S + ", " + L1T + ", " + L2S + ", " + L2T + ", " + L3S + ", " + L3T + ", " + L4S + ", " + L4T + ") ";
                service.Update(exeSql);
            }
            
        }
        /// <summary>
        /// 连铸机到达开浇停浇状态
        /// </summary>
        /// <param name="status"></param>
        public void acceptCasterStatus(int status)
        {
            //保存到数据库
            oraDbHelp service = new oraDbHelp();
            service.connectionString = "Data Source = 192.168.48.117/XGMES; User Id = XGMES; Password =XGMES;";
            string exeSql = " insert into CUT_CCMSTATUS_DATA(CCMID,CCMSTATUS) values('"+CCMID+"', "+status+") ";
            service.Update(exeSql);
        }

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
            exeSql += " values('"+CCMID+"',"+status+","+L1T+","+L2T+","+L3T+","+L4T+") ";
            service.Update(exeSql);
        }
    }
}
