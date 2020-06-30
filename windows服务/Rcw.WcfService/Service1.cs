using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Rcw.WcfService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“Service1”。
    public class Service1 : IService1
    {
        public void DoWork()
        {
        }
        [WebGet()]
        public List<ComProcess> testwcf()
        {
            Process[] ps = Process.GetProcessesByName("AppServer");
            List<ComProcess> list = new List<ComProcess>();
            foreach (var item in ps)
            {
                ComProcess cp = new ComProcess();
                cp.pid = item.Id.ToString();
                cp.name = item.ProcessName;
                cp.size = (item.WorkingSet64 / 1024 / 1024).ToString();
                cp.starttime = item.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                list.Add(cp);

            }           
            return list;
        }
    }
}
