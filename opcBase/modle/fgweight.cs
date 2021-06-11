using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;

namespace opcBase
{
    public  class fgweight
    {
        public string fgid = "5";
        //净重变量地址
        public int val_netweight = 89;
        //皮重变量地址
        public int val_tareweight = 90;
        /// <summary>
        /// 设置变量
        /// </summary>
        /// <param name="id">lfid</param>
        /// <param name="dianhaoid">电耗变量地址</param>
        public void setupDateConfig(string id,int netweightid,int tareweightid)
        {
            fgid = id;
            val_netweight = netweightid;
            val_tareweight = tareweightid;
        }

        public Int32 caijishijian = 0;
        public void calWeight(Int32  shijian)
        {
            if (shijian == 0)
            {
                return;
            }
            if (shijian == caijishijian)
            {
                return;
            }
            else
            {

                caijishijian = shijian;
                //暂停2秒钟
                Thread.Sleep(2000);
                #region 查询电耗数据，查询当前精炼炉炉号，将电量保存到数据库
                double netweight =Math.Round( getValue(val_netweight),2);
                double tareweight = Math.Round(getValue(val_tareweight), 2);
                try
                {               
                    oraDbHelp service = new oraDbHelp();                   
                    string exeSql = " insert into ts_fg_weight(fgid,netweight,tareweight) values('" + fgid + "'," + netweight + "," + tareweight + ") ";
                    service.Update(exeSql);
                }
                catch
                {

                }

            }
            
            

            
            #endregion
        }

        public double getValue(int id)
        {
            return Convert.ToDouble(KepServer.GetInstance().getVal(id));
        }
    }
}
