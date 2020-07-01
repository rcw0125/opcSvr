using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace layui.Models
{
    public class tree
    {
        public string id { get; set; }
        public string title { get; set; }

        public string checkArr { get; set; }

        public string parentId { get; set; }
    }
}