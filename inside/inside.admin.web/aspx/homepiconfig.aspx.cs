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
    public partial class homepiconfig : basepage
    {
        public List<t_subject_list_model> list = new List<t_subject_list_model>();
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!this.havingRights)
            //{
            //    Response.Redirect("noRights.aspx");
            //}

            if (!IsPostBack)
            {
                EFHELP efhelp = new EFHELP();
                list = efhelp.QueryList<t_subject_list_model>(subjectSqlhelper.HomeSubjectPicConfigString);
            }
        }
    }
}