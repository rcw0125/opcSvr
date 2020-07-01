using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace L2Ctr
{
    public class remark
    {
        [DisplayName("序号")]
        public string id { get; set; }
        [DisplayName("描述")]
        public string des { get; set; }
        [DisplayName("时间")]
        public string time { get; set; }
    }
}
