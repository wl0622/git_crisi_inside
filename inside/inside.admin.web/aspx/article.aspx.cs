using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace admin.web.aspx
{
    public partial class article : basepage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.havingRights)
            {
                Response.Redirect("noRights.aspx");
            }


            if (IsPostBack)
            {
                if (Request.Params["editorValue"] != null || Request.Params["ueditorValue"] != null)
                {
                    string subjectName = Request.Params["cmbSubject"].ToString();//所属栏目
                    string specialName = Request.Params["cmbSpecial"].ToString();//所属专题
                    string newsTitle = Request.Params["txtTitle"].ToString();//新闻标题
                    string picTitle = Request.Params["txtPicTitle"].ToString();//图片新闻标题
                    string topNewsTitle = Request.Params["txtTopNewsTitle"].ToString();//头条新闻标题
                    string keyword = Request.Params["txtKeyword"].ToString();//关键字
                    string author = Request.Params["txtAuthor"].ToString();//作者
                    string articleOrigin = Request.Params["cmbArticleOrigin"].ToString();//院内稿件来源
                    bool isTransferNews = Request.Params["ckIsTransferNews"] != null ? true : false; //是否转载新闻
                    string transferNewsOrigin = Request.Params["txtTransferNewsOrigin"].ToString();//转载新闻稿件来源
                    string transferNewsLink = Request.Params["txtTransferNewsLink"].ToString();//转载新闻稿件链接
                    string transferNewsDate = Request.Params["txtTransferNewsDate"].ToString();//转载新闻发布时间
                    string edit = Request.Params["txtEdit"].ToString();//编辑
                    string editCompany = Request.Params["txtEditCompany"].ToString();//编辑单位
                    string picSort = Request.Params["txtPicSort"].ToString();//图片排序
                    string content = string.Empty;//文稿内容
                    if (Request.Params["editorValue"] != null) { content = Request.Params["editorValue"].ToString(); }
                    else if (Request.Params["ueditorValue"] != null) { content = Request.Params["ueditorValue"].ToString(); }
                }
            }

        }


    }
}