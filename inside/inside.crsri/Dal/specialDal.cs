using crsri.cn.DbObject;
using crsri.cn.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.crsri.Dal
{
    public class specialDal : baseDal
    {

        public static List<t_web_special_model> requestSpecialItems()
        {
            return CrsriEntityFramework.GetAll<t_web_special_model>();
        }

    }
}