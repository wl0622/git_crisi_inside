using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework
{
    public class SQLString
    {
        public static string defaultDBString
        {
                //get { return @"server=127.0.0.1,1433;database=cjslkjw;uid=sa;pwd=db@Crsri#1028"; }
            get { return @"server=127.0.0.1,1433;database=CRSRIWEB20211107;uid=sa;pwd=sa"; }
        }

        public static string outSideDBString
        {
            get { return @"server=10.6.66.114,1433;database=cjslkjw;uid=sa;pwd=db@Crsri#1028"; }
        }

    }
}