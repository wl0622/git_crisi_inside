
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
    public partial class upload : basepage
    {

        string uploadDir = string.Format(@"/ueditor/1.4.3/net/upload/homepicconfig/");
        protected void Page_Load(object sender, EventArgs e)
        {
            string res = string.Empty;

            if (rightsId.FindAll(a => a.Equals("01203")).Count() == 0)
            {
                res = "{ error:'无权限操作,请联系管理员', msg:'',imgurl:''}";
            }
            else
            {
                HttpFileCollection files = Request.Files;//这里只能用<input type="file" />才能有效果,因为服务器控件是HttpInputFile类型
                string msg = string.Empty;
                string error = string.Empty;
                string imgurl = string.Empty;
                string id = string.Empty;
                if (files.Count > 0)
                {
                    try
                    {
                        //string newFileName = DateTime.Now.ToFileTimeUtc().ToString() + files[0].FileName;
                        string newFileName = DateTime.Now.ToFileTimeUtc().ToString() + files[0].FileName.Substring(files[0].FileName.LastIndexOf("."));
                        files[0].SaveAs(Server.MapPath("/") + uploadDir + System.IO.Path.GetFileName(newFileName));
                        msg = " 成功! 文件大小为:" + files[0].ContentLength;
                        imgurl = "/" + newFileName;
                        int position = this.position(files.AllKeys[0].ToString());
                        bool isShortPic = this.isShortPic(files.AllKeys[0].ToString());

                        //更新数据库
                        EFHELP efhelper = new EFHELP();
                        string sql = homePicConfigHelper.insertSitePic;
                        List<SqlParameter> param = new List<SqlParameter>();
                        param.Add(new SqlParameter() { ParameterName = "@isShortPic", Value = isShortPic });
                        param.Add(new SqlParameter() { ParameterName = "@picName", Value = newFileName });
                        param.Add(new SqlParameter() { ParameterName = "@position", Value = position });

                        sql += "select @@IDENTITY as id";
                        //efhelper.ExecuteSql(sql, param.ToArray());
                        System.Data.DataTable dt= efhelper.QueryDataTable(sql, param.ToArray());
                        if (dt.Rows.Count > 0)
                        {
                            id =dt.Rows[0][0].ToString();
                        }

                        operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【网站配置】图片上传{0}", newFileName));
                    }
                    catch (Exception err)
                    {
                        error = err.Message.ToString();
                    }
                    res = "{ error:'" + error + "', msg:'" + msg + "',imgurl:'" + imgurl + "',id:'" + id + "'}";
                }
            }

            Response.Write(res);
            Response.End();
        }


        private bool isShortPic(string elName)
        {
            return elName == "cfgFlashshort3" || elName == "cfgFlashshortP2" || elName == "cfgCntPagePicS3" || elName == "cfgRightFloat4" || elName == "cfgslkj5" || elName == "cfgkjjl6" || elName == "cfgxmdt7" || elName == "cfgdwwh8";
        }

        public int position(string elName)
        {
            return StringHelper.GetNums(elName);
        }
    }
}