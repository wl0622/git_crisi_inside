using inside.admin.web.entityframework;
using inside.admin.web.entityframework.reqmodel;
using inside.admin.web.entityframework.sqlstringhelper;
using inside.admin.web.entityframework.tableEntity;
using inside.admin.web.helper;
using inside.admin.web.model;
using inside.admin.web.util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace inside.admin.web.ashx
{
    /// <summary>
    /// Summary description for subjects
    /// </summary>
    public class user : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;

            if (request["method"] != null)
            {
                //获取用户信息
                List<string> rightsId = new List<string>();
                HttpCookie requestCookies = HttpContext.Current.Request.Cookies[".INSIDEAUTH"];
                UsersModel curUserModel = new UsersModel();
                if (requestCookies != null)
                {
                    string userJsonString = FormsAuthentication.Decrypt(requestCookies.Value).Name.ToString();
                    reqUserSessionInfoModel reqUserInfo = JsonConvert.DeserializeObject<reqUserSessionInfoModel>(userJsonString);
                    curUserModel = reqUserInfo.uInfo;

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

                }


                EFHELP efhelp = new EFHELP();
                string method = request["method"];
                if (method.Equals("userlistbind"))
                {
                    var stream = request.InputStream;
                    stream.Position = 0;
                    using (var streamReader = new StreamReader(stream, System.Text.Encoding.UTF8))
                    {

                        string jsonString = streamReader.ReadToEnd();
                        PaginatorRequestModel d = JsonConvert.DeserializeObject<PaginatorRequestModel>(jsonString);
                        userpaginatorModel m = JsonConvert.DeserializeObject<userpaginatorModel>(d.data);

                        int TotalPage = 0;
                        List<userlistModel> list = new List<userlistModel>();

                        int PageIndex = Convert.ToInt32(d.PageIndex);
                        int PageSize = Convert.ToInt32(d.PageSize);

                        StringBuilder sqlWhere = new StringBuilder();
                        List<SqlParameter> param = new List<SqlParameter>();


                        if (m.userName != "")
                        {
                            sqlWhere.Append(string.Format(" and userName=@userName"));
                            param.Add(new SqlParameter() { ParameterName = "@userName", Value = m.userName });
                        }

                        if (m.userCnName != "")
                        {
                            sqlWhere.Append(string.Format(" and userCnName=@userCnName"));
                            param.Add(new SqlParameter() { ParameterName = "@userCnName", Value = m.userName });
                        }

                        if (m.deptID != "")
                        {
                            sqlWhere.Append(string.Format(" and deptID=@deptID"));
                            param.Add(new SqlParameter() { ParameterName = "@deptID", Value = m.deptID });
                        }

                        if (m.userGroupID != "")
                        {
                            sqlWhere.Append(string.Format(" and userGroupID=@userGroupID"));
                            param.Add(new SqlParameter() { ParameterName = "@userGroupID", Value = m.deptID });
                        }

                        string sql = string.Format("select COUNT(*) from Users where 1=1 {0}", sqlWhere.ToString());

                        DataTable dtCount = new DataTable();
                        if (param.Count > 0)
                        {
                            dtCount = efhelp.QueryDataTable(sql, param.ToArray());
                        }
                        else
                        {
                            dtCount = efhelp.QueryDataTable(sql);
                        }

                        if (dtCount.Rows.Count > 0)
                        {
                            int totalRows = Convert.ToInt32(dtCount.Rows[0][0]);
                            if (totalRows > 0)
                            {
                                int iStart = PageIndex == 1 ? 1 : (PageIndex - 1) * PageSize + 1;
                                int iEnd = PageIndex * PageSize;

                                param.Add(new SqlParameter() { ParameterName = "@iStart", Value = iStart });
                                param.Add(new SqlParameter() { ParameterName = "@iEnd", Value = iEnd });

                                sql = string.Format(@"select * from (
                        
                                                       SELECT ROW_NUMBER() OVER(ORDER BY userID desc) as row,a.*,b.deptName,c.name as [userGroupName] from Users as a  
                                                       left join Department as b on a.deptID=b.deptID 
                                                       left join UserGroups as c on a.userGroupID=c.userGroupID
                                                       where 1=1 {0}) tt where row between @iStart and @iEnd
                                             ", sqlWhere);

                                DataTable dtUser = efhelp.QueryDataTable(sql, param.ToArray());


                                if (dtUser.Rows.Count > 0)
                                {
                                    int totalCount = totalRows;
                                    TotalPage = (totalCount % PageSize == 0) ? (totalCount / PageSize) : ((totalCount / PageSize) + 1);
                                    list = ConvertType.ConvertToModel<userlistModel>(dtUser);
                                }

                                IsoDateTimeConverter timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
                                PaginatorParameter<userlistModel> jsonParam = new PaginatorParameter<userlistModel>() { list = list, PageIndex = PageIndex, PageSize = PageSize, TotalPage = TotalPage };
                                String rtnJsonString = JsonConvert.SerializeObject(jsonParam, timeConverter);
                                context.Response.Write(rtnJsonString);
                            }
                            else
                            {
                                PaginatorParameter<userlistModel> jsonParam = new PaginatorParameter<userlistModel>() { list = new List<userlistModel>(), PageIndex = 0, PageSize = PageSize, TotalPage = 0 };
                                String rtnJsonString = JsonConvert.SerializeObject(jsonParam);
                                context.Response.Write(rtnJsonString);
                            }
                        }
                    }
                }

                else if (method.Equals("userlogbind"))
                {
                    if (rightsId.FindAll(a => a.Equals("0301")).Count() > 0)//用户登陆日志
                    {
                        var stream = request.InputStream;
                        stream.Position = 0;
                        using (var streamReader = new StreamReader(stream, System.Text.Encoding.UTF8))
                        {
                            string jsonString = streamReader.ReadToEnd();
                            PaginatorRequestModel d = JsonConvert.DeserializeObject<PaginatorRequestModel>(jsonString);
                            loginLogpaginatorModel m = JsonConvert.DeserializeObject<loginLogpaginatorModel>(d.data);

                            int TotalPage = 0;
                            List<loginLogModel> list = new List<loginLogModel>();
                            int PageIndex = Convert.ToInt32(d.PageIndex);
                            int PageSize = Convert.ToInt32(d.PageSize);

                            StringBuilder sqlWhere = new StringBuilder();
                            List<SqlParameter> param = new List<SqlParameter>();


                            if (m.loginName != "")
                            {
                                sqlWhere.Append(string.Format(" and loginName=@loginName"));
                                param.Add(new SqlParameter() { ParameterName = "@loginName", Value = m.loginName });
                            }

                            if (m.loginTime != "")
                            {
                                sqlWhere.Append(string.Format(" and loginTime=@loginTime"));
                                param.Add(new SqlParameter() { ParameterName = "@loginTime", Value = m.loginTime });
                            }

                            if (m.loginIp != "")
                            {
                                sqlWhere.Append(string.Format(" and loginIp=@loginIp"));
                                param.Add(new SqlParameter() { ParameterName = "@loginIp", Value = m.loginIp });
                            }


                            string sql = string.Format("select COUNT(*) from t_loginLog where 1=1 {0}", sqlWhere.ToString());

                            DataTable dtCount = new DataTable();
                            if (param.Count > 0)
                            {
                                dtCount = efhelp.QueryDataTable(sql, param.ToArray());
                            }
                            else
                            {
                                dtCount = efhelp.QueryDataTable(sql);
                            }

                            if (dtCount.Rows.Count > 0)
                            {
                                int totalRows = Convert.ToInt32(dtCount.Rows[0][0]);
                                if (totalRows > 0)
                                {

                                    int iStart = PageIndex == 1 ? 1 : (PageIndex - 1) * PageSize + 1;
                                    int iEnd = PageIndex * PageSize;



                                    sql = string.Format(@"select * from (
                        
                                                       SELECT ROW_NUMBER() OVER(ORDER BY id desc) as row,* from t_loginLog 
                                                     
                                                       where 1=1 {0}) tt where row>= @iStart and row<= @iEnd
                                             ", sqlWhere);

                                    param.Add(new SqlParameter() { ParameterName = "@iStart", Value = iStart });
                                    param.Add(new SqlParameter() { ParameterName = "@iEnd", Value = iEnd });


                                    DataTable dtLoginLog = efhelp.QueryDataTable(sql, param.ToArray());


                                    if (dtLoginLog.Rows.Count > 0)
                                    {
                                        int totalCount = totalRows;
                                        TotalPage = (totalCount % PageSize == 0) ? (totalCount / PageSize) : ((totalCount / PageSize) + 1);
                                        list = ConvertType.ConvertToModel<loginLogModel>(dtLoginLog);
                                    }

                                    IsoDateTimeConverter timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
                                    PaginatorParameter<loginLogModel> jsonParam = new PaginatorParameter<loginLogModel>() { list = list, PageIndex = PageIndex, PageSize = PageSize, TotalPage = TotalPage };
                                    String rtnJsonString = JsonConvert.SerializeObject(jsonParam, timeConverter);
                                    context.Response.Write(rtnJsonString);
                                }
                                else
                                {
                                    PaginatorParameter<loginLogModel> jsonParam = new PaginatorParameter<loginLogModel>() { list = new List<loginLogModel>(), PageIndex = 1, PageSize = PageSize, TotalPage = 1 };
                                    String rtnJsonString = JsonConvert.SerializeObject(jsonParam);
                                    context.Response.Write(rtnJsonString);
                                }
                            }
                        }
                    }
                    else
                    {
                        var Operatorlist = new List<loginLogModel>();
                        PaginatorParameter<loginLogModel> jsonParam = new PaginatorParameter<loginLogModel>() { list = Operatorlist, PageIndex = 1, PageSize = 1, TotalPage = 1 };
                        String rtnJsonString = JsonConvert.SerializeObject(jsonParam);
                        context.Response.Write(rtnJsonString);
                    }

                }
                else if (method.Equals("optlogbind"))
                {
                    if (rightsId.FindAll(a => a.Equals("0301")).Count() > 0)//操作日志
                    {
                        var stream = request.InputStream;
                        stream.Position = 0;
                        using (var streamReader = new StreamReader(stream, System.Text.Encoding.UTF8))
                        {
                            string jsonString = streamReader.ReadToEnd();
                            PaginatorRequestModel d = JsonConvert.DeserializeObject<PaginatorRequestModel>(jsonString);
                            loginLogpaginatorModel m = JsonConvert.DeserializeObject<loginLogpaginatorModel>(d.data);

                            int TotalPage = 0;
                            List<OperatorLogModel> list = new List<OperatorLogModel>();
                            int PageIndex = Convert.ToInt32(d.PageIndex);
                            int PageSize = Convert.ToInt32(d.PageSize);

                            StringBuilder sqlWhere = new StringBuilder();
                            List<SqlParameter> param = new List<SqlParameter>();


                            if (m.loginName != "")
                            {
                                sqlWhere.Append(string.Format(" and uname=@uname"));
                                param.Add(new SqlParameter() { ParameterName = "@uname", Value = m.loginName });
                            }

                            if (m.loginTime != "")
                            {
                                sqlWhere.Append(string.Format(" and operatorTime=@operatorTime"));
                                param.Add(new SqlParameter() { ParameterName = "@operatorTime", Value = m.loginTime });
                            }


                            string sql = string.Format("select COUNT(*) from OperatorLog where 1=1 {0}", sqlWhere.ToString());

                            DataTable dtCount = new DataTable();
                            if (param.Count > 0)
                            {
                                dtCount = efhelp.QueryDataTable(sql, param.ToArray());
                            }
                            else
                            {
                                dtCount = efhelp.QueryDataTable(sql);
                            }

                            if (dtCount.Rows.Count > 0)
                            {
                                int totalRows = Convert.ToInt32(dtCount.Rows[0][0]);
                                if (totalRows > 0)
                                {

                                    int iStart = PageIndex == 1 ? 1 : (PageIndex - 1) * PageSize + 1;
                                    int iEnd = PageIndex * PageSize;



                                    sql = string.Format(@"select * from (
                        
                                                       SELECT ROW_NUMBER() OVER(ORDER BY id desc) as row,* from OperatorLog 
                                                     
                                                       where 1=1 {0}) tt where row>= @iStart and row<= @iEnd
                                             ", sqlWhere);

                                    param.Add(new SqlParameter() { ParameterName = "@iStart", Value = iStart });
                                    param.Add(new SqlParameter() { ParameterName = "@iEnd", Value = iEnd });


                                    DataTable dtLoginLog = efhelp.QueryDataTable(sql, param.ToArray());


                                    if (dtLoginLog.Rows.Count > 0)
                                    {
                                        int totalCount = totalRows;
                                        TotalPage = (totalCount % PageSize == 0) ? (totalCount / PageSize) : ((totalCount / PageSize) + 1);
                                        list = ConvertType.ConvertToModel<OperatorLogModel>(dtLoginLog);
                                    }

                                    IsoDateTimeConverter timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
                                    PaginatorParameter<OperatorLogModel> jsonParam = new PaginatorParameter<OperatorLogModel>() { list = list, PageIndex = PageIndex, PageSize = PageSize, TotalPage = TotalPage };
                                    String rtnJsonString = JsonConvert.SerializeObject(jsonParam, timeConverter);
                                    context.Response.Write(rtnJsonString);
                                }
                                else
                                {
                                    PaginatorParameter<OperatorLogModel> jsonParam = new PaginatorParameter<OperatorLogModel>() { list = new List<OperatorLogModel>(), PageIndex = 1, PageSize = PageSize, TotalPage = 1 };
                                    String rtnJsonString = JsonConvert.SerializeObject(jsonParam);
                                    context.Response.Write(rtnJsonString);
                                }
                            }
                        }
                    }
                    else
                    {
                        var Operatorlist = new List<OperatorLogModel>();
                        PaginatorParameter<OperatorLogModel> jsonParam = new PaginatorParameter<OperatorLogModel>() { list = Operatorlist, PageIndex = 1, PageSize = 1, TotalPage = 1 };
                        String rtnJsonString = JsonConvert.SerializeObject(jsonParam);
                        context.Response.Write(rtnJsonString);
                    }
                }
                else if (method.Equals("userErrorlogbind"))
                {
                    List<errorLoginRecordModel> list = new List<errorLoginRecordModel>();

                    if (rightsId.FindAll(a => a.Equals("0301")).Count() > 0)//用户错误日志
                    {
                        var stream = request.InputStream;
                        stream.Position = 0;
                        using (var streamReader = new StreamReader(stream, System.Text.Encoding.UTF8))
                        {

                            string jsonString = streamReader.ReadToEnd();
                            PaginatorRequestModel d = JsonConvert.DeserializeObject<PaginatorRequestModel>(jsonString);
                            loginLogpaginatorModel m = JsonConvert.DeserializeObject<loginLogpaginatorModel>(d.data);


                            StringBuilder sqlWhere = new StringBuilder();
                            List<SqlParameter> param = new List<SqlParameter>();


                            if (m.loginName != "")
                            {
                                sqlWhere.Append(string.Format(" and login=@login"));
                                param.Add(new SqlParameter() { ParameterName = "@login", Value = m.loginName });
                            }

                            if (m.loginTime != "")
                            {
                                sqlWhere.Append(string.Format(" and lastTime=@lastTime"));
                                param.Add(new SqlParameter() { ParameterName = "@lastTime", Value = m.loginTime });
                            }

                            if (m.loginIp != "")
                            {
                                sqlWhere.Append(string.Format(" and ipAddress=@ipAddress"));
                                param.Add(new SqlParameter() { ParameterName = "@loginIp", Value = m.loginIp });
                            }

                            string sql = string.Format(@"select * from t_errorLoginRecord where 1=1 {0}", sqlWhere);
                            DataTable dtLoginLog = efhelp.QueryDataTable(sql, param.ToArray());
                            if (dtLoginLog.Rows.Count > 0)
                            {
                                list = ConvertType.ConvertToModel<errorLoginRecordModel>(dtLoginLog);
                                IsoDateTimeConverter timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
                                String rtnJsonString = JsonConvert.SerializeObject(list, timeConverter);
                                context.Response.Write(rtnJsonString);
                            }
                            else
                            {
                                String rtnJsonString = JsonConvert.SerializeObject(list);
                                context.Response.Write(rtnJsonString);
                            }
                        }
                    }
                    else
                    {
                        String rtnJsonString = JsonConvert.SerializeObject(list);
                        context.Response.Write(rtnJsonString);
                    }
                }
                else if (method.Equals("usersInfoByuserID"))
                {
                    if (request["userID"] != null)
                    {
                        string rtnJsonString = string.Empty;
                        List<UsersModel> UserInfo = new List<UsersModel>();
                        List<ExpressionModelField> fieldWhere = new List<ExpressionModelField>();
                        fieldWhere.Add(new ExpressionModelField() { Name = "userID", Value = Convert.ToInt16(request["userID"]) });
                        UserInfo = efhelp.GetList<UsersModel>(fieldWhere.ToArray());
                        if (UserInfo.Count > 0)
                        {
                            rtnJsonString = JsonConvert.SerializeObject(UserInfo.First());
                        }
                        context.Response.Write(rtnJsonString);
                    }
                }
                else if (method.Equals("saveUserInfo"))
                {
                    jsonMessageModel jm = new jsonMessageModel();
                    jm.status = "ok";

                    UsersModel model = new UsersModel();

                    if (request["userID"] != null)
                    {
                        model.userID = Convert.ToInt16(request["userID"]);
                        bool havingRight = true;

                        if (model.userID > 0)
                        {
                            if (rightsId.FindAll(a => a.Equals("0102")).Count() == 0)//修改帐号
                            {
                                havingRight = false;
                                jm.status = "error";
                            }
                        }
                        else
                        {
                            if (rightsId.FindAll(a => a.Equals("0101")).Count == 0)//0101
                            {
                                havingRight = false;
                                jm.status = "error";
                            }
                        }

                        if (!havingRight)
                        {
                            jm.message = "无操作权限,请联系管理员！";
                        }
                        else
                        {
                            if (request["deptID"] != null && request["userGroupID"] != null && request["userName"] != null && request["userCnName"] != null && request["userEmail"] != null)
                            {
                                model.deptID = request["deptID"].ToString();
                                model.userGroupID = Convert.ToInt16(request["userGroupID"]);
                                model.userName = request["userName"].ToString();
                                model.userCnName = request["userCnName"].ToString();
                                model.userEmail = request["userEmail"].ToString();

                                if (model.userID > 0)
                                {
                                    string sql = userSqlHelper.updateUserInfo();
                                    List<SqlParameter> para = new List<SqlParameter>();

                                    para.Add(new SqlParameter() { ParameterName = "@deptID", Value = model.deptID });
                                    para.Add(new SqlParameter() { ParameterName = "@userGroupID", Value = model.userGroupID });
                                    para.Add(new SqlParameter() { ParameterName = "@userName", Value = model.userName });
                                    para.Add(new SqlParameter() { ParameterName = "@userCnName", Value = model.userCnName });
                                    para.Add(new SqlParameter() { ParameterName = "@userEmail", Value = model.userEmail });
                                    para.Add(new SqlParameter() { ParameterName = "@userID", Value = model.userID });
                                    try
                                    {
                                        efhelp.ExecuteSql(sql, para.ToArray());
                                        operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【修改】对{0}帐号进行了操作", model.userName));
                                    }
                                    catch (Exception err)
                                    {
                                        jm.status = "error";
                                        jm.message = err.Message.ToString();
                                    }

                                }
                                else
                                {
                                    model.addDate = DateTime.Now;
                                    model.userPassword = SecurityHelper.Md5("a123456A");
                                    try
                                    {
                                        efhelp.Add(model);
                                        operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【创建新帐号】帐号名称为{0}", model.userName));
                                    }
                                    catch (Exception err)
                                    {
                                        jm.status = "error";
                                        jm.message = err.Message.ToString();
                                    }

                                }
                            }
                            else
                            {
                                jm.status = "error";
                                jm.message = "非法访问";
                            }
                        }
                    }
                    else
                    {
                        jm.status = "error";
                        jm.message = "非法访问";
                    }

                    context.Response.Write(JsonConvert.SerializeObject(jm));

                }
                else if (method.Equals("resetPassword"))
                {
                    jsonMessageModel jm = new jsonMessageModel();
                    jm.status = "ok";

                    if (rightsId.FindAll(a => a.Equals("0104")).Count() == 0)//帐号密码重置
                    {
                        jm.status = "error";
                        jm.message = "无权限操作,请联系管理员！";
                    }
                    else
                    {
                        if (request["userID"] != null && request["userName"] != null)
                        {
                            Int16 userID = Convert.ToInt16(request["userID"]);
                            string userName = request["userName"].ToString();
                            string userPassword = SecurityHelper.Md5("a123456A");
                            string sql = userSqlHelper.resetPassword();
                            List<SqlParameter> para = new List<SqlParameter>();

                            para.Add(new SqlParameter() { ParameterName = "@userID", Value = userID });
                            para.Add(new SqlParameter() { ParameterName = "@userPassword", Value = userPassword });

                            try
                            {
                                efhelp.ExecuteSql(sql, para.ToArray());
                                operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【密码重置】对{0}帐号进行了操作", userName));
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
                else if (method.Equals("delUserByID"))
                {
                    jsonMessageModel jm = new jsonMessageModel();
                    jm.status = "ok";

                    if (rightsId.FindAll(a => a.Equals("0103")).Count() == 0)//删除帐号
                    {
                        jm.status = "error";
                        jm.message = "无权限操作,请联系管理员！";
                    }
                    else
                    {
                        if (request["userID"] != null && request["userName"] != null)
                        {
                            Int16 userID = Convert.ToInt16(request["userID"]);
                            string userName = request["userName"].ToString();
                            string sql = userSqlHelper.delUser();
                            List<SqlParameter> para = new List<SqlParameter>();
                            para.Add(new SqlParameter() { ParameterName = "@userID", Value = userID });
                            try
                            {
                                efhelp.ExecuteSql(sql, para.ToArray());
                                operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【删除帐号】对{0}帐号进行了操作", userName));
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
                else if (method.Equals("unlock"))
                {
                    jsonMessageModel jm = new jsonMessageModel();
                    jm.status = "ok";

                    if (rightsId.FindAll(a => a.Equals("0302")).Count() == 0)//登录解锁
                    {
                        jm.status = "error";
                        jm.message = "无权限操作,请联系管理员！";
                    }
                    else
                    {
                        if (request["id"] != null && request["username"] != null)
                        {
                            int id = Convert.ToInt32(request["id"]);
                            string username = request["username"].ToString();
                            string sql = userSqlHelper.delUnlock();
                            List<SqlParameter> para = new List<SqlParameter>();
                            para.Add(new SqlParameter() { ParameterName = "@id", Value = id });
                            try
                            {
                                efhelp.ExecuteSql(sql, para.ToArray());
                                operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【帐号解锁】对{0}帐号进行了操作", username));
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
                else if (method.Equals("userGroups"))
                {
                    List<ExpressionModelField> fieldWhere = new List<ExpressionModelField>();
                    fieldWhere.Add(new ExpressionModelField() { Name = "isDeleted", Value = false });
                    List<UserGroups_model> userGroupsList = efhelp.GetList<UserGroups_model>(fieldWhere.ToArray());
                    context.Response.Write(JsonConvert.SerializeObject(userGroupsList));
                }
                else if (method.Equals("setUserRights"))
                {
                    jsonMessageModel jm = new jsonMessageModel();
                    jm.status = "ok";

                    if (rightsId.FindAll(a => a.Equals("0201")).Count() == 0)//权限分配
                    {
                        jm.status = "error";
                        jm.message = "无权限操作,请联系管理员！";
                    }
                    else
                    {

                        if (request["userGroupID"] != null && request["rightsID"] != null && request["userGroupName"] != null)
                        {
                            string rightsid = request["rightsID"];
                            if (rightsid.Length > 0)
                            {
                                int groupsid = Convert.ToInt16(request["userGroupID"]);
                                string userGroupName = request["userGroupName"].ToString();
                                List<string> sql = new List<string>();
                                sql.Add(userSqlHelper.resetGroupsRights(groupsid));
                                foreach (string s in rightsid.Split(','))
                                {
                                    sql.Add(userSqlHelper.setUserRights(groupsid, s));
                                }

                                try
                                {
                                    efhelp.ExecuteTransaction(sql, null);
                                    operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【权限设置】 对{0}角色组进行了操作", userGroupName));
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
                                jm.message = "至少指定一个权限操作才能进行分配";
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
                else if (method.Equals("getGroupsRights"))
                {
                    if (request["userGroupID"] != null)
                    {
                        string sql = userSqlHelper.getGroupRight();
                        List<SqlParameter> para = new List<SqlParameter>();
                        para.Add(new SqlParameter() { ParameterName = "@groupid", Value = request["userGroupID"].ToString() });
                        List<reqGroupRightModel> userGroupsRightsList = efhelp.QueryList<reqGroupRightModel>(sql, para.ToArray());
                        context.Response.Write(JsonConvert.SerializeObject(userGroupsRightsList).Replace("Checked", "checked"));
                    }

                }
                else if (method.Equals("modify"))
                {
                    jsonMessageModel jm = new jsonMessageModel();
                    jm.status = "ok";

                    if (rightsId.FindAll(a => a.Equals("0401")).Count() == 0)//密码修改
                    {
                        jm.status = "error";
                        jm.message = "无权限操作,请联系管理员！";
                    }
                    else
                    {
                        if (request["oldPwd"] != null && request["newPwd"] != null && request["again"] != null)
                        {
                            try
                            {
                                JsEncryptHelper jsHelper = new helper.JsEncryptHelper();
                                string oldPwd = request["oldPwd"] + "";
                                string newPwd = request["newPwd"] + "";

                                oldPwd = jsHelper.Decrypt(oldPwd);
                                newPwd = jsHelper.Decrypt(newPwd);
                                short user = curUserModel.userID;

                                string sql = userSqlHelper.updatePwd();
                                List<SqlParameter> para = new List<SqlParameter>();
                                para.Add(new SqlParameter() { ParameterName = "@userID", Value = user });
                                para.Add(new SqlParameter() { ParameterName = "@oldUserPassword", Value = SecurityHelper.Md5(oldPwd) });
                                para.Add(new SqlParameter() { ParameterName = "@userPassword", Value = SecurityHelper.Md5(newPwd) });
                                if (efhelp.ExecuteSql(sql, para.ToArray()) > 0)
                                {
                                    operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【密码修改】: 对自己的帐号进行了操作"));

                                }
                                else
                                {
                                    jm.status = "error";
                                    jm.message = "原密码错误";
                                }
                            }

                            catch (Exception err)
                            {
                                jm.status = "error";
                                jm.message = err.Message.ToString();
                            }
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