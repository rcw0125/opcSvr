using System;
using System.Collections.Generic;
using System.Text;

namespace opc
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

        public DateTime lasttime { get; set; }
    }
}
