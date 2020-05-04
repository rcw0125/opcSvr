using System;
using System.Collections.Generic;
using System.Text;

namespace opcBase
{
    public class L1tag
    {
        public int id { get; set; }
        public string name { get; set; }
        /// <summary>
        /// 服务端句柄
        /// </summary>
        public int itmHandleServer { get; set; }
        /// <summary>
        /// 扫描周期
        /// </summary>
        public int scanrate { get; set; }
        /// <summary>
        /// 数据类型：0-整数；1-小数；2-字符串
        /// </summary>
        public int datatype { get; set; }
        /// <summary>
        /// 服务类型：0-netopc；1-135；2-48135；3-125；4-48.125
        /// </summary>
        public int type { get; set; }

        public DateTime lasttime { get; set; }
    }
}
