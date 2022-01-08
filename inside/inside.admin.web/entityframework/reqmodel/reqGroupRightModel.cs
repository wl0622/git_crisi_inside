using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.reqmodel
{
    public class reqGroupRightModel
    {
        public string id { get; set; }
        public string pId { get; set; }
        public string name { get; set; }
        public bool Checked { get; set; }
        public bool open { get; set; }
    }
}