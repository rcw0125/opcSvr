using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Rcw.Data;

namespace cfyc
{
    class L_Alloy_Config : DbEntity
    {
        private string _code;
        [DisplayName("合金编码")]
        [DbTableColumn(IsPrimaryKey = true)]
        public string code
        {
            get
            {
                return _code;
            }
            set
            {
                if (_code != value)
                {
                    _code = value;
                    RaisePropertyChanged("code", true);
                }
            }
        }

        private string _code_des;
        [DisplayName("合金名称")]
        public string code_des
        {
            get
            {
                return _code_des;
            }
            set
            {
                if (_code_des != value)
                {
                    _code_des = value;
                    RaisePropertyChanged("code_des", true);
                }
            }
        }

        public enum ztj
        {
            否 = 0,
            是 = 1
        }

        private ztj _zcj;
        [DisplayName("是否是增碳剂，否 = 0,是 = 1")]
        public ztj zcj
        {
            get
            {
                return _zcj;
            }
            set
            {
                if (_zcj != value)
                {
                    _zcj = value;
                    RaisePropertyChanged("zcj", true);
                }
            }
        }

        public double _rate;
        [DisplayName("吸收率")]
        public double rate
        {
            get
            {
                return _rate;
            }
            set
            {
                if (_rate != value)
                {
                    _rate = value;
                    RaisePropertyChanged("rate", true);
                }
            }
        }


        //public enum c_js
        //{
        //    否 = 0,
        //    计算 = 1
        //}
        private double _c_cal;
        [DisplayName("C品位")]
        public double c_cal
        {
            get
            {
                return _c_cal;
            }
            set
            {
                if (_c_cal != value)
                {
                    _c_cal = value;
                    RaisePropertyChanged("c_cal", true);
                }
            }
        }


        //public enum si_js
        //{
        //    否 = 0,
        //    计算 = 1
        //}
        private double _si_cal;
        [DisplayName("Si品位")]
        public double si_cal
        {
            get
            {
                return _si_cal;
            }
            set
            {
                if (_si_cal != value)
                {
                    _si_cal = value;
                    RaisePropertyChanged("si_cal", true);
                }
            }
        }


        //public enum Mn_js
        //{
        //    否 = 0,
        //    计算 = 1
        //}
        private double _Mn_cal;
        [DisplayName("Mn品位")]
        public double Mn_cal
        {
            get
            {
                return _Mn_cal;
            }
            set
            {
                if (_Mn_cal != value)
                {
                    _Mn_cal = value;
                    RaisePropertyChanged("Mn_cal", true);
                }
            }
        }

        //public enum Cr_js
        //{
        //    否 = 0,
        //    计算 = 1
        //}
        private double _Cr_cal;
        [DisplayName("Cr品位")]
        public double Cr_cal
        {
            get
            {
                return _Cr_cal;
            }
            set
            {
                if (_Cr_cal != value)
                {
                    _Cr_cal = value;
                    RaisePropertyChanged("Cr_cal", true);
                }
            }
        }

        /// <summary>
		/// 获取数据列表
		/// </summary>
		public static List<L_Alloy_Config> GetList(string whereSql = "1=1", params object[] args)
        {
            return DbContext.LoadDataByWhere<L_Alloy_Config>(whereSql, args);
        }



    }
}
