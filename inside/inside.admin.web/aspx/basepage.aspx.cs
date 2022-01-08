using admin.web.model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace admin.web.aspx
{
    public partial class basepage : System.Web.UI.Page
    {

        protected UsersModel curUserModel
        {
            get;
            set;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }
        }
    }
}



<%--    <script type="text/javascript">
        var jQuery_general = jQuery.noConflict();
    </script>--%>

