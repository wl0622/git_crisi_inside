using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crsri.cn.Model
{
    public class t_web_downService_model
    {
        public int DSID { get; set; }
        public string subjectID { get; set; }
        public string specialID { get; set; }
        public string DSName { get; set; }
        public string keywords { get; set; }
        public string content { get; set; }
        public string author { get; set; }
        public string releaseDep { get; set; }
        public DateTime releaseTime { get; set; }
        public string editor { get; set;}
        public string editorDep { get; set; }
        public DateTime updateTime { get; set; }
        public string picUrL { get; set; }
        public Int16? hits { get; set; }
        public bool? isDeleted { get; set; }
        public bool? isHot { get; set; }
        public bool? isOnTop { get; set; }
        public bool? isElite{get;set;}
        public bool? isPassed { get; set; }
        public Int16? layoutID { get; set; }

    }
}
