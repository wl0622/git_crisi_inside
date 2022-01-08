
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
    public partial class userlist : basepage
    {
        public List<UserGroups_model> userGroupsList = new List<UserGroups_model>();
      
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                EFHELP efhelp = new EFHELP();

                List<ExpressionModelField> fieldWhere = new List<ExpressionModelField>();
                fieldWhere.Add(new ExpressionModelField() { Name = "userGroupID", Value =Convert.ToInt16(0), Relation = EnumRelation.GreaterThan });
                userGroupsList = efhelp.GetList<UserGroups_model>(fieldWhere.ToArray());


            }
        }
    }
}