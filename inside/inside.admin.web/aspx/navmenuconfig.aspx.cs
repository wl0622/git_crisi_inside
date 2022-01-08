
using inside.admin.web.entityframework;
using inside.admin.web.entityframework.mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace inside.admin.web.aspx
{
    public partial class navmenuconfig : basepage
    {
        public List<t_navmenu_list_model> sortlist = new List<t_navmenu_list_model>();
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!this.havingRights)
            //{
            //    Response.Redirect("noRights.aspx");
            //}

            if (!IsPostBack)
            {
                EFHELP efhelp = new EFHELP();
                List<ExpressionModelField> fieldWhere = new List<ExpressionModelField>();
                fieldWhere.Add(new ExpressionModelField() { Name = "id", Value = 0, Relation = EnumRelation.GreaterThan });
                List<t_navmenu_list_model> list = efhelp.GetList<t_navmenu_list_model>(fieldWhere.ToArray());
                sortlist = new List<t_navmenu_list_model>();
                //遍历一级菜单
                list.FindAll(a => a.nodeId.Length == 3).ForEach(delegate(t_navmenu_list_model parent)
                {
                    List<t_navmenu_list_model> children = list.FindAll(b => b.parentNodeId == parent.nodeId);
                    sortlist.Add(parent);
                    children.ForEach(delegate(t_navmenu_list_model c)
                       {
                           sortlist.Add(c);
                       });


                });
            }
        }
    }
}