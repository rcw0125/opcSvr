using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SBGL
{
    public class Database
    {
        //public static SqlSugarClient GetInstance()
        //{
        //    SqlSugarClient db = new SqlSugarClient(new ConnectionConfig() { ConnectionString = Config.ConnectionString, DbType = DbType.MySql, IsAutoCloseConnection = true });
        //    db.Ado.IsEnableLogEvent = true;
        //    //db.Ado.LogEventStarting = (sql, pars) =>
        //    //{
        //    //    Console.WriteLine(sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
        //    //    Console.WriteLine();
        //    //};
        //    return db;
        //}
    }

    public class Config
    {
        public static string ConnectionString = "server=192.168.48.20;Database=lgsb;Uid=root;Pwd=root";
      
    }
}
