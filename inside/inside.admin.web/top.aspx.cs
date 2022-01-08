using inside.admin.web.aspx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace inside.admin.web
{
    public partial class top : basepage
    {
        public string loginName = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            loginName = this.curUserModel.userCnName;
        }
    }
}