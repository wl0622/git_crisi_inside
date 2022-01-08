
using inside.admin.web.entityframework.tableEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace inside.admin.web.aspx
{
    public partial class userright : basepage
    {

        public List<UserRights_model> UserRights_list = new List<UserRights_model>();
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!this.havingRights)
            //{
            //    Response.Redirect("noRights.aspx");
            //}

            if (!IsPostBack)
            {
                //EFHELP efhelp = new EFHELP();
                //List<ExpressionModelField> fieldWhere = new List<ExpressionModelField>();
                //fieldWhere.Add(new ExpressionModelField() { Name = "id", Value = 0, Relation = EnumRelation.GreaterThan });
                //UserRights_list = efhelp.GetList<UserRights_model>(fieldWhere.ToArray());
            }
        }
    }
}