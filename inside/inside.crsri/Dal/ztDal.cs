using crsri.cn.DbObject;
using crsri.cn.Model;
using inside.crsri.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace inside.crsri.Dal
{
    public class ztDal : baseDal
    {
        public static List<requestHomeArticleClass> requestList(string specialID, string number = "12")
        {
            List<requestHomeArticleClass> reqlist = new List<requestHomeArticleClass>();
            List<SqlParameter> para = new List<SqlParameter>();

            string sql = @"
                            select * from (

						    SELECT subjectID,specialID,articleID,title,keywords,releaseTime,isTop,ROW_NUMBER() OVER(PARTITION BY specialID ORDER BY isTop DESC,releaseTime DESC) NUM FROM Article 
                                      where  isDeleted=0 AND isPassed=1 and specialID in(select specialID from [Special] where specialID like @specialID+'%')
								    )as a  join [Special] as b on a.specialID=b.specialID where NUM<=@number order by a.specialID asc";

            para.Add(new SqlParameter() { ParameterName = "@specialID", Value = specialID });
            para.Add(new SqlParameter() { ParameterName = "@number", Value = number });

            DataTable dtSource = CrsriEntityFramework.QueryDataTable(sql, para.ToArray());

            //按subjectID分组
            IEnumerable<IGrouping<string, DataRow>> result = dtSource.Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["specialID"].ToString());

            foreach (IGrouping<string, DataRow> ig in result)
            {
                requestHomeArticleClass request = new requestHomeArticleClass();
                request.subjectID = ig.Key;
                request.articleList = new List<HomeArticleClass>();
                foreach (var dr in ig)
                {
                    HomeArticleClass hac = new HomeArticleClass();
                    hac.articleID = Convert.ToInt32(dr["articleID"]);
                    hac.title = dr["title"].ToString().Trim();
                    hac.keywords = dr["keywords"].ToString();
                    hac.releaseTime = Convert.ToDateTime(dr["releaseTime"]).ToString("yyyy-MM-dd");
                    hac.isTop = dr["isTop"] != null ? Convert.ToBoolean(dr["isTop"]) : false;

                    request.articleList.Add(hac);
                }

                reqlist.Add(request);
            }

            return reqlist;

        }



        /// <summary>
        /// 分页获取专题标题列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="subjectID"></param>
        /// <returns></returns>
        public static List<t_web_article_model> reqZtTitleOnPage(int pageIndex, int pageSize, string specialID, out int totalCount)
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



        public static List<t_web_special_model> requestDjzcItems(string specialID)
        {
            List<SqlParameter> para = new List<SqlParameter>();
            para.Add(new SqlParameter() { ParameterName = "@specialID", Value = specialID });
            string sql = "select * from [Special] where specialID like @specialID+'%'";
            return CrsriEntityFramework.QueryList<t_web_special_model>(sql, para.ToArray());

        }
    }
}