using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.sqlstringhelper
{
    public class jhxySqlhelper
    {
        public static string JHXYItemSqlString = string.Format("select * from JHXY order by id desc ");

        public static string JHXYMaxItemSqlString = string.Format("select top 1 * from JHXY order by id Desc");

        public static string JHXYSaveNewItem = string.Format("insert into JHXY([year],volume,total_volume,volume_name)values(@year,@volume,@total_volume,@volume_name)");
    }

}