using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace layui
{
    public class SingleCase
    {
        private static NetMESJianKong instance = null;
        /// <summary>         
        /// 获取实例 （单例模式）       
        /// </summary>         
        /// <returns></returns>   
        public static NetMESJianKong GetInstance()
        {
            if (instance == null)
            {
                instance = new NetMESJianKong();      
            }
            return instance;
        }
    }
}