using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crsri.cn.Model
{
    public class t_web_zhuanjia_model
    {
        public int zhuanjiaID { get; set; }
        public string name { get; set; }
        public string brief { get; set; }
        public string zhuanye { get; set; }
        public string zhicheng { get; set; }
        public string xuewei { get; set; }
        public string photoname { get; set; }
        public int id { get; set; }
        public bool? isDeleted { get; set; }
        public int? hits { get; set; }
        public bool? isPassed { get; set; }
        public string EditorInCharge { get; set; }
    }
}
