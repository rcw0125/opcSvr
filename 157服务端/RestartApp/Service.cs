using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Text;

namespace RestartApp
{

    public class Service
    {
        public Service()
        {

            //如果使用设计的组件，请取消注释以下行 
            //InitializeComponent(); 
            gkconnstr = "data source=XGMES117;password=xgmes;user id=xgmes;";
            gkconn.ConnectionString = gkconnstr;

        }

        private string gkconnstr;
        private OracleConnection gkconn = new OracleConnection();


        public OracleTransaction gktrans;


        public bool NewTrans()
        {
            try
            {
                if (gkconn.State == ConnectionState.Closed)
                {
                    gkconn.Open();
                    gktrans = gkconn.BeginTransaction();
                }

                return true;
            }
            catch (Exception e)
            {
                if (gkconn.State == ConnectionState.Open)
                {
                    gktrans.Rollback();
                    gkconn.Close();
                }
                return false;
            }
        }

        public bool CommitTrans()
        {
            try
            {
                gktrans.Commit();
                gkconn.Close();
                return true;
            }
            catch (Exception e)
            {
                if (gkconn != null)
                    gktrans.Rollback();
                gkconn.Close();
                return false;
            }
        }

        public bool RollbackTrans()
        {
            try
            {
                gktrans.Rollback();
                gkconn.Close();
                return true;
            }
            catch (Exception ex)
            {
                if (gkconn.State == ConnectionState.Open)
                {
                    gkconn.Close();
                }
                return false;
            }
        }

  
        public string dmldata(string strSql)
        {
            bool bMark = false;
            try
            {
                OracleCommand myCommand = new OracleCommand(strSql);
                //if (gkconn.State == ConnectionState.Closed)
                //{
                //    gkconn.Open();
                //    gktrans = gkconn.BeginTransaction();
                //    bMark = true;
                //}
                myCommand.Connection = gkconn;
                myCommand.Transaction = gktrans;
                myCommand.ExecuteNonQuery();
                //if (bMark == true)
                //{
                //    gktrans.Commit();
                //    gkconn.Close();
                //}
                return "0";
            }
            catch (Exception e)
            {
                //if (bMark == true)
                //{
                //    gktrans.Rollback();
                //    gkconn.Close();
                //}
                return e.Message;
            }
        }



   
        public DataSet Query(string SqlStr)
        {

            string connstr;
            connstr = gkconnstr;
            DataSet myDataSet = new DataSet();
            OracleConnection conn = new OracleConnection(connstr);
            try
            {
                conn.Open();
            }
            catch
            {
                return null;
            }
           
            try
            {           
                OracleDataAdapter adapter = new OracleDataAdapter();
                adapter.SelectCommand = new OracleCommand(SqlStr, conn);
                adapter.Fill(myDataSet);

                conn.Close();
            }
            catch (Exception e)
            {
                conn.Close();
                string cc = e.Message;
                return null;

            }
            return myDataSet;
        }

  
        public string Update(string SqlStr)
        {
            ////连接数据库
            string connstr;
            connstr = gkconnstr;
            OracleConnection conn = new OracleConnection(connstr);
            conn.Open();
            OracleTransaction mytrans = conn.BeginTransaction();
            try
            {
                OracleCommand myCommand = new OracleCommand(SqlStr);
                myCommand.Connection = conn;
                myCommand.Transaction = mytrans;
                myCommand.ExecuteNonQuery();
                mytrans.Commit();
                conn.Close();
            }
            catch (Exception e)
            {
                mytrans.Rollback();
                conn.Close();
                return e.Message;
            }
            return "0";

        }



      


    }

}
