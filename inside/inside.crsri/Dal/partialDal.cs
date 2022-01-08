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
    public class partialDal : baseDal
    {
        public static List<t_homePicConfig_list_model> requestPageFloatImages()
        {
            List<ExpressionModelField> field_And = new List<ExpressionModelField>();
            field_And.Add(new ExpressionModelField() { Name = "groupName", Value = "homefloat" });
            field_And.Add(new ExpressionModelField() { Name = "picName", Value = "", Relation = EnumRelation.NotEqual });
            List<t_homePicConfig_list_model> reqlist = CrsriEntityFramework.GetList<t_homePicConfig_list_model>(field_And.ToArray(), new OrderModelField[] { });
            return reqlist;
        }

        public static string requestWebCounter()
        {
            try
            {
                string sql = string.Format("select [counter] from dbo.[Counter] where shuoming='newWeb'");
                DataTable d = CrsriEntityFramework.QueryDataTable(sql);
                if (d.Rows.Count > 0)
                {
                    return d.Rows[0][0].ToString();
                }
                else
                {
                    return "计数器错误";
                }

            }
            catch (Exception err)
            {
                return err.Message.ToString();
            }
        }
    }
}