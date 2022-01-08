using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crsri.cn.Model
{
    public class t_web_article_model
    {
        public int articleID { get; set; }
        public string subjectID { get; set; }
        public string specialID { get; set; }
        public string title { get; set; }
        public string keywords { get; set; }
        public string content { get; set; }
        public string author { get; set; }
        public string releaseDep { get; set; }
        public DateTime? releaseTime { get; set; }
        public string editor { get; set; }
        public string editorDep { get; set; }
        public string EditorInCharge { get; set; }
        public DateTime? updateTime { get; set; }
        public bool? isIncludePic { get; set; }
        public Int16? defaultPicID { get; set; }
        public string defaultPicUrl { get; set; }
        public bool? isIncludeAcc { get; set; }
        public Int16? hits { get; set; }
        public bool? isDeleted { get; set; }
        public bool? isHot { get; set; }
        public bool? isOnTop { get; set; }
        public bool? isPicxw { get; set; }
        public bool? isElite { get; set; }
        public bool? isComment { get; set; }
        public bool? isPassed { get; set; }
        public Int16? layoutID { get; set; }
        public bool? isPost { get; set; }
        public bool? isChecked { get; set; }
        public bool? isexarticle { get; set; }
        public string titlejiancheng { get; set; }
        public string linkurl { get; set; }
        public string titletoutiao { get; set; }
        public string JHXYid { get; set; }
        public bool? isEnglishpic { get; set; }
        public bool? isEngpic { get; set; }
        public bool? isTop { get; set; }
        public bool? isspecialpic { get; set; }
        public bool? isspecialtop { get; set; }
        public bool? isspecialpicxw { get; set; }
        public int? picID { get; set; }
        public bool? isNewsTop { get; set; }
        public string reprint { get; set; }

    }
}
