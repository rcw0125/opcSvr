using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Rcw.Data;

namespace cfyc
{
    class L_Alloy_Rate : DbEntity
    {
        private string _steelgrade;
        [DisplayName("标识")]
        [DbTableColumn(IsPrimaryKey = true)]
        public string steelgrade
        {
            get
            {
                return _steelgrade;
            }
            set
            {
                if (_steelgrade != value)
                {
                    _steelgrade = value;
                    RaisePropertyChanged("steelgrade", true);
                }
            }
        }

        private string _steelgrade_des;
        [DisplayName("钢种分类")]           
        public string steelgrade_des
        {
            get
            {
                return _steelgrade_des;
            }
            set
            {
                if (_steelgrade_des != value)
                {
                    _steelgrade_des = value;
                    RaisePropertyChanged("steelgrade_des", true);
                }
            }
        }

        private double _c;
        [DisplayName("C吸收率")]
        public double c
        {
            get
            {
                return _c;
            }
            set
            {
                if (_c != value)
                {
                    _c = value;
                    RaisePropertyChanged("c", true);
                }
            }
        }
  
        private double _si;
        [DisplayName("Si吸收率")]
        public double si
        {
            get
            {
                return _si;
            }
            set
            {
                if (_si != value)
                {
                    _si = value;
                    RaisePropertyChanged("si", true);
                }
            }
        }

        private double _mn;
        [DisplayName("Mn吸收率")]
        public double mn
        {
            get
            {
                return _mn;
            }
            set
            {
                if (_mn != value)
                {
                    _mn = value;
                    RaisePropertyChanged("mn", true);
                }
            }
        }

        //public enum Cr_js
        //{
        //    否 = 0,
        //    计算 = 1
        //}
        private double _cr;
        [DisplayName("Cr吸收率")]
        public double cr
        {
            get
            {
                return _cr;
            }
            set
            {
                if (_cr != value)
                {
                    _cr = value;
                    RaisePropertyChanged("cr", true);
                }
            }
        }

        /// <summary>
		/// 获取数据列表
		/// </summary>
		public static List<L_Alloy_Rate> GetList(string whereSql = "1=1", params object[] args)
        {
            return DbContext.LoadDataByWhere<L_Alloy_Rate>(whereSql, args);
        }



    }
}
