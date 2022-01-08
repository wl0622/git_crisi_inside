
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace inside.inside.admin.weblogin
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            //string bbbb = SecurityHelper.Md5("a123456A");

            if (Request.Params["logout"] != null)
            {
                HttpCookieCollection cookies = HttpContext.Current.Request.Cookies;
                foreach (string key in cookies.AllKeys)
                {
                    cookies[key].Expires = DateTime.Now.AddHours(-1);
                    HttpContext.Current.Response.Cookies.Set(cookies[key]);
                }
                FormsAuthentication.SignOut();
                HttpContext.Current.Response.Redirect("/login/login.aspx");
            }

        }

    }
}