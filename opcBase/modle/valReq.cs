using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opcBase
{
   /// <summary>
   /// 变量数据格式（客户端向服务器请求的数据格式，需要有变量的id和变量的服务器类型）
   /// </summary>
    public class valReq
    {
        /// <summary>
        /// 变量在数据库表中的id
        /// </summary>
        public int id { get; set; }
        // public enum svrType
        //{
        //    NET服务=0,
        //    不锈钢=2,
        //    连铸服务 = 3,
        //    连铸不锈钢 = 4,
        //    冶炼服务=1               
        //}
        /// <summary>
        /// 变量类型（决定链接不同的opc服务）
        /// NET服务=0,冶炼服务=1，不锈钢=2,连铸服务 = 3,连铸不锈钢 = 4,   
        /// </summary>
        public int type { get; set; }
       
    }
}
