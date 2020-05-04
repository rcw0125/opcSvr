using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
namespace opcBase
{
    public class sqlDbHelp
    {
        public sqlDbHelp()
        {
            SetConn("192.168.36.162", "cmsdb", "sa", "xgmes");
        }
        public sqlDbHelp(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }
        public sqlDbHelp(string host, string db, string user, string password)
        {

            this.ConnectionString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}", host, db, user, password);
        }
        public void SetConn(string host, string db, string user, string password)
        {
            this.ConnectionString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}", host, db, user, password);
        }
       
        public string ConnectionString = "";
        public void ExeSql(string sql)
        {
            SqlConnection conn = (SqlConnection)this.GetDbConnection();
            conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            SqlCommand cmd = conn.CreateCommand();
            cmd.Transaction = trans;
            cmd.CommandText = sql;

            cmd.ExecuteNonQuery();
            trans.Commit();
            conn.Close();
        }
        public SqlConnection GetDbConnection()
        {
            SqlConnection conn = new SqlConnection(this.ConnectionString);
            return conn;
        }

        public DataTable Query(string loadSql)
        {
            try
            {
                SqlConnection conn = this.GetDbConnection() as SqlConnection;
                conn.Open();
                SqlCommand selectCmd = conn.CreateCommand();
                selectCmd.CommandText = loadSql;
                SqlDataAdapter oradap = new SqlDataAdapter(selectCmd);
                DataSet ds = new DataSet();
                oradap.Fill(ds);
                conn.Close();
                return ds.Tables[0];
            }
            catch
            {
                return null;
            }

        }
    }
}
