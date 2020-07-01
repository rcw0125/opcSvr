using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inputscrap
{
    public class lfstatus
    {
        public int status { get; set; }
        public DateTime logtime { get; set; }
        public int gears { get; set; }

        public string treatno { get; set; }
    }

    public class heatdata
    {      
        public DateTime? kstime { get; set; }
        public DateTime? jstime { get; set; }
    }


    public class fdjs
    {
        public int gears { get; set; }
        public double val { get; set; }
    }



}
