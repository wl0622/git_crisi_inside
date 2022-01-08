
using inside.crsri.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace inside.crsri.Filters
{
    public class XSSFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// OnActionExecuting
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //获取参数集合
            var ps = context.ActionDescriptor.GetParameters();
            if (ps.Count() == 0)
            {
                return;
            }
            //遍历参数集合
            foreach (var p in ps)
            {
                if (context.ActionParameters[p.ParameterName] != null)
                {
                    //当参数是str
                    if (p.ParameterType.Equals(typeof(string)))
                    {
                        context.ActionParameters[p.ParameterName] = XSSHelper.XssFilter(context.ActionParameters[p.ParameterName].ToString());
                    }
                }
            }
        }
    }
}