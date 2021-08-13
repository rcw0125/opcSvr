using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace layui
{
 
    /// <summary>
    /// MESHandler 的摘要说明
    /// </summary>
    public class MESHandler : IHttpHandler
    {
        Entities mes = new Entities();
        static int i = 0;
        public void ProcessRequest(HttpContext context)
        {
            string EditType = context.Request["Type"];
            if (EditType == "mes")
            {
                //SingleCase.GetInstance().times++;
                i++;
               // mes.Database.ExecuteSqlCommand("insert into TS_RESET_LOG( c_ip, c_type) values('MES系统重启', '')");
                DbMySql.ExeSql("insert into TS_RESET_LOG( ip, type,ts) values('MES系统重启', '','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
                context.Response.ContentType = "text/plain";
                //context.Response.Write(SingleCase.GetInstance().times);
                context.Response.Write(i);
            }
            else if (EditType!=null&& EditType.StartsWith("1"))
            {
                //将计算机IP地址写入数据库
                // mes.Database.ExecuteSqlCommand("insert into TS_RESET_LOG( c_ip, c_type) values('"+EditType+"', '关闭开始')");
                DbMySql.ExeSql("insert into TS_RESET_LOG( ip, type,ts) values('" + EditType + "', '关闭开始','"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");
            }
            else
            {
                context.Response.ContentType = "text/plain";
                //context.Response.Write(SingleCase.GetInstance().times);
                context.Response.Write(i);
            }
                
        }
        
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}