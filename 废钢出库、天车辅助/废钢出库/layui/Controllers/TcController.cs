using layui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace layui.Controllers
{
    [RoutePrefix("api/tc")]
    public class TcController : ApiController
    {
        XGDADBEntities db = new XGDADBEntities();
        Entities mes = new Entities();
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        /// <summary>
        /// 获取重量
        /// </summary>
        /// <returns></returns>

        [Route("getCcmWeight")]
        [HttpGet]
        
        public List<CCM_WEIGHT> getCcmWeight()
        {
            List<CCM_WEIGHT> listWeight = new List<CCM_WEIGHT>();
            try
            {
                var ccm3 = db.CCM_Weight_His.Where(o => o.CCMID == 3).OrderByDescending(o => o.MeasureTime).FirstOrDefault();
                var m3 = new CCM_WEIGHT();
                m3.ccmid = "3";
                m3.weight = Math.Round((double)ccm3.LadleWeight, 1).ToString();
                listWeight.Add(m3);

                var ccm4 = db.CCM_Weight_His.Where(o => o.CCMID == 4).OrderByDescending(o => o.MeasureTime).FirstOrDefault();
                var m4 = new CCM_WEIGHT();
                m4.ccmid = "4";
                m4.weight = Math.Round((double)ccm4.LadleWeight, 1).ToString();
                listWeight.Add(m4);
            }
            catch
            {

            }
           

            return listWeight;
          
        }

        /// <summary>
        /// 获取精炼炉工位计划
        /// </summary>
        /// <returns></returns>
        [Route("getLfPlan")]
        [HttpGet]
        public List<lfplan> getLfPlan()
        {
            List<lfplan> listplan = new List<lfplan>();
           
            try
            {
                string sql = "select a.code,case b.casterid when 'S64' then '4#机' when 'S63' then '3#机' when 'S66' then '6#机' when 'S67' then '7#机'  ";
                sql += " else b.casterid end as caster from CLF_PROD_AREA a,cplan_tapping b  ";
                sql += " where (a.code='S41A' or a.code='S41B' or a.code='S42A' or a.code='S42B')  ";
                sql += " and a.status >0 and a.heatid=b.heatid  ";
                listplan = mes.Database.SqlQuery<lfplan>(sql).ToList();
            }
            catch
            {

            }

            return listplan;

        }

        /// <summary>
        /// 获取连铸机计划
        /// </summary>
        /// <returns></returns>
        [Route("getCcmPlan")]
        [HttpGet]
        public List<ccmplan> getCcmPlan()
        {

            List<ccmplan> listplan = new List<ccmplan>();
            try
            {
                string sql = " select case a.position when 'S63P01' then '3#机' when 'S64P01' then '4#机' else a.position end    ";
                sql += " as position ,case b.lfid when 'S41' then '1#LF' when 'S42' then '2#LF' else b.lfid end as lfid,  ";
                sql += " case b.bofid when 'S21' then '1#炉' when 'S22' then '2#炉' when 'S23' then '3#炉' when 'S24' then '4#炉'   ";
                sql += " else b.bofid end as bofid  from materialposition a ,cplan_tapping b   ";
                sql += " where(a.position = 'S63P01' or a.position = 'S64P01') and a.materialid = b.heatid  ";
                listplan = mes.Database.SqlQuery<ccmplan>(sql).ToList();
            }
            catch
            {

            }
            
            return listplan;

        }
        /// <summary>
        /// 获取连铸机的预测工位（预测在精炼炉还是转炉）
        /// </summary>
        /// <param name="ccmid"></param>
        /// <returns></returns>
        [Route("getCcmyc")]
        [HttpGet]
        public List<ccmyc> getCcmyc(string ccmid)
        {
            List<ccmyc> listplan = new List<ccmyc>();

            try
            {
                string sql = " select substr(a.code,3,1)||'号精炼出站'  as name from CLF_PROD_AREA a,cplan_tapping b  where(a.code like 'S41%' or a.code like 'S42%')    ";
                sql += " and a.STATUS >= 4 and a.heatid = b.heatid and b.casterid = '" + ccmid + "' ";
                listplan = mes.Database.SqlQuery<ccmyc>(sql).ToList();
                if (listplan == null || listplan.Count == 0)
                {
                    sql = " select substr(bofid,3,1)||'号转炉出钢' as name from cplan_tapping where status = 12 and casterid = '" + ccmid + "'    ";
                    sql += " and lfid = '0' and bof_status >= '04' ";
                    listplan = mes.Database.SqlQuery<ccmyc>(sql).ToList();
                }

                if (listplan == null || listplan.Count == 0)
                {
                    ccmyc yc = new ccmyc();
                    yc.name = "无";
                    listplan.Add(yc);

                }
            }
            catch
            {

            }
           
            return listplan;

        }

        [Route("getCcmtime")]
        [HttpGet]
        public List<ccmtime> getCcmtime()
        {
            List<ccmtime> listccm = new List<ccmtime>();
            try
            {
                //remaintime remaintime
                string sql = " select to_char(remaincastingtime) as remaintime from cccm_unit_mag where code='S63' or code='S64' order by code    ";
                listccm = mes.Database.SqlQuery<ccmtime>(sql).ToList();
            }
            catch(Exception ex)
            {
                string e = ex.ToString();
            }
               
            return listccm;

        }

        [Route("getScjz")]
        [HttpGet]
        public List<scjz> getScjz()
        {
            List<scjz> listccm = new List<scjz>();
            try
            {               
                string sql = "  select a.heatid,a.steelgrade,a.casterid,a.bofid,a.lfid,a.rhid,a.AIM_TIME_IRONPREPARED,a.AIM_TIME_BOFSTART,a.AIM_TIME_BOFTAPPED,a.AIM_TIME_TAPPEDSIDEEND,    ";
                sql += "  a.AIM_TIME_LFARRIVAL,a.AIM_TIME_LFSTART,a.AIM_TIME_LFEND,a.AIM_TIME_LFLEAVE,a.AIM_TIME_RHARRIVAL,    ";
                sql += "  a.AIM_TIME_RHSTART,a.AIM_TIME_RHEND,a.AIM_TIME_RHLEAVE,a.AIM_TIME_CASTERARRIVAL,a.AIM_TIME_CASTINGSTART,a.AIM_TIME_CASTINGEND ,    ";
                sql += "  b.ACT_TIME_IRONPREPARED,b.ACT_TIME_BOFSTART,b.ACT_TIME_BOFTAPPED,    ";
                sql += "  b.ACT_TIME_TAPPEDSIDEEND,b.ACT_TIME_LFARRIVAL,b.ACT_TIME_LFSTART,b.ACT_TIME_LFEND,b.ACT_TIME_LFLEAVE,  ";
                sql += "  b.ACT_TIME_RHARRIVAL,b.ACT_TIME_RHSTART,b.ACT_TIME_RHEND, b.ACT_TIME_RHLEAVE,    ";
                sql += "  b.ACT_TIME_CASTERARRIVAL, b.ACT_TIME_CASTINGSTART,b.ACT_TIME_CASTINGEND from cplan_tapping a, cplan_tapping_act b    ";
                sql += "  where(a.status= 11 or a.status= 12) and a.heatid = b.heatid(+)    ";
                listccm = mes.Database.SqlQuery<scjz>(sql).ToList();
            }
            catch (Exception ex)
            {
                string e = ex.ToString();
            }
            return listccm;
        }

        public List<funcInfo> listFunc = null;
        [Route("getIpList")]
        [HttpGet]
        public List<funcInfo> getIpList()
        {
         
            try
            {
                if (listFunc == null)
                {
                    XMLConfig config = XMLManager.LoadXML();               
                    listFunc = config.funcInfoList;
                }
            }
            catch (Exception ex)
            {
                string e = ex.ToString();
            }
            return listFunc;
        }



    }
}