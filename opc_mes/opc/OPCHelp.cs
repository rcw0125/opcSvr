using System;
using System.Collections.Generic;
using System.Text;
using OPCAutomation;
using System.Data;

namespace opc
{
    public class OPCHelp
    {
        #region 私有变量
        /// <summary>
        /// 连接状态
        /// </summary>
        bool opc_connected = false;
        /// <summary>
        /// 客户端句柄
        /// </summary>
        int itmHandleClient = 0;
        /// <summary>
        /// 服务端句柄
        /// </summary>
        int itmHandleServer = 0;
        /// <summary>
        /// OPCServer Object
        /// </summary>
        OPCServer KepServer;
        /// <summary>
        /// OPCGroups Object
        /// </summary>
        OPCGroups KepGroups;
        /// <summary>
        /// OPCGroup Object
        /// </summary>
        OPCGroup KepGroup;
        /// <summary>
        /// OPCItems Object
        /// </summary>
        OPCItems KepItems;
        /// <summary>
        /// OPCItem Object
        /// </summary>
        OPCItem KepItem;
        #endregion
        private static OPCHelp _instance = null;
        /// <summary>         
        /// 获取实例 （单例模式）       
        /// </summary>         
        /// <returns></returns>         
        public static OPCHelp GetInstance()
        {
            if (_instance == null)
            {
                _instance = new OPCHelp();
            }                         
            return _instance;
        }
        public OPCHelp()
        {
            loadValList();
            connectOPC();
        }

        List<L1tag> listTag = null;
        /// <summary>
        /// 加载变量表
        /// </summary>
        public void loadValList()
        {
            var db = new DbHelp("192.168.36.162", "cmsdb", "sa", "xgmes");
            string sql = "select id,L1name as name,scanrate from  L1OPC_TAG_MES where used=1 order by id ";
            var dt = db.Query(sql);
            listTag = dt_to_list(dt);
            
        }

        /// <summary>
        /// 初始化OPC服务
        /// </summary>
        public void connectOPC()
        {

            KepServer = new OPCServer();
            try
            {
                //"OPC.SimaticNET";
                KepServer.Connect("OPC.SimaticNET", "192.168.48.232");
                //KepServer.Connect("OPCServer.WinCC", "192.168.36.135");
            }
            catch (Exception ex)
            {
                
            }
            KepGroups = KepServer.OPCGroups;
            KepGroup = KepGroups.Add("myGroup");
            KepGroup.IsSubscribed = true;
            KepGroup.DataChange += Group_DataChange;
            foreach (var item in listTag)
            {
                OPCItem myItem = KepGroup.OPCItems.AddItem(item.name, item.id);
                item.itmHandleServer = myItem.ServerHandle;
            }          
         
        }

        public List<L1tag> dt_to_list(DataTable dt)
        {
            List<L1tag> taglist = new List<L1tag>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    L1tag tag = new L1tag();
                    tag.id = Convert.ToInt16(item["id"]);
                    tag.name = item["name"].ToString();
                    tag.scanrate = Convert.ToInt16(item["scanrate"]);
                    tag.lasttime = DateTime.Now.AddHours(-2);
                    taglist.Add(tag);
                }

            }

            return taglist;
        }

        private void Group_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            //项目实际不需要，数据变化触发的操作
            //for (int i = 1; i < NumItems + 1; i++)
            //{
            //    readval(ClientHandles.GetValue(i).ToString(), ItemValues.GetValue(i).ToString());
            //}
        }
        public void readval(string i, string val)
        {     
            L1tag curitem = listTag.Find(o => o.id == Convert.ToInt16(i));
            //如果扫描周期小于1，按事件采集的，则不向数据库插入数据
            if (curitem.scanrate < 1)
            {
                return;
            }
            if ((DateTime.Now - curitem.lasttime).TotalSeconds >= curitem.scanrate)
            {
                curitem.lasttime = DateTime.Now;
                savedata(Convert.ToInt16(i), Convert.ToDouble(val));
            }
        }
        public void savedata(int i, double val)
        {
            var db = new DbHelp("192.168.36.162", "cmsdb", "sa", "xgmes");

            string sql = "insert into l1_tag_value(id,val) values(" + i + "," + val + ")";
            db.ExeSql(sql);
        }


        #region 根据变量ID获取OPC变量的值
        /// <summary>
        /// 根据变量id获取服务端句柄
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int getHandleServerById(int id)
        {
            L1tag curitem = listTag.Find(o => o.id == id);
            return curitem.itmHandleServer;
        }
        /// <summary>
        /// 根据变量ID获取变量的值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double getVal(int id)
        {         
            return Convert.ToDouble(KepGroup.OPCItems.GetOPCItem(getHandleServerById(id)).Value);
        }
        /// <summary>
        /// 根据变量ID获取变量的值(int32类型)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Int32 getValInt(int id)
        {           
            return Convert.ToInt32(KepGroup.OPCItems.GetOPCItem(getHandleServerById(id)).Value);
        }
        /// <summary>
        /// 设置变量的值,(写入double类型)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="val"></param>
        public bool setVal(int id, double val)
        {
            try
            {
                KepGroup.OPCItems.GetOPCItem(getHandleServerById(id)).Write(val);
            }
            catch
            {
                return false;
            }
            return true;
            
        }
        /// <summary>
        /// 设置变量的值,(写入double类型)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="val"></param>
        public bool setVal(int id, int val)
        {
            try
            {
                KepGroup.OPCItems.GetOPCItem(getHandleServerById(id)).Write(val);
            }
            catch
            {
                return false;
            }
            return true;
        }
       
        #endregion       
     

    }
}
