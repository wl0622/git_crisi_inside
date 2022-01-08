using crsri.cn.DbObject;
using crsri.cn.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.crsri.Dal
{
    public class qualManageDal : baseDal
    {
        public static List<t_web_subject_model> reqQualManageSubject()
        {
            string sql = @"select * from Subject where subjectID like '005%' and LEN(subjectID)=6 and subjectID<> 005002 order by CONVERT(int,subjectID) asc";

            return CrsriEntityFramework.QueryList<t_web_subject_model>(sql);
        }

        public static List<t_web_qualManage_model> reqQualByQMID(string subjectID)
        {
            List<ExpressionModelField> listWhere = new List<ExpressionModelField>();
            listWhere.Add(new ExpressionModelField() { Name = "subjectID", Value = subjectID });
            listWhere.Add(new ExpressionModelField() { Name = "isDeleted", Value = false });
            List<OrderModelField> orderField = new List<OrderModelField>();
            orderField.Add(new OrderModelField() { PropertyName = "updateTime", IsDesc = true });
            return CrsriEntityFramework.GetList<t_web_qualManage_model>(listWhere.ToArray(), orderField.ToArray());
        }

    }
}