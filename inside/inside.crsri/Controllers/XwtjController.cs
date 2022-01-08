using inside.crsri.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace inside.crsri.Controllers
{
    public class XwtjController : Controller
    {
        //
        // GET: /Xwtj/


        public ActionResult Index()
        {
            string startDate = Request.Params["startDate"] == null ? DateTime.Now.Year.ToString() + "-01-01" : Request.Params["startDate"].ToString();
            string endDate = Request.Params["endDate"] == null ? DateTime.Now.Year.ToString() + "-12-31" : Request.Params["endDate"].ToString(); ;
            List<statisticsModel> list = inside.crsri.Dal.articleDal.reqStatistics(startDate, endDate);
            return View(list);
        }

    }
}
