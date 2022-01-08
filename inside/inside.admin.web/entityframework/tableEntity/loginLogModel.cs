using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.tableEntity
{
    public class loginLogModel
    {
        public int id { get; set; }
        public string loginName { get; set; }
        public DateTime loginTime { get; set; }
        public string loginIp { get; set; }
    }
}