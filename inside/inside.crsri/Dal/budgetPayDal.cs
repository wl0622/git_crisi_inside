using crsri.cn.DbObject;
using crsri.cn.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.crsri.Dal
{
    public class budgetPayDal : baseDal
    {

        /// <summary>
        /// 分页获取栏目文稿列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="subjectID"></param>
        /// <returns></returns>
        public static List<t_web_article_model> reqArticleListOnPage(int pageIndex, int pageSize, out int totalCount)
        {
            totalCount = 0;
            List<t_web_article_model> list = null;
            List<ExpressionModelField> listWhere = new List<ExpressionModelField>();
            listWhere.Add(new ExpressionModelField() { Name = "specialID", Value = "026001" });
            listWhere.Add(new ExpressionModelField() { Name = "isDeleted", Value = false });
            listWhere.Add(new ExpressionModelField() { Name = "isPassed", Value = true });
            OrderModelField orderField = new OrderModelField() { PropertyName = "releaseTime", IsDesc = true };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic = CrsriEntityFramework.GetListPaged<t_web_article_model>(pageIndex, pageSize, listWhere.ToArray(), new[] { orderField });

            if (dic != null)
            {
                totalCount = (int)dic["total"];
                list = dic["rows"] as List<t_web_article_model>;
            }
            return list;
        }
    }
}