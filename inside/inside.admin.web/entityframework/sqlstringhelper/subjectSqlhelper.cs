using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.sqlstringhelper
{
    public class subjectSqlhelper
    {
        public static string HomeSubjectSqlString = string.Format("select * from dbo.[Subject] where (layoutID=3 and subjectID like '002%' and len(subjectID)=6)  or subjectID='017'");


        public static string HomeSubjectPicConfigString = string.Format("select * from Subject where subjectID in('002004','002005','002006')");



    }
}