using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crsri.cn.Model
{
    public class t_web_special_model
    {
        public Int16? spID { get; set; }
        public string specialID { get; set; }
        public string specialName { get; set; }
        public Int16? child { get; set; }
        public string readMe { get; set; }
        public string linkUrl { get; set; }
        public bool? isElite { get; set; }
        public bool? isShowOnTop { get; set; }
        public int? orderID { get; set; }
        public int? layoutID { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool? english { get; set; }
        public bool? isShowHome { get; set; }
        public int? showSN { get; set; }
    }
}
