
using crsri.cn.Model;
using inside.admin.web.entityframework;
using inside.admin.web.entityframework.reqmodel;
using inside.admin.web.entityframework.sqlstringhelper;
using inside.admin.web.model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace inside.admin.web.ashx
{
    /// <summary>
    /// Summary description for chengguo
    /// </summary>
    public class chengguo : IHttpHandler, IRequiresSessionState
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
                if (method.Equals("chengguobind"))
                {
                    var stream = request.InputStream;
                    stream.Position = 0;
                    using (var streamReader = new StreamReader(stream, System.Text.Encoding.UTF8))
                    {
                        string jsonString = streamReader.ReadToEnd();
                        PaginatorRequestModel d = JsonConvert.DeserializeObject<PaginatorRequestModel>(jsonString);
                        chengguoClass m = JsonConvert.DeserializeObject<chengguoClass>(d.data);

                        int TotalPage = 0;
                        List<chengguoClass> list = new List<chengguoClass>();

                        int PageIndex = Convert.ToInt32(d.PageIndex);
                        int PageSize = Convert.ToInt32(d.PageSize);

                        List<OrderModelField> orderField = new List<OrderModelField>();
                        orderField.Add(new OrderModelField() { PropertyName = "chengguoID", IsDesc = true });

                        List<ExpressionModelField> fieldWhere = new List<ExpressionModelField>();
                        fieldWhere.Add(new ExpressionModelField() { Name = "isDeleted", Value = false });
                        if (m.huojiangniandai != null) { if (m.huojiangniandai != "") { fieldWhere.Add(new ExpressionModelField() { Name = "huojiangniandai", Value = m.huojiangniandai }); } }

                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic = efhelp.GetListPaged<chengguoClass>(PageIndex, PageSize, fieldWhere.ToArray(), orderField.ToArray());
                        var json = new { total = 0, rows = new List<chengguoClass>() };

                        if (dic != null)
                        {
                            int totalCount = (int)dic["total"];
                            TotalPage = (totalCount % PageSize == 0) ? (totalCount / PageSize) : ((totalCount / PageSize) + 1);
                            list = dic["rows"] as List<chengguoClass>;
                        }


                        PaginatorParameter<chengguoClass> param = new PaginatorParameter<chengguoClass>() { list = list, PageIndex = PageIndex, PageSize = PageSize, TotalPage = TotalPage };
                        String rtnJsonString = JsonConvert.SerializeObject(param);

                        context.Response.Write(rtnJsonString);
                    }
                }
                else if (method.Equals("chengguoInfoByID"))
                {
                    if (request["chengguoID"] != null)
                    {
                        string rtnJsonString = string.Empty;
                        List<chengguoClass> chengguoInfo = new List<chengguoClass>();
                        List<ExpressionModelField> fieldWhere = new List<ExpressionModelField>();
                        fieldWhere.Add(new ExpressionModelField() { Name = "chengguoID", Value = Convert.ToInt32(request["chengguoID"]) });
                        chengguoInfo = efhelp.GetList<chengguoClass>(fieldWhere.ToArray());
                        if (chengguoInfo.Count > 0)
                        {
                            rtnJsonString = JsonConvert.SerializeObject(chengguoInfo.First());
                        }
                        context.Response.Write(rtnJsonString);
                    }
                }
                else if (method.Equals("delByID"))
                {
                    jsonMessageModel jm = new jsonMessageModel();
                    jm.status = "ok";

                    if (rightsId.FindAll(a => a.Equals("0803")).Count() == 0)
                    {
                        jm.status = "error";
                        jm.message = "无权限操作,请联系管理员!";
                    }
                    else
                    {
                        if (request["chengguoID"] != null && request["xiangmuname"] != null)
                        {
                            int chengguoID = Convert.ToInt16(request["chengguoID"]);
                            string xiangmuname = request["xiangmuname"].ToString();

                            string sql = chengguohelper.delById();
                            List<SqlParameter> para = new List<SqlParameter>();
                            para.Add(new SqlParameter() { ParameterName = "@chengguoID", Value = chengguoID });
                            try
                            {
                                efhelp.ExecuteSql(sql, para.ToArray());
                                operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【删除成果】 成果标题:{0}", xiangmuname));
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