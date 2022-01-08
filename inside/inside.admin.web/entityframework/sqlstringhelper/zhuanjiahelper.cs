using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.sqlstringhelper
{
    public class zhuanjiahelper
    {
        public static string update()
        {
            return string.Format(@"update zhuanjia set name=@name,zhuanye=@zhuanye,zhicheng=@zhicheng,xuewei=@xuewei,EditorInCharge=@EditorInCharge,brief=@brief,isPassed=0 where zhuanjiaID=@zhuanjiaID and isDeleted=0");
        }

        public static string delById()
        {
            return string.Format(@"update zhuanjia  set isDeleted=1  where zhuanjiaID=@zhuanjiaID and isDeleted=0");
        }

        public static string chkById()
        {
            return string.Format(@"update zhuanjia  set isPassed=1  where zhuanjiaID=@zhuanjiaID and isDeleted=0");
        }


        public static string add()
        {
            return string.Format(@"INSERT INTO [cjslkjw].[dbo].[zhuanjia]
           (
            [zhuanjiaID]
           ,[name]
           ,[brief]
           ,[zhuanye]
           ,[zhicheng]
           ,[xuewei]
           ,[isDeleted]
           ,[hits]
           ,[isPassed]
           ,[EditorInCharge])
           VALUES
           (@zhuanjiaID,@name,@brief,@zhuanye,@zhicheng,@xuewei,0,0,0,@EditorInCharge)  select @@IDENTITY");
        }

        public static string getzhuanjiaID()
        {
            return string.Format("select max(zhuanjiaID) from zhuanjia where isDeleted=0");
        }

        public static string updatephotoname()
        {
            return string.Format("update zhuanjia set photoname=@photoname where zhuanjiaID=@zhuanjiaID and isDeleted=0");
        }
    }
}