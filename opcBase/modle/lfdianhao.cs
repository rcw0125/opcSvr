using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace opcBase
{
    public  class lfdianhao
    {
        public string lfid = "1";
        //电耗变量地址
        public int val_dianhao = 1;
        /// <summary>
        /// 设置变量
        /// </summary>
        /// <param name="id">lfid</param>
        /// <param name="dianhaoid">电耗变量地址</param>
        public void setupDateConfig(string id,int dianhaoid)
        {
            lfid = id;
            val_dianhao = dianhaoid;
        }

        public Int32 caijishijian = 0;
        public void calDianhao(Int32  shijian)
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

                #region 查询电耗数据，查询当前精炼炉炉号，将电量保存到数据库
                double dianhao = getValue(val_dianhao);
                try
                {
                    string sql = " select treatno from  (  select treatno from clf_process_data  ";
                    sql += " where treatno like '" + lfid + "%' and dt < sysdate - 2/1440  order by dt desc ) where rownum = 1";
                    oraDbHelp service = new oraDbHelp();
                    DataSet ds = service.Query(sql);
                    string heatid = "";
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            heatid = item["treatno"].ToString();
                        }
                    }
                    string exeSql = " insert into ts_lf_dianliang(lfid,treatno,dh) values('" + lfid + "','" + heatid + "'," + dianhao + ") ";
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
            return Convert.ToDouble(PlcSvr.GetInstance().getVal(id));
        }
    }
}
