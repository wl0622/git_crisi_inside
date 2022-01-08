using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crsri.cn.Model
{
    public class t_navmenu_model
    {
        public int id { get; set; }
        public string nodeId { get; set; }
        public string nodeName { get; set; }
        public string parentNodeId { get; set; }
        public string linkurl { get; set; }
        public int? sort { get; set; }
    }
}
