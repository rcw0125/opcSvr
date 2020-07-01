using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace layui.menu
{
    public class TS_MENU
    {
        public string C_ID { get; set; }
        public string C_PARENT_ID { get; set; }
        public string C_NAME { get; set; }
        public string C_DESC { get; set; }
        public string C_URL { get; set; }
        public string N_SOURCEPATH { get; set; }
        public Nullable<short> N_STATUS { get; set; }
        public Nullable<int> N_MENULEVEL { get; set; }
        public string C_ICON { get; set; }
        public Nullable<int> N_SORT { get; set; }
        public string C_EMP_ID { get; set; }
        public string C_EMP_NAME { get; set; }
        public Nullable<System.DateTime> D_MOD_DT { get; set; }
    }
}