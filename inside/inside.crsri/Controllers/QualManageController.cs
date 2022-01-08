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
    public class QualManageController : Controller
    {
        //
        // GET: /QualManage/

        public ActionResult Index(string id = null)
        {
            List<t_web_qualManage_model> list = new List<t_web_qualManage_model>();
            List<t_web_subject_model> QMSubject = qualManageDal.reqQualManageSubject();
            ViewBag.ManageSubject = QMSubject;
            string subjectID = id == null ? QMSubject.First().subjectID : id;
            ViewBag.QMSubjectID = subjectID;
            list = qualManageDal.reqQualByQMID(subjectID);
            return View(list);
        }

    }
}
