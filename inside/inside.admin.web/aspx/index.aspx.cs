
using crsri.cn.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace inside.admin.web.aspx
{
    public partial class index : basepage
    {
        public List<t_web_siteCfg_model> cfgList
        { get; set; }

        public List<t_web_siteCfgPic_model> cfgPicList
        { get; set; }

        public List<t_web_siteCfgBase_model> cfgBaseList
        { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                crsri.cn.DbObject.EFHelp efhelp = new crsri.cn.DbObject.EFHelp();
                cfgList = efhelp.GetAll<t_web_siteCfg_model>();
                cfgPicList = efhelp.GetAll<t_web_siteCfgPic_model>();
                cfgBaseList = efhelp.GetAll<t_web_siteCfgBase_model>();
            }
        }
    }
}