using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.sqlstringhelper
{
    public class nvamenuSqlhelper
    {
        public static string addChildrenSql()
        {
            return string.Format(@"
                                   
                                   if exists(select * from t_navMenu where parentNodeId=@parentNode)
                                   begin
                                   insert into t_navMenu(nodeId,nodeName,parentNodeId,linkurl)
                                    select @parentNode+'00'+convert(varchar(2),count(*)+1),@nodeName,@parentNode,@linkurl from t_navMenu where parentNodeId=@parentNode group by parentNodeId
                                   end
                                   else
                                   insert into t_navMenu(nodeId,nodeName,parentNodeId,linkurl)
                                   values(@parentNode+'001',@nodeName,@parentNode,@linkurl)
                                    
                                  "
                );
        }

        public static string updateNavMenu()
        {
            return string.Format(@"update t_navMenu set nodeName=@nodeName,linkurl=@linkurl where nodeId=@nodeId");
        }

        public static string delNavMenu()
        {
            return string.Format(@"delete t_navMenu where nodeId=@nodeId");
        }
    }
}