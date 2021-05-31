using System;
using System.Collections.Generic;
using System.Text;
using OPCAutomation;
using System.Data;
using Rcw.Method;

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
                string sql = "select xuhao as id,L1name as name,scanrate,datatype from  L1OPC_TAG where used=1 and type=" + type + " order by id ";
                //DbMySql.GetDataTable(sql);
                //var dt = new sqlDbHelp().Query(sql);
                var dt = DbMySql.GetDataTable(sql);
                listTag = dt_to_list(dt);
            }
            catch (Exception ex)
            {
                SysLog.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "--loadValList方法加载变量列表时出错" + ex.ToString());
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
                    // KepServer.Connect("OPC.SimaticNET", "192.168.48.232");
                    KepServer.Connect("KEPware.KEPServerEx.V6", "192.168.48.233");
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
                    //KepServer.Connect("KEPware.KEPServerEx.V5", "192.168.48.232");
                    KepServer.Connect("KEPware.KEPServerEx.V6", "192.168.48.233");
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
                        myItem.ClientHandle = item.id;
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
                readval(ClientHandles.GetValue(i).ToString(), ItemValues.GetValue(i));
            }
        }
        public void readval(string i, object val)
        {
            try
            {
                #region 5#机大包剩钢事件
                if (i == "78")
                {
                    if (Convert.ToInt32(val) == 1)
                    {
                        ccm5dabaoshenggang.GetInstance().calData(1);
                    }
                    return;
                }
                if (i == "79")
                {
                    if (Convert.ToInt32(val) == 1)
                    {
                        ccm5dabaoshenggang.GetInstance().calData(0);
                    }
                    return;
                }
                #endregion

                #region 精炼炉电耗事件
                if (i == "1")
                {
                    lf1dianhao.GetInstance().calDianhao(Convert.ToInt32(val));
                    SysLog.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "----1#精炼炉电耗--变量：" + i + ";值：" + val.ToString());
                    return;
                }
                if (i == "3")
                {
                    lf2dianhao.GetInstance().calDianhao(Convert.ToInt32(val));
                    SysLog.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "----2#精炼炉电耗--变量：" + i + ";值：" + val.ToString());
                    return;
                }
                if (i == "5")
                {
                    lf3dianhao.GetInstance().calDianhao(Convert.ToInt32(val));
                    SysLog.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "----3#精炼炉电耗--变量：" + i + ";值：" + val.ToString());
                    return;
                }
                if (i == "7")
                {
                    lf4dianhao.GetInstance().calDianhao(Convert.ToInt32(val));
                    SysLog.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "----4#精炼炉电耗--变量：" + i + ";值：" + val.ToString());
                    return;
                }

                if (i == "9")
                {
                    lf5dianhao.GetInstance().calDianhao(Convert.ToInt32(val));
                    SysLog.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "----5#精炼炉电耗--变量：" + i + ";值：" + val.ToString());
                    return;
                }

                #endregion


                #region 3#机切割优化状态采集事件 （已不再使用）
                //3#机大包到达
                //if (i == "88")
                //{
                //    if (Convert.ToInt32(val) == 1)
                //    {
                //        ccm3cut.GetInstance().acceptCasterStatus(1);
                //    }
                //    return;
                //}
                ////3#机大包开浇
                //if (i == "89")
                //{
                //    if (Convert.ToInt32(val) == 1)
                //    {
                //        ccm3cut.GetInstance().acceptCasterStatus(2);
                //    }
                //    return;
                //}
                ////3#机大包停浇
                //if (i == "90")
                //{
                //    if (Convert.ToInt32(val) == 1)
                //    {
                //        ccm3cut.GetInstance().acceptCasterStatus(3);
                //    }
                //    return;
                //}

                ////3#机大包接缝状态
                //if (i == "91")
                //{
                //    if (Convert.ToInt32(val) == 1)
                //    {
                //        ccm3cut.GetInstance().acceptLadlefeng(Convert.ToInt32(val));
                //    }                    
                //    return;
                //}

                ////3#机1流切割状态
                //if (i == "92")
                //{
                //    ccm3cut.GetInstance().ccmCutStrand_1.acceptCutStatus(Convert.ToInt32(val));
                //    return;
                //}
                ////3#机1流开浇状态
                //if (i == "93")
                //{
                //    ccm3cut.GetInstance().ccmCutStrand_1.acceptStrandStatus(Convert.ToInt32(val));
                //    return;
                //}

                ////3#机2流切割状态
                //if (i == "98")
                //{
                //    ccm3cut.GetInstance().ccmCutStrand_2.acceptCutStatus(Convert.ToInt32(val));
                //    return;
                //}
                ////3#机2流开浇状态
                //if (i == "99")
                //{
                //    ccm3cut.GetInstance().ccmCutStrand_2.acceptStrandStatus(Convert.ToInt32(val));
                //    return;
                //}

                ////3#机3流切割状态
                //if (i == "104")
                //{
                //    ccm3cut.GetInstance().ccmCutStrand_3.acceptCutStatus(Convert.ToInt32(val));
                //    return;
                //}
                ////3#机3流开浇状态
                //if (i == "105")
                //{
                //    ccm3cut.GetInstance().ccmCutStrand_3.acceptStrandStatus(Convert.ToInt32(val));
                //    return;
                //}

                ////3#机4流切割状态
                //if (i == "110")
                //{
                //    ccm3cut.GetInstance().ccmCutStrand_4.acceptCutStatus(Convert.ToInt32(val));
                //    return;
                //}
                ////3#机4流开浇状态
                //if (i == "111")
                //{
                //    ccm3cut.GetInstance().ccmCutStrand_4.acceptStrandStatus(Convert.ToInt32(val));
                //    return;
                //}
                #endregion

                if (val == null)
                {
                    return;
                }
                //L1tag curitem = listTag.Find(o => o.id == Convert.ToInt16(i));
                //if (curitem.datatype == 0)
                //{
                //    savedata(Convert.ToInt16(i), Convert.ToInt32(val));
                //}
                //else
                //{
                //    savedata(Convert.ToInt16(i), Math.Round(Convert.ToDouble(val), 3));
                //}
                ////如果扫描周期小于1，按事件采集的，则不向数据库插入数据
                //if (curitem.scanrate < 1)
                //{
                //    return;
                //}
                //if ((DateTime.Now - curitem.lasttime).TotalSeconds >= curitem.scanrate)
                //{
                //    curitem.lasttime = DateTime.Now;
                //    if (curitem.datatype == 0)
                //    {
                //        savedata(Convert.ToInt16(i), Convert.ToInt32(val));
                //    }
                //    else
                //    {
                //        savedata(Convert.ToInt16(i), Math.Round(Convert.ToDouble(val), 3));
                //    }
                    
                //}
            }
            catch (Exception ex)
            {
                //throw new Exception("处理数据变化时出错：" + ex.ToString());
                SysLog.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "--变量："+i+";值："+val.ToString()+"readval方法处理数据变化时出错" + ex.ToString());
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
                    if (curitem.datatype==0)
                    {                
                        return (Math.Round(Convert.ToDouble(KepGroup.OPCItems.GetOPCItem(curitem.itmHandleServer).Value), 2)).ToString();
                    }
                    else if (curitem.datatype == 1)
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
                    if (curitem.datatype == 0)
                    {                                        
                        return (Math.Round(Convert.ToDouble(KepEventGroup.OPCItems.GetOPCItem(curitem.itmHandleServer).Value), 2)).ToString().ToString();
                    }
                    else if (curitem.datatype == 1)
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
                SysLog.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "--变量："+id+"；getVal方法获取值时出错" + ex.ToString());
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
