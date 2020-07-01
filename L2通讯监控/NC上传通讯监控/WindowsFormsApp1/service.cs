using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Text;

namespace L2Ctr
{
  
    public class Service1
    {
        public Service1()
        {

            //如果使用设计的组件，请取消注释以下行 
            //InitializeComponent(); 
            gkconnstr = ConfigurationManager.AppSettings["conn"];
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




      

     
        public DataSet Query( string SqlStr)
        {

          
            DataSet myDataSet = new DataSet();
            try
            {
                OracleConnection conn = null;
                try
                {
                    conn = new OracleConnection(gkconnstr);
                    OracleDataAdapter adapter = new OracleDataAdapter();

                    //conn.Open();

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
            }
            catch
            {

            }

          
            return myDataSet;
        }

       



    }



}
