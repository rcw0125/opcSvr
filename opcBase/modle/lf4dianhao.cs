using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opcBase
{
    public class lf4dianhao
    {
        private static lfdianhao instance = null;
        /// <summary>         
        /// 获取实例 （单例模式）       
        /// </summary>         
        /// <returns></returns>   
        public static lfdianhao GetInstance()
        {
            if (instance == null)
            {
                instance = new lfdianhao();
                instance.setupDateConfig("4", 125);
            }
            return instance;
        }
    }
}
