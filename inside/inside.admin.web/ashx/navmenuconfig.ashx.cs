
using inside.admin.web.entityframework;
using inside.admin.web.entityframework.reqmodel;
using inside.admin.web.entityframework.sqlstringhelper;
using inside.admin.web.model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace inside.admin.web.ashx
{
    /// <summary>
    /// Summary description for navmenuconfig
    /// </summary>
    public class navmenuconfig : IHttpHandler, IRequiresSessionState
    {
        EFHELP efhelp = new EFHELP();
        public void ProcessRequest(HttpContext context)
        {
            //获取用户信息
            string userJsonString = FormsAuthentication.Decrypt(HttpContext.Current.Request.Cookies[".INSIDEAUTH"].Value).Name.ToString();
            reqUserSessionInfoModel reqUserInfo = JsonConvert.DeserializeObject<reqUserSessionInfoModel>(userJsonString);
            UsersModel curUserModel = reqUserInfo.uInfo;
            List<string> rightsId = new List<string>();
            if (reqUserInfo.uRightsID.Contains(","))
            {
                rightsId = reqUserInfo.uRightsID.Split(',').ToList();
            }
            else
            {
                if (reqUserInfo.uRightsID.Length > 0)
                {
                    rightsId.Add(reqUserInfo.uRightsID);
                }
            }

            var request = context.Request;
            if (request["method"] != null)
            {
                string method = request["method"];
                if (method.Equals("navmenusave"))
                {
                    jsonMessageModel jm = new jsonMessageModel();
                    jm.status = "ok";
                    bool ishaving = true;
                    if (request["id"] != null)
                    {
                        string id = request["id"].ToString();

                        if (id.Equals("0"))
                        {
                            if (rightsId.FindAll(a => a.Equals("0701")).Count() == 0)
                            {
                                ishaving = false;
                            }
                        }
                        else
                        {
                            if (rightsId.FindAll(a => a.Equals("0702")).Count() == 0)
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
                            if (request["parent"] != null && request["menuname"] != null && request["link"] != null)
                            {
                                string parent = request["parent"].ToString();
                                string menuname = request["menuname"].ToString();
                                string link = request["link"].ToString();

                                string sql = string.Empty;
                                List<SqlParameter> para = new List<SqlParameter>();

                                if (id.Equals("0"))
                                {
                                    sql = nvamenuSqlhelper.addChildrenSql();
                                    para.Add(new SqlParameter() { ParameterName = "@parentNode", Value = parent });
                                    para.Add(new SqlParameter() { ParameterName = "@nodeName", Value = menuname });
                                    para.Add(new SqlParameter() { ParameterName = "@linkurl", Value = link });
                                }
                                else
                                {
                                    sql = nvamenuSqlhelper.updateNavMenu();
                                    para.Add(new SqlParameter() { ParameterName = "@nodeId", Value = id });
                                    para.Add(new SqlParameter() { ParameterName = "@nodeName", Value = menuname });
                                    para.Add(new SqlParameter() { ParameterName = "@linkurl", Value = link });
                                }

                                try
                                {
                                    efhelp.ExecuteSql(sql, para.ToArray());
                                    if (id.Equals("0"))
                                    {
                                        operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【添加子导航】 {0},链接:{1}", menuname, link));
                                    }
                                    else
                                    {
                                        operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【修改导航】   {0},链接:{1}", menuname, link));
                                    }

                                }
                                catch (Exception err)
                                {
                                    jm.status = "error";
                                    jm.message = err.Message.ToString();
                                }

                            }
                            else
                            {
                                jm.status = "error";
                                jm.message = "请求参数错误";
                            }
                        }
                    }



                    context.Response.Write(JsonConvert.SerializeObject(jm));
                }
                else if (method.Equals("delNavMenu"))
                {
                    jsonMessageModel jm = new jsonMessageModel();
                    jm.status = "ok";

                    if (rightsId.FindAll(a => a.Equals("0703")).Count() == 0)
                    {
                        jm.status = "error";
                        jm.message = "无权限操作";
                    }
                    else
                    {
                        if (request["nodeId"] != null && request["nodeName"] != null)
                        {
                            string nodeId = request["nodeId"];
                            string nodeName = request["nodeName"].ToString();
                            string sql = nvamenuSqlhelper.delNavMenu();
                            List<SqlParameter> para = new List<SqlParameter>();
                            para.Add(new SqlParameter() { ParameterName = "@nodeId", Value = nodeId });
                            try
                            {
                                efhelp.ExecuteSql(sql, para.ToArray());
                                operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【删除子导航】 {0}", nodeName));
                            }
                            catch (Exception err)
                            {
                                jm.status = "error";
                                jm.message = err.Message.ToString();
                            }
                        }
                        else
                        {
                            jm.status = "error";
                            jm.message = "请求参数错误";
                        }
                    }
                    context.Response.Write(JsonConvert.SerializeObject(jm));
                }

            }
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