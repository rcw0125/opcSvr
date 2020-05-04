using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace opcBase
{
    public class OpcHelp
    {
        public static List<valRes> GetOpcVal(List<valReq> listQes)
        {
            
            List<valRes> listRes = new List<valRes>();
            if (listQes == null)
            {
                return listRes;
            }
            foreach (var item in listQes)
            {
                valRes curRes = new valRes();
                curRes.id = item.id;
                if (item.type == 0)
                {
                    curRes.val=PlcSvr.GetInstance().getVal(item.id);
                }
                else if (item.type == 1)
                {
                    curRes.val = WinccBof.GetInstance().getVal(item.id);
                }
                else if (item.type == 2)
                {
                    curRes.val = WinccBof_B.GetInstance().getVal(item.id);
                }
                else if (item.type == 3)
                {
                    curRes.val = WinccCcm.GetInstance().getVal(item.id);
                }
                else if (item.type == 4)
                {
                    curRes.val = WinccCcm_B.GetInstance().getVal(item.id);
                }
                listRes.Add(curRes);          
            }
            return listRes;
        }

        public static List<L1tag> listTag = null;
        public static List<L1tag> GetTagList()
        {
            if (listTag == null)
            {
                listTag = new List<L1tag>();
                string sql = "select id,type from  L1OPC_TAG where used=1  order by id ";
                var dt = new sqlDbHelp().Query(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        L1tag tag = new L1tag();
                        tag.id = Convert.ToInt16(item["id"]);
                        //tag.name = item["name"].ToString();
                        //tag.scanrate = Convert.ToInt16(item["scanrate"]);
                        //tag.datatype = Convert.ToInt16(item["datatype"]);
                        tag.type = Convert.ToInt16(item["type"]);
                        //tag.lasttime = DateTime.Now.AddHours(-2);
                        listTag.Add(tag);
                    }
                }
            }
            return listTag;
        }

        public static List<valRes> GetValById(int id)
        {
            List<valRes> listRes = new List<valRes>();
            valRes curRes = new valRes();
            curRes.id = id;
            List<L1tag> listtag = OpcHelp.GetTagList();
            L1tag item = listtag.Find(o => o.id == id);
            string val = "0";
            if (item == null)
            {
                val = "-9999";
            }
            else
            {
                if (item.type == 0)
                {
                    val = PlcSvr.GetInstance().getVal(id);
                }
                else if (item.type == 1)
                {
                    val = WinccBof.GetInstance().getVal(id);
                }
                else if (item.type == 2)
                {
                    val = WinccBof_B.GetInstance().getVal(id);
                }
                else if (item.type == 3)
                {
                    val = WinccCcm.GetInstance().getVal(id);
                }
                else if (item.type == 4)
                {
                    val = WinccCcm_B.GetInstance().getVal(id);
                }
                else if (item.type == 5)
                {
                    val = KepServer.GetInstance().getVal(id);
                }
            }
            curRes.val = val;
            listRes.Add(curRes);
            return listRes;
        
        }
    }
}
