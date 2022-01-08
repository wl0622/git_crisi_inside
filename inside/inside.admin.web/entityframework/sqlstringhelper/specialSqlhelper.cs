using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.sqlstringhelper
{
    public class specialSqlhelper
    {
        public static string addSpecialItem(string specId, string specItemName)
        {
            return string.Format(@"
                declare @spID smallint
                declare @specialID varchar(10)

                select @spID=MAX(spID)+1 from Special

                select @specialID= COUNT(*)+1 from dbo.Special where specialID like '{0}%' and len(specialID)>3
                if len(@specialID)=1
                  set @specialID='{0}'+'00'+@specialID
                  else 
                  set @specialID='{0}'+'0'+@specialID

                
                --初始设置专题
                insert into Special(spID,specialID,specialName,linkUrl,english)
                values(@spID,@specialID,'{1}','',0)
            ", specId, specItemName);
        }

        public static string updateSpecialItem(string specId, string specItemName)
        {
            return string.Format(@"update Special set specialName='{0}' where specialID='{1}'", specItemName, specId);
        }

        public static string delSpecialItem()
        {
            return string.Format("delete Special where specialID=@specId");
        }
    }
}