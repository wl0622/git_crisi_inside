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
    public class EnNewsController : Controller
    {
        //
        // GET: /EnNews/

        public ActionResult Index(string ID)
        {
            int reqID = helper.UrlDecrypt(ID);
            List<t_web_subject_model> articleList = articleDal.requestSubjectItems();
            t_web_article_model model = articleDal.reqArticle(reqID);
            articleDal.reqUpdatePV(reqID);
            ViewBag.CurrentSubjectName = "English News";
            model.content = cjkxyUrlHelper.articleOldImgUrlReplace(model.content);//替换原网站图片路径
            model.content = cjkxyUrlHelper.articleOldLinkReplace(model.content);//替换原网站文件路径
            model.content = cjkxyUrlHelper.articleOldFileIconReplace(model.content);
            return View(model);
        }

        public ActionResult list(string ID, int pageindex = 1)
        {
            ViewBag.currentPage = pageindex;
            ViewBag.subjectName = articleDal.reqSubjectNameBySubjectID(ID);
            int totalCount = 0;
            List<t_web_article_model> articleItems = articleDal.reqArticleListOnPage(pageindex, 15, ID, out totalCount);
            ViewBag.totalCount = totalCount;
            return View(articleItems);
        }

    }
}
