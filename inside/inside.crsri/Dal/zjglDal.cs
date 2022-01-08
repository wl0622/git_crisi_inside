using crsri.cn.Model;
using inside.crsri.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.crsri.Dal
{
    public class zjglDal : baseDal
    {
        public static List<requestHomeArticleClass> requestList()
        {
            List<requestHomeArticleClass> reqlist = ztDal.requestList("028");
            return reqlist;
        }

        public static List<t_web_special_model> requestDjzcItems()
        {
            List<t_web_special_model> list = ztDal.requestDjzcItems("028");
            return list;
        }
    }
}