using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Rcw.Data;

namespace SBGL
{
    [DbTable(IsView =true)]
    public class sbjxjh_view:DbEntity
    {
      

        private Int64 _id;            
        public Int64 id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    RaisePropertyChanged("id", true);
                }
            }
        }

        private Int64 _sblist_id;
        public Int64 sblist_id
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
                    RaisePropertyChanged("sblist_id", true);
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
                    RaisePropertyChanged("sblistname", true);
                }
            }
        }

        private string _sblistcode;
        public string sblistcode
        {
            get
            {
                return _sblistcode;
            }
            set
            {
                if (_sblistcode != value)
                {
                    _sblistcode = value;
                    RaisePropertyChanged("sblistcode", true);
                }
            }
        }

        private string _beizhu;
        public string beizhu
        {
            get
            {
                return _beizhu;
            }
            set
            {
                if (_beizhu != value)
                {
                    _beizhu = value;
                    RaisePropertyChanged("beizhu", true);
                }
            }
        }

        private Int32 _sbjxdw_id;
        public Int32 sbjxdw_id
        {
            get
            {
                return _sbjxdw_id;
            }
            set
            {
                if (_sbjxdw_id != value)
                {
                    _sbjxdw_id = value;
                    RaisePropertyChanged("sbjxdw_id", true);
                }
            }
        }

        private Int32 _sbjxbzu_id;
        public Int32 sbjxbzu_id
        {
            get
            {
                return _sbjxbzu_id;
            }
            set
            {
                if (_sbjxbzu_id != value)
                {
                    _sbjxbzu_id = value;
                    RaisePropertyChanged("sbjxbzu_id", true);
                }
            }
        }

        private Int16 _sbjxzq;
        public Int16 sbjxzq
        {
            get
            {
                return _sbjxzq;
            }
            set
            {
                if (_sbjxzq != value)
                {
                    _sbjxzq = value;
                    RaisePropertyChanged("sbjxzq", true);
                }
            }
        }


        private Int16 _syts;
        public Int16 syts
        {
            get
            {
                return _syts;
            }
            set
            {
                if (_syts != value)
                {
                    _syts = value;
                    RaisePropertyChanged("syts", true);
                }
            }
        }

        private DateTime _scdate;
        public DateTime scdate
        {
            get
            {
                return _scdate;
            }
            set
            {
                if (_scdate != value)
                {
                    _scdate = value;
                    RaisePropertyChanged("scdate", true);
                }
            }
        }

        private DateTime _jhdate;
        public DateTime jhdate
        {
            get
            {
                return _jhdate;
            }
            set
            {
                if (_jhdate != value)
                {
                    _jhdate = value;
                    RaisePropertyChanged("jhdate", true);
                }
            }
        }

       

        private string _jxnr;
        public string jxnr
        {
            get
            {
                return _jxnr;
            }
            set
            {
                if (_jxnr != value)
                {
                    _jxnr = value;
                    RaisePropertyChanged("jxnr", true);
                }
            }
        }

        private int _status;
        public int status
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
                    RaisePropertyChanged("status", true);
                }
            }
        }

        private int _jxlb;
        public int jxlb
        {
            get
            {
                return _jxlb;
            }
            set
            {
                if (_jxlb != value)
                {
                    _jxlb = value;
                    RaisePropertyChanged("jxlb", true);
                }
            }
        }

        public static List<lgsb_sbdjjl_table> GetList(string whereSql = "1=1", params object[] args)
        {
            return DbContext.LoadDataByWhere<lgsb_sbdjjl_table>(whereSql, args);
        }

    }
}
