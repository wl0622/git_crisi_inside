using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.model
{
    public class articlepaginatorModel
    {
        public string title { get; set; }
        public string keyword { get; set; }
        public string author { get; set; }
        public string editor { get; set; }
        public string subjectID { get; set; }
        public string isPassed { get; set; }
        public string articleID { get; set; }
        public string isPicxw { get; set; }
    }

    public class PaginatorRequestModel
    {
        public string data { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

    }

}