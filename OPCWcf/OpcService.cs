using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using opcBase;

namespace OPCWcf
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“OpcService”。
    public class OpcService : IOpcService
    {
        public string DoWork()
        {
            return "hello world!";
        }
        public List<valRes> getValById(int id)
        {
            return OpcHelp.GetValById(id);
        }
        public List<valRes> getval(List<valReq> listReq)
        {          
            return OpcHelp.GetOpcVal(listReq);
        }
    }
}
