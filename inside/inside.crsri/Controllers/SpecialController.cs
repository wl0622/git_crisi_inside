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
    public class SpecialController : Controller
    {
        //
        // GET: /Special/

        public ActionResult show(string ID)
        {

            int reqID = helper.UrlDecrypt(ID);
            List<t_web_special_model> specialList = inside.crsri.Dal.specialDal.requestSpecialItems();
            t_web_article_model model = articleDal.reqArticle(reqID);
            articleDal.reqUpdatePV(reqID);
            ViewBag.specialItem = specialList.Find(a => a.specialID == model.specialID);
            model.content = cjkxyUrlHelper.articleOldImgUrlReplace(model.content);//替换原网站图片路径
            model.content = cjkxyUrlHelper.articleOldLinkReplace(model.content);//替换原网站文件路径
            model.content = cjkxyUrlHelper.articleOldFileIconReplace(model.content);
            return View(model);
        }

        public ActionResult list()
        {
            List<t_web_special_model> specialList = inside.crsri.Dal.specialDal.requestSpecialItems().FindAll(a => a.specialID.Length == 3);
            return View(specialList);
        }

    }
}
