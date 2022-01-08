using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.model
{
    public class PaginatorParameter<T>
    {
        public List<T> list { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalPage { get; set; }

    }


}