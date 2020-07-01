using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using Rcw.Data;
using System.ComponentModel;
namespace layui.Models
{
    //CSCRAP_OUTPUT_LOG

    public class CSCRAP_OUTPUT_LOG : DbEntity
	{
   		#region  属性    
      			
		private string _guid;	
		/// <summary>
		/// 主键
        /// </summary>		
		[DbTableColumn(IsPrimaryKey = true)]		
		[DisplayName("主键")]
        public string GUID
        {
            get
            {
            	return _guid; 
            }
            set
            {
                if (_guid != value)
                {
                    _guid = value;
                   RaisePropertyChanged("GUID", true);	                   
                }
            }
        }        
				
		

        private string _materialtype;
        /// <summary>
        /// 物料类型
        /// </summary>		

        [DisplayName("物料类型")]
        public string MATERIALTYPE
        {
            get
            {
                return _materialtype;
            }
            set
            {
                if (_materialtype != value)
                {
                    _materialtype = value;
                    RaisePropertyChanged("MATERIALTYPE", true);
                }
            }
        }

        private DateTime _logtime;
        /// <summary>
        /// 时间
        /// </summary>		

        [DisplayName("时间")]
        public DateTime LOGDATE
        {
            get
            {
                return _logtime;
            }
            set
            {
                if (_logtime != value)
                {
                    _logtime = value;
                    RaisePropertyChanged("LOGDATE", true);
                }
            }
        }


        private string _materialid;
        /// <summary>
        /// 物料id
        /// </summary>		

        [DisplayName("物料id")]
        public string MATERIALID
        {
            get
            {
                return _materialid;
            }
            set
            {
                if (_materialid != value)
                {
                    _materialid = value;
                    RaisePropertyChanged("MATERIALID", true);
                }
            }
        }

        private double _amount;
        /// <summary>
        /// 重量
        /// </summary>		

        [DisplayName("重量")]
        public double AMOUNT
        {
            get
            {
                return _amount;
            }
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    RaisePropertyChanged("AMOUNT", true);
                }
            }
        }


        private double _type;
        /// <summary>
        /// 类型
        /// </summary>		

        [DisplayName("类型")]
        public double TYPE
        {
            get
            {
                return _type;
            }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    RaisePropertyChanged("TYPE", true);
                }
            }
        }

        private string _storeareaid;
        /// <summary>
        /// 仓库id
        /// </summary>		

        [DisplayName("出库id")]
        public string STOREAREAID
        {
            get
            {
                return _storeareaid;
            }
            set
            {
                if (_storeareaid != value)
                {
                    _storeareaid = value;
                    RaisePropertyChanged("STOREAREAID", true);
                }
            }
        }

        private string _operator;
        /// <summary>
        /// 操作员id
        /// </summary>		

        [DisplayName("操作员id")]
        public string OPERATOR
        {
            get
            {
                return _operator;
            }
            set
            {
                if (_operator != value)
                {
                    _operator = value;
                    RaisePropertyChanged("OPERATOR", true);
                }
            }
        }

        private string _shift;
        /// <summary>
        /// 班次
        /// </summary>		

        [DisplayName("班次")]
        public string SHIFT
        {
            get
            {
                return _shift;
            }
            set
            {
                if (_shift != value)
                {
                    _shift = value;
                    RaisePropertyChanged("SHIFT", true);
                }
            }
        }

        private string _team;
        /// <summary>
        /// 班别
        /// </summary>		

        [DisplayName("班别")]
        public string TEAM
        {
            get
            {
                return _team;
            }
            set
            {
                if (_team != value)
                {
                    _team = value;
                    RaisePropertyChanged("TEAM", true);
                }
            }
        }

        private string _scrap_slot_id;
        /// <summary>
        /// 废钢斗号
        /// </summary>		

        [DisplayName("废钢斗号")]
        public string SCRAP_SLOT_ID
        {
            get
            {
                return _scrap_slot_id;
            }
            set
            {
                if (_scrap_slot_id != value)
                {
                    _scrap_slot_id = value;
                    RaisePropertyChanged("SCRAP_SLOT_ID", true);
                }
            }
        }

        private string _heatid;
        /// <summary>
        /// 废钢斗号
        /// </summary>		

        [DisplayName("废钢斗号")]
        public string HEATID
        {
            get
            {
                return _heatid;
            }
            set
            {
                if (_heatid != value)
                {
                    _heatid = value;
                    RaisePropertyChanged("HEATID", true);
                }
            }
        }

        private double _net_weight;
        /// <summary>
        /// 净重
        /// </summary>		

        [DisplayName("净重")]
        public double NET_WEIGHT
        {
            get
            {
                return _net_weight;
            }
            set
            {
                if (_net_weight != value)
                {
                    _net_weight = value;
                    RaisePropertyChanged("NET_WEIGHT", true);
                }
            }
        }

        private double _gross_weight;
        /// <summary>
        /// 毛重
        /// </summary>		

        [DisplayName("毛重")]
        public double GROSS_WEIGHT
        {
            get
            {
                return _gross_weight;
            }
            set
            {
                if (_gross_weight != value)
                {
                    _gross_weight = value;
                    RaisePropertyChanged("GROSS_WEIGHT", true);
                }
            }
        }

        private double _tare_weight;
        /// <summary>
        /// 皮重
        /// </summary>		

        [DisplayName("皮重")]
        public double TARE_WEIGHT
        {
            get
            {
                return _tare_weight;
            }
            set
            {
                if (_tare_weight != value)
                {
                    _tare_weight = value;
                    RaisePropertyChanged("TARE_WEIGHT", true);
                }
            }
        }

        private double _iron_flag;
        /// <summary>
        /// 指定标志
        /// </summary>		

        [DisplayName("指定标志")]
        public double IRON_FLAG
        {
            get
            {
                return _iron_flag;
            }
            set
            {
                if (_iron_flag != value)
                {
                    _iron_flag = value;
                    RaisePropertyChanged("IRON_FLAG", true);
                }
            }
        }

        #endregion

        #region  扩展方法


        /// <summary>
        /// 获取数据列表
        /// </summary>
        public static List<CSCRAP_OUTPUT_LOG> GetList(string whereSql="1=1", params object[] args)
		{
		    return DbContext.LoadDataByWhere<CSCRAP_OUTPUT_LOG>(whereSql, args);
		}
		

        #endregion 扩展方法   
    }
}