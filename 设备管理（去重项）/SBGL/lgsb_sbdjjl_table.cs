using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Rcw.Data;

namespace SBGL
{
    public class lgsb_sbdjjl_table:DbEntity
    {
      

        private Int64 _id;      
        [DisplayName("名称")]
        [DbTableColumn(IsPrimaryKey = true)]
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
                    RaisePropertyChanged("status", true);
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
                    RaisePropertyChanged("comment", true);
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
                    RaisePropertyChanged("djr", true);
                }
            }
        }

        private Int64 _sbdjlx_id;
        public Int64 sbdjlx_id
        {
            get
            {
                return _sbdjlx_id;
            }
            set
            {
                if (_sbdjlx_id != value)
                {
                    _sbdjlx_id = value;
                    RaisePropertyChanged("sbdjlx_id", true);
                }
            }
        }
        //sbdjlb_id
        private Int64 _sbdjlb_id;
        public Int64 sbdjlb_id
        {
            get
            {
                return _sbdjlb_id;
            }
            set
            {
                if (_sbdjlb_id != value)
                {
                    _sbdjlb_id = value;
                    RaisePropertyChanged("sbdjlb_id", true);
                }
            }
        }


        public static List<lgsb_sbdjjl_table> GetList(string whereSql = "1=1", params object[] args)
        {
            return DbContext.LoadDataByWhere<lgsb_sbdjjl_table>(whereSql, args);
        }

    }
}
