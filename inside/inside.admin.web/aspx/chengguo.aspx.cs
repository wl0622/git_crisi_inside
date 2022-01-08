using crsri.cn.Model;
using inside.admin.web.entityframework;
using inside.admin.web.entityframework.sqlstringhelper;
using inside.admin.web.model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace inside.admin.web.aspx
{
    public partial class chengguo : basepage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!this.havingRights)
            //{
            //    Response.Redirect("noRights.aspx");
            //}

            if (Request.Form.AllKeys.Count() >= 10)
            {
                chengguoClass model = new chengguoClass();
                PropertyInfo[] propertys = model.GetType().GetProperties();
                foreach (string param in Request.Form.AllKeys)
                {
                    var findPropertys = propertys.Where(a => a.Name.Equals(param));

                    if (findPropertys.Count() > 0)
                    {
                        findPropertys.First().SetValue(model, Convert.ChangeType(Request.Form[param], findPropertys.First().PropertyType), null);
                    }
                }


                jsonMessageModel jm = new jsonMessageModel();
                jm.status = "ok";

                EFHELP efhelp = new EFHELP();

                try
                {
                    bool ishaving = true;

                    if (model.chengguoID != 0)
                    {
                        if (rightsId.FindAll(a => a.Equals("0802")).Count() == 0)
                        {
                            ishaving = false;
                        }
                    }
                    else
                    {
                        if (rightsId.FindAll(a => a.Equals("0801")).Count() == 0)
                        {
                            ishaving = false;
                        }
                    }

                    if (ishaving == false)
                    {
                        jm.status = "error";
                        jm.message = "无权限操作,请联系管理员!";
                    }
                    else
                    {
                        if (model.chengguoID != 0)
                        {
                            string sql = chengguohelper.update();
                            List<SqlParameter> para = new List<SqlParameter>();
                            para.Add(new SqlParameter() { ParameterName = "@chengguoID", Value = model.chengguoID });
                            para.Add(new SqlParameter() { ParameterName = "@xiangmuname", Value = model.xiangmuname });
                            para.Add(new SqlParameter() { ParameterName = "@canyudept", Value = model.canyudept });
                            para.Add(new SqlParameter() { ParameterName = "@chengguojj", Value = model.chengguojj });
                            para.Add(new SqlParameter() { ParameterName = "@dept", Value = model.dept });
                            para.Add(new SqlParameter() { ParameterName = "@EditorInCharge", Value = curUserModel.userCnName });
                            para.Add(new SqlParameter() { ParameterName = "@huojiangdengji", Value = model.huojiangdengji });
                            para.Add(new SqlParameter() { ParameterName = "@huojiangname", Value = model.huojiangname });
                            para.Add(new SqlParameter() { ParameterName = "@huojiangniandai", Value = model.huojiangniandai });
                            para.Add(new SqlParameter() { ParameterName = "@huojiangrenyuan", Value = model.huojiangrenyuan });
                            para.Add(new SqlParameter() { ParameterName = "@leibie", Value = model.leibie });
                            efhelp.ExecuteSql(sql, para.ToArray());
                            operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【成果修改】 	项目名称:{0}", model.xiangmuname));
                        }
                        else
                        {
                            model.isDeleted = false;
                            model.isPassed = true;
                            efhelp.Add(model);
                            operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【新增成果】 	项目名称:{0}", model.xiangmuname));
                        }
                    }

                }
                catch (Exception err)
                {
                    jm.status = "error";
                    jm.message = err.Message.ToString();
                }
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(jm));
            }
        }




    }
}