using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opcBase
{
    public class fg5weight
    {
        private static fgweight instance = null;
        /// <summary>         
        /// 获取实例 （单例模式）       
        /// </summary>         
        /// <returns></returns>   
        public static fgweight GetInstance()
        {
            if (instance == null)
            {
                instance = new fgweight();             
                instance.setupDateConfig("5", 89,90);
            }
            return instance;
        }
    }
}
