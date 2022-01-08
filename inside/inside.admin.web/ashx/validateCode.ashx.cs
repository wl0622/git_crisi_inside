using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace inside.admin.web.ashx
{
    /// <summary>
    /// Summary description for ValidateCode
    /// </summary>
    public class ValidateCode : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "image/Jpeg";

            var request = context.Request;
            if (request["method"] != null)
            {
                string method = request["method"];
                if (method.Equals("validateCode"))
                {
                    //要输出的格式
                    string code = ValidateCodeHelp.CreateValidateCode(5);
                    context.Session["validateCode"] = code;
                    byte[] bytes = ValidateCodeHelp.CreateValidateGraphic(code);

                    context.Response.ClearContent();
                    context.Response.BinaryWrite(bytes);
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}