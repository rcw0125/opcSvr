using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opcSvr
{
    public class valRes
    {
        /// <summary>
        /// 变量在数据库表中的id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 获取到的变量的值，返回给请求的客户端
        /// </summary>
        public string val { get; set; }
    }
}
