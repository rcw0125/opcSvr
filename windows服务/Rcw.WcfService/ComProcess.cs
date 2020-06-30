using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rcw.WcfService
{
    public class ComProcess
    {
        public string pid { set; get; }
        public string name { get; set; }
        public string size { get; set; }

        public string starttime { get; set; }
    }
}
