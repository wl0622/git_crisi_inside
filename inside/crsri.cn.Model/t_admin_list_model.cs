using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crsri.cn.Model
{
    public class t_admin_list_model
    {
        public int id { get; set; }
        public string uname { get; set; }
        public string upwd { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public int roleid { get; set; }
        public DateTime regtime { get; set; }
        public bool isdisable { get; set; }

    }
}