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
    /// Summary description for sitecfg
    /// </summary>
    public class sitecfg : IHttpHandler, IRequiresSessionState
    {

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
                EFHELP efhelp = new EFHELP();
                string method = request["method"];
                if (method.Equals("setIsBlackCfg"))
                {
                    jsonMessageModel jm = new jsonMessageModel();
                    jm.status = "ok";

                    if (rightsId.FindAll(a => a.Equals("01201")).Count() == 0)
                    {
                        jm.status = "error";
                        jm.message = "无权限操作,请联系管理员";
                    }
                    else
                    {
                        if (request["isBlack"] != null)
                        {

                            try
                            {
                                bool isBlack = Convert.ToBoolean(request["isBlack"]);
                                string sql = siteSqlhelper.setIsBlack();
                                List<SqlParameter> para = new List<SqlParameter>();
                                para.Add(new SqlParameter() { ParameterName = "@isBlack", Value = isBlack });
                                efhelp.ExecuteSql(sql, para.ToArray());
                                operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【站点配置】设置黑白{0}", isBlack));
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
                else if (method.Equals("setIsNewYearBgCfg"))
                {
                    jsonMessageModel jm = new jsonMessageModel();
                    jm.status = "ok";

                    if (rightsId.FindAll(a => a.Equals("01202")).Count() == 0)
                    {
                        jm.status = "error";
                        jm.message = "无权限操作,请联系管理员";
                    }
                    else
                    {
                        if (request["isNewYearBg"] != null)
                        {

                            try
                            {
                                bool isNewYearBg = Convert.ToBoolean(request["isNewYearBg"]);
                                string sql = siteSqlhelper.setIsNewYearBg();
                                List<SqlParameter> para = new List<SqlParameter>();
                                para.Add(new SqlParameter() { ParameterName = "@isNewYearBg", Value = isNewYearBg });
                                efhelp.ExecuteSql(sql, para.ToArray());
                                operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【站点配置】设置新年背景{0}", isNewYearBg));
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
                if (method.Equals("setIsTopBoldCfg"))
                {
                    jsonMessageModel jm = new jsonMessageModel();
                    jm.status = "ok";

                    if (rightsId.FindAll(a => a.Equals("01201")).Count() == 0)
                    {
                        jm.status = "error";
                        jm.message = "无权限操作,请联系管理员";
                    }
                    else
                    {
                        if (request["isTopBold"] != null)
                        {

                            try
                            {
                                bool isTopBold = Convert.ToBoolean(request["isTopBold"]);
                                string sql = siteSqlhelper.setIsTopBold();
                                List<SqlParameter> para = new List<SqlParameter>();
                                para.Add(new SqlParameter() { ParameterName = "@isTopBold", Value = isTopBold });
                                efhelp.ExecuteSql(sql, para.ToArray());
                                operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【站点配置】设置顶粗体{0}", isTopBold));
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
                if (method.Equals("setIsOnTopBoldCfg"))
                {
                    jsonMessageModel jm = new jsonMessageModel();
                    jm.status = "ok";

                    if (rightsId.FindAll(a => a.Equals("01201")).Count() == 0)
                    {
                        jm.status = "error";
                        jm.message = "无权限操作,请联系管理员";
                    }
                    else
                    {
                        if (request["isOnTopBold"] != null)
                        {

                            try
                            {
                                bool isOnTopBold = Convert.ToBoolean(request["isOnTopBold"]);
                                string sql = siteSqlhelper.setIsOnTopBold();
                                List<SqlParameter> para = new List<SqlParameter>();
                                para.Add(new SqlParameter() { ParameterName = "@isOnTopBold", Value = isOnTopBold });
                                efhelp.ExecuteSql(sql, para.ToArray());
                                operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【站点配置】设置头条粗体{0}", isOnTopBold));
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
                else if (method.Equals("updateLinkConfig"))
                {
                    if (request["item"] != null && request["link"] != null)
                    {
                        jsonMessageModel jm = new jsonMessageModel();
                        jm.status = "ok";

                        if (rightsId.FindAll(a => a.Equals("01203")).Count() > 0)
                        {
                            try
                            {
                                string sql = entityframework.sqlstringhelper.homePicConfigHelper.updateSiteLinkUrl;
                                List<SqlParameter> param = new List<SqlParameter>();
                                param.Add(new SqlParameter() { ParameterName = "@item", Value = Convert.ToInt32(request["item"]) });
                                param.Add(new SqlParameter() { ParameterName = "@linkurl", Value = request["link"] });
                                efhelp.ExecuteSql(sql, param.ToArray());
                                operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【图片链接地址】 {0}", request["link"].ToString()));

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
                            jm.message = "无权限操作,请联系管理员!";
                        }
                        context.Response.Write(JsonConvert.SerializeObject(jm));
                    }
                }
                else if (method.Equals("delCfgPic"))
                {
                    if (request["item"] != null)
                    {
                        jsonMessageModel jm = new jsonMessageModel();
                        jm.status = "ok";

                        if (rightsId.FindAll(a => a.Equals("01203")).Count() > 0)
                        {
                            try
                            {
                                string sql = entityframework.sqlstringhelper.homePicConfigHelper.delSitePic;
                                List<SqlParameter> param = new List<SqlParameter>();
                                param.Add(new SqlParameter() { ParameterName = "@item", Value = Convert.ToInt32(request["item"]) });
                                efhelp.ExecuteSql(sql, param.ToArray());
                                operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【删除站点配置图片】"));
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
                            jm.message = "无权限操作,请联系管理员!";
                        }

                        context.Response.Write(JsonConvert.SerializeObject(jm));
                    }
                }
                else if (method.Equals("setTitleColor"))
                {
                    if (request["item"] != null)
                    {
                        jsonMessageModel jm = new jsonMessageModel();
                        jm.status = "ok";

                        if (rightsId.FindAll(a => a.Equals("01203")).Count() > 0)
                        {
                            try
                            {
                                string sql = entityframework.sqlstringhelper.homePicConfigHelper.setTitleColor;
                                List<SqlParameter> param = new List<SqlParameter>();
                                param.Add(new SqlParameter() { ParameterName = "@itemVal", Value = request["item"] });
                                efhelp.ExecuteSql(sql, param.ToArray());
                                operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【新闻头条颜色设置】"));
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
                            jm.message = "无权限操作,请联系管理员!";
                        }

                        context.Response.Write(JsonConvert.SerializeObject(jm));
                    }
                }
                else if (method.Equals("setTopColor"))
                {
                    if (request["item"] != null)
                    {
                        jsonMessageModel jm = new jsonMessageModel();
                        jm.status = "ok";

                        if (rightsId.FindAll(a => a.Equals("01203")).Count() > 0)
                        {
                            try
                            {
                                string sql = entityframework.sqlstringhelper.homePicConfigHelper.setTopColor;
                                List<SqlParameter> param = new List<SqlParameter>();
                                param.Add(new SqlParameter() { ParameterName = "@itemVal", Value = request["item"] });
                                efhelp.ExecuteSql(sql, param.ToArray());
                                operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【栏目置顶颜色设置】"));
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
                            jm.message = "无权限操作,请联系管理员!";
                        }

                        context.Response.Write(JsonConvert.SerializeObject(jm));
                    }
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