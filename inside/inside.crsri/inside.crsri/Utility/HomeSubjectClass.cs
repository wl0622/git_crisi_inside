using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.crsri.Utility
{
    public class HomeArticleClass
    {
        public int articleID { get; set; }
        public string title { get; set; }
        public string keywords { get; set; }
        public string releaseTime { get; set; }
        public bool isTop { get; set; }

    }

    public class requestHomeArticleClass
    {
        public string subjectID { get; set; }
        public string subjectName { get; set; }
        public List<HomeArticleClass> articleList { get; set; }
    }


    public class reqSpecialClass : HomeArticleClass
    {
        public string specialID { get; set; }
        public string specialName { get; set; }
    }
}