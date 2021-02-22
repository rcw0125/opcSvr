using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using layui.menu;
using layui.Models;
//using Rcw.Data;

namespace layui.Controllers
{
    public class HomeController : Controller
    {
        VehIcEntities db = new VehIcEntities();
        Entities mes = new Entities();
        public ActionResult Index()
        {
            return RedirectToAction("scrap");
        }

        public ActionResult About()
        {
            ViewBag.Message = "layui前端框架学习.";

            return View();
        }

        public ActionResult scrap()
        {
            if (this.HttpContext.Session["curUser"] == null)
            {
                return RedirectToAction("login");
            }

            ViewBag.Message = "废钢出库";
            ViewBag.userid = HttpContext.Session["curUser"];

            return View();
        }

        public ActionResult ladle()
        {
            if (this.HttpContext.Session["curUser"] == null)
            {
                return RedirectToAction("login");
            }

            ViewBag.Message = "钢包指定查询";
            ViewBag.userid = HttpContext.Session["curUser"];

            return View();
        }
        public ActionResult test()
        {
            ViewBag.Message = "测试";

            return View();
        }

        public ActionResult login(string userid,string password)
        {
            if (string.IsNullOrEmpty(userid) && string.IsNullOrEmpty(password))
            {
                ViewBag.Message = "";
                return View();
            }

            if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(password))
            {
                ViewBag.Message = "用户名或密码不能为空！";
                return View();
            }
            else
            {
                //var list = mes.TS_USER.ToList();

                var curuser = mes.TS_USER.Where(o => o.USERID == userid).FirstOrDefault();
                if (curuser == null)
                {
                    ViewBag.Message = "用户名不存在";
                    return View();
                }
                if (password != curuser.PASSWORD)
                {
                    ViewBag.Message = "密码不正确";
                    return View();
                }
                this.HttpContext.Session["curUser"] = userid;
                return RedirectToAction("scrap");

            }
           
        }

