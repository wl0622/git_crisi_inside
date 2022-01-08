
using crsri.cn.Model;
using inside.admin.web.entityframework;
using inside.admin.web.entityframework.sqlstringhelper;
using inside.admin.web.model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace inside.admin.web.aspx
{
    public partial class zhuangjia : basepage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Form.AllKeys.Count() >= 7)
            {
                t_web_zhuanjia_model model = new t_web_zhuanjia_model();
                PropertyInfo[] propertys = model.GetType().GetProperties();
                foreach (string param in Request.Form.AllKeys)
                {
                    var findPropertys = propertys.Where(a => a.Name.Equals(param));

                    if (findPropertys.Count() > 0)
                    {
                        findPropertys.First().SetValue(model, Convert.ChangeType(Request.Form[param], findPropertys.First().PropertyType), null);
                    }
                }

                if (Request.Form["editorValue"] != "")
                {
                    model.brief = Request.Form["editorValue"].ToString();
                }

                jsonMessageModel jm = new jsonMessageModel();
                jm.status = "ok";

                EFHELP efhelp = new EFHELP();

                try
                {
                    bool ishaving = true;

                    if (model.zhuanjiaID != 0)
                    {
                        if (this.rightsId.FindAll(a => a.Equals("0902")).Count() == 0)
                        {
                            ishaving = false;
                        }
                    }
                    else
                    {
                        if (this.rightsId.FindAll(a => a.Equals("0901")).Count() == 0)
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
                        List<SqlParameter> para = new List<SqlParameter>();
                        para.Add(new SqlParameter() { ParameterName = "@name", Value = model.name });
                        para.Add(new SqlParameter() { ParameterName = "@zhuanye", Value = model.zhuanye });
                        para.Add(new SqlParameter() { ParameterName = "@zhicheng", Value = model.zhicheng });
                        para.Add(new SqlParameter() { ParameterName = "@xuewei", Value = model.xuewei });
                        para.Add(new SqlParameter() { ParameterName = "@EditorInCharge", Value = curUserModel.userCnName });
                        para.Add(new SqlParameter() { ParameterName = "@brief", Value = model.brief });

                        if (model.zhuanjiaID != 0)
                        {
                            string sql = zhuanjiahelper.update();
                            para.Add(new SqlParameter() { ParameterName = "@zhuanjiaID", Value = model.zhuanjiaID });

                            if (efhelp.ExecuteSql(sql, para.ToArray()) > 0)
                            {
                                jm.message = model.zhuanjiaID.ToString();
                                operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【更新专家】 	项目名称:{0}", model.name));
                            }
                            else
                            {
                                jm.status = "error";
                                jm.message = "数据库执行错误";
                            }
                        }
                        else
                        {
                            DataTable zhuanjiaID = efhelp.QueryDataTable(zhuanjiahelper.getzhuanjiaID());
                            if (zhuanjiaID.Rows.Count > 0)
                            {
                                model.zhuanjiaID = Convert.ToInt32(zhuanjiaID.Rows[0][0]) + 1;
                            }
                            string sql = zhuanjiahelper.add();
                            para.Add(new SqlParameter() { ParameterName = "@zhuanjiaID", Value = model.zhuanjiaID });

                            if (efhelp.ExecuteSql(sql, para.ToArray()) > 0)
                            {
                                jm.message = model.zhuanjiaID.ToString();
                                operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【新增专家】 	项目名称:{0}", model.name));
                            }
                            else
                            {
                                jm.status = "error";
                                jm.message = "数据库执行错误";
                            }
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