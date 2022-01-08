using crsri.cn.DbObject;
using crsri.cn.Model;
using inside.crsri.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace inside.crsri.Dal
{
    public class PostgradeduDal : baseDal
    {
        public static List<requestHomeArticleClass> requestPostgradItems()
        {
            List<requestHomeArticleClass> reqlist = new List<requestHomeArticleClass>();

            List<t_web_special_model> specialItems = requestPostgraduate();

            List<string> specialCollection = specialItems.Select(a => a.specialID).ToList();

            string sql = string.Format(@"select specialID as subjectID,specialName as subjectName,articleID,title,keywords,convert(varchar(5),releaseTime,10) as 'releaseTime',isTop  from (

            select b.specialID,b.specialName,articleID,title,keywords,releaseTime,isTop from (
	            SELECT *,ROW_NUMBER() OVER(PARTITION BY specialID ORDER BY isTop DESC,updateTime DESC) NUM FROM Article where  isDeleted=0 AND isPassed=1 and
								            specialID in('{0}')
	            ) as a   join [Special] as b on a.specialID=b.specialID where NUM<=10
	
            ) as b order by isTop desc,specialID asc, releaseTime desc", string.Join(",", specialCollection.ToArray()).Replace(",", "','"));

            DataTable dtSource = CrsriEntityFramework.QueryDataTable(sql);

            //按subjectID分组
            IEnumerable<IGrouping<string, DataRow>> result = dtSource.Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["subjectID"].ToString());
            foreach (IGrouping<string, DataRow> ig in result)
            {
                requestHomeArticleClass request = new requestHomeArticleClass();
                request.subjectID = ig.Key;
                request.subjectName = ig.First()["subjectName"].ToString();

                request.articleList = new List<HomeArticleClass>();
                foreach (var dr in ig)
                {
                    HomeArticleClass hac = new HomeArticleClass();
                    hac.articleID = Convert.ToInt32(dr["articleID"]);
                    hac.title = dr["title"].ToString().Trim();
                    hac.keywords = dr["keywords"].ToString();
                    hac.releaseTime = dr["releaseTime"].ToString();
                    hac.isTop = dr["isTop"] != null ? Convert.ToBoolean(dr["isTop"]) : false;

                    request.articleList.Add(hac);
                }

                reqlist.Add(request);
            }

            return reqlist;

        }


        public static t_web_postGraduate_model requestScore(string name, string number)
        {
            t_web_postGraduate_model reqModel = null;
            List<ExpressionModelField> field_And = new List<ExpressionModelField>();
            field_And.Add(new ExpressionModelField() { Name = "ksbh", Value = number });
            field_And.Add(new ExpressionModelField() { Name = "xm", Value = name });
            List<t_web_postGraduate_model> requestResult = CrsriEntityFramework.GetList<t_web_postGraduate_model>(field_And.ToArray(), new OrderModelField[] { });
            if (requestResult.Count > 0)
            {
                reqModel = requestResult.First();
            }
            return reqModel;
        }


        public static List<t_web_special_model> requestPostgraduate()
        {
            List<ExpressionModelField> field_And = new List<ExpressionModelField>();
            field_And.Add(new ExpressionModelField() { Name = "specialID", Value = "024", Relation = EnumRelation.Contains });
            return CrsriEntityFramework.GetList<t_web_special_model>(field_And.ToArray(), new OrderModelField[] { });
        }


        /// <summary>
        /// 分页获取专题栏目列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="subjectID"></param>
        /// <returns></returns>
        public static List<t_web_article_model> reqSpecialListOnPage(int pageIndex, int pageSize, string specialID, out int totalCount)
        {
            totalCount = 0;
            List<t_web_article_model> list = null;
            List<ExpressionModelField> listWhere = new List<ExpressionModelField>();
            listWhere.Add(new ExpressionModelField() { Name = "specialID", Value = specialID });
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