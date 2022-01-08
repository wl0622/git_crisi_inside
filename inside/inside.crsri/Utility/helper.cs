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
        /// 取得HTML中所有图片的 URL。 
        /// </summary> 
        /// <param name="sHtmlText">HTML代码</param> 
        /// <returns>图片的URL列表</returns> 
        public static string HtmlImgUrlReplace(string sHtmlText, string oldURL, string newUrl)
        {
            // 定义正则表达式用来匹配 img 标签 
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串 
            MatchCollection matches = regImg.Matches(sHtmlText);
            // 取得匹配项列表 
            foreach (Match match in matches)
                sHtmlText = sHtmlText.Replace(match.Value, match.Value.Replace(oldURL, newUrl));
            return sHtmlText;
        }

        public static string LinkUrlReplace(string sHtmlText, string oldLink, string newLink)
        {
            Regex regLink = new Regex(@"\<a([^\>]+|\s+)href\s*=\s*((""[^""]*"")|(\'[^\']*\')|[^>^\s]+)");
            // 搜索匹配的字符串 
            MatchCollection matches = regLink.Matches(sHtmlText);
            // 取得匹配项列表 
            foreach (Match match in matches)
                sHtmlText = sHtmlText.Replace(match.Value, match.Value.Replace(oldLink, newLink));
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

        /// <summary>
        /// 获取字符串内的数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetNums(string str)
        {
            var reg = new Regex(@"[0-9]+");
            var ms = reg.Matches(str);
            string[] nums = new string[ms.Count];
            for (int i = 0, len = nums.Length; i < len; i++)
            {
                nums[i] = ms[i].Value;
            }
            return Convert.ToInt32(string.Join("\n", nums));
        }

        /// <summary>
        /// 识别urlStr是否是网络路径
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsHttp(string url)
        {
            if (url.Contains("http://"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<string> GetImgAll(string sHtmlText)
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

    }
}