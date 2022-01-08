using inside.admin.web.entityframework;
using inside.admin.web.entityframework.sqlstringhelper;
using inside.admin.web.entityframework.tableEntity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace inside.admin.web.aspx
{
    public partial class articlelist : basepage
    {

        public List<t_subject_list_model> subjectlist = new List<t_subject_list_model>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!this.havingRights)
            //{
            //    Response.Redirect("noRights.aspx");
            //}

            if (!IsPostBack)
            {
                EFHELP efhelp = new EFHELP();
                //获取首页栏目
                subjectlist = efhelp.QueryList<t_subject_list_model>(subjectSqlhelper.HomeSubjectSqlString);
                subjectlist.Add(new t_subject_list_model() { subjectID = "005", subjectName = "产品与技术", child = 0, linkUrl = null, isElite = false, orderID = 0, layoutID = 1 });
            }
        }
    }
}