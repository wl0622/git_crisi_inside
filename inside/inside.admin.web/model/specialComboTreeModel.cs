using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.model
{
    public class ComboTreeModel
    {
        public string id { get; set; }
        public string text { get; set; }
    }

    public class reqComboTreeModel
    {
        public string id { get; set; }
        public string text { get; set; }
        public List<ComboTreeModel> children { get; set; }
    }
}