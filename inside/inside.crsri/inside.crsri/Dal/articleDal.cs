using crsri.cn.DbObject;
using crsri.cn.Model;
using inside.crsri.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace inside.crsri.Dal
{
    public class articleDal : baseDal
    {

        #region 栏目

        /// <summary>
        /// 获取栏目(首页栏目00200)
        /// </summary>
        /// <returns></returns>
        public static List<t_web_subject_model> requestSubjectItems()
        {
            List<ExpressionModelField> field_And = new List<ExpressionModelField>();
            field_And.Add(new ExpressionModelField() { Name = "subjectID", Value = "0020", Relation = EnumRelation.Contains });
            field_And.Add(new ExpressionModelField() { Name = "child", Value = Convert.ToInt16(0) });
            field_And.Add(new ExpressionModelField() { Name = "orderID", Value = Convert.ToInt16(0), Relation = EnumRelation.GreaterThan });

            List<OrderModelField> orderby = new List<OrderModelField>();
            orderby.Add(new OrderModelField() { PropertyName = "orderID", IsDesc = false });
            List<t_web_subject_model> reqlist = CrsriEntityFramework.GetList<t_web_subject_model>(field_And.ToArray(), orderby.ToArray());
            return reqlist;
        }

        /// <summary>
        /// 获取质量管理栏目
        /// </summary>
        /// <returns></returns>
        public static List<t_web_subject_model> requestSubjectQualityItems()
        {
            List<ExpressionModelField> field_And = new List<ExpressionModelField>();
            field_And.Add(new ExpressionModelField() { Name = "subjectID", Value = "0050", Relation = EnumRelation.Contains });
            field_And.Add(new ExpressionModelField() { Name = "child", Value = Convert.ToInt16(0) });
            List<OrderModelField> orderby = new List<OrderModelField>();
            orderby.Add(new OrderModelField() { PropertyName = "subjectID", IsDesc = false });
            List<t_web_subject_model> reqlist = CrsriEntityFramework.GetList<t_web_subject_model>(field_And.ToArray(), orderby.ToArray());
            reqlist.RemoveAll((item) => item.subjectName.Contains("加密"));
            return reqlist;
        }

        /// <summary>
        /// 获取文稿内容
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        public static t_web_article_model reqArticle(int articleID)
        {
            List<ExpressionModelField> field_And = new List<ExpressionModelField>();
            field_And.Add(new ExpressionModelField() { Name = "articleID", Value = articleID });
            return CrsriEntityFramework.GetList<t_web_article_model>(field_And.ToArray(), new OrderModelField[] { }).First();
        }

        /// <summary>
        /// 获取栏目标题列表集合
        /// </summary>
        /// <param name="specialID"></param>
        /// <returns></returns>
        public static List<reqSpecialClass> reqTitleBySpecialID(string specialID, string sizeCount)
        {
            List<reqSpecialClass> list = new List<reqSpecialClass>();
            try
            {
                string sql = string.Format(@"

                            select a.articleID, a.specialID,title,keywords,convert(varchar(5),releaseTime,10) as 'releaseTime' from (

                            SELECT *,ROW_NUMBER() OVER(PARTITION BY specialID ORDER BY isTop DESC,releaseTime DESC) NUM FROM Article where  isDeleted=0 AND 
                            isPassed=1 and specialID in(select specialID from [Special] where specialID like '{0}%' and specialID<>'{0}')

                            ) as a  where NUM<={1}", specialID, sizeCount);
                list = CrsriEntityFramework.QueryList<reqSpecialClass>(sql);
            }
            catch
            {

            }
            return list;
        }

        /// <summary>
        /// 浏览数
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        public static bool reqUpdatePV(int articleID)
        {
            try
            {
                string sql = string.Format("update Article set hits=hits+1 where articleID=@articleID");
                List<SqlParameter> para = new List<SqlParameter>();
                para.Add(new SqlParameter() { ParameterName = "@articleID", Value = articleID });
                if (CrsriEntityFramework.ExecuteSql(sql, para.ToArray()) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 分页获取栏目文稿列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="subjectID"></param>
        /// <returns></returns>
        public static List<t_web_article_model> reqArticleListOnPage(int pageIndex, int pageSize, string subjectID, out int totalCount)
        {
            totalCount = 0;
            List<t_web_article_model> list = null;
            List<ExpressionModelField> listWhere = new List<ExpressionModelField>();
            listWhere.Add(new ExpressionModelField() { Name = "subjectID", Value = subjectID });
            listWhere.Add(new ExpressionModelField() { Name = "isDeleted", Value = false });
            listWhere.Add(new ExpressionModelField() { Name = "isPassed", Value = true });
            OrderModelField orderField = new OrderModelField() { PropertyName = "articleID", IsDesc = true };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic = CrsriEntityFramework.GetListPaged<t_web_article_model>(pageIndex, pageSize, listWhere.ToArray(), new[] { orderField });

            if (dic != null)
            {
                totalCount = (int)dic["total"];
                list = dic["rows"] as List<t_web_article_model>;
            }
            return list;
        }


        /// <summary>
        /// 分页获取查询结果
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="subjectID"></param>
        /// <returns></returns>
        public static List<t_web_article_model> reqSearchResultListOnPage(int pageIndex, int pageSize, string keyWord, out int totalCount)
        {
            totalCount = 0;
            List<t_web_article_model> list = null;
            List<ExpressionModelField> listWhere = new List<ExpressionModelField>();
            listWhere.Add(new ExpressionModelField() { Name = "keywords", Value = keyWord, Relation = EnumRelation.Contains });
            listWhere.Add(new ExpressionModelField() { Name = "isDeleted", Value = false });
            OrderModelField orderField = new OrderModelField() { PropertyName = "articleID", IsDesc = true };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic = CrsriEntityFramework.GetListPaged<t_web_article_model>(pageIndex, pageSize, listWhere.ToArray(), new[] { orderField });

            if (dic != null)
            {
                totalCount = (int)dic["total"];
                list = dic["rows"] as List<t_web_article_model>;
            }
            return list;
        }

        /// <summary>
        /// 获取栏目名称
        /// </summary>
        /// <param name="subjectID"></param>
        /// <returns></returns>
        public static string reqSubjectNameBySubjectID(string subjectID)
        {
            List<ExpressionModelField> field_And = new List<ExpressionModelField>();
            field_And.Add(new ExpressionModelField() { Name = "subjectID", Value = subjectID });
            return CrsriEntityFramework.GetList<t_web_subject_model>(field_And.ToArray(), new OrderModelField[] { }).First().subjectName;
        }



        #endregion
    }
}