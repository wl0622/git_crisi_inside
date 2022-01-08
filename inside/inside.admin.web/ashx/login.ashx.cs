
using inside.admin.web.entityframework;
using inside.admin.web.entityframework.reqmodel;
using inside.admin.web.entityframework.sqlstringhelper;
using inside.admin.web.entityframework.tableEntity;
using inside.admin.web.helper;
using inside.admin.web.model;
using inside.admin.web.util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace inside.admin.web.ashx
{
    /// <summary>
    /// Summary description for login
    /// </summary>
    public class login : IHttpHandler, IRequiresSessionState
    {
        EFHELP efhelp = new EFHELP();
        public void ProcessRequest(HttpContext context)
        {
            #region 明文传递
            //var request = context.Request;
            //string uname = request.Form["username"];
            //string upwd = request.Form["password"];
            //string code = request.Form["code"];
            #endregion

            #region 加密传输

            JsEncryptHelper jsHelper = new JsEncryptHelper();
            string uname = context.Request["username"] + "";
            string upwd = context.Request["password"] + "";
            string code = context.Request["code"] + "";
            uname = jsHelper.Decrypt(uname);
            upwd = jsHelper.Decrypt(upwd);
            code = jsHelper.Decrypt(code);

            jsonMessageModel jm = new jsonMessageModel();
            #endregion

            try
            {
                if (context.Session["validateCode"] != null)
                {
                    if (context.Session["validateCode"].ToString() == code)
                    {
                        //验证是否错误登陆超过3次
                        List<ExpressionModelField> fieldWhere = new List<ExpressionModelField>();
                        fieldWhere.Add(new ExpressionModelField() { Name = "ipAddress", Value = IPAddresHelper.GetWebClientIp() });
                        fieldWhere.Add(new ExpressionModelField() { Name = "login", Value = uname });
                        List<errorLoginRecordModel> errorlist = efhelp.GetList<errorLoginRecordModel>(fieldWhere.ToArray());
                        bool ContinueValidate = true;

                        if (errorlist.Count > 0)
                        {
                            if (errorlist.First().errorCount == 3)
                            {
                                ContinueValidate = false;
                                jm.status = "error";
                                jm.message = "错误次数已达上限,已被限制访问,请联系管理员解除";
                            }
                        }

                        if (ContinueValidate)
                        {
                            UsersModel u = new UsersModel();
                            u.userName = uname;
                            u.userPassword = SecurityHelper.Md5(upwd);

                            fieldWhere.Clear();
                            fieldWhere.Add(new ExpressionModelField() { Name = "username", Value = u.userName });
                            fieldWhere.Add(new ExpressionModelField() { Name = "userPassword", Value = u.userPassword });
                            List<UsersModel> users = efhelp.GetList<UsersModel>(fieldWhere.ToArray());
                            if (users.Count > 0)
                            {
                                try
                                {
                                    //插入登录日志
                                    loginLogModel logModel = new loginLogModel();
                                    logModel.loginName = u.userName;
                                    logModel.loginIp = IPAddresHelper.GetWebClientIp();
                                    logModel.loginTime = DateTime.Now;
                                    efhelp.AddEntity(logModel);

                                    //清空记录错误的日志
                                    if (errorlist.Count > 0)
                                    {
                                        efhelp.Delete(errorlist.First());
                                    }

                                    //获取当前用的权限
                                    DataTable dt = efhelp.QueryDataTable(userSqlHelper.getUserRights(), new object[] { new SqlParameter() { ParameterName = "@groupsid", Value = users.First().userGroupID } });
                                    List<string> uRightsId = new List<string>();
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        uRightsId.Add(dr["rightsid"].ToString());
                                    }

                                    reqUserSessionInfoModel user = new reqUserSessionInfoModel();
                                    user.uInfo = users.First();
                                    user.uRightsID = string.Join(",", uRightsId.ToArray());
                                    string userJson =    JsonConvert.SerializeObject(user);


                                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                                       1,
                                       userJson,
                                       DateTime.Now,
                                       DateTime.Now.AddMinutes(30),
                                       false,
                                       "admins"
                                       );

                                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);//加密票证
                                    //System.Web.HttpCookie authCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                                    System.Web.HttpCookie authCookie = new System.Web.HttpCookie(".INSIDEAUTH", encryptedTicket);
                                 
                                    System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);

                                    jm.status = "ok";
                                    jm.message = "/main.aspx";

                                }
                                catch (Exception err)
                                {
                                    jm.status = "error";
                                    jm.message = err.Message.ToString();
                                }
                            }
                            else //用户名密码误
                            {
                                //插入错误登录记录
                                errorLoginRecordModel error = new errorLoginRecordModel();


                                if (errorlist.Count > 0)
                                {
                                    //更新数据(错误次数)

                                    try
                                    {
                                        error = errorlist.First();
                                        error.errorCount += 1;
                                        error.lastTime = DateTime.Now;
                                        efhelp.Update(error);
                                    }
                                    catch
                                    {

                                    }

                                }
                                else
                                {
                                    //插入数据
                                    try
                                    {
                                        error.login = u.userName;
                                        error.firstTime = DateTime.Now;
                                        error.lastTime = DateTime.Now;
                                        error.ipAddress = IPAddresHelper.GetWebClientIp();
                                        error.errorCount = 1;
                                        efhelp.Add(error);
                                    }
                                    catch
                                    {

                                    }
                                }

                                jm.status = "error";
                                jm.message = "用户名密码不匹配";
                            }
                        }
                    }
                    else
                    {
                        jm.status = "error";
                        jm.message = "验证码输入错误";
                    }
                }
                else
                {
                    jm.status = "error";
                    jm.message = "请刷新验证码";
                }
            }
            catch (Exception err)
            {
                jm.status = "error";
                jm.message = err.Message.ToString();
            }

            context.Response.Write(JsonConvert.SerializeObject(jm));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}