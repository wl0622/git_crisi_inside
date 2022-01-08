using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.tableEntity
{
    public class t_department_list_model
	{
        public string deptID { get; set; }

        public string deptName { get; set; }
        public string briefName { get; set; }
        public string englishName { get; set; }
        public Int16? child { get; set; }
        public string dID { get; set; }
        public Int16? orderID { get; set; }
        public int? value { get; set; }

        public int DepartmentID { get; set; }
	}
}