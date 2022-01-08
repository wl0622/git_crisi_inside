using crsri.cn.Model;
using inside.crsri.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.crsri.Dal
{
    public class aqscDal : baseDal
    {
        public static List<requestHomeArticleClass> requestList()
        {
            List<requestHomeArticleClass> reqlist = ztDal.requestList("007");
            return reqlist;
        }

        public static List<t_web_special_model> requestDjzcItems()
        {
            List<t_web_special_model> list = ztDal.requestDjzcItems("007");
            list.RemoveAll(a => a.specialID == "007004");
            return list;
        }
    }
}