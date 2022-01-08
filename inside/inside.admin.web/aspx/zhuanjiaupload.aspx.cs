using inside.admin.web.entityframework;
using inside.admin.web.entityframework.sqlstringhelper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace inside.admin.web.aspx
{
    public partial class zhuanjiaupload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string res = "{ error:'', msg:'',imgurl:''}";

            if (Request["zhuanjiaid"] != null)
            {
                string fileName = Request["zhuanjiaid"].ToString();
                string uploadDir = string.Format(@"/Article/UploadPic/new/");
                HttpFileCollection files = Request.Files;//这里只能用<input type="file" />才能有效果,因为服务器控件是HttpInputFile类型
                string msg = string.Empty;
                string error = string.Empty;
                string imgurl = string.Empty;
                if (files.Count > 0)
                {
                    try
                    {
                        string newFileName = fileName + files[0].FileName.Substring(files[0].FileName.LastIndexOf("."));
                        files[0].SaveAs(Server.MapPath("../") + uploadDir + newFileName);
                        //更新数据库
                        EFHELP efhelper = new EFHELP();
                        string sql = zhuanjiahelper.updatephotoname();
                        List<SqlParameter> param = new List<SqlParameter>();
                        param.Add(new SqlParameter() { ParameterName = "@photoname", Value = uploadDir + newFileName });
                        param.Add(new SqlParameter() { ParameterName = "@zhuanjiaID", Value = fileName });
                        efhelper.ExecuteSql(sql, param.ToArray());
                        res = "{ error:'" + error + "', msg:'" + msg + "',imgurl:'" + uploadDir + newFileName + "'}";

                    }
                    catch (Exception err)
                    {
                        error = err.Message.ToString();
                        res = "{ error:'" + error + "', msg:'" + msg + "',imgurl:'" + imgurl + "'}";
                    }

                }
            }
            else
            {
                res = "{ error:'上传参数错误', msg:'',imgurl:''}";
            }

            Response.Write(res);
            Response.End();
        }
    }
}