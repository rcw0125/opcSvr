using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace netcomm
{
    public class DbHelp
    {
        public DbHelp(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }
        public DbHelp(string host, string db, string user, string password)
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
    }
}
