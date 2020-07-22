using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opcBase
{
    public class ccm3cut
    {
        private static ccmCut instance = null;
        /// <summary>         
        /// 获取实例 （单例模式）       
        /// </summary>         
        /// <returns></returns>   
        public static ccmCut GetInstance()
        {
            if (instance == null)
            {
                instance = new ccmCut();
                instance.setupDateConfig("S63", 88);
            }
            return instance;
        }
    }
}
