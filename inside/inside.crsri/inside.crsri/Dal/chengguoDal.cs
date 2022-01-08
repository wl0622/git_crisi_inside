using crsri.cn.DbObject;
using crsri.cn.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace inside.crsri.Dal
{
    public class chengguoDal : baseDal
    {


        #region 走进长科院-获奖成果

        public static List<reqChengGuoClass> requestChengGuo()
        {
            List<reqChengGuoClass> reqlist = new List<reqChengGuoClass>();

            string sql = string.Format(@"

                  select chengguoID,huojiangname,xiangmuname,huojiangdengji,huojiangniandai from dbo.chengguo where isDeleted=0  order by  huojiangniandai desc");

            DataTable dtSource = CrsriEntityFramework.QueryDataTable(sql);

            //按subjectID分组
            IEnumerable<IGrouping<string, DataRow>> result = dtSource.Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["huojiangniandai"].ToString());

            foreach (IGrouping<string, DataRow> ig in result)
            {
                reqChengGuoClass request = new reqChengGuoClass();
                request.year = ig.Key;
                request.list = new List<chengguoClass>();

                foreach (var dr in ig)
                {
                    chengguoClass hac = new chengguoClass();
                    hac.chengguoID = Convert.ToInt32(dr["chengguoID"]);
                    hac.huojiangname = dr["huojiangname"].ToString().Trim();
                    hac.xiangmuname = dr["xiangmuname"].ToString();
                    hac.huojiangdengji = dr["huojiangdengji"].ToString();
                    request.list.Add(hac);
                }

                reqlist.Add(request);
            }
            return reqlist;
        }


        public static chengguoClass requestChengguoClass(int id)
        {


            List<ExpressionModelField> field_And = new List<ExpressionModelField>();
            field_And.Add(new ExpressionModelField() { Name = "chengguoID", Value = id });
            List<chengguoClass> reqlist = CrsriEntityFramework.GetList<chengguoClass>(field_And.ToArray(), new OrderModelField[] { });
            if (reqlist.Count > 0)
            {
                return reqlist.First();
            }
            else
            {
                return null;
            }



        }

        #endregion
    }
}