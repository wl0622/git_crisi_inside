using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.sqlstringhelper
{
    public class siteSqlhelper
    {
        public static string setIsBlack()
        {
            return string.Format("update siteCfg set cfgVal=@isBlack where cfgName='isBlack'");
        }

        public static string setIsNewYearBg()
        {
            return string.Format("update siteCfg set cfgVal=@isNewYearBg where cfgName='isNewYearBg'");
        }

        public static string setIsTopBold()
        {
            return string.Format("update siteCfg set cfgVal=@isTopBold where cfgName='isTopBold'");
        }

        public static string setIsOnTopBold()
        {
            return string.Format("update siteCfg set cfgVal=@isOnTopBold where cfgName='isOnTopBold'");
        }
    }
}