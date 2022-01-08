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
    public class homeDal : baseDal
    {

        #region 首页包含的栏目

        public static List<requestHomeArticleClass> requestHomeArticle()
        {

            List<requestHomeArticleClass> reqlist = new List<requestHomeArticleClass>();



            string sql = string.Format(@"select subjectID,subjectName,articleID,title,keywords,convert(varchar(5),updateTime,10) as 'releaseTime',isTop  from (
                
					select b.subjectID,b.subjectName,articleID,title,keywords,updateTime,isTop from (

						SELECT *,ROW_NUMBER() OVER(PARTITION BY subjectID ORDER BY isTop DESC,updateTime DESC) NUM FROM Article where  isDeleted=0 AND isPassed=1 and
								subjectID in(select subjectID from [Subject] where subjectID like '002%'
								)

								) as a   join [Subject] as b on a.subjectID=b.subjectID where NUM<=12
	                            
								union all
	                            
								select repostSubjectID as subjectID,b.subjectName,articleID,title,keywords,updateTime,isTop from (

								SELECT *,ROW_NUMBER() OVER(PARTITION BY repostSubjectID ORDER BY isTop DESC,updateTime DESC) NUM FROM Article where  isDeleted=0 AND isPassed=1 and
								repostSubjectID in(select subjectID from [Subject] where subjectID like '002%')

								) as a   join [Subject] as b on a.subjectID=b.subjectID where NUM<=12
                            
                            ) as b order by isTop desc,subjectID asc, updateTime desc");

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

        #endregion

        #region 首页-产品与技术
        public static List<ProductTechnicalClass> reqProductTechnical()
        {
            List<ProductTechnicalClass> reqlist = new List<ProductTechnicalClass>();
            string subjectId = "017";
            string sql = string.Format(@"select TOP 5 articleID,subjectID,title,defaultPicUrl from Article  where subjectID='{0}' and isDeleted=0 and  isPassed=1 order by releaseTime desc", subjectId);
            DataTable dtSource = CrsriEntityFramework.QueryDataTable(sql);
            foreach (DataRow dr in dtSource.Rows)
            {
                ProductTechnicalClass pt = new ProductTechnicalClass();
                pt.articleID = Convert.ToInt32(dr["articleID"]);
                pt.subjectID = dr["subjectID"].ToString();
                pt.title = dr["title"].ToString();
                pt.defaultPicUrl = dr["defaultPicUrl"].ToString();
                reqlist.Add(pt);
            }

            return reqlist;
        }
        #endregion

        #region 首页-英文新闻
        public static requestHomeArticleClass reqHomeEnglishNewsArticle()
        {
            requestHomeArticleClass req = new requestHomeArticleClass();
            string sql = @"select TOP 6 subjectID,articleID,title,keywords,convert(varchar(5),releaseTime,10) as 'releaseTime' from Article where subjectID = '018003' and isDeleted=0 and isPassed=1  ORDER BY articleID DESC";
            DataTable dtSource = CrsriEntityFramework.QueryDataTable(sql);
            req.subjectID = dtSource.Rows[0].ToString();
            List<HomeArticleClass> list = new List<HomeArticleClass>();
            foreach (DataRow dr in dtSource.Rows)
            {
                HomeArticleClass hac = new HomeArticleClass();
                hac.articleID = Convert.ToInt32(dr["articleID"]);
                hac.title = dr["title"].ToString();
                hac.keywords = dr["keywords"].ToString();
                hac.releaseTime = dr["releaseTime"].ToString();
                list.Add(hac);
            }
            req.articleList = list;

            return req;
        }

        public static List<t_web_article_model> reqHomeEnglishNewsOnPage(int pageIndex, int pageSize, out int totalCount)
        {
            totalCount = 0;
            List<t_web_article_model> list = new List<t_web_article_model>();
            List<ExpressionModelField> listWhere = new List<ExpressionModelField>();
            listWhere.Add(new ExpressionModelField() { Name = "subjectID", Value = "007003" });
            listWhere.Add(new ExpressionModelField() { Name = "isPassed", Value = true });
            listWhere.Add(new ExpressionModelField() { Name = "isDeleted", Value = false });
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


        #endregion

        #region 首页-专题栏目
        public static List<requestHomeArticleClass> requestHomeSpecial()
        {
            List<requestHomeArticleClass> reqlist = new List<requestHomeArticleClass>();

            string sql = "select specialID,specialName,linkUrl from dbo.Special where isShowHome=1 order by showSN ASC";
            DataTable dtSource = CrsriEntityFramework.QueryDataTable(sql);
            List<reqSpecialClass> special = new List<reqSpecialClass>();
            List<string> exeSql = new List<string>();
            foreach (DataRow dr in dtSource.Rows)
            {
                special.Add(new reqSpecialClass() { specialID = dr["specialID"].ToString(), specialName = dr["specialName"].ToString() });

                exeSql.Add(string.Format(@"
                               SELECT * FROM (								
							   select top 8 subjectID,articleID,title,a.specialID,b.specialName 'keywords',convert(varchar(5),a.updateTime,10) as 'releaseTime',isTop from Article as a 
							   join Special as b on a.specialID =b.specialID
							   where a.specialID like '{0}%' order by articleID desc ) AS A ", dr["specialID"].ToString()));

            }
            sql = string.Join(" UNION ALL ", exeSql);
            dtSource = CrsriEntityFramework.QueryDataTable(sql);

            foreach (reqSpecialClass s in special)
            {
                requestHomeArticleClass request = new requestHomeArticleClass();
                request.subjectID = s.specialID;
                request.subjectName = s.specialName;
                request.articleList = new List<HomeArticleClass>();

                foreach (DataRow dr in dtSource.Rows)
                {
                    if (dr["specialID"].ToString().Substring(0, 3) == s.specialID)
                    {
                        HomeArticleClass hac = new HomeArticleClass();
                        hac.articleID = Convert.ToInt32(dr["articleID"]);
                        hac.title = dr["title"].ToString().Trim();
                        hac.keywords = dr["keywords"].ToString();
                        hac.releaseTime = dr["releaseTime"].ToString();
                        hac.isTop = dr["isTop"] != null ? Convert.ToBoolean(dr["isTop"]) : false;
                        request.articleList.Add(hac);
                    }
                }

                reqlist.Add(request);
            }

            return reqlist;
        }

        #endregion

        #region 首页-最近消息
        public static List<t_web_article_model> reqTop3NewArticle()
        {
            List<t_web_article_model> reqList = new List<t_web_article_model>();
            string sql = string.Format(@"select top 3 articleID,subjectID,title,releaseTime from Article where isDeleted=0 and isPassed=1  order by releaseTime DESC");
            DataTable dtSource = CrsriEntityFramework.QueryDataTable(sql);
            foreach (DataRow dr in dtSource.Rows)
            {
                t_web_article_model sc = new t_web_article_model();
                sc.articleID = Convert.ToInt32(dr["articleID"]);
                sc.subjectID = dr["subjectID"].ToString();
                sc.title = dr["title"].ToString();
                sc.releaseTime = Convert.ToDateTime(dr["releaseTime"]);
                reqList.Add(sc);
            }
            return reqList;
        }

        #endregion

        #region 首页-头条新闻
        public static t_web_article_model reqTouTiaoNewArticle()
        {
            t_web_article_model reqTouTiao = new t_web_article_model();
            string sql = string.Format(@"select top 1 * from Article where isOnTop=1 and isDeleted=0 and isPassed=1  order by releaseTime DESC");
            DataTable dtSource = CrsriEntityFramework.QueryDataTable(sql);
            foreach (DataRow dr in dtSource.Rows)
            {
                reqTouTiao.articleID = Convert.ToInt32(dr["articleID"]);
                reqTouTiao.subjectID = dr["subjectID"].ToString();
                reqTouTiao.title = dr["title"].ToString();
                reqTouTiao.content = dr["content"].ToString();
                reqTouTiao.titletoutiao = dr["titletoutiao"].ToString();
            }
            return reqTouTiao;
        }

        #endregion

        #region 首页-请求配置图片
        public static List<t_homePicConfig_list_model> reqPicConfig()
        {
            List<ExpressionModelField> fieldWhere = new List<ExpressionModelField>();
            fieldWhere.Add(new ExpressionModelField() { Name = "id", Value = 0, Relation = EnumRelation.GreaterThan });
            List<t_homePicConfig_list_model> list = CrsriEntityFramework.GetList<t_homePicConfig_list_model>(fieldWhere.ToArray());
            return list;
        }
        #endregion

        #region 首页 幻灯片

        public static List<t_web_article_model> reqPicxw()
        {
            List<t_web_article_model> list = new List<t_web_article_model>();

            try
            {
                string sql = string.Format("select top 5 * from Article where isPicxw=1 and isDeleted=0 and isPassed=1 and defaultPicUrl is not null and defaultPicUrl!='' order by articleID desc");

                list = CrsriEntityFramework.QueryList<t_web_article_model>(sql);
            }
            catch
            {

            }

            return list;
        }

        #endregion

        #region 首页-自定义列表

        public static List<t_web_article_customized_model> reqArticleCustomizedList()
        {
            try
            {
                List<ExpressionModelField> fieldWhere = new List<ExpressionModelField>();
                fieldWhere.Add(new ExpressionModelField() { Name = "id", Value = 0, Relation = EnumRelation.GreaterThan });
                List<t_web_article_customized_model> list = CrsriEntityFramework.GetList<t_web_article_customized_model>(fieldWhere.ToArray());
                return list;
            }
            catch
            {
                return new List<t_web_article_customized_model>();
            }


        }
        #endregion

        #region 首页-获取配置(黑色配色/新年背景)
        public static List<t_web_siteCfg_model> cfgList()
        {
            return CrsriEntityFramework.GetAll<t_web_siteCfg_model>();
        }
        #endregion

        #region 首页-通栏图片
        public  static List<t_web_siteCfgPic_model> cfgPicList()
        {
            return CrsriEntityFramework.GetAll<t_web_siteCfgPic_model>();
        }
        #endregion

        #region 首页-其它配置(头条字体颜色)
        public  static List<t_web_siteCfgBase_model> cfgBaseList()
        {
            return CrsriEntityFramework.GetAll<t_web_siteCfgBase_model>();
        }
        #endregion



        #region
        public static void reqAddCounter()
        {
            try
            {
                CrsriEntityFramework.ExecuteSql("update dbo.[Counter] set [counter]=[counter]+1 where shuoming='newWeb'");
            }
            catch
            { }

        }

        #endregion

    }
}