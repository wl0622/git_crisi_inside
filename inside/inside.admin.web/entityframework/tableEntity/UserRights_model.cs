using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.tableEntity
{
    public class UserRights_model
    {
        public int id { get; set; }
        public string name { get; set; }
        public string nodeId { get; set; }
        public string parentNodeId { get; set; }

    }
}