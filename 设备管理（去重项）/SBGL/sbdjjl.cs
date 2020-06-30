using System;
using System.Collections.Generic;
using Rcw.Data;

namespace SBGL
{
    public class sbdjjl
    {


    
        private string _sblist_id;
        public string sblist_id
        {
            get
            {
                return _sblist_id;
            }
            set
            {
                if (_sblist_id != value)
                {
                    _sblist_id = value;
                  
                }
            }
        }

        private string _sblistname;
        public string sblistname
        {
            get
            {
                return _sblistname;
            }
            set
            {
                if (_sblistname != value)
                {
                    _sblistname = value;
                }
            }
        }
        //点检路线
        private string _djlx;
        public string djlx
        {
            get
            {
                return _djlx;
            }
            set
            {
                if (_djlx != value)
                {
                    _djlx = value;
                }
            }
        }


        //点检单位
        private string _djdw;
        public string djdw
        {
            get
            {
                return _djdw;
            }
            set
            {
                if (_djdw != value)
                {
                    _djdw = value;
                }
            }
        }
        // //点检班组
        //select a.sblist_id, a.sblistname, a.status, a.comment, a.djr, b.name as djlx , c.name as djdw, d.name as djbz, e.name as djlb
        private string _djbz;
        public string djbz
        {
            get
            {
                return _djbz;
            }
            set
            {
                if (_djbz != value)
                {
                    _djbz = value;
                }
            }
        }
        //点检周期
        private string _djzq;
        public string djzq
        {
            get
            {
                return _djzq;
            }
            set
            {
                if (_djzq != value)
                {
                    _djzq = value;
                }
            }
        }
        //点检类别
        private string _djlb;
        public string djlb
        {
            get
            {
                return _djlb;
            }
            set
            {
                if (_djlb != value)
                {
                    _djlb = value;
                }
            }
        }

        private string _status;
        public string status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    _status = value;              
                }
            }
        }

        private string _comment;
        public string comment
        {
            get
            {
                return _comment;
            }
            set
            {
                if (_comment != value)
                {
                    _comment = value;            
                }
            }
        }

        private string _djr;
        public string djr
        {
            get
            {
                return _djr;
            }
            set
            {
                if (_djr != value)
                {
                    _djr = value;
               
                }
            }
        }
        //点检日期
        private string _djrq;
        public string djrq
        {
            get
            {
                return _djrq;
            }
            set
            {
                if (_djrq != value)
                {
                    _djrq = value;

                }
            }
        }




    }
}
