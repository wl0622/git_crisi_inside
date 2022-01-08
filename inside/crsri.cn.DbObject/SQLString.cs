using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crsri.cn.DbObject
{
    public class SQLString
    {
        public static string defaultDBString
        {
            //get { return @"server=127.0.0.1,1433;database=demo;uid=sa;pwd=sa"; }
            //get { return @"database=CRSRIWEB;server=127.0.0.1,1433;uid=sa;pwd=db@Crsri#1028"; }
            get { return @"server=127.0.0.1,1433;database=CRSRIWEB20211107;uid=sa;pwd=sa"; }
        }

    }
}