
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace admin.web.aspx
{
    public partial class kindeditornews : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["kindeditorVal"] != null)
            {
                string newsTitle = Request.Params["txtTitle"].ToString();//新闻标题
                string content = content = Request.Params["kindeditorVal"].ToString();


                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("title", newsTitle);
                dic.Add("content", content);
                string errorMsg = string.Empty;
                if (errorMsg == string.Empty)
                {

                }
            }
        }
    }
}