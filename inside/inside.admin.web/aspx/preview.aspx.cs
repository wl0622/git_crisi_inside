
using inside.admin.web.entityframework;
using inside.admin.web.entityframework.tableEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace inside.admin.web.aspx
{
    public partial class preview : basepage
    {

        public t_article_list_model model = null;
        public string auditButtonHtml = string.Empty;
        public string currentUserCnName = string.Empty;
        public string previewAuditHtml = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!this.havingRights)
            //{
            //    Response.Redirect("noRights.aspx");
            //}

            if (Request["articleID"] != null && Request["opt"] != null)
            {
                auditButtonHtml = Request["opt"].ToString().Trim() == "audit" ? "<img src='../css/preview/img/content_03.gif' />" : "";
                previewAuditHtml = Request["opt"].ToString().Trim() == "audit" ? "<img src='../images/dsh_max.png' />" : "";

                List<ExpressionModelField> fieldWhere = new List<ExpressionModelField>();
                fieldWhere.Add(new ExpressionModelField() { Name = "articleID", Value = Convert.ToInt32(Request["articleID"]) });
                EFHELP efhelp = new EFHELP();
                model = efhelp.GetList<t_article_list_model>(fieldWhere.ToArray()).First();
                currentUserCnName = this.curUserModel.userCnName;
            }
        }
    }
}