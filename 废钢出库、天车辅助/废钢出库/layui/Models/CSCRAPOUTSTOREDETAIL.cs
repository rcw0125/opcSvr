using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using Rcw.Data;
using System.ComponentModel;
namespace layui.Models
{
    //CSCRAP_OUTPUT_LOG

    public class CSCRAPOUTSTOREDETAIL : DbEntity
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
				
		

        private string _name;
        /// <summary>
        /// 未知
        /// </summary>		

        [DisplayName("未知")]
        public string NAME
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged("NAME", true);
                }
            }
        }

        

        private string _material;
        /// <summary>
        /// 物料类型
        /// </summary>		

        [DisplayName("物料类型")]
        public string MATERIAL
        {
            get
            {
                return _material;
            }
            set
            {
                if (_material != value)
                {
                    _material = value;
                    RaisePropertyChanged("MATERIAL", true);
                }
            }
        }

        private string _material_code;
        /// <summary>
        /// 物料编码
        /// </summary>		

        [DisplayName("物料编码")]
        public string MATERIAL_CODE
        {
            get
            {
                return _material_code;
            }
            set
            {
                if (_material_code != value)
                {
                    _material_code = value;
                    RaisePropertyChanged("MATERIAL_CODE", true);
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

        

        #endregion

        #region  扩展方法

        
		/// <summary>
		/// 获取数据列表
		/// </summary>
		public static List<CSCRAPOUTSTOREDETAIL> GetList(string whereSql="1=1", params object[] args)
		{
		    return DbContext.LoadDataByWhere<CSCRAPOUTSTOREDETAIL>(whereSql, args);
		}
		

        #endregion 扩展方法   
    }
}