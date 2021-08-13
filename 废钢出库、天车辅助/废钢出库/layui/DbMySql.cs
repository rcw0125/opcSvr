using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace layui
{
    public class DbMySql
    {

        static string ConnectionString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};Allow User Variables=True", "192.168.36.9", "lgdb", "root", "root");

        public static System.Data.IDbConnection GetDbConnection()
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            return conn;
        }
        public static DataTable GetDataTable(string loadSql, params object[] args)
        {
            try
            {
                MySqlConnection conn = GetDbConnection() as MySqlConnection;
                conn.Open();
                MySqlCommand selectCmd = conn.CreateCommand();
                //loadSql = loadSql.Replace("@", ":");
                selectCmd.CommandText = loadSql;
                //selectCmd.BindByName = true;

                //2019-12-23 屏蔽下面带参数 @会出现异常
                //int i = 0;
                //foreach (var item in ParseParameters(loadSql))
                //{
                //    selectCmd.Parameters.Add(item, args[i++]);
                //}
                MySqlDataAdapter oradap = new MySqlDataAdapter(selectCmd);
                DataSet ds = new DataSet();
                oradap.Fill(ds);
                conn.Close();
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("出现异常" + ex.ToString());
            }

        }

        public static void ExeSql(string sql)
        {
            try
            {
                MySqlConnection conn = GetDbConnection() as MySqlConnection;
                MySqlCommand cmd = conn.CreateCommand();
                conn.Open();
                MySqlTransaction trans = conn.BeginTransaction();
                cmd.Transaction = trans;
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                trans.Commit();
                conn.Close();
            }
            catch
            { 
            
            }
           
            
           
           
        }
    }
}