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
            string name = string.Empty;
            if (model.subjectID != null)
            {
                if (model.subjectID == "017")
                {
                    name = "产品与技术";
                }
                else if (model.subjectID.Substring(0, 3) == "005")
                {
                    name = "质量管理";
                }
                else
                {
                    var list = articleList.Find(a => a.subjectID == model.subjectID);

                    if (list.subjectName != null)
                    {
                        name = list.subjectName;
                    }
                }
            }

            if (name == string.Empty)
            {
                if (model.specialID != null)
                {
                    if (model.specialID.Length > 3)
                    {
                        if (model.specialID.Substring(0, 3) == "038")
                        {
                            name = "经济发展调研心得体会";
                        }
                    }
                }
            }
            ViewBag.CurrentSubjectName = name;

            model.content = cjkxyUrlHelper.articleOldImgUrlReplace(model.content);//替换原网站图片路径
            model.content = cjkxyUrlHelper.articleOldLinkReplace(model.content);//替换原网站文件路径
            model.content = cjkxyUrlHelper.articleOldFileIconReplace(model.content);

            ViewBag.siteCfgPic = homeDal.cfgPicList();
            return View(model);
        }

        public ActionResult list(string ID, int pageindex = 1)
        {
            List<t_web_subject_model> articleList = ID.Substring(0, 3) == "005" ? articleDal.requestSubjectQualityItems() : articleDal.requestSubjectItems();
            ViewBag.SubjectItems = articleList;
            ViewBag.currentPage = pageindex;
            ViewBag.subjectName = articleDal.reqSubjectNameBySubjectID(ID);
            ViewBag.statistics = articleDal.reqStatistics();
            int totalCount = 0;
            List<t_web_article_model> articleItems = articleDal.reqArticleListOnPage(pageindex,28, ID, out totalCount);
            ViewBag.totalCount = totalCount;
            return View(articleItems);
        }

        [ValidateInput(false)]
        [inside.crsri.Filters.XSSFilter]
        public ActionResult SearchResult(string keyword, int pageindex = 1)
        {
            ViewBag.currentPage = pageindex;
            ViewBag.keyWords = keyword;
            int totalCount = 0;
            List<t_web_article_model> articleItems = articleDal.reqSearchResultListOnPage(pageindex, 15, keyword, out totalCount);
            ViewBag.totalCount = totalCount;
            return View(articleItems);

        }

    }
}
