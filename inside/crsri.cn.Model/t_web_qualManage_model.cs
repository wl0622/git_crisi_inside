using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crsri.cn.Model
{
    public class t_web_qualManage_model
    {
        public int QMID { get; set; }
        public string subjectID { get; set; }
        public string specialID { get; set; }
        public string accName { get; set; }
        public string accType { get; set; }
        public string accUrl { get; set; }
        public string editor { get; set; }
        public string editorDep { get; set; }
        public DateTime? updateTime { get; set; }
        public Int16 hits { get; set; }
        public bool? isDeleted { get; set; }
        public Int16 layoutID { get; set; }
    }
}
