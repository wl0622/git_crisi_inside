
using inside.admin.web.entityframework.reqmodel;
using inside.admin.web.model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace inside.admin.web.aspx
{
    public class basepage : System.Web.UI.Page
    {
        public UsersModel curUserModel { get; set; }
        protected List<string> rightsId { get; set; }
        public basepage()
        {
            if (HttpContext.Current.Request.Cookies[".INSIDEAUTH"] != null)
            {
                //获取用户信息
                string jsonString = FormsAuthentication.Decrypt(HttpContext.Current.Request.Cookies[".INSIDEAUTH"].Value).Name.ToString();
                reqUserSessionInfoModel user = JsonConvert.DeserializeObject<reqUserSessionInfoModel>(jsonString);
                curUserModel = user.uInfo;

                //获取用户的权限
                if (user.uRightsID.Contains(","))
                {
                    rightsId = user.uRightsID.Split(',').ToList();
                }
                else
                {
                    if (user.uRightsID.Length > 0)
                    {
                        rightsId = new List<string>();
                        rightsId.Add(user.uRightsID);
                    }
                }


            }
            else
            {
                HttpCookieCollection cookies = HttpContext.Current.Request.Cookies;
                foreach (string key in cookies.AllKeys)
                {
                    cookies[key].Expires = DateTime.Now.AddHours(-1);
                    HttpContext.Current.Response.Cookies.Set(cookies[key]);
                }
                FormsAuthentication.SignOut();
            }
        }
    }
}
