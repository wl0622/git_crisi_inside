using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace crsri.cn.Model
{
    public class reqDownload
    {
        public string subjectID { get; set; }
        public string subjectName { get; set; }
        public List<t_web_downService_model> downloadItems { get; set; }
    }
}
