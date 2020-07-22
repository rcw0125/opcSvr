using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opcBase
{
    public  class ccmCutStrand
    {
        public string CCMID = "S63";
        public int strandid { get; set; }
        
        //切割状态
        public int cutstatus  { get; set; }
        public double billetLength { get; set; }
        //跟踪值
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
        public void calDbjf(int status)
        {
            double trackvalue = getValue(valid_trackvalue);
        }
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
            oraDbHelp service = new oraDbHelp();
            service.connectionString = "Data Source = 192.168.48.117/XGMES; User Id = XGMES; Password =XGMES;";
            string exeSql = " insert into CUT_STRAND_DATA(CCMID, STRANDID, STATUS, BILLETLENGTH, TRACKVALUE) ";
            exeSql += " values('"+CCMID+"', "+strandid+", "+cutStatus+", "+billet_length+", "+track_value+") ";
            service.Update(exeSql);
        }

        public double getSpeed()
        {
            return getValue(valid_speed);
        }
        public double getTrack()
        {
            return getValue(valid_trackvalue);
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
            

            string exeSql = " insert into CUT_STRANDSTATUS_DATA(CCMID,STRANDID,STRANDSTATUS) ";
            exeSql += " values('"+CCMID+"', "+strandid+", "+ strand_Status + ") ";
            service.Update(exeSql);
        }




    }
}
