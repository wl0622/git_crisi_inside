using inside.crsri.Dal;
using crsri.cn.Model;
using inside.crsri.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace inside.crsri.Controllers
{
    public class DisplayLayoutController : Controller
    {

        public ActionResult GetRightFloatPic()
        {
            return PartialView("_FloatPartial");
        }

        public ActionResult GetTitleStyle(string name = "")
        {
            ViewBag.title = name;
            return PartialView("_HomeStylePartial");
        }

        public ActionResult GetFloatImage()
        {
            List<t_homePicConfig_list_model> reqModel = partialDal.requestPageFloatImages();
            return PartialView("_FloatPartial", reqModel);
        }

        public ActionResult GetCounter()
        {
            ViewBag.Counter = partialDal.requestWebCounter();
            return PartialView("_FooterWebInfoPartial");
        }

    }
}
