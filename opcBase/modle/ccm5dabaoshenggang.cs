using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opcBase
{
   public  class ccm5dabaoshenggang
    {
        private static dabaoshenggang instance = null;
        /// <summary>         
        /// 获取实例 （单例模式）       
        /// </summary>         
        /// <returns></returns>   
        public static dabaoshenggang GetInstance()
        {
            if (instance == null)
            {
                instance = new dabaoshenggang();
               
            }
            return instance;
        }
    }
}
