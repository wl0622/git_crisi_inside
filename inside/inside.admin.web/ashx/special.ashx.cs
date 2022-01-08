using inside.admin.web.entityframework;
using inside.admin.web.entityframework.reqmodel;
using inside.admin.web.entityframework.sqlstringhelper;
using inside.admin.web.entityframework.tableEntity;
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
    /// Summary description for special
    /// </summary>
    public class special : IHttpHandler, IRequiresSessionState
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
                if (method.Equals("getSpecialComboTree"))
                {

                    List<reqComboTreeModel> comboTree = new List<reqComboTreeModel>();

                    List<ExpressionModelField> fieldWhere = new List<ExpressionModelField>();
                    fieldWhere.Add(new ExpressionModelField() { Name = "english", Value = false });

                    List<OrderModelField> orderField = new List<OrderModelField>();
                    orderField.Add(new OrderModelField() { PropertyName = "specialID", IsDesc = false });

                    List<t_special_list_model> list = efhelp.GetList<t_special_list_model>(fieldWhere.ToArray());

                    List<t_special_list_model> parentNode = list.FindAll(a => a.specialID.Length == 3);
                    foreach (t_special_list_model m in parentNode)
                    {
                        reqComboTreeModel t = new reqComboTreeModel();

                        t.text = m.specialName;
                        t.id = m.specialID;

                        List<t_special_list_model> children = list.FindAll(a => a.specialID.Substring(0, 3) == m.specialID && a.specialID.Length > 3);
                        if (children.Count > 0)
                        {
                            t.children = new List<ComboTreeModel>();

                            foreach (t_special_list_model c in children)
                            {
                                ComboTreeModel sct = new ComboTreeModel() { id = c.specialID, text = c.specialName };
                                t.children.Add(sct);
                            }
                        }
                        comboTree.Add(t);
                    }
                    comboTree = comboTree.OrderBy(a => a.id).ToList();
                    String rtnJsonString = JsonConvert.SerializeObject(comboTree);
                    context.Response.Write(rtnJsonString);
                }
                else if (method.Equals("specialConfigTitle"))
                {
                    jsonMessageModel jm = new jsonMessageModel();
                    jm.status = "ok";
                    bool ishaving = true;
                    if (request["id"] != null)
                    {
                        string id = request["id"].ToString();

                        if (id.Equals("0"))
                        {
                            if (rightsId.FindAll(a => a.Equals("01001")).Count() == 0)
                            {
                                ishaving = false;
                            }
                        }
                        else
                        {
                            if (rightsId.FindAll(a => a.Equals("01002")).Count() == 0)
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
                        if (request["parent"] != null && request["title"] != null)
                        {
                            string parent = request["parent"].ToString();
                            string title = request["title"].ToString();
                            string sql = string.Empty;
                            List<SqlParameter> para = new List<SqlParameter>();

                            if (id.Equals("0"))
                            {
                                sql = specialSqlhelper.addSpecialItem(parent, title);

                            }
                            else
                            {
                                sql = specialSqlhelper.updateSpecialItem(id, title);

                            }

                            try
                            {
                                efhelp.ExecuteSql(sql, para.ToArray());
                                if (id.Equals("0"))
                                {
                                    operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【添加专题子栏目】 {0}", title));
                                }
                                else
                                {
                                    operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【添加专题子栏目】 {0}", title));
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
                else if (method.Equals("delSpecialItem"))
                {
                    jsonMessageModel jm = new jsonMessageModel();
                    jm.status = "ok";

                    if (rightsId.FindAll(a => a.Equals("01003")).Count() == 0)
                    {
                        jm.status = "error";
                        jm.message = "无权限操作";
                    }
                    else
                    {
                        if (request["id"] != null && request["itemName"] != null)
                        {
                            string id = request["id"];
                            string itemName = request["itemName"];

                            string sql = specialSqlhelper.delSpecialItem();
                            List<SqlParameter> para = new List<SqlParameter>();
                            para.Add(new SqlParameter() { ParameterName = "@specId", Value = id });
                            try
                            {
                                efhelp.ExecuteSql(sql, para.ToArray());
                                operatorloghelper.insertOperatorLog(curUserModel.userName, string.Format("【操作删除子栏目】{0}", itemName));
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