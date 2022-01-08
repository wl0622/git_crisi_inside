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
    public class ArticleController : Controller
    {
        //
        // GET: /Article/

        public ActionResult Index(string ID)
        {
            int reqID = helper.UrlDecrypt(ID);
            List<t_web_subject_model> articleList = articleDal.requestSubjectItems();
            //ViewBag.SubjectItems = articleList;
            t_web_article_model model = articleDal.reqArticle(reqID);
            articleDal.reqUpdatePV(reqID);
            ViewBag.CurrentSubjectName = model.subjectID == "017" ? "产品与技术" : model.subjectID.Substring(0,3) == "005" ? "质量管理" : articleList.Find(a => a.subjectID == model.subjectID).subjectName;
            model.content = Regex.Replace(model.content, "(<img[\\s\\S]+?)src=([\"'])(?!(https?://))([^\"']+)", string.Format("$1src=$2{0}$4", helper.sitePicPath), RegexOptions.Compiled | RegexOptions.IgnoreCase);
            model.content = Regex.Replace(model.content, "(<a[\\s\\S]+?)href=([\"'])(?!(https?://))([^\"']+)", string.Format("$1href=$2{0}$4", helper.sitePicPath), RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return View(model);
        }

        public ActionResult list(string ID, int pageindex = 1)
        {
            List<t_web_subject_model> articleList = ID.Substring(0, 3) == "005" ? articleDal.requestSubjectQualityItems() : articleDal.requestSubjectItems();
            ViewBag.SubjectItems = articleList;
            ViewBag.currentPage = pageindex;
            ViewBag.subjectName = articleDal.reqSubjectNameBySubjectID(ID);
            int totalCount = 0;
            List<t_web_article_model> articleItems = articleDal.reqArticleListOnPage(pageindex,15, ID, out totalCount);
            ViewBag.totalCount = totalCount;
            return View(articleItems);
        }

    }
}
