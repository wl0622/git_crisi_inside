
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.sqlstringhelper
{
    public class userSqlHelper
    {
        public static string updateUserInfo()
        {
            return string.Format("update Users set deptID=@deptID,userGroupID=@userGroupID,userName=@userName,userCnName=@userCnName,UserEmail=@UserEmail where userID=@userID");
        }

        public static string resetPassword()
        {
            return string.Format("update Users set userPassword=@userPassword where userID=@userID");
        }

        public static string delUser()
        {
            return string.Format("delete Users where userID=@userID");
        }

        public static string delUnlock()
        {
            return string.Format("delete t_errorLoginRecord where id=@id");
        }

        public static string updatePwd()
        {
            return string.Format("if exists(select * from Users where userID=@userID and userPassword=@oldUserPassword) update Users set userPassword=@userPassword where userID=@userID");
        }

        public static string resetGroupsRights(int groupsid)
        {
            return string.Format("delete UserGroupsRights where groupsid={0}", groupsid);
        }

        public static string setUserRights(int groupsid, string rightsid)
        {
            return string.Format(@"insert into dbo.UserGroupsRights(groupsid,rightsid)values({0},'{1}')", groupsid, rightsid);
        }


        public static string getUserRights()
        {
            return string.Format("select * from UserGroupsRights where groupsid=@groupsid");
        }

        public static string getGroupRight()
        {
            return string.Format(@"select nodeId as 'id',parentNodeId as 'pid',name,Convert(bit,ISNULL(b.rightsid,0)) as 'checked',Convert(bit,1) as 'open' 
            from dbo.UserRights as a left join (select * from UserGroupsRights where groupsid=@groupid) as b on a.nodeId=b.rightsid order by id asc,pid asc");
        }
    }
}