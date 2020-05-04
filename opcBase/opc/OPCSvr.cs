using System;
using System.Collections.Generic;
using System.Text;
using OPCAutomation;
using System.Data;

namespace opcBase
{
    public class OPCSvr
    {
        #region 0、私有变量
        /// <summary>
        /// 连接状态
        /// </summary>
       public bool opc_connected = false;
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
        /// 与KepGroup相比，没有预约属性
        /// </summary>
        OPCGroup KepEventGroup;
        /// <summary>
        /// OPCItems Object
        /// </summary>
        OPCItems KepItems;
        /// <summary>
        /// OPCItem Object
        /// </summary>
        OPCItem KepItem;
        #endregion
        public int type = 0;

        public OPCSvr()
        {

        }
        #region 1、从数据库读取变量配置，并转换为变量列表List

        List<L1tag> listTag = new List<L1tag>();
        /// <summary>
        /// 加载变量表
        /// </summary>
        public void loadValList()
        {
            try
            {
                string sql = "select id,L1name as name,scanrate,datatype from  L1OPC_TAG where used=1 and type=" + type + " order by id ";
                var dt = new sqlDbHelp().Query(sql);
                listTag = dt_to_list(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("加载变量列表时出错：" + ex.ToString());
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
                    tag.datatype = Convert.ToInt16(item["datatype"]);
                    tag.lasttime = DateTime.Now.AddHours(-2);
                    taglist.Add(tag);
                }
            }
            return taglist;
        }
        #endregion


        #region 2、根据变量列表，初始化OPC服务

        /// <summary>
        /// 初始化OPC服务
        /// </summary>
        public void connectOPC()
        {
            KepServer = new OPCServer();
            try
            {
                if (type == 0)
                {
                    KepServer.Connect("OPC.SimaticNET", "192.168.48.232");
                }
                else if (type == 1)
                {
                    KepServer.Connect("OPCServer.WinCC", "192.168.36.135");
                }
                else if (type == 2)
                {
                    KepServer.Connect("OPCServer.WinCC", "192.168.48.135");
                }
                else if (type == 3)
                {
                    KepServer.Connect("OPCServer.WinCC", "192.168.36.125");
                }
                else if (type == 4)
                {
                    KepServer.Connect("OPCServer.WinCC", "192.168.48.125");
                }
                else if (type == 5)
                {
                    //KepServer.Connect("KEPware.KEPServerEx.V5", "192.168.48.243");
                    KepServer.Connect("KEPware.KEPServerEx.V6", "192.168.48.163");
                }
                opc_connected = true;
                KepGroups = KepServer.OPCGroups;
                KepGroup = KepGroups.Add("myGroup");
                KepGroup.IsSubscribed = true;
                KepGroup.DataChange += Group_DataChange;

                KepEventGroup = KepGroups.Add("myEventGroup");
                KepEventGroup.IsSubscribed = true;

                foreach (var item in listTag)
                {
                    if (item.scanrate > 0)
                    {
                        OPCItem myItem = KepGroup.OPCItems.AddItem(item.name, item.id);
                        item.itmHandleServer = myItem.ServerHandle;
                    }
                    else
                    {
                        OPCItem myItem = KepEventGroup.OPCItems.AddItem(item.name, item.id);
                        item.itmHandleServer = myItem.ServerHandle;
                    }

                }
            }
            catch (Exception ex)
            {

                throw new Exception("初始化OPC服务时，出现错误："+type+ex.ToString());
            }
        }

        #endregion
   

        #region 3、预约属性数据变化，变化时，根据扫描周期，大于周期时，将数据写入到数据库

        private void Group_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            //项目实际不需要，数据变化触发的操作
            for (int i = 1; i < NumItems + 1; i++)
            {
                readval(ClientHandles.GetValue(i).ToString(), ItemValues.GetValue(i).ToString());
            }
        }
        public void readval(string i, string val)
        {
            try
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
                    savedata(Convert.ToInt16(i), Math.Round(Convert.ToDouble(val), 3));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("处理数据变化时出错：" + ex.ToString());
            }
           
        }
        public void savedata(int i, double val)
        {
            var db = new sqlDbHelp();
            string sql = "insert into l1_tag_value(id,val) values(" + i + "," + val + ")";
            db.ExeSql(sql);
        }
        #endregion


        #region 4、根据变量ID获取OPC变量的值
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
        public string getVal(int id)
        {         
            try
            {
               
                L1tag curitem = listTag.Find(o => o.id == id);
                //根据扫描周期、数据类型，返回变量的值
                if (curitem.scanrate >= 1)
                {
                    if (curitem.datatype==1)
                    {                
                        return (Math.Round(Convert.ToDouble(KepGroup.OPCItems.GetOPCItem(curitem.itmHandleServer).Value), 2)).ToString();
                    }
                    else if (curitem.datatype == 0)
                    {                       
                        return (Convert.ToInt32(KepGroup.OPCItems.GetOPCItem(curitem.itmHandleServer).Value)).ToString();
                    }
                    else
                    {
                        return KepGroup.OPCItems.GetOPCItem(curitem.itmHandleServer).Value.ToString();
                    }

                }
                else
                {
                    if (curitem.datatype == 1)
                    {                                        
                        return (Math.Round(Convert.ToDouble(KepEventGroup.OPCItems.GetOPCItem(curitem.itmHandleServer).Value), 2)).ToString().ToString();
                    }
                    else if (curitem.datatype == 0)
                    {                    
                        return (Convert.ToInt32(KepEventGroup.OPCItems.GetOPCItem(curitem.itmHandleServer).Value)).ToString();
                    }
                    else 
                    {
                        return KepEventGroup.OPCItems.GetOPCItem(curitem.itmHandleServer).Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return "获取值时出错："+ex.ToString();
            }

           
        }
       


        #endregion


        #region 5、向变量中写入值
        /// <summary>
        /// 设置变量的值,(写入double类型)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="val"></param>
        public bool setVal(int id, double val)
        {
            try
            {
                L1tag curitem = listTag.Find(o => o.id == id);
                if (curitem.scanrate >= 1)
                {
                    KepGroup.OPCItems.GetOPCItem(curitem.itmHandleServer).Write(val);
                }
                else
                {
                    KepEventGroup.OPCItems.GetOPCItem(curitem.itmHandleServer).Write(val);
                }

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
                L1tag curitem = listTag.Find(o => o.id == id);
                if (curitem.scanrate >= 1)
                {
                    KepGroup.OPCItems.GetOPCItem(curitem.itmHandleServer).Write(val);
                }
                else
                {
                    KepEventGroup.OPCItems.GetOPCItem(curitem.itmHandleServer).Write(val);
                }
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
