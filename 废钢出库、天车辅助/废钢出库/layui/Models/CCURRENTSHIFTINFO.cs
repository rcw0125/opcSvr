using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using Rcw.Data;
using System.ComponentModel;
namespace layui.Models
{
    //CSCRAP_OUTPUT_LOG

    public class CCURRENTSHIFTINFO : DbEntity
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
				
		

       

       
        private double _shiftid;
        /// <summary>
        /// 班次
        /// </summary>		

        [DisplayName("班次")]
        public double SHIFTID
        {
            get
            {
                return _shiftid;
            }
            set
            {
                if (_shiftid != value)
                {
                    _shiftid = value;
                    RaisePropertyChanged("SHIFTID", true);
                }
            }
        }

        private double _teamid;
        /// <summary>
        /// 班别
        /// </summary>		

        [DisplayName("班别")]
        public double TEAMID
        {
            get
            {
                return _teamid;
            }
            set
            {
                if (_teamid != value)
                {
                    _teamid = value;
                    RaisePropertyChanged("TEAMID", true);
                }
            }
        }


        #endregion

        #region  扩展方法


        /// <summary>
        /// 获取数据列表
        /// </summary>
        public static List<CCURRENTSHIFTINFO> GetList(string whereSql="1=1", params object[] args)
		{
		    return DbContext.LoadDataByWhere<CCURRENTSHIFTINFO>(whereSql, args);
		}
		

        #endregion 扩展方法   
    }
}