using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.sqlstringhelper
{
    public class chengguohelper
    {
        public static string update()
        {
            return string.Format(@"update chengguo set huojiangname=@huojiangname,xiangmuname=@xiangmuname,huojiangdengji=@huojiangdengji,leibie=@leibie,dept=@dept,canyudept=@canyudept,huojiangniandai=@huojiangniandai,huojiangrenyuan=@huojiangrenyuan,chengguojj=@chengguojj where chengguoID=@chengguoID");
        }

        public static string delById()
        {
            return string.Format(@"delete chengguo where chengguoID=@chengguoID");
        }
    }
}