using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.tableEntity
{
    public class t_subject_list_model
    {
        public Int16 sID { get; set; }
        public string subjectID { get; set; }
        public string subjectName { get; set; }
        public Int16? child { get; set; }
        public string linkUrl { get; set; }
        public bool? isElite { get; set; }
        public Int16? orderID { get; set; }
        public Int16? layoutID { get; set; }

    }
}