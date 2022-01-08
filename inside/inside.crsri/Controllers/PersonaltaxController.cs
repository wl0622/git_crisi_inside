using crsri.cn.Model;
using inside.crsri.Dal;
using inside.crsri.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace inside.crsri.Controllers
{
    public class PersonaltaxController : Controller
    {

        public ActionResult Index()
        {

            requestHomeArticleClass mode = taxDal.reqTax();
            return View(mode);
        }

        public ActionResult Show(string ID)
        {
            int reqID = helper.UrlDecrypt(ID);
            t_web_article_model model = articleDal.reqArticle(reqID);
            articleDal.reqUpdatePV(reqID);
            string name = string.Empty;
            model.content = cjkxyUrlHelper.articleOldImgUrlReplace(model.content);//替换原网站图片路径
            model.content = cjkxyUrlHelper.articleOldLinkReplace(model.content);//替换原网站文件路径
            model.content = cjkxyUrlHelper.articleOldFileIconReplace(model.content);
            return View(model);
        }
    }
}
