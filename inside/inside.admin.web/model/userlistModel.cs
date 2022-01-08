using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.model
{
    public class userlistModel
    {
        public Int16 userID { get; set; }
        public string deptID { get; set; }
        public Int16? userGroupID { get; set; }
        public string userName { get; set; }
        public string userCnName { get; set; }
        public string userEmail { get; set; }
        public DateTime? addDate { get; set; }
        public DateTime? lastlogin { get; set; }
        public string userLastIP { get; set; }
        public string deptName { get; set; }
        public string userGroupName { get; set; }
    }
}