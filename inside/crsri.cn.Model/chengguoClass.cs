using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crsri.cn.Model
{
    public class chengguoClass
    {
        public int chengguoID { get; set; }
        public string huojiangname { get; set; }
        public string xiangmuname { get; set; }
        public string huojiangdengji { get; set; }
        public string leibie { get; set; }
        public string dept { get; set; }
        public string canyudept { get; set; }
        public string huojiangniandai { get; set; }
        public string huojiangrenyuan { get; set; }
        public string chengguojj { get; set; }
        public bool? isDeleted { get; set; }
        public bool? isPassed { get; set; }
        public string EditorInCharge { get; set; }

    }

    public class reqChengGuoClass
    {
        public string year { get; set; }

        public List<chengguoClass> list { get; set; }
    }
}