using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Rcw.Method;

namespace opcSvr
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            List<valReq> listreq = new List<valReq>();
            valReq curreq = new valReq();
            curreq.id = 5;
            curreq.type = 0;
            listreq.Add(curreq);
            valReq curreq2 = new valReq();
            curreq2.id = 4;
            curreq2.type = 0;
            listreq.Add(curreq2);

            string Str = JsonConvert.SerializeObject(listreq);
            String url = "http://192.168.48.232:1111/OpcService/getval";
            List<valRes> listres = WebOper.HttpPost(url, Str, typeof(List<valRes>)) as List<valRes>;
            Label1.Text = listres[0].id + "-" + listres[0].val;        
        }
    }
}
