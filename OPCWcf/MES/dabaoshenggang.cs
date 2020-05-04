using opcBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace OPCWcf
{
    public class dabaoshenggang
    {
        public string status { get; set; }
        public int valid_status { get; set; }
        public double tundishweight { get; set; }
        public int valid_tundishweight { get; set; }
        public double ladleweight { get; set; }
        public int valid_ladelweight { get; set; }
        public string dabaobi { get; set; }
        public double grossweight { get; set; }
        public int valid_A_weight { get; set; }
        public int valid_B_weight { get; set; }

        public double genzongzhi1 { get; set; }
        public int valid_genzongzhi1 { get; set; }
        public double genzongzhi2 { get; set; }
        public int valid_genzongzhi2 { get; set; }

        public double genzongzhi3 { get; set; }
        public int valid_genzongzhi3 { get; set; }
        public double genzongzhi4 { get; set; }
        public int valid_genzongzhi4 { get; set; }

        public int flag = 0;

        public DateTime casting_begin { get; set; }

       
        public void calData()
        {

            #region  大包开浇时，记录各个值
            if (getValueInt(valid_status) == 1 && flag == 0)
            {
                flag = 1;
                tundishweight = getValue(valid_tundishweight);
                var A_weight = getValue(valid_A_weight);
                var B_weight = getValue(valid_B_weight);
                if (A_weight > B_weight)
                {
                    dabaobi = "A";
                    grossweight = A_weight;
                }
                else
                {
                    dabaobi = "B";
                    grossweight = B_weight;
                }

                genzongzhi1 = getValue(valid_genzongzhi1);
                genzongzhi2 = getValue(valid_genzongzhi2);
                genzongzhi3 = getValue(valid_genzongzhi3);
                genzongzhi4 = getValue(valid_genzongzhi4);
                casting_begin = DateTime.Now;
                status = "浇注中";


               
            }
            #endregion

            #region 大包停浇时，计算各个值，并保存到数据库
            if (getValueInt(valid_status) == 0 && flag == 1)
            {
                flag = 0;
                //中包浇注重量
                var tundishweight_end = getValue(valid_tundishweight);
                var tundishweight_add = tundishweight_end - tundishweight;
               
                //大包壁称量重量
                var tare_weight = 0.0;
                if (dabaobi == "A")
                {
                    tare_weight = getValue(valid_A_weight);
                }
                else
                {
                    tare_weight = getValue(valid_B_weight);
                }
                var remainweight= getValue(valid_ladelweight);

                //大包浇注时长
                var casting_end = DateTime.Now;
                var castingtime =Math.Round( (casting_end-casting_begin).TotalMinutes,1);
                //大包铸坯长度等
                var genzongzhi1_end = getValue(valid_genzongzhi1);
                var genzongzhi2_end = getValue(valid_genzongzhi2);
                var genzongzhi3_end = getValue(valid_genzongzhi3);
                var genzongzhi4_end = getValue(valid_genzongzhi4);
                var zhupi_length = Math.Round(genzongzhi1_end - genzongzhi1 + genzongzhi2_end - genzongzhi2 + genzongzhi3_end - genzongzhi3 + genzongzhi4_end - genzongzhi4, 2);
                string heatid = "";
                double danzhong = 0;
                string sql = " select heatid,(select round(weight * 1000 / length, 3) from CQA_CAL_WEIGHT_STD  where spec = '280*325' and steelgrade = cccm_base_data.steelgrade and length = cccm_base_data.length and rownum = 1) as danzhong ";
                sql += " from cccm_base_data where heatid = (select max(heatid) from cccm_base_data where ccmid = 'S65') ";
                oraDbHelp service = new oraDbHelp();
                DataSet ds = service.Query(sql);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        heatid = item["heatid"].ToString();
                        try
                        {
                            danzhong = Convert.ToDouble(item["danzhong"]);
                        }
                        catch
                        {
                            danzhong = 0.72;
                        }                      
                    }
                }
                var zhupi_weight = zhupi_length * danzhong;

                string exeSql = " insert into cccm_ladle_weight(heatid, casting_weight, tundish_weight, zhupi_weight, zhupi_length, zhupi_danzhong, dabaobi, grossweight, tareweight, ";
                exeSql += " castingtime, casting_begin, casting_end, tundish_begin, tundish_end, zhupi1_begin, zhupi1_end, zhupi2_begin, zhupi2_end, ";
                exeSql += " zhupi3_begin, zhupi3_end, zhupi4_begin, zhupi4_end,remainweight) ";
                exeSql += " values('"+heatid+"', "+ (tundishweight_add+ zhupi_weight) + ", "+ tundishweight_add + ", "+ zhupi_weight + ", "+ zhupi_length + ", "+danzhong+", '"+dabaobi;
                exeSql += "'," + grossweight + ", " + tare_weight + "," + castingtime + ", '" + casting_begin.ToString("HH:mm:ss") + "', '" + casting_end.ToString("HH:mm:ss");
                exeSql += "',"+ tundishweight + ", "+ tundishweight_end + ", "+ genzongzhi1 + ", "+ genzongzhi1_end + ", "+ genzongzhi2 + ", "+ genzongzhi2_end + ", "+ genzongzhi3 + ","+ genzongzhi3_end + ", "+ genzongzhi4 + ", "+ genzongzhi4_end + ", " + remainweight + ") ";
                service.Update(exeSql);
                status = "已停浇";
            }
            #endregion

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
