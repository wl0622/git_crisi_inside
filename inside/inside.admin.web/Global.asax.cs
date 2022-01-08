using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace inside.admin.web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

            HttpRequest req = HttpContext.Current.Request;


            if (req.CurrentExecutionFilePathExtension.Equals(".aspx") || req.CurrentExecutionFilePathExtension.Equals(".ashx"))
            {
                if (req.FilePath != "/ashx/ValidateCode.ashx" && req.FilePath != "/ashx/login.ashx" && req.FilePath != "/login/login.aspx" && req.FilePath != "/main.aspx")
                {
                    if (HttpContext.Current.Request.Cookies[".INSIDEAUTH"] == null)
                    {
                        Response.Write("<script>this.parent.location='/html/timeout.html';self.close();</script>");
                    }
                }
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Exception exception = Server.GetLastError();
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}