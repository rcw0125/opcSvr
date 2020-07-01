using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RestartApp
{
    public class dbsb
    {
        [DisplayName("DB用户名")]
        public string oracle_username { get; set; }
        [DisplayName("锁模式")]
        public string sms { get; set; }
        [DisplayName("DB对象所有者")]
        public string owner { get; set; }
        [DisplayName("DB对象名")]
        public string object_name { get; set; }
        [DisplayName("DB对象类型")]
        public string object_type { get; set; }
        [DisplayName("OS机器名")]
        public string machine { get; set; }
        [DisplayName("IP地址")]
        public string client_info { get; set; }
        [DisplayName("OS程序名")]
        public string program { get; set; }
        [DisplayName("SQL文")]

        public string sql_text { get; set; }
        [DisplayName("DB结束会话")]
        public string jshh { get; set; }
        [DisplayName("OS结束进程")]
        public string jsjc { get; set; }

        [DisplayName("时间")]
        public string c_ts { get; set; }

    }
}
