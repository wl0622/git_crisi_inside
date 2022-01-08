using crsri.cn.Model;
using inside.crsri.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace inside.crsri.Dal
{
    public class DjzcDal : baseDal
    {

        public static List<requestHomeArticleClass> requestList()
        {
            List<requestHomeArticleClass> reqlist = ztDal.requestList("033");
            return reqlist;
        }

        public static List<t_web_article_model> requestPicXW()
        {
            List<t_web_article_model> list = new List<t_web_article_model>();

            try
            {
                string sql = string.Format("select top 8 * from Article where isspecialPicxw=1 and isDeleted=0 AND isPassed=1 and specialID like '033%' order by releaseTime asc");

                list = CrsriEntityFramework.QueryList<t_web_article_model>(sql);
            }
            catch
            {

            }

            return list;
        }


        public static List<t_web_special_model> requestDjzcItems()
        {
            List<t_web_special_model> list = ztDal.requestDjzcItems("033");
            list.RemoveAll(a => a.specialID == "033007" || a.specialID == "033001");
            return list;
        }
    }
}