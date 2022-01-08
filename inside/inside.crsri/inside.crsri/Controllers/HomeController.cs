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
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {

            List<requestHomeArticleClass> article = null;
            try
            {
                article = homeDal.requestHomeArticle();//栏目
                ViewBag.productTechnical = homeDal.reqProductTechnical();//产品技术
                ViewBag.englishNews = homeDal.reqHomeEnglishNewsArticle();//英文新闻
                ViewBag.specialList = homeDal.requestHomeSpecial();//专题
                ViewBag.topNews = homeDal.reqTop3NewArticle();//最新资讯
                t_web_article_model m = homeDal.reqTouTiaoNewArticle();//头条
                string title = System.Web.HttpUtility.HtmlDecode(m.title);
                m.titletoutiao = m.titletoutiao != "" ? m.titletoutiao : m.title;
                ViewBag.toutiao = m;
                ViewBag.picxw = homeDal.reqPicxw();//图片新闻
                ViewBag.siteCfg = homeDal.cfgList();
                ViewBag.siteCfgPic = homeDal.cfgPicList();
                ViewBag.siteCfgBase = homeDal.cfgBaseList();
                homeDal.reqAddCounter();
            }
            catch
            {

            }
            return View(article);

        }

    }
}
