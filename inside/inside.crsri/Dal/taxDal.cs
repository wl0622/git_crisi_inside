using inside.crsri.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace inside.crsri.Dal
{
    public class taxDal : baseDal
    {

        public static requestHomeArticleClass reqTax()
        {
            requestHomeArticleClass req = new requestHomeArticleClass();
            string sql = @"select articleID,title,keywords,convert(varchar(5),releaseTime,10) as 'releaseTime' from Article where specialID like '035%' and isDeleted=0 and isPassed=1  ORDER BY releaseTime DESC";
            DataTable dtSource = CrsriEntityFramework.QueryDataTable(sql);
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
    }
}