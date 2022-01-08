using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.model
{
    public class UsersModel
    {
        public Int16 userID { get; set; }
        public string deptID { get; set; }
        public Int16? userGroupID { get; set; }
        public string userName { get; set; }
        public string userCnName { get; set; }
        public string userEmail { get; set; }
        public string userPassword { get; set; }
        public string Sex { get; set; }
        public DateTime? addDate { get; set; }
        public DateTime? lastlogin { get; set; }
        public string userLastIP { get; set; }
        public Int16? userLevel { get; set; }
        public Int16? Purview { get; set; }
        public Int16? layoutID { get; set; }
        public Int16? article { get; set; }
        public int? logins { get; set; }
        public Int16? lockuser { get; set; }
        public Int16? articlechecked { get; set; }
    }
}