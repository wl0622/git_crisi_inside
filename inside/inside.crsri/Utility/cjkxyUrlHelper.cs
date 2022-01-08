using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.crsri.Utility
{
    public class cjkxyUrlHelper
    {
        public static string adminWebUrl = "http://localhost:50000";
        public static string oldContentImgUrl = "http://10.6.66.100:8001/article/uploadpic/";
        public static string oldContentFilesUrl = "http://10.6.66.100:8001/Article/uploadfiles/";
        public static string oldContentIconUrl = "http://10.6.66.100:8001/ewebeditor/sysimage/file/";
        public static string articleOldImgUrlReplace(string htmlContent)
        {
            string content = helper.HtmlImgUrlReplace(htmlContent.ToLower(), "../article/uploadpic/", oldContentImgUrl);
            List<string> imgSrc = helper.GetImgAll(content);
            content = httpURL(imgSrc, content);
            return content;
        }

        public static string articleOldLinkReplace(string htmlContent)
        {
            string content = helper.LinkUrlReplace(htmlContent.ToLower(), "../article/uploadfiles/", oldContentFilesUrl);
            List<string> imgSrc = helper.GetImgAll(content);
            content = httpURL(imgSrc, content);
            return content;
        }

        public static string articleOldFileIconReplace(string htmlContent)
        {
            string content = helper.LinkUrlReplace(htmlContent.ToLower(), "/ewebeditor/sysimage/file/", oldContentIconUrl);
            List<string> imgSrc = helper.GetImgAll(content);
            content = httpURL(imgSrc, content);
            return content;
        }

        public static string httpURL(List<string> imgSrcList, string content)
        {
            foreach (string s in imgSrcList)
            {
                if (!helper.IsHttp(s))
                {
                    content = content.Replace(s, adminWebUrl + s);
                }
            }

            return content;
        }



    }
}