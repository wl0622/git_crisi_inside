using crsri.cn.DbObject;
using crsri.cn.Model;
using inside.crsri.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.crsri.Dal
{
    public class jhxyDal : baseDal
    {
        public static List<t_web_jhxy_model> requestJHXYItems()
        {
            List<OrderModelField> orderList = new List<OrderModelField>();
            OrderModelField orderField = new OrderModelField() { PropertyName = "id", IsDesc = true };
            orderList.Add(orderField);
            return CrsriEntityFramework.GetAll<t_web_jhxy_model>(orderList.ToArray());
        }

        public static List<t_web_article_model> requestArticle(string id,int pageIndex, int pageSize,out int totalCount)
        {
            totalCount = 0;
            List<t_web_article_model> list = null;
            List<ExpressionModelField> field_And = new List<ExpressionModelField>();
            field_And.Add(new ExpressionModelField() { Name = "specialID", Value = "021" });
            field_And.Add(new ExpressionModelField() { Name = "isDeleted", Value = Convert.ToBoolean(false) });
            field_And.Add(new ExpressionModelField() { Name = "isPassed", Value = Convert.ToBoolean(true) });
            field_And.Add(new ExpressionModelField() { Name = "JHXYid", Value = id });

            List<OrderModelField> orderby = new List<OrderModelField>();
            orderby.Add(new OrderModelField() { PropertyName = "articleID", IsDesc = false });
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic = CrsriEntityFramework.GetListPaged<t_web_article_model>(pageIndex, pageSize, field_And.ToArray(), orderby.ToArray());
            if (dic != null)
            {
                totalCount = (int)dic["total"];
                list = dic["rows"] as List<t_web_article_model>;
            }

            return list;
        }
    }
}