        public ActionResult yccx(string BeginTime,string EndTime,string fenlei)
        {
            ViewBag.Message = "layui前端框架学习.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult layuitest()
        {
            ViewBag.Message = "layui前端框架学习";

            return View();
        }

        public ActionResult layuitable()
        {
            ViewBag.Message = "layui前端table学习";

            return View();
        }


        public JsonResult getyccxtable(int page, int limit, string begintime, string endtime, string fenlei)
        {
            var bt = DateTime.Parse(begintime);
            var et = DateTime.Parse(endtime);
            var yccx = db.QC_YCCX.Where(o => o.RIQI > bt&&o.RIQI<et);
            if (fenlei == "1")
            {
                yccx = yccx.Where(o => o.leixing == "煤" || o.leixing == "焦炭");
            }
            else if (fenlei == "2")
            {
                yccx = yccx.Where(o => o.leixing == "精粉" || o.leixing == "外矿");
            }
            else if (fenlei == "3")
            {
                yccx = yccx.Where(o => o.leixing == "熔剂" || o.leixing == "石子");
            }
            else if (fenlei == "4")
            {
                yccx = yccx.Where(o => o.leixing == "合金");
            }
            //var yccx = db.QC_YCCX.Where(o => o.leixing == fenlei).OrderBy(o=>o.SAMPLE_YCCX_ID);
            var count = yccx.Count();
            var data = yccx.OrderBy(o => o.SAMPLE_YCCX_ID).Skip((page-1)*limit).Take(limit).ToList();
            foreach (var item in data)
            {

                item.rq = Convert.ToDateTime(item.RIQI).ToString("yyyy-MM-dd hh:mm:ss");
            }

            return Json(new { count = count, data = data, code = 0, msg = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ckjl(int page, int limit)
        {
            var sql = "select to_char(logdate,'hh24:MI') as logtime,operator as userid,scrap_slot_id as slot,heatid,( select sum(amount) from CSCRAPOUTSTOREDETAIL where scrap_slot_id=CSCRAP_OUTPUT_LOG.scrap_slot_id) as amount from CSCRAP_OUTPUT_LOG where logdate >sysdate-1 and materialid ='Scrap' order by logdate desc  ";     
            var count =Convert.ToInt32( mes.Database.SqlQuery<scraptout>(sql).Count());
            var list = mes.Database.SqlQuery<scraptout>(sql).Skip((page-1)*limit).Take(limit).ToList();
            //var list = mes.Database.SqlQuery<scraptout>(sql).ToList();
            return Json(new { count = 25, data = list, code = 0, msg = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Gettable(int page,int limit,string begintime,string endtime,string fenlei)
        {
            List<user> listUser = new List<user>();
            for (int i = 1; i <= 1000; i++)
            {
                user usedata = new user();
                usedata.id = "ID" + i;
                usedata.username = i % 2 == 0 ? "春暖花开" : "雪山飞狐";
                usedata.sex= i%2==0?"男":"女";
                usedata.city = i % 2 == 0 ? "大城市" : "小城市";
                listUser.Add(usedata);
            }
            var count = listUser.Count;
            var data = listUser.Skip(page).Take(limit).ToList();

            return Json(new { count=count,data=data,code=0,msg=""},JsonRequestBehavior.AllowGet);
        }


        public JsonResult Gettree()
        {
            List<tree> listtree = new List<tree>();

            tree tree = new tree();
            tree.id = "001";
            tree.title = "河北省";
            tree.checkArr = "0";
            tree.parentId = "0";

            tree tree1 = new tree();
            tree1.id = "002";
            tree1.title = "河南省";
            tree1.checkArr = "0";
            tree1.parentId = "0";

            tree tree2 = new tree();
            tree2.id = "003";
            tree2.title = "邢台市";
            tree2.checkArr = "0";
            tree2.parentId = "001";


            listtree.Add(tree);
            listtree.Add(tree1);
            listtree.Add(tree2);



            return Json(new { data=listtree,status=new { code="200",message="操作成功" } }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Getladleid()
        {
            List<ladlebase> listladle = new List<ladlebase>();
            ladlebase lb = new ladlebase();
            lb.laldeid = "G01";
            listladle.Add(lb);
            ladlebase lb2 = new ladlebase();
            lb2.laldeid = "G02";
            listladle.Add(lb2);

            return Json(new { data =listladle }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Gettest(string userid,string slot,string fg,string weight,string fg1,string weight1,string st,string stweight)
        {
            if (slot.Trim() == "")
            {
                return Json(new { message = "废钢斗号不能为空", flag = "100" }, JsonRequestBehavior.AllowGet);
            }
        
            double w1;
            #region 对第一种废钢进行数据校验
            if (fg == "fg0")
            {
                return Json(new { message = "请选择废钢类型", flag = "100" }, JsonRequestBehavior.AllowGet);
            }
            if (!double.TryParse(weight, out w1))
            {
                return Json(new { message = "废钢重量" + weight + "不是有效的数字", flag = "100" }, JsonRequestBehavior.AllowGet);
            }
            if (weight=="0")
            {
                //输入0.0 可能是个漏洞
                return Json(new { message = "废钢重量不能为零", flag = "100" }, JsonRequestBehavior.AllowGet);
            }
            #endregion
            double w2;
            bool secFlag = false;
            #region 对第二种废钢进行数据校验
            if (!double.TryParse(weight1, out w2))
            {
                return Json(new { message = "第二种废钢重量" + weight1 + "不是有效的数字", flag = "100" }, JsonRequestBehavior.AllowGet);
            }       
            if (w2>0 && fg1=="fg0")
            {
                return Json(new { message = "第二种废钢重量不为0，请选择类型", flag = "100" }, JsonRequestBehavior.AllowGet);
            }
            //选择了废钢且重量大于0 则 第二种标志 为true
            if (w2 > 0 && fg1 != "fg0")
            {
                secFlag = true;
            }

            #endregion

            double w3;
            bool stFlag = false;
            #region  对生铁数据进行校验

            if (!double.TryParse(stweight, out w3))
            {
                return Json(new { message = "生铁重量" + weight1 + "不是有效的数字", flag = "100" }, JsonRequestBehavior.AllowGet);
            }
            if (w3 > 0 && st == "st0")
            {
                return Json(new { message = "生铁重量不为0，请选择类型", flag = "100" }, JsonRequestBehavior.AllowGet);
            }
            //选择了生铁且重量大于0 则 生铁标志 为true
            if (w3 > 0 && st != "st0")
            {
                stFlag = true;
            }

            #endregion;


            string shift = "", team = "";

            try
            {
                var curban = mes.CCURRENTSHIFTINFO.FirstOrDefault();
                if (curban == null)
                {
                    return Json(new { message = "操作失败！没有查询到班次信息", flag = "100" }, JsonRequestBehavior.AllowGet);
                }
                shift = curban.SHIFTID.ToString();
                team = curban.TEAMID.ToString();

            }
            catch (Exception ex)
            {
                return Json(new { message = "操作失败！"+ex.ToString(), flag = "100" }, JsonRequestBehavior.AllowGet);
            }



            //开始事务
           var trans= mes.Database.BeginTransaction();
            try
            {
                string prix = "";
                if (userid == "1004")
                {
                    prix = "4-";
                }
                string slotid = prix+ slot + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");

                List<CSCRAP_OUTPUT_LOG> listScrap = new List<CSCRAP_OUTPUT_LOG>();
                //添加废钢
                CSCRAP_OUTPUT_LOG scrap = new CSCRAP_OUTPUT_LOG();
                scrap.GUID = System.Guid.NewGuid().ToString("B").ToUpper();
                scrap.LOGDATE = DateTime.Now;
                scrap.SCRAP_SLOT_ID = slotid;
                scrap.MATERIALTYPE = "CScrap_Data";
                scrap.MATERIALID = "Scrap";
                scrap.STOREAREAID = "S82A01";
                //废钢重量等于两种相加
                scrap.AMOUNT = (decimal)(w1+w2);
                scrap.TYPE = 0;
                scrap.OPERATOR = userid;
                scrap.NET_WEIGHT = 0;
                scrap.GROSS_WEIGHT = 0;
                scrap.TARE_WEIGHT = 0;
                scrap.IRON_FLAG = 0;
                scrap.SHIFT = shift;
                scrap.TEAM = team;

                listScrap.Add(scrap);
                mes.CSCRAP_OUTPUT_LOG.Add(scrap);

                //添加铁块
                CSCRAP_OUTPUT_LOG pigiron = new CSCRAP_OUTPUT_LOG();
                pigiron.GUID = System.Guid.NewGuid().ToString("B").ToUpper();
                pigiron.LOGDATE = DateTime.Now;
                pigiron.SCRAP_SLOT_ID = slotid;
                pigiron.MATERIALTYPE = "CScrap_Data";
                pigiron.MATERIALID = "PigIron";
                pigiron.STOREAREAID = "S82A02";
                pigiron.AMOUNT = (decimal)w3;
                pigiron.OPERATOR = userid;
                pigiron.TYPE = 0;
                pigiron.NET_WEIGHT = 0;
                pigiron.GROSS_WEIGHT = 0;
                pigiron.TARE_WEIGHT = 0;
                pigiron.IRON_FLAG = 0;
                pigiron.SHIFT = shift;
                pigiron.TEAM = team;
                listScrap.Add(pigiron);
                mes.CSCRAP_OUTPUT_LOG.Add(pigiron);


                //添加废钢明细           
                CSCRAPOUTSTOREDETAIL sd = new CSCRAPOUTSTOREDETAIL();
                sd.GUID = System.Guid.NewGuid().ToString("B").ToUpper();
                sd.NAME = "Unknown";
                sd.SCRAP_SLOT_ID = slotid;
                sd.MATERIAL = "ScrapGroupName";
                sd.MATERIAL_CODE = fg;
                sd.AMOUNT = w1.ToString();           
                mes.CSCRAPOUTSTOREDETAIL.Add(sd);

                if (secFlag)
                {
                    CSCRAPOUTSTOREDETAIL sd2 = new CSCRAPOUTSTOREDETAIL();
                    sd2.GUID = System.Guid.NewGuid().ToString("B").ToUpper();
                    sd2.NAME = "Unknown";
                    sd2.SCRAP_SLOT_ID = slotid;
                    sd2.MATERIAL = "ScrapGroupName";
                    sd2.MATERIAL_CODE = fg1;
                    sd2.AMOUNT = w2.ToString();
                    mes.CSCRAPOUTSTOREDETAIL.Add(sd2);
                }

                if (stFlag)
                {
                    CSCRAPOUTSTOREDETAIL std = new CSCRAPOUTSTOREDETAIL();
                    std.GUID = System.Guid.NewGuid().ToString("B").ToUpper();
                    std.NAME = "Unknown";
                    std.SCRAP_SLOT_ID = slotid;
                    std.MATERIAL = "PigIronGroupName";
                    std.MATERIAL_CODE = st;
                    std.AMOUNT = w3.ToString();
                    mes.CSCRAPOUTSTOREDETAIL.Add(std);

                }

                //mes.CSCRAP_OUTPUT_LOG.AddRange(listScrap);
                //mes.CSCRAPOUTSTOREDETAIL.AddRange(listsd);
                mes.SaveChanges();
                               
                trans.Commit();
            
            }
            catch (Exception ex)
            {
                trans.Rollback();          
                return Json(new { message = "操作失败！" + ex.ToString(), flag = "100" }, JsonRequestBehavior.AllowGet);
            }






            return Json(new { flag = "200", message = "操作成功"  }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult layuiAdmin()
        {
            ViewBag.Message = "layui后台框架学习";
            ViewBag.menu = "[{\"OperationType\":0,\"Title\":null,\"icon\":\"icon-comments\",\"id\":\"83CFD869E4E14E0D8F60BB051B4E8933\",\"menus\":[{\"OperationType\":0,\"Title\":null,\"icon\":\"icon-glass\",\"id\":\"F606ECCD8B974940AD6539829651E5A2\",\"menus\":[],\"text\":\"在线咨询\",\"url\":\"\\/Menu\\/MenuManage\"},{\"OperationType\":0,\"Title\":null,\"icon\":\"icon-glass\",\"id\":\"EBA1B1A192B946E8B6218DD78F40177D\",\"menus\":[],\"text\":\"技术咨询\",\"url\":null},{\"OperationType\":0,\"Title\":null,\"icon\":\"icon-glass\",\"id\":\"B8C73F971DDD46D6B9AA85A4F3DB0A9F\",\"menus\":[],\"text\":\"用户使用报告\",\"url\":null},{\"OperationType\":0,\"Title\":null,\"icon\":\"icon-glass\",\"id\":\"8809F159EBA44321914394C2C4771DC8\",\"menus\":[],\"text\":\"满意度问卷调查\",\"url\":null},{\"OperationType\":0,\"Title\":null,\"icon\":\"icon-glass\",\"id\":\"87570F68FBD741EB93FEB0A011603469\",\"menus\":[],\"text\":\"质量异议反馈\",\"url\":null}],\"text\":\"在线服务\",\"url\":\"\\/\"}]";

            //ViewBag.test = leftMenu.getMenu(leftMenu.Load());
            ViewBag.test = leftMenu.getMenuNew(leftMenu.Load());
            return View();
        }
    }
}