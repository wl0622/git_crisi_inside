using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.sqlstringhelper
{
    public class homePicConfigHelper
    {
        public static string updateHomePic = "update t_homePicConfig set picName=@picName where item=@item";

        public static string updateHomePicUrlConfig = "update t_homePicConfig set url=@url";

        public static string updatepicUrlConfig = "update t_homePicConfig set linkurl=@linkurl where item=@item";

        public static string delpicUrlConfig = "update t_homePicConfig set picName='nopic.png' where item=@item";


        public static string insertSitePic = "insert into siteCfgPic(isShortPic,picName,position) values(@isShortPic,@picName,@position)";
        public static string updateSiteLinkUrl = "update siteCfgPic set linkUrl=@linkUrl where id=@item";
        public static string delSitePic = "delete siteCfgPic where id=@item";
        public static string setTitleColor = "update siteCfgBase set itemVal=@itemVal where item='toutiaoColor'";
        public static string setTopColor = "update siteCfgBase set itemVal=@itemVal where item='topColor'";

    }
}