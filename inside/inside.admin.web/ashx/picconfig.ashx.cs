
using crsri.cn.Model;
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
    /// Summary description for picconfig
    /// </summary>
    public class picconfig : IHttpHandler, IRequiresSessionState
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
                if (method.Equals("getGroupPic"))
                {
                    if (request["groupName"] != null)
                    {
                        jsonMessageModel jm = new jsonMessageModel();
                        jm.status = "ok";
                        try
                        {
                            List<ExpressionModelField> fieldWhere = new List<ExpressionModelField>();
                            fieldWhere.Add(new ExpressionModelField() { Name = "groupName", Value = request["groupName"].ToString().Trim() });
                            List<t_homePicConfig_list_model> list = efhelp.GetList<t_homePicConfig_list_model>(fieldWhere.ToArray());
                            jm.message = JsonConvert.SerializeObject(list);
                        }
                        catch (Exception err)
                        {
                            jm.status = "error";
                            jm.message = err.Message.ToString();
                        }

                        context.Response.Write(JsonConvert.SerializeObject(jm));
                    }
                }
                else if (method.Equals("updateUrlConfig"))
                {
                    if (request["url"] != null)
                    {
                        jsonMessageModel jm = new jsonMessageModel();
                        jm.status = "ok";

                        if (rightsId.FindAll(a => a.Equals("0601")).Count() > 0) //通栏图片上传权限
                        {
                            try
                            {
                                string sql = entityframework.sqlstringhelper.homePicConfigHelper.updateHomePicUrlConfig;
                                List<SqlParameter> param = new List<SqlParameter>();
                                param.Add(new SqlParameter() { ParameterName = "@url", Value = request["url"].ToString() });
                                efhelp.ExecuteSql(sql, param.ToArray());
                                operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【设置图片路径】 修改地址为:{0}", request["url"].ToString()));
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
                else if (method.Equals("updateLinkConfig"))
                {
                    if (request["item"] != null && request["link"] != null)
                    {
                        jsonMessageModel jm = new jsonMessageModel();
                        jm.status = "ok";

                        if (rightsId.FindAll(a => a.Equals("0601")).Count() > 0) //通栏图片上传权限
                        {
                            try
                            {
                                string sql = entityframework.sqlstringhelper.homePicConfigHelper.updatepicUrlConfig;
                                List<SqlParameter> param = new List<SqlParameter>();
                                param.Add(new SqlParameter() { ParameterName = "@item", Value = request["item"].ToString() });
                                param.Add(new SqlParameter() { ParameterName = "@linkurl", Value = request["link"].ToString() });
                                efhelp.ExecuteSql(sql, param.ToArray());
                                operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【修改链接地址】 修改为:{0}", request["link"].ToString()));

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
                else if (method.Equals("delUrlConfig"))
                {
                    if (request["item"] != null)
                    {
                        jsonMessageModel jm = new jsonMessageModel();
                        jm.status = "ok";

                        if (rightsId.FindAll(a => a.Equals("0601")).Count() > 0) //通栏图片上传权限
                        {
                            try
                            {
                                string sql = entityframework.sqlstringhelper.homePicConfigHelper.delpicUrlConfig;
                                List<SqlParameter> param = new List<SqlParameter>();
                                param.Add(new SqlParameter() { ParameterName = "@item", Value = request["item"].ToString() });
                                efhelp.ExecuteSql(sql, param.ToArray());
                                operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【删除图片】"));
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