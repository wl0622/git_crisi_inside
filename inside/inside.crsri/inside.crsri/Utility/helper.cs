using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace inside.crsri.Utility
{
    public class helper
    {
        public static string sitePicPath = ConfigurationManager.AppSettings["sitePicPath"].ToString();
        public static string domainName = ConfigurationManager.AppSettings["domainName"].ToString();
        public static string specialDomainName = ConfigurationManager.AppSettings["specialDomainName"].ToString();


        //正则替换整个IMG标签
        public static string ReplaceHtmlImgTag(string sHtmlText)
        {
            //List<string> list = new List<string>();
            //// 定义正则表达式用来匹配 img 标签 
            //Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            Regex regHTML = new Regex(@"<[^>]+>");
            //// 搜索匹配的字符串 
            MatchCollection matches = regHTML.Matches(sHtmlText);
            //// 取得匹配项列表 
            //foreach (Match match in matches)
            //{
            //    sHtmlText = sHtmlText.Replace(match.Value, "");
            //}


            matches = regHTML.Matches(sHtmlText);
            foreach (Match match in matches)
            {
                sHtmlText = sHtmlText.Replace(match.Value, "");
            }

            return sHtmlText;
        }

        /// <summary>
        /// 替换标签外的空格
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ReplaceSpaces(string s)
        {
            return Regex.Replace(s, @">[^>]*<", delegate(Match match)
            {
                string v = match.ToString();
                v = v.Replace("　", "").Replace(" ", "");
                v = Regex.Replace(v, @"\s", "");
                v = v.Replace("&nbsp;", "");
                return v;
            });
        }

        // <summary>
        /// 将字符串转换成数字字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UrlEncry(int val)
        {
            string value = val.ToString();
            StringBuilder sb = new StringBuilder();

            foreach (char c in value)
            {
                int cAscil = (int)c;
                sb.Append(Convert.ToString(c, 8) + "9");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 将数字字符串转换成普通字符字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int32 UrlDecrypt(string value)
        {
            string[] splitInt = value.ToString().Split(new char[] { '9' }, StringSplitOptions.RemoveEmptyEntries);

            var splitChars = splitInt.Select(s => Convert.ToChar(
                                              Convert.ToInt32(s, 8)
                                            ).ToString());

            return Convert.ToInt32(string.Join("", splitChars));
        }

    }
}