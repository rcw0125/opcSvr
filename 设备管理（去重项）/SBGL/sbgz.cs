using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SBGL
{
    /// <summary>
    /// 设备故障报修
    /// </summary>
    public class sbgz
    {
        //djtime,djrdw,jsrdw,clrdw,yzrdw,datediff(jstime, djtime) as sjc
        /// <summary>
        /// 登记时间
        /// </summary>
        public DateTime djtime { get; set; }
        /// <summary>
        /// 登记人
        /// </summary>
        public string djrdw { get; set; }
        /// <summary>
        /// 接收人
        /// </summary>
        public string jsrdw { get; set; }
        /// <summary>
        /// 处理人
        /// </summary>
        public string clrdw { get; set; }
        /// <summary>
        /// 验证人
        /// </summary>
        public string yzrdw { get; set; }
        /// <summary>
        /// 登记与接收时间差
        /// </summary>
        public int sjc { get; set; }

        public int status { get; set; }
    }


    public class sbgztj
    {
       
        /// <summary>
        /// 单位
        /// </summary>
        public string dw { get; set; }
        /// <summary>
        /// 登记数量
        /// </summary>
        public double djsl { get; set; }
        /// <summary>
        /// 处理数量（也等于应验证数量）
        /// </summary>
        public double clsl { get; set; }
        /// <summary>
        /// 应验证数量
        /// </summary>
        public double yyzsl { get; set; }
        /// <summary>
        /// 验证数量
        /// </summary>
        public double yzsl { get; set; }
        /// <summary>
        /// 验证率
        /// </summary>
        public double yzrate { get; set; }

        /// <summary>
        /// 待处理数量
        /// </summary>
        public double dclsl { get; set; }
        /// <summary>
        /// 已接收数量
        /// </summary>
        public double jssl { get; set; }

        /// <summary>
        /// 应接收数量
        /// </summary>
        public double yjssl { get; set; }

        /// <summary>
        /// 延误率（超两天未接收数量/生成数量）
        /// </summary>
        public double ywsl { get; set; }

        /// <summary>
        /// 延误率（超两天未接收数量/生成数量）
        /// </summary>
        public double ywrate { get; set; }

        /// <summary>
        /// 及时接收率（前5天接收/生成数量）
        /// </summary>
        public double jsrate { get; set; }

        /// <summary>
        /// 及时处理率（前5天处理/生成数量）
        /// </summary>
        public double clrate { get; set; }
    }

    public class sbgzdw
    {
        public string dw { get; set; }
    }
}
