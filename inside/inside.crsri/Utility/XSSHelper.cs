using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace inside.crsri.Utility
{
    public class XSSHelper
    {
        /// <summary>
        /// XSS过滤
        /// </summary>
        /// <param name="html">html代码</param>
        /// <returns>过滤结果</returns>
        public static string XssFilter(string html)
        {
            string str = HtmlFilter(html);
            return str;
        }

        /// <summary>
        /// 过滤HTML标记
        /// </summary>
        /// <param name="Htmlstring"></param>
        /// <returns></returns>
        public static string HtmlFilter(string Htmlstring)
        {
            string result = System.Web.HttpUtility.HtmlEncode(Htmlstring); ;
            return result;
        }
    }
}