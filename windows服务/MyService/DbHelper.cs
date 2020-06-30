using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Security.Cryptography;
using System.Text;

namespace MyService
{
    /// <summary>
    /// Copyright (C) Maticsoft
    /// 数据访问基础类(基于Oracle)
    /// 可以用户可以修改满足自己项目的需要。
    /// </summary>
    public abstract class DbHelper
    {
        //数据库连接字符串(web.config来配置)，可以动态更改connectionString支持多数据库.	
        // public static string connectionString = Decrypt(ConfigurationManager.ConnectionStrings["xgcap"].ConnectionString);
        public static string connectionString = "Data Source = 192.168.48.117/XGMES; User Id = XGMES; Password =XGMES;";
        //public static string connectionString = "Data Source = 192.168.2.3/XGMES; User Id = XGMES; Password =xgmes;";
        //public static string connectionString = ConfigurationManager.ConnectionStrings["xgcap"].ConnectionString;
        public DbHelper()
        {
        }



        #region  执行简单SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (OracleException E)
                    {
                        connection.Close();
                        throw new Exception(E.Message);
                    }
                }
            }
        }



       

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    OracleDataAdapter command = new OracleDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (OracleException ex)
                {
                    connection.Close();
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }
        #endregion

      


    }
}
