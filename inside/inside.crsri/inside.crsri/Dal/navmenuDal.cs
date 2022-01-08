using crsri.cn.DbObject;
using crsri.cn.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.crsri.Dal
{
    public class navmenuDal : baseDal
    {
        /// <summary>
        /// 导航菜单
        /// </summary>
        /// <returns></returns>
        public static List<t_navmenu_model> requestNavMenuItems()
        {
            List<ExpressionModelField> field_And = new List<ExpressionModelField>();
            field_And.Add(new ExpressionModelField() { Name = "id", Value = 0, Relation = EnumRelation.GreaterThan });
            List<t_navmenu_model> reqlist = CrsriEntityFramework.GetList<t_navmenu_model>(field_And.ToArray(), new OrderModelField[] { });
            return reqlist;

        }
    }
}