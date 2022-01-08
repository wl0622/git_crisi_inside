using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.tableEntity
{
    public class UserGroups_model
    {
        public Int16 userGroupID { get; set; }
        public string name { get; set; }
        public bool? isDeleted { get; set; }
        public Int16? orderID { get; set; }
    }
}