
using inside.admin.web.entityframework.tableEntity;
using inside.admin.web.model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.ashx
{
    /// <summary>
    /// Summary description for department
    /// </summary>
    public class department : IHttpHandler
    {


        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            if (request["method"] != null)
            {
                string method = request["method"];
                if (method.Equals("getDepartmentComboTree"))
                {
                    string valueColumn = request["valueColumn"].ToString();
                    List<reqComboTreeModel> comboTree = new List<reqComboTreeModel>();

                    List<t_department_list_model> list = ashxHelper.getDepartment();

                    List<t_department_list_model> parentNode = list.FindAll(a => a.dID.Length == 3);

                    foreach (t_department_list_model m in parentNode)
                    {
                        reqComboTreeModel t = new reqComboTreeModel();

                        t.text = m.deptName;
                        t.id = m.dID;

                        List<t_department_list_model> children = list.FindAll(a => a.dID.Substring(0, 3) == m.dID && a.dID.Length > 3);
                        if (children.Count > 0)
                        {
                            t.children = new List<ComboTreeModel>();

                            foreach (t_department_list_model c in children)
                            {
                                ComboTreeModel sct = new ComboTreeModel();
                                if (valueColumn == "briefName")
                                {
                                    sct = new ComboTreeModel() { id = c.briefName, text = c.briefName };
                                }
                                else if (valueColumn == "deptID")
                                {
                                    sct = new ComboTreeModel() { id = c.deptID.ToString(), text = c.briefName };
                                }

                                t.children.Add(sct);
                            }
                        }
                        comboTree.Add(t);
                    }
                    comboTree = comboTree.OrderBy(a => a.id).ToList();
                    String rtnJsonString = JsonConvert.SerializeObject(comboTree);
                    context.Response.Write(rtnJsonString);
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}