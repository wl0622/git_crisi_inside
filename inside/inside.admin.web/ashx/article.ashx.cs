
using inside.admin.web.entityframework;
using inside.admin.web.entityframework.reqmodel;
using inside.admin.web.entityframework.sqlstringhelper;
using inside.admin.web.entityframework.tableEntity;
using inside.admin.web.model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace inside.admin.web.ashx
{
    /// <summary>
    /// Summary description for article
    /// </summary>
    public class article : IHttpHandler, IRequiresSessionState
    {
        //正则匹配整个IMG标签
        public List<string> GetImgAll(string sHtmlText)
        {
            List<string> list = new List<string>();
            // 定义正则表达式用来匹配 img 标签 
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串 
            MatchCollection matches = regImg.Matches(sHtmlText);
            // 取得匹配项列表 
            foreach (Match match in matches)
            {
                list.Add(match.Groups["imgUrl"].Value);
            }
            return list;
        }

        public bool IsContainsAttachment(List<string> imgPath)
        {
            bool IsContains = false;
            foreach (string s in imgPath)
            {
                if (s.Contains("/ueditor/1.4.3/dialogs/attachment/fileTypeImages/"))
                {
                    IsContains = true;
                    break;
                }
            }
            return IsContains;
        }


        public bool IsContainsPic(List<string> imgPath)
        {
            bool IsContains = false;
            foreach (string s in imgPath)
            {
                if (s.Contains("/ueditor/1.4.3/net/upload/image/"))
                {
                    IsContains = true;
                    break;
                }
            }
            return IsContains;
        }


        public t_article_list_model add_form_model(string formjsondata)
        {
            List<formModel> FormData = JsonConvert.DeserializeObject<List<formModel>>(formjsondata);
            articleFormModel model = new articleFormModel();
            PropertyInfo[] propertys = model.GetType().GetProperties();
            foreach (PropertyInfo property in propertys)
            {
                var val = FormData.FindAll(a => a.name == property.Name);
                if (val.Count > 0)
                {
                    property.SetValue(model, val.First().value, null);
                }
            }
            t_article_list_model article_model = new t_article_list_model();
            if (model.subjectID != "")
            {
                article_model.subjectID = model.subjectID;
            }

            if (model.specialID != "")
            {
                article_model.specialID = model.specialID;
            }

            article_model.title = model.title;


            if (model.keywords != "")
            {
                article_model.keywords = model.keywords;
            }

            article_model.isOnTop = model.isOnTop == "on" ? true : false;

            if (model.titletoutiao != "")
            {
                article_model.titletoutiao = model.titletoutiao;
            }

            if (model.ueditorValue != "")
            {
                article_model.content = model.ueditorValue;
            }

            if (model.author != "")
            {
                article_model.author = model.author;
            }

            if (model.ckIsreprint == "on")
            {
                article_model.releaseDep = model.releaseDep_reprint;
                article_model.reprint = model.reprint;
            }
            else
            {
                article_model.releaseDep = model.releaseDep;
                article_model.reprint = "";
            }

            if (model.releaseTime != "")
            {
                article_model.releaseTime = Convert.ToDateTime(model.releaseTime);
                article_model.updateTime = DateTime.Now;
            }
            else
            {
                article_model.releaseTime = null;
                article_model.updateTime = null;
            }

            if (model.editor != "")
            {
                article_model.editor = model.editor;
            }

            if (model.editorDep != "")
            {
                article_model.editorDep = model.editorDep;
            }

            if (model.JHXYid != "")
            {
                article_model.JHXYid = model.JHXYid;
            }

            List<string> imglist = this.GetImgAll(article_model.content);
            article_model.isIncludePic = IsContainsPic(imglist);
            article_model.isIncludeAcc = IsContainsAttachment(imglist);
            article_model.hits = 0;
            article_model.isDeleted = false;
            article_model.isHot = false;
            article_model.isPicxw = model.isPicxw == "on" ? true : false;
            article_model.isTop = model.isTop == "on" ? true : false;
            article_model.isElite = false;
            article_model.isPassed = false;
            article_model.isexarticle = false;
            article_model.isEnglishpic = false;
            article_model.isEngpic = false;
            article_model.isspecialpicxw = false;
            article_model.IsNewsTop = false;
            article_model.issync = model.issync == "on" ? true : false;

            return article_model;
        }

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
                //发布新文稿
                if (method.Equals("release"))
                {
                    if (request["jsondata"] != null)
                    {
                        t_article_list_model article_model = add_form_model(request["jsondata"].ToString());

                        if (request["modeldata"] != null)
                        {
                            string reqModelJsonString = request["modeldata"].ToString();
                            if (reqModelJsonString != "")
                            {
                                t_article_list_model dbModel = JsonConvert.DeserializeObject<t_article_list_model>(reqModelJsonString);
                                article_model.articleID = dbModel.articleID;
                                article_model.hits = dbModel.hits;
                                article_model.updateTime = DateTime.Now;
                                article_model.isDeleted = dbModel.isDeleted;
                                article_model.isPassed = false;
                            }
                        }
                        jsonMessageModel jm = new jsonMessageModel();
                        jm.status = "ok";

                        bool ishaving = true;

                        if (article_model.articleID == 0)
                        {
                            if (rightsId.FindAll(a => a.Equals("0501")).Count() == 0)
                            {
                                ishaving = false;
                            }
                        }
                        else if (article_model.articleID > 0)
                        {
                            if (rightsId.FindAll(a => a.Equals("0502")).Count() == 0)
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
                            try
                            {
                                if (article_model.articleID == 0)
                                {
                                    t_article_list_model t = efhelp.AddEntity(article_model);
                                    article_model.articleID = t.articleID;
                                    operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【发布新文章】 文章标题:{0}", article_model.title));
                                }
                                else if (article_model.articleID > 0)
                                {
                                    efhelp.Update(article_model);
                                    operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【修改文章】 文章标题:{0}", article_model.title));
                                }

                                if (article_model.issync == true)//同步外网
                                {
                                    EFHELP efhelpOutSide = new EFHELP(SQLString.outSideDBString);
                                    string sql = articleSqlhelper.syncOutSide();
                                    List<SqlParameter> para = new List<SqlParameter>();
                                    para.Add(new SqlParameter() { ParameterName = "@articleID", Value = article_model.articleID });
                                    efhelpOutSide.ExecuteSql(sql, para.ToArray());
                                    operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【数据同步】 文章标题:{0}", article_model.title));
                                }
                            }
                            catch (Exception err)
                            {
                                jm.status = "error";
                                jm.message = err.Message.ToString();
                            }
                        }
                        context.Response.Write(JsonConvert.SerializeObject(jm));
                    }
                }
                //文稿列表绑定
                else if (method.Equals("articlelistbind"))
                {
                    var stream = request.InputStream;
                    stream.Position = 0;
                    using (var streamReader = new StreamReader(stream, System.Text.Encoding.UTF8))
                    {
                        string jsonString = streamReader.ReadToEnd();
                        PaginatorRequestModel d = JsonConvert.DeserializeObject<PaginatorRequestModel>(jsonString);
                        articlepaginatorModel m = JsonConvert.DeserializeObject<articlepaginatorModel>(d.data);

                        int TotalPage = 0;
                        List<t_article_list_model> list = new List<t_article_list_model>();

                        int PageIndex = Convert.ToInt32(d.PageIndex);
                        int PageSize = Convert.ToInt32(d.PageSize);

                        List<OrderModelField> orderField = new List<OrderModelField>();
                        orderField.Add(new OrderModelField() { PropertyName = "updateTime", IsDesc = true });

                        List<ExpressionModelField> fieldWhere = new List<ExpressionModelField>();
                        fieldWhere.Add(new ExpressionModelField() { Name = "isDeleted", Value = false });
                        if (m.title != null) { if (m.title != "") { fieldWhere.Add(new ExpressionModelField() { Name = "title", Value = m.title, Relation = EnumRelation.Contains }); } }
                        if (m.keyword != null) { if (m.keyword != "") { fieldWhere.Add(new ExpressionModelField() { Name = "keywords", Value = m.keyword, Relation = EnumRelation.Contains }); } }
                        if (m.subjectID != null) { if (m.subjectID != "") { fieldWhere.Add(new ExpressionModelField() { Name = "subjectID", Value = m.subjectID }); } }
                        if (m.author != null) { if (m.author != "") { fieldWhere.Add(new ExpressionModelField() { Name = "author", Value = m.author }); } }
                        if (m.editor != null) { if (m.editor != "") { fieldWhere.Add(new ExpressionModelField() { Name = "editor", Value = m.editor }); } }
                        if (m.isPassed != null)
                        {
                            if (m.isPassed != "")
                            {
                                fieldWhere.Add(new ExpressionModelField() { Name = "isPassed", Value = m.isPassed == "1" ? true : false });
                                if (m.isPassed != "1")
                                {
                                    fieldWhere.Add((new ExpressionModelField() { Name = "updateTime", Value = Convert.ToDateTime("2019-12-20 00:00:00"), Relation = EnumRelation.GreaterThan }));
                                }
                            }
                        }
                        if (m.articleID != null) { if (m.articleID != "") { fieldWhere.Add(new ExpressionModelField() { Name = "articleID", Value = Convert.ToInt32(m.articleID) }); } }
                        if (m.isPicxw != null) { if (m.isPicxw != "") { fieldWhere.Add(new ExpressionModelField() { Name = "isPicxw", Value = m.isPicxw == "1" ? true : false }); } }
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic = efhelp.GetListPaged<t_article_list_model>(PageIndex, PageSize, fieldWhere.ToArray(), orderField.ToArray());
                        var json = new { total = 0, rows = new List<t_article_list_model>() };

                        if (dic != null)
                        {
                            int totalCount = (int)dic["total"];
                            TotalPage = (totalCount % PageSize == 0) ? (totalCount / PageSize) : ((totalCount / PageSize) + 1);
                            list = dic["rows"] as List<t_article_list_model>;
                        }

                        IsoDateTimeConverter timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
                        PaginatorParameter<t_article_list_model> param = new PaginatorParameter<t_article_list_model>() { list = list, PageIndex = PageIndex, PageSize = PageSize, TotalPage = TotalPage };
                        String rtnJsonString = JsonConvert.SerializeObject(param, timeConverter);

                        context.Response.Write(rtnJsonString);
                    }
                }
                //审核文稿
                else if (method.Equals("audit"))
                {
                    jsonMessageModel jm = new jsonMessageModel();
                    jm.status = "ok";

                    if (rightsId.FindAll(a => a.Equals("0504")).Count() == 0)
                    {
                        jm.status = "error";
                        jm.message = "无权限操作,请联系管理员";
                    }
                    else
                    {
                        if (request["articleID"] != null && request["userCnName"] != null && request["title"] != null)
                        {

                            try
                            {
                                int articleID = Convert.ToInt32(request["articleID"]);
                                string userCnName = request["userCnName"].ToString();
                                string articleTitle = request["title"].ToString();
                                string sql = articleSqlhelper.articleAuditSqlStriing(articleID, userCnName);
                                efhelp.ExecuteSql(sql);
                                operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【文章审核】文章标题:{0}", articleTitle));
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
                //删除文稿
                else if (method.Equals("delByArticle"))
                {
                    jsonMessageModel jm = new jsonMessageModel();
                    jm.status = "ok";

                    if (rightsId.FindAll(a => a.Equals("0503")).Count() == 0)
                    {
                        jm.status = "error";
                        jm.message = "无权限操作,请联系管理员";
                    }
                    else
                    {
                        if (request["articleID"] != null && request["articleTitle"] != null)
                        {

                            try
                            {
                                int articleID = Convert.ToInt32(request["articleID"]);
                                string articleTitle = request["articleTitle"].ToString();
                                string sql = articleSqlhelper.articleDeleteByID();
                                List<SqlParameter> para = new List<SqlParameter>();
                                para.Add(new SqlParameter() { ParameterName = "@articleID", Value = articleID });
                                efhelp.ExecuteSql(sql, para.ToArray());
                                operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【文章删除】文章标题:{0}", articleTitle));
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
                //获取文稿内容中所有的图片(图片新闻)
                else if (method.Equals("xwConfigPic"))
                {
                    List<reqXwPicModel> reqModelList = new List<reqXwPicModel>();

                    if (request["articleID"] != null)
                    {
                        try
                        {
                            int articleID = Convert.ToInt32(request["articleID"]);
                            List<ExpressionModelField> fieldWhere = new List<ExpressionModelField>();
                            fieldWhere.Add(new ExpressionModelField() { Name = "articleID", Value = Convert.ToInt32(articleID) });
                            List<t_article_list_model> list = efhelp.GetList<t_article_list_model>(fieldWhere.ToArray());

                            if (list.Count > 0)
                            {
                                t_article_list_model first = list.First();
                                List<string> imglist = this.GetImgAll(first.content);
                                foreach (string url in imglist)
                                {
                                    reqXwPicModel reqModel = new reqXwPicModel();
                                    reqModel.picUrl = url;
                                    if (first.defaultPicUrl != null)
                                    {
                                        reqModel.isDefault = first.defaultPicUrl.Substring(first.defaultPicUrl.LastIndexOf("/")) == url.Substring(url.LastIndexOf("/")) ? true : false;
                                    }
                                    else
                                    {
                                        reqModel.isDefault = false;
                                    }

                                    reqModelList.Add(reqModel);
                                }
                            }

                        }
                        catch (Exception err)
                        {
                            string msg = err.Message.ToString();
                        }
                    }
                    context.Response.Write(JsonConvert.SerializeObject(reqModelList));
                }
                //设置幻灯片的图片(图片新闻)
                else if (method.Equals("articleXwNesDefaultPic"))
                {

                    jsonMessageModel jm = new jsonMessageModel();
                    jm.status = "ok";

                    if (rightsId.FindAll(a => a.Equals("0602")).Count() == 0)
                    {
                        jm.status = "error";
                        jm.message = "无权限操作,请联系管理员";
                    }
                    else
                    {
                        if (request["defaultPicUrl"] != null && request["articleID"] != null && request["articleTitle"] != null)
                        {
                            try
                            {
                                string articleTitle = request["articleTitle"].ToString();
                                string oper = string.Format("【设置图片新闻轮播并指定图片】文章标题:{0}", articleTitle);
                                if (request["type"] != null)
                                {
                                    if (request["type"].ToString().Equals("产品"))
                                    {
                                        oper = string.Format("【设置首页产品图片】文章标题:{0}", articleTitle);
                                    }
                                }

                                int articleID = Convert.ToInt32(request["articleID"]);
                                string defaultPicUrl = request["defaultPicUrl"].ToString();


                                string sql = articleSqlhelper.articleXwPicDefault();
                                List<SqlParameter> para = new List<SqlParameter>();
                                para.Add(new SqlParameter() { ParameterName = "@articleID", Value = articleID });
                                para.Add(new SqlParameter() { ParameterName = "@defaultPicUrl", Value = defaultPicUrl });
                                efhelp.ExecuteSql(sql, para.ToArray());
                                operatorloghelper.insertOperatorLog(curUserModel.userName, oper);
                            }
                            catch (Exception err)
                            {
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
                else if (method.Equals("buildJHXY"))
                {
                    jsonMessageModel jm = new jsonMessageModel();
                    jm.status = "ok";

                    try
                    {
                        List<crsri.cn.Model.t_web_jhxy_model> list = efhelp.QueryList<crsri.cn.Model.t_web_jhxy_model>(jhxySqlhelper.JHXYMaxItemSqlString);
                        if (list.Count > 0)
                        {
                            crsri.cn.Model.t_web_jhxy_model m = list.First();
                            crsri.cn.Model.t_web_jhxy_model m_new = new crsri.cn.Model.t_web_jhxy_model();
                            m_new.volume = Convert.ToInt16(m.volume) == 4 ? "1" : Convert.ToString(Convert.ToInt16(m.volume) + 1);
                            m_new.year = Convert.ToInt16(m.volume) == 4 ? Convert.ToString(Convert.ToInt16(m.year) + 1) : m.year;
                            m_new.total_volume = StringHelper.NumToChinese(Convert.ToString(StringHelper.ParseCnToInt(m.total_volume) + 1));
                            m_new.volume_name = string.Format("{0}年第{1}期 总第{2}期", m_new.year, m_new.volume, m_new.total_volume);

                            List<SqlParameter> param = new List<SqlParameter>();
                            param.Add(new SqlParameter() { ParameterName = "@year", Value = m_new.year });
                            param.Add(new SqlParameter() { ParameterName = "@volume", Value = m_new.volume });
                            param.Add(new SqlParameter() { ParameterName = "@total_volume", Value = m_new.total_volume });
                            param.Add(new SqlParameter() { ParameterName = "@volume_name", Value = m_new.volume_name });

                            if (efhelp.ExecuteSql(jhxySqlhelper.JHXYSaveNewItem, param.ToArray()) > 0)
                            {
                                m_new.id=m.id + 1;
                                jm.message = JsonConvert.SerializeObject(m_new);
                            }
                            else
                            {
                                jm.status = "error";
                                jm.message = "创建失败";
                            }
                        }
                        else
                        {
                            jm.status = "error";
                            jm.message = "未获取到期次数据";
                        }
                    }
                    catch (Exception err)
                    {
                        jm.status = "error";
                        jm.message = err.Message.ToString();
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