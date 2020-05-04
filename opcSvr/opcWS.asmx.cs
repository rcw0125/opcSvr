using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using Rcw.Method;

namespace opcSvr
{
    /// <summary>
    /// opcWS 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class opcWS : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public List<valRes> GetOpcVal(List<valReq> listreq)
        {
            //List<valReq> listQes
            //List<valReq> listreq = new List<valReq>();
            //valReq curreq = new valReq();
            //curreq.id = 5;
            //curreq.type = 0;
            //listreq.Add(curreq);
            //valReq curreq2 = new valReq();
            //curreq2.id = 4;
            //curreq2.type = 0;
            //listreq.Add(curreq2);

            string Str = JsonConvert.SerializeObject(listreq);
            String url = "http://192.168.48.232:1111/OpcService/getval";
            List<valRes> ja2 = WebOper.HttpPost(url, Str, typeof(List<valRes>)) as List<valRes>;
            

            return ja2;
            //return "Hello World";
        }
    }
}
