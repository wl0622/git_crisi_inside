
using crsri.cn.Model;
using inside.admin.web.ashx;
using inside.admin.web.entityframework;
using inside.admin.web.entityframework.sqlstringhelper;
using inside.admin.web.entityframework.tableEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace inside.admin.web.aspx
{
    public partial class releasearticle : basepage
    {
        public List<t_subject_list_model> subjectlist = new List<t_subject_list_model>();
        public string curUserCnName = string.Empty;
        public string curUserbriefName = string.Empty;
        public List<t_special_list_model> speciallist = new List<t_special_list_model>();
        public List<t_web_jhxy_model> jhxyItems = new List<t_web_jhxy_model>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!this.havingRights)
            //{
            //    Response.Redirect("noRights.aspx");
            //}

            if (!IsPostBack)
            {

                //验证是用户状态是否过期
                if (this.curUserModel != null)
                {
                    EFHELP efhelp = new EFHELP();
                    //获取首页栏目
                    subjectlist = efhelp.QueryList<t_subject_list_model>(subjectSqlhelper.HomeSubjectSqlString);
                    subjectlist.Add(new t_subject_list_model() { subjectID = "005", subjectName = "产品与技术", child = 0, linkUrl = null, isElite = false, orderID = 0, layoutID = 1 });
                    curUserCnName = this.curUserModel.userCnName;
                    curUserbriefName = ashxHelper.getDepartment(this.curUserModel.deptID).First().deptName;

                    jhxyItems = efhelp.QueryList<t_web_jhxy_model>(jhxySqlhelper.JHXYItemSqlString);


                    //专题的栏目
                    List<ExpressionModelField> fieldWhere = new List<ExpressionModelField>();
                    fieldWhere.Add(new ExpressionModelField() { Name = "spID", Value = Convert.ToInt16(0), Relation = EnumRelation.GreaterThan });

                    List<t_special_list_model> list = efhelp.GetList<t_special_list_model>(fieldWhere.ToArray());

                    //遍历一级菜单
                    list.FindAll(a => a.specialID.Length == 3).ForEach(delegate(t_special_list_model parent)
                    {
                        List<t_special_list_model> children = list.FindAll(b => b.specialID.Substring(0, 3) == parent.specialID && b.specialID.Length > 3);
                        speciallist.Add(parent);
                        children.ForEach(delegate(t_special_list_model c)
                        {
                            speciallist.Add(c);
                        });
                    });
                }
                else
                {
                    Response.Write("<script>window.parent.location.href='/login/login.aspx'</script>");
                }
            }
        }
    }
}