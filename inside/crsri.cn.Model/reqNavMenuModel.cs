using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crsri.cn.Model
{
    public class reqNavMenuTreeModel
    {
        public t_navmenu_model node { get; set; }
        public List<t_navmenu_model> childrenNode { get; set; }
    }

    public class reqNavMenuModel
    {
        public List<reqNavMenuTreeModel> treelist { get; set; }
        public bool isTree { get; set; }
    }
}
