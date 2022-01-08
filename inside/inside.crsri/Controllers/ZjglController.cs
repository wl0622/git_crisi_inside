using crsri.cn.Model;
using inside.crsri.Dal;
using inside.crsri.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace inside.crsri.Controllers
{
    public class ZjglController : Controller
    {
        //
        // GET: /Zjgl/

        public ActionResult Index()
        {
            List<requestHomeArticleClass> article = zjglDal.requestList();
            return View(article);
        }
        public ActionResult show(string ID)
        {
            int reqID = helper.UrlDecrypt(ID);
            t_web_article_model model = articleDal.reqArticle(reqID);
            articleDal.reqUpdatePV(reqID);
            string name = string.Empty;
            model.content = cjkxyUrlHelper.articleOldImgUrlReplace(model.content);//替换原网站图片路径
            model.content = cjkxyUrlHelper.articleOldLinkReplace(model.content);//替换原网站文件路径
            model.content = cjkxyUrlHelper.articleOldFileIconReplace(model.content);
            List<t_web_special_model> specialItems = zjglDal.requestDjzcItems().FindAll(a => a.specialID == model.specialID);
            ViewBag.itemTit = specialItems.Count > 0 ? specialItems.First().specialName : "";
            return View(model);
        }
    }
}
