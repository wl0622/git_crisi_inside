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
    public class DownloadController : Controller
    {
        //
        // GET: /download/
        public ActionResult Index(string id = null)
        {
            List<t_web_subject_model> model = downloadDal.reqSubjectItem();
            ViewBag.subjectItems = model;
            ViewBag.sID = id;

            List<reqDownload> request = downloadDal.reqDownItems(id);

            return View(request);
        }

        public ActionResult Show(string ID)
        {
            t_web_downService_model model = new t_web_downService_model();
            int reqID = helper.UrlDecrypt(ID);
            List<t_web_downService_model> request = downloadDal.reqServiceContent(reqID);
            if (request.Count > 0)
            {
                model = request.First();
                articleDal.reqUpdatePV(reqID);
                string name = string.Empty;
                model.content = cjkxyUrlHelper.articleOldImgUrlReplace(model.content);//替换原网站图片路径
                model.content = cjkxyUrlHelper.articleOldLinkReplace(model.content);//替换原网站文件路径
                model.content = cjkxyUrlHelper.articleOldFileIconReplace(model.content);
            }


            return View(model);
        }

    }
}
