using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.tableEntity
{
    public class t_special_list_model
    {

        public Int16 spID { get; set; }
        public string specialID { get; set; }
        public string specialName { get; set; }
        public Int16? child { get; set; }
        public string linkUrl { get; set; }
        public bool? isElite { get; set; }
        public int? orderID { get; set; }
        public int? layoutID { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool? english { get; set; }

    }
}