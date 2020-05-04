using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opcBase
{
    public class WinccCcm_B
    {
        private static OPCSvr instance = null;
        /// <summary>         
        /// 获取实例 （单例模式）       
        /// </summary>         
        /// <returns></returns>   
        public static OPCSvr GetInstance()
        {
            if (instance == null)
            {
                instance = new OPCSvr();
                instance.type = 4;
                instance.loadValList();
                instance.connectOPC();
            }
            return instance;
        }

    }
}
