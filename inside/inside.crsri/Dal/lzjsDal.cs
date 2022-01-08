using crsri.cn.Model;
using inside.crsri.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.crsri.Dal
{
    public class lzjsDal : baseDal
    {

        public static List<requestHomeArticleClass> requestList()
        {
            List<requestHomeArticleClass> reqlist = ztDal.requestList("008");
            return reqlist;
        }

        public static List<t_web_article_model> requestPicXW()
        {
            List<t_web_article_model> list = new List<t_web_article_model>();

            try
            {
                string sql = string.Format("select top 5 * from Article where isspecialPicxw=1 and isDeleted=0 AND isPassed=1 and specialID like '008%' order by releaseTime asc");

                list = CrsriEntityFramework.QueryList<t_web_article_model>(sql);
            }
            catch
            {

            }

            return list;
        }

        public static List<t_web_special_model> requestDjzcItems()
        {
            List<t_web_special_model> list = ztDal.requestDjzcItems("008");
            return list;
        }
    }
}