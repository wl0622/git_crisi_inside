using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.model
{
    public class userpaginatorModel
    {
        public string userName { get; set; }
        public string userCnName { get; set; }
        public string deptID { get; set; }
        public string userGroupID { get; set; }

    }

    public class loginLogpaginatorModel
    {
        public string loginName { get; set; }
        public string loginTime { get; set; }
        public string loginIp { get; set; }
    }

}