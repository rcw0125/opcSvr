using System;
using System.Collections.Generic;
using System.Text;
using opcBase;

namespace OPCWcf
{
    /// <summary>
    /// 煤气回收计算
    /// </summary>
    public class meiqihuishou
    {
        public string bofid { get; set; }
        public int hssk { get; set; }
        public int hstime { get; set; }
        public int sjhssk { get; set; }
        public int sjhstime { get; set; }
        public double co5 { get; set; }
        public double co8 { get; set; }
        public int tqcount { get; set; }
        public int tqsk { get; set; }

        //变量id
        public int  o_hangliangid { get; set; }
        public int blowtimeid { get; set; }
        public int blowflagid { get; set; }
        public int tapid { get; set; }
        public int fjid { get; set; }
        public int co5id { get; set; }
        public int co8id { get; set; }

        //
        public int tqflag { get; set; }

        public void calHssk()
        {
            //吹炼信号大于0，吹炼时间大于0，煤气中氧含量小于2，则具备回收条件

            int blowflag=getValueInt(blowflagid);       
            int blowtime = getValueInt(blowtimeid);

            
            if (blowflag > 0 && blowtime > 0 && getValue(o_hangliangid) <= 2)
            {
                hstime++; //可回收时间+1
                if (hssk == 0)
                {
                    hssk = blowtime;
                    co5 = getValue(co5id);
                    co8 = getValue(co8id);
                }
            }
            //实际回收时间计算   风机信号0
            if (blowtime > 0 && getValueInt(fjid) > 0  && getValueInt(tapid) < 1)
            {
                sjhstime++;//实际回收时间+1
                if (sjhssk == 0)
                {
                    sjhssk = blowtime;
                }
            }
            #region 提枪次数计算
            //提枪时刻，次数计算
            //
            if (hssk > 0)
            {
                if (blowflag < 1 && tqflag == 0)
                {
                    tqflag = 1;
                    tqcount++;//提枪次数+1
                    if (tqsk == 0)
                    {
                        tqsk = blowtime;
                    }
                }
            }
            if (blowflag > 0 && hssk > 0 && tqflag == 1)
            {
                tqflag = 0;
            }
            #endregion
           
            if (hssk > 0)
            {
                if (getValueInt(tapid) > 0)
                {
                    string sql = "insert into cbof_mq_data_bak(bofid,hssk,khstime,sjhssk,sjhstime,co5,co8,tqcount,tqsk) values('"+bofid+"',";
                    sql += hssk +"," +hstime+ "," +sjhssk+ "," +sjhstime+ "," +co5+ "," +co8+ "," +tqcount+ "," +tqsk+ ")";
                    oraDbHelp db = new oraDbHelp();
                    db.Update(sql);
                    clearData();  
                }
            }
        }

        public void clearData()
        {
            hssk = 0;
            hstime = 0;
            sjhssk = 0;
            sjhstime = 0;
            co5 = 0;
            co8 = 0;
            tqcount = 0;
            tqsk = 0;
            tqflag = 0;
        }

        public double getValue(int id)
        {
            return Convert.ToDouble(PlcSvr.GetInstance().getVal(id));
        }
        public int getValueInt(int id)
        {
            return Convert.ToInt16(PlcSvr.GetInstance().getVal(id));
        }
        


      
    }
}
