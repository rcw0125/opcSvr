using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Rcw.Data;
using SqlSugar;

namespace SBGL
{
    [SugarTable("lgsb_sbjxjh_table")]
    public class lgsb_sbjxjh_table
    {


        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "ID")]
        public Int64 id
        {
            get;set;
        }

      
        public Int64 sblist_id
        {
            get; set;
        }

        public string sblistname
        {
            get; set;
        }

        public string sblistcode
        {
            get; set;
        }

        public string beizhu
        {
            get; set;
        }

        public Int32 sbjxdw_id
        {
            get; set;
        }
    
        public Int32 sbjxbzu_id
        {
            get; set;
        }
     
        public Int16 sbjxzq
        {
            get; set;
        }

        public Int16 syts
        {
            get; set;
        }
        public DateTime scdate
        {
            get; set;
        }     
        public DateTime jhdate
        {
            get; set;
        }

      
        public string jxnr
        {
            get; set;
        }


        public int status
        {
            get; set;
        }


        public int jxlb
        {
            get; set;
        }    

    }
}
