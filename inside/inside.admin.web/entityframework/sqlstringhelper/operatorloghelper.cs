using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.sqlstringhelper
{
    public class operatorloghelper
    {
        public static bool insertOperatorLog(string uname, string oper)
        {
            try
            {
                EFHELP efhelp = new EFHELP();
                string sql = string.Format("insert into OperatorLog(uname,opt,operatorTime)values(@uname,@operator,getdate())");
                List<SqlParameter> para = new List<SqlParameter>();
                para.Add(new SqlParameter() { ParameterName = "@uname", Value = uname });
                para.Add(new SqlParameter() { ParameterName = "@operator", Value = oper });
                if (efhelp.ExecuteSql(sql, para.ToArray()) > 0)
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
    }
}