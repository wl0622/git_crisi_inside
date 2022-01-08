using inside.admin.web.entityframework;
using inside.admin.web.entityframework.tableEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.ashx
{
    public class ashxHelper
    {
        public static List<t_department_list_model> getDepartment(string deptID = null)
        {
            try
            {
                EFHELP efhelp = new EFHELP();
                List<ExpressionModelField> fieldWhere = new List<ExpressionModelField>();

                if (deptID != null)
                {
                    fieldWhere.Add(new ExpressionModelField() { Name = "deptID", Value =deptID });
                }


                List<OrderModelField> orderField = new List<OrderModelField>();
                orderField.Add(new OrderModelField() { PropertyName = "deptID", IsDesc = false });

                return efhelp.GetList<t_department_list_model>(fieldWhere.ToArray());
            }
            catch
            {
                return null;
            }

        }
    }
}