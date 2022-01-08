using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.tableEntity
{
    public class errorLoginRecordModel
    {
        public int id { get; set; }
        public string ipAddress { get; set; }
        public string login { get; set; }
        public DateTime? firstTime { get; set; }
        public DateTime? lastTime { get; set; }
        public int errorCount { get; set; }
    }
}