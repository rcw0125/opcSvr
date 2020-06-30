using Rcw.Method;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rcw.WcfService
{
    class serviceList
    {
        public List<serviceClassType> serviceClassList;

        public serviceClassType serviceClassType1;

        public serviceList()
        {
            serviceClassList = new List<serviceClassType>();
            serviceClassList.Add(new serviceClassType(typeof(Service1), typeof(IService1)));         
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
