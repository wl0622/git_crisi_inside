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
    public class JhxyController : Controller
    {
        //
        // GET: /Jhxy/

        public ActionResult Index(string id = "0", int pageIndex = 1)
        {
            List<t_web_jhxy_model> listItems = inside.crsri.Dal.jhxyDal.requestJHXYItems();
            ViewBag.listItems = listItems;

            t_web_jhxy_model curItems = id == "0" ? listItems.First() : listItems.FindAll(a => a.id == Convert.ToInt16(id)).First();
            ViewBag.jhxyItemsCur = curItems;

            int totalCount = 0;
            List<t_web_article_model> articleList = jhxyDal.requestArticle(curItems.id.ToString(), pageIndex, 25, out totalCount);
            ViewBag.currentPage = pageIndex;
            ViewBag.totalCount = totalCount;

            return View(articleList);
        }

        public ActionResult xu()
        {
            return View();
        }

        public ActionResult ckc()
        {
            return View();
        }

        public ActionResult tz()
        {
            return View();
        }

        public ActionResult hc()
        {
            return View();
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
