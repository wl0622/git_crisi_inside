using inside.admin.web.entityframework;
using crsri.cn.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using inside.admin.web.entityframework.reqmodel;
using inside.admin.web.model;
using inside.admin.web.entityframework;
using inside.admin.web.entityframework.sqlstringhelper;

namespace inside.admin.web.ashx
{
    /// <summary>
    /// Summary description for zhuanjia
    /// </summary>
    public class zhuanjia : IHttpHandler, IRequiresSessionState
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
                if (method.Equals("zhuanjiabind"))
                {
                    var stream = request.InputStream;
                    stream.Position = 0;
                    using (var streamReader = new StreamReader(stream, System.Text.Encoding.UTF8))
                    {
                        string jsonString = streamReader.ReadToEnd();
                        PaginatorRequestModel d = JsonConvert.DeserializeObject<PaginatorRequestModel>(jsonString);
                        t_web_zhuanjia_model m = JsonConvert.DeserializeObject<t_web_zhuanjia_model>(d.data);

                        int TotalPage = 0;
                        List<t_web_zhuanjia_model> list = new List<t_web_zhuanjia_model>();

                        int PageIndex = Convert.ToInt32(d.PageIndex);
                        int PageSize = Convert.ToInt32(d.PageSize);

                        List<OrderModelField> orderField = new List<OrderModelField>();
                        orderField.Add(new OrderModelField() { PropertyName = "zhuanjiaID", IsDesc = true });

                        List<ExpressionModelField> fieldWhere = new List<ExpressionModelField>();
                        fieldWhere.Add(new ExpressionModelField() { Name = "isDeleted", Value = false });
                        if (m.name != null) { if (m.name != "") { fieldWhere.Add(new ExpressionModelField() { Name = "name", Value = m.name }); } }

                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic = efhelp.GetListPaged<t_web_zhuanjia_model>(PageIndex, PageSize, fieldWhere.ToArray(), orderField.ToArray());
                        var json = new { total = 0, rows = new List<t_web_zhuanjia_model>() };

                        if (dic != null)
                        {
                            int totalCount = (int)dic["total"];
                            TotalPage = (totalCount % PageSize == 0) ? (totalCount / PageSize) : ((totalCount / PageSize) + 1);
                            list = dic["rows"] as List<t_web_zhuanjia_model>;
                        }


                        PaginatorParameter<t_web_zhuanjia_model> param = new PaginatorParameter<t_web_zhuanjia_model>() { list = list, PageIndex = PageIndex, PageSize = PageSize, TotalPage = TotalPage };
                        String rtnJsonString = JsonConvert.SerializeObject(param);

                        context.Response.Write(rtnJsonString);
                    }
                }
                else if (method.Equals("zhuanjiaInfoByID"))
                {
                    if (request["zhuanjiaID"] != null)
                    {
                        string rtnJsonString = string.Empty;
                        List<t_web_zhuanjia_model> zhuanjiaInfo = new List<t_web_zhuanjia_model>();
                        List<ExpressionModelField> fieldWhere = new List<ExpressionModelField>();
                        fieldWhere.Add(new ExpressionModelField() { Name = "zhuanjiaID", Value = Convert.ToInt32(request["zhuanjiaID"]) });
                        zhuanjiaInfo = efhelp.GetList<t_web_zhuanjia_model>(fieldWhere.ToArray());
                        if (zhuanjiaInfo.Count > 0)
                        {
                            rtnJsonString = JsonConvert.SerializeObject(zhuanjiaInfo.First());
                        }
                        context.Response.Write(rtnJsonString);
                    }
                }
                else if (method.Equals("delByID"))
                {
                    jsonMessageModel jm = new jsonMessageModel();
                    jm.status = "ok";

                    if (rightsId.FindAll(a => a.Equals("0903")).Count() == 0)
                    {
                        jm.status = "error";
                        jm.message = "无权限操作,请联系管理员!";
                    }
                    else
                    {
                        if (request["zhuanjiaID"] != null && request["name"] != null)
                        {
                            int zhuanjiaID = Convert.ToInt16(request["zhuanjiaID"]);
                            string name = request["name"].ToString();

                            string sql = zhuanjiahelper.delById();
                            List<SqlParameter> para = new List<SqlParameter>();
                            para.Add(new SqlParameter() { ParameterName = "@zhuanjiaID", Value = zhuanjiaID });
                            try
                            {
                                efhelp.ExecuteSql(sql, para.ToArray());
                                operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【删除专家】 专家姓名:{0}", name));
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
                else if (method.Equals("chkByID"))
                {
                    jsonMessageModel jm = new jsonMessageModel();
                    jm.status = "ok";

                    if (rightsId.FindAll(a => a.Equals("0904")).Count() == 0)
                    {
                        jm.status = "error";
                        jm.message = "无权限操作,请联系管理员!";
                    }
                    else
                    {
                        if (request["zhuanjiaID"] != null && request["name"] != null)
                        {
                            int zhuanjiaID = Convert.ToInt16(request["zhuanjiaID"]);
                            string name = request["name"].ToString();

                            string sql = zhuanjiahelper.delById();
                            List<SqlParameter> para = new List<SqlParameter>();
                            para.Add(new SqlParameter() { ParameterName = "@zhuanjiaID", Value = zhuanjiaID });
                            try
                            {
                                efhelp.ExecuteSql(sql, para.ToArray());
                                operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【专家发布审核】 专家姓名:{0}", name));
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