
using crsri.cn.DbObject;
using crsri.cn.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace inside.crsri.Dal
{
    public class zhuanjiaDal : baseDal
    {
        public static t_web_zhuanjia_model reqZhuanjiaInfo(int ZhuanjiaID)
        {
            List<ExpressionModelField> field_And = new List<ExpressionModelField>();
            field_And.Add(new ExpressionModelField() { Name = "zhuanjiaID", Value = ZhuanjiaID });
            List<t_web_zhuanjia_model> reqlist = CrsriEntityFramework.GetList<t_web_zhuanjia_model>(field_And.ToArray(), new OrderModelField[] { });
            if (reqlist.Count > 0)
            {
                //浏览数
                string sql = string.Format("update zhuanjia set hits=hits+1 where zhuanjiaID=@zhuanjiaID and isdeleted=0 and ispassed=1");
                List<SqlParameter> para = new List<SqlParameter>();
                para.Add(new SqlParameter() { ParameterName = "@zhuanjiaID", Value = ZhuanjiaID });
                try
                {
                    CrsriEntityFramework.ExecuteSql(sql, para.ToArray());
                }
                catch
                {

                }
                return reqlist.First();
            }
            else
            {
                return null;
            }
        }


    }
}