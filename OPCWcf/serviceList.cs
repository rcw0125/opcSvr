using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rcw.Method;

namespace OPCWcf
{
    public class serviceList
    {
        public List<serviceClassType> serviceClassList;
   
        public serviceList()
        {
            serviceClassList = new List<serviceClassType>();
            serviceClassList.Add(new serviceClassType(typeof(OpcService), typeof(IOpcService)));
        }

        public void Open()
        {
            if (serviceClassList != null)
            {
                foreach (var item in serviceClassList)
                {
                    wcfHost wcfhost = new wcfHost(item);
                    //Thread thread = new Thread(new ParameterizedThreadStart(start));
                    //thread.Start(item);

                    //var act= new Action<serviceClassType>(start)).BeginInvoke(null, null);
                    //act(item);
                    //new Action(start)(item).BeginInvoke(null, null);

                }
            }
        }
    }
}